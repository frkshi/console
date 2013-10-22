﻿using System;
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
    public partial class MainParent : Form
    {

        [DllImport(@"clayui_forcsharp.dll")]
        public static extern void CLAYUI_CSharp_Init(IntPtr handle);

        [DllImport(@"clayui_forcsharp.dll")]
        public static extern void CLAYUI_CSharp_Release();

        [DllImport(@"clayui_forcsharp.dll")]
        public static extern void CLAYUI_OnAnimation(IntPtr handle, int vert, int flag, int anitype, int invert);

        [DllImport(@"clayui_forcsharp.dll")]
        public static extern void Redraw(IntPtr handle, int usetime);

        [DllImport(@"clayui_forcsharp.dll")]
        public static extern int IsPlay();

        [DllImport(@"clayui_forcsharp.dll")]
        public static extern void CLAYUI_InitDialog2(IntPtr handle, IntPtr handle1);

        [DllImport(@"clayui_forcsharp.dll")]
        public static extern void MakeWindowTpt(IntPtr handle, int factor);

        [DllImport(@"clayui_forcsharp.dll")]
        public static extern void WinRedraw(IntPtr handle, int redraw);

        [DllImport(@"clayui_forcsharp.dll")]
        public static extern void desktomemdc1(IntPtr handle);


        public int m_isredraw = 1;
       
        private Dictionary<string, Form> _forms=new Dictionary<string,Form>();
        public MainParent()
        {
            InitializeComponent();
        }

        //public void RemoveChildForm(string formname)
        //{
        //    if (_forms.ContainsKey(formname))
        //    {
        //        _forms.Remove(formname);
        //    }
        //}
        private Form GetChildForm(string formname)
        {
            Form result = null;
            foreach (Form f in this.MdiChildren)
            {
                if (f.Name == formname)
                {
                    result = f;
                    break;

                }
            }

            if (result == null)
            {
                result = CreateForm(formname);
            }
            //if (_forms.ContainsKey(formname))
            //{
            //    result = _forms[formname];
            //}
            //else
            //{
            //    result = CreateForm(formname);
            //}

            return result;
        }

        private Form CreateForm(string formname)
        {
            Form result = null;

            switch (formname)
            {
                case "frm_MtuSetting":
                    result = new frm_MtuSetting();
                 
                    
                    break;
                case "frm_DeviceSetting":
                    result = new frm_DeviceSetting();
                    break;
                case "frm_deviceReg":
                    result = new frm_deviceReg();
                    break;
                case "frm_DataMonitor":
                    result = new frm_DataMonitor();
                
                    break;
                case "frm_CommunicationMonitor":
                    result = new frm_CommunicationMonitor();
                    break;
                default:
                    break;
            }
            result.MdiParent = this;

        
            return result;
        }
        private void ShouUI(Form frm)
        {
            IntPtr handle = this.Handle;
            IntPtr h1 = (IntPtr)0, h2 = (IntPtr)0;

            //frm.WindowState = FormWindowState.Maximized;
            frm.StartPosition = FormStartPosition.Manual;
            CLAYUI_OnAnimation((IntPtr)0, 0, 2, 1, 0);
            MainParent.Redraw(handle, 1);
            frm.Show();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {

            Form frm = GetChildForm("frm_CommunicationMonitor");

            ShouUI(frm);
        }

        private void OpenFile(object sender, EventArgs e)
        {
           
            Form frm = GetChildForm("frm_DataMonitor");


            ShouUI(frm);
            //IntPtr handle = this.Handle; 
            //IntPtr h1 = (IntPtr)0, h2 = (IntPtr)0;

            //frm.StartPosition = FormStartPosition.Manual;
            //CLAYUI_OnAnimation((IntPtr)0, 0, 2, 1, 0);
            //MainParent.Redraw(handle, 1);
            //frm.Show();
    
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = GetChildForm("frm_DeviceSetting");
            ShouUI(frm);
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = GetChildForm("frm_deviceReg");
            ShouUI(frm);
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (Form childForm in MdiChildren)
            //{
            //    childForm.Close();
            //}
            foreach (Form childform in _forms.Values)
            {
                childform.Close();
        
            }
            _forms.Clear();
        }

        private void fileMenu_Click(object sender, EventArgs e)
        {

        }

        private void mtuSettingMenuItem_Click(object sender, EventArgs e)
        {
     
            
            Form frm = GetChildForm("frm_MtuSetting");

            ShouUI(frm);
  

        }

        private void MainParent_Load(object sender, EventArgs e)
        {
            IntPtr handle = this.Handle;
           CLAYUI_CSharp_Init(handle);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            IntPtr handle = this.Handle;
            if (m_isredraw == 1)
                base.OnPaint(e);
        }

        public void EnableControl(int isenable)
        {
            foreach (System.Windows.Forms.Control control in this.Controls)
            {
               MainParent.WinRedraw(control.Handle, isenable);
            }
        }

        private void MainParent_FormClosed(object sender, FormClosedEventArgs e)
        {
            CLAYUI_CSharp_Release();
        }

        private void MainParent_Shown(object sender, EventArgs e)
        {
           
        }

        private void MainParent_ResizeEnd(object sender, EventArgs e)
        {

        }
    }
}
