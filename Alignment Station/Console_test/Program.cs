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
            //BeetleControl BS = new BeetleControl();
            //double[] P1 = { 0, 0, 140, 1, -0.5, 0 }; // { x, y, z, Rx, Ry, Rz }
            //BeetleControl.Calibration(); //Only run this at startup
            //Thread.Sleep(20000);
            //BS.GotoReset();
            //Thread.Sleep(3000);
            //BS.XMoveTo(-4);
            //Thread.Sleep(3000);
            //BS.YMoveTo(3);
            //Thread.Sleep(3000);
            //BS.ZMoveTo(140);
            //Thread.Sleep(3000);
            //BS.GotoPosition(P1);
            //Thread.Sleep(3000);
            //BS.GotoClose(); //Only run this if control box will be powered off
            //Thread.Sleep(3000);
            //Console.WriteLine(GlobalVar.errors);


            double a = 2.5964;
            Console.WriteLine(Math.Round(a, 1));
        }

    } 
}
