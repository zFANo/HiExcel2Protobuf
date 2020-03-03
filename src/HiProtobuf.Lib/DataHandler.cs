/****************************************************************************
 * Description: 
 * 
 * Document: https://github.com/hiramtan/HiProtobuf
 * Author: hiramtan@live.com
 * Modifier: zf-ano@163.com
 ****************************************************************************/

using Google.Protobuf;
using Google.Protobuf.Collections;
using HiFramework.Assert;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace HiProtobuf.Lib
{
    internal class DataHandler
    {
        private Assembly _assembly;
        private object _listClzIns;
        public DataHandler()
        {
            var folder = Settings.Export_Folder + Settings.dat_folder;
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
            Directory.CreateDirectory(folder);
        }

        public void Process()
        {
            var dllPath = Settings.Export_Folder + Settings.language_folder + Settings.csharp_dll_folder + Compiler.DllName;
            _assembly = Assembly.LoadFrom(dllPath);
            var protoFolder = Settings.Export_Folder + Settings.proto_folder;
            string[] files = Directory.GetFiles(protoFolder, "*.proto", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                string protoPath = files[i];
                string protoName = Path.GetFileNameWithoutExtension(protoPath);
                DataInfo.Data clzData = DataInfo.AllDataClassInfo[protoName];
                _listClzIns = _assembly.CreateInstance($"{clzData.PkgName}.{clzData.ListClzName}");
                string excelPath = Settings.Excel_Folder + "/" + clzData.ExcelName + ".xlsx";
                ProcessData(excelPath, clzData);
            }
        }

        private void ProcessData(string path, DataInfo.Data clzData)
        {
            AssertThat.IsTrue(File.Exists(path), "Excel file can not find");
            var excelApp = new Application();
            var workbooks = excelApp.Workbooks.Open(path);
            try
            {
                int pageCount = workbooks.Sheets.Count;
                AssertThat.IsTrue(pageCount > 2, "Excel's page count MUST > 2");
                for (int pageIndex = 3; pageIndex <= workbooks.Sheets.Count; pageIndex++)
                {
                    var sheet = workbooks.Sheets[pageIndex];
                    AssertThat.IsNotNull(sheet, "Excel's sheet is null");
                    Worksheet worksheet = sheet as Worksheet;
                    AssertThat.IsNotNull(sheet, "Excel's worksheet is null");
                    var usedRange = worksheet.UsedRange;
                    int rowCount = usedRange.Rows.Count;
                    int colCount = usedRange.Columns.Count;
                    for (int rowIndex = 2; rowIndex <= rowCount; rowIndex++)
                    {
                        var excel_Type = _listClzIns.GetType();
                        var dataProp = excel_Type.GetProperty("List");
                        var dataIns = dataProp.GetValue(_listClzIns);
                        var dataType = dataProp.PropertyType;
                        var ins = _assembly.CreateInstance($"{clzData.PkgName}.{clzData.DataClzName}");
                        var addMethod = dataType.GetMethod("Add", new [] {ins.GetType()});
                        for (int columnIndex = 1; columnIndex <= colCount; columnIndex++)
                        {
                            var variableName = ((Range) usedRange.Cells[1, columnIndex]).Text.ToString();
                            if (string.IsNullOrEmpty(variableName))
                            {
                                continue;
                            }
                            var variableType = clzData.VarType[variableName];
                            var variableValue = ((Range) usedRange.Cells[rowIndex, columnIndex]).Text.ToString();
                            var insType = ins.GetType();
                            var fieldName = variableName + "_";
                            fieldName = fieldName.Substring(0, 1).ToLower() + fieldName.Substring(1);
                            FieldInfo insField =
                                insType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                            var value = GetVariableValue(variableType, variableValue);
                            insField.SetValue(ins, value);
                        }
                        addMethod.Invoke(dataIns, new[] {ins});
                    }
                }
                Serialize(_listClzIns);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                workbooks.Close();
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        object GetVariableValue(string type, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (type == Common.double_)
                    return (double) 0;
                if (type == Common.float_)
                    return (float) 0;
                if (type == Common.int32_)
                    return (int) 0;
                if (type == Common.int64_)
                    return (long) 0;
                if (type == Common.uint32_)
                    return (uint) 0;
                if (type == Common.uint64_)
                    return (ulong) 0;
                if (type == Common.sint32_)
                    return (int) 0;
                if (type == Common.sint64_)
                    return (long) 0;
                if (type == Common.fixed32_)
                    return (uint) 0;
                if (type == Common.fixed64_)
                    return (ulong) 0;
                if (type == Common.sfixed32_)
                    return (int) 0;
                if (type == Common.sfixed64_)
                    return (long) 0;
                if (type == Common.bool_)
                    return false;
                if (type == Common.string_)
                    return "";
                if (type == Common.bytes_)
                    return ByteString.Empty;
                if (type == Common.double_s)
                    return new RepeatedField<double>();
                if (type == Common.float_s)
                    return new RepeatedField<float>();
                if (type == Common.int32_s)
                    return new RepeatedField<int>();
                if (type == Common.int64_s)
                    return new RepeatedField<long>();
                if (type == Common.uint32_s)
                    return new RepeatedField<uint>();
                if (type == Common.uint64_s)
                    return new RepeatedField<ulong>();
                if (type == Common.sint32_s)
                    return new RepeatedField<int>();
                if (type == Common.sint64_s)
                    return new RepeatedField<long>();
                if (type == Common.fixed32_s)
                    return new RepeatedField<uint>();
                if (type == Common.fixed64_s)
                    return new RepeatedField<ulong>();
                if (type == Common.sfixed32_s)
                    return new RepeatedField<int>();
                if (type == Common.sfixed64_s)
                    return new RepeatedField<long>();
                if (type == Common.bool_s)
                    return new RepeatedField<bool>();
                if (type == Common.string_s)
                    return new RepeatedField<string>();
                AssertThat.Fail("Type error");
                return null;
            }
            if (type == Common.float_)
                return float.Parse(value);
            if (type == Common.int32_)
                return int.Parse(value);
            if (type == Common.int64_)
                return long.Parse(value);
            if (type == Common.uint32_)
                return uint.Parse(value);
            if (type == Common.uint64_)
                return ulong.Parse(value);
            if (type == Common.sint32_)
                return int.Parse(value);
            if (type == Common.sint64_)
                return long.Parse(value);
            if (type == Common.fixed32_)
                return uint.Parse(value);
            if (type == Common.fixed64_)
                return ulong.Parse(value);
            if (type == Common.sfixed32_)
                return int.Parse(value);
            if (type == Common.sfixed64_)
                return long.Parse(value);
            if (type == Common.bool_)
                return value == "1";
            if (type == Common.string_)
                return value.ToString();
            if (type == Common.bytes_)
                return ByteString.CopyFromUtf8(value.ToString());
            if (type == Common.double_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<double> newValue = new RepeatedField<double>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(double.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.float_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<float> newValue = new RepeatedField<float>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(float.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.int32_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<int> newValue = new RepeatedField<int>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(int.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.int64_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<long> newValue = new RepeatedField<long>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(long.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.uint32_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<uint> newValue = new RepeatedField<uint>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(uint.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.uint64_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<ulong> newValue = new RepeatedField<ulong>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(ulong.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.sint32_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<int> newValue = new RepeatedField<int>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(int.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.sint64_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<long> newValue = new RepeatedField<long>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(long.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.fixed32_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<uint> newValue = new RepeatedField<uint>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(uint.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.fixed64_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<ulong> newValue = new RepeatedField<ulong>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(ulong.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.sfixed32_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<int> newValue = new RepeatedField<int>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(int.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.sfixed64_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<long> newValue = new RepeatedField<long>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(long.Parse(datas[i]));
                }
                return newValue;
            }
            if (type == Common.bool_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<bool> newValue = new RepeatedField<bool>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(datas[i] == "1");
                }
                return newValue;
            }
            if (type == Common.string_s)
            {
                string data = value.Trim('"');
                string[] datas = data.Split('|');
                RepeatedField<string> newValue = new RepeatedField<string>();
                for (int i = 0; i < datas.Length; i++)
                {
                    newValue.Add(datas[i]);
                }
                return newValue;
            }
            AssertThat.Fail("Type error");
            return null;
        }

        void Serialize(object obj)
        {
            var type = obj.GetType();
            var path = Settings.Export_Folder + Settings.dat_folder + "/" + type.Name + ".bytes";
            using (var output = File.Create(path))
            {
                MessageExtensions.WriteTo((IMessage)obj, output);
            }
        }
    }
}