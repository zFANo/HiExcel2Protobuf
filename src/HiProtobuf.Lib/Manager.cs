﻿/****************************************************************************
 * Description: 
 * 
 * Document: https://github.com/hiramtan/HiProtobuf
 * Author: hiramtan@live.com
 * Modifier: zf-ano@163.com
 ****************************************************************************/

using HiFramework.Log;

namespace HiProtobuf.Lib
{
    public static class Manager
    {
        public static void Export()
        {
            if (string.IsNullOrEmpty(Settings.Export_Folder))
            {
                Log.Error("导出文件夹未配置");
                return;
            }
            if (string.IsNullOrEmpty(Settings.Excel_Folder))
            {
                Log.Error("Excel文件夹未配置");
                return;
            }
            if (string.IsNullOrEmpty(Settings.Compiler_Path))
            {
                Log.Error("编译器路径未配置");
                return;
            }
            
            DataInfo.AllDataClassInfo.Clear();
            Log.Info("开始生成协议");
            new ProtoHandler().Process();
            Log.Info("生成协议结束");
            Log.Info("开始生成语言");
            new LanguageGenerater().Process();
            Log.Info("生成语言结束");
            Log.Info("开始编译语言");
            new Compiler().Process();
            Log.Info("编译语言结束");
            Log.Info("开始生成数据");
            new DataHandler().Process();
            Log.Info("生成数据结束");
        }
    }
}
