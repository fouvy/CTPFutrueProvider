#pragma once

#include "StdAfx.h"



struct CTPMarketDataSpiCallbackStruct
{

///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。   
OnFrontConnectedCallback m_OnFrontConnectedCallback;

///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。   
///@param nReason 错误原因   
///  0x1001 网络读失败   
///  0x1002 网络写失败   
///  0x2001 接收心跳超时   
///  0x2002 发送心跳失败   
///  0x2003 收到错误报文   
OnFrontDisconnectedCallback m_OnFrontDisconnectedCallback;

///心跳超时警告。当长时间未收到报文时，该方法被调用。   
///@param nTimeLapse 距离上次接收报文的时间   
OnHeartBeatWarningCallback m_OnHeartBeatWarningCallback;


///登录请求响应   
OnRspUserLoginCallback m_OnRspUserLoginCallback;

///登出请求响应   
OnRspUserLogoutCallback m_OnRspUserLogoutCallback;

///错误应答   
OnRspErrorCallback m_OnRspErrorCallback;

///订阅行情应答   
OnRspSubMarketDataCallback m_OnRspSubMarketDataCallback;

///取消订阅行情应答   
OnRspUnSubMarketDataCallback m_OnRspUnSubMarketDataCallback;

///深度行情通知   
OnRtnDepthMarketDataCallback m_OnRtnDepthMarketDataCallback;


};



class CTPMarketDataSpi : public CThostFtdcMdSpi
{
public:

	CTPMarketDataSpi(CTPResponseCallback callback)
  {
    this->callback = callback;
  }

	///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
	virtual void OnFrontConnected()
  {
    this->OnResponse(OnFrontConnectedResponse);
  }
	
	///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
	///@param nReason 错误原因
	///        0x1001 网络读失败
	///        0x1002 网络写失败
	///        0x2001 接收心跳超时
	///        0x2002 发送心跳失败
	///        0x2003 收到错误报文
	virtual void OnFrontDisconnected(int nReason)
  {
    this->OnResponse(OnFrontDisconnectedResponse,(void*)nReason);
  }
		
	///心跳超时警告。当长时间未收到报文时，该方法被调用。
	///@param nTimeLapse 距离上次接收报文的时间
	virtual void OnHeartBeatWarning(int nTimeLapse)
  {
    this->OnResponse(OnHeartBeatWarningResponse,(void*)nTimeLapse);
  }
	

	///登录请求响应
	virtual void OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspUserLoginResponse,pRspUserLogin,pRspInfo,nRequestID,bIsLast);
  }

	///登出请求响应
	virtual void OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspUserLogoutResponse,pUserLogout,pRspInfo,nRequestID,bIsLast);
  }

	///错误应答
	virtual void OnRspError(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspErrorResponse,pRspInfo,nRequestID,bIsLast);
  }

	///订阅行情应答
	virtual void OnRspSubMarketData(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspSubMarketDataResponse,pSpecificInstrument,pRspInfo,nRequestID,bIsLast);
  }

	///取消订阅行情应答
	virtual void OnRspUnSubMarketData(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspSubMarketDataResponse,pSpecificInstrument,pRspInfo,nRequestID,bIsLast);
  }

	///深度行情通知
	virtual void OnRtnDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData)
  {
    this->OnResponse(OnRtnDepthMarketDataResponse,pDepthMarketData);
  }

	
private:
	

	/*    回调函数指针    */
  CTPResponseCallback callback;

  void OnResponse(CTP_RESPONSE_TYPE type, void *pRspData, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    if(this->callback)
    {
      this->callback(type,pRspData,pRspInfo,nRequestID,bIsLast);
    }
  }

  void OnResponse(CTP_RESPONSE_TYPE type, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    OnResponse(type,NULL,pRspInfo,nRequestID,bIsLast);
  }

  void OnResponse(CTP_RESPONSE_TYPE type, void *pRspData, CThostFtdcRspInfoField *pRspInfo)
  {
    OnResponse(type,pRspData,pRspInfo,0,TRUE);
  }

  void OnResponse(CTP_RESPONSE_TYPE type, void *pRspData)
  {
    OnResponse(type,pRspData,NULL,0,TRUE);
  }

  void OnResponse(CTP_RESPONSE_TYPE type)
  {
    OnResponse(type,NULL,NULL,0,TRUE);
  }
};


