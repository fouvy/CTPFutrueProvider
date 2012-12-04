#pragma once


#ifdef CTPWRAPPER_EXPORTS
#define CTPWRAPPER_API __declspec(dllexport)
#else
#define CTPWRAPPER_API __declspec(dllimport)
#endif 

//CTP回调函数
typedef void (WINAPI *CTPResponseCallback)(int rspType, void *pRspData, void *pRspInfo, int nRequestID, bool bIsLast) ;

//打印函数
typedef void (WINAPI *PTRFUN)(const char*);
