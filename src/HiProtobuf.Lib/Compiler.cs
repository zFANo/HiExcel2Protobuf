/****************************************************************************
 * Description: 
 * 
 * Document: https://github.com/hiramtan/HiProtobuf
 * Author: hiramtan@live.com
 * Modifier: zf-ano@163.com
 ****************************************************************************/

using System.IO;

namespace HiProtobuf.Lib
{
    internal class Compiler
    {
        public static readonly string DllName = "/ProtoBufData.dll";

        public Compiler()
        {
            var folder = Settings.Export_Folder + Settings.language_folder + Settings.csharp_dll_folder;
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
            Directory.CreateDirectory(folder);
        }

        public void Process()
        {
            var command = @"-target:library -out:{0} -reference:{1} -recurse:{2}\*.cs";
            var dllPath = Settings.Export_Folder + Settings.language_folder + Settings.csharp_dll_folder + DllName;
            var csharpFolder = Settings.Export_Folder + Settings.language_folder + Settings.csharp_folder;
            command = Settings.Compiler_Path + " " + string.Format(command, dllPath, Settings.Protobuf_Dll_Path, csharpFolder);
            Common.Cmd(command);
        }
    }
}
