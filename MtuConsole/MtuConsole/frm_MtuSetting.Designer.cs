namespace MtuConsole
{
    partial class frm_MtuSetting
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Btn_Start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Txt_Port = new System.Windows.Forms.TextBox();
            this.Txt_IP = new System.Windows.Forms.TextBox();
            this.Txt_APN = new System.Windows.Forms.TextBox();
            this.Btn_Quit = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_listenport = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 2;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Btn_Start
            // 
            this.Btn_Start.Location = new System.Drawing.Point(298, 178);
            this.Btn_Start.Name = "Btn_Start";
            this.Btn_Start.Size = new System.Drawing.Size(62, 23);
            this.Btn_Start.TabIndex = 0;
            this.Btn_Start.Text = "监听";
            this.Btn_Start.UseVisualStyleBackColor = true;
            this.Btn_Start.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "APN:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "端口:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Txt_Port);
            this.groupBox1.Controls.Add(this.Txt_IP);
            this.groupBox1.Controls.Add(this.Txt_APN);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 145);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备通讯参数";
            // 
            // Txt_Port
            // 
            this.Txt_Port.Location = new System.Drawing.Point(61, 86);
            this.Txt_Port.Name = "Txt_Port";
            this.Txt_Port.Size = new System.Drawing.Size(208, 21);
            this.Txt_Port.TabIndex = 8;
            // 
            // Txt_IP
            // 
            this.Txt_IP.Location = new System.Drawing.Point(61, 53);
            this.Txt_IP.Name = "Txt_IP";
            this.Txt_IP.Size = new System.Drawing.Size(208, 21);
            this.Txt_IP.TabIndex = 7;
            // 
            // Txt_APN
            // 
            this.Txt_APN.Location = new System.Drawing.Point(61, 20);
            this.Txt_APN.Name = "Txt_APN";
            this.Txt_APN.Size = new System.Drawing.Size(208, 21);
            this.Txt_APN.TabIndex = 6;
            // 
            // Btn_Quit
            // 
            this.Btn_Quit.Location = new System.Drawing.Point(285, 244);
            this.Btn_Quit.Name = "Btn_Quit";
            this.Btn_Quit.Size = new System.Drawing.Size(75, 23);
            this.Btn_Quit.TabIndex = 5;
            this.Btn_Quit.Text = "退出";
            this.Btn_Quit.UseVisualStyleBackColor = true;
            this.Btn_Quit.Click += new System.EventHandler(this.Btn_Quit_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(206, 244);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 6;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "内部端口:";
            // 
            // txt_listenport
            // 
            this.txt_listenport.Location = new System.Drawing.Point(73, 178);
            this.txt_listenport.Name = "txt_listenport";
            this.txt_listenport.Size = new System.Drawing.Size(208, 21);
            this.txt_listenport.TabIndex = 9;
            // 
            // frm_MtuSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 291);
            this.Controls.Add(this.txt_listenport);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.Btn_Quit);
            this.Controls.Add(this.Btn_Start);
            this.Controls.Add(this.groupBox1);
            this.Name = "frm_MtuSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "主台设置";
            this.Activated += new System.EventHandler(this.frm_MtuSetting_Activated);
            this.Load += new System.EventHandler(this.frm_MtuSetting_Load);
            this.MdiChildActivate += new System.EventHandler(this.frm_MtuSetting_MdiChildActivate);
            this.Shown += new System.EventHandler(this.frm_MtuSetting_Shown);
            this.VisibleChanged += new System.EventHandler(this.frm_MtuSetting_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       
        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button Btn_Start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox Txt_Port;
        private System.Windows.Forms.TextBox Txt_IP;
        private System.Windows.Forms.TextBox Txt_APN;
        private System.Windows.Forms.Button Btn_Quit;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_listenport;
    }
}