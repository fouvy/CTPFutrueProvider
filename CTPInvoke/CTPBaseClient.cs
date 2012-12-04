using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.ComponentModel;

namespace CalmBeltFund.Trading.CTP
{
  /// <summary>
  /// CTP终端基类
  /// </summary>
  /// <typeparam name="TRequestAction"></typeparam>
  /// <typeparam name="TResponseType"></typeparam>
  public abstract partial class CTPBaseClient<TRequestAction, TResponseType>: IDisposable
  {
    protected int requestID = 0;

    protected EventHandlerList events = new EventHandlerList();
    protected Dictionary<int, object> requestDataList = new Dictionary<int, object>();

    //临时文件
    protected string connTempFile = null;

    /// <summary>
    /// 查询类请求时间间隔：1秒钟
    /// </summary>
    public const int QueryTaskProcessPeriod = 1000;
    protected Timer queryTaskTimer = null;

    protected Queue<CTPTaskBase<TRequestAction>> queryTasks = new Queue<CTPTaskBase<TRequestAction>>();
    protected Dictionary<int, CTPTaskBase<TRequestAction>> processedTasks = new Dictionary<int, CTPTaskBase<TRequestAction>>();

    protected IntPtr _instance = IntPtr.Zero;

    internal OutputCallback outputCallback = null;
    protected CTPResponseCallback callback = null;

    protected Dictionary<int, List<Object>> responseDataMap = new Dictionary<int, List<object>>();
    protected Dictionary<TResponseType, Type> responseDataTypeMapping = new Dictionary<TResponseType, Type>();

    protected bool isConnect = false;
    protected bool isLogin = false;
    protected bool isDispose = false;

