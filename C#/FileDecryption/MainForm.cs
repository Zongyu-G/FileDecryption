using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileDecryption
{
    public partial class MainForm : Form
    {
        public string path = null;
        private static bool m_lock = false;
        public static string m_SelectPath = "C:\\Users\\admin\\Desktop";
        public List<string> m_SelectFiles = new List<string>();
        private string m_ParamFile = "";
        public static byte[] m_Keys = new byte[256];
        public static byte[] keys = {
            86, 219, 48, 233, 113, 242, 179, 117, 35, 16, 197, 37, 49, 108, 212, 91, 71, 32, 232, 125, 145, 166, 70, 169, 207, 213, 1,
            239, 43, 98, 128, 9, 162, 168, 79, 99, 57, 55, 245, 132, 172, 199, 231, 69, 238, 66, 20, 202, 56, 164, 74, 120, 173, 183, 18, 158, 221, 105,
            51, 92, 76, 61, 97, 119, 170, 39, 129, 2, 203, 196, 188, 159, 103, 206, 68, 208, 149, 111, 17, 229, 63, 21, 11, 178, 90, 22, 31, 87, 23, 167,
            72, 26, 102, 116, 234, 176, 38, 54, 15, 153, 122, 223, 222, 131, 216, 44, 133, 161, 62, 67, 201, 160, 110, 25, 46, 12, 140, 210, 52, 135, 78,
            225, 124, 205, 73, 33, 24, 192, 193, 19, 252, 190, 34, 115, 84, 121, 137, 7, 141, 157, 253, 243, 146, 89, 215, 175, 209, 255, 53, 181, 226,
            40, 224, 165, 5, 118, 10, 80, 6, 227, 240, 177, 152, 85, 244, 186, 94, 151, 195, 47, 45, 156, 30, 123, 204, 155, 134, 42, 248, 14, 250, 182,
            136, 95, 228, 214, 200, 58, 36, 138, 251, 237, 235, 198, 59, 109, 75, 29, 28, 104, 139, 194, 211, 174, 150, 130, 148, 142, 187, 127, 220,
            163, 13, 101, 81, 60, 65, 126, 50, 246, 8, 236, 147, 0, 254, 143, 96, 191, 144, 217, 112, 241, 249, 4, 171, 107, 230, 100, 64, 247, 185, 83,
            114, 106, 3, 93, 184, 82, 218, 189, 41, 77, 180, 154, 88, 27
        };
        public static byte[] fHead = { 135, 125, 28, 20, 33, 3, 9, 254, 255 };

        //定义一个关联进度条时间刷新的委托
        public delegate void OnProcessStepHandle(int nStep);
        public static event OnProcessStepHandle OnProcessStepChangeEvent;

        public MainForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Icon768;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OnProcessStepChangeEvent += OnProcessStepChanged;
            LoadParamFile();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            IniOperation.WriteValue(m_ParamFile, "Setting", "SelectPath", m_SelectPath);
            IniOperation.WriteValue(m_ParamFile, "Setting", "Keys", KeysToString(m_Keys));
        }

        private void LoadParamFile()
        {
            m_ParamFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + this.GetType().Namespace + ".ini";
            m_Keys = keys;
            try
            {
                if (!File.Exists(m_ParamFile))
                {
                    File.Create(m_ParamFile);
                    Thread.Sleep(100);
                    IniOperation.WriteValue(m_ParamFile, "Setting", "SelectPath", "C:\\Users\\admin\\Desktop");
                    IniOperation.WriteValue(m_ParamFile, "Setting", "Keys", KeysToString(keys));
                }
                m_SelectPath = IniOperation.GetStringValue(m_ParamFile, "Setting", "SelectPath", "C:\\Users\\admin\\Desktop");
                m_Keys = KeysToBytes(IniOperation.GetStringValue(m_ParamFile, "Setting", "Keys", KeysToString(keys)));
            }
            catch (Exception ex)
            {
                m_ParamFile = "";
            }
        }

        private void lbFilePath_DoubleClick(object sender, EventArgs e)
        {
            GetKeysForm frm = new GetKeysForm();
            frm.Show();
        }

        /// <summary>
        /// 进度条时间刷新
        /// </summary>
        /// <param name="nStep"></param>
        public void OnProcessStepChanged(int nStep)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                if (nStep <= 100 && nStep >= 0)
                {
                    progressBar1.Value = nStep;
                    lb_progress.Text = string.Format("{0}%", nStep);
                    if (nStep == 0)
                    {

                    }
                    else if (nStep == 100)
                    {
                        m_lock = false;
                    }
                }
            });
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (m_lock)
                return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "所有文件|*.*";
            ofd.InitialDirectory = m_SelectPath;
            ofd.Title = "选择文件";
            ofd.Multiselect = true;
            DialogResult d = ofd.ShowDialog();
            if (d == DialogResult.Cancel)
                return;
            else
            {
                m_SelectFiles = ofd.FileNames.ToList();
                string str = "";
                m_SelectFiles.ForEach(delegate (string x) { str += string.Format("\"{0}\",", x); });
                str = str.TrimEnd(',');
                if (m_SelectFiles.Count > 1)
                    txtFilePath.Text = str;
                else
                    txtFilePath.Text = m_SelectFiles[0];
                ShowLog(string.Format("选择文件：{0}", str));
                m_SelectPath = Path.GetDirectoryName(m_SelectFiles[0]);
            }
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            txtMessage.SelectionStart = txtMessage.Text.Length - 1;
            txtMessage.ScrollToCaret();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            this.BeginInvoke((MethodInvoker)delegate
            {
                string[] dragData = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                txtFilePath.Text = path = dragData[0]; 
                m_SelectFiles.Add(txtFilePath.Text);
                btnDecryption.PerformClick();
            });
        }

        private void btnDecryption_Click(object sender, EventArgs e)
        {
            if (m_lock)
                return;
            m_lock = true;
            try
            {
                Task.Run(delegate
              {
                  bool isSame = true;
                  List<string> txtlist = (from string s in txtFilePath.Text.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries) where s != "," select s).ToList();
                  m_SelectFiles.ForEach(delegate (string x) { if (!txtlist.Contains(x)) isSame = false; });
                  if (!isSame)
                  {
                      if (string.IsNullOrEmpty(txtFilePath.Text))
                      {
                          m_lock = false;
                          return;
                      }
                      else if (txtFilePath.Text.ElementAt(0) == ' ')
                      {
                          m_lock = false;
                          return;
                      }
                      m_SelectFiles = txtlist;
                      ShowLog(string.Format("选择文件：{0}", txtFilePath.Text));
                  }
                  if (m_SelectFiles == null || m_SelectFiles.Count == 0)
                  {
                      m_lock = false;
                      return;
                  }
                  foreach (var s in m_SelectFiles)
                  {
                      if (!File.Exists(s))
                      {
                          ShowLog($"文件\"{s}\"不存在！请检测文件路径及文件名是否正确！");
                          //m_lock = false;
                          continue;
                      }
                      else
                      {
                          if (DecryptFile(s/*, cbHeadVerify.Checked*/))
                          {
                              if (!cbHeadVerify.Checked)
                                  ShowLog($"文件\"{s}\"解密完成！文件已保存至原文件路径，请查看解密文件是否正常。");
                              else
                                  ShowLog($"文件\"{s}\"解密完成！文件已保存至原文件路径。");
                          }
                          else if (!cbHeadVerify.Checked)
                          {
                              ShowLog($"文件\"{s}\"解密失败！");
                          }
                          else
                          {
                              ShowLog($"文件\"{s}\"解密失败！或尝试去数据头验证解密。");
                          }
                      }
                  }
                  m_lock = false;
              });
            }
            catch (Exception ex)
            {
                m_lock = false;
                MessageBox.Show(ex.Message);
            }
        }

        public static bool DecryptFile(string encrypFilePath, bool bWinform = true)
        {
            try
            {
                long length = new FileInfo(encrypFilePath).Length;
                string decryFile = Path.GetDirectoryName(encrypFilePath) + "\\" + Path.GetFileNameWithoutExtension(encrypFilePath) + "_out" + Path.GetExtension(encrypFilePath);
                using (FileStream fsread = new FileStream(encrypFilePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[1024 * 1024 * 5];
                    int r = 0, index = 0, j = 0;
                    long position = 0;
                    while (true)
                    {
                        List<byte> data = new List<byte>();
                        List<byte> decData = new List<byte>();
                        r = fsread.Read(buffer, 0, buffer.Length);
                        if (r == 0)
                        {
                            break;
                        }
                        data.AddRange(buffer.Take(r));

                        for (int i = 0; i < data.Count; i++)
                        {
                            if (index >= 512)
                            {
                                if (j == 256)
                                    j = 0;
                                decData.Add((byte)(data[i] ^ m_Keys[j]));
                                j++;
                            }
                            index++;
                        }
                        double step = (double)position / length;
                        OnProcessStepChangeEvent((int)(step * 100));
                        using (FileStream fswrite = new FileStream(decryFile, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            fswrite.Position = position;
                            fswrite.Write(decData.ToArray(), 0, decData.Count);
                            position = fswrite.Position;
                            fswrite.Close();
                        }
                    }
                    fsread.Close();
                }

                OnProcessStepChangeEvent(100);
                return true;
            }
            catch (Exception ex)
            {
                OnProcessStepChangeEvent(100);
                if (bWinform)
                    MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static string KeysToString(byte[] keys)
        {
            string str = "";
            if (keys != null && keys.Length > 0)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    str += keys[i].ToString();
                    if (i < keys.Length - 1)
                        str += "-";
                }
                return str;
            }
            else
                return "";
        }

        public static byte[] KeysToBytes(string key)
        {
            List<string> str = new List<string>();
            List<byte> keys = new List<byte>();
            if (key != null && key.Length > 0)
            {
                if (key.Contains("-"))
                {
                    str.AddRange(key.Split('-'));
                    foreach (string s in str)
                    {
                        keys.Add(Convert.ToByte(s));
                    }
                    return keys.ToArray();
                }
                else
                    return null;
            }
            else
                return null;
        }

        public void ShowLog(string strlog)
        {
            if (txtMessage.InvokeRequired)  //c#中禁止跨线程直接访问控件，InvokeRequired是为了解决这个问题而产生的,用一个异步执行委托
            {
                Action<string> actionDelegate = (x) => { this.txtMessage.Text += x.ToString() + "\r\n"; };
                // 或者
                // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                this.txtMessage.BeginInvoke(actionDelegate, strlog);
            }
            else
            {
                this.txtMessage.Text += strlog + "\r\n";
            }
        }
    }


    #region ini文件操作类

    public class IniOperation
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, [In][Out] char[] lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        public static string[] GetAllSectionNames(string iniFile)
        {
            uint num = 32767u;
            string[] result = new string[0];
            IntPtr intPtr = Marshal.AllocCoTaskMem((int)(num * 2));
            uint privateProfileSectionNames = GetPrivateProfileSectionNames(intPtr, num, iniFile);
            if (privateProfileSectionNames != 0)
            {
                string text = Marshal.PtrToStringAuto(intPtr, (int)privateProfileSectionNames).ToString();
                result = text.Split(new char[1], StringSplitOptions.RemoveEmptyEntries);
            }

            Marshal.FreeCoTaskMem(intPtr);
            return result;
        }

        public static string[] GetAllItems(string iniFile, string section)
        {
            uint num = 32767u;
            string[] result = new string[0];
            IntPtr intPtr = Marshal.AllocCoTaskMem((int)(num * 2));
            uint privateProfileSection = GetPrivateProfileSection(section, intPtr, num, iniFile);
            if (privateProfileSection != num - 2 || privateProfileSection == 0)
            {
                string text = Marshal.PtrToStringAuto(intPtr, (int)privateProfileSection);
                result = text.Split(new char[1], StringSplitOptions.RemoveEmptyEntries);
            }

            Marshal.FreeCoTaskMem(intPtr);
            return result;
        }

        public static string[] GetAllItemKeys(string iniFile, string section)
        {
            string[] result = new string[0];
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            char[] array = new char[10240];
            if (GetPrivateProfileString(section, null, null, array, 10240u, iniFile) != 0)
            {
                result = new string(array).Split(new char[1], StringSplitOptions.RemoveEmptyEntries);
            }

            array = null;
            return result;
        }

        public static string GetStringValue(string iniFile, string section, string key, string defaultValue)
        {
            string result = defaultValue;
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称(key)", "key");
            }

            StringBuilder stringBuilder = new StringBuilder(10240);
            if (GetPrivateProfileString(section, key, defaultValue, stringBuilder, 10240u, iniFile) != 0)
            {
                result = stringBuilder.ToString();
            }

            stringBuilder = null;
            return result;
        }

        public static bool WriteItems(string iniFile, string section, string items)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(items))
            {
                throw new ArgumentException("必须指定键值对", "items");
            }

            return WritePrivateProfileSection(section, items, iniFile);
        }

        public static bool WriteValue(string iniFile, string section, string key, string value)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }

            if (value == null)
            {
                throw new ArgumentException("值不能为null", "value");
            }

            return WritePrivateProfileString(section, key, value, iniFile);
        }

        public static bool DeleteKey(string iniFile, string section, string key)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }

            return WritePrivateProfileString(section, key, null, iniFile);
        }

        public static bool DeleteSection(string iniFile, string section)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            return WritePrivateProfileString(section, null, null, iniFile);
        }

        public static bool EmptySection(string iniFile, string section)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            return WritePrivateProfileSection(section, string.Empty, iniFile);
        }
    }

    #endregion
}
