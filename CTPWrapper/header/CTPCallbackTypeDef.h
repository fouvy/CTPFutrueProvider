#pragma once
#include "stdafx.h"


enum CTP_RESPONSE_TYPE
{
  ///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
  OnFrontConnectedResponse = 0,

  ///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
  ///@param nReason 错误原因
  ///        0x1001 网络读失败
  ///        0x1002 网络写失败
  ///        0x2001 接收心跳超时
  ///        0x2002 发送心跳失败
  ///        0x2003 收到错误报文
  OnFrontDisconnectedResponse,

  ///心跳超时警告。当长时间未收到报文时，该方法被调用。
  ///@param nTimeLapse 距离上次接收报文的时间
  OnHeartBeatWarningResponse,

  ///客户端认证响应
  //【20120828新增】
  OnRspAuthenticateResponse,

  ///登录请求响应
  OnRspUserLoginResponse,

  ///登出请求响应
  OnRspUserLogoutResponse,

  ///订阅行情应答
  OnRspSubMarketDataResponse,

  ///取消订阅行情应答
  OnRspUnSubMarketDataResponse,

  ///深度行情通知
  OnRtnDepthMarketDataResponse,

  ///用户口令更新请求响应
  OnRspUserPasswordUpdateResponse,

  ///资金账户口令更新请求响应
  OnRspTradingAccountPasswordUpdateResponse,

  ///报单录入请求响应
  OnRspOrderInsertResponse,

  ///预埋单录入请求响应
  OnRspParkedOrderInsertResponse,

  ///预埋撤单录入请求响应
  OnRspParkedOrderActionResponse,

  ///报单操作请求响应
  OnRspOrderActionResponse,

  ///查询最大报单数量响应
  OnRspQueryMaxOrderVolumeResponse,

  ///投资者结算结果确认响应
  OnRspSettlementInfoConfirmResponse,

  ///删除预埋单响应
  OnRspRemoveParkedOrderResponse,

  ///删除预埋撤单响应
  OnRspRemoveParkedOrderActionResponse,

  ///请求查询报单响应
  OnRspQryOrderResponse,

  ///请求查询成交响应
  OnRspQryTradeResponse,

  ///请求查询投资者持仓响应
  OnRspQryInvestorPositionResponse,

  ///请求查询资金账户响应
  OnRspQryTradingAccountResponse,

  ///请求查询投资者响应
  OnRspQryInvestorResponse,

  ///请求查询交易编码响应
  OnRspQryTradingCodeResponse,

  ///请求查询合约保证金率响应
  OnRspQryInstrumentMarginRateResponse,

  ///请求查询合约手续费率响应
  OnRspQryInstrumentCommissionRateResponse,

  ///请求查询交易所响应
  OnRspQryExchangeResponse,

  ///请求查询合约响应
  OnRspQryInstrumentResponse,

  ///请求查询行情响应
  OnRspQryDepthMarketDataResponse,

  ///请求查询投资者结算结果响应
  OnRspQrySettlementInfoResponse,

  ///请求查询转帐银行响应
  OnRspQryTransferBankResponse,

  ///请求查询投资者持仓明细响应
  OnRspQryInvestorPositionDetailResponse,

  ///请求查询客户通知响应
  OnRspQryNoticeResponse,

  ///请求查询结算信息确认响应
  OnRspQrySettlementInfoConfirmResponse,

  ///请求查询投资者持仓明细响应
  OnRspQryInvestorPositionCombineDetailResponse,

  ///查询保证金监管系统经纪公司资金账户密钥响应
  OnRspQryCFMMCTradingAccountKeyResponse,

  //请求查询仓单折抵信息响应
  ////【20120828新增】
  OnRspQryEWarrantOffsetResponse,

  ///请求查询转帐流水响应
  OnRspQryTransferSerialResponse,

  //请求查询银期签约关系响应
  ////【20120828新增】
  OnRspQryAccountregisterResponse,

  ///错误应答
  OnRspErrorResponse,

  ///报单通知
  OnRtnOrderResponse,

  ///成交通知
  OnRtnTradeResponse,