    /// <summary>
    /// 经纪公司代码
    /// </summary>
    public string BrokerID { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string InvestorID { get; set; }
    /// <summary>
    /// 服务器地址
    /// </summary>
    public string FrontAddress { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// 前置机ID
    /// </summary>
    public int FrontID { get; set; }
    /// <summary>
    /// SessionID
    /// </summary>
    public int SessionID { get; set; }

    /// <summary>
    /// 是否已连接
    /// </summary>
    public bool IsConnect
    {
      get { return isConnect; }
    }

    /// <summary>
    /// 是否已登录
    /// </summary>
    public bool IsLogin
    {
      get { return isLogin; }
    }


    public CTPBaseClient()
    {

      this.callback = new CTPResponseCallback(CTPResponseHandler);

      InitEvents();
      InitResponseDataTypeMapping();

      queryTaskTimer = new Timer(new TimerCallback(this.ProcessQueryTask));
    }


    /// <summary>
    /// 追加查询任务
    /// </summary>
    /// <param name="para"></param>
    /// <param name="action"></param>
    protected void AddQueryTask(object para, TRequestAction action, Action<CTPTaskBase<TRequestAction>,CTPEventArgs> callback = null)
    {
      CTPTaskBase<TRequestAction> task = new CTPTaskBase<TRequestAction>();
      task.Action = action;
      task.Parameter = para;
      task.Callback = callback;

      lock (this.queryTasks)
      {
        this.queryTasks.Enqueue(task);
      }

      //立刻启动处理
      this.queryTaskTimer.Change(0, QueryTaskProcessPeriod);
    }


    /// <summary>
    /// 处理查询任务
    /// （20090918API中，对查询函数增加了时间限制，每秒钟只能查询一次）
    /// </summary>
    protected void ProcessQueryTask(object obj)
    {
      lock (this.queryTasks)
      {

        if (this.isDispose == true)
        {
          this.queryTasks.Clear();
        }
        else if (this.queryTasks.Count > 0)
        {

          //取得队列首位的任务
          CTPTaskBase<TRequestAction> task = this.queryTasks.Peek();
          task.RequestID = this.CreateRequestID();

          int result = InvokeAPI(task);


          //请求发送成功，则移除队列
          if (result == 0)
          {
            this.queryTasks.Dequeue();

            if (processedTasks.ContainsKey(task.RequestID) == false)
            {
              processedTasks.Add(task.RequestID, task);
            }

          }
        }

        //没有要处理的任务，则关闭计时器
        if (this.queryTasks.Count == 0)
        {
          this.queryTaskTimer.Change(Timeout.Infinite, Timeout.Infinite);
          return;
        }
      }
    }

    /// <summary>
    /// 初始化事件列表
    /// </summary>
    private void InitEvents()
    {
      Type type = this.GetType();

      EventInfo[] events = type.GetEvents();

      //foreach (EventInfo item in events)
      //{
      //  if (this.events.ContainsKey(item.Name) == false)
      //  {
      //    this.events.Add(item.Name, null);
      //  }
      //}
    }

    /// <summary>
    /// 初始化接受数据类型
    /// </summary>
    private void InitResponseDataTypeMapping()
    {
      //列举响应类型中的所有枚举值
      Type rspType = typeof(TResponseType);

      foreach (FieldInfo field in rspType.GetFields(BindingFlags.Static | BindingFlags.Public))
      {

        string name = field.Name;

        //查找枚举值对应的响应类型
        TResponseType item = (TResponseType)field.GetValue(rspType);
        object[] attrs = field.GetCustomAttributes(typeof(CTPResponseDataTypeAttribute), false);

        if (attrs != null && attrs.Length > 0)
        {
          CTPResponseDataTypeAttribute attr = attrs[0] as CTPResponseDataTypeAttribute;

          responseDataTypeMapping.Add((TResponseType)item, attr.Type);
        }
      }
    }


    /// <summary>
    /// 移除代理
    /// </summary>
    /// <param name="value"></param>
    public void RemoveHandler(TResponseType rspType, Delegate value)
    {
      if (value != null)
      {
        string key = rspType.ToString();
        this.events.RemoveHandler(key, value);
      }
    }

    /// <summary>
    /// 增加代理
    /// </summary>
    /// <param name="value"></param>
    public void AddHandler(TResponseType rspType, Delegate value)
    {
      if (value != null)
      {
        string key = rspType.ToString();
        events.AddHandler(key, value);
      }
    }

    /// <summary>
    /// 引发事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rspType"></param>
    /// <param name="e"></param>
    protected void OnEventHandler<T>(TResponseType rspType, T e) where T : EventArgs
    {
      string eventName = rspType.ToString();
      Delegate handler = this.events[eventName];

      if (handler == null)
      {
        return;
      }
      else
      {

        Delegate[] delArray = handler.GetInvocationList();

        //异步调用响应列表
        foreach (Delegate del in delArray)
        {
          EventHandler<T> method = (EventHandler<T>)del;
          method.BeginInvoke(this, e, null, null);
        }
      }
    }

    protected int CreateRequestID()
    {
      return System.Threading.Interlocked.Increment(ref this.requestID);
    }

    protected object GetRequestData(int requestID)
    {
      if (this.requestDataList.ContainsKey(requestID))
      {
        return this.requestDataList[requestID];
      }
      else
      {
        return null;
      }
    }

    protected abstract int ConvertActionToInt(TRequestAction action);

    protected abstract TResponseType ConvertToResponseType(int rsp);

    #region CTP响应处理

    /// <summary>
    /// 回调函数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pData"></param>
    /// <param name="pRspInfo"></param>
    /// <param name="requestID"></param>
    /// <param name="isLast"></param>
    protected void CTPResponseHandler(int type, IntPtr pData, IntPtr pRspInfo, int requestID, bool isLast)
    {
      TResponseType responseType = ConvertToResponseType(type);

      ReceiveResponseData(responseType, pData, requestID);

      if (isLast == true)
      {
        CTPResponseInfo rspInfo = GetResponseInfo(pRspInfo);

        this.ProcessBusinessResponse(responseType, pData, rspInfo, requestID);
      }
    }

    /// <summary>
    /// 返回响应消息，具体由继承类实现
    /// </summary>
    /// <param name="pRspInfo"></param>
    /// <returns></returns>
    protected abstract CTPResponseInfo GetResponseInfo(IntPtr pRspInfo);


    /// <summary>
    /// 处理业务响应
    /// </summary>
    /// <param name="responseType"></param>
    /// <param name="pData"></param>
    /// <param name="rspInfo"></param>
    /// <param name="requestID"></param>
    protected abstract void ProcessBusinessResponse(TResponseType responseType, IntPtr pData, CTPResponseInfo rspInfo, int requestID);


    /// <summary>
    /// 接收响应数据
    /// </summary>
    /// <param name="responseType"></param>
    /// <param name="pData"></param>
    /// <param name="requestID"></param>
    protected void ReceiveResponseData(TResponseType responseType, IntPtr pData, int requestID)
    {
      //建立响应数据缓存
      if (this.responseDataMap.ContainsKey(requestID) == false)
      {
        this.responseDataMap[requestID] = new List<Object>();
      }

      //保存响应数据
      if (pData != IntPtr.Zero)
      {
        Object value = null;

        if (responseDataTypeMapping.ContainsKey(responseType))
        {
          Type t = responseDataTypeMapping[responseType];

          if (t != null)
          {
            value = PInvokeUtility.GetObjectFromIntPtr(t, pData);
          }
        }

        if (value != null)
        {
          this.responseDataMap[requestID].Add(value);
        }
      }
    }


    /// <summary>
    /// 创建事件参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="event"></param>
    /// <param name="requestID"></param>
    /// <param name="rspInfo"></param>
    protected CTPEventArgs<T> CreateEventArgs<T>(int requestID, CTPResponseInfo rspInfo) where T : struct
    {
      //if (requestID == 0)
      //{
      //  throw new CBFNativeException("Invalid Operation in PreProcessResponse");
      //}


      //转换响应数据
      List<Object> rspDataList = this.responseDataMap[requestID];

      T value = default(T);
      if (rspDataList.Count > 0)
      {
        value = (T)(rspDataList[0]);
      }

      //创建事件参数
      CTPEventArgs<T> args = new CTPEventArgs<T>(value, rspInfo);
      args.RequestData = this.GetRequestData(requestID);

      return args;
    }

    /// <summary>
    /// 创建事件参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="requestID"></param>
    /// <param name="rspInfo"></param>
    /// <returns></returns>
    protected CTPEventArgs<List<T>> CreateListEventArgs<T>(int requestID, CTPResponseInfo rspInfo) where T : struct
    {
      if (requestID == 0)
      {
        throw new Exception("Invalid Operation in PreProcessResponse");
      }

      List<T> list = new List<T>();

      //转换响应数据
      foreach (Object p in this.responseDataMap[requestID])
      {
        list.Add((T)p);
      }

      //创建事件参数
      CTPEventArgs<List<T>> args = new CTPEventArgs<List<T>>(list, rspInfo);
      args.RequestData = this.GetRequestData(requestID);

      return args;
    }

    /// <summary>
    /// 创建事件参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rspData"></param>
    /// <param name="rspInfo"></param>
    /// <returns></returns>
    protected CTPEventArgs<T> CreateEventArgs<T>(IntPtr rspData, CTPResponseInfo rspInfo)
    {

      //转换响应数据
      T value = default(T);
      if (rspData != IntPtr.Zero)
      {
        value = PInvokeUtility.GetObjectFromIntPtr<T>(rspData);
      }

      //创建事件参数
      CTPEventArgs<T> args = new CTPEventArgs<T>(value, rspInfo);

      return args;
    }

    #endregion

    #region CTP API Invoke

    protected abstract unsafe int ProcessRequest(void* hTrader, int type, void* pReqData, int requestID);

    protected abstract int Process(IntPtr handle, int type, int p1, StringBuilder p2);


    /// <summary>
    /// 调用CTP接口
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    protected int InvokeAPI(CTPTaskBase<TRequestAction> task)
    {
      int result = -1;

      try
      {
        result = InvokeAPI(this._instance, ConvertActionToInt(task.Action), task.Parameter, task.RequestID);
      }
      catch (Exception ex)
      {
        throw ex;
      }

      return result;
    }

    /// <summary>
    /// 调用CTP接口
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    protected int InvokeAPI(TRequestAction action, object data)
    {
      return InvokeAPI(this._instance, ConvertActionToInt(action), data, CreateRequestID());
    }

    protected int InvokeAPI(IntPtr handler, int action, object reqData, int requestID)
    {

      if (requestID > 0)
      {
        if (this.requestDataList.ContainsKey(requestID) == false)
        {
          this.requestDataList.Add(requestID, reqData);
        }
        else
        {
          Exception e = new Exception("Duplicate request");

          e.Data.Add("RequestID", requestID);
          e.Data.Add("First Data", this.requestDataList[requestID]);
          e.Data.Add("Second Data", reqData);

          throw e;
        }
      }

      unsafe
      {

        IntPtr buffer = IntPtr.Zero;
        int result = 0;
        int rawSize = 0;
        byte[] rawData = null;


        GCHandle handle;


        if (reqData is string)
        {
          rawData = ASCIIEncoding.ASCII.GetBytes((string)reqData);

          handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
          buffer = handle.AddrOfPinnedObject();


          buffer = Marshal.StringToHGlobalAnsi((string)reqData);
        }
        else
        {
          rawSize = Marshal.SizeOf(reqData);
          rawData = new byte[rawSize];

          handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
          buffer = handle.AddrOfPinnedObject();
          Marshal.StructureToPtr(reqData, buffer, false);
        }

        result = ProcessRequest(handler.ToPointer(), action, buffer.ToPointer(), requestID);

        handle.Free();

        return result;
      }
    }

    #endregion

    public virtual void Dispose()
    {
      this.responseDataMap.Clear();
      this._instance = IntPtr.Zero;

      this.events.Dispose();

      //删除临时文件
      if (string.IsNullOrEmpty(connTempFile) == false)
      {
        DeleteTempFile(connTempFile);
        DeleteTempFile(connTempFile + "DialogRsp.con");
        DeleteTempFile(connTempFile + "Private.con");
        DeleteTempFile(connTempFile + "Public.con");
        DeleteTempFile(connTempFile + "QueryRsp.con");
        DeleteTempFile(connTempFile + "TradingDay.con");
      }
    }

    protected void DeleteTempFile(string filePath)
    {
      try
      {
        if (File.Exists(filePath))
        {
          File.Delete(filePath);
        }
      }
      catch (Exception)
      {

      }

    }
  }
}
