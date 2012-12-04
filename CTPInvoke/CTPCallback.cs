using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CalmBeltFund.Trading.CTP
{
  /// <summary>
  /// CTP回调函数指针
  /// </summary>
  /// <param name="type"></param>
  /// <param name="pData"></param>
  /// <param name="pRspInfo"></param>
  /// <param name="requestID"></param>
  /// <param name="isLast"></param>
  public delegate void CTPResponseCallback(int type, IntPtr pData, IntPtr pRspInfo, int requestID, [MarshalAs(UnmanagedType.U1)]bool isLast);


  internal delegate void OutputCallback(string msg);


  public class CTPResponseInfo
  {
    public int ErrorID { get; set; }
    public string Message { get; set; }

    //internal CTPResponseInfo(IntPtr pRspInfo)
    //{

    //  CThostFtdcRspInfoField rspInfo = PInvokeUtility.GetObjectFromIntPtr<CThostFtdcRspInfoField>(pRspInfo);

    //  this.ErrorID = rspInfo.ErrorID;
    //  this.Message = PInvokeUtility.GetUnicodeString(rspInfo.ErrorMsg);

    //}

    internal CTPResponseInfo()
    {
      this.ErrorID = 0;
      this.Message = "";
    }

    public static CTPResponseInfo Empty
    {
      get
      {
        CTPResponseInfo info = new CTPResponseInfo();
        return info;
      }
    }
  }

  public class CTPEventArgs : EventArgs
  {
    public CTPResponseInfo ResponseInfo { get; internal set; }
    public int RequestID { get; internal set; }

    public CTPEventArgs(CTPResponseInfo rspInfo, int requestID)
    {
      this.ResponseInfo = rspInfo;
      this.RequestID = requestID;
    }

    public CTPEventArgs(CTPResponseInfo rspInfo)
      : this(rspInfo, 0)
    { }

    public CTPEventArgs()
      : this(CTPResponseInfo.Empty, 0)
    { }
  }

  public class CTPEventArgs<T> : CTPEventArgs
  {
    T value;

    public object RequestData { get; internal set; }

    public T Value
    {
      get { return this.value; }
      set { this.value = value; }
    }

    public CTPEventArgs(T value)
      : base()
    {
      this.value = value;
    }

    internal CTPEventArgs(T value, CTPResponseInfo rspInfo)
      : this(value, rspInfo, 0)
    {

    }

    internal CTPEventArgs(T value, CTPResponseInfo rspInfo, int requestID)
      : base(rspInfo, requestID)
    {
      this.value = value;
    }
  }

  /// <summary>
  /// 返回的数据类型
  /// </summary>
  public class CTPResponseDataTypeAttribute : Attribute
  {
    public Type Type { get; set; }

    public CTPResponseDataTypeAttribute(Type value)
    {
      this.Type = value;
    }
  }


  /// <summary>
  /// 队列任务
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class CTPTaskBase<T>
  {
    /// <summary>
    /// 请求动作
    /// </summary>
    public T Action{get;set;}

    /// <summary>
    /// 请求参数
    /// </summary>
    public object Parameter{get;set;}

    /// <summary>
    /// 请求序号
    /// </summary>
    public int RequestID{get;set;}

    /// <summary>
    /// 存在异步回调时，用于缓存调用列表
    /// </summary>
    public Action<CTPTaskBase<T>,CTPEventArgs> Callback { get; set; }
  }
}
