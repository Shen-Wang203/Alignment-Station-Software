using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http.Headers;
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
        public static bool errorFlag = false;
        // Position in mm { x, y, z, Rx, Ry, Rz}
        //public static double[] position = { 0, 0, 140.4, 0, 0, 0 };
        public static double[] position = { 0, 0, 138, 0, 0, 0 };
        public static double loss = -50.0f;

        public static string productName = "SM1xN";
        // Product type and its focal length (to be complete)
        // focal length can be get through productName: GlobalVar.product[GlobalVar.productName]
        public static Dictionary<string, float> product =
            new Dictionary<string, float>()
            {
                { "VOA", 0.05f },
                { "SM1xN", 0.2f },
                { "MM1xN", 0.14f },
                { "UWDM", 0.05f }
            };
        
        
    }
}
