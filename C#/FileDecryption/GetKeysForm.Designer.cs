namespace FileDecryption
{
    partial class GetKeysForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lb_EncryptFilePath = new System.Windows.Forms.Label();
            this.txt_EncryptFilePath = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_SelectEncryptFile = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_GetKeys = new System.Windows.Forms.Button();
            this.btn_UpdateKeys = new System.Windows.Forms.Button();
            this.btn_Help = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(12, 118);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(657, 294);
            this.txtMessage.TabIndex = 6;
            // 
            // lb_EncryptFilePath
            // 
            this.lb_EncryptFilePath.AutoSize = true;
            this.lb_EncryptFilePath.Location = new System.Drawing.Point(12, 21);
            this.lb_EncryptFilePath.Name = "lb_EncryptFilePath";
            this.lb_EncryptFilePath.Size = new System.Drawing.Size(112, 15);
            this.lb_EncryptFilePath.TabIndex = 7;
            this.lb_EncryptFilePath.Text = "加密文件路径：";
            // 
            // txt_EncryptFilePath
            // 
            this.txt_EncryptFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_EncryptFilePath.Location = new System.Drawing.Point(134, 16);
            this.txt_EncryptFilePath.Name = "txt_EncryptFilePath";
            this.txt_EncryptFilePath.Size = new System.Drawing.Size(433, 25);
            this.txt_EncryptFilePath.TabIndex = 9;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(134, 47);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(433, 25);
            this.textBox2.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "解密文件路径：";
            // 
            // btn_SelectEncryptFile
            // 
            this.btn_SelectEncryptFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SelectEncryptFile.Location = new System.Drawing.Point(573, 16);
            this.btn_SelectEncryptFile.Name = "btn_SelectEncryptFile";
            this.btn_SelectEncryptFile.Size = new System.Drawing.Size(92, 25);
            this.btn_SelectEncryptFile.TabIndex = 12;
            this.btn_SelectEncryptFile.Text = "选择文件";
            this.btn_SelectEncryptFile.UseVisualStyleBackColor = true;
            this.btn_SelectEncryptFile.Click += new System.EventHandler(this.btn_SelectEncryptFile_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(573, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 25);
            this.button1.TabIndex = 13;
            this.button1.Text = "选择文件";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btn_GetKeys
            // 
            this.btn_GetKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_GetKeys.Location = new System.Drawing.Point(461, 83);
            this.btn_GetKeys.Name = "btn_GetKeys";
            this.btn_GetKeys.Size = new System.Drawing.Size(92, 25);
            this.btn_GetKeys.TabIndex = 14;
            this.btn_GetKeys.Text = "解析Keys";
            this.btn_GetKeys.UseVisualStyleBackColor = true;
            this.btn_GetKeys.Click += new System.EventHandler(this.btn_GetKeys_Click);
            // 
            // btn_UpdateKeys
            // 
            this.btn_UpdateKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_UpdateKeys.Location = new System.Drawing.Point(573, 83);
            this.btn_UpdateKeys.Name = "btn_UpdateKeys";
            this.btn_UpdateKeys.Size = new System.Drawing.Size(92, 25);
            this.btn_UpdateKeys.TabIndex = 15;
            this.btn_UpdateKeys.Text = "更新Keys";
            this.btn_UpdateKeys.UseVisualStyleBackColor = true;
            this.btn_UpdateKeys.Click += new System.EventHandler(this.btn_UpdateKeys_Click);
            // 
            // btn_Help
            // 
            this.btn_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Help.Location = new System.Drawing.Point(349, 83);
            this.btn_Help.Name = "btn_Help";
            this.btn_Help.Size = new System.Drawing.Size(92, 25);
            this.btn_Help.TabIndex = 16;
            this.btn_Help.Text = "使用说明";
            this.btn_Help.UseVisualStyleBackColor = true;
            this.btn_Help.Click += new System.EventHandler(this.btn_Help_Click);
            // 
            // GetKeysForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 424);
            this.Controls.Add(this.btn_Help);
            this.Controls.Add(this.btn_UpdateKeys);
            this.Controls.Add(this.btn_GetKeys);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_SelectEncryptFile);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_EncryptFilePath);
            this.Controls.Add(this.lb_EncryptFilePath);
            this.Controls.Add(this.txtMessage);
            this.Name = "GetKeysForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "获取解密Keys";
            this.Load += new System.EventHandler(this.GetKeysForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lb_EncryptFilePath;
        private System.Windows.Forms.TextBox txt_EncryptFilePath;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_SelectEncryptFile;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_GetKeys;
        private System.Windows.Forms.Button btn_UpdateKeys;
        private System.Windows.Forms.Button btn_Help;
    }
}