  ///报单录入错误回报
  OnErrRtnOrderInsertResponse,

  ///报单操作错误回报
  OnErrRtnOrderActionResponse,

  ///合约交易状态通知
  OnRtnInstrumentStatusResponse,

  ///交易通知
  OnRtnTradingNoticeResponse,

  ///提示条件单校验错误
  OnRtnErrorConditionalOrderResponse,

  ///请求查询签约银行响应
  OnRspQryContractBankResponse,

  ///请求查询预埋单响应
  OnRspQryParkedOrderResponse,

  ///请求查询预埋撤单响应
  OnRspQryParkedOrderActionResponse,

  ///请求查询交易通知响应
  OnRspQryTradingNoticeResponse,

  ///请求查询经纪公司交易参数响应
  OnRspQryBrokerTradingParamsResponse,

  ///请求查询经纪公司交易算法响应
  OnRspQryBrokerTradingAlgosResponse,

  ///银行发起银行资金转期货通知
  OnRtnFromBankToFutureByBankResponse,

  ///银行发起期货资金转银行通知
  OnRtnFromFutureToBankByBankResponse,

  ///银行发起冲正银行转期货通知
  OnRtnRepealFromBankToFutureByBankResponse,

  ///银行发起冲正期货转银行通知
  OnRtnRepealFromFutureToBankByBankResponse,

  ///期货发起银行资金转期货通知
  OnRtnFromBankToFutureByFutureResponse,

  ///期货发起期货资金转银行通知
  OnRtnFromFutureToBankByFutureResponse,

  ///系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
  OnRtnRepealFromBankToFutureByFutureManualResponse,

  ///系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
  OnRtnRepealFromFutureToBankByFutureManualResponse,

  ///期货发起查询银行余额通知
  OnRtnQueryBankBalanceByFutureResponse,

  ///期货发起银行资金转期货错误回报
  OnErrRtnBankToFutureByFutureResponse,

  ///期货发起期货资金转银行错误回报
  OnErrRtnFutureToBankByFutureResponse,

  ///系统运行时期货端手工发起冲正银行转期货错误回报
  OnErrRtnRepealBankToFutureByFutureManualResponse,

  ///系统运行时期货端手工发起冲正期货转银行错误回报
  OnErrRtnRepealFutureToBankByFutureManualResponse,

  ///期货发起查询银行余额错误回报
  OnErrRtnQueryBankBalanceByFutureResponse,

  ///期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
  OnRtnRepealFromBankToFutureByFutureResponse,

  ///期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
  OnRtnRepealFromFutureToBankByFutureResponse,

  ///期货发起银行资金转期货应答
  OnRspFromBankToFutureByFutureResponse,

  ///期货发起期货资金转银行应答
  OnRspFromFutureToBankByFutureResponse,

  ///期货发起查询银行余额应答
  OnRspQueryBankAccountMoneyByFutureResponse,

  ///银行发起银期开户通知
  OnRtnOpenAccountByBankResponse,

  ///银行发起银期销户通知
  OnRtnCancelAccountByBankResponse,

  ///银行发起变更银行账号通知
  OnRtnChangeAccountByBankResponse
};

enum CTP_REQUEST_TYPE
{

  TraderApiCreate = 1,


  ///删除接口对象本身
	///@remark 不再使用本接口对象时,调用该函数删除接口对象
	TraderApiRelease,
	
	///初始化
	///@remark 初始化运行环境,只有调用后,接口才开始工作
	TraderApiInit,
	
	///等待接口线程结束运行
	///@return 线程退出代码
	TraderApiJoin,
	
	///获取当前交易日
	///@retrun 获取到的交易日
	///@remark 只有登录成功后,才能得到正确的交易日
	TraderApiGetTradingDay,
	
	///注册前置机网络地址
	///@param pszFrontAddress：前置机网络地址。
	///@remark 网络地址的格式为：“protocol://ipaddress:port”，如：”tcp://127.0.0.1:17001”。 
	///@remark “tcp”代表传输协议，“127.0.0.1”代表服务器地址。”17001”代表服务器端口号。
	TraderApiRegisterFront,
	
