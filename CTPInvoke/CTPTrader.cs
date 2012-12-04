using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Collections;

namespace CalmBeltFund.Trading.CTP
{
  public partial class CTPTrader : CTPFutureClient
  {
    private string userProductInfo = "CalmBelt";

    //报单引用为NUMBER(12)，需为单调递增的数字
    //内部扩展其格式为：nnnnn.MMM.TT.ii
    //    nnnnn ： Seq，递增序列（按每秒钟一单计算：4H × 60min × 60sec = 14400，5位数字已够用）
    //    MMM   ： ModelID，交易模块识别序号
    //    TT    ： OrderType，区分订单类型
    //    ii    ： Index，组合报单中单腿合约的序号
    const int BaseOrderRef = 1000000;
    int orderRef = 0;
    
    CTPInstrumentStatusType tradingStatus;

    bool isSettlementConfim = false;
    bool isInitialized = false;
    bool isSimulationServer = false;


    string loginTime = "";

    DateTime tradingDate = DateTime.MinValue;

    TimeSpan loginTimeDCE = TimeSpan.Zero;
    TimeSpan loginTimeCZCE = TimeSpan.Zero;
    TimeSpan loginTimeSHFE = TimeSpan.Zero;
    TimeSpan loginTimeCFFEX = TimeSpan.Zero;
    TimeSpan currentTime = TimeSpan.Zero;

    TimeSpan second = new TimeSpan(TimeSpan.TicksPerSecond);
    Stopwatch wallTimeStopwatch = new Stopwatch();
    Timer timer = null;


    /// <summary>
    /// 查询类请求时间间隔：1秒钟
    /// </summary>
    //public const int QueryTaskProcessPeriod = 500;
    
    //Timer queryTaskTimer = null;

    //Queue<CTPTask> queryTasks = new Queue<CTPTask>();
    //Dictionary<int, CTPTask> processedTasks = new Dictionary<int, CTPTask>();


    CThostFtdcInvestorField investor;
    CThostFtdcTradingAccountField tradingAccount;
    CThostFtdcCFMMCTradingAccountKeyField cfmmcTradingAccountKey;

    //成交列表
    List<CThostFtdcTradeField> tradeList = new List<CThostFtdcTradeField>();
    //委托单列表
    List<CThostFtdcOrderField> orderList = new List<CThostFtdcOrderField>();

    List<CThostFtdcParkedOrderField> parkedOrderList = new List<CThostFtdcParkedOrderField>();
    List<CThostFtdcParkedOrderActionField> parkedOrderActionList = new List<CThostFtdcParkedOrderActionField>();

    //持仓列表
    List<CThostFtdcInvestorPositionField> positionList = new List<CThostFtdcInvestorPositionField>();
    //持仓明细列表
    List<CThostFtdcInvestorPositionDetailField> positionDetailList = new List<CThostFtdcInvestorPositionDetailField>();
    //组合持仓明细列表
    List<CThostFtdcInvestorPositionCombineDetailField> investorPositionCombineDetailList = new List<CThostFtdcInvestorPositionCombineDetailField>();

    List<CThostFtdcTransferSerialField> transferSerialList = new List<CThostFtdcTransferSerialField>();


    Dictionary<string, CTPSettlementInfo> settlementInfoDictionary = new Dictionary<string, CTPSettlementInfo>();
    Dictionary<string, CTPExchange> exchanges = new Dictionary<string, CTPExchange>();
    //Dictionary<string, CTPSymbolProduct> symbolProducts = new Dictionary<string, CTPSymbolProduct>();
    Dictionary<string, CTPInstrument> instrumentDictionary = new Dictionary<string, CTPInstrument>();

    Dictionary<string, CTPInstrumentStatusType> exchangeStatus = new Dictionary<string, CTPInstrumentStatusType>();

    public CThostFtdcCFMMCTradingAccountKeyField CfmmcTradingAccountKey
    {
      get { return cfmmcTradingAccountKey; }
    }


    /// <summary>
    /// 查询队列中是否存在任务
    /// </summary>
    public bool HasQueryTask
    {
      get { return this.queryTasks.Count > 0; }
    }

    /// <summary>
    /// 是否是模拟服务器
    /// </summary>
    public bool IsSimulationServer
    {
      get { return isSimulationServer; }
    }

    /// <summary>
    /// 终端标识
    /// </summary>
    public string UserProductInfo
    {
      get { return userProductInfo; }
      set { userProductInfo = value; }
    }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserKey
    {
      get { return string.Format("{0}@{1}@CTP", InvestorID, BrokerID); }
    }
    
    public int CurrentOrderRef
    {
      get { return orderRef; }
    }

    /// <summary>
    /// 交易日
    /// </summary>
    public DateTime TradingDate
    {
      get { return tradingDate; }
    }


    /// <summary>
    /// 当前时间
    /// </summary>
    public TimeSpan CurrentTime
    {
      get { return currentTime; }
    }

    public CTPInstrumentStatusType TradingStatus
    {
      get { return tradingStatus; }
    }

    public Dictionary<string, CTPInstrumentStatusType> ExchangeStatus
    {
      get { return exchangeStatus; }
      set { exchangeStatus = value; }
    }


    //public Dictionary<string, CTPSymbolProduct> SymbolProducts
    //{
    //  get { return symbolProducts; }
    //  set { symbolProducts = value; }
    //}
    

    public Dictionary<string, CTPInstrument> InstrumentDictionary
    {
      get { return instrumentDictionary; }
      set { instrumentDictionary = value; }
    }

    /// <summary>
    /// 投资者
    /// </summary>
    public CThostFtdcInvestorField Investor
    {
      get { return investor; }
    }

    public Dictionary<string, CTPExchange> Exchanges
    {
      get { return exchanges; }
    }

    public CThostFtdcTradingAccountField TradingAccount
    {
      get { return tradingAccount; }
      private set { tradingAccount = value; }
    }



    public List<CThostFtdcOrderField> OrderList
    {
      get { return orderList; }
    }

    public List<CThostFtdcParkedOrderField> ParkedOrderList
    {
      get { return parkedOrderList; }
    }

    public List<CThostFtdcParkedOrderActionField> ParkedOrderActionList
    {
      get { return parkedOrderActionList; }
    }

    public List<CThostFtdcTradeField> TradeList
    {
      get { return tradeList; }
    }
    public List<CThostFtdcInvestorPositionField> PositionList
    {
      get { return positionList; }
    }

    public List<CThostFtdcInvestorPositionDetailField> PositionDetailList
    {
      get { return positionDetailList; }
    }

    /// <summary>
    /// 转账明细
    /// </summary>
    public List<CThostFtdcTransferSerialField> TransferSerialList
    {
      get { return transferSerialList; }
    }


    public CTPTrader()
    {
      timer = new Timer(new TimerCallback(this.UpdateTime));

      this.exchanges.Add("SHFE", new CTPExchange() { ExchangeID = "SHFE", ExchangeName = "上海期货交易所" });
      this.exchanges.Add("DCE", new CTPExchange() { ExchangeID = "DCE", ExchangeName = "大连商品交易所" });
      this.exchanges.Add("CZCE", new CTPExchange() { ExchangeID = "CZCE", ExchangeName = "郑州商品交易所" });
      this.exchanges.Add("CFFEX", new CTPExchange() { ExchangeID = "CFFEX", ExchangeName = "中国金融交易所" });

    }

    /// <summary>
    /// 交易所时间
    /// </summary>
    /// <param name="obj"></param>
    void UpdateTime(object obj)
    {

      exchanges["DCE"].CurrentTime = this.loginTimeDCE.Add(wallTimeStopwatch.Elapsed);
      exchanges["CZCE"].CurrentTime = this.loginTimeCZCE.Add(wallTimeStopwatch.Elapsed);
      exchanges["SHFE"].CurrentTime = this.loginTimeSHFE.Add(wallTimeStopwatch.Elapsed);
      exchanges["CFFEX"].CurrentTime = this.loginTimeCFFEX.Add(wallTimeStopwatch.Elapsed);

      this.currentTime = exchanges["SHFE"].CurrentTime;
    }

