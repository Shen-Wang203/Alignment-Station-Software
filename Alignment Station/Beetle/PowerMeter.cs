using System;
using System.Threading;
using NationalInstruments.VisaNS;
using System.Windows.Forms;

namespace Beetle
{
    static class PowerMeter
    {
        // Power Meter paramters
        public static double loss = -50.0f;
        public static double lossReference = -19.4588f;
        public static byte addr = 12;
        public static byte channel = 2;

        private static MessageBasedSession mSession;
        private static string readcmd;

        //private static readonly string[] testPowerLines = System.IO.File.ReadAllLines(@"C:\Users\swang\Desktop\Alignment Stage\C# Software\Alignment Station\Beetle\powerread.txt");
        //private static int testlinenum = 0;
        //private static Random rnd = new Random();

        static PowerMeter()
        {
            Open();
        }

        public static void Open()
        {
            try
            {
                if (mSession == null)
                    mSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open("GPIB0::" + addr.ToString() + "::INSTR");
                readcmd = "READ" + channel.ToString() + ":POW?";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nFailed to Connect Power Meter");
            }
        }

        //unit can be either dBm or dB
        public static double Read(string unit = "dB")
        {
            try
            {
                if (mSession != null)
                    loss = double.Parse(mSession.Query(readcmd));
                else
                    loss = -90;
            }
            catch (Exception)
            {
                //if (Parameters.errors == "")
                //    Parameters.errors = "Power Meter Error\n";
                //Parameters.errorFlag = true;
                loss = -90;
            }
            if (loss > 10)
            {
                Thread.Sleep(50);
                try
                {
                    if (mSession != null)
                        loss = double.Parse(mSession.Query(readcmd));
                    else
                        loss = -90;
                }
                catch (Exception)
                {
                    //if (Parameters.errors == "")
                    //    Parameters.errors = "Power Meter Error\n";
                    //Parameters.errorFlag = true;
                    loss = -90;
                }
                if (loss > 10)
                    loss = -90.0;
            }
            if (loss < -90)
                loss = -90;
            if (unit == "dBm")
                return loss;
            loss -= lossReference;
            loss = Math.Round(loss, 4);
            Console.WriteLine($"Loss: {loss}");
            Parameters.Log($"Loss: {loss}");
            return loss;
        }

        public static double ReadNoPrint()
        {
            try
            {
                if (mSession != null)
                    loss = double.Parse(mSession.Query(readcmd));
                else
                    loss = -90;
            }
            catch (Exception)
            {
                //if (Parameters.errors == "")
                //    Parameters.errors = "Power Meter Error\n";
                //Parameters.errorFlag = true;
                loss = -90;
            }
            if (loss < -90 || loss > 10)
                loss = -90;
            loss -= lossReference;
            loss = Math.Round(loss, 4);
            return loss;
        }

        //public static double Read(string unit = "dB")
        //{
        //    if (testlinenum > testPowerLines.Length - 1)
        //        loss = -0.27;
        //    else
        //    {
        //        string eachLine = testPowerLines[testlinenum];
        //        loss = double.Parse(eachLine.Substring(10));
        //        testlinenum += 1;
        //    }
        //    loss += Parameters.lossReference;
        //    if (unit == "dBm")
        //        return loss;
        //    loss -= Parameters.lossReference;
        //    loss = Math.Round(loss, 4);
        //    Console.WriteLine($"Loss: {loss}");
        //    Parameters.Log($"Loss: {loss}");
        //    Parameters.loss = loss;
        //    Thread.Sleep(20);
        //    return loss;
        //}

        //public static double ReadNoPrint()
        //{
        //    loss = rnd.NextDouble() * Parameters.loss * 0.002 + Parameters.loss;
        //    loss = Math.Round(loss, 4);
        //    //Parameters.loss = loss;
        //    Thread.Sleep(20);
        //    return loss;
        //}

    }
}
