﻿namespace MtuConsole
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
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 2;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frm_MtuSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "frm_MtuSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "主台设置";
            this.Activated += new System.EventHandler(this.frm_MtuSetting_Activated);
            this.Load += new System.EventHandler(this.frm_MtuSetting_Load);
            this.MdiChildActivate += new System.EventHandler(this.frm_MtuSetting_MdiChildActivate);
            this.Shown += new System.EventHandler(this.frm_MtuSetting_Shown);
            this.VisibleChanged += new System.EventHandler(this.frm_MtuSetting_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
    }
}