  ///注册前置机网络地址
	///@param pszFrontAddress：前置机网络地址。
	///@remark 网络地址的格式为：“protocol://ipaddress:port”，如：”tcp://127.0.0.1:17001”。 
	///@remark “tcp”代表传输协议，“127.0.0.1”代表服务器地址。”17001”代表服务器端口号。
  TraderApiRegisterNameServer,

	///注册回调接口
	///@param pSpi 派生自回调接口类的实例
	TraderApiRegisterSpi,
	
	///订阅私有流。
	///@param nResumeType 私有流重传方式  
	///        THOST_TERT_RESTART:从本交易日开始重传
	///        THOST_TERT_RESUME:从上次收到的续传
	///        THOST_TERT_QUICK:只传送登录后私有流的内容
	///@remark 该方法要在Init方法前调用。若不调用则不会收到私有流的数据。
	TraderApiSubscribePrivateTopic,
	
	///订阅公共流。
	///@param nResumeType 公共流重传方式  
	///        THOST_TERT_RESTART:从本交易日开始重传
	///        THOST_TERT_RESUME:从上次收到的续传
	///        THOST_TERT_QUICK:只传送登录后公共流的内容
	///@remark 该方法要在Init方法前调用。若不调用则不会收到公共流的数据。
	TraderApiSubscribePublicTopic,

  ///客户端认证请求
  TraderApiAuthenticate,

  ///用户登录请求
  TraderApiReqUserLogin,

  ///登出请求
  TraderApiReqUserLogout,


  ///创建MdApi
  ///@param pszFlowPath 存贮订阅信息文件的目录，默认为当前目录
  ///@return 创建出的UserApi
  ///modify for udp marketdata
  MarketDataCreate,

  ///删除接口对象本身
  ///@remark 不再使用本接口对象时,调用该函数删除接口对象
  MarketDataRelease,

  ///初始化
  ///@remark 初始化运行环境,只有调用后,接口才开始工作
  MarketDataInit,

  ///等待接口线程结束运行
  ///@return 线程退出代码
  MarketDataJoin,

  ///获取当前交易日
  ///@retrun 获取到的交易日
  ///@remark 只有登录成功后,才能得到正确的交易日
  MarketDataGetTradingDay,

  ///注册前置机网络地址
  ///@param pszFrontAddress：前置机网络地址。
  ///@remark 网络地址的格式为：“protocol://ipaddress:port”，如：”tcp://127.0.0.1:17001”。 
  ///@remark “tcp”代表传输协议，“127.0.0.1”代表服务器地址。”17001”代表服务器端口号。
  MarketDataRegisterFront,

  ///注册名字服务器网络地址
  ///@param pszNsAddress：名字服务器网络地址。
  ///@remark 网络地址的格式为：“protocol://ipaddress:port”，如：”tcp://127.0.0.1:12001”。 
  ///@remark “tcp”代表传输协议，“127.0.0.1”代表服务器地址。”12001”代表服务器端口号。
  ///@remark RegisterNameServer优先于RegisterFront
  MarketDataRegisterNameServer,

  ///注册回调接口
  ///@param pSpi 派生自回调接口类的实例
  MarketDataRegisterSpi,

  ///用户登录请求
  MarketDataReqUserLogin,

  ///登出请求
  MarketDataReqUserLogout,

  ///订阅行情。
  ///@param ppInstrumentID 合约ID  
  ///@param nCount 要订阅/退订行情的合约个数
  ///@remark 
  MarketDataSubscribeMarketData,

  ///退订行情。
  ///@param ppInstrumentID 合约ID  
  ///@param nCount 要订阅/退订行情的合约个数
  ///@remark 
  MarketDataUnSubscribeMarketData,



  ///用户口令更新请求
  ReqUserPasswordUpdate = 100,

  ///资金账户口令更新请求
  ReqTradingAccountPasswordUpdate,

  ///报单录入请求
  ReqOrderInsert,

  ///预埋单录入请求
  ReqParkedOrderInsert,

  ///预埋撤单录入请求
  ReqParkedOrderAction,