    public int IncrementOrderRef()
    {
      return Interlocked.Increment(ref this.orderRef);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instrumentID"></param>
    /// <returns></returns>
    public CTPInstrument GetInstrument(string instrumentID)
    {

      if (string.IsNullOrEmpty(instrumentID))
      {
        return null;
      }

      if (instrumentDictionary.ContainsKey(instrumentID))
      {
        return instrumentDictionary[instrumentID];
      }
      else if (instrumentDictionary.ContainsKey(instrumentID.ToUpper()))
      {
        return instrumentDictionary[instrumentID.ToUpper()];
      }
      else if (instrumentDictionary.ContainsKey(instrumentID.ToLower()))
      {
        return instrumentDictionary[instrumentID.ToLower()];
      }
      else
      {
        return null;
      }
    }

    void SetInstrumentRate(CThostFtdcInstrumentCommissionRateField instrumentCommissionRate)
    {

      if (string.IsNullOrEmpty(instrumentCommissionRate.InstrumentID))
      {
        return;
      }

      //保存手续费信息
      CTPInstrument ctpInstrument = this.GetInstrument(instrumentCommissionRate.InstrumentID);
      if (ctpInstrument != null)
      {
        //单合约手续费
        ctpInstrument.SetNativeValue(instrumentCommissionRate);
      }
      else
      {
        //该品种手续费
        foreach (CTPInstrument item in this.instrumentDictionary.Values)
        {
          if (string.Compare(instrumentCommissionRate.InstrumentID, item.ProductID, true) == 0)
          {
            item.SetNativeValue(instrumentCommissionRate);
          }
        }
      }
    }

    void SetInstrumentRate(CThostFtdcInstrumentMarginRateField instrumentMarginRate)
    {

      if (string.IsNullOrEmpty(instrumentMarginRate.InstrumentID))
      {
        return;
      }

      //保存保证金信息
      CTPInstrument ctpInstrument = this.GetInstrument(instrumentMarginRate.InstrumentID);
      if (ctpInstrument != null)
      {
        //单合约保证金
        ctpInstrument.SetNativeValue(instrumentMarginRate);
      }
      else
      {

        //Symbol symbol = SymbolManager.Manager.CreateSymbol(instrumentMarginRate.InstrumentID);

        //if (symbol == null)
        //{
        //  //该品种保证金
        //  foreach (CTPInstrument item in this.instrumentDictionary.Values)
        //  {
        //    if (string.Compare(instrumentMarginRate.InstrumentID, item.ProductID, true) == 0)
        //    {
        //      item.SetNativeValue(instrumentMarginRate);
        //    }
        //  }
        //}
        //else
        //{
        //  //未查询合约时，就查询保证金和手续费时，则预先构造合约信息
        //  CTPInstrument item = new CTPInstrument();

        //  CThostFtdcInstrumentField data = new CThostFtdcInstrumentField();
        //  data.ExchangeID = symbol.ExchangeCode;
        //  data.InstrumentID = symbol.ExchangeSymbolCode;
        //  data.ExchangeInstID = symbol.ExchangeSymbolCode;
        //  data.ProductID = symbol.SymbolProductCode;
        //  data.ProductClass = CTPOrderConvert.ConvertToCTP(symbol.SymbolType);
        //  data.PriceTick = Convert.ToDouble(symbol.PriceTick);
        //  data.IsTrading = symbol.TradingFlag;

        //  //创建
        //  item.SetNativeValue(data);

        //  //设置保证金
        //  item.SetNativeValue(instrumentMarginRate);

        //  this.instrumentDictionary.Add(item.ID, item); 
        //}
      }
    }

    /// <summary>
    /// 连接交易服务器
    /// </summary>
    /// <param name="frontAddress"></param>
    /// <param name="brokerID"></param>
    /// <param name="userID"></param>
    /// <param name="password"></param>
    public void Connect(string[] frontAddress, string brokerID, string userID, string password,bool restart = true)
    {

      this.BrokerID = brokerID;
      this.InvestorID = userID;
      this.Password = password;

      //创建Trader实例
      this.connTempFile = Path.GetTempFileName();
      //订阅
      int resumeMode = restart ? (int)CTPResumeType.TERT_RESTART : (int)CTPResumeType.TERT_QUICK;

      try
      {
        //创建
        _instance = (IntPtr)Process(Marshal.GetFunctionPointerForDelegate(this.callback), (int)CTPRequestAction.TraderApiCreate, (int)resumeMode, new StringBuilder(connTempFile));

        //注册前置机地址
        foreach (string front in frontAddress)
        {
          string address = front;

          if (address.StartsWith("tcp://", StringComparison.OrdinalIgnoreCase) == false)
          {
            address = "tcp://" + address;
          }

          Process(this._instance, (int)CTPRequestAction.TraderApiRegisterFront, 0, new StringBuilder(address));
          this.FrontAddress = front;
        }

        Process(this._instance, (int)CTPRequestAction.TraderApiInit, 0, null);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    void UserLogin()
    {
      CThostFtdcReqUserLoginField userLogin = new CThostFtdcReqUserLoginField();

      userLogin.BrokerID = this.BrokerID;
      userLogin.UserID = this.InvestorID;
      userLogin.Password = this.Password;
      userLogin.UserProductInfo = this.UserProductInfo;

      int result = InvokeAPI(CTPRequestAction.TraderApiUserLoginAction, userLogin);

    }

    void UserLogout()
    {
      CThostFtdcUserLogoutField userLogout = new CThostFtdcUserLogoutField();

      userLogout.BrokerID = this.BrokerID;
      userLogout.UserID = this.InvestorID;

      int result = InvokeAPI(CTPRequestAction.TraderApiUserLogoutAction, userLogout);
    }

    /*
    public void InitializeData()
    {
      //确认结算单
      //this.SettlementInfoConfirm();
      //this.QueryOrder();
      //this.QueryTrade();
      //this.QueryInvestorPosition();
      //this.QueryInvestorPositionDetail();
      //this.QueryInvestorPositionCombineDetail();

      //this.QueryCFMMCTradingAccountKey();

      //this.QueryTradingAccount();
      //this.QueryInvestor();
    }
     */


    /// <summary>
    /// 确认结算结果
    /// </summary>
    public void SettlementInfoConfirm()
    {
      CThostFtdcSettlementInfoConfirmField req = new CThostFtdcSettlementInfoConfirmField();
      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;

      //int iResult = CTPWrapper.TraderReqSettlementInfoConfirm(this._instance, req, CreateRequestID());
      InvokeAPI(CTPRequestAction.SettlementInfoConfirmAction, req);
    }

    public void ChangePassword(string oldPassword,string newPassword,bool accountAmountPassword = false)
    {

      if (accountAmountPassword == true)
      {
        CThostFtdcTradingAccountPasswordUpdateField req = new CThostFtdcTradingAccountPasswordUpdateField();

        req.BrokerID = this.BrokerID;
        req.AccountID = this.InvestorID;
        req.OldPassword = oldPassword;
        req.NewPassword = newPassword;

        //CTPWrapper.TraderReqTradingAccountPasswordUpdate(this._instance, req, CreateRequestID());
        InvokeAPI(CTPRequestAction.TradingAccountPasswordUpdateAction, req);
      }
      else
      {
        CThostFtdcUserPasswordUpdateField req = new CThostFtdcUserPasswordUpdateField();

        req.BrokerID = this.BrokerID;
        req.UserID = this.InvestorID;
        req.OldPassword = oldPassword;
        req.NewPassword = newPassword;

        //CTPWrapper.TraderReqUserPasswordUpdate(this._instance, req, CreateRequestID());
        InvokeAPI(CTPRequestAction.UserPasswordUpdateAction, req);
      }
      
    }

    #region 下单类请求

    public CThostFtdcInputOrderField InsertOrder(string symbolCode, double price, CTPDirectionType direct, int volume, CTPOffsetFlagType flag,string orderRef = "")
    {


      CThostFtdcInputOrderField order = new CThostFtdcInputOrderField();

      order.BrokerID = this.BrokerID;
      order.InvestorID = this.InvestorID;

      //合约
      order.InstrumentID = symbolCode;

      if (string.IsNullOrEmpty(orderRef))
      {
        order.OrderRef = this.IncrementOrderRef().ToString();
      }
      else
      {
        order.OrderRef = orderRef;
      }

      //限价单
      order.OrderPriceType =  CTPOrderPriceType.LimitPrice;

      //方向
      order.Direction = direct;

      //开平仓
      order.CombOffsetFlag = new byte[] { (byte)flag, 0, 0, 0, 0 };

      //投机/套保
      order.CombHedgeFlag = new byte[] { (byte)CTPHedgeFlagType.Speculation, 0, 0, 0, 0 };

      //套利合约
      if (this.instrumentDictionary.ContainsKey(symbolCode))
      {
        if (this.instrumentDictionary[symbolCode].ProductClass == CTPProductClassType.Combination)
        {
          //开平仓
          order.CombOffsetFlag = new byte[] { (byte)flag, (byte)flag, 0, 0, 0 };

          //投机/套保
          order.CombHedgeFlag = new byte[] { (byte)CTPHedgeFlagType.Speculation, (byte)CTPHedgeFlagType.Speculation, 0, 0, 0 };
        }
      }


      ///价格
      order.LimitPrice = price;
      ///数量: 1
      order.VolumeTotalOriginal = volume;
      ///有效期类型: 当日有效
      order.TimeCondition = CTPTimeConditionType.GFD;
      ///GTD日期
      //	TThostFtdcDateType	GTDDate;
      ///成交量类型: 任何数量
      order.VolumeCondition = CTPVolumeConditionType.AV;
      ///最小成交量: 1
      order.MinVolume = 1;
      ///触发条件: 立即
      order.ContingentCondition = CTPContingentConditionType.Immediately;
      ///强平原因: 非强平
      order.ForceCloseReason = CTPForceCloseReasonType.NotForceClose;
      ///自动挂起标志: 是
      order.IsAutoSuspend = true;

      ///用户强评标志: 否
      order.UserForceClose = false;


      SendInsertOrder(order);

      return order;
    }

    public CThostFtdcParkedOrderField InsertParkedOrder(string symbolCode, double price, CTPDirectionType direct, int volume, CTPOffsetFlagType flag, string orderRef)
    {
      CThostFtdcParkedOrderField order = new CThostFtdcParkedOrderField();

      order.BrokerID = this.BrokerID;
      order.InvestorID = this.InvestorID;


      //合约
      order.InstrumentID = symbolCode;

      order.OrderRef = orderRef;
      //currentOrderRef++;

      //限价单
      order.OrderPriceType = CTPOrderPriceType.LimitPrice;

      //方向
      order.Direction = direct;

      //开平仓
      order.CombOffsetFlag = new byte[] { (byte)flag, 0, 0, 0, 0 };

      //投机/套保
      order.CombHedgeFlag = new byte[] { (byte)CTPHedgeFlagType.Speculation, 0, 0, 0, 0 };

      ///价格
      order.LimitPrice = price;
      ///数量: 1
      order.VolumeTotalOriginal = volume;
      ///有效期类型: 当日有效
      order.TimeCondition = CTPTimeConditionType.GFD;
      ///GTD日期
      //	TThostFtdcDateType	GTDDate;
      ///成交量类型: 任何数量
      order.VolumeCondition = CTPVolumeConditionType.AV;
      ///最小成交量: 1
      order.MinVolume = 1;
      ///触发条件: 立即
      order.ContingentCondition = CTPContingentConditionType.ParkedOrder;
      ///止损价
      //	TThostFtdcPriceType	StopPrice;
      ///强平原因: 非强平
      order.ForceCloseReason = CTPForceCloseReasonType.NotForceClose;
      ///自动挂起标志: 是
      order.IsAutoSuspend = true;
      ///业务单元
      //	TThostFtdcBusinessUnitType	BusinessUnit;
      ///请求编号
      //	TThostFtdcorderuestIDType	orderuestID;
      ///用户强评标志: 否
      order.UserForceClose = false;

      order.Status = CTPParkedOrderStatusType.NotSend;
      //order.UserType = UserType.Investor;

      //CTPWrapper.TraderReqParkedOrderInsert(this._instance, order, CreateRequestID());
      InvokeAPI(CTPRequestAction.ParkedOrderInsertAction, order);

      return order;

    }

    public int SendInsertOrder(CThostFtdcInputOrderField order)
    {
      //Trace.WriteLine(string.Format("{0} [{1}]:{2},{3},{4}", this.wallTimeStopwatch.ElapsedMilliseconds, this.UserKey, "SendInsertOrder", order.OrderRef, order.RequestID));
      return InvokeAPI(CTPRequestAction.OrderInsertAction, order);
    }

    public CThostFtdcInputOrderActionField DeleteOrder(CThostFtdcOrderField order)
    {
      CThostFtdcInputOrderActionField orderAction = new CThostFtdcInputOrderActionField();

      orderAction.BrokerID = order.BrokerID;
      orderAction.InvestorID = order.InvestorID;
      orderAction.UserID = order.UserID;
      orderAction.InstrumentID = order.InstrumentID;


      orderAction.FrontID = order.FrontID;
      orderAction.SessionID = order.SessionID;
      orderAction.OrderRef = order.OrderRef;

      orderAction.ExchangeID = order.ExchangeID;
      orderAction.OrderSysID = order.OrderSysID;


      orderAction.ActionFlag = CTPActionFlagType.Delete;

      InvokeAPI(CTPRequestAction.OrderActionAction, orderAction);

      return orderAction;
    }

    public CThostFtdcInputOrderActionField DeleteOrder(CThostFtdcInputOrderField order)
    {
      CThostFtdcInputOrderActionField orderAction = new CThostFtdcInputOrderActionField();

      orderAction.BrokerID = order.BrokerID;
      orderAction.InvestorID = order.InvestorID;

      orderAction.FrontID = this.FrontID;
      orderAction.SessionID = this.SessionID;
      orderAction.OrderRef = order.OrderRef;

      orderAction.InstrumentID = order.InstrumentID;

      orderAction.ActionFlag = CTPActionFlagType.Delete;

      //CTPWrapper.TraderReqOrderAction(this._instance, orderAction, CreateRequestID());
      InvokeAPI(CTPRequestAction.OrderActionAction, orderAction);

      return orderAction;
    }

    #endregion

    #region 查询类请求



    /// <summary>
    /// 查询交易编码
    /// </summary>
    public void QueryTradingCode()
    {
      CThostFtdcQryTradingCodeField req = new CThostFtdcQryTradingCodeField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;
      //req.ExchangeID

      AddQueryTask(req, CTPRequestAction.QueryTradingCodeAction);
      //int iResult = CTPWrapper.TraderReqQryTradingCode(this._instance, req, ++nRequestID);
    }

    /// <summary>
    /// 查询历史结算单
    /// </summary>
    public void QuerySettlementInfo(DateTime date)
    {
      CThostFtdcQrySettlementInfoField req = new CThostFtdcQrySettlementInfoField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;
      req.TradingDay = date.ToString("yyyyMMdd");

      AddQueryTask(req, CTPRequestAction.QuerySettlementInfoAction);
      //int iResult = CTPWrapper.TraderReqQrySettlementInfo(this._instance, req, ++nRequestID);

    }

    /// <summary>
    /// 查询结算单
    /// </summary>
    public void QuerySettlementInfo()
    {
      CThostFtdcQrySettlementInfoField req = new CThostFtdcQrySettlementInfoField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;
      //req.TradingDay = "";

      AddQueryTask(req, CTPRequestAction.QuerySettlementInfoAction);
    }

    /// <summary>
    /// 查询结算单确认结果
    /// </summary>
    public void QuerySettlementInfoConfirm()
    {
      CThostFtdcQrySettlementInfoConfirmField req = new CThostFtdcQrySettlementInfoConfirmField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;

      AddQueryTask(req, CTPRequestAction.QuerySettlementInfoConfirmAction);
    }



    public void QueryNotice()
    {
      CThostFtdcQryNoticeField req = new CThostFtdcQryNoticeField();

      req.BrokerID = this.BrokerID;

      AddQueryTask(req, CTPRequestAction.QueryNoticeAction);
    }

    public void QueryTradingNotice()
    {
      CThostFtdcQryTradingNoticeField req = new CThostFtdcQryTradingNoticeField();

      req.BrokerID = this.BrokerID;
      req.InvestorID = this.InvestorID;

      AddQueryTask(req, CTPRequestAction.QueryTradingNoticeAction);
    }

    /// <summary>
    /// 投资者查询
    /// </summary>
    public void QueryInvestor()
    {
      CThostFtdcQryInvestorField req = new CThostFtdcQryInvestorField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;

      AddQueryTask(req, CTPRequestAction.QueryInvestorAction);
      //int iResult = CTPWrapper.TraderReqQryInvestor(this._instance, req, ++nRequestID);

    }

    public void QueryTradingAccount()
    {
      CThostFtdcQryTradingAccountField req = new CThostFtdcQryTradingAccountField();

      req.BrokerID = this.BrokerID;
      req.InvestorID = this.InvestorID;

      AddQueryTask(req, CTPRequestAction.QueryTradingAccountAction);
      //int iResult = CTPWrapper.TraderReqQryTradingAccount(this._instance, req, ++nRequestID);
    }

    /// <summary>
    /// 查询指定合约
    /// </summary>
    /// <param name="instrumentID"></param>
    public void QueryInstrument(string exchangeID,string instrumentID)
    {
      CThostFtdcQryInstrumentField req = new CThostFtdcQryInstrumentField();

      req.ExchangeID = exchangeID;
      req.InstrumentID = instrumentID;

      AddQueryTask(req, CTPRequestAction.QueryInstrumentAction);
      //int iResult = CTPWrapper.TraderReqQryInstrument(this._instance, req, ++nRequestID);
    }

    /// <summary>
    /// 查询指定市场的合约
    /// </summary>
    /// <param name="instrumentID"></param>
    public void QueryInstrument(string exchangeID)
    {
      this.QueryInstrument(exchangeID, "");
    }
    
    /// <summary>
    /// 查询全部合约
    /// </summary>
    public void QueryInstrument()
    {
      this.QueryInstrument("", "");
    }

    /// <summary>
    /// 查询合约保证金
    /// </summary>
    /// <param name="instrumentID"></param>
    public void QueryInstrumentMarginRate(string instrumentID, Action<CTPTaskBase<CTPRequestAction>, CTPEventArgs> callback = null)
    {
      CThostFtdcQryInstrumentMarginRateField req = new CThostFtdcQryInstrumentMarginRateField();

      req.BrokerID = this.BrokerID;
      req.InvestorID = this.InvestorID;

      req.InstrumentID = instrumentID;

      req.HedgeFlag = CTPHedgeFlagType.Speculation;

      AddQueryTask(req, CTPRequestAction.QueryInstrumentMarginRateAction, callback);
      //int iResult = CTPWrapper.TraderReqQryInstrumentMarginRate(this._instance, req, ++nRequestID);
    }

    /// <summary>
    /// 查询合约手续费
    /// </summary>
    /// <param name="instrumentID"></param>
    public void QueryInstrumentCommissionRate(string instrumentID, Action<CTPTaskBase<CTPRequestAction>, CTPEventArgs> callback = null)
    {
      CThostFtdcQryInstrumentCommissionRateField req = new CThostFtdcQryInstrumentCommissionRateField();

      req.BrokerID = this.BrokerID;
      req.InvestorID = this.InvestorID;
      
      req.InstrumentID = instrumentID;

      AddQueryTask(req, CTPRequestAction.QueryInstrumentCommissionRateAction, callback);
      //int iResult = CTPWrapper.TraderReqQryInstrumentCommissionRate(this._instance, req, ++nRequestID);
    }


    /// <summary>
    /// 查询成交情况
    /// </summary>
    public void QueryTrade()
    {
      CThostFtdcQryTradeField req = new CThostFtdcQryTradeField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;

      this.tradeList = new List<CThostFtdcTradeField>();

      this.AddQueryTask(req, CTPRequestAction.QueryTradeAction);
      //int iResult = CTPWrapper.TraderReqQryTrade(this._instance, req, ++nRequestID);

    }

    /// <summary>
    /// 持仓情况查询
    /// </summary>
    public void QueryInvestorPosition()
    {
      CThostFtdcQryInvestorPositionField req = new CThostFtdcQryInvestorPositionField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;

      this.positionList = new List<CThostFtdcInvestorPositionField>();

      this.AddQueryTask(req, CTPRequestAction.QueryInvestorPositionAction);
      //int iResult = CTPWrapper.TraderReqQryInvestorPosition(this._instance, req, ++nRequestID);

    }

    /// <summary>
    /// 持仓明细查询
    /// </summary>
    public void QueryInvestorPositionDetail()
    {
      CThostFtdcQryInvestorPositionDetailField req = new CThostFtdcQryInvestorPositionDetailField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;

      this.positionDetailList = new List<CThostFtdcInvestorPositionDetailField>();

      this.AddQueryTask(req, CTPRequestAction.QueryInvestorPositionDetailAction);
      //int iResult = CTPWrapper.TraderReqQryInvestorPositionDetail(this._instance, req, ++nRequestID);

    }

    /// <summary>
    /// 查询组合持仓明细
    /// </summary>
    public void QueryInvestorPositionCombineDetail()
    {
      CThostFtdcQryInvestorPositionCombineDetailField req = new CThostFtdcQryInvestorPositionCombineDetailField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;

      this.investorPositionCombineDetailList = new List<CThostFtdcInvestorPositionCombineDetailField>();

      this.AddQueryTask(req, CTPRequestAction.QueryInvestorPositionCombineDetailAction);
      //int iResult = CTPWrapper.TraderReqQryInvestorPositionCombineDetail(this._instance, req, ++nRequestID);

    }

    /// <summary>
    /// 报单查询
    /// </summary>
    public void QueryOrder()
    {
      CThostFtdcQryOrderField req = new CThostFtdcQryOrderField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;

      //lock (this.orderList)
      //{
      //  this.orderList.Clear();
      //}

      this.AddQueryTask(req, CTPRequestAction.QueryOrderAction);
      //int iResult = CTPWrapper.TraderReqQryOrder(this._instance, req, ++nRequestID);

    }


    public void QueryParkedOrder()
    {
      CThostFtdcQryParkedOrderField req = new CThostFtdcQryParkedOrderField();

      req.BrokerID = BrokerID;
      req.InvestorID = InvestorID;
      req.ExchangeID = "";
      req.InstrumentID = "";

      this.parkedOrderList = new List<CThostFtdcParkedOrderField>();

      this.AddQueryTask(req, CTPRequestAction.QueryParkedOrderAction);
    }

    /// <summary>
    /// 查询行情
    /// </summary>
    /// <param name="instrumentID"></param>
    public void QueryDepthMarketData(string instrumentID)
    {
      CThostFtdcQryDepthMarketDataField req = new CThostFtdcQryDepthMarketDataField();

      req.InstrumentID = instrumentID;

      //int iResult = CTPWrapper.TraderReqQryDepthMarketData(this._instance, req, ++nRequestID);
      this.AddQueryTask(req, CTPRequestAction.QueryDepthMarketDataAction);
    }

    /// <summary>
    /// 查询交易所
    /// </summary>
    /// <param name="exchangeID"></param>
    public void QueryExchange(string exchangeID)
    {
      CThostFtdcQryExchangeField req = new CThostFtdcQryExchangeField();

      req.ExchangeID = exchangeID;

      this.AddQueryTask(req, CTPRequestAction.QueryExchangeAction);
      //int iResult = CTPWrapper.TraderReqQryExchange(this._instance, req, ++nRequestID);
    }

    /// <summary>
    /// 查询交易所
    /// </summary>
    /// <param name="exchangeID"></param>
    public void QueryCFMMCTradingAccountKey()
    {
      CThostFtdcQryCFMMCTradingAccountKeyField req = new CThostFtdcQryCFMMCTradingAccountKeyField();

      req.BrokerID = this.BrokerID;
      req.InvestorID = this.InvestorID;

      this.AddQueryTask(req, CTPRequestAction.QueryCFMMCTradingAccountKeyAction);
      //int iResult = CTPWrapper.TraderReqQryExchange(this._instance, req, ++nRequestID);
    }

    #region 银期转账

    public void QueryContractBank()
    {


      CThostFtdcQryContractBankField req = new CThostFtdcQryContractBankField();

      req.BrokerID = this.BrokerID;

      this.AddQueryTask(req, CTPRequestAction.QueryContractBankAction);

    }


    public void QueryBankAccount(CThostFtdcContractBankField bank,string password)
    {
      CThostFtdcReqQueryAccountField req = new CThostFtdcReqQueryAccountField();

      req.BankID = bank.BankID;
      req.BankBranchID = bank.BankBrchID;
      req.BrokerID = this.BrokerID;
      req.AccountID = this.InvestorID;



    }


    public void QueryTransferSerial()
    {
      CThostFtdcQryTransferSerialField req = new CThostFtdcQryTransferSerialField();

      req.BrokerID = this.BrokerID;
      req.AccountID = this.InvestorID;

      this.AddQueryTask(req, CTPRequestAction.QueryTransferSerialAction);
    }


    #endregion


    #endregion

    protected override void ProcessBusinessResponse(CTPResponseType responseType, IntPtr pData, CTPResponseInfo rspInfo, int requestID)
    {

      Trace.WriteLine(string.Format("{0} [{1}]:{2},{3},{4}", this.wallTimeStopwatch.ElapsedMilliseconds, this.UserKey, "ProcessBusinessResponse", responseType, requestID));

      switch (responseType)
      {
        #region 当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
        case CTPResponseType.FrontConnectedResponse:
          {

            this.isConnect = true;

            this.UserLogin();

            //调用事件
            OnEventHandler(CTPResponseType.FrontConnectedResponse, new CTPEventArgs());

            break;
          }
        #endregion

        #region 通信中断
        case CTPResponseType.FrontDisconnectedResponse:
          ///  当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
          ///@param nReason 错误原因
          ///        0x1001 网络读失败
          ///        0x1002 网络写失败
          ///        0x2001 接收心跳超时
          ///        0x2002 发送心跳失败
          ///        0x2003 收到错误报文
          {
            this.isConnect = false;

            //调用事件
            OnEventHandler(CTPResponseType.FrontDisconnectedResponse, new CTPEventArgs());
          }
          break;
        #endregion

        #region 心跳超时警告
        case CTPResponseType.HeartBeatWarningResponse:
          {
            /// <summary>
            ///心跳超时警告。当长时间未收到报文时，该方法被调用。
            ///@param nTimeLapse 距离上次接收报文的时间
            /// </summary>
            /// <param name="nTimeLapse"></param>
            break;
          }
        #endregion

        #region 客户端认证响应
        case CTPResponseType.AuthenticateResponse:
          {
            CTPEventArgs<CThostFtdcRspAuthenticateField> args = CreateEventArgs<CThostFtdcRspAuthenticateField>(requestID, rspInfo);

            this.OnEventHandler(CTPResponseType.AuthenticateResponse, args);

            break;
          }
        #endregion

        #region 用户登录
        case CTPResponseType.UserLoginResponse:
          {

            CTPEventArgs<CThostFtdcRspUserLoginField> args = CreateEventArgs<CThostFtdcRspUserLoginField>(requestID, rspInfo);

            CThostFtdcRspUserLoginField userLogin = args.Value;


            if (rspInfo.ErrorID == 0)
            {

              this.BrokerID = userLogin.BrokerID;
              this.FrontID = userLogin.FrontID;
              this.SessionID = userLogin.SessionID;
              this.isLogin = true;
              this.loginTime = userLogin.LoginTime;

              //最大报单引用
              int orderRef = 0;

              if (int.TryParse(userLogin.MaxOrderRef, out orderRef) == false)
              {
                orderRef = 0;
              }

              this.orderRef = orderRef;
              //this.currentOrderRef++;

              DateTime.TryParseExact(userLogin.TradingDay, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out this.tradingDate);
              
              //大连
              TimeSpan.TryParse(userLogin.DCETime, out this.loginTimeDCE);
              this.exchanges["DCE"].CurrentTime = this.loginTimeDCE;
              //郑州
              TimeSpan.TryParse(userLogin.CZCETime, out this.loginTimeCZCE);
              this.exchanges["CZCE"].CurrentTime = this.loginTimeCZCE;
              //中金
              TimeSpan.TryParse(userLogin.FFEXTime, out this.loginTimeCFFEX);
              this.exchanges["CFFEX"].CurrentTime = this.loginTimeCFFEX;
              //上海
              TimeSpan.TryParse(userLogin.SHFETime, out this.loginTimeSHFE);
              this.exchanges["SHFE"].CurrentTime = this.loginTimeSHFE;

              this.wallTimeStopwatch.Start();
              this.timer.Change(0, 1000);
            }

            this.OnEventHandler(CTPResponseType.UserLoginResponse, args);
          }
          break;
        #endregion

        #region 登出请求响应
        case CTPResponseType.UserLogoutResponse:
          {
            this.isLogin = false;

            //调用事件
            OnEventHandler(CTPResponseType.UserLogoutResponse, new CTPEventArgs());
            break;
          }
        #endregion


        #region 用户口令更新请求响应
        case CTPResponseType.UserPasswordUpdateResponse:
          /// <summary>
          /// 用户口令更新请求响应
          /// </summary>
          {
            CTPEventArgs<CThostFtdcUserPasswordUpdateField> args = CreateEventArgs<CThostFtdcUserPasswordUpdateField>(requestID, rspInfo);

            this.OnEventHandler(CTPResponseType.UserPasswordUpdateResponse, args);

            break;
          }
        #endregion

        #region 资金账户口令更新请求响应
        case CTPResponseType.TradingAccountPasswordUpdateResponse:
          /// <summary>
          /// 资金账户口令更新请求响应
          /// </summary>
          /// <param name="pTradingAccountPasswordUpdate"></param>
          /// <param name="pRspInfo"></param>
          /// <param name="nRequestID"></param>
          /// <param name="bIsLast"></param>
          {
            CTPEventArgs<CThostFtdcTradingAccountPasswordUpdateField> args = CreateEventArgs<CThostFtdcTradingAccountPasswordUpdateField>(requestID, rspInfo);

            this.OnEventHandler(CTPResponseType.TradingAccountPasswordUpdateResponse, args);

            break;
          }
        #endregion

        #region 报单录入请求响应
        case CTPResponseType.OrderInsertResponse:
          {

            CTPEventArgs<CThostFtdcInputOrderField> args = CreateEventArgs<CThostFtdcInputOrderField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.OrderInsertResponse, args);

            break;
          }
        #endregion

        #region 预埋单录入请求响应
        case CTPResponseType.ParkedOrderInsertResponse:
          {
            CTPEventArgs<CThostFtdcParkedOrderField> args = CreateEventArgs<CThostFtdcParkedOrderField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ParkedOrderInsertResponse, args);

            break;
          }
        #endregion

        #region 预埋单撤单请求响应
        case CTPResponseType.ParkedOrderActionResponse:
          {
            CTPEventArgs<CThostFtdcParkedOrderActionField> args = CreateEventArgs<CThostFtdcParkedOrderActionField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ParkedOrderActionResponse, args);
            break;
          }
        #endregion

        #region 改单响应
        case CTPResponseType.OrderActionResponse:
          {
            CTPEventArgs<CThostFtdcInputOrderActionField> args = CreateEventArgs<CThostFtdcInputOrderActionField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.OrderActionResponse, args);

            break;
          }
        #endregion

        #region 查询最大报单数量响应
        case CTPResponseType.QueryMaxOrderVolumeResponse:
          {
            CTPEventArgs<CThostFtdcQueryMaxOrderVolumeField> args = CreateEventArgs<CThostFtdcQueryMaxOrderVolumeField>(requestID, rspInfo);

            this.OnEventHandler(CTPResponseType.QueryMaxOrderVolumeResponse, args);

            break;
          }
        #endregion

        #region 投资者结算结果确认响应
        case CTPResponseType.SettlementInfoConfirmResponse:
          {

            if (rspInfo.ErrorID == 0)
            {
              this.isSettlementConfim = true;
            }

            CTPEventArgs<CThostFtdcSettlementInfoConfirmField> args = CreateEventArgs<CThostFtdcSettlementInfoConfirmField>(requestID, rspInfo);

            this.OnEventHandler(CTPResponseType.SettlementInfoConfirmResponse, args);

            break;
          }
        #endregion

        #region 删除预埋单响应
        case CTPResponseType.RemoveParkedOrderResponse:
          {
            CTPEventArgs<CThostFtdcRemoveParkedOrderField> args = CreateEventArgs<CThostFtdcRemoveParkedOrderField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.RemoveParkedOrderResponse, args);

            break;
          }
        #endregion

