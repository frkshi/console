

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace MtuConsole
{
    /// <summary>
    /// RTU状态枚举值
    /// </summary>
    public enum RtuStatusEnum
    {
        Normal,
        Error,
        Warning,
        Disable
    }

    public partial class RtuStatus : UserControl
    {
        private const string NORMAL = "Normal";
        private const string ERROR = "Error";
        private const string WARNING = "Warning";
        private const string DISABLE = "Disable";

        private List<StatusItem> _states = new List<StatusItem>();

        #region << ctor >>

        /// <summary>
        /// 构造函数
        /// </summary>
        public RtuStatus()
        {
            InitializeComponent();

            Initialize();
        }

        #endregion

        public string RtuId { get; set; }

        public string RtuName 
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public RtuStatusEnum State
        {
            get { return GetStatus(); }
            set { SetStatus(value); }
        }

        /// <summary>
        /// 设置ToolTip文本
        /// </summary>
        /// <param name="tipText"></param>
        public void SetToolTip(string tipText)
        {
            tipDetail.SetToolTip(this, tipText);
        }

        /// <summary>
        /// 获取或设置ToolTip的标题
        /// </summary>
        public string ToolTipTitle
        {
            get { return tipDetail.ToolTipTitle; }
            set { tipDetail.ToolTipTitle = value; }
        }

        /// <summary>
        /// 获取或设置ToolTip的图标
        /// </summary>
        public ToolTipIcon ToolTipIcon
        {
            get { return tipDetail.ToolTipIcon; }
            set { tipDetail.ToolTipIcon = value; }
        }

        /// <summary>
        /// 初始化状态
        /// </summary>
        private void Initialize()
        {
            System.Reflection.Assembly assembly = this.GetType().Assembly;
            System.IO.Stream stream = null;

            StatusItem sItem1 = new StatusItem(RtuStatus.NORMAL, Color.Green);
            stream = assembly.GetManifestResourceStream("SH3H.DataLog.MTUConsole.Images.Normal.jpg");
            sItem1.Image = new Bitmap(stream);
            statusIndicator1.StatusItems.Add(sItem1);

            StatusItem sItem2 = new StatusItem(RtuStatus.ERROR, Color.Red);
            stream = assembly.GetManifestResourceStream("SH3H.DataLog.MTUConsole.Images.Error.jpg");
            sItem2.Image = new Bitmap(stream);
            statusIndicator1.StatusItems.Add(sItem2);

            StatusItem sItem3 = new StatusItem(RtuStatus.WARNING, Color.Yellow);
            stream = assembly.GetManifestResourceStream("SH3H.DataLog.MTUConsole.Images.Warning.jpg");
            sItem3.Image = new Bitmap(stream);
            statusIndicator1.StatusItems.Add(sItem3);

            StatusItem sItem4 = new StatusItem(RtuStatus.DISABLE, Color.Gray);
            stream = assembly.GetManifestResourceStream("SH3H.DataLog.MTUConsole.Images.Disable.jpg");
            sItem4.Image = new Bitmap(stream);
            statusIndicator1.StatusItems.Add(sItem4);

            _states.AddRange(new StatusItem[] { sItem1, sItem2, sItem3, sItem4 });

            statusIndicator1.DisplayStyle = DisplayStyle.Image;
            statusIndicator1.Current = sItem1;
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="status">状态</param>
        private void SetStatus(RtuStatusEnum status)
        {
            string name = Enum.GetName(typeof(RtuStatusEnum), status);

            StatusItem tmp = _states.Single((item) => { return item.Name == name; });

            if (tmp == null)
                return;

            statusIndicator1.Current = tmp;
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        private RtuStatusEnum GetStatus()
        {
            return (RtuStatusEnum)Enum.Parse(typeof(RtuStatusEnum), statusIndicator1.Current.Name);
        }
    }
}
