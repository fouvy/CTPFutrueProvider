using System;
using System.Collections.Generic;

namespace CalmBeltFund.Trading.CTP
{
  partial class CTPTrader
  {
    #region Events

    /// <summary>
    /// 客户端认证响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcRspAuthenticateField>> AuthenticateResponse
    {
      add { AddHandler(CTPResponseType.AuthenticateResponse, value); }
      remove { RemoveHandler(CTPResponseType.AuthenticateResponse, value); }
    }

    /// <summary>
    /// 资金账户口令更新请求响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcTradingAccountPasswordUpdateField>> TradingAccountPasswordUpdateResponse
    {
      add { AddHandler(CTPResponseType.TradingAccountPasswordUpdateResponse, value); }
      remove { RemoveHandler(CTPResponseType.TradingAccountPasswordUpdateResponse, value); }
    }

    public event EventHandler<CTPEventArgs<CThostFtdcUserPasswordUpdateField>> UserPasswordUpdateResponse
    {
      add { AddHandler(CTPResponseType.UserPasswordUpdateResponse, value); }
      remove { RemoveHandler(CTPResponseType.UserPasswordUpdateResponse, value); }
    }

    /// <summary>
    /// 报单录入请求响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcInputOrderField>> OrderInsertResponse
    {
      add { AddHandler(CTPResponseType.OrderInsertResponse, value); }
      remove { RemoveHandler(CTPResponseType.OrderInsertResponse, value); }
    }

    /// <summary>
    /// 预埋单录入请求响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcParkedOrderField>> ParkedOrderInsertResponse
    {
      add { AddHandler(CTPResponseType.ParkedOrderInsertResponse, value); }
      remove { RemoveHandler(CTPResponseType.ParkedOrderInsertResponse, value); }
    }

    /// <summary>
    /// 预埋撤单录入请求响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcParkedOrderActionField>> ParkedOrderActionResponse
    {
      add { AddHandler(CTPResponseType.ParkedOrderActionResponse, value); }
      remove { RemoveHandler(CTPResponseType.ParkedOrderActionResponse, value); }
    }

    /// <summary>
    /// 报单操作请求响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcInputOrderActionField>> OrderActionResponse
    {
      add { AddHandler(CTPResponseType.OrderActionResponse, value); }
      remove { RemoveHandler(CTPResponseType.OrderActionResponse, value); }
    }

    /// <summary>
    /// 查询最大报单数量响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcQueryMaxOrderVolumeField>> QueryMaxOrderVolumeResponse
    {
      add { AddHandler(CTPResponseType.QueryMaxOrderVolumeResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryMaxOrderVolumeResponse, value); }
    }

    /// <summary>
    /// 投资者结算结果确认响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcSettlementInfoConfirmField>> SettlementInfoConfirmResponse
    {
      add { AddHandler(CTPResponseType.SettlementInfoConfirmResponse, value); }
      remove { RemoveHandler(CTPResponseType.SettlementInfoConfirmResponse, value); }
    }


    /// <summary>
    /// 删除预埋单响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcRemoveParkedOrderField>> RemoveParkedOrderResponse
    {
      add { AddHandler(CTPResponseType.RemoveParkedOrderResponse, value); }
      remove { RemoveHandler(CTPResponseType.RemoveParkedOrderResponse, value); }
    }

    /// <summary>
    /// 删除预埋撤单响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcRemoveParkedOrderActionField>> RemoveParkedOrderActionResponse
    {
      add { AddHandler(CTPResponseType.RemoveParkedOrderActionResponse, value); }
      remove { RemoveHandler(CTPResponseType.RemoveParkedOrderActionResponse, value); }
    }

    /// <summary>
    /// 请求查询报单响应
    /// </summary>
    public event EventHandler<CTPEventArgs<List<CThostFtdcOrderField>>> QueryOrderResponse
    {
      add { AddHandler(CTPResponseType.QueryOrderResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryOrderResponse, value); }
    }