        #region 删除预埋撤单响应
        case CTPResponseType.RemoveParkedOrderActionResponse:
          {
            CTPEventArgs<CThostFtdcRemoveParkedOrderActionField> args = CreateEventArgs<CThostFtdcRemoveParkedOrderActionField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.RemoveParkedOrderActionResponse, args);

            break;
          }
        #endregion

        #region 查询报单响应
        case CTPResponseType.QueryOrderResponse:
          {
            CTPEventArgs<List<CThostFtdcOrderField>> args = CreateListEventArgs<CThostFtdcOrderField>(requestID, rspInfo);

            lock (this.OrderList)
            {
              this.OrderList.Clear();
              this.OrderList.AddRange(args.Value);
            }

            //调用事件
            OnEventHandler(CTPResponseType.QueryOrderResponse, args);

            break;
          }
        #endregion

        #region 查询成交单响应
        case CTPResponseType.QueryTradeResponse:
          {
            CTPEventArgs<List<CThostFtdcTradeField>> args = CreateListEventArgs<CThostFtdcTradeField>(requestID, rspInfo);

            this.TradeList.Clear();
            this.TradeList.AddRange(args.Value);

            //调用事件
            OnEventHandler(CTPResponseType.QueryTradeResponse, args);

            break;
          }
        #endregion

