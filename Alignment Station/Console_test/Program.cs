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
            bool xEpoxySolid = false;
            bool usePiezo = false;
            double x = 645;
            if (!xEpoxySolid && ((usePiezo && !PiezoSteppingSearch()) || (!usePiezo && !AxisSteppingSearch())))
            {
                ushort dacValue;
                dacValue = (ushort)x;
                Console.WriteLine((dacValue & 0x0f00) >> 8);
            }
        }

        private static bool PiezoSteppingSearch()
        {
            Console.WriteLine("piezo");
            return false;
        }

        private static bool AxisSteppingSearch()
        {
            Console.WriteLine("step");
            return false;
        }


    } 
}
