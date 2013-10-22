using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace MtuConsole
{
    public class RtuStatusViewImpl:IRtuStatusView 
    {
        #region << Private Fields >>

        private RtuStatus _control = null;
        private Dictionary<string,MeasureMessage> _measureValues = new Dictionary<string, MeasureMessage>();

        private const string TIP_HEADER = "编号: {0}\r\n名称: {1}\r\n";
        private const string TIP_MEASURE_ITEM = "{0}: [T={1}, V={2}]\r\n";
        private const string TIP_LIMIT_ALERT = "{0}: [T={1}, V={2}]\r\n";
        private const string TIP_LIMIT_MUTATION = "{0}: [T={1}, V1={2}, V2={3}]\r\n";

        #endregion

        #region << ctor >>

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="control"></param>
        public RtuStatusViewImpl(RtuStatus control)
        {
            _control = control;
        }

        #endregion

        #region << IRtuStatusView Members >>

        /// <summary>
        /// RTU ID
        /// </summary>
        public string Id 
        {
            get { return _control.RtuId; }
            set { _control.RtuId = value; } 
        }

        /// <summary>
        /// RTU Name
        /// </summary>
        public string Name
        {
            get { return _control.RtuName; }
            set { _control.RtuName = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public RtuStatusEnum State
        {
            get { return GetState(); }
            set { SetState(value); }
        }

        /// <summary>
        /// Tip 标题
        /// </summary>
        public string TipTitle
        {
            get { return _control.ToolTipTitle; }
            set { _control.ToolTipTitle = value; }
        }

        /// <summary>
        /// Tip 图标
        /// </summary>
        public ToolTipIcon TipIcon
        {
            get { return _control.ToolTipIcon; }
            set { _control.ToolTipIcon = value; }
        }

        /// <summary>
        /// 背景色
        /// </summary>
        public System.Drawing.Color Color
        {
            get { return _control.BackColor; }
            set { _control.BackColor = value; }
        }

        /// <summary>
        /// 检测量数据
        /// </summary>
        public void Measure(MeasureMessage message)
        {
            // TODO : (RtuStatus) Message Render Refactor

            if (string.IsNullOrEmpty(message.Id) || string.IsNullOrEmpty(message.Id.Trim()))
                return;

            if (_measureValues.ContainsKey(message.Id))
                _measureValues[message.Id] = message;
            else
                _measureValues.Add(message.Id, message);

            State = RtuStatusEnum.Normal;

            SetToolTip(RebuildMeasureMessage());
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        public void Alert(AlertMessage message)
        {
            // TODO : (RtuStatus) Message Render Refactor

            State = RtuStatusEnum.Warning;

            string body = string.Empty;

            if (message is LimitAlertMessage)
            {
                body = string.Format(TIP_LIMIT_ALERT, message.Name,
                    message.Time, ((LimitAlertMessage)message).Value1);
            }
            else
            {
                body = string.Format(TIP_LIMIT_MUTATION, message.Name,
                    message.Time, ((MutationAlertMessage)message).Value1,
                    ((MutationAlertMessage)message).Value2);
            }

            SetToolTip(Header + body);
        }

        public void Clear()
        {
            State = RtuStatusEnum.Normal;
            SetToolTip(Header);
        }

        #endregion

        #region << Private Property >>

        private string Header
        {
            get { return string.Format(TIP_HEADER, Id, Name); }
        }

        private void SetToolTip(string tooltip)
        {
            if (!_control.InvokeRequired)
                _control.SetToolTip(tooltip);
            else
                _control.Invoke(new Action<string>(SetToolTip), tooltip);
        }

        private RtuStatusEnum GetState()
        {
            if (!_control.InvokeRequired)
                return _control.State;
            else
                return (RtuStatusEnum)_control.Invoke(new Func<RtuStatusEnum>(GetState));
        }

        private void SetState(RtuStatusEnum state)
        {
            if (!_control.InvokeRequired)
                _control.State = state;
            else
                _control.Invoke(new Action<RtuStatusEnum>(SetState), state);
        }

        private string RebuildMeasureMessage() 
        {
            StringBuilder sbBody = new StringBuilder();
            sbBody.Append(Header);

            foreach (string key in _measureValues.Keys)
            {
                MeasureMessage message = _measureValues[key];

                sbBody.Append(string.Format(TIP_MEASURE_ITEM,
                        message.Name, message.Time, message.Value));
            }

            return sbBody.ToString();
        }

        #endregion
    }
}