        #region 查询投资者持仓响应
        case CTPResponseType.QueryInvestorPositionResponse:
          {
            CTPEventArgs<List<CThostFtdcInvestorPositionField>> args = CreateListEventArgs<CThostFtdcInvestorPositionField>(requestID, rspInfo);

            this.PositionList.Clear();
            this.PositionList.AddRange(args.Value);


            //调用事件
            OnEventHandler(CTPResponseType.QueryInvestorPositionResponse, args);
            break;
          }
        #endregion

        #region 查询资金账户响应
        case CTPResponseType.QueryTradingAccountResponse:
          {

            /// 查询资金账户响应
            CTPEventArgs<CThostFtdcTradingAccountField> args = CreateEventArgs<CThostFtdcTradingAccountField>(requestID, rspInfo);

            this.tradingAccount = args.Value;

            this.OnEventHandler(CTPResponseType.QueryTradingAccountResponse, args);

            break;
          }
        #endregion

        #region 查询投资者响应
        case CTPResponseType.QueryInvestorResponse:
          {
            CTPEventArgs<CThostFtdcInvestorField> args = CreateEventArgs<CThostFtdcInvestorField>(requestID, rspInfo);

            this.investor = args.Value;

            this.OnEventHandler(CTPResponseType.QueryInvestorResponse, args);

            break;
          }
        #endregion

