using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataAccess;
using DataEntity;
using FunctionLib;
using log4net;
using MtuConsole.TcpProcess;
using MtuConsole.Common;

namespace MtuConsole
{
    public partial class DeviceEdit : Form
    {

        RWDatabase _rwdata;
        MainParent _parent;
        MtuLog _loger;
        public DeviceEdit()
        {
            InitializeComponent();
        }

        public DeviceEdit(RWDatabase rw)
        {
            _rwdata = rw;
            InitializeComponent();
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            Save();
           // this.Close();

        }
        /// <summary>
        /// 保存至localsetting
        /// </summary>
        private bool Save()
        {
            bool result = false;
            try
            {
                RTUSetting rtusetting = new RTUSetting();
                rtusetting.RTUId = txt_rtuid.Text;
                rtusetting.RTUName = txt_rtuname.Text;
                rtusetting.SaveCycle = Convert.ToInt32(txt_savecycle.Text);
                rtusetting.SendCycle = Convert.ToInt32(txt_sendcycle.Text);



                MeasureSetting measuresetting = new MeasureSetting();
                DataTable dt = _rwdata.LocalSettingManager.LoadMeasureSetting();
                DataRow[] drs= dt.Select("rtuid='" + rtusetting.RTUId + "' and datatype='01'");
                if (drs.Length > 0)
                {
                    measuresetting.MeasureId = Convert.ToInt32(drs[0]["measureid"].ToString());
                    measuresetting.RTUId = txt_rtuid.Text;
                    measuresetting.Scale = Convert.ToDecimal(txt_scale.Text);
                    measuresetting.Offset = Convert.ToDecimal(txt_offset.Text);
                    result = true;
                }
                else
                {
                   result= Addnew();
                    
                }

                

            }
            catch (Exception e)
            {
                _loger.Error(e.ToString(), e);
                result = false;
            }
            return result;
        }



        private bool Addnew()
        {
            bool result = false;

            try
            {
                RTUSetting rtusetting = new RTUSetting();

                //成功插入新设备后，获取所属measuresetting
                if (_rwdata.LocalSettingManager.InsertRTUSetting(rtusetting))
                {
                    DataTable dt = _rwdata.LocalSettingManager.LoadMeasureSetting();
                    DataRow[] drs = dt.Select("rtuid='" + rtusetting.RTUId + "' and datatype='01'");
                    if (drs.Length > 0)
                    {
                        string str_measureid = drs[0]["measureid"].ToString();
                        MeasureSetting measuresetting = new MeasureSetting();
                        measuresetting.RTUId = rtusetting.RTUId;
                        measuresetting.MeasureId = Convert.ToInt32(str_measureid);
                        measuresetting.Scale = Convert.ToDecimal(txt_scale.Text);
                        measuresetting.Offset = Convert.ToDecimal(txt_offset.Text);

                        _rwdata.LocalSettingManager.UpdateMeasureSetting(measuresetting);
                        result = true;

                    }
                }
            }
            catch (Exception e)
            {

                result = false;
                _loger.Error(e.ToString(), e);
            }

            return result;
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //this.Close();
        }

    }
}
