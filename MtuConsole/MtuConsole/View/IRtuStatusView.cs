
using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Drawing;

namespace MtuConsole
{
    public interface IRtuStatusView
    {
        /// <summary>
        /// RTU ID
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// RTU Name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        RtuStatusEnum State { get; set; }

        /// <summary>
        /// Tip Title
        /// </summary>
        string TipTitle { get; set; }

        /// <summary>
        /// Tip Icon
        /// </summary>
        ToolTipIcon TipIcon { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// 接收到Measure检测量报告.
        /// </summary>
        /// <param name="item"></param>
        void Measure(MeasureMessage item);

        /// <summary>
        /// 清理
        /// </summary>
        void Clear();

        /// <summary>
        /// 警告报告
        /// </summary>
        /// <param name="item"></param>
        void Alert(AlertMessage item);
    }
}