        #region 请求查询交易编码响应
        case CTPResponseType.QueryTradingCodeResponse:
          {
            CTPEventArgs<CThostFtdcTradingCodeField> args = CreateEventArgs<CThostFtdcTradingCodeField>(requestID, rspInfo);

            this.OnEventHandler(CTPResponseType.QueryTradingCodeResponse, args);

            break;
          }
        #endregion

        #region 查询保证金响应
        case CTPResponseType.QueryInstrumentMarginRateResponse:
          {
            CTPEventArgs<CThostFtdcInstrumentMarginRateField> args = CreateEventArgs<CThostFtdcInstrumentMarginRateField>(requestID, rspInfo);

            this.SetInstrumentRate(args.Value);

            //调用事件
            OnEventHandler(CTPResponseType.QueryInstrumentMarginRateResponse, args);

            break;
          }
        #endregion

        #region 查询手续费响应
        case CTPResponseType.QueryInstrumentCommissionRateResponse:
          {
            CTPEventArgs<CThostFtdcInstrumentCommissionRateField> args = CreateEventArgs<CThostFtdcInstrumentCommissionRateField>(requestID, rspInfo);

            this.SetInstrumentRate(args.Value);

            //调用事件
            OnEventHandler(CTPResponseType.QueryInstrumentCommissionRateResponse, args);

            CTPTaskBase<CTPRequestAction> task = null;
            if (this.processedTasks.TryGetValue(args.RequestID, out task))
            {
              if (task.Callback != null)
              {
                task.Callback(task,args);
              }
            }
            break;
          }
        #endregion

        #region 查询交易所响应
        case CTPResponseType.QueryExchangeResponse:
          {

            CTPEventArgs<CThostFtdcExchangeField> args = CreateEventArgs<CThostFtdcExchangeField>(requestID, rspInfo);

            //保存市场信息
            CTPExchange ctpExchange = new CTPExchange();
            ctpExchange.SetNativeValue(args.Value);
            this.Exchanges.Add(args.Value.ExchangeID, ctpExchange);

            //调用事件
            OnEventHandler(CTPResponseType.QueryExchangeResponse, new CTPEventArgs<CTPExchange>(ctpExchange));

            break;
          }
        #endregion

        #region 查询合约响应
        case CTPResponseType.QueryInstrumentResponse:
          {

            CTPEventArgs<List<CThostFtdcInstrumentField>> values = CreateListEventArgs<CThostFtdcInstrumentField>(requestID, rspInfo);

            foreach (var instrument in values.Value)
            {

              if (instrument.ProductClass == CTPProductClassType.EFP)
              {
                continue;
              }

              CTPInstrument ctpInstrument = new CTPInstrument();
              ctpInstrument.SetNativeValue(instrument);

              //加入到市场列表
              if (this.Exchanges.ContainsKey(ctpInstrument.ExchangeID))
              {
                this.Exchanges[instrument.ExchangeID].Instruments.Add(ctpInstrument);
              }

              //if (this.SymbolProducts.ContainsKey(ctpInstrument.ProductID.ToUpper()) == false)
              //{
              //  this.SymbolProducts.Add(ctpInstrument.ProductID.ToUpper(), new CTPSymbolProduct(instrument));
              //}

              //加入到合约表
              if (this.InstrumentDictionary.ContainsKey(ctpInstrument.ID) == false)
              {
                this.InstrumentDictionary.Add(ctpInstrument.ID, ctpInstrument);
              }
            }

            //创建新的事件参数
            List<CTPInstrument> list = new List<CTPInstrument>(this.InstrumentDictionary.Values);
            CTPEventArgs<List<CTPInstrument>> args = new CTPEventArgs<List<CTPInstrument>>(list);

            //调用事件
            OnEventHandler(CTPResponseType.QueryInstrumentResponse, args);

            break;
          }
        #endregion

        #region 查询行情响应
        case CTPResponseType.QueryDepthMarketDataResponse:
          {
            CTPEventArgs<CThostFtdcDepthMarketDataField> args = CreateEventArgs<CThostFtdcDepthMarketDataField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.QueryDepthMarketDataResponse, args);

