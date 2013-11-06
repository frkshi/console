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
    public class DeviceParameter
    {
        public string rtuid;
        public string rtuname;
        public string scale;
        public string offset;
        public string savecycle;
        public string sendcycle;
    }
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
        public void SetRtuID(string rtuid)
        {
            txt_rtuid.Enabled = true;
            txt_rtuid.Text = rtuid;
        }
        public void SetForm(DeviceParameter parameter)
        {
            txt_rtuid.Enabled = false;
            txt_rtuid.Text = parameter.rtuid;
            txt_rtuname.Text = parameter.rtuname;
            txt_savecycle.Text = parameter.savecycle;
            txt_sendcycle.Text = parameter.sendcycle;
            txt_scale.Text = parameter.scale;
            txt_offset.Text = parameter.offset;
        
        
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
                rtusetting.ProductTypeId = "Datalog";


                MeasureSetting measuresetting = new MeasureSetting();
                DataTable dt = _rwdata.LocalSettingManager.LoadMeasureSetting();
                DataRow[] drs= dt.Select("rtuid='" + rtusetting.RTUId + "' and datatype='01'");
                if (drs.Length > 0)
                {
                    measuresetting.MeasureId = Convert.ToInt32(drs[0]["measureid"].ToString());
                    measuresetting.RTUId = txt_rtuid.Text;
                    measuresetting.Scale = Convert.ToDecimal(txt_scale.Text);
                    measuresetting.Offset = Convert.ToDecimal(txt_offset.Text);



                    _rwdata.LocalSettingManager.UpdateRTUSetting(rtusetting);

                    _rwdata.LocalSettingManager.UpdateMeasureSetting(measuresetting);
                    result = true;
                }
                else
                {
                    result = Addnew(rtusetting);
                    
                }

                

            }
            catch (Exception e)
            {
                _loger.Error(e.ToString(), e);
                result = false;
            }
            return result;
        }



        private bool Addnew(RTUSetting rtusetting)
        {
            bool result = false;

            try
            {
              

                //成功插入新设备后，获取所属measuresetting
                if (_rwdata.LocalSettingManager.InsertRTUSetting(rtusetting))
                {
                    List<MeasureTemplet> mtlist = _rwdata.LocalSettingManager.LoadMeasureTemplet(rtusetting.ProductTypeId);
                    List<MeasureSetting> mslist = new List<MeasureSetting>();
                    foreach (MeasureTemplet mt in mtlist)
                    {
                        MeasureSetting tmpms = new MeasureSetting(mt);
                        tmpms.RTUId = rtusetting.RTUId;
                        mslist.Add(tmpms);

                    }
                    _rwdata.LocalSettingManager.BulkInsertMeasureSetting(mslist);

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
