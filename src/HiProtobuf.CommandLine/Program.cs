using System;
using System.Collections.Generic;
using HiFramework.Log;
using HiProtobuf.Lib;

namespace HiProtobufCommandLine
{
    internal class Program
    {
        private const int INDEX_EXPORT = 0;
        private const int INDEX_EXCEL = 1;
        private const int INDEX_CSC = 2;
        private const int INDEX_MAX = 3;

        private const string PARAM_EXPORT_SHORT = "-o";
        private const string PARAM_EXCEL_SHORT = "-e";
        private const string PARAM_CSC_SHORT = "-c";
        
        private const string PARAM_EXPORT_LONG = "--out";
        private const string PARAM_EXCEL_LONG = "--excel";
        private const string PARAM_CSC_LONG = "--csc";
        
        private static string[] HELP_INFO =
        {
            "-o [ExportFolder] or --out [ExportFolder]",
            "param -e [ExcelFolder] or --excel [ExcelFolder]",
            "param -c [csc.exe Path] or --csc [csc.exe Path]"
        };

        private static Dictionary<string, int> ParamMap;

        static Program()
        {
            ParamMap = new Dictionary<string, int>();
            ParamMap.Add(PARAM_EXPORT_SHORT, INDEX_EXPORT);
            ParamMap.Add(PARAM_EXCEL_SHORT, INDEX_EXCEL);
            ParamMap.Add(PARAM_CSC_SHORT, INDEX_CSC);
            
            ParamMap.Add(PARAM_EXPORT_LONG, INDEX_EXPORT);
            ParamMap.Add(PARAM_EXCEL_LONG, INDEX_EXCEL);
            ParamMap.Add(PARAM_CSC_LONG, INDEX_CSC);
        }
        
        public static void Main(string[] args)
        {
            if (args.Length < 6)
            {
                return;
            }
            
            string[] config = new string[INDEX_MAX];

            for (int i = 0; i < args.Length; i++)
            {
                string s = args[i].ToLower().Trim();
                if (!ParamMap.ContainsKey(s))
                {
                    continue;
                }
                
                i++;
                int index = ParamMap[s];
                if (string.IsNullOrEmpty(config[index]))
                {
                    config[index] = args[i];
                }
            }

            bool paramErr = false;
            for (int i = 0; i < INDEX_MAX; i++)
            {
                if (string.IsNullOrEmpty(config[i]))
                {
                    Console.WriteLine($"\t{HELP_INFO} missing");
                    paramErr = true;
                }
            }

            if (paramErr)
            {
                Console.WriteLine($"HiProtobuf Export occue error.");
                return;
            }
            
            Log.LogHandler = new Logger();
            
            Log.OnInfo += (x) =>
            {
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] == null)
                    {
                        continue;
                    }
                    
                    string xx = x[i].ToString();
                    Console.WriteLine(xx);
                }
            };
            Log.OnWarning += (x) =>
            {
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] == null)
                    {
                        continue;
                    }
                    
                    string xx = x[i].ToString();
                    Console.WriteLine(xx);
                }
            };
            Log.OnError += (x) =>
            {
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] == null)
                    {
                        continue;
                    }
                    
                    string xx = x[i].ToString();
                    Console.WriteLine(xx);
                }
            };

            Settings.Export_Folder = config[INDEX_EXPORT];
            Settings.Excel_Folder = config[INDEX_EXCEL];
            Settings.Compiler_Path = config[INDEX_CSC];
            
            Log.Info("开始导出");
            Manager.Export();
            Log.Info("导出结束");
        }

        private static void PrintHelp()
        {
            Console.WriteLine("HiProtobuf Useage:");
            for (int i = 0; i < INDEX_MAX; i++)
            {
                Console.WriteLine($"\t{HELP_INFO[i]}");
            }
        }
    }
}