  ///报单操作请求
  ReqOrderAction,

  ///查询最大报单数量请求
  ReqQueryMaxOrderVolume,

  ///投资者结算结果确认
  ReqSettlementInfoConfirm,

  ///请求删除预埋单
  ReqRemoveParkedOrder,

  ///请求删除预埋撤单
  ReqRemoveParkedOrderAction,

  ///请求查询报单
  ReqQryOrder,

  ///请求查询成交
  ReqQryTrade,

  ///请求查询投资者持仓
  ReqQryInvestorPosition,

  ///请求查询资金账户
  ReqQryTradingAccount,

  ///请求查询投资者
  ReqQryInvestor,

  ///请求查询交易编码
  ReqQryTradingCode,

  ///请求查询合约保证金率
  ReqQryInstrumentMarginRate,

  ///请求查询合约手续费率
  ReqQryInstrumentCommissionRate,

  ///请求查询交易所
  ReqQryExchange,

  ///请求查询合约
  ReqQryInstrument,

  ///请求查询行情
  ReqQryDepthMarketData,

  ///请求查询投资者结算结果
  ReqQrySettlementInfo,

  ///请求查询转帐银行
  ReqQryTransferBank,

  ///请求查询投资者持仓明细
  ReqQryInvestorPositionDetail,

  ///请求查询客户通知
  ReqQryNotice,

  ///请求查询结算信息确认
  ReqQrySettlementInfoConfirm,

  ///请求查询投资者持仓明细
  ReqQryInvestorPositionCombineDetail,

  ///请求查询保证金监管系统经纪公司资金账户密钥
  ReqQryCFMMCTradingAccountKey,

  ///请求查询仓单折抵信息
  ReqQryEWarrantOffset,

  ///请求查询转帐流水
  ReqQryTransferSerial,

  ///请求查询银期签约关系
  ReqQryAccountregister,

  ///请求查询签约银行
  ReqQryContractBank,

  ///请求查询预埋单
  ReqQryParkedOrder,

  ///请求查询预埋撤单
  ReqQryParkedOrderAction,

  ///请求查询交易通知
  ReqQryTradingNotice,

  ///请求查询经纪公司交易参数
  ReqQryBrokerTradingParams,

  ///请求查询经纪公司交易算法
  ReqQryBrokerTradingAlgos,

  ///期货发起银行资金转期货请求
  ReqFromBankToFutureByFuture,

  ///期货发起期货资金转银行请求
  ReqFromFutureToBankByFuture,

  ///期货发起查询银行余额请求
  ReqQueryBankAccountMoneyByFuture,

};


///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
typedef void (WINAPI *OnFrontConnectedCallback)();

///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
///@param nReason 错误原因
///        0x1001 网络读失败
///        0x1002 网络写失败
///        0x2001 接收心跳超时
///        0x2002 发送心跳失败
///        0x2003 收到错误报文
typedef void (WINAPI *OnFrontDisconnectedCallback)(int nReason);

///心跳超时警告。当长时间未收到报文时，该方法被调用。
///@param nTimeLapse 距离上次接收报文的时间
typedef void (WINAPI *OnHeartBeatWarningCallback)(int nTimeLapse);

///错误响应
typedef void (WINAPI *OnRspErrorCallback)(CThostFtdcRspInfoField *pRspInfo) ;


///登录请求响应
typedef void (WINAPI *OnRspUserLoginCallback)(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///登出请求响应
typedef void (WINAPI *OnRspUserLogoutCallback)(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///用户口令更新请求响应
typedef void (WINAPI *OnRspUserPasswordUpdateCallback)(CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;


//#####################################################################################
//            Market Data Spi Begin
//#####################################################################################
///订阅行情应答
typedef void (WINAPI *OnRspSubMarketDataCallback)(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

///取消订阅行情应答
typedef void (WINAPI *OnRspUnSubMarketDataCallback)(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

///深度行情通知
typedef void (WINAPI *OnRtnDepthMarketDataCallback)(CThostFtdcDepthMarketDataField *pDepthMarketData);
//#####################################################################################
//            Market Data Spi End
//#####################################################################################

