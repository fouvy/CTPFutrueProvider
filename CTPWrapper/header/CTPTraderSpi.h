#pragma once
#include "StdAfx.h"

class CTPTraderSpi : public CThostFtdcTraderSpi
{
public:


  CTPTraderSpi(CTPResponseCallback callback)
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
  
  ///客户端认证响应
	virtual void OnRspAuthenticate(CThostFtdcRspAuthenticateField *pRspAuthenticateField, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspAuthenticateResponse ,pRspAuthenticateField,pRspInfo,nRequestID,bIsLast);
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

  ///用户口令更新请求响应
  virtual void OnRspUserPasswordUpdate(CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspUserPasswordUpdateResponse,pUserPasswordUpdate,pRspInfo,nRequestID,bIsLast);
  }

  ///资金账户口令更新请求响应			
  void OnRspTradingAccountPasswordUpdate(CThostFtdcTradingAccountPasswordUpdateField *pTradingAccountPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspTradingAccountPasswordUpdateResponse, pTradingAccountPasswordUpdate, pRspInfo, nRequestID, bIsLast);
  }


  ///报单录入请求响应			
  void OnRspOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspOrderInsertResponse, pInputOrder, pRspInfo, nRequestID, bIsLast);
  }


  ///预埋单录入请求响应			
  void OnRspParkedOrderInsert(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspParkedOrderInsertResponse, pParkedOrder, pRspInfo, nRequestID, bIsLast);
  }


  ///预埋撤单录入请求响应			
  void OnRspParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspParkedOrderActionResponse, pParkedOrderAction, pRspInfo, nRequestID, bIsLast);
  }


  ///报单操作请求响应			
  void OnRspOrderAction(CThostFtdcInputOrderActionField *pInputOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspOrderActionResponse, pInputOrderAction, pRspInfo, nRequestID, bIsLast);
  }


  ///查询最大报单数量响应			
  void OnRspQueryMaxOrderVolume(CThostFtdcQueryMaxOrderVolumeField *pQueryMaxOrderVolume, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQueryMaxOrderVolumeResponse, pQueryMaxOrderVolume, pRspInfo, nRequestID, bIsLast);
  }


  ///投资者结算结果确认响应			
  void OnRspSettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspSettlementInfoConfirmResponse, pSettlementInfoConfirm, pRspInfo, nRequestID, bIsLast);
  }


  ///删除预埋单响应			
  void OnRspRemoveParkedOrder(CThostFtdcRemoveParkedOrderField *pRemoveParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspRemoveParkedOrderResponse, pRemoveParkedOrder, pRspInfo, nRequestID, bIsLast);
  }


  ///删除预埋撤单响应			
  void OnRspRemoveParkedOrderAction(CThostFtdcRemoveParkedOrderActionField *pRemoveParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspRemoveParkedOrderActionResponse, pRemoveParkedOrderAction, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询报单响应			
  void OnRspQryOrder(CThostFtdcOrderField *pOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryOrderResponse, pOrder, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询成交响应			
  void OnRspQryTrade(CThostFtdcTradeField *pTrade, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryTradeResponse, pTrade, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询投资者持仓响应			
  void OnRspQryInvestorPosition(CThostFtdcInvestorPositionField *pInvestorPosition, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryInvestorPositionResponse, pInvestorPosition, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询资金账户响应			
  void OnRspQryTradingAccount(CThostFtdcTradingAccountField *pTradingAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryTradingAccountResponse, pTradingAccount, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询投资者响应			
  void OnRspQryInvestor(CThostFtdcInvestorField *pInvestor, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryInvestorResponse, pInvestor, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询交易编码响应			
  void OnRspQryTradingCode(CThostFtdcTradingCodeField *pTradingCode, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryTradingCodeResponse, pTradingCode, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询合约保证金率响应			
  void OnRspQryInstrumentMarginRate(CThostFtdcInstrumentMarginRateField *pInstrumentMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryInstrumentMarginRateResponse, pInstrumentMarginRate, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询合约手续费率响应			
  void OnRspQryInstrumentCommissionRate(CThostFtdcInstrumentCommissionRateField *pInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryInstrumentCommissionRateResponse, pInstrumentCommissionRate, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询交易所响应			
  void OnRspQryExchange(CThostFtdcExchangeField *pExchange, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryExchangeResponse, pExchange, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询合约响应			
  void OnRspQryInstrument(CThostFtdcInstrumentField *pInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryInstrumentResponse, pInstrument, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询行情响应			
  void OnRspQryDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryDepthMarketDataResponse, pDepthMarketData, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询投资者结算结果响应			
  void OnRspQrySettlementInfo(CThostFtdcSettlementInfoField *pSettlementInfo, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQrySettlementInfoResponse, pSettlementInfo, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询转帐银行响应			
  void OnRspQryTransferBank(CThostFtdcTransferBankField *pTransferBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryTransferBankResponse, pTransferBank, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询投资者持仓明细响应			
  void OnRspQryInvestorPositionDetail(CThostFtdcInvestorPositionDetailField *pInvestorPositionDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryInvestorPositionDetailResponse, pInvestorPositionDetail, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询客户通知响应			
  void OnRspQryNotice(CThostFtdcNoticeField *pNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryNoticeResponse, pNotice, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询结算信息确认响应			
  void OnRspQrySettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQrySettlementInfoConfirmResponse, pSettlementInfoConfirm, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询投资者持仓明细响应			
  void OnRspQryInvestorPositionCombineDetail(CThostFtdcInvestorPositionCombineDetailField *pInvestorPositionCombineDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryInvestorPositionCombineDetailResponse, pInvestorPositionCombineDetail, pRspInfo, nRequestID, bIsLast);
  }


  ///错误应答			
  void OnRspError(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspErrorResponse, pRspInfo, nRequestID, bIsLast);
  }


  ///报单通知			
  void OnRtnOrder(CThostFtdcOrderField *pOrder) 			
  {
    this->OnResponse(OnRtnOrderResponse, pOrder);
  }


  ///成交通知			
  void OnRtnTrade(CThostFtdcTradeField *pTrade) 			
  {
    this->OnResponse(OnRtnTradeResponse, pTrade);
  }


  ///报单录入错误回报			
  void OnErrRtnOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo) 			
  {
    this->OnResponse(OnErrRtnOrderInsertResponse, pInputOrder, pRspInfo);
  }


  ///报单操作错误回报			
  void OnErrRtnOrderAction(CThostFtdcOrderActionField *pOrderAction, CThostFtdcRspInfoField *pRspInfo) 			
  {
    this->OnResponse(OnErrRtnOrderActionResponse, pOrderAction, pRspInfo);
  }


  ///合约交易状态通知			
  void OnRtnInstrumentStatus(CThostFtdcInstrumentStatusField *pInstrumentStatus) 			
  {
    this->OnResponse(OnRtnInstrumentStatusResponse, pInstrumentStatus);
  }


  ///交易通知			
  void OnRtnTradingNotice(CThostFtdcTradingNoticeInfoField *pTradingNoticeInfo) 			
  {
    this->OnResponse(OnRtnTradingNoticeResponse, pTradingNoticeInfo);
  }


  ///请求查询签约银行响应			
  void OnRspQryContractBank(CThostFtdcContractBankField *pContractBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryContractBankResponse, pContractBank, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询预埋单响应			
  void OnRspQryParkedOrder(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryParkedOrderResponse, pParkedOrder, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询预埋撤单响应			
  void OnRspQryParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryParkedOrderActionResponse, pParkedOrderAction, pRspInfo, nRequestID, bIsLast);
  }


  ///请求查询交易通知响应			
  void OnRspQryTradingNotice(CThostFtdcTradingNoticeField *pTradingNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 			
  {
    this->OnResponse(OnRspQryTradingNoticeResponse, pTradingNotice, pRspInfo, nRequestID, bIsLast);
  }


  void OnRspQryCFMMCTradingAccountKey(CThostFtdcCFMMCTradingAccountKeyField *pCFMMCTradingAccountKey, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspQryCFMMCTradingAccountKeyResponse, pCFMMCTradingAccountKey, pRspInfo, nRequestID, bIsLast);
  }

  ///请求查询仓单折抵信息响应
  ///
	void OnRspQryEWarrantOffset(CThostFtdcEWarrantOffsetField *pEWarrantOffset, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspQryEWarrantOffsetResponse, pEWarrantOffset, pRspInfo, nRequestID, bIsLast);
  }

  ///请求查询银期签约关系响应
	void OnRspQryAccountregister(CThostFtdcAccountregisterField *pAccountregister, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspQryAccountregisterResponse, pAccountregister, pRspInfo, nRequestID, bIsLast);
  }

  ///请求查询转帐流水响应
  virtual void OnRspQryTransferSerial(CThostFtdcTransferSerialField *pTransferSerial, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspQryTransferSerialResponse,pTransferSerial,pRspInfo,nRequestID,bIsLast);
  }

  ///请求查询经纪公司交易参数响应
  virtual void OnRspQryBrokerTradingParams(CThostFtdcBrokerTradingParamsField *pBrokerTradingParams, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspQryBrokerTradingParamsResponse,pBrokerTradingParams,pRspInfo,nRequestID,bIsLast);
  }

  ///请求查询经纪公司交易算法响应
  virtual void OnRspQryBrokerTradingAlgos(CThostFtdcBrokerTradingAlgosField *pBrokerTradingAlgos, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspQryBrokerTradingAlgosResponse,pBrokerTradingAlgos,pRspInfo,nRequestID,bIsLast);
  }

  ///银行发起银行资金转期货通知
  virtual void OnRtnFromBankToFutureByBank(CThostFtdcRspTransferField *pRspTransfer)
  {
    this->OnResponse(OnRtnFromBankToFutureByBankResponse,pRspTransfer);
  }

  ///银行发起期货资金转银行通知
  virtual void OnRtnFromFutureToBankByBank(CThostFtdcRspTransferField *pRspTransfer)
  {
    this->OnResponse(OnRtnFromFutureToBankByBankResponse,pRspTransfer);
  }

  ///银行发起冲正银行转期货通知
  virtual void OnRtnRepealFromBankToFutureByBank(CThostFtdcRspRepealField *pRspRepeal)
  {
    this->OnResponse(OnRtnRepealFromBankToFutureByBankResponse,pRspRepeal);
  }

  ///银行发起冲正期货转银行通知
  virtual void OnRtnRepealFromFutureToBankByBank(CThostFtdcRspRepealField *pRspRepeal)
  {
    this->OnResponse(OnRtnRepealFromFutureToBankByBankResponse,pRspRepeal);
  }

  ///期货发起银行资金转期货通知
  virtual void OnRtnFromBankToFutureByFuture(CThostFtdcRspTransferField *pRspTransfer)
  {
    this->OnResponse(OnRtnFromBankToFutureByFutureResponse,pRspTransfer);
  }

  ///期货发起期货资金转银行通知
  virtual void OnRtnFromFutureToBankByFuture(CThostFtdcRspTransferField *pRspTransfer)
  {
    this->OnResponse(OnRtnFromFutureToBankByFutureResponse,pRspTransfer);
  }

  ///系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
  virtual void OnRtnRepealFromBankToFutureByFutureManual(CThostFtdcRspRepealField *pRspRepeal)
  {
    this->OnResponse(OnRtnRepealFromBankToFutureByFutureManualResponse,pRspRepeal);
  }

  ///系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
  virtual void OnRtnRepealFromFutureToBankByFutureManual(CThostFtdcRspRepealField *pRspRepeal)
  {
    this->OnResponse(OnRtnRepealFromFutureToBankByFutureManualResponse,pRspRepeal);
  }

  ///期货发起查询银行余额通知
  virtual void OnRtnQueryBankBalanceByFuture(CThostFtdcNotifyQueryAccountField *pNotifyQueryAccount)
  {
    this->OnResponse(OnRtnQueryBankBalanceByFutureResponse,pNotifyQueryAccount);
  }

  ///期货发起银行资金转期货错误回报
  virtual void OnErrRtnBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo)
  {
    this->OnResponse(OnErrRtnBankToFutureByFutureResponse,pReqTransfer,pRspInfo);
  }

  ///期货发起期货资金转银行错误回报
  virtual void OnErrRtnFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo)
  {
    this->OnResponse(OnErrRtnFutureToBankByFutureResponse,pReqTransfer,pRspInfo);
  }

  ///系统运行时期货端手工发起冲正银行转期货错误回报
  virtual void OnErrRtnRepealBankToFutureByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo)
  {
    this->OnResponse(OnErrRtnRepealBankToFutureByFutureManualResponse,pReqRepeal,pRspInfo);
  }

  ///系统运行时期货端手工发起冲正期货转银行错误回报
  virtual void OnErrRtnRepealFutureToBankByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo)
  {
    this->OnResponse(OnErrRtnRepealFutureToBankByFutureManualResponse,pReqRepeal,pRspInfo);
  }

  ///期货发起查询银行余额错误回报
  virtual void OnErrRtnQueryBankBalanceByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo)
  {
    this->OnResponse(OnErrRtnQueryBankBalanceByFutureResponse,pReqQueryAccount,pRspInfo);
  }

  ///期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
  virtual void OnRtnRepealFromBankToFutureByFuture(CThostFtdcRspRepealField *pRspRepeal)
  {
    this->OnResponse(OnRtnRepealFromBankToFutureByFutureResponse,pRspRepeal);
  }

  ///期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
  virtual void OnRtnRepealFromFutureToBankByFuture(CThostFtdcRspRepealField *pRspRepeal)
  {
    this->OnResponse(OnRtnRepealFromFutureToBankByFutureResponse,pRspRepeal);
  }

  ///期货发起银行资金转期货应答
  virtual void OnRspFromBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspFromBankToFutureByFutureResponse,pReqTransfer,pRspInfo,nRequestID,bIsLast);
  }

  ///期货发起期货资金转银行应答
  virtual void OnRspFromFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspFromFutureToBankByFutureResponse,pReqTransfer,pRspInfo,nRequestID,bIsLast);
  }

  ///期货发起查询银行余额应答
  virtual void OnRspQueryBankAccountMoneyByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
  {
    this->OnResponse(OnRspQueryBankAccountMoneyByFutureResponse,pReqQueryAccount,pRspInfo,nRequestID,bIsLast);
  }



	///银行发起银期开户通知
  ///【20120828新增】
	virtual void OnRtnOpenAccountByBank(CThostFtdcOpenAccountField *pOpenAccount)
  {
    this->OnResponse(OnRtnOpenAccountByBankResponse,pOpenAccount);
  }

	///银行发起银期销户通知
  ///【20120828新增】
	virtual void OnRtnCancelAccountByBank(CThostFtdcCancelAccountField *pCancelAccount)
  {
    this->OnResponse(OnRtnCancelAccountByBankResponse,pCancelAccount);
  }

	///银行发起变更银行账号通知
  ///【20120828新增】
	virtual void OnRtnChangeAccountByBank(CThostFtdcChangeAccountField *pChangeAccount)
  {
    this->OnResponse(OnRtnChangeAccountByBankResponse,pChangeAccount);
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
