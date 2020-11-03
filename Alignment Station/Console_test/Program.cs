using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace Console_test
{
    class Program
    {

        static void Main(string[] args)
        {
            BeetleControl BS = new BeetleControl();
            double[] P1 = { 0, 0, 138, 0, 0, 0 };
            BeetleControl.Calibration();
            //BS.GotoPosition(P1);
            //Thread.Sleep(3000);
            //BS.XMoveTo(-4);
            //Thread.Sleep(3000);
            //BS.YMoveTo(3);
            //Thread.Sleep(3000);
            //BS.ZMoveTo(140);
            //Thread.Sleep(3000);
            //P1 = new double[] { 0, 0, 142, 0, 0, 0};
            //BS.GotoPosition(P1);
            //Thread.Sleep(3000);
            //BS.GotoReset();
            //Thread.Sleep(3000);
            //BS.GotoClose();

            Console.WriteLine(GlobalVar.errors);
        }

    } 
}
