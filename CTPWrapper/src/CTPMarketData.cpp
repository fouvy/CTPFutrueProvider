
#include "..\header\stdafx.h"

///订阅行情。
///@param ppInstrumentID 合约ID  
///@param nCount 要订阅/退订行情的合约个数
///@remark 
extern "C" CTPWRAPPER_API int SubscribeMarketData(void *instance,char *ppInstrumentID[], int nCount)
{
	return ((CThostFtdcMdApi *)instance)->SubscribeMarketData(ppInstrumentID, nCount);
};

///退订行情。
///@param ppInstrumentID 合约ID  
///@param nCount 要订阅/退订行情的合约个数
///@remark 
extern "C" CTPWRAPPER_API int UnSubscribeMarketData(void *instance, char *ppInstrumentID[], int nCount)
{
	return ((CThostFtdcMdApi *)instance)->UnSubscribeMarketData(ppInstrumentID, nCount);
};

