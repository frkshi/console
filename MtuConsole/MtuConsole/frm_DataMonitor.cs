using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace MtuConsole
{
    public partial class frm_DataMonitor : Form
    {

        int m_isredraw = 1;
        
        public delegate void HandleListShowMsg(ListMessage value);
        private HandleListShowMsg intertacShowMsg;
        
   
        public frm_DataMonitor()
        {
            InitializeComponent();
        }
      
        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {

                string copyStr = "时间:" + listView1.SelectedItems[0].SubItems[0].Text + "收发:" + listView1.SelectedItems[0].SubItems[1].Text + " 内容:" + listView1.SelectedItems[0].SubItems[2].Text;// +" 解包:" + listView1.SelectedItems[0].SubItems[2].Text;
                if (listView1.SelectedItems[0].SubItems.Count == 4)
                {
                    copyStr += " 解包:" + listView1.SelectedItems[0].SubItems[3].Text;
                }


                Clipboard.SetDataObject(copyStr, true);

            }
        }

        private void toolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
        private void ShowMsg(ListMessage msg)
        {
            try
            {
                string[] itemstring = new string[4] { msg.time.ToString("yyyy-MM-dd HH:mm:ss.fff"), msg.Direct, msg.Content, msg.Encode };

                ListViewItem myItem = new ListViewItem(itemstring);
                listView1.Items.Insert(0, myItem);
            }
            catch
            { 
            
            }
        }

       
        public void WriteMsg(ListMessage msg)
        {
            this.Invoke(intertacShowMsg, msg);
        }
        void button1_Click(object sender, System.EventArgs e)
        {
          

            WriteMsg(new ListMessage { Content = "fadfds", Direct = "收到", Encode = "压力=1.2Mpa", time = DateTime.Now });
        }



        private void frm_DataMonitor_Load(object sender, EventArgs e)
        {
            IntPtr handle = this.Handle;

          // MainParent.MakeWindowTpt(handle, 0);
            m_isredraw = 0;
            intertacShowMsg = new HandleListShowMsg(ShowMsg);
        }
      

        private void timer1_Tick(object sender, EventArgs e)
        {
            //IntPtr handle = this.Handle;
            //if (MainParent.IsPlay() == 0)
            //{
            //    timer1.Stop();
            //    foreach (System.Windows.Forms.Control control in this.Controls)
            //    {
            //        MainParent.WinRedraw(control.Handle, 1);
            //    }
            //    Update();
            //}
            //else
            //    MainParent.Redraw(handle, 1);
        }

        private void frm_DataMonitor_Shown(object sender, EventArgs e)
        {
            //IntPtr handle = this.Handle;
            //IntPtr handle1 = this.MdiParent.Handle;// m_f1.Handle;

            //MainParent.CLAYUI_InitDialog2(handle, handle);

            //foreach (System.Windows.Forms.Control control in this.Controls)
            //{
            //    MainParent.WinRedraw(control.Handle, 0);
            //}

            //this.Location = new Point(0, 0);
            //timer1.Start();

        

        }

      
        protected override void OnPaint(PaintEventArgs e)
        {
            IntPtr handle = this.Handle;
            if (m_isredraw == 1)
                base.OnPaint(e);
        }
       
        private void frm_DataMonitor_VisibleChanged(object sender, EventArgs e)
        {
          
        }

        private void frm_DataMonitor_Activated(object sender, EventArgs e)
        {

        }

        private void frm_DataMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}
