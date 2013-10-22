using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using FunctionLib;
using System.IO;
using System.Xml.Schema;
using System.Reflection;
namespace MtuConsole
{
    public partial class frm_CommunicationMonitor : Form
    {
        //private ICommunicationService _communicationService = null;
        private IBasicDataService _basicDataService = null;

        private RtuModel _model = null;

        private IDictionary<string, Color> _dicColoredConfig = null;

        // 默认方案保存全路径
        private readonly string CONFIG_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "colored-config.xml");
        int m_isredraw = 1;
        private bool _refreshRtuStatus = false;
        public frm_CommunicationMonitor()
        {
            InitializeComponent();
        }
        #region << Private Fields >>

        private const string TIP_TITLE = "RTU 运行状态";

     
        private Dictionary<string, IRtuStatusView> _dicItems = new Dictionary<string, IRtuStatusView>();        

        private Color _currentColor = Color.Empty;

        #endregion

        #region << ctor >>

        

        #endregion

        #region << Form Event Handler >>

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_CommunicationMonitor_Load(object sender, EventArgs e)
        {
            IntPtr handle = this.Handle;

            m_isredraw = 0;
            _model = new RtuModel();

           
            _basicDataService = App.CurrentApp.GetService<IBasicDataService>();


            InitColors();
            LoadColorConfig();
            //InitRtuStatus();


            //_communicationService = App.CurrentApp.GetService<ICommunicationService>();
            //_communicationService.OnMessage += CommunicationService_OnMessage;
        }

