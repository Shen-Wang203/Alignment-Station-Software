using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NationalInstruments.VisaNS;

namespace Console_test
{
    static class PowerMeter
    {
        private static MessageBasedSession mSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open("GPIB0::12::INSTR");
        private static double loss;

        public static double Read()
        {
            loss = double.Parse(mSession.Query("READ1:POW?"));
            if (loss > 10)
            {
                Thread.Sleep(50);
                loss = double.Parse(mSession.Query("READ1:POW?"));
                if (loss > 10)
                    loss = -90.0;
            }
            loss -= GlobalVar.lossReference;
            loss = Math.Round(loss, 4);
            Console.WriteLine(loss);
            GlobalVar.loss = loss;
            return loss;
        }
    }
}
