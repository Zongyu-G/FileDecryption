using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileDecryption
{
    public partial class GetKeysForm : Form
    {
        private string m_EncryptFile = "";
        private string m_DecryptFile = "";
        public static byte[] m_Keys = null;

        public GetKeysForm()
        {
            InitializeComponent();
        }

        private void GetKeysForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_SelectEncryptFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "所有文件|*.*";
            ofd.InitialDirectory = MainForm.m_SelectPath;
            ofd.Title = "选择文件";
            DialogResult d = ofd.ShowDialog();
            if (d == DialogResult.Cancel)
                return;
            else
            {
                txt_EncryptFilePath.Text = m_EncryptFile = ofd.FileName;
                txtMessage.Text += string.Format("选择文件：{0}\r\n", m_EncryptFile);
                MainForm.m_SelectPath = Path.GetDirectoryName(m_EncryptFile);
            }
        }

        private void btn_GetKeys_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_EncryptFile))
            {
                txtMessage.Text += "请选择文件！\r\n";
                return;
            }
            if (!File.Exists(m_EncryptFile))
            {
                txtMessage.Text += "文件不存在！请检测文件路径及文件名是否正确！\r\n";
                return;
            }

            List<byte> data = new List<byte>();
            List<byte> encryptData = new List<byte>();

            using (FileStream fsread = new FileStream(m_EncryptFile, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[1024 * 1024 * 5];
                int r = 0;
                while (true)
                {
                    r = fsread.Read(buffer, 0, buffer.Length);
                    if (r == 0)
                        break;
                    data.AddRange(buffer.Take(r));
                }
            }
            encryptData = data.GetRange(512, data.Count - 512);
            m_Keys = GetKeys(encryptData.ToArray());
            if (m_Keys != null)
                txtMessage.Text += "Keys=" + MainForm.KeysToString(m_Keys) + "\r\n";

        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="data">加密的数据</param>
        /// <param name="cpData">对比的数据，默认值为48，字符‘0’</param>
        /// <returns></returns>
        private byte[] GetKeys(byte[] data, byte cpData = 48)
        {
            List<byte> listdata1 = new List<byte>();
            List<byte> listdata2 = new List<byte>();
            List<byte> keys = new List<byte>();

            if (data == null || data.Length < 512)
            {
                txtMessage.Text += "加密文件数据过小，小于1KB！\r\n";
                return null;
            }
            listdata1.AddRange(data.ToList().GetRange(0, 256));
            listdata2.AddRange(data.ToList().GetRange(256, 256));
            if (!listdata1.SequenceEqual(listdata2))
            {
                txtMessage.Text += "可能不支持该加密类型！\r\n";
                return null;
            }
            else
            {
                foreach (byte b in listdata1)
                {
                    keys.Add((byte)(b ^ cpData));
                }
            }
            txtMessage.Text += "解析成功，请更换keys尝试解密！\r\n";
            return keys.ToArray();
        }

        private void btn_UpdateKeys_Click(object sender, EventArgs e)
        {
            if (m_Keys != null && m_Keys.Length == 256)
            {
                MainForm.m_Keys = m_Keys;
                txtMessage.Text += "更新Keys完成！\r\n";
            }
            else
            {
                txtMessage.Text += "请先获取Keys！\r\n";
            }
        }

        private void btn_Help_Click(object sender, EventArgs e)
        {
            HelpForm frm = new HelpForm();
            frm.Show();
        }
    }
}
