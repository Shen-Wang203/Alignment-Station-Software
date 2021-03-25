using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NationalInstruments.VisaNS;

namespace Beetle
{
    static class PowerMeter
    {
        private static double loss;
        //private static MessageBasedSession mSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open("GPIB0::12::INSTR");
        //private static readonly string readcmd = "READ" + Parameters.channel + ":POW?";
        private static readonly string[] testPowerLines = System.IO.File.ReadAllLines(@"C:\Users\swang\Desktop\Alignment Stage\C# Software\Alignment Station\Beetle\powerread.txt");
        private static int testlinenum = 0;


        // unit can be either dBm or dB
        //public static double Read(string unit = "dB")
        //{
        //    try
        //    {
        //        loss = double.Parse(mSession.Query(readcmd));
        //    }
        //    catch (Exception)
        //    {
        //        Parameters.errors = "Power Meter Error";
        //        Parameters.errorFlag = true;
        //    }
        //    if (loss > 10)
        //    {
        //        Thread.Sleep(50);
        //        loss = double.Parse(mSession.Query(readcmd));
        //        if (loss > 10)
        //            loss = -90.0;
        //    }
        //    if (unit == "dBm")
        //        return loss;
        //    loss -= Parameters.lossReference;
        //    loss = Math.Round(loss, 4);
        //    Console.WriteLine($"Loss: {loss}");
        //    Parameters.Log($"Loss: {loss}");
        //    Parameters.loss = loss;
        //    return loss;
        //}

        //public static void ReadNoPrint()
        //{
        //    try
        //    {
        //        loss = double.Parse(mSession.Query(readcmd));
        //    }
        //    catch (Exception)
        //    {
        //        Parameters.errors = "Power Meter Error";
        //        Parameters.errorFlag = true;
        //    }
        //    if (loss > 10)
        //    {
        //        Thread.Sleep(50);
        //        loss = double.Parse(mSession.Query(readcmd));
        //        if (loss > 10)
        //            loss = -90.0;
        //    }
        //    loss -= Parameters.lossReference;
        //    loss = Math.Round(loss, 4);
        //    Parameters.loss = loss;
        //}

        public static double Read(string unit = "dB")
        {
            if (testlinenum > testPowerLines.Length - 1)
                loss = -0.27;
            else
            {
                string eachLine = testPowerLines[testlinenum];
                loss = double.Parse(eachLine.Substring(10));
                testlinenum += 1;
            }
            loss += Parameters.lossReference;
            if (unit == "dBm")
                return loss;
            loss -= Parameters.lossReference;
            loss = Math.Round(loss, 4);
            Console.WriteLine($"Loss: {loss}");
            Parameters.Log($"Loss: {loss}");
            Parameters.loss = loss;
            Thread.Sleep(20);
            return loss;
        }
    }
}
