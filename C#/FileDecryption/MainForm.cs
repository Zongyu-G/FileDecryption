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
    public partial class MainForm : Form
    {
        public string path = null;
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

        public MainForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Icon768;
        }

        public static bool DecryptFile(string encrypFilePath, bool HeadVerify = true)
        {
            List<byte> data = new List<byte>();
            List<byte> decData = new List<byte>();
            string decryFile = Path.GetDirectoryName(encrypFilePath) + "\\" + Path.GetFileNameWithoutExtension(encrypFilePath) + "_out" + Path.GetExtension(encrypFilePath);
            using (FileStream fsread = new FileStream(encrypFilePath, FileMode.Open, FileAccess.Read))
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
            List<byte> head = data.GetRange(0, 26);
            //List<byte> temp = data.GetRange(26, 124);
            //int sum = 0;
            head.RemoveRange(3, 17);
            if (head.SequenceEqual(fHead.ToList()) || !HeadVerify)
            {
                //temp.ForEach(delegate (byte x) { if (x == 0) sum++; else sum--; });
                //if (sum != 124)
                //   return false;
                int j = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    if (i >= 512)
                    {
                        if (j == 256)
                            j = 0;
                        decData.Add((byte)(data[i] ^ keys[j]));
                        j++;
                    }
                }
                using (FileStream fswrite = new FileStream(decryFile, FileMode.Create, FileAccess.Write))
                {
                    fswrite.Write(decData.ToArray(), 0, decData.Count);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "所有文件|*.*";
            ofd.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ofd.Title = "选择文件";
            DialogResult d = ofd.ShowDialog();
            if (d == DialogResult.Cancel)
                return;
            else
            {
                txtFilePath.Text = path = ofd.FileName;
                txtMessage.Text += string.Format("选择文件：{0}\r\n", path);
            }
        }

        private void btnDecryption_Click(object sender, EventArgs e)
        {
            if (path != txtFilePath.Text)
            {
                if (path == null || path == string.Empty)
                    return;
                else if (path.ElementAt(0) == ' ')
                    return;
                path = txtFilePath.Text;
                txtMessage.Text += string.Format("选择文件：{0}\r\n", path);
            }
            if (path == null || path == string.Empty)
                return;
            if (!File.Exists(path))
            {
                txtMessage.Text += "文件不存在！请检测文件路径及文件名是否正确！\r\n";
                return;
            }
            else
            {
                if (DecryptFile(path, cbHeadVerify.Checked))
                {
                    if (!cbHeadVerify.Checked)
                        txtMessage.Text += "解密完成！文件已保存至原文件路径，请查看解密文件是否正常。\r\n";
                    else
                        txtMessage.Text += "解密完成！文件已保存至原文件路径。\r\n";
                }
                else if (!cbHeadVerify.Checked)
                {
                    txtMessage.Text += "解密失败！\r\n";
                }
                else
                {
                    txtMessage.Text += "解密失败！或尝试去数据头验证解密。\r\n";
                }
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
            string[] dragData = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            txtFilePath.Text = path = dragData[0];
            btnDecryption.PerformClick();
        }
    }
}
