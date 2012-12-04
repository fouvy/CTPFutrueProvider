using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace CalmBeltFund.Trading.CTP
{
  public class CTPMarketData : CTPFutureClient
  {

    #region Event

    public event EventHandler<CTPEventArgs<CThostFtdcDepthMarketDataField>> DepthMarketDataResponse
    {
      add { AddHandler(CTPResponseType.DepthMarketDataResponse, value); }
      remove { RemoveHandler(CTPResponseType.DepthMarketDataResponse, value); }
    }

    #endregion


    public void Connect(string[] frontAddress, string brokerID, string userID, string password)
    {
      this.BrokerID = brokerID;
      this.InvestorID = userID;
      this.Password = password;

      connTempFile = Path.GetTempFileName();

      //创建
      _instance = (IntPtr)Process(Marshal.GetFunctionPointerForDelegate(this.callback), (int)CTPRequestAction.MarketDataCreate, 0, new StringBuilder(connTempFile));

      foreach (string front in frontAddress)
      {
        string address = front;

        if (address.StartsWith("tcp://", StringComparison.OrdinalIgnoreCase) == false)
        {
          address = "tcp://" + address;
        }

        Process(this._instance, (int)CTPRequestAction.MarketDataRegisterFront, 0, new StringBuilder(address));

        this.FrontAddress = address;
      }

      Process(this._instance, (int)CTPRequestAction.MarketDataInit, 0, null);
    }

    void UserLogin()
    {
      CThostFtdcReqUserLoginField userLogin = new CThostFtdcReqUserLoginField();

      userLogin.BrokerID = this.BrokerID;
      userLogin.UserID = this.InvestorID;
      userLogin.Password = this.Password;

      int result = InvokeAPI(CTPRequestAction.MarketDataUserLoginAction, userLogin);

    }


    /// <summary>
    /// 订阅行情
    /// </summary>
    /// <param name="symbols"></param>
    public void SubscribeMarketData(string[] symbols)
    {

      IntPtr[] handlers = new IntPtr[symbols.Length];

      for (int i = 0; i < symbols.Length; i++)
      {
        handlers[i] = Marshal.StringToHGlobalAnsi(symbols[i]);
      }

      CTPWrapper.SubscribeMarketData(this._instance, handlers, symbols.Length);

      //StringBuilder buffer = new StringBuilder();

      //foreach (var item in symbols)
      //{
      //  buffer.Append(item).Append('\0');
      //}

      //CTPWrapper.Process(this._instance, (int)CTPRequestAction.MarketDataSubscribeMarketData, symbols.Length, buffer);

    }

    /// <summary>
    /// 退订行情
    /// </summary>
    /// <param name="symbols"></param>
    public void UnSubscribeMarketData(string[] symbols)
    {

      if (symbols == null || symbols.Length == 0)
      {
        return;
      }

      IntPtr[] handlers = new IntPtr[symbols.Length];

      for (int i = 0; i < symbols.Length; i++)
      {
        handlers[i] = Marshal.StringToHGlobalAnsi(symbols[i]);
      }

      CTPWrapper.UnSubscribeMarketData(this._instance, handlers, symbols.Length);
    }


    protected override void ProcessBusinessResponse(CTPResponseType responseType, IntPtr pData, CTPResponseInfo rspInfo, int requestID)
    {
      switch (responseType)
      {
        #region 当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
        case CTPResponseType.FrontConnectedResponse:
          {
            this.isConnect = true;

            this.UserLogin();

            //调用事件
            OnEventHandler(CTPResponseType.FrontConnectedResponse, new CTPEventArgs());

            break;
          }
        #endregion

        #region 用户登录
        case CTPResponseType.UserLoginResponse:
          {
            CTPEventArgs<CThostFtdcRspUserLoginField> args = CreateEventArgs<CThostFtdcRspUserLoginField>(requestID, rspInfo);

            this.isLogin = true;

            this.OnEventHandler(CTPResponseType.UserLoginResponse, args);
          }
          break;
        #endregion


        case CTPResponseType.DepthMarketDataResponse:
          {
            if (this == null || this.isDispose == true)
            {
              return;
            }

            CTPEventArgs<CThostFtdcDepthMarketDataField> args = CreateEventArgs<CThostFtdcDepthMarketDataField>(pData, rspInfo);

            OnEventHandler(CTPResponseType.DepthMarketDataResponse, args);

            break;
          }
      }
    }

    #region IDisposable 成员

    public override void Dispose()
    {
      isDispose = true;

      if (this._instance != IntPtr.Zero)
      {
        Process(this._instance, (int)CTPRequestAction.MarketDataRelease, 0, null);

        this._instance = IntPtr.Zero;
      }

      base.Dispose();
    }

    #endregion


    #region Response

    /// <summary>
    /// 订阅行情应答
    /// </summary>
    internal void OnSubMarketData(IntPtr pSpecificInstrument, IntPtr pRspInfo, int nRequestID, bool bIsLast)
    {
      CThostFtdcSpecificInstrumentField instrument = PInvokeUtility.GetObjectFromIntPtr<CThostFtdcSpecificInstrumentField>(pSpecificInstrument);
    }

    /// <summary>
    /// 取消订阅行情应答
    /// </summary>
    internal void OnUnSubMarketData(IntPtr pSpecificInstrument, IntPtr pRspInfo, int nRequestID, bool bIsLast)
    {

    }

    #endregion





  }
}

