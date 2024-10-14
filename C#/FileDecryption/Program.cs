using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileDecryption
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }
            try
            {
                string m_ParamFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\FileDecryption.ini";
                MainForm.m_Keys = MainForm.KeysToBytes(IniOperation.GetStringValue(m_ParamFile, "Setting", "Keys", MainForm.KeysToString(MainForm.keys)));
                if (args != null && args.Length != 0)
                {
                    MainForm.OnProcessStepChangeEvent += Program.OnProcessStepChanged;
                    string str = null;
                    AllocConsole();
                    Console.Write(str);
                    if (MainForm.DecryptFile(args[0], false))
                    {
                        str = "解密完成！文件已保存至原文件路径。\r\n";
                        Console.Write(str);
                    }
                    else
                    {
                        str = "解密失败！\r\n";
                        Console.Write(str);
                    }
                    //}
                    Console.ReadKey();
                    FreeConsole();
                    return;
                }
            }
            catch (Exception ex)
            {
                AllocConsole();
                Console.Write("解密异常：" + ex.Message);
                FreeConsole();
                return;
            }
            //AllocConsole();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            //FreeConsole();
        }

        public static void OnProcessStepChanged(int nStep)
        {
            if (nStep <= 100 && nStep >= 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(string.Format("解密中.......{0}%\r\n", nStep));
            }
        }
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
