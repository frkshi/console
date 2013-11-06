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
using log4net;
using MtuConsole.Common;

namespace MtuConsole
{
    public partial class frm_DeviceSetting : Form
    {
        int m_isredraw = 1;

        RWDatabase _rwdata;
        MainParent _parent;
        MtuLog _loger;
        public frm_DeviceSetting()
        {
            InitializeComponent();
        }

        public frm_DeviceSetting(RWDatabase rw, MainParent parent)
        {
            _loger = new MtuLog();
            _rwdata = rw;
            _parent = parent;
            InitializeComponent();
        }
        private void frm_DeviceSetting_Load(object sender, EventArgs e)
        {
            IntPtr handle = this.Handle;

            m_isredraw = 0;

            RefreshGrid();
        }

        private void RefreshGrid()
        {

            try
            {
                DataTable dt =ConvertToGrid( _rwdata.LocalSettingManager.LoadRTUSetting());
                dataGridView1.Value(dt);
            }
            catch(Exception e)
            {
                _loger.Error(e.ToString(), e);
            }

        }

        private DataTable ConvertToGrid(DataTable dt)
        {
            DataTable result = GetDataSchema();
            DataTable dtmeasure = _rwdata.LocalSettingManager.LoadMeasureSetting();
            foreach (DataRow dr in dt.Rows)
            {
                DataRow resultrow=result.NewRow();
                resultrow["rtuid"]=dr["rtuid"];
                resultrow["rtuname"]=dr["rtuname"];
                resultrow["savecycle"]=dr["savecycle"];
                resultrow["sendcycle"]=dr["sendcycle"];

                DataRow[] drs = dtmeasure.Select("rtuid='" + dr["rtuid"] + "' and datatype='01'");
                if (drs.Length > 0)
                {
                    resultrow["scale"] = drs[0]["scale"];
                    resultrow["offset"] = drs[0]["offset"];
                }
                result.Rows.Add(resultrow);
            }

            return result;
        }

        private DataTable GetDataSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("rtuid");
            dt.Columns.Add("rtuname");
            dt.Columns.Add("scale");
            dt.Columns.Add("offset");
            dt.Columns.Add("sendcycle",typeof(int));
            dt.Columns.Add("savecycle",typeof(int));
         
            return dt;
        }

        private void Save()
        { 
            RTUSetting rtusetting=new RTUSetting();
            _rwdata.LocalSettingManager.UpdateRTUSetting(rtusetting);

        }

        private void frm_DeviceSetting_Shown(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            DeviceEdit frm_deviceedit = new DeviceEdit(_rwdata);

            DialogResult result = frm_deviceedit.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                RefreshGrid();
            }


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 1 && dataGridView1.SelectedRows.Count > 0)
            {
                DeviceEdit frm_deviceedit = new DeviceEdit(_rwdata);
                DeviceParameter parameter = new DeviceParameter();
                parameter.rtuid = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                parameter.rtuname = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                parameter.savecycle = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                parameter.sendcycle = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                parameter.scale = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                parameter.offset= dataGridView1.SelectedRows[0].Cells[5].Value.ToString();


                frm_deviceedit.SetForm(parameter);
                DialogResult result = frm_deviceedit.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    RefreshGrid();
                }

            }
        }
    }
}
