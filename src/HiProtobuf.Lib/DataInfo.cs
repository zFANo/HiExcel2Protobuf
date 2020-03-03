using System.Collections.Generic;

namespace HiProtobuf.Lib
{
    internal static class DataInfo
    {
        public static Dictionary<string, Data> AllDataClassInfo = new Dictionary<string, Data>();

        internal struct Data
        {
            public Data(string pkgName, string listClzName, string dataClzName, string excelName, Dictionary<string, string> varType)
            {
                _pkgName = pkgName;
                _listClzName = listClzName;
                _dataClzName = dataClzName;
                _excelName = excelName;
                _varType = varType;
            }

            private string _pkgName;
            private string _listClzName;
            private string _dataClzName;
            private string _excelName;
            private Dictionary<string, string> _varType;

            public string PkgName => _pkgName;

            public string ListClzName => _listClzName;

            public string DataClzName => _dataClzName;

            public string ExcelName => _excelName;

            public Dictionary<string, string> VarType => _varType;
        }
    }
}