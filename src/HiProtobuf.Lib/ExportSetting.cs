namespace HiProtobuf.Lib
{
    public class ExportSetting
    {
        public static ExportSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExportSetting();
                }

                return _instance;
            }
        }

        private static ExportSetting _instance;

        public bool ExportCs { get; set; }
        public bool ExportCpp { get; set; }
        public bool ExportGo { get; set; }
        public bool ExportJava { get; set; }
        public bool ExportPython { get; set; }

        public bool ExportData { get; set; }

        private ExportSetting()
        {
            ExportCs = false;
            ExportCpp = false;
            ExportGo = false;
            ExportJava = false;
            ExportPython = false;

            ExportData = false;
        }
    }
}