        /// <summary>
        /// Form Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_CommunicationMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                //_basicDataService.RtusChanged -= BasicDataService_RtusChanged;
                //_communicationService.OnMessage -= CommunicationService_OnMessage;
                //_communicationService = null;
                //_model = null;
                //_view = null;
            }
        }

        /// <summary>
        /// 保存方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        /// <summary>
        /// 保存方案
        /// </summary>
        private void SaveConfig()
        {
            var childs = from it in this.Items
                         select new XElement("rtu-item",
                             new XAttribute("rtu-id", it.Id),
                             new XAttribute("color", it.Color.Name)
                         );

            XElement root = new XElement("root", childs);

            XDocument doc = new XDocument(new XDeclaration("1.0", "UTF-8", null), root);

            doc.Save(CONFIG_PATH, SaveOptions.None);
        }
        #endregion

        #region << IBaseView Members >>

        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        public void Message(string msg)
        {
            this.MessageEx(msg);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void CloseView()
        {
            this.CloseEx();
        }

        #endregion

        #region << IRtuMonitoringView Members >>

        /// <summary>
        /// 索引器,根据RtuId查找Rtu
        /// </summary>
        /// <param name="rutId"></param>
        /// <returns></returns>
        public IRtuStatusView this[string rutId]
        {
            get { return _dicItems[rutId]; }
        }

        public bool ContainsItem(string id)
        {
            return _dicItems.ContainsKey(id);
        }

        /// <summary>
        /// 获得所有RtuStatus项目
        /// </summary>
        public IEnumerable<IRtuStatusView> Items
        {
            get { return _dicItems.Values; }
        }

        /// <summary>
        /// 添加Rtu
        /// </summary>
        /// <param name="item">新的Rtu</param>
        public void AddItem(RtuItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (_dicItems.ContainsKey(item.Id))
                throw new Exception("RutItem Id已存在.");

            RtuStatus control = new RtuStatus();

            control.MouseDown += new MouseEventHandler(control_MouseDown);
            control.MouseMove += new MouseEventHandler(control_MouseMove);

            IRtuStatusView rView = new RtuStatusViewImpl(control) {
                Id = item.Id,
                Name = item.Name,
                State = RtuStatusEnum.Normal,
                TipTitle = TIP_TITLE,
                TipIcon = ToolTipIcon.Info
            };

            rView.Clear();

            //lypRtus.Controls.Add(control);
            Add2List(control);
            _dicItems.Add(item.Id, rView);
        }

        private void Add2List(RtuStatus control)
        {
            if (!lypRtus.InvokeRequired)
                lypRtus.Controls.Add(control);
            else
                lypRtus.Invoke(new Action<RtuStatus>(Add2List), control);
        }

        /// <summary>
        /// 清理RTU状态控件
        /// </summary>
        public void ClearItems() 
        {
            if (!lypRtus.InvokeRequired)
            {
                // -TODO : RTUMonitoring Clear RtuStatus controls
                _dicItems.Clear();

                for (int i = 0; i < lypRtus.Controls.Count; i++)
                    if (lypRtus.Controls[i] is RtuStatus)
                    {
                        (lypRtus.Controls[i] as RtuStatus).MouseDown -= control_MouseDown;
                        (lypRtus.Controls[i] as RtuStatus).MouseMove -= control_MouseMove;
                    }

                lypRtus.Controls.Clear();
            }
            else
                lypRtus.Invoke(new Action(ClearItems));
        }

        /// <summary>
        /// 当前颜色
        /// </summary>
        public Color CurrentColor
        {
            get { return _currentColor; }
            set
            {
                if (value != _currentColor)
                {
                    _currentColor = value;

                    foreach (var item in lypColoredItems.Controls)
                    {
                        if (item is ColoredButton)
                        {
                            ((ColoredButton)item).Switch =
                                ((ColoredButton)item).Color != _currentColor ? SwitchEnum.OFF : SwitchEnum.ON;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 增加颜色按钮
        /// </summary>
        /// <param name="color"></param>
        public void AddColored(Color color)
        {
            var cbItem = new ColoredButton()
            {
                Color = color,
                Switch = SwitchEnum.OFF
            };

            cbItem.Click += new EventHandler(ColoredButton_Click);

            lypColoredItems.Controls.Add(cbItem);
        }

        /// <summary>
        /// 清理颜色控件
        /// </summary>
        public void ClearColors() 
        {
            if (!lypColoredItems.InvokeRequired)
            {
                // -TODO : RTUMonitoring Clear Color Controls

                for (int i = 0; i < lypColoredItems.Controls.Count; i++)
                    if (lypColoredItems.Controls[i] is ColoredButton)
                        (lypColoredItems.Controls[i] as ColoredButton).Click -= ColoredButton_Click;

                lypColoredItems.Controls.Clear();
            }
            else
                lypColoredItems.Invoke(new Action(ClearColors));
        }

        #endregion
        
        #region << Private Methods >>

        private void control_MouseMove(object sender, MouseEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("mouse move");

            RtuStatus focusControl = lypRtus.GetChildAtPoint(e.Location) as RtuStatus;
            if (focusControl != null) 
                SetColor(focusControl, e);

            //SetColor(((RtuStatus)sender), e);
        }

        private void control_MouseDown(object sender, MouseEventArgs e)
        {
            SetColor(((RtuStatus)sender), e);
        }

        private void SetColor(RtuStatus c, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && CurrentColor != Color.Empty)
            {
                //System.Diagnostics.Debug.WriteLine("set color....");
                this[((RtuStatus)c).RtuId].Color = CurrentColor;
            }
        }

        private void ColoredButton_Click(object sender, EventArgs e)
        {
            ColoredButton obj = sender as ColoredButton;

            CurrentColor = obj.Switch == SwitchEnum.ON ? obj.Color : Color.Empty;

            //_persenter.SelectedColorChanged(obj.Color);
        }

        #endregion


        #region << Init >>

        /// <summary>
        /// 初始化颜色控件
        /// </summary>
        private void InitColors()
        {
            Color[] colors = new Color[9] { 
                Color.Silver, Color.Chocolate, Color.Orange, Color.Yellow, Color.GreenYellow, 
                Color.Cyan, Color.MediumBlue, Color.Purple, Color.Pink
            };

            // 初始化预置颜色
            colors.ForEach(c => this.AddColored(c));

            // 设置当前颜色
            this.CurrentColor = Color.Empty;
        }

        /// <summary>
        /// 加载颜色配置
        /// </summary>
        private void LoadColorConfig()
        {
            // 加载保存的方案
            _dicColoredConfig = LoadConfig();
        }

        /// <summary>
        /// 初始化RTUStatus控件
        /// </summary>
        private void InitRtuStatus()
        {
            // 初始化图表
            if (_model.Items.Count() > 0)
                _model.Items.ForEach((item) =>
                {
                    try
                    {
                        //只添加状态为启用的rtu
                        if (item.Status && !this.ContainsItem(item.Id))
                        {
                            this.AddItem(item);

                            // 默认为白色
                            this[item.Id].Color = _dicColoredConfig.ContainsKey(item.Id)
                                ? _dicColoredConfig[item.Id] : Color.White;
                        }
                    }
                    catch (Exception ex)
                    {
                       
                    }
                });
        }

        #endregion
        #region << Color Config >>

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, Color> LoadConfig()
        {
            IDictionary<string, Color> dic = new Dictionary<string, Color>();

            // 默认配置文件不存在，返回空。
            if (!File.Exists(CONFIG_PATH))
                return dic;

            XDocument doc = XDocument.Load(CONFIG_PATH);

            string errorMessage = string.Empty;
            if (!ValidateConfig(doc, out errorMessage))
            {
                this.Message("配置文件格式无效。\n\n错误提示：" + errorMessage);
                return dic;
            }

            XElement root = doc.Element("root");

            if (root != null)
            {
                IEnumerable<XElement> childs = root.Elements("rtu-item");
                childs.ForEach(it =>
                {
                    dic.Add(it.Attribute("rtu-id").Value,
                        Color.FromName(it.Attribute("color").Value));
                });
            }

            return dic;
        }

        /// <summary>
        /// 采用默认的xsd文件验证XDocment。
        /// </summary>
        /// <param name="doc">XDocment</param>
        /// <param name="errorMessage">错误消息。如果验证失败，通过此参数获得错误消息</param>
        /// <returns>返回验证是否成功</returns>
        private bool ValidateConfig(XDocument doc, out string errorMessage)
        {
            errorMessage = string.Empty;

            Assembly currentAssembly = this.GetType().Assembly;
            Stream stream = currentAssembly.GetManifestResourceStream("SH3H.DataLog.MTUConsole.color-config.xsd");  //得到架构文件

            XmlSchemaSet schemaSet = new XmlSchemaSet();
            // 加载Schema
            schemaSet.Add(null, System.Xml.XmlReader.Create(stream));

            bool hasError = false;
            string innerErrorMessage = string.Empty;

            // Validate
            doc.Validate(schemaSet, (s, e) =>
            {
                hasError = true;
                innerErrorMessage = e.Message;
            });

            errorMessage = innerErrorMessage;
            return !hasError;
        }

        #endregion

        private void button14_Click_1(object sender, EventArgs e)
        {
            SaveConfig();
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

        private void frm_CommunicationMonitor_Shown(object sender, EventArgs e)
        {
            IntPtr handle = this.Handle;
            IntPtr handle1 = this.MdiParent.Handle;// m_f1.Handle;

            MainParent.CLAYUI_InitDialog2(handle, handle);

            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                MainParent.WinRedraw(control.Handle, 0);
            }

            this.Location = new Point(0, 0);
            timer1.Start();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            IntPtr handle = this.Handle;
            if (m_isredraw == 1)
                base.OnPaint(e);
        }

      
    }
}