            break;
          }
        #endregion

        #region 查询结算单
        case CTPResponseType.QuerySettlementInfoResponse:
          {

            CTPEventArgs<List<CThostFtdcSettlementInfoField>> rspData = CreateListEventArgs<CThostFtdcSettlementInfoField>(requestID, rspInfo);

            CTPSettlementInfo sinfo = new CTPSettlementInfo();

            foreach (var item in rspData.Value)
            {
              sinfo.Context += PInvokeUtility.GetUnicodeString(item.Content);
            }

            //读取结算单信息
            //sinfo.ReadContext();

            //从请求参数中获取日期
            CThostFtdcQrySettlementInfoField queryData = (CThostFtdcQrySettlementInfoField)this.processedTasks[requestID].Parameter;
            string tradingDate = queryData.TradingDay;

            if (string.IsNullOrEmpty(tradingDate))
            {
              tradingDate = sinfo.TradingDate;
            }

            if (string.IsNullOrEmpty(tradingDate))
            {
              tradingDate = this.TradingDate.ToString("yyyyMMdd");
            }

            if (this.settlementInfoDictionary.ContainsKey(tradingDate) == false)
            {
              this.settlementInfoDictionary.Add(tradingDate, sinfo);
            }
            else
            {
              this.settlementInfoDictionary[tradingDate] = sinfo;
            }

            //调用事件
            OnEventHandler(CTPResponseType.QuerySettlementInfoResponse, new CTPEventArgs<CTPSettlementInfo>(sinfo, rspInfo));

            break;
          }
        #endregion

        #region 请求查询转帐银行响应
        case CTPResponseType.QueryTransferBankResponse:
          {
            CTPEventArgs<CThostFtdcTransferBankField> args = CreateEventArgs<CThostFtdcTransferBankField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.QueryTransferBankResponse, args);

            break;
          }
        #endregion

        #region 查询投资者持仓明细响应
        case CTPResponseType.QueryInvestorPositionDetailResponse:
          {

            CTPEventArgs<List<CThostFtdcInvestorPositionDetailField>> args = CreateListEventArgs<CThostFtdcInvestorPositionDetailField>(requestID, rspInfo);

            this.PositionDetailList.Clear();
            this.PositionDetailList.AddRange(args.Value);

            //调用事件
            OnEventHandler(CTPResponseType.QueryInvestorPositionDetailResponse, args);

            break;
          }
        #endregion

        #region 请求查询客户通知响应
        case CTPResponseType.QueryNoticeResponse:
          {

            CTPEventArgs<List<CThostFtdcNoticeField>> args = CreateListEventArgs<CThostFtdcNoticeField>(requestID, rspInfo);

            StringBuilder buffer = new StringBuilder();

            foreach (var item in args.Value)
            {
              buffer.Append(PInvokeUtility.GetUnicodeString(item.Content));
            }

            //调用事件
            OnEventHandler(CTPResponseType.QueryNoticeResponse, new CTPEventArgs<String>(buffer.ToString()));

            break;
          }
        #endregion

        #region 查询结算单确认响应
        case CTPResponseType.QuerySettlementInfoConfirmResponse:
          {
            CTPEventArgs<CThostFtdcSettlementInfoConfirmField> args = CreateEventArgs<CThostFtdcSettlementInfoConfirmField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.QuerySettlementInfoConfirmResponse, args);

            break;
          }
        #endregion

        #region 查询投资者组合持仓明细响应
        case CTPResponseType.QueryInvestorPositionCombineDetailResponse:
          {
            CTPEventArgs<List<CThostFtdcInvestorPositionCombineDetailField>> args = CreateListEventArgs<CThostFtdcInvestorPositionCombineDetailField>(requestID, rspInfo);


            this.investorPositionCombineDetailList = args.Value;


            //调用事件
            OnEventHandler(CTPResponseType.QueryInvestorPositionCombineDetailResponse, args);
            break;
          }
        #endregion

        #region 查询保证金中心响应
        case CTPResponseType.QueryCFMMCTradingAccountKeyResponse:
          {
            if (pData == IntPtr.Zero)
            {
              //不存在保证金KEY的服务器，是模拟服务器
              this.isSimulationServer = true;
            }

            //查询保证金中心，为登录后内部初始化时的最后调用
            //因此，当该查询返回时，可认为初始化完成
            this.isInitialized = true;

            CTPEventArgs<CThostFtdcCFMMCTradingAccountKeyField> args = CreateEventArgs<CThostFtdcCFMMCTradingAccountKeyField>(requestID, rspInfo);

            this.cfmmcTradingAccountKey = args.Value;

            //调用事件
            OnEventHandler(CTPResponseType.QueryCFMMCTradingAccountKeyResponse, args);

            break;
          }
        #endregion

        #region 【20120828增加（未实现）】请求查询仓单折抵信息响应 
        case CTPResponseType.QueryEWarrantOffsetResponse:
          {
            break;
          }
        #endregion

        #region 请求查询转帐流水响应
        case CTPResponseType.QueryTransferSerialResponse:
          {
            CTPEventArgs<List<CThostFtdcTransferSerialField>> args = CreateListEventArgs<CThostFtdcTransferSerialField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.QueryTransferSerialResponse, args);

            break;
          }
        #endregion

        #region 【20120828增加（未实现）】请求查询银期签约关系响应
        case CTPResponseType.QueryAccountregisterResponse:
          {
            break;
          }
        #endregion

        #region 错误回报
        case CTPResponseType.ErrorResponse:
          {
            this.OnEventHandler(CTPResponseType.ErrorResponse, new CTPEventArgs(rspInfo));
            break;
          }
        #endregion

        #region 报单回报
        case CTPResponseType.ReturnOrderResponse:
          {

            CTPEventArgs<CThostFtdcOrderField> args = CreateEventArgs<CThostFtdcOrderField>(pData, rspInfo);

            lock (this.orderList)
            {
              AppendOrReplaceOrder(this.orderList, args.Value);
            }

            //调用事件
            OnEventHandler(CTPResponseType.ReturnOrderResponse, args);

            break;
          }
        #endregion

        #region 成交回报
        case CTPResponseType.ReturnTradeResponse:
          {
            CTPEventArgs<CThostFtdcTradeField> args = CreateEventArgs<CThostFtdcTradeField>(pData, rspInfo);

            //插入到列表中
            AppendOrReplaceOrder(this.tradeList, args.Value);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnTradeResponse, args);

            break;
          }
        #endregion

        #region 错单回报
        case CTPResponseType.ErrorReturnOrderInsertResponse:
          {

            CTPEventArgs<CThostFtdcInputOrderField> args = CreateEventArgs<CThostFtdcInputOrderField>(pData, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ErrorReturnOrderInsertResponse, args);

            break;
          }
        #endregion

        #region 报单操作错误回报
        case CTPResponseType.ErrorReturnOrderActionResponse:
          {

            CTPEventArgs<CThostFtdcOrderActionField> args = CreateEventArgs<CThostFtdcOrderActionField>(pData, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ErrorReturnOrderActionResponse, args);

            break;
          }
        #endregion

        #region 合约交易状态通知
        case CTPResponseType.ReturnInstrumentStatusResponse:
          {

            CTPEventArgs<CThostFtdcInstrumentStatusField> args = CreateEventArgs<CThostFtdcInstrumentStatusField>(pData, rspInfo);

            CThostFtdcInstrumentStatusField instrumentStatus = args.Value;

            Trace.WriteLine(string.Format("{0}:{1} {2}", instrumentStatus.ExchangeID, instrumentStatus.InstrumentStatus,instrumentStatus.EnterTime));

            //交易所状态
            if (this.exchangeStatus.ContainsKey(instrumentStatus.ExchangeID) == false)
            {
              this.exchangeStatus.Add(instrumentStatus.ExchangeID, instrumentStatus.InstrumentStatus);
            }
            //更新交易所状态
            this.exchangeStatus[instrumentStatus.ExchangeID] = instrumentStatus.InstrumentStatus;

            this.tradingStatus = instrumentStatus.InstrumentStatus;

            //初始化完成之前不推送状态事件
            if (isInitialized)
            {
              //调用事件
              OnEventHandler(CTPResponseType.ReturnInstrumentStatusResponse, args);
            }
            break;
          }
        #endregion

        #region 交易通知
        case CTPResponseType.ReturnTradingNoticeResponse:
          {
            //CTPEventArgs<CThostFtdcTradingNoticeInfoField> args = CreateEventArgs<CThostFtdcTradingNoticeInfoField>(pData, rspInfo);
            CThostFtdcTradingNoticeInfoField value = PInvokeUtility.GetObjectFromIntPtr<CThostFtdcTradingNoticeInfoField>(pData);

            CTPEventArgs<string> args = new CTPEventArgs<string>(PInvokeUtility.GetUnicodeString(value.FieldContent));

            //调用事件
            OnEventHandler(CTPResponseType.ReturnTradingNoticeResponse, args);

            break;
          }
        #endregion

        #region 【已删除】提示条件单校验错误
        //case CTPResponseType.ReturnErrorConditionalOrderResponse:
        //  {

        //    CTPEventArgs<CThostFtdcErrorConditionalOrderField> args = CreateEventArgs<CThostFtdcErrorConditionalOrderField>(pData, rspInfo);

        //    //调用事件
        //    OnEventHandler(CTPResponseType.ReturnErrorConditionalOrderResponse, args);

        //    break;
        //  }
        #endregion

        #region 请求查询签约银行响应
        case CTPResponseType.QueryContractBankResponse:
          {
            CTPEventArgs<List<CThostFtdcContractBankField>> args = CreateListEventArgs<CThostFtdcContractBankField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.QueryContractBankResponse, args);

            break;
          }
        #endregion

        #region 查询预埋单响应
        case CTPResponseType.QueryParkedOrderResponse:
          {
            CTPEventArgs<List<CThostFtdcParkedOrderField>> args = CreateListEventArgs<CThostFtdcParkedOrderField>(requestID, rspInfo);

            this.ParkedOrderList.Clear();
            this.ParkedOrderList.AddRange(args.Value);

            //调用事件
            OnEventHandler(CTPResponseType.QueryParkedOrderResponse, args);

            break;
          }
        #endregion

        #region 查询预埋撤单响应
        case CTPResponseType.QueryParkedOrderActionResponse:
          {

            CTPEventArgs<List<CThostFtdcParkedOrderActionField>> args = CreateListEventArgs<CThostFtdcParkedOrderActionField>(requestID, rspInfo);

            this.ParkedOrderActionList.Clear();
            this.ParkedOrderActionList.AddRange(args.Value);

            //调用事件
            OnEventHandler(CTPResponseType.QueryParkedOrderActionResponse, args);

            break;
          }
        #endregion

        #region 请求查询交易通知响应
        case CTPResponseType.QueryTradingNoticeResponse:
          {
            CTPEventArgs<List<CThostFtdcTradingNoticeField>> args = CreateListEventArgs<CThostFtdcTradingNoticeField>(requestID, rspInfo);

            StringBuilder buffer = new StringBuilder();

            //合并消息内容
            foreach (var item in args.Value)
            {
              buffer.Append(PInvokeUtility.GetUnicodeString(item.FieldContent));
            }

            //调用事件
            OnEventHandler(CTPResponseType.QueryTradingNoticeResponse, new CTPEventArgs<string>(buffer.ToString()));

            break;
          }
        #endregion

        #region 请求查询经纪公司交易参数响应
        case CTPResponseType.QueryBrokerTradingParamsResponse:
          {

            CTPEventArgs<CThostFtdcBrokerTradingParamsField> args = CreateEventArgs<CThostFtdcBrokerTradingParamsField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.QueryBrokerTradingParamsResponse, args);

          }
          break;
        #endregion

        #region 请求查询经纪公司交易参数响应
        case CTPResponseType.QueryBrokerTradingAlgosResponse:
          {
            CTPEventArgs<CThostFtdcBrokerTradingAlgosField> args = CreateEventArgs<CThostFtdcBrokerTradingAlgosField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.QueryBrokerTradingAlgosResponse, args);

            break;
          }
        #endregion

        #region 银行发起银行资金转期货通知
        case CTPResponseType.ReturnFromBankToFutureByBankResponse:
          {
            CTPEventArgs<CThostFtdcRspTransferField> args = CreateEventArgs<CThostFtdcRspTransferField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnFromBankToFutureByBankResponse, args);

            break;
          }
        #endregion

        #region 银行发起期货资金转银行通知
        case CTPResponseType.ReturnFromFutureToBankByBankResponse:
          {
            CTPEventArgs<CThostFtdcRspTransferField> args = CreateEventArgs<CThostFtdcRspTransferField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnFromFutureToBankByBankResponse, args);

            break;
          }
        #endregion

        #region 银行发起冲正银行转期货通知
        case CTPResponseType.ReturnRepealFromBankToFutureByBankResponse:
          {
            CTPEventArgs<CThostFtdcRspRepealField> args = CreateEventArgs<CThostFtdcRspRepealField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnRepealFromBankToFutureByBankResponse, args);

            break;
          }
        #endregion

        #region 银行发起冲正期货转银行通知
        case CTPResponseType.ReturnRepealFromFutureToBankByBankResponse:
          {
            CTPEventArgs<CThostFtdcRspRepealField> args = CreateEventArgs<CThostFtdcRspRepealField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnRepealFromFutureToBankByBankResponse, args);

            break;
          }
        #endregion

        #region 期货发起银行资金转期货通知
        case CTPResponseType.ReturnFromBankToFutureByFutureResponse:
          {
            CTPEventArgs<CThostFtdcRspTransferField> args = CreateEventArgs<CThostFtdcRspTransferField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnFromBankToFutureByFutureResponse, args);

            break;
          }
        #endregion

        #region 期货发起期货资金转银行通知
        case CTPResponseType.ReturnFromFutureToBankByFutureResponse:
          {
            CTPEventArgs<CThostFtdcRspTransferField> args = CreateEventArgs<CThostFtdcRspTransferField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnFromFutureToBankByFutureResponse, args);

            break;
          }
        #endregion

        #region 系统运行时期手工冲正
        case CTPResponseType.ReturnRepealFromBankToFutureByFutureManualResponse:
          {
            CTPEventArgs<CThostFtdcRspRepealField> args = CreateEventArgs<CThostFtdcRspRepealField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnRepealFromBankToFutureByFutureManualResponse, args);

            break;
          }
        case CTPResponseType.ReturnRepealFromFutureToBankByFutureManualResponse:
          {
            CTPEventArgs<CThostFtdcRspRepealField> args = CreateEventArgs<CThostFtdcRspRepealField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnRepealFromFutureToBankByFutureManualResponse, args);

            break;
          }
        #endregion

        #region 期货发起查询银行余额通知
        case CTPResponseType.ReturnQueryBankBalanceByFutureResponse:
          {
            CTPEventArgs<CThostFtdcNotifyQueryAccountField> args = CreateEventArgs<CThostFtdcNotifyQueryAccountField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnQueryBankBalanceByFutureResponse, args);

            break;
          }
        #endregion

        #region 期货端出入金错误回报
        case CTPResponseType.ErrorReturnBankToFutureByFutureResponse:
          {
            CTPEventArgs<CThostFtdcReqTransferField> args = CreateEventArgs<CThostFtdcReqTransferField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ErrorReturnBankToFutureByFutureResponse, args);

            break;
          }
        case CTPResponseType.ErrorReturnFutureToBankByFutureResponse:
          {
            CTPEventArgs<CThostFtdcReqTransferField> args = CreateEventArgs<CThostFtdcReqTransferField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ErrorReturnFutureToBankByFutureResponse, args);

            break;
          }
        #endregion

        #region 手工冲正错误回报
        case CTPResponseType.ErrorReturnRepealBankToFutureByFutureManualResponse:
          {
            CTPEventArgs<CThostFtdcReqRepealField> args = CreateEventArgs<CThostFtdcReqRepealField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ErrorReturnRepealBankToFutureByFutureManualResponse, args);

            break;
          }
        case CTPResponseType.ErrorReturnRepealFutureToBankByFutureManualResponse:
          {
            CTPEventArgs<CThostFtdcReqRepealField> args = CreateEventArgs<CThostFtdcReqRepealField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ErrorReturnRepealFutureToBankByFutureManualResponse, args);

            break;
          }
        #endregion

        #region 期货发起查询银行余额错误回报
        case CTPResponseType.ErrorReturnQueryBankBalanceByFutureResponse:
          {
            CTPEventArgs<CThostFtdcReqQueryAccountField> args = CreateEventArgs<CThostFtdcReqQueryAccountField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ErrorReturnQueryBankBalanceByFutureResponse, args);

            break;
          }
        #endregion

        #region 期货发起冲正请求，银行处理完毕后报盘发回的通知
        case CTPResponseType.ReturnRepealFromBankToFutureByFutureResponse:
          {
            CTPEventArgs<CThostFtdcRspRepealField> args = CreateEventArgs<CThostFtdcRspRepealField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnRepealFromBankToFutureByFutureResponse, args);

            break;
          }
        case CTPResponseType.ReturnRepealFromFutureToBankByFutureResponse:
          {
            CTPEventArgs<CThostFtdcRspRepealField> args = CreateEventArgs<CThostFtdcRspRepealField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.ReturnRepealFromFutureToBankByFutureResponse, args);

            break;
          }
        #endregion

        #region 期货发起出入金应答
        case CTPResponseType.FromBankToFutureByFutureResponse:
          {
            CTPEventArgs<CThostFtdcReqTransferField> args = CreateEventArgs<CThostFtdcReqTransferField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.FromBankToFutureByFutureResponse, args);

            break;
          }
        case CTPResponseType.FromFutureToBankByFutureResponse:
          {
            CTPEventArgs<CThostFtdcReqTransferField> args = CreateEventArgs<CThostFtdcReqTransferField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.FromFutureToBankByFutureResponse, args);

            break;
          }
        #endregion

        #region 期货发起查询银行余额应答
        case CTPResponseType.QueryBankAccountMoneyByFutureResponse:
          {
            CTPEventArgs<CThostFtdcReqQueryAccountField> args = CreateEventArgs<CThostFtdcReqQueryAccountField>(requestID, rspInfo);

            //调用事件
            OnEventHandler(CTPResponseType.QueryBankAccountMoneyByFutureResponse, args);

            break;
          }
        #endregion

        #region 【20120828增加（未实现）】银行发起银期开户通知
        case CTPResponseType.OpenAccountByBankResponse:
          {
            break;
          }
        #endregion

        #region 【20120828增加（未实现）】银行发起银期销户通知
        case CTPResponseType.CancelAccountByBankResponse:
          {
            break;
          }
        #endregion

        #region 【20120828增加（未实现）】银行发起变更银行账号通知
        case CTPResponseType.ChangeAccountByBankResponse:
          {
            break;
          }
        #endregion


        default:
          break;
      }
    }


    #region IDisposable 成员

    public override void Dispose()
    {

      if (this.isDispose == true)
      {
        return;
      }

      this.isDispose = true;

      try
      {
        if (this._instance != IntPtr.Zero)
        {
          //CTPWrapper.TraderRegisterSpi(this._instance, IntPtr.Zero);
          //CTPWrapper.TraderRelease(this._instance);
          Process(this._instance, (int)CTPRequestAction.TraderApiRelease, 0, null);

          this._instance = IntPtr.Zero;
        }

      }
      catch (Exception ex)
      {

      }
      finally
      {

        this.timer.Change(Timeout.Infinite, Timeout.Infinite);
        this.wallTimeStopwatch.Stop();

        this.queryTaskTimer.Change(Timeout.Infinite, Timeout.Infinite);
        this.queryTasks.Clear();

        this.processedTasks.Clear();
        this.orderList.Clear();

        this.orderList.Clear();
        this.tradeList.Clear();
        this.positionList.Clear();
        this.positionDetailList.Clear();
        this.parkedOrderList.Clear();
        this.parkedOrderActionList.Clear();

      }

      base.Dispose();
    }

    #endregion


    public override bool Equals(object obj)
    {

      if (obj == null)
      {
        return false;
      }

      if (obj is CTPTrader)
      {
        CTPTrader trader = obj as CTPTrader;

        if (trader == this)
        {
          return true;
        }
        else
        {
          return trader.UserKey == this.UserKey;
        }
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return this.UserKey.GetHashCode();
    }

    static void AppendOrReplaceOrder(IList list, object ctpOrder)
    {

      for (int i = 0; i < list.Count; i++)
      {
        object order = list[i];

        if (GetOrderUniqueKey(order) == GetOrderUniqueKey(ctpOrder))
        {
          list[i] = ctpOrder;
          return;
        }
      }

      list.Add(ctpOrder);

    }

    static string GetOrderUniqueKey(object field)
    {
      //委托单
      //if (field is CThostFtdcInputOrderField)
      //{
      //  CThostFtdcInputOrderField ctpOrder = (CThostFtdcInputOrderField)field;
      //  return string.Format("{0}_{1}_{2}_{3}", ctpOrder.BrokerID, ctpOrder.InvestorID, ctpOrder.InstrumentID, ctpOrder.OrderRef);
      //}

      //撤单
      if (field is CThostFtdcOrderActionField)
      {
        CThostFtdcOrderActionField ctpOrder = (CThostFtdcOrderActionField)field;
        return string.Format("{0:X}_{1:X}_{2}", ctpOrder.FrontID, ctpOrder.SessionID, ctpOrder.OrderRef.Trim());
      }

      //订单
      if (field is CThostFtdcOrderField)
      {
        CThostFtdcOrderField ctpOrder = (CThostFtdcOrderField)field;
        return string.Format("{0:X}_{1:X}_{2}", ctpOrder.FrontID, ctpOrder.SessionID, ctpOrder.OrderRef.Trim());
      }

      //预埋单
      if (field is CThostFtdcParkedOrderField)
      {
        CThostFtdcParkedOrderField ctpOrder = (CThostFtdcParkedOrderField)field;
        return string.Format("{0}_{1}_{2}", ctpOrder.BrokerID, ctpOrder.InvestorID, ctpOrder.ParkedOrderID);
      }

      //删除预埋单
      if (field is CThostFtdcRemoveParkedOrderField)
      {
        CThostFtdcRemoveParkedOrderField ctpOrder = (CThostFtdcRemoveParkedOrderField)field;
        return string.Format("{0}_{1}_{2}", ctpOrder.BrokerID, ctpOrder.InvestorID, ctpOrder.ParkedOrderID);
      }


      //成交单
      if (field is CThostFtdcTradeField)
      {
        CThostFtdcTradeField ctpData = (CThostFtdcTradeField)field;
        return string.Format("{0}_{1}_{2}_{3}", ctpData.BrokerID, ctpData.InvestorID, ctpData.TradingDay, ctpData.TradeID);
      }

      //持仓
      if (field is CThostFtdcInvestorPositionField)
      {
        CThostFtdcInvestorPositionField ctpData = (CThostFtdcInvestorPositionField)field;
        return string.Format("{0}_{1}_{2}_{3}_{4}", ctpData.BrokerID, ctpData.InvestorID, ctpData.TradingDay, ctpData.InstrumentID, ctpData.PosiDirection);
      }

      //持仓明细
      if (field is CThostFtdcInvestorPositionDetailField)
      {
        //注意！！成交单ID可能重复
        CThostFtdcInvestorPositionDetailField ctpData = (CThostFtdcInvestorPositionDetailField)field;
        return string.Format("{0}_{1}_{2}_{3}", ctpData.BrokerID, ctpData.InvestorID, ctpData.TradingDay, ctpData.TradeID);
      }

      throw new Exception("Unknow type : " + field.GetType().ToString());
    }

  }
}