    /// <summary>
    /// 请求查询成交响应
    /// </summary>
    public event EventHandler<CTPEventArgs<List<CThostFtdcTradeField>>> QueryTradeResponse
    {
      add { AddHandler(CTPResponseType.QueryTradeResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryTradeResponse, value); }
    }

    /// <summary>
    /// 请求查询投资者持仓响应
    /// </summary>
    public event EventHandler<CTPEventArgs<List<CThostFtdcInvestorPositionField>>> QueryInvestorPositionResponse
    {
      add { AddHandler(CTPResponseType.QueryInvestorPositionResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryInvestorPositionResponse, value); }
    }

    /// <summary>
    /// 请求查询资金账户响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcTradingAccountField>> QueryTradingAccountResponse
    {
      add { AddHandler(CTPResponseType.QueryTradingAccountResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryTradingAccountResponse, value); }
    }

    /// <summary>
    /// 请求查询投资者响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcInvestorField>> QueryInvestorResponse
    {
      add { AddHandler(CTPResponseType.QueryInvestorResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryInvestorResponse, value); }
    }

    /// <summary>
    /// 请求查询交易编码响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcTradingCodeField>> QueryTradingCodeResponse
    {
      add { AddHandler(CTPResponseType.QueryTradingCodeResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryTradingCodeResponse, value); }
    }

    /// <summary>
    /// 请求查询合约保证金率响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcInstrumentMarginRateField>> QueryInstrumentMarginRateResponse
    {
      add { AddHandler(CTPResponseType.QueryInstrumentMarginRateResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryInstrumentMarginRateResponse, value); }
    }

    /// <summary>
    /// 请求查询合约手续费率响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcInstrumentCommissionRateField>> QueryInstrumentCommissionRateResponse
    {
      add { AddHandler(CTPResponseType.QueryInstrumentCommissionRateResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryInstrumentCommissionRateResponse, value); }
    }

    /// <summary>
    /// 请求查询交易所响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CTPExchange>> QueryExchangeResponse
    {
      add { AddHandler(CTPResponseType.QueryExchangeResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryExchangeResponse, value); }
    }

    /// <summary>
    /// 请求查询合约响应
    /// </summary>
    public event EventHandler<CTPEventArgs<List<CTPInstrument>>> QueryInstrumentResponse
    {
      add { AddHandler(CTPResponseType.QueryInstrumentResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryInstrumentResponse, value); }
    }

    /// <summary>
    /// 请求查询行情响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcDepthMarketDataField>> QueryDepthMarketDataResponse
    {
      add { AddHandler(CTPResponseType.QueryDepthMarketDataResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryDepthMarketDataResponse, value); }
    }

    /// <summary>
    /// 请求查询投资者结算结果响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CTPSettlementInfo>> QuerySettlementInfoResponse
    {
      add { AddHandler(CTPResponseType.QuerySettlementInfoResponse, value); }
      remove { RemoveHandler(CTPResponseType.QuerySettlementInfoResponse, value); }
    }

    /// <summary>
    /// 请求查询转帐银行响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcTransferBankField>> QueryTransferBankResponse
    {
      add { AddHandler(CTPResponseType.QueryTransferBankResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryTransferBankResponse, value); }
    }

    /// <summary>
    /// 请求查询投资者持仓明细响应
    /// </summary>
    public event EventHandler<CTPEventArgs<List<CThostFtdcInvestorPositionCombineDetailField>>> QueryInvestorPositionCombineDetailResponse
    {
      add { AddHandler(CTPResponseType.QueryInvestorPositionCombineDetailResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryInvestorPositionCombineDetailResponse, value); }
    }

    /// <summary>
    /// 请求查询投资者持仓明细响应
    /// </summary>
    public event EventHandler<CTPEventArgs<List<CThostFtdcInvestorPositionDetailField>>> QueryInvestorPositionDetailResponse
    {
      add { AddHandler(CTPResponseType.QueryInvestorPositionDetailResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryInvestorPositionDetailResponse, value); }
    }

    /// <summary>
    /// 请求查询客户通知响应
    /// </summary>
    public event EventHandler<CTPEventArgs<String>> QueryNoticeResponse
    {
      add { AddHandler(CTPResponseType.QueryNoticeResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryNoticeResponse, value); }
    }

    /// <summary>
    /// 请求查询结算信息确认响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcSettlementInfoConfirmField>> QuerySettlementInfoConfirmResponse
    {
      add { AddHandler(CTPResponseType.QuerySettlementInfoConfirmResponse, value); }
      remove { RemoveHandler(CTPResponseType.QuerySettlementInfoConfirmResponse, value); }
    }


    /// <summary>
    /// 报单通知
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcOrderField>> ReturnOrderResponse
    {
      add { AddHandler(CTPResponseType.ReturnOrderResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnOrderResponse, value); }
    }

    /// <summary>
    /// 成交通知
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcTradeField>> ReturnTradeResponse
    {
      add { AddHandler(CTPResponseType.ReturnTradeResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnTradeResponse, value); }
    }

    /// <summary>
    /// 报单录入错误回报
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcInputOrderField>> ErrorReturnOrderInsertResponse
    {
      add { AddHandler(CTPResponseType.ErrorReturnOrderInsertResponse, value); }
      remove { RemoveHandler(CTPResponseType.ErrorReturnOrderInsertResponse, value); }
    }

    /// <summary>
    /// 报单操作错误回报
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcOrderActionField>> ErrorReturnOrderActionResponse
    {
      add { AddHandler(CTPResponseType.ErrorReturnOrderActionResponse, value); }
      remove { RemoveHandler(CTPResponseType.ErrorReturnOrderActionResponse, value); }
    }

    /// <summary>
    /// 合约交易状态通知
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcInstrumentStatusField>> ReturnInstrumentStatusResponse
    {
      add { AddHandler(CTPResponseType.ReturnInstrumentStatusResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnInstrumentStatusResponse, value); }
    }

    /// <summary>
    /// 交易通知
    /// </summary>
    public event EventHandler<CTPEventArgs<String>> ReturnTradingNoticeResponse
    {
      add { AddHandler(CTPResponseType.ReturnTradingNoticeResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnTradingNoticeResponse, value); }
    }

    /// <summary>
    /// 请求查询签约银行响应
    /// </summary>
    public event EventHandler<CTPEventArgs<List<CThostFtdcContractBankField>>> QueryContractBankResponse
    {
      add { AddHandler(CTPResponseType.QueryContractBankResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryContractBankResponse, value); }
    }

    /// <summary>
    /// 请求查询预埋单响应
    /// </summary>
    public event EventHandler<CTPEventArgs<List<CThostFtdcParkedOrderField>>> QueryParkedOrderResponse
    {
      add { AddHandler(CTPResponseType.QueryParkedOrderResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryParkedOrderResponse, value); }
    }

    /// <summary>
    /// 请求查询预埋撤单响应
    /// </summary>
    public event EventHandler<CTPEventArgs<List<CThostFtdcParkedOrderActionField>>> QueryParkedOrderActionResponse
    {
      add { AddHandler(CTPResponseType.QueryParkedOrderActionResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryParkedOrderActionResponse, value); }
    }

    /// <summary>
    /// 请求查询交易通知响应
    /// </summary>
    public event EventHandler<CTPEventArgs<String>> QueryTradingNoticeResponse
    {
      add { AddHandler(CTPResponseType.QueryTradingNoticeResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryTradingNoticeResponse, value); }
    }

    /// <summary>
    /// 请求查询转帐流水响应
    /// </summary>
    public event EventHandler<CTPEventArgs<List<CThostFtdcTransferSerialField>>> QueryTransferSerialResponse
    {
      add { AddHandler(CTPResponseType.QueryTransferSerialResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryTransferSerialResponse, value); }
    }

    /// <summary>
    /// 请求查询经纪公司交易参数响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcBrokerTradingParamsField>> QueryBrokerTradingParamsResponse
    {
      add { AddHandler(CTPResponseType.QueryBrokerTradingParamsResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryBrokerTradingParamsResponse, value); }
    }

    /// <summary>
    /// 请求查询经纪公司交易算法响应
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcBrokerTradingAlgosField>> QueryBrokerTradingAlgosResponse
    {
      add { AddHandler(CTPResponseType.QueryBrokerTradingAlgosResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryBrokerTradingAlgosResponse, value); }
    }

    /// <summary>
    /// 银行发起银行资金转期货通知
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcRspTransferField>> ReturnFromBankToFutureByBankResponse
    {
      add { AddHandler(CTPResponseType.ReturnFromBankToFutureByBankResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnFromBankToFutureByBankResponse, value); }
    }

    /// <summary>
    /// 银行发起期货资金转银行通知
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcRspTransferField>> ReturnFromFutureToBankByBankResponse
    {
      add { AddHandler(CTPResponseType.ReturnFromFutureToBankByBankResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnFromFutureToBankByBankResponse, value); }
    }

    /// <summary>
    /// 银行发起冲正银行转期货通知
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcRspRepealField>> ReturnRepealFromBankToFutureByBankResponse
    {
      add { AddHandler(CTPResponseType.ReturnRepealFromBankToFutureByBankResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnRepealFromBankToFutureByBankResponse, value); }
    }

    /// <summary>
    /// 银行发起冲正期货转银行通知
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcRspRepealField>> ReturnRepealFromFutureToBankByBankResponse
    {
      add { AddHandler(CTPResponseType.ReturnRepealFromFutureToBankByBankResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnRepealFromFutureToBankByBankResponse, value); }
    }

    /// <summary>
    /// 期货发起银行资金转期货通知
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcRspTransferField>> ReturnFromBankToFutureByFutureResponse
    {
      add { AddHandler(CTPResponseType.ReturnFromBankToFutureByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnFromBankToFutureByFutureResponse, value); }
    }

    /// <summary>
    /// 期货发起期货资金转银行通知
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcRspTransferField>> ReturnFromFutureToBankByFutureResponse
    {
      add { AddHandler(CTPResponseType.ReturnFromFutureToBankByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnFromFutureToBankByFutureResponse, value); }
    }

    /// <summary>
    /// 系统运行时期货端手工发起冲正银行转期货请求
    /// </summary>，银行处理完毕后报盘发回的通知
    public event EventHandler<CTPEventArgs<CThostFtdcRspRepealField>> ReturnRepealFromBankToFutureByFutureManualResponse
    {
      add { AddHandler(CTPResponseType.ReturnRepealFromBankToFutureByFutureManualResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnRepealFromBankToFutureByFutureManualResponse, value); }
    }

    /// <summary>
    /// 系统运行时期货端手工发起冲正期货转银行请求
    /// </summary>，银行处理完毕后报盘发回的通知
    public event EventHandler<CTPEventArgs<CThostFtdcRspRepealField>> ReturnRepealFromFutureToBankByFutureManualResponse
    {
      add { AddHandler(CTPResponseType.ReturnRepealFromFutureToBankByFutureManualResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnRepealFromFutureToBankByFutureManualResponse, value); }
    }

    /// <summary>
    /// 期货发起查询银行余额通知
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcNotifyQueryAccountField>> ReturnQueryBankBalanceByFutureResponse
    {
      add { AddHandler(CTPResponseType.ReturnQueryBankBalanceByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnQueryBankBalanceByFutureResponse, value); }
    }

    /// <summary>
    /// 期货发起银行资金转期货错误回报
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcReqTransferField>> ErrorReturnBankToFutureByFutureResponse
    {
      add { AddHandler(CTPResponseType.ErrorReturnBankToFutureByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.ErrorReturnBankToFutureByFutureResponse, value); }
    }

    /// <summary>
    /// 期货发起期货资金转银行错误回报
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcReqTransferField>> ErrorReturnFutureToBankByFutureResponse
    {
      add { AddHandler(CTPResponseType.ErrorReturnFutureToBankByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.ErrorReturnFutureToBankByFutureResponse, value); }
    }

    /// <summary>
    /// 系统运行时期货端手工发起冲正银行转期货错误回报
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcReqRepealField>> ErrorReturnRepealBankToFutureByFutureManualResponse
    {
      add { AddHandler(CTPResponseType.ErrorReturnRepealBankToFutureByFutureManualResponse, value); }
      remove { RemoveHandler(CTPResponseType.ErrorReturnRepealBankToFutureByFutureManualResponse, value); }
    }

    /// <summary>
    /// 系统运行时期货端手工发起冲正期货转银行错误回报
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcReqRepealField>> ErrorReturnRepealFutureToBankByFutureManualResponse
    {
      add { AddHandler(CTPResponseType.ErrorReturnRepealFutureToBankByFutureManualResponse, value); }
      remove { RemoveHandler(CTPResponseType.ErrorReturnRepealFutureToBankByFutureManualResponse, value); }
    }

    /// <summary>
    /// 期货发起查询银行余额错误回报
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcReqQueryAccountField>> ErrorReturnQueryBankBalanceByFutureResponse
    {
      add { AddHandler(CTPResponseType.ErrorReturnQueryBankBalanceByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.ErrorReturnQueryBankBalanceByFutureResponse, value); }
    }

    /// <summary>
    /// 期货发起冲正银行转期货请求
    /// </summary>，银行处理完毕后报盘发回的通知
    public event EventHandler<CTPEventArgs<CThostFtdcRspRepealField>> ReturnRepealFromBankToFutureByFutureResponse
    {
      add { AddHandler(CTPResponseType.ReturnRepealFromBankToFutureByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnRepealFromBankToFutureByFutureResponse, value); }
    }

    /// <summary>
    /// 期货发起冲正期货转银行请求
    /// </summary>，银行处理完毕后报盘发回的通知
    public event EventHandler<CTPEventArgs<CThostFtdcRspRepealField>> ReturnRepealFromFutureToBankByFutureResponse
    {
      add { AddHandler(CTPResponseType.ReturnRepealFromFutureToBankByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.ReturnRepealFromFutureToBankByFutureResponse, value); }
    }

    /// <summary>
    /// 期货发起银行资金转期货应答
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcReqTransferField>> FromBankToFutureByFutureResponse
    {
      add { AddHandler(CTPResponseType.FromBankToFutureByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.FromBankToFutureByFutureResponse, value); }
    }

    /// <summary>
    /// 期货发起期货资金转银行应答
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcReqTransferField>> FromFutureToBankByFutureResponse
    {
      add { AddHandler(CTPResponseType.FromFutureToBankByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.FromFutureToBankByFutureResponse, value); }
    }

    /// <summary>
    /// 期货发起查询银行余额应答
    /// </summary>
    public event EventHandler<CTPEventArgs<CThostFtdcReqQueryAccountField>> QueryBankAccountMoneyByFutureResponse
    {
      add { AddHandler(CTPResponseType.QueryBankAccountMoneyByFutureResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryBankAccountMoneyByFutureResponse, value); }
    }


    public event EventHandler<CTPEventArgs<CThostFtdcCFMMCTradingAccountKeyField>> QueryCFMMCTradingAccountKeyResponse
    {
      add { AddHandler(CTPResponseType.QueryCFMMCTradingAccountKeyResponse, value); }
      remove { RemoveHandler(CTPResponseType.QueryCFMMCTradingAccountKeyResponse, value); }
    } 



    #endregion

	}
}
