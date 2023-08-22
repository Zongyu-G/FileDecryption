using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileDecryption
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            lb_Help.Text = "\r\n1、先创建1个内容全为数字0的加密的txt文本，至少有513个0；\r\n\r\n2、选择该文件，点击解析Keys，成功后再更新Keys即可。";
        }
    }
}
