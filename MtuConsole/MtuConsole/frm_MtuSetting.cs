using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;
using System.Windows.Forms;

namespace MtuConsole
{
    public partial class frm_MtuSetting : Form
    {
        int m_isredraw = 1;
        public frm_MtuSetting()
        {
            InitializeComponent();
        }

        private void frm_MtuSetting_Shown(object sender, EventArgs e)
        {
            IntPtr handle = this.Handle;
            IntPtr handle1 = this.MdiParent.Handle;// m_f1.Handle;

            MainParent.CLAYUI_InitDialog2(handle, handle);

            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                MainParent.WinRedraw(control.Handle, 0);
            }

            this.Location = new Point(0,0);
            timer1.Start();
        }

        private void frm_MtuSetting_MdiChildActivate(object sender, EventArgs e)
        {

        }

        private void frm_MtuSetting_Activated(object sender, EventArgs e)
        {

        }

        private void frm_MtuSetting_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void frm_MtuSetting_Load(object sender, EventArgs e)
        {
            IntPtr handle = this.Handle;

            m_isredraw = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            IntPtr handle = this.Handle;
            if (MainParent.IsPlay() == 0)
            {
                timer1.Stop();
                foreach (System.Windows.Forms.Control control in this.Controls)
                {
                    MainParent.WinRedraw(control.Handle, 1);
                }
                Update();
            }
            else
                MainParent.Redraw(handle, 1);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            IntPtr handle = this.Handle;
            if (m_isredraw == 1)
                base.OnPaint(e);
        }
    }
}
