using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace FunctionLib
{
    /// <summary>
    /// 提供基本控件的默认值，get,set方法。使这些控件支持跨线程安全操作。
    /// </summary>
    public static partial class ControlHelper
    {
        #region << Get, Set Default Property Value >>

        /// <summary>
        /// get label control's text value
        /// </summary>
        /// <param name="control">control</param>
        /// <returns></returns>
        public static string Value(this Label control)
        {
            if (!control.InvokeRequired)
                return control.Text.Trim();
            else
                return (string)control.Invoke(new Func<Label, string>(Value), control);
        }

        /// <summary>
        /// set label control's text value
        /// </summary>
        /// <param name="control">control</param>
        /// <param name="value">new value</param>
        public static void Value(this Label control, string value)
        {
            if (!control.InvokeRequired)
                control.Text = value;
            else
                control.Invoke(new Action<Label, string>(Value), control, value);
        }

        /// <summary>
        /// get TextBoxBase control's text value
        /// </summary>
        /// <param name="control">control</param>
        /// <returns></returns>
        public static string Value(this TextBoxBase control)
        {
            if (!control.InvokeRequired)
                return control.Text.Trim();
            else
                return (string)control.Invoke(new Func<TextBoxBase, string>(Value), control);
        }

        /// <summary>
        /// set TextBoxBase control's Text value
        /// </summary>
        /// <param name="control">control</param>
        /// <param name="value">new value</param>
        public static void Value(this TextBoxBase control, string value)
        {
            if (!control.InvokeRequired)
                control.Text = value;
            else
                control.Invoke(new Action<TextBoxBase, string>(Value), control, value);
        }

        /// <summary>
        /// get CheckBox control's Checked value
        /// </summary>
        /// <param name="control">control</param>
        /// <returns></returns>
        public static bool Value(this CheckBox control)
        {
            if (!control.InvokeRequired)
                return control.Checked;
            else
                return (bool)control.Invoke(new Func<CheckBox, bool>(Value), control);
        }

        /// <summary>
        /// set CheckBox control's Checked value
        /// </summary>
        /// <param name="control">control</param>
        /// <param name="value">new value</param>
        public static void Value(this CheckBox control, bool value)
        {
            if (!control.InvokeRequired)
                control.Checked = value;
            else
                control.Invoke(new Action<CheckBox, bool>(Value), control, value);
        }

        /// <summary>
        /// get DateTimePicker control's value
        /// </summary>
        /// <param name="control">control</param>
        /// <returns></returns>
        public static DateTime Value(this DateTimePicker control)
        {
            if (!control.InvokeRequired)
                return control.Value;
            else
                return (DateTime)control.Invoke(new Func<DateTimePicker, DateTime>(Value), control);
        }

        /// <summary>
        /// set DateTimePicker control's value
        /// </summary>
        /// <param name="control">control</param>
        /// <param name="value">new value</param>
        public static void Value(this DateTimePicker control, DateTime value)
        {
            if (!control.InvokeRequired)
                control.Value = value;
            else
                control.Invoke(new Action<DateTimePicker, DateTime>(Value), control, value);
        }

        /// <summary>
        /// get RadioButton control's Checked value
        /// </summary>
        /// <param name="control">control</param>
        /// <returns></returns>
        public static bool Value(this RadioButton control)
        {
            if (!control.InvokeRequired)
                return control.Checked;
            else
                return (bool)control.Invoke(new Func<RadioButton, bool>(Value), control);
        }

        /// <summary>
        /// set RadioButton control's Checked value
        /// </summary>
        /// <param name="control">control</param>
        /// <param name="value">new value</param>
        public static void Value(this RadioButton control, bool value)
        {
            if (!control.InvokeRequired)
                control.Checked = value;
            else
                control.Invoke(new Action<RadioButton, bool>(Value), control, value);
        }

        /// <summary>
        /// get DataGridView control's DataSource value
        /// </summary>
        /// <param name="control">control</param>
        /// <returns></returns>
        public static object Value(this DataGridView control)
        {
            if (!control.InvokeRequired)
                return control.DataSource;
            else
                return (object)control.Invoke(new Func<DataGridView, object>(Value), control);
        }

        /// <summary>
        /// set DataGridView control's DataSource value
        /// </summary>
        /// <param name="control">control</param>
        /// <param name="value">new value</param>
        public static void Value<T>(this DataGridView control, T value)
        {
            if (!control.InvokeRequired)
                control.DataSource = value;
            else
                control.Invoke(new Action<DataGridView, T>(Value), control, value);
        }

        #endregion

        public static void Sort(this DataGridViewColumn column, ListSortDirection direction)
        {
            if (column != null)
            {
                if (!column.DataGridView.InvokeRequired)
                    column.DataGridView.Sort(column, direction);
                else
                    column.DataGridView.Invoke(new Action<DataGridViewColumn, ListSortDirection>(Sort),
                        column, direction);
            }
        }

        #region << Enabled >>

        public static bool Enabled(this Control control)
        {

            if (!control.InvokeRequired)
                return control.Enabled;
            else
                return (bool)control.Invoke(new Func<Control, bool>(Enabled), control);
        }

        public static void Enabled(this Control control, bool value)
        {

            if (!control.InvokeRequired)
                control.Enabled = value;
            else
                control.Invoke(new Action<Label, bool>(Enabled), control, value);
        }

        #endregion

        #region << ReadOnly >>

        public static bool ReadOnly(this TextBoxBase control)
        {
            if (!control.InvokeRequired)
                return control.ReadOnly;
            else
                return (bool)control.Invoke(new Func<TextBoxBase, bool>(ReadOnly), control);
        }

        public static void ReadOnly(this TextBoxBase control, bool value)
        {
            if (!control.InvokeRequired)
                control.ReadOnly = value;
            else
                control.Invoke(new Action<TextBoxBase, bool>(ReadOnly), control, value);
        }

        #endregion

        public static int SelectedIndex(this ComboBox control)
        {
            if (!control.InvokeRequired)
                return control.SelectedIndex;
            else
                return (int)control.Invoke(new Func<ComboBox, int>(SelectedIndex), control);
        }

        public static void SelectedIndex(this ComboBox control, int value)
        {
            if (!control.InvokeRequired)
                control.SelectedIndex = value;
            else
                control.Invoke(new Action<ComboBox, int>(SelectedIndex), control, value);
        }

        public static object SelectedItem(this ComboBox control)
        {
            if (!control.InvokeRequired)
                return control.SelectedItem;
            else
                return (int)control.Invoke(new Func<ComboBox, object>(SelectedItem), control);
        }

        public static void SelectedItem(this ComboBox control, object value)
        {
            if (!control.InvokeRequired)
                control.SelectedItem = value;
            else
                control.Invoke(new Action<ComboBox, object>(SelectedItem), control, value);
        }

        /// <summary>
        /// 针对Form提供询问的扩展方法，支持跨线程访问。
        /// </summary>
        /// <param name="form"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ConfirmEx(this Form form, string message)
        {
            if (!form.InvokeRequired)
            {
                DialogResult result = MessageBox.Show(form, message, "询问",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                return result == DialogResult.Yes ? true : false;
            }
            else
            {
                return (bool)form.Invoke(new Func<Form, string, bool>(ConfirmEx), form, message);
            }
        }

        /// <summary>
        /// 针对UserControl，支持跨线程访问。
        /// </summary>
        /// <param name="form"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ConfirmEx(this UserControl control, string message)
        {
            if (!control.InvokeRequired)
            {
                DialogResult result = MessageBox.Show(control, message, "询问",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                return result == DialogResult.Yes ? true : false;
            }
            else
            {
                return (bool)control.Invoke(new Func<UserControl, string, bool>(ConfirmEx), control, message);
            }
        }

        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="msg">消息内容</param>
        public static void MessageEx(this Form form, string msg)
        {
            if (!form.InvokeRequired)
                MessageBox.Show(form, msg, "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else
                form.Invoke(new Action<Form, string>(MessageEx), form, msg);
        }

        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="msg">消息内容</param>
        public static void MessageEx(this UserControl control, string msg)
        {
            if (!control.InvokeRequired)
                MessageBox.Show(control.ParentForm, msg, "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else
                control.Invoke(new Action<UserControl, string>(MessageEx), control, msg);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public static void CloseEx(this Form form)
        {
            if (!form.InvokeRequired)
                form.Close();
            else
                form.Invoke(new Action<Form>(CloseEx), form);
        }

        /// <summary>
        /// Save file dialog show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string ChooseSaveFilePath(this Form parent, string filter, string defaultExt, bool checkPathExists)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            //dialog.Filter = "CSV文件(*.csv)|*.csv";
            //dialog.CheckPathExists = true;

            dialog.CheckPathExists = checkPathExists;

            if (!string.IsNullOrEmpty(filter))
                dialog.Filter = filter;
            if (!string.IsNullOrEmpty(defaultExt))
                dialog.DefaultExt = defaultExt;

            DialogResult result;
            if (parent != null)
                result = dialog.ShowDialog(parent);
            else
                result = dialog.ShowDialog();

            if (result == DialogResult.OK)
                return dialog.FileName;
            else
                return string.Empty;
        }
    }
}
