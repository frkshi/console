namespace MtuConsole
{
    partial class frm_CommunicationMonitor
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lypColoredItems = new System.Windows.Forms.FlowLayoutPanel();
            this.button14 = new System.Windows.Forms.Button();
            this.lypRtus = new System.Windows.Forms.FlowLayoutPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.lypRtus);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(911, 536);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "终端监控列表";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lypColoredItems);
            this.groupBox2.Controls.Add(this.button14);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(3, 473);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(905, 60);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "显示选项";
            // 
            // lypColoredItems
            // 
            this.lypColoredItems.Location = new System.Drawing.Point(16, 20);
            this.lypColoredItems.Margin = new System.Windows.Forms.Padding(0);
            this.lypColoredItems.Name = "lypColoredItems";
            this.lypColoredItems.Padding = new System.Windows.Forms.Padding(3);
            this.lypColoredItems.Size = new System.Drawing.Size(341, 30);
            this.lypColoredItems.TabIndex = 31;
            // 
            // button14
            // 
            this.button14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button14.Location = new System.Drawing.Point(812, 25);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(75, 23);
            this.button14.TabIndex = 28;
            this.button14.Text = "存储方案";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click_1);
            // 
            // lypRtus
            // 
            this.lypRtus.BackColor = System.Drawing.SystemColors.Control;
            this.lypRtus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lypRtus.Location = new System.Drawing.Point(3, 17);
            this.lypRtus.Name = "lypRtus";
            this.lypRtus.Size = new System.Drawing.Size(905, 450);
            this.lypRtus.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 2;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frm_CommunicationMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 536);
            this.Controls.Add(this.groupBox1);
            this.Name = "frm_CommunicationMonitor";
            this.Text = "通讯监控";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_CommunicationMonitor_FormClosing);
            this.Load += new System.EventHandler(this.frm_CommunicationMonitor_Load);
            this.Shown += new System.EventHandler(this.frm_CommunicationMonitor_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel lypRtus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel lypColoredItems;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Timer timer1;

    }
}