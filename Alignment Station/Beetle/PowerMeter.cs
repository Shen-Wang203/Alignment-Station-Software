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
        private static MessageBasedSession mSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open("GPIB0::12::INSTR");
        private static double loss;
        private static readonly string readcmd = "READ" + Parameters.channel + ":POW?";
        //private static readonly string[] testPowerLines = System.IO.File.ReadAllLines(@"C:\Users\swang\Desktop\Alignment Stage\C# Software\Alignment Station\Beetle\powerread.txt");
        //private static int testlinenum = 0;


        public static double Read()
        {
            try
            {
                loss = double.Parse(mSession.Query(readcmd));
            }
            catch (Exception)
            {
                Parameters.errors = "Power Meter Error";
                Parameters.errorFlag = true;
            }
            if (loss > 10)
            {
                Thread.Sleep(50);
                loss = double.Parse(mSession.Query(readcmd));
                if (loss > 10)
                    loss = -90.0;
            }
            loss -= Parameters.lossReference;
            loss = Math.Round(loss, 4);
            Console.WriteLine($"Loss: {loss}");
            Parameters.Log($"Loss: {loss}");
            Parameters.loss = loss;
            return loss;
        }

        //public static double Read()
        //{
        //    Parameters.loss = -40.0;
        //    Thread.Sleep(30);
        //    return -40.0;
        //}

        //public static double Read()
        //{
        //    if (testlinenum > testPowerLines.Length - 1)
        //        loss = -0.27;
        //    else
        //    {
        //        string eachLine = testPowerLines[testlinenum];
        //        loss = double.Parse(eachLine.Substring(10));
        //        testlinenum += 1;
        //    }
        //    // loss = Math.Round(loss, 4);
        //    Console.WriteLine(loss);
        //    Parameters.Log(loss.ToString());
        //    Parameters.loss = loss;
        //    Thread.Sleep(50);
        //    return loss;
        //}
    }
}
