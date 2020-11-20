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
            GlobalVar.pivotPoint = new float[4] { 0, 0, 42, 0 };
            BeetleAlignment BA = BeetleAlignment.GetInstance();
            BeetleControl.resetPosition = new double[6] { 0.0, 0.0, 141, 0.6, -1.0, 0.0 };

            BA.Run("global", backDistanceAfterSearching: 0.01);


            
        }

    } 
}
