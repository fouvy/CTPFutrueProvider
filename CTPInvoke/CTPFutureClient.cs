using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalmBeltFund.Trading.CTP;
using System.Reflection;

namespace CalmBeltFund.Trading.CTP
{
  public abstract class CTPFutureClient : CTPBaseClient<CTPRequestAction,CTPResponseType>
  {


    #region BaseEvents

    public event EventHandler<CTPEventArgs<CThostFtdcRspUserLoginField>> UserLoginResponse
    {
      add { AddHandler(CTPResponseType.UserLoginResponse, value); }
      remove { RemoveHandler(CTPResponseType.UserLoginResponse, value); }
    }

    public event EventHandler<CTPEventArgs> UserLogoutResponse
    {
      add { AddHandler(CTPResponseType.UserLogoutResponse, value); }
      remove { RemoveHandler(CTPResponseType.UserLogoutResponse, value); }
    }


    public event EventHandler<CTPEventArgs> FrontConnectedResponse
    {
      add { AddHandler(CTPResponseType.FrontConnectedResponse, value); }
      remove { RemoveHandler(CTPResponseType.FrontConnectedResponse, value); }
    }

    public event EventHandler<CTPEventArgs> FrontDisconnectedResponse
    {
      add { AddHandler(CTPResponseType.FrontDisconnectedResponse, value); }
      remove { RemoveHandler(CTPResponseType.FrontDisconnectedResponse, value); }
    }

    public event EventHandler<CTPEventArgs> HeartBeatWarningResponse
    {
      add { AddHandler(CTPResponseType.HeartBeatWarningResponse, value); }
      remove { RemoveHandler(CTPResponseType.HeartBeatWarningResponse, value); }
    }

    public event EventHandler<CTPEventArgs> ErrorResponse
    {
      add { AddHandler(CTPResponseType.ErrorResponse, value); }
      remove { RemoveHandler(CTPResponseType.ErrorResponse, value); }
    }

    #endregion

    #region CTP API Invoke


    protected override unsafe int ProcessRequest(void* hTrader, int type, void* pReqData, int requestID)
    {
      return CTPWrapper.ProcessRequest(hTrader, type, pReqData, requestID);
    }

    protected override int Process(IntPtr handle, int type, int p1, StringBuilder p2)
    {
      return CTPWrapper.Process(handle, type, p1, p2);
    }

    #endregion

    protected override CTPResponseInfo GetResponseInfo(IntPtr pRspInfo)
    {

      CTPResponseInfo rsp = new CTPResponseInfo();

      CThostFtdcRspInfoField rspInfo = PInvokeUtility.GetObjectFromIntPtr<CThostFtdcRspInfoField>(pRspInfo);

      rsp.ErrorID = rspInfo.ErrorID;
      rsp.Message = PInvokeUtility.GetUnicodeString(rspInfo.ErrorMsg);

      return rsp;
    }

    protected override CTPResponseType ConvertToResponseType(int rsp)
    {
      return (CTPResponseType)rsp;
    }

    protected override int ConvertActionToInt(CTPRequestAction action)
    {
      return (int)action;
    }
  }
}
