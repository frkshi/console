namespace MtuConsole
{
    partial class RtuStatus
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.tipDetail = new System.Windows.Forms.ToolTip(this.components);
            this.statusIndicator1 = new StatusIndicator();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "0001";
            // 
            // tipDetail
            // 
            this.tipDetail.AutomaticDelay = 0;
            this.tipDetail.AutoPopDelay = 20000;
            this.tipDetail.InitialDelay = 50;
            this.tipDetail.IsBalloon = true;
            this.tipDetail.ReshowDelay = 0;
            this.tipDetail.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tipDetail.ToolTipTitle = "RTU 详细信息";
            // 
            // statusIndicator1
            // 
            this.statusIndicator1.Current = null;
            this.statusIndicator1.DisplayStyle = DisplayStyle.Image;
            this.statusIndicator1.Location = new System.Drawing.Point(11, 5);
            this.statusIndicator1.Name = "statusIndicator1";
            this.statusIndicator1.Size = new System.Drawing.Size(25, 25);
            this.statusIndicator1.TabIndex = 0;
            // 
            // RtuStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusIndicator1);
            this.Name = "RtuStatus";
            this.Size = new System.Drawing.Size(123, 33);
            this.tipDetail.SetToolTip(this, "编号: 0001\r\n压力: 10023 (Mpa)");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusIndicator statusIndicator1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip tipDetail;
    }
}
