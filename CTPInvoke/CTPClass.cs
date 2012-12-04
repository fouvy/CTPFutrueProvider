using System;
using System.Collections.Generic;
using System.Text;
using CalmBeltFund.Trading;
using System.Text.RegularExpressions;

namespace CalmBeltFund.Trading.CTP
{


  /// <summary>
  /// 请求动作
  /// </summary>
  public enum CTPRequestAction : int
  {
    /// <summary>
    /// 创建交易接口
    /// </summary>
    TraderApiCreate = 1,


    /// <summary>
    /// 删除接口对象本身
    /// </summary>
    TraderApiRelease,

    /// <summary>
    /// 初始化
    /// </summary>
    TraderApiInit,

    /// <summary>
    /// 等待接口线程结束运行
    /// </summary>
    TraderApiJoin,

    /// <summary>
    /// 获取当前交易日
    /// </summary>
    TraderApiGetTradingDay,

    /// <summary>
    /// 注册前置机网络地址
    /// </summary>
    TraderApiRegisterFront,

    /// <summary>
    /// 注册前置机网络地址
    /// </summary>
    TraderApiRegisterNameServer,

    /// <summary>
    /// 注册回调接口
    /// </summary>
    TraderApiRegisterSpi,

    /// <summary>
    /// 订阅私有流。
    /// </summary>
    TraderApiSubscribePrivateTopic,

    /// <summary>
    /// 订阅公共流。
    /// </summary>
    TraderApiSubscribePublicTopic,

    /// <summary>
    /// 客户端认证请求
    /// </summary>
    TraderApiAuthenticate,

    /// <summary>
    /// 用户登录请求
    /// </summary>
    TraderApiUserLoginAction,

    /// <summary>
    /// 登出请求
    /// </summary>
    TraderApiUserLogoutAction,


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
    MarketDataUserLoginAction,

    ///登出请求
    MarketDataUserLogoutAction,

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

    /// <summary>
    /// 用户口令更新请求
    /// </summary>
    UserPasswordUpdateAction = 100,

    /// <summary>
    /// 资金账户口令更新请求
    /// </summary>
    TradingAccountPasswordUpdateAction,

    /// <summary>
    /// 报单录入请求
    /// </summary>
    OrderInsertAction,

    /// <summary>
    /// 预埋单录入请求
    /// </summary>
    ParkedOrderInsertAction,

    /// <summary>
    /// 预埋撤单录入请求
    /// </summary>
    ParkedOrderActionAction,

    /// <summary>
    /// 报单操作请求
    /// </summary>
    OrderActionAction,

    /// <summary>
    /// 查询最大报单数量请求
    /// </summary>
    QueryMaxOrderVolumeAction,

    /// <summary>
    /// 投资者结算结果确认
    /// </summary>
    SettlementInfoConfirmAction,

    /// <summary>
    /// 请求删除预埋单
    /// </summary>
    RemoveParkedOrderAction,

    /// <summary>
    /// 请求删除预埋撤单
    /// </summary>
    RemoveParkedOrderActionAction,

    /// <summary>
    /// 请求查询报单
    /// </summary>
    QueryOrderAction,

    /// <summary>
    /// 请求查询成交
    /// </summary>
    QueryTradeAction,

    /// <summary>
    /// 请求查询投资者持仓
    /// </summary>
    QueryInvestorPositionAction,

    /// <summary>
    /// 请求查询资金账户
    /// </summary>
    QueryTradingAccountAction,

    /// <summary>
    /// 请求查询投资者
    /// </summary>
    QueryInvestorAction,

    /// <summary>
    /// 请求查询交易编码
    /// </summary>
    QueryTradingCodeAction,

    /// <summary>
    /// 请求查询合约保证金率
    /// </summary>
    QueryInstrumentMarginRateAction,

    /// <summary>
    /// 请求查询合约手续费率
    /// </summary>
    QueryInstrumentCommissionRateAction,

    /// <summary>
    /// 请求查询交易所
    /// </summary>
    QueryExchangeAction,

    /// <summary>
    /// 请求查询合约
    /// </summary>
    QueryInstrumentAction,

    /// <summary>
    /// 请求查询行情
    /// </summary>
    QueryDepthMarketDataAction,

    /// <summary>
    /// 请求查询投资者结算结果
    /// </summary>
    QuerySettlementInfoAction,

    /// <summary>
    /// 请求查询转帐银行
    /// </summary>
    QueryTransferBankAction,

    /// <summary>
    /// 请求查询投资者持仓明细
    /// </summary>
    QueryInvestorPositionDetailAction,

    /// <summary>
    /// 请求查询客户通知
    /// </summary>
    QueryNoticeAction,

    /// <summary>
    /// 请求查询结算信息确认
    /// </summary>
    QuerySettlementInfoConfirmAction,

    /// <summary>
    /// 请求查询投资者持仓明细
    /// </summary>
    QueryInvestorPositionCombineDetailAction,

    /// <summary>
    /// 请求查询保证金监管系统经纪公司资金账户密钥
    /// </summary>
    QueryCFMMCTradingAccountKeyAction,

    /// <summary>
    /// 请求查询仓单折抵信息
    /// </summary>
    QueryEWarrantOffsetAction,

    /// <summary>
    /// 请求查询转帐流水
    /// </summary>
    QueryTransferSerialAction,

    /// <summary>
    /// 请求查询银期签约关系
    /// </summary>
    QueryAccountregisterAction,

    /// <summary>
    /// 请求查询签约银行
    /// </summary>
    QueryContractBankAction,

    /// <summary>
    /// 请求查询预埋单
    /// </summary>
    QueryParkedOrderAction,

    /// <summary>
    /// 请求查询预埋撤单
    /// </summary>
    QueryParkedOrderActionAction,

    /// <summary>
    /// 请求查询交易通知
    /// </summary>
    QueryTradingNoticeAction,

    /// <summary>
    /// 请求查询经纪公司交易参数
    /// </summary>
    QueryBrokerTradingParamsAction,

    /// <summary>
    /// 请求查询经纪公司交易算法
    /// </summary>
    QueryBrokerTradingAlgosAction,

    /// <summary>
    /// 期货发起银行资金转期货请求
    /// </summary>
    FromBankToFutureByFutureAction,

    /// <summary>
    /// 期货发起期货资金转银行请求
    /// </summary>
    FromFutureToBankByFutureAction,

    /// <summary>
    /// 期货发起查询银行余额请求
    /// </summary>
    QueryBankAccountMoneyByFutureAction

  }

  /// <summary>
  /// 响应类型
  /// </summary>
  public enum CTPResponseType : int
  {
    /// <summary>
    /// 当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
    /// </summary>
    FrontConnectedResponse = 0,

    /// <summary>
    /// 当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
    /// </summary>
    FrontDisconnectedResponse,

    /// <summary>
    /// 心跳超时警告。当长时间未收到报文时，该方法被调用。
    /// </summary>
    HeartBeatWarningResponse,

    /// <summary>
    /// 客户端认证响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcRspAuthenticateField))]
    AuthenticateResponse,

    /// <summary>
    /// 登录请求响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcRspUserLoginField))]
    UserLoginResponse,

    /// <summary>
    /// 登出请求响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcUserLogoutField))]
    UserLogoutResponse,

    /// <summary>
    /// 订阅行情应答
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcSpecificInstrumentField))]
    SubMarketDataResponse,

    /// <summary>
    /// 取消订阅行情应答
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcSpecificInstrumentField))]
    UnSubMarketDataResponse,

    /// <summary>
    /// 行情响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcDepthMarketDataField))]
    DepthMarketDataResponse,

    /// <summary>
    /// 用户口令更新请求响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcUserPasswordUpdateField))]
    UserPasswordUpdateResponse,

    /// <summary>
    /// 资金账户口令更新请求响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcTradingAccountPasswordUpdateField))]
    TradingAccountPasswordUpdateResponse,

    /// <summary>
    /// 报单录入请求响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInputOrderField))]
    OrderInsertResponse,

    /// <summary>
    /// 预埋单录入请求响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcParkedOrderField))]
    ParkedOrderInsertResponse,

    /// <summary>
    /// 预埋撤单录入请求响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcParkedOrderActionField))]
    ParkedOrderActionResponse,

    /// <summary>
    /// 报单操作请求响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInputOrderActionField))]
    OrderActionResponse,

    /// <summary>
    /// 查询最大报单数量响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcQueryMaxOrderVolumeField))]
    QueryMaxOrderVolumeResponse,

    /// <summary>
    /// 投资者结算结果确认响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcSettlementInfoConfirmField))]
    SettlementInfoConfirmResponse,

    /// <summary>
    /// 删除预埋单响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcRemoveParkedOrderField))]
    RemoveParkedOrderResponse,

    /// <summary>
    /// 删除预埋撤单响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcRemoveParkedOrderActionField))]
    RemoveParkedOrderActionResponse,

    /// <summary>
    /// 请求查询报单响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcOrderField))]
    QueryOrderResponse,

    /// <summary>
    /// 请求查询成交响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcTradeField))]
    QueryTradeResponse,

    /// <summary>
    /// 请求查询投资者持仓响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInvestorPositionField))]
    QueryInvestorPositionResponse,

    /// <summary>
    /// 请求查询资金账户响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcTradingAccountField))]
    QueryTradingAccountResponse,

    /// <summary>
    /// 请求查询投资者响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInvestorField))]
    QueryInvestorResponse,

    /// <summary>
    /// 请求查询交易编码响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcTradingCodeField))]
    QueryTradingCodeResponse,

    /// <summary>
    /// 请求查询合约保证金率响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInstrumentMarginRateField))]
    QueryInstrumentMarginRateResponse,

    /// <summary>
    /// 请求查询合约手续费率响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInstrumentCommissionRateField))]
    QueryInstrumentCommissionRateResponse,

    /// <summary>
    /// 请求查询交易所响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcExchangeField))]
    QueryExchangeResponse,

    /// <summary>
    /// 请求查询合约响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInstrumentField))]
    QueryInstrumentResponse,

    /// <summary>
    /// 请求查询行情响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcDepthMarketDataField))]
    QueryDepthMarketDataResponse,

    /// <summary>
    /// 请求查询投资者结算结果响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcSettlementInfoField))]
    QuerySettlementInfoResponse,

    /// <summary>
    /// 请求查询转帐银行响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcTransferBankField))]
    QueryTransferBankResponse,

    /// <summary>
    /// 请求查询投资者持仓明细响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInvestorPositionDetailField))]
    QueryInvestorPositionDetailResponse,

    /// <summary>
    /// 请求查询客户通知响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcNoticeField))]
    QueryNoticeResponse,

    /// <summary>
    /// 请求查询结算信息确认响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcSettlementInfoConfirmField))]
    QuerySettlementInfoConfirmResponse,

    /// <summary>
    /// 请求查询投资者持仓明细响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInvestorPositionCombineDetailField))]
    QueryInvestorPositionCombineDetailResponse,

    /// <summary>
    /// 查询保证金监管系统经纪公司资金账户密钥响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcCFMMCTradingAccountKeyField))]
    QueryCFMMCTradingAccountKeyResponse,

    /// <summary>
    /// 请求查询仓单折抵信息响应
    /// </summary>
    //[ResponseDataType(typeof(CThostFtdcEWarrantOffsetField))]
    QueryEWarrantOffsetResponse,

    /// <summary>
    /// 请求查询转帐流水响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcTransferSerialField))]
    QueryTransferSerialResponse,

    /// <summary>
    /// 请求查询银期签约关系响应
    /// </summary>
    //[ResponseDataType(typeof(CThostFtdcAccountregisterField))]
    QueryAccountregisterResponse,

    /// <summary>
    /// 错误应答
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcRspInfoField))]
    ErrorResponse,

    /// <summary>
    /// 报单通知
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcOrderField))]
    ReturnOrderResponse,

    /// <summary>
    /// 成交通知
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcTradeField))]
    ReturnTradeResponse,

    /// <summary>
    /// 报单录入错误回报
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInputOrderField))]
    ErrorReturnOrderInsertResponse,

    /// <summary>
    /// 报单操作错误回报
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcOrderActionField))]
    ErrorReturnOrderActionResponse,

    /// <summary>
    /// 合约交易状态通知
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcInstrumentStatusField))]
    ReturnInstrumentStatusResponse,

    /// <summary>
    /// 交易通知
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcTradingNoticeInfoField))]
    ReturnTradingNoticeResponse,

    /// <summary>
    /// 提示条件单校验错误
    /// </summary>
    //[ResponseDataType(typeof(CThostFtdcErrorConditionalOrderField))]
    ReturnErrorConditionalOrderResponse,

    /// <summary>
    /// 请求查询签约银行响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcContractBankField))]
    QueryContractBankResponse,

    /// <summary>
    /// 请求查询预埋单响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcParkedOrderField))]
    QueryParkedOrderResponse,

    /// <summary>
    /// 请求查询预埋撤单响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcParkedOrderActionField))]
    QueryParkedOrderActionResponse,

    /// <summary>
    /// 请求查询交易通知响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcTradingNoticeField))]
    QueryTradingNoticeResponse,

    /// <summary>
    /// 请求查询经纪公司交易参数响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcBrokerTradingParamsField))]
    QueryBrokerTradingParamsResponse,

    /// <summary>
    /// 请求查询经纪公司交易算法响应
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcBrokerTradingAlgosField))]
    QueryBrokerTradingAlgosResponse,

    /// <summary>
    /// 银行发起银行资金转期货通知
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcRspTransferField))]
    ReturnFromBankToFutureByBankResponse,

    /// <summary>
    /// 银行发起期货资金转银行通知
    /// </summary>
    [CTPResponseDataType(typeof(CThostFtdcRspTransferField))]
    ReturnFromFutureToBankByBankResponse,

    /// <summary>
    /// 银行发起冲正银行转期货通知
    /// </summary>
    ReturnRepealFromBankToFutureByBankResponse,

    /// <summary>
    /// 银行发起冲正期货转银行通知
    /// </summary>
    ReturnRepealFromFutureToBankByBankResponse,

    /// <summary>
    /// 期货发起银行资金转期货通知
    /// </summary>
    ReturnFromBankToFutureByFutureResponse,

    /// <summary>
    /// 期货发起期货资金转银行通知
    /// </summary>
    ReturnFromFutureToBankByFutureResponse,

    /// <summary>
    /// 系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
    /// </summary>
    ReturnRepealFromBankToFutureByFutureManualResponse,

    /// <summary>
    /// 系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
    /// </summary>
    ReturnRepealFromFutureToBankByFutureManualResponse,

    /// <summary>
    /// 期货发起查询银行余额通知
    /// </summary>
    ReturnQueryBankBalanceByFutureResponse,

    /// <summary>
    /// 期货发起银行资金转期货错误回报
    /// </summary>
    ErrorReturnBankToFutureByFutureResponse,

    /// <summary>
    /// 期货发起期货资金转银行错误回报
    /// </summary>
    ErrorReturnFutureToBankByFutureResponse,

    /// <summary>
    /// 系统运行时期货端手工发起冲正银行转期货错误回报
    /// </summary>
    ErrorReturnRepealBankToFutureByFutureManualResponse,

    /// <summary>
    /// 系统运行时期货端手工发起冲正期货转银行错误回报
    /// </summary>
    ErrorReturnRepealFutureToBankByFutureManualResponse,

    /// <summary>
    /// 期货发起查询银行余额错误回报
    /// </summary>
    ErrorReturnQueryBankBalanceByFutureResponse,

    /// <summary>
    /// 期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
    /// </summary>
    ReturnRepealFromBankToFutureByFutureResponse,

    /// <summary>
    /// 期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
    /// </summary>
    ReturnRepealFromFutureToBankByFutureResponse,

    /// <summary>
    /// 期货发起银行资金转期货应答
    /// </summary>
    FromBankToFutureByFutureResponse,

    /// <summary>
    /// 期货发起期货资金转银行应答
    /// </summary>
    FromFutureToBankByFutureResponse,

    /// <summary>
    /// 期货发起查询银行余额应答
    /// </summary>
    QueryBankAccountMoneyByFutureResponse,

    /// <summary>
    /// 银行发起银期开户通知
    /// </summary>
    OpenAccountByBankResponse,

    /// <summary>
    /// 银行发起银期销户通知
    /// </summary>
    CancelAccountByBankResponse,

    /// <summary>
    /// 银行发起变更银行账号通知
    /// </summary>
    ChangeAccountByBankResponse

  }


  public class CTPSettlementInfo
  {
    public string TradingDate;
    public string Context = "";
    public List<CTPClosePositionDetail> ClosePositionDetails = new List<CTPClosePositionDetail>();

    public void ReadContext()
    {
      string[] lines = Context.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);


      int beginLine = 0;
      int endLine = 0;

      for (int i = 0; i < lines.Length; i++)
      {
        string line = lines[i];

        if (line.Trim() == "平仓明细")
        {
          beginLine = i;
        }
        else if (line.Trim() == "持仓明细")
        {
          endLine = i - 1;

          ReadClosePositionInfo(lines, beginLine, endLine);

        }

      }

    }

    private void ReadClosePositionInfo(string[] lines, int beginLine, int endLine)
    {
      for (int i = beginLine; i < endLine; i++)
      {
        string line = lines[i];

        if (line.StartsWith("|") && line.Length > 9 && char.IsDigit(line, 2))
        {
          string[] items = line.Split('|');

          CTPClosePositionDetail cpd = new CTPClosePositionDetail();

          cpd.CloseDate = items[1].Trim();
          cpd.InstrumentID = items[3].Trim();
          cpd.OpenDate = items[4].Trim();
          cpd.Direction = items[5].Trim() == "买" ? CTPDirectionType.Buy : CTPDirectionType.Sell;

          cpd.Volume = int.Parse(items[6].Trim());
          cpd.OpenPrice = double.Parse(items[7].Trim());
          cpd.ClosePrice = double.Parse(items[9].Trim());

          this.ClosePositionDetails.Add(cpd);


        }
      }
    }
  }

  public class CTPClosePositionDetail
  {
    private string instrumentID;

    private int volume;

    private CTPDirectionType direction;
    private string openDate;
    private double openPrice;
    private string closeDate;
    private double closePrice;


    /// <summary>
    /// 合约
    /// </summary>
    public string InstrumentID
    {
      get { return instrumentID; }
      set { instrumentID = value; }
    }

    /// <summary>
    /// 平仓日
    /// </summary>
    public string CloseDate
    {
      get { return closeDate; }
      set { closeDate = value; }
    }

    /// <summary>
    /// 开仓日
    /// </summary>
    public string OpenDate
    {
      get { return openDate; }
      set { openDate = value; }
    }

    /// <summary>
    /// 方向
    /// </summary>
    public CTPDirectionType Direction
    {
      get { return direction; }
      set { direction = value; }
    }

    /// <summary>
    /// 量
    /// </summary>
    public int Volume
    {
      get { return volume; }
      set { volume = value; }
    }

    /// <summary>
    /// 开仓价
    /// </summary>
    public double OpenPrice
    {
      get { return openPrice; }
      set { openPrice = value; }
    }

    /// <summary>
    /// 平仓价
    /// </summary>
    public double ClosePrice
    {
      get { return closePrice; }
      set { closePrice = value; }
    }

    public override string ToString()
    {

      return string.Format("{0},{1},{2},{3},{4},{5},{6}",
        InstrumentID,
        OpenDate,
        CloseDate,
        Direction,
        Volume,
        OpenPrice,
        ClosePrice);
    }
  }

  public class CTPExchange
  {
    List<CTPInstrument> instruments = new List<CTPInstrument>();

    CThostFtdcExchangeField nativeValue;

    public void SetNativeValue(CThostFtdcExchangeField nativeValue)
    {
      this.nativeValue = nativeValue;

      this.ExchangeName = nativeValue.ExchangeName;
      this.ExchangeID = nativeValue.ExchangeID;
    }

    public string ExchangeName { get; set; }

    public string ExchangeID { get; set; }

    public CTPInstrumentStatusType TradingStatus { get; set; }

    public TimeSpan CurrentTime { get; set; }

    /// <summary>
    /// 合约列表
    /// </summary>
    public List<CTPInstrument> Instruments
    {
      get { return instruments; }
      set { instruments = value; }
    }

    public override bool Equals(object obj)
    {
      if (obj is CTPExchange && obj != null)
      {
        return this.nativeValue.ExchangeID == ((CTPExchange)obj).nativeValue.ExchangeID;
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return this.nativeValue.ExchangeID.GetHashCode();
    }

  }


  /// <summary>
  /// 合约
  /// </summary>
  public class CTPInstrument
  {
    CThostFtdcInstrumentField nativeValue;
    CThostFtdcInstrumentMarginRateField instrumentMarginRate;
    CThostFtdcInstrumentCommissionRateField instrumentCommissionRate;

    bool isRefreshMarginRate = false;
    bool isRefreshCommissionRate = false;

    /// <summary>
    /// 是否查询更新过保证金率
    /// </summary>
    public bool IsRefreshMarginRate
    {
      get { return isRefreshMarginRate; }
      set { isRefreshMarginRate = value; }
    }

    /// <summary>
    /// 是否查询更新过手续费
    /// </summary>
    public bool IsRefreshCommissionRate
    {
      get { return isRefreshCommissionRate; }
      set { isRefreshCommissionRate = value; }
    }

    public void SetNativeValue(CThostFtdcInstrumentField nativeValue)
    {
      this.nativeValue = nativeValue;
    }
    public void SetNativeValue(CThostFtdcInstrumentMarginRateField instrumentMarginRate)
    {
      this.instrumentMarginRate = instrumentMarginRate;
      this.isRefreshMarginRate = true;
    }
    public void SetNativeValue(CThostFtdcInstrumentCommissionRateField instrumentCommissionRate)
    {
      this.instrumentCommissionRate = instrumentCommissionRate;
      this.isRefreshCommissionRate = true;
    }

    /// <summary>
    /// 合约名
    /// </summary>
    public string Name
    {
      get { return nativeValue.InstrumentName; }
    }

    public string ID
    {
      get { return nativeValue.InstrumentID; }
    }

    public string ExchangeID
    {
      get { return nativeValue.ExchangeID; }
    }

    public string ProductID
    {
      get { return nativeValue.ProductID; }
    }

    public string DeliveryYear
    {
      get { return nativeValue.DeliveryYear.ToString(); }
    }

    public string DeliveryMonth
    {
      get { return nativeValue.DeliveryMonth.ToString("D2"); }
    }

    public string ExpireDate
    {
      get { return nativeValue.ExpireDate; }
    }


    public string ExchangeInstID
    {
      get { return nativeValue.ExchangeInstID; }
    }

    /// <summary>
    /// 上市日
    /// </summary>
    public string OpenDate
    {
      get { return nativeValue.OpenDate; }
    }

    /// <summary>
    /// 产品类型
    /// </summary>
    public CTPProductClassType ProductClass
    {
      get { return nativeValue.ProductClass; }
    }

    public double PriceTick
    {
      get { return nativeValue.PriceTick; }
    }

    public int VolumeMultiple
    {
      get { return nativeValue.VolumeMultiple; }
    }


    public bool IsTrading
    {
      get { return nativeValue.IsTrading; }
    }

    public double LongMarginRatio
    {
      get { return nativeValue.LongMarginRatio; }
    }

    public double ShortMarginRatio
    {
      get { return nativeValue.ShortMarginRatio; }
    }


    /// <summary>
    /// 多头保证金（成交额百分比）
    /// </summary>
    public double LongMarginRatioByMoney
    {
      get { return instrumentMarginRate.LongMarginRatioByMoney; }
    }
    /// <summary>
    /// 多头保证金
    /// </summary>
    public double LongMarginRatioByVolume
    {
      get { return instrumentMarginRate.LongMarginRatioByVolume; }
    }
    /// <summary>
    /// 空头保证金（成交额百分比）
    /// </summary>
    public double ShortMarginRatioByMoney
    {
      get { return instrumentMarginRate.ShortMarginRatioByMoney; }
    }
    /// <summary>
    /// 空头保证金
    /// </summary>
    public double ShortMarginRatioByVolume
    {
      get { return instrumentMarginRate.ShortMarginRatioByVolume; }
    }

    /// <summary>
    /// 开仓手续费率
    /// </summary>
    public double OpenRatioByMoney
    {
      get { return instrumentCommissionRate.OpenRatioByMoney; }
    }
    /// <summary>
    /// 开仓手续费
    /// </summary>
    public double OpenRatioByVolume
    {
      get { return instrumentCommissionRate.OpenRatioByVolume; }
    }
    /// <summary>
    /// 平仓手续费率
    /// </summary>
    public double CloseRatioByMoney
    {
      get { return instrumentCommissionRate.CloseRatioByMoney; }
    }
    /// <summary>
    /// 平仓手续费
    /// </summary>
    public double CloseRatioByVolume
    {
      get { return instrumentCommissionRate.CloseRatioByVolume; }
    }
    /// <summary>
    /// 平今手续费率
    /// </summary>
    public double CloseTodayRatioByMoney
    {
      get { return instrumentCommissionRate.CloseTodayRatioByMoney; }
    }
    /// <summary>
    /// 平今手续费
    /// </summary>
    public double CloseTodayRatioByVolume
    {
      get { return instrumentCommissionRate.CloseTodayRatioByVolume; }
    }

    public double CalcMargin(double price, bool longMargin)
    {

      double margin = double.MaxValue;

      //保证金
      if (this.IsRefreshMarginRate)
      {


        double r1 = this.LongMarginRatio + this.ShortMarginRatio;
        double r2 = this.LongMarginRatioByMoney + this.ShortMarginRatioByMoney;
        double r3 = this.LongMarginRatioByVolume + this.ShortMarginRatioByVolume;

        if (r3 != 0)
        {
          //存在按量保证金费时：
          if (this.LongMarginRatio == this.ShortMarginRatio &&
            this.LongMarginRatioByVolume == this.ShortMarginRatioByVolume)
          {

            margin = price * this.nativeValue.VolumeMultiple * this.LongMarginRatioByMoney + this.LongMarginRatioByVolume;
          }
          else
          {

            if (longMargin)
            {
              margin = price * this.nativeValue.VolumeMultiple * this.LongMarginRatioByMoney + this.LongMarginRatioByVolume;
            }
            else
            {
              margin = price * this.nativeValue.VolumeMultiple * this.ShortMarginRatioByMoney + this.ShortMarginRatioByVolume;
            }
          }
        }
        else
        {
          if (this.LongMarginRatio == this.ShortMarginRatio)
          {
            margin = price * this.nativeValue.VolumeMultiple * this.LongMarginRatioByMoney;
          }
          else
          {
             if (longMargin)
            {
              margin = price * this.nativeValue.VolumeMultiple * this.LongMarginRatioByMoney;
            }
            else
            {
              margin = price * this.nativeValue.VolumeMultiple * this.ShortMarginRatioByMoney;
            }
          }
        }
      }

       return margin;
    }

    string FormatCommissionRate(double value)
    {
      value = value * 10000;

      return string.Concat(value, "%%");
    }

    public string GetCommissionString()
    {
      string s = "";
      //手续费
      if (this.IsRefreshCommissionRate)
      {
        double r1 = this.OpenRatioByMoney + this.CloseRatioByMoney + this.CloseTodayRatioByMoney;
        double r2 = this.OpenRatioByVolume + this.CloseRatioByVolume + this.CloseTodayRatioByVolume;


        if (r1 != 0)
        {

          //按百分比收取手续费
          if (this.OpenRatioByMoney == this.CloseRatioByMoney)
          {
            s = FormatCommissionRate(this.OpenRatioByMoney);
          }
          else
          {
            s = string.Format("开|平：{0}|{1}", FormatCommissionRate(this.OpenRatioByMoney), FormatCommissionRate(this.CloseRatioByMoney));
          }

          //平今
          if (this.CloseTodayRatioByMoney == 0)
          {
            s = s + "(平今免)";
          }
          else
          {
            if (this.CloseRatioByMoney != this.CloseTodayRatioByMoney)
            {
              s = s + string.Format("(平今{0})", FormatCommissionRate(this.CloseTodayRatioByMoney));
            }
          }
        }


        if (r1 != 0 && r2 != 0)
        {
          s = s + " + ";
        }

        if(r2!=0)
        {

          //按手数收取手续费

          if (this.OpenRatioByVolume == this.CloseRatioByVolume)
          {
            s += string.Format("{0}元/手", this.OpenRatioByVolume);

          }
          else
          {
            s += string.Format("开|平：{0}|{1}元/手", this.OpenRatioByVolume, this.CloseRatioByVolume);
          }

          //平今
          if (this.CloseTodayRatioByVolume == 0)
          {
            s = s + "(平今免)";
          }
          else
          {
            if (this.CloseTodayRatioByVolume != this.CloseRatioByVolume)
            {
              s = s + string.Format("(平今{0}元/手)", this.CloseTodayRatioByVolume);
            }
          }
        }


      }
      return s;
    }


    public string GetMarginRateString()
    {
      string s = "";

      //保证金
      if (this.IsRefreshMarginRate)
      {


        double r1 = this.LongMarginRatio + this.ShortMarginRatio;
        double r2 = this.LongMarginRatioByMoney + this.ShortMarginRatioByMoney;
        double r3 = this.LongMarginRatioByVolume + this.ShortMarginRatioByVolume;

        if (r3 != 0)
        {
          //存在按量保证金费时：
          if (this.LongMarginRatio == this.ShortMarginRatio &&
            this.LongMarginRatioByVolume == this.ShortMarginRatioByVolume)
          {
            s = string.Format("{0:P}+{1}", this.LongMarginRatioByMoney, this.LongMarginRatioByVolume);
          }
          else
          {
            s = string.Format("多|空：{0:P}+{1}|{2:P}+{3}",
              this.LongMarginRatioByMoney, this.LongMarginRatioByVolume,
              this.ShortMarginRatioByMoney, this.ShortMarginRatioByVolume);
          }
        }
        else
        {
          if (this.LongMarginRatio == this.ShortMarginRatio)
          {
            s = string.Format("{0:P}", this.LongMarginRatioByMoney);
          }
          else
          {
            s = string.Format("多|空：{0:P}|{1:P}", this.LongMarginRatioByMoney, this.ShortMarginRatioByMoney);
          }
        }
      }
      return s;

    }
  }


  public class CTPTask : CTPTaskBase<CTPRequestAction>
  {
    
  }

  public class CTPEnumFormater
  {
    public static string OffsetFlagString(CTPOffsetFlagType flag)
    {
      switch (flag)
      {
        case CTPOffsetFlagType.Open:
          return "开仓";
        case CTPOffsetFlagType.Close:
          return "平仓";
        case CTPOffsetFlagType.ForceClose:
          return "强平";
        case CTPOffsetFlagType.CloseToday:
          return "平今";
        case CTPOffsetFlagType.CloseYesterday:
          return "平昨";
        default:
          return "";
      }
    }

    public static string DirectionTypeString(CTPDirectionType tyep)
    {
      return tyep == CTPDirectionType.Buy ? "买入" : "卖出";
    }

    public static string HedgeFlagTypeString(CTPHedgeFlagType tyep)
    {
      return tyep == CTPHedgeFlagType.Speculation ? "投机" : "保值";
    }

    public static string PositionDirectionTypeString(CTPPositionDirectionType type)
    {
      switch (type)
      {
        case CTPPositionDirectionType.Net:
          return "净仓";
        case CTPPositionDirectionType.Long:
          return "多仓";
        case CTPPositionDirectionType.Short:
          return "空仓";
        default:
          return "";
      }
    }

    /// <summary>
    /// 持仓日期类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string PositionDateTypeString(CTPPositionDateType type)
    {
      return type == CTPPositionDateType.Today ? "当日仓" : "历史仓";
    }

    public static string TradeTypeString(CTPTradeType type)
    {
      switch (type)
      {
        case CTPTradeType.Common:
          return "普通成交";
        case CTPTradeType.OptionsExecution:
          return "期权执行";
        case CTPTradeType.OTC:
          return "OTC成交";
        case CTPTradeType.EFPDerived:
          return "期转现衍生成交";
        case CTPTradeType.CombinationDerived:
          return "组合衍生成交";
        default:
          return "";
      }
    }


    public static string TimeConditionTypeString(CTPTimeConditionType type)
    {
      string stimeCondition = "";
      switch (type)
      {
        case CTPTimeConditionType.IOC:
          stimeCondition = "IOC-不成即撤";
          break;
        case CTPTimeConditionType.GFS:
          stimeCondition = "GFS-本节有效";
          break;
        case CTPTimeConditionType.GFD:
          stimeCondition = "GFD-当日有效";
          break;
        case CTPTimeConditionType.GTD:
          stimeCondition = "GTD-指定日前有效";
          break;
        case CTPTimeConditionType.GTC:
          stimeCondition = "GTC-撤销前有效";
          break;
        case CTPTimeConditionType.GFA:
          stimeCondition = "GFA-集合竞价有效";
          break;
      }

      return stimeCondition;
    }


    public static string InstrumentStatusTypeString(CTPInstrumentStatusType type)
    {
      switch (type)
      {
        case CTPInstrumentStatusType.BeforeTrading:
          return "开盘前";
        case CTPInstrumentStatusType.NoTrading:
          return "非交易";
        case CTPInstrumentStatusType.Continous:
          return "连续交易";
        case CTPInstrumentStatusType.AuctionOrdering:
          return "集合竞价报单";
        case CTPInstrumentStatusType.AuctionBalance:
          return "集合竞价价格平衡";
        case CTPInstrumentStatusType.AuctionMatch:
           return "集合竞价撮合";
        case CTPInstrumentStatusType.Closed:
          return "收盘";
        default:
          return "";
      }
    }

    public static string ContingentConditionTypeString(CTPContingentConditionType type)
    {

      switch (type)
      {
        case CTPContingentConditionType.Immediately:
          break;
        case CTPContingentConditionType.Touch:
          break;
        case CTPContingentConditionType.TouchProfit:
          break;
        case CTPContingentConditionType.ParkedOrder:
          break;
        
        default:
          break;
      }

      switch (type)
      {
        case CTPContingentConditionType.Immediately:
          return "立即";
        case CTPContingentConditionType.Touch:
          return "止损";
        case CTPContingentConditionType.TouchProfit:
          return "止赢";
        case CTPContingentConditionType.ParkedOrder:
          return "预埋单";
        case CTPContingentConditionType.LastPriceGreaterThanStopPrice:
          return "最新价>条件价";
        case CTPContingentConditionType.LastPriceGreaterEqualStopPrice:
          return "最新价>=条件价";
        case CTPContingentConditionType.LastPriceLesserThanStopPrice:
          return "最新价<条件价";
        case CTPContingentConditionType.LastPriceLesserEqualStopPrice:
          return "最新价<=条件价";

        case CTPContingentConditionType.AskPriceGreaterThanStopPrice:
          return "卖1价>条件价";
        case CTPContingentConditionType.AskPriceGreaterEqualStopPrice:
          return "卖1价>=条件价";
        case CTPContingentConditionType.AskPriceLesserThanStopPrice:
          return "卖1价<条件价";
        case CTPContingentConditionType.AskPriceLesserEqualStopPrice:
          return "卖1价<=条件价";

        case CTPContingentConditionType.BidPriceGreaterThanStopPrice:
          return "买1价>条件价";
        case CTPContingentConditionType.BidPriceGreaterEqualStopPrice:
          return "买1价>=条件价";
        case CTPContingentConditionType.BidPriceLesserThanStopPrice:
          return "买1价<条件价";
        case CTPContingentConditionType.BidPriceLesserEqualStopPrice:
          return "买1价<=条件价";
        
        default:
          return "";
      }


      
  
    /////
    //Touch = (byte)'2',
    /////
    //TouchProfit = (byte)'3',
    /////
    //ParkedOrder = (byte)'4',
    //StopPriceGreaterThanLastPrice = (byte)'5',
    /////
    //StopPriceGreaterEqualLastPrice = (byte)'6',
    /////
    //StopPriceLesserThanLastPrice = (byte)'7',
    /////
    //StopPriceLesserEqualLastPrice = (byte)'8',
    /////
    //StopPriceGreaterThanAskPrice = (byte)'9',
    /////
    //StopPriceGreaterEqualAskPrice = (byte)'A',
    /////
    //StopPriceLesserThanAskPrice = (byte)'B',
    /////
    //StopPriceLesserEqualAskPrice = (byte)'C',
    /////
    //StopPriceGreaterThanBidPrice = (byte)'D',
    /////
    //StopPriceGreaterEqualBidPrice = (byte)'E',
    /////
    //StopPriceLesserThanBidPrice = (byte)'F',
    /////
    //StopPriceLesserEqualBidPrice = (byte)'a'
  
    }

    public static string ContingentConditionTypeString(CTPContingentConditionType type,double price)
    {

      switch (type)
      {
        case CTPContingentConditionType.Immediately:
          return "立即";
        case CTPContingentConditionType.Touch:
          return "止损";
        case CTPContingentConditionType.TouchProfit:
          return "止赢";
        case CTPContingentConditionType.ParkedOrder:
          return "预埋单";
        case CTPContingentConditionType.LastPriceGreaterThanStopPrice:
          return string.Format("最新价>{0}", price);
        case CTPContingentConditionType.LastPriceGreaterEqualStopPrice:
          return string.Format("最新价>={0}", price);
        case CTPContingentConditionType.LastPriceLesserThanStopPrice:
          return string.Format("最新价<{0}", price);
        case CTPContingentConditionType.LastPriceLesserEqualStopPrice:
          return string.Format("最新价<={0}", price);

        case CTPContingentConditionType.AskPriceGreaterThanStopPrice:
          return string.Format("卖价>{0}", price);
        case CTPContingentConditionType.AskPriceGreaterEqualStopPrice:
          return string.Format("卖价>={0}", price);
        case CTPContingentConditionType.AskPriceLesserThanStopPrice:
          return string.Format("卖价<{0}", price);
        case CTPContingentConditionType.AskPriceLesserEqualStopPrice:
          return string.Format("卖价<={0}", price);

        case CTPContingentConditionType.BidPriceGreaterThanStopPrice:
          return string.Format("买价>{0}", price);
        case CTPContingentConditionType.BidPriceGreaterEqualStopPrice:
          return string.Format("买价>={0}", price);
        case CTPContingentConditionType.BidPriceLesserThanStopPrice:
          return string.Format("买价<{0}", price);
        case CTPContingentConditionType.BidPriceLesserEqualStopPrice:
          return string.Format("买价<={0}", price);
        default:
          return "";
      }
    }


    public static string OrderTypeString(CTPOrderType type)
    {
      switch (type)
      {
        case CTPOrderType.Normal:
          return "正常";
        case CTPOrderType.DeriveFromQuote:
          return "报价衍生";
        case CTPOrderType.DeriveFromCombination:
          return "组合衍生";
        case CTPOrderType.Combination:
          return "组合报单";
        case CTPOrderType.ConditionalOrder:
          return "条件单";
        case CTPOrderType.Swap:
          return "互换单";
        default:
          return "";
      }
    }

    public static string OrderPriceTypeString(CTPOrderPriceType type, double price)
    {
      switch (type)
      {
        case CTPOrderPriceType.AnyPrice:
          return "任意价";
        case CTPOrderPriceType.LimitPrice:
          return price.ToString();
        case CTPOrderPriceType.BestPrice:
          return "最优价";
        case CTPOrderPriceType.LastPrice:
          return "最新价";
        case CTPOrderPriceType.LastPricePlusOneTicks:
          return "最新价(+1跳)";
        case CTPOrderPriceType.LastPricePlusTwoTicks:
          return "最新价(+2跳)";
        case CTPOrderPriceType.LastPricePlusThreeTicks:
          return "最新价(+3跳)";
        case CTPOrderPriceType.AskPrice1:
          return "卖价";
        case CTPOrderPriceType.AskPrice1PlusOneTicks:
          return "卖价(+1跳)";
        case CTPOrderPriceType.AskPrice1PlusTwoTicks:
          return "卖价(+2跳)";
        case CTPOrderPriceType.AskPrice1PlusThreeTicks:
          return "卖价(+3跳)";
        case CTPOrderPriceType.BidPrice1:
          return "买价";
        case CTPOrderPriceType.BidPrice1PlusOneTicks:
          return "买价(+1跳)";
        case CTPOrderPriceType.BidPrice1PlusTwoTicks:
          return "买价(+2跳)";
        case CTPOrderPriceType.BidPrice1PlusThreeTicks:
          return "买价(+3跳)";
        default:
          return "";
      }
    }


    public static string ProductClassTypeString(CTPProductClassType type)
    {
      switch (type)
      {
        case CTPProductClassType.Futures:
          return "期货";
        case CTPProductClassType.Options:
          return "期权";
        case CTPProductClassType.Combination:
          return "组合";
        case CTPProductClassType.Spot:
          return "即期";
        case CTPProductClassType.EFP:
          return "期转现";
        default:
          return "";
      }
    }

    public static string ParkedOrderStatus(CTPParkedOrderStatusType type)
    {
      switch (type)
      {
        case CTPParkedOrderStatusType.NotSend:
          return "未发送";
        case CTPParkedOrderStatusType.Send:
          return "已发送";
        case CTPParkedOrderStatusType.Deleted:
          return "已删除";
        default:
          return "";
      }
    }
   
  }
}
