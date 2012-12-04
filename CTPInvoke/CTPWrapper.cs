using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection;
using System.IO;

namespace CalmBeltFund.Trading.CTP
{
  internal class CTPWrapper
  {
    [DllImport("CTPWrapper.dll")]
    internal unsafe static extern int ProcessRequest(void* hTrader, int type, void* pReqData, int requestID);

    [DllImport("CTPWrapper.dll")]
    internal static extern int Process(IntPtr handle, int type, int p1, StringBuilder p2);

    /// <summary>
    /// 订阅行情
    /// </summary>
    /// <param name="hMarketData"></param>
    [DllImport("CTPWrapper.dll")]
    internal static extern void SubscribeMarketData(IntPtr hMarketDatachar, IntPtr[] ppInstrumentID, int nCount);

    /// <summary>
    /// 退订行情
    /// </summary>
    /// <param name="hMarketData"></param>
    [DllImport("CTPWrapper.dll")]
    internal static extern void UnSubscribeMarketData(IntPtr hMarketData, IntPtr[] ppInstrumentID, int nCount);


    [DllImport("CTPWrapper.dll")]
    internal static extern void SetOutputCallback(IntPtr hSpi, OutputCallback cb);

    public void OutputString(string msg)
    {
      Trace.WriteLine(msg);
      Trace.Flush();
    }
  }

}
