using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_test
{
    static class GlobalVar
    {
        public static string T1ComPortName = "COM60";
        public static string T2ComPortName = "COM26";
        public static string T3ComPortName = "COM23";
        public static byte fixtureNumber = 3;

        public static string errors = "";
        // Position in mm { x, y, z, Rx, Ry, Rz}
        //public static double[] position = { 0, 0, 140.4, 0, 0, 0 };
        public static double[] position = { 0, 0, 138, 0, 0, 0 };
        public static float loss = -50.0f;

    }
}
