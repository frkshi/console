using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MtuConsole
{
    public enum SwitchEnum 
    {
        OFF,
        ON
    }

    public partial class ColoredButton : Label
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ColoredButton()
        {
            InitializeComponent();

            this.FlatStyle = FlatStyle.Flat;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Switch = SwitchEnum.OFF;
            this.Color = Color.Red;
            this.AutoSize = false;
            this.Size = new Size(23, 23);
            this.Text = string.Empty;
            this.Click += new EventHandler(ToggleButton_Click);
        }

        private SwitchEnum _switchEnum = SwitchEnum.OFF;
        public SwitchEnum Switch 
        {
            get { return _switchEnum; }
            set 
            {
                _switchEnum = value;

                // switch changed 
                OnSwitchChanged();
            } 
        }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color 
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }

        private void ToggleButton_Click(object sender, EventArgs e)
        {
            Switch = Switch == SwitchEnum.OFF ? SwitchEnum.ON : SwitchEnum.OFF;

            OnSwitchChanged();
        }

        private void OnSwitchChanged()
        {
            switch (Switch)
            {
                case SwitchEnum.OFF:
                    this.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case SwitchEnum.ON:
                    this.BorderStyle = BorderStyle.Fixed3D;
                    break;
                default:
                    break;
            }
        }
    }
}
