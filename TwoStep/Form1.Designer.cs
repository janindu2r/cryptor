namespace Two_Step_File_Protector
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnBrowseEncrypt = new System.Windows.Forms.Button();
            this.txtFileToEncrypt = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFileEncrypt = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFileDecrypt = new System.Windows.Forms.Button();
            this.btnBrowseDecrypt = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFileToDecrypt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(25, 315);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(305, 82);
            this.listBox1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(336, 315);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(96, 82);
            this.listBox3.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(47, 135);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(447, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(35, 38);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(175, 16);
            this.Label1.TabIndex = 19;
            this.Label1.Text = "Select the file you want to protect :";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBrowseEncrypt
            // 
            this.btnBrowseEncrypt.Location = new System.Drawing.Point(216, 36);
            this.btnBrowseEncrypt.Name = "btnBrowseEncrypt";
            this.btnBrowseEncrypt.Size = new System.Drawing.Size(72, 21);
            this.btnBrowseEncrypt.TabIndex = 18;
            this.btnBrowseEncrypt.Text = "Browse";
            this.btnBrowseEncrypt.Click += new System.EventHandler(this.btnBrowseEncrypt_Click);
            // 
            // txtFileToEncrypt
            // 
            this.txtFileToEncrypt.Location = new System.Drawing.Point(47, 67);
            this.txtFileToEncrypt.Name = "txtFileToEncrypt";
            this.txtFileToEncrypt.ReadOnly = true;
            this.txtFileToEncrypt.Size = new System.Drawing.Size(447, 20);
            this.txtFileToEncrypt.TabIndex = 17;
            this.txtFileToEncrypt.Text = "Click Browse to load file.";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(539, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(332, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "select the usb original where the file was put. Otherwise can\'t decrypt";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnFileEncrypt
            // 
            this.btnFileEncrypt.Enabled = false;
            this.btnFileEncrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFileEncrypt.Location = new System.Drawing.Point(103, 208);
            this.btnFileEncrypt.Name = "btnFileEncrypt";
            this.btnFileEncrypt.Size = new System.Drawing.Size(266, 32);
            this.btnFileEncrypt.TabIndex = 21;
            this.btnFileEncrypt.Text = "Encrypt and Copy to Drive";
            this.btnFileEncrypt.Click += new System.EventHandler(this.btnFileEncrypt_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(278, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "You wont be able to decrypt this file without this pen drive";
            // 
            // btnFileDecrypt
            // 
            this.btnFileDecrypt.Enabled = false;
            this.btnFileDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFileDecrypt.Location = new System.Drawing.Point(640, 208);
            this.btnFileDecrypt.Name = "btnFileDecrypt";
            this.btnFileDecrypt.Size = new System.Drawing.Size(249, 32);
            this.btnFileDecrypt.TabIndex = 32;
            this.btnFileDecrypt.Text = "Decrypt";
            this.btnFileDecrypt.Click += new System.EventHandler(this.btnFileDecrypt_Click);
            // 
            // btnBrowseDecrypt
            // 
            this.btnBrowseDecrypt.Location = new System.Drawing.Point(858, 38);
            this.btnBrowseDecrypt.Name = "btnBrowseDecrypt";
            this.btnBrowseDecrypt.Size = new System.Drawing.Size(72, 21);
            this.btnBrowseDecrypt.TabIndex = 31;
            this.btnBrowseDecrypt.Text = "Browse";
            this.btnBrowseDecrypt.Click += new System.EventHandler(this.btnBrowseDecrypt_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(658, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(175, 16);
            this.label4.TabIndex = 33;
            this.label4.Text = "Select the file you want to decrypt:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFileToDecrypt
            // 
            this.txtFileToDecrypt.Location = new System.Drawing.Point(640, 67);
            this.txtFileToDecrypt.Name = "txtFileToDecrypt";
            this.txtFileToDecrypt.ReadOnly = true;
            this.txtFileToDecrypt.Size = new System.Drawing.Size(447, 20);
            this.txtFileToDecrypt.TabIndex = 34;
            this.txtFileToDecrypt.Text = "Click Browse to load file.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 435);
            this.Controls.Add(this.txtFileToDecrypt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnFileDecrypt);
            this.Controls.Add(this.btnBrowseDecrypt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnFileEncrypt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.btnBrowseEncrypt);
            this.Controls.Add(this.txtFileToEncrypt);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.ComboBox comboBox1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button btnBrowseEncrypt;
        internal System.Windows.Forms.TextBox txtFileToEncrypt;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Button btnFileEncrypt;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Button btnFileDecrypt;
        internal System.Windows.Forms.Button btnBrowseDecrypt;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtFileToDecrypt;
    }
}

