using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;
using System.Windows.Forms;
using DataAccess;
using DataEntity;
using MtuConsole.TcpProcess;
using FunctionLib;
namespace MtuConsole
{
    public partial class frm_MtuSetting : Form
    {
        int m_isredraw = 1;
        RWDatabase _rwdata;
        MainParent _parent;
        bool _communicationenable;
        public frm_MtuSetting()
        {
            InitializeComponent();
        }

        public frm_MtuSetting(RWDatabase rw,MainParent parent)
        {
            _rwdata = rw;
            _parent = parent;
            _communicationenable = parent.CommunicationEnable;
           

            InitializeComponent();

            ini();
        }

        /// <summary>
        /// 界面填充
        /// </summary>
        private void ini()
        { 
            CommunicationSetting entity=new CommunicationSetting();
            entity = _rwdata.LocalSettingManager.LoadCommunicationSetting("1");
            Txt_APN.Text = entity.APN;
            Txt_IP.Text = entity.IP;
            Txt_Port.Text = entity.ExTcp;
            txt_listenport.Text = ConfigureAppConfig.GetAppSettingsKeyValue("listenport");
            ResetBtnStart();
        }

        private void ResetBtnStart()
        {
            Btn_Start.Text = _communicationenable ? "关闭" : "启动";
        }
        private void frm_MtuSetting_Shown(object sender, EventArgs e)
        {
            //IntPtr handle = this.Handle;
            //IntPtr handle1 = this.MdiParent.Handle;// m_f1.Handle;

            //MainParent.CLAYUI_InitDialog2(handle, handle);

            //foreach (System.Windows.Forms.Control control in this.Controls)
            //{
            //    MainParent.WinRedraw(control.Handle, 0);
            //}

            //this.Location = new Point(0,0);
            //timer1.Start();
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

        protected override void OnPaint(PaintEventArgs e)
        {
            //IntPtr handle = this.Handle;
            //if (m_isredraw == 1)
            //    base.OnPaint(e);
        }

        private void Btn_Start_Click(object sender, System.EventArgs e)
        {
            try
            {
                _communicationenable = !_communicationenable;
                _parent.EnableCommunication(_communicationenable);
                ResetBtnStart();
            }
            catch
            {
                _communicationenable = !_communicationenable;
            }
        }

        private void Save()
        { 
           CommunicationSetting entity=new CommunicationSetting();
            entity.CommunicationId = 1;
            entity.APN=Txt_APN.Text;
            entity.IP=Txt_IP.Text;
            entity.ExTcp=Txt_Port.Text;
            entity.Dns1=string.Empty;
            entity.Dns2=string.Empty;
            entity.CommunicationName=string.Empty;
            entity.CommunicationName="1";
          
            
            _rwdata.LocalSettingManager.UpdateCommunicationSetting(entity);
        }

        private void Btn_Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {

            Save();
            this.Close();
        }

    }
}
