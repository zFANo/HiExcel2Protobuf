using System;
using HiFramework.Log;

namespace HiProtobufCommandLine
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Log.LogHandler = new Logger();
            
            Log.OnInfo += (x) =>
            {
                textBox6.Text = Logger.Log;
            };
            Log.OnWarning += (x) =>
            {
                textBox6.Text = Logger.Log;
            };
            Log.OnError += (x) =>
            {
                textBox6.Text = Logger.Log;
            };
            
            Config.Load();
            
            Settings.Export_Folder = textBox1.Text;
            Settings.Excel_Folder = textBox2.Text;
            Settings.Compiler_Path = textBox5.Text;
            
            Log.Info("开始导出");
            Manager.Export();
            Log.Info("导出结束");
            Config.Save();
        }

        private static void PrintLog(string log)
        {
            Console.WriteLine(log);
        }
    }
}