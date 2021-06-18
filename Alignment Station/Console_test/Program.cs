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
        private static SerialPort T1Port;

        static void Main(string[] args)
        {
            T1Port = new SerialPort("COM23", 115200, Parity.None, 8, StopBits.One);
            T1Port.ReadTimeout = 200;
            T1Port.WriteTimeout = 200;
            T1Port.Open();

            string strx = "d";
            //string strx = "r axis0.encoder.shadow_count";
            Console.WriteLine(T1Talk(strx));
            T1Port.Close();
        }

        

        private static string T1Talk(string str)
        {
            string message = "";
            try
            {
                T1Port.WriteLine(str);
            }
            catch (Exception e)
            {
                return "Fail";
            }
            try
            {
                message = T1Port.ReadLine();
            }
            catch (Exception e)
            {
                return "fail";
            }
            return message;
        }




    } 
}
