// CTPWrapper.cpp : 定义 DLL 应用程序的入口点。
//
#include "..\header\include_CTPStock.h"


extern "C" 
{

  CTPWRAPPER_API int CTPStockProcess(void *instance,CTP_STOCK_REQUEST_TYPE type,int p1,char *p2)
  {

    printf("Process %s","Process");

    switch(type)
    {

    case TraderApiCreate:
      {
        //C#回调函数
        CTPResponseCallback callback = (CTPResponseCallback)instance;

        //创建交易接口
        CZQThostFtdcTraderApi* pTrader = CZQThostFtdcTraderApi::CreateFtdcTraderApi(p2);
        //创建交易回调
        CTPStockTraderSpi *spi = new CTPStockTraderSpi(callback);

        //注册回调
        pTrader->RegisterSpi((CZQThostFtdcTraderSpi*)spi);

        //订阅流
        pTrader->SubscribePublicTopic((ZQTHOST_TE_RESUME_TYPE)p1);
        pTrader->SubscribePrivateTopic((ZQTHOST_TE_RESUME_TYPE)p1);

        return (int)pTrader;
      }


      ///删除接口对象本身
      ///@remark 不再使用本接口对象时,调用该函数删除接口对象
    case TraderApiRelease:
      {

        CZQThostFtdcTraderApi* pTrader = (CZQThostFtdcTraderApi *)instance;

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
        ((CZQThostFtdcTraderApi *)instance)->Init();
        return 0;
      }

      ///等待接口线程结束运行
      ///@return 线程退出代码
    case TraderApiJoin:
      {
        return ((CZQThostFtdcTraderApi *)instance)->Join();
      }

      ///获取当前交易日
      ///@retrun 获取到的交易日
      ///@remark 只有登录成功后,才能得到正确的交易日
    case TraderApiGetTradingDay:
      {
        const char* date = ((CZQThostFtdcTraderApi *)instance)->GetTradingDay();
        memcpy(p2,date,sizeof(date));
        return 0;
      }

      ///注册前置机网络地址
      ///@param pszFrontAddress：前置机网络地址。
      ///@remark 网络地址的格式为：“protocol://ipaddress:port”，如：”tcp://127.0.0.1:17001”。 
      ///@remark “tcp”代表传输协议，“127.0.0.1”代表服务器地址。”17001”代表服务器端口号。
    case TraderApiRegisterFront:
      {
        ((CZQThostFtdcTraderApi *)instance)->RegisterFront(p2);
        return 0;
      }

      ///注册前置机网络地址
      ///@param pszFrontAddress：前置机网络地址。
      ///@remark 网络地址的格式为：“protocol://ipaddress:port”，如：”tcp://127.0.0.1:17001”。 
      ///@remark “tcp”代表传输协议，“127.0.0.1”代表服务器地址。”17001”代表服务器端口号。
    //case TraderApiRegisterNameServer:
    //  {
    //    ((CZQThostFtdcTraderApi *)instance)->RegisterNameServer(p2);
    //    return 0;
    //  }

      ///注册回调接口
      ///@param pSpi 派生自回调接口类的实例
    case TraderApiRegisterSpi:
      {
        ((CZQThostFtdcTraderApi *)instance)->RegisterSpi((CZQThostFtdcTraderSpi*)p1);
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
        ((CZQThostFtdcTraderApi *)instance)->SubscribePrivateTopic((ZQTHOST_TE_RESUME_TYPE)p1);
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
        ((CZQThostFtdcTraderApi *)instance)->SubscribePublicTopic((ZQTHOST_TE_RESUME_TYPE)p1);
        return 0;
      }


    case MarketDataCreate:
      {
        //C#回调函数
        CTPResponseCallback callback = (CTPResponseCallback)instance;

        //创建行情接口
        CZQThostFtdcMdApi* pMdApi = CZQThostFtdcMdApi::CreateFtdcMdApi(p2);
        //创建交易回调
        CTPStockMarketDataSpi *spi = new CTPStockMarketDataSpi(callback);

        //注册回调
        pMdApi->RegisterSpi(spi);

        //订阅流
        //pMdApi->SubscribePublicTopic((THOST_TE_RESUME_TYPE)p1);
        //pMdApi->SubscribePrivateTopic((THOST_TE_RESUME_TYPE)p1);

        return (int)pMdApi;
      }

    case MarketDataRelease:
      {
        CZQThostFtdcMdApi* pMarketData = (CZQThostFtdcMdApi *)instance;

        pMarketData->RegisterSpi(NULL);
        pMarketData->Release();

        delete instance;
        instance = NULL;

        return 0;
      }

    case MarketDataInit:
      {
        ((CZQThostFtdcMdApi *)instance)->Init();
        return 0;
      }

    case MarketDataRegisterFront:
      {
        ((CZQThostFtdcMdApi *)instance)->RegisterFront(p2);
        return 0;
      }

    //case MarketDataRegisterNameServer:
    //  {
    //    ((CZQThostFtdcMdApi *)instance)->RegisterNameServer(p2);
    //    return 0;
    //  }
      
       ///订阅行情
    case MarketDataSubscribeMarketData:
      {
        //char** p = (char**)calloc(p1, sizeof(char*));

        //p = (char**)p2;

        //p[0] = *p;

        //for(int i=1;i<p1-1;i++)
        //{
        //  p[i] = p[i-1] + strlen(p[i-1]) + 1;
        //}

        //return ((CZQThostFtdcMdApi*)instance)->SubscribeMarketData(p, p1, p2);
        return -1;
      }

       ///退订行情
    case MarketDataUnSubscribeMarketData:
      {
        //char** p = (char**)calloc(p1, sizeof(char*));

        ////p = (char**)p2;
        //OutputDebugStringA(p2);
        //p[0] = p2;

        //for(int i=1;i<p1-1;i++)
        //{
        //  p[i] = p[i-1] + strlen(p[i-1]) + 1;
        //}

        //return ((CZQThostFtdcMdApi*)instance)->UnSubscribeMarketData(p, p1, p2);
        return -1;
      }
    }
  }

  ///用户登录请求
  CTPWRAPPER_API int CTPStockProcessRequest(void *instance, CTP_STOCK_REQUEST_TYPE type, void *pReqData, int nRequestID)
  {
    switch(type)
    {
      ///客户端认证请求
      ///【20120828增加】
    case TraderApiAuthenticate:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqAuthenticate((CZQThostFtdcReqAuthenticateField*)pReqData, nRequestID);
      }

      ///用户登录请求
    case TraderApiReqUserLogin:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqUserLogin((CZQThostFtdcReqUserLoginField*)pReqData, nRequestID);
      }


      ///登出请求
    case TraderApiReqUserLogout:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqUserLogout((CZQThostFtdcUserLogoutField*)pReqData, nRequestID);
      }

      ///用户登录请求
    case MarketDataReqUserLogin:
      {
        return ((CZQThostFtdcMdApi*)instance)->ReqUserLogin((CZQThostFtdcReqUserLoginField*)pReqData, nRequestID);
      }


      ///登出请求
    case MarketDataReqUserLogout:
      {
        return ((CZQThostFtdcMdApi*)instance)->ReqUserLogout((CZQThostFtdcUserLogoutField*)pReqData, nRequestID);
      }



      ///用户口令更新请求
    case ReqUserPasswordUpdate:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqUserPasswordUpdate((CZQThostFtdcUserPasswordUpdateField*)pReqData, nRequestID);
      }

      ///报单录入请求
    case ReqOrderInsert:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqOrderInsert((CZQThostFtdcInputOrderField*)pReqData, nRequestID);
      }


      ///报单操作请求
    case ReqOrderAction:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqOrderAction((CZQThostFtdcInputOrderActionField*)pReqData, nRequestID);
      }


      ///查询最大报单数量请求
    case ReqQueryMaxOrderVolume:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQueryMaxOrderVolume((CZQThostFtdcQueryMaxOrderVolumeField*)pReqData, nRequestID);
      }


      ///请求查询报单
    case ReqQryOrder:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryOrder((CZQThostFtdcQryOrderField*)pReqData, nRequestID);
      }


      ///请求查询成交
    case ReqQryTrade:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryTrade((CZQThostFtdcQryTradeField*)pReqData, nRequestID);
      }


      ///请求查询投资者持仓
    case ReqQryInvestorPosition:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryInvestorPosition((CZQThostFtdcQryInvestorPositionField*)pReqData, nRequestID);
      }


      ///请求查询资金账户
    case ReqQryTradingAccount:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryTradingAccount((CZQThostFtdcQryTradingAccountField*)pReqData, nRequestID);
      }


      ///请求查询投资者
    case ReqQryInvestor:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryInvestor((CZQThostFtdcQryInvestorField*)pReqData, nRequestID);
      }


      ///请求查询交易编码
    case ReqQryTradingCode:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryTradingCode((CZQThostFtdcQryTradingCodeField*)pReqData, nRequestID);
      }

      ///请求查询合约手续费率
    case ReqQryInstrumentCommissionRate:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryInstrumentCommissionRate((CZQThostFtdcQryInstrumentCommissionRateField*)pReqData, nRequestID);
      }


      ///请求查询交易所
    case ReqQryExchange:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryExchange((CZQThostFtdcQryExchangeField*)pReqData, nRequestID);
      }


      ///请求查询合约
    case ReqQryInstrument:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryInstrument((CZQThostFtdcQryInstrumentField*)pReqData, nRequestID);
      }


      ///请求查询行情
    case ReqQryDepthMarketData:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryDepthMarketData((CZQThostFtdcQryDepthMarketDataField*)pReqData, nRequestID);
      }

      ///请求查询投资者持仓明细
    case ReqQryInvestorPositionDetail:
      {
        return ((CZQThostFtdcTraderApi*)instance)->ReqQryInvestorPositionDetail((CZQThostFtdcQryInvestorPositionDetailField*)pReqData, nRequestID);
      }
    }
  };

};
