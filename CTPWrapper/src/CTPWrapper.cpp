// CTPWrapper.cpp : 定义 DLL 应用程序的入口点。
//
#include "..\header\stdafx.h"

using namespace std;  

#ifdef _MANAGED
#pragma managed(push, off)
#endif

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
    return TRUE;
}

#ifdef _MANAGED
#pragma managed(pop)
#endif



extern "C" 
{

  CTPWRAPPER_API int Process(void *instance,CTP_REQUEST_TYPE type,int p1,char *p2)
  {

    printf("Process %s","Process");

    switch(type)
    {

    case TraderApiCreate:
      {
        //C#回调函数
        CTPResponseCallback callback = (CTPResponseCallback)instance;

        //创建交易接口
        CThostFtdcTraderApi* pTrader = CThostFtdcTraderApi::CreateFtdcTraderApi(p2);
        //创建交易回调
        CTPTraderSpi *spi = new CTPTraderSpi(callback);

        //注册回调
        pTrader->RegisterSpi((CThostFtdcTraderSpi*)spi);

        //订阅流
        pTrader->SubscribePublicTopic((THOST_TE_RESUME_TYPE)p1);
        pTrader->SubscribePrivateTopic((THOST_TE_RESUME_TYPE)p1);

        return (int)pTrader;
      }


      ///删除接口对象本身
      ///@remark 不再使用本接口对象时,调用该函数删除接口对象
    case TraderApiRelease:
      {

        CThostFtdcTraderApi* pTrader = (CThostFtdcTraderApi *)instance;

        pTrader->RegisterSpi(NULL);
        pTrader->Release();

        delete instance;
        instance = NULL;

        return 0;
      }

      ///初始化
      ///@remark 初始化运行环境,只有调用后,接口才开始工作
    case  TraderApiInit:
      {
        ((CThostFtdcTraderApi *)instance)->Init();
        return 0;
      }

      ///等待接口线程结束运行
      ///@return 线程退出代码
    case TraderApiJoin:
      {
        return ((CThostFtdcTraderApi *)instance)->Join();
      }

      ///获取当前交易日
      ///@retrun 获取到的交易日
      ///@remark 只有登录成功后,才能得到正确的交易日
    case TraderApiGetTradingDay:
      {
        const char* date = ((CThostFtdcTraderApi *)instance)->GetTradingDay();
        memcpy(p2,date,sizeof(date));
        return 0;
      }

      ///注册前置机网络地址
      ///@param pszFrontAddress：前置机网络地址。
      ///@remark 网络地址的格式为：“protocol://ipaddress:port”，如：”tcp://127.0.0.1:17001”。 
      ///@remark “tcp”代表传输协议，“127.0.0.1”代表服务器地址。”17001”代表服务器端口号。
    case TraderApiRegisterFront:
      {
        ((CThostFtdcTraderApi *)instance)->RegisterFront(p2);
        return 0;
      }

      ///注册前置机网络地址
      ///@param pszFrontAddress：前置机网络地址。
      ///@remark 网络地址的格式为：“protocol://ipaddress:port”，如：”tcp://127.0.0.1:17001”。 
      ///@remark “tcp”代表传输协议，“127.0.0.1”代表服务器地址。”17001”代表服务器端口号。
    case TraderApiRegisterNameServer:
      {
        ((CThostFtdcTraderApi *)instance)->RegisterNameServer(p2);
        return 0;
      }

      ///注册回调接口
      ///@param pSpi 派生自回调接口类的实例
    case TraderApiRegisterSpi:
      {
        ((CThostFtdcTraderApi *)instance)->RegisterSpi((CThostFtdcTraderSpi*)p1);
        return 0;
      }

      ///订阅私有流。
      ///@param nResumeType 私有流重传方式  
      ///        THOST_TERT_RESTART:从本交易日开始重传
      ///        THOST_TERT_RESUME:从上次收到的续传
      ///        THOST_TERT_QUICK:只传送登录后私有流的内容
      ///@remark 该方法要在Init方法前调用。若不调用则不会收到私有流的数据。
    case TraderApiSubscribePrivateTopic:
      {
        ((CThostFtdcTraderApi *)instance)->SubscribePrivateTopic((THOST_TE_RESUME_TYPE)p1);
        return 0;
      }

      ///订阅公共流。
      ///@param nResumeType 公共流重传方式  
      ///        THOST_TERT_RESTART:从本交易日开始重传
      ///        THOST_TERT_RESUME:从上次收到的续传
      ///        THOST_TERT_QUICK:只传送登录后公共流的内容
      ///@remark 该方法要在Init方法前调用。若不调用则不会收到公共流的数据。
    case TraderApiSubscribePublicTopic:
      {
        ((CThostFtdcTraderApi *)instance)->SubscribePublicTopic((THOST_TE_RESUME_TYPE)p1);
        return 0;
      }


    case MarketDataCreate:
      {
        //C#回调函数
        CTPResponseCallback callback = (CTPResponseCallback)instance;

        //创建行情接口
        CThostFtdcMdApi* pMdApi = CThostFtdcMdApi::CreateFtdcMdApi(p2);
        //创建交易回调
        CTPMarketDataSpi *spi = new CTPMarketDataSpi(callback);

        //注册回调
        pMdApi->RegisterSpi(spi);

        //订阅流
        //pMdApi->SubscribePublicTopic((THOST_TE_RESUME_TYPE)p1);
        //pMdApi->SubscribePrivateTopic((THOST_TE_RESUME_TYPE)p1);

        return (int)pMdApi;
      }

    case MarketDataRelease:
      {
        CThostFtdcMdApi* pMarketData = (CThostFtdcMdApi *)instance;

        pMarketData->RegisterSpi(NULL);
        pMarketData->Release();

        delete instance;
        instance = NULL;

        return 0;
      }

    case MarketDataInit:
      {
        ((CThostFtdcMdApi *)instance)->Init();
        return 0;
      }

    case MarketDataRegisterFront:
      {
        ((CThostFtdcMdApi *)instance)->RegisterFront(p2);
        return 0;
      }

    case MarketDataRegisterNameServer:
      {
        ((CThostFtdcMdApi *)instance)->RegisterNameServer(p2);
        return 0;
      }
      
       ///订阅行情
    case MarketDataSubscribeMarketData:
      {
        char** p = (char**)calloc(p1, sizeof(char*));

        p = (char**)p2;

        p[0] = *p;

        for(int i=1;i<p1-1;i++)
        {
          p[i] = p[i-1] + strlen(p[i-1]) + 1;
        }

        return ((CThostFtdcMdApi*)instance)->SubscribeMarketData(p, p1);
      }

       ///退订行情
    case MarketDataUnSubscribeMarketData:
      {
        char** p = (char**)calloc(p1, sizeof(char*));

        

        //p = (char**)p2;
        OutputDebugStringA(p2);
        p[0] = p2;

        for(int i=1;i<p1-1;i++)
        {
          p[i] = p[i-1] + strlen(p[i-1]) + 1;
        }

        return ((CThostFtdcMdApi*)instance)->UnSubscribeMarketData(p, p1);
      }
    }
  }

  ///用户登录请求
  CTPWRAPPER_API int ProcessRequest(void *instance, CTP_REQUEST_TYPE type, void *pReqData, int nRequestID)
  {
    switch(type)
    {
      ///客户端认证请求
      ///【20120828增加】
    case TraderApiAuthenticate:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqAuthenticate((CThostFtdcReqAuthenticateField*)pReqData, nRequestID);
      }

      ///用户登录请求
    case TraderApiReqUserLogin:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqUserLogin((CThostFtdcReqUserLoginField*)pReqData, nRequestID);
      }


      ///登出请求
    case TraderApiReqUserLogout:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqUserLogout((CThostFtdcUserLogoutField*)pReqData, nRequestID);
      }

      ///用户登录请求
    case MarketDataReqUserLogin:
      {
        return ((CThostFtdcMdApi*)instance)->ReqUserLogin((CThostFtdcReqUserLoginField*)pReqData, nRequestID);
      }


      ///登出请求
    case MarketDataReqUserLogout:
      {
        return ((CThostFtdcMdApi*)instance)->ReqUserLogout((CThostFtdcUserLogoutField*)pReqData, nRequestID);
      }



      ///用户口令更新请求
    case ReqUserPasswordUpdate:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqUserPasswordUpdate((CThostFtdcUserPasswordUpdateField*)pReqData, nRequestID);
      }


      ///资金账户口令更新请求
    case ReqTradingAccountPasswordUpdate:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqTradingAccountPasswordUpdate((CThostFtdcTradingAccountPasswordUpdateField*)pReqData, nRequestID);
      }


      ///报单录入请求
    case ReqOrderInsert:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqOrderInsert((CThostFtdcInputOrderField*)pReqData, nRequestID);
      }


      ///预埋单录入请求
    case ReqParkedOrderInsert:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqParkedOrderInsert((CThostFtdcParkedOrderField*)pReqData, nRequestID);
      }


      ///预埋撤单录入请求
    case ReqParkedOrderAction:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqParkedOrderAction((CThostFtdcParkedOrderActionField*)pReqData, nRequestID);
      }


      ///报单操作请求
    case ReqOrderAction:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqOrderAction((CThostFtdcInputOrderActionField*)pReqData, nRequestID);
      }


      ///查询最大报单数量请求
    case ReqQueryMaxOrderVolume:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQueryMaxOrderVolume((CThostFtdcQueryMaxOrderVolumeField*)pReqData, nRequestID);
      }


      ///投资者结算结果确认
    case ReqSettlementInfoConfirm:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqSettlementInfoConfirm((CThostFtdcSettlementInfoConfirmField*)pReqData, nRequestID);
      }


      ///请求删除预埋单
    case ReqRemoveParkedOrder:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqRemoveParkedOrder((CThostFtdcRemoveParkedOrderField*)pReqData, nRequestID);
      }


      ///请求删除预埋撤单
    case ReqRemoveParkedOrderAction:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqRemoveParkedOrderAction((CThostFtdcRemoveParkedOrderActionField*)pReqData, nRequestID);
      }


      ///请求查询报单
    case ReqQryOrder:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryOrder((CThostFtdcQryOrderField*)pReqData, nRequestID);
      }


      ///请求查询成交
    case ReqQryTrade:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTrade((CThostFtdcQryTradeField*)pReqData, nRequestID);
      }


      ///请求查询投资者持仓
    case ReqQryInvestorPosition:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInvestorPosition((CThostFtdcQryInvestorPositionField*)pReqData, nRequestID);
      }


      ///请求查询资金账户
    case ReqQryTradingAccount:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTradingAccount((CThostFtdcQryTradingAccountField*)pReqData, nRequestID);
      }


      ///请求查询投资者
    case ReqQryInvestor:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInvestor((CThostFtdcQryInvestorField*)pReqData, nRequestID);
      }


      ///请求查询交易编码
    case ReqQryTradingCode:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTradingCode((CThostFtdcQryTradingCodeField*)pReqData, nRequestID);
      }


      ///请求查询合约保证金率
    case ReqQryInstrumentMarginRate:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInstrumentMarginRate((CThostFtdcQryInstrumentMarginRateField*)pReqData, nRequestID);
      }


      ///请求查询合约手续费率
    case ReqQryInstrumentCommissionRate:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInstrumentCommissionRate((CThostFtdcQryInstrumentCommissionRateField*)pReqData, nRequestID);
      }


      ///请求查询交易所
    case ReqQryExchange:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryExchange((CThostFtdcQryExchangeField*)pReqData, nRequestID);
      }


      ///请求查询合约
    case ReqQryInstrument:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInstrument((CThostFtdcQryInstrumentField*)pReqData, nRequestID);
      }


      ///请求查询行情
    case ReqQryDepthMarketData:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryDepthMarketData((CThostFtdcQryDepthMarketDataField*)pReqData, nRequestID);
      }


      ///请求查询投资者结算结果
    case ReqQrySettlementInfo:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQrySettlementInfo((CThostFtdcQrySettlementInfoField*)pReqData, nRequestID);
      }


      ///请求查询转帐银行
    case ReqQryTransferBank:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTransferBank((CThostFtdcQryTransferBankField*)pReqData, nRequestID);
      }


      ///请求查询投资者持仓明细
    case ReqQryInvestorPositionDetail:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInvestorPositionDetail((CThostFtdcQryInvestorPositionDetailField*)pReqData, nRequestID);
      }


      ///请求查询客户通知
    case ReqQryNotice:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryNotice((CThostFtdcQryNoticeField*)pReqData, nRequestID);
      }


      ///请求查询结算信息确认
    case ReqQrySettlementInfoConfirm:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQrySettlementInfoConfirm((CThostFtdcQrySettlementInfoConfirmField*)pReqData, nRequestID);
      }


      ///请求查询投资者持仓明细
    case ReqQryInvestorPositionCombineDetail:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryInvestorPositionCombineDetail((CThostFtdcQryInvestorPositionCombineDetailField*)pReqData, nRequestID);
      }


      ///请求查询保证金监管系统经纪公司资金账户密钥
    case ReqQryCFMMCTradingAccountKey:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryCFMMCTradingAccountKey((CThostFtdcQryCFMMCTradingAccountKeyField*)pReqData, nRequestID);
      }

      ///请求查询仓单折抵信息
      ///【20120828增加】
    case ReqQryEWarrantOffset:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryEWarrantOffset((CThostFtdcQryEWarrantOffsetField*)pReqData, nRequestID);
      }

      ///请求查询转帐流水
    case ReqQryTransferSerial:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTransferSerial((CThostFtdcQryTransferSerialField*)pReqData, nRequestID);
      }

      ///请求查询银期签约关系
      ///【20120828增加】
    case ReqQryAccountregister:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryAccountregister((CThostFtdcQryAccountregisterField*)pReqData, nRequestID);
      }

      ///请求查询签约银行
    case ReqQryContractBank:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryContractBank((CThostFtdcQryContractBankField*)pReqData, nRequestID);
      }


      ///请求查询预埋单
    case ReqQryParkedOrder:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryParkedOrder((CThostFtdcQryParkedOrderField*)pReqData, nRequestID);
      }


      ///请求查询预埋撤单
    case ReqQryParkedOrderAction:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryParkedOrderAction((CThostFtdcQryParkedOrderActionField*)pReqData, nRequestID);
      }


      ///请求查询交易通知
    case ReqQryTradingNotice:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryTradingNotice((CThostFtdcQryTradingNoticeField*)pReqData, nRequestID);
      }


      ///请求查询经纪公司交易参数
    case ReqQryBrokerTradingParams:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryBrokerTradingParams((CThostFtdcQryBrokerTradingParamsField*)pReqData, nRequestID);
      }


      ///请求查询经纪公司交易算法
    case ReqQryBrokerTradingAlgos:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQryBrokerTradingAlgos((CThostFtdcQryBrokerTradingAlgosField*)pReqData, nRequestID);
      }


      ///期货发起银行资金转期货请求
    case ReqFromBankToFutureByFuture:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqFromBankToFutureByFuture((CThostFtdcReqTransferField*)pReqData, nRequestID);
      }


      ///期货发起期货资金转银行请求
    case ReqFromFutureToBankByFuture:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqFromFutureToBankByFuture((CThostFtdcReqTransferField*)pReqData, nRequestID);
      }


      ///期货发起查询银行余额请求
    case ReqQueryBankAccountMoneyByFuture:
      {
        return ((CThostFtdcTraderApi*)instance)->ReqQueryBankAccountMoneyByFuture((CThostFtdcReqQueryAccountField*)pReqData, nRequestID);
      }
    }
  };

  typedef void (WINAPI *PTRFUN)(const char*);

  PTRFUN debug_output;

  CTPWRAPPER_API void WINAPI SetOutputCallback(PTRFUN pfun) 
  {
	  debug_output = pfun;
  }

  void OutputLog(const char* msg)
  {
    if(debug_output)
	    debug_output(msg);
  }
  


};
