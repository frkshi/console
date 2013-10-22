

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MtuConsole
{
    public partial class StatusIndicator : UserControl
    {
        #region << Private Fields >>

        private StatusCollection _items = null;
        private StatusItem _current = null;
        private Point _location = new Point(2, 2);
        private Size _size = new Size(30, 30);
        private Size _sizeBorder = new Size(30, 30);

        #endregion

        #region << ctor >>

        public StatusIndicator()
        {
            InitializeComponent();

            _items = new StatusCollection();
            DisplayStyle = DisplayStyle.Image;
            //_items.PropertyChanged += new PropertyChangedEventHandler(_items_PropertyChanged);
        }

        #endregion

        #region << Public Property >>

        public StatusCollection StatusItems
        {
            get { return _items; }
        }

        public StatusItem Current
        {
            get { return _current; }
            set
            {
                if (value == null)
                {
                    //throw new NullReferenceException("");
                    _current = null;
                    return;
                }

                if (!_items.Contains(value.Name))
                    throw new Exception("设置的状态不存在");

                if (_current == null || !_current.Name.Equals(value.Name))
                {
                    _current = _items.Single((item) => { return item.Name == value.Name; });

                    OnStatusChanged();

                    RenderView();   // 重绘界面
                }
            }
        }

        public DisplayStyle DisplayStyle
        {
            get;
            set;
        }

        #endregion

        #region << Private Methods >>

        private void RenderView()
        {
            Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            int width = this.Size.Width <= this.Size.Height ? this.Size.Height : this.Size.Width;
            _size = new Size(width - 20, width - 20);
            _sizeBorder = new Size(width - 19, width - 19);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_current == null)
                return;

            Graphics g = e.Graphics;

            //Pen p = new Pen(_current.Color);
            //g.DrawEllipse(p, new Rectangle(_location, _sizeBorder));
            //g.FillEllipse(p.Brush, new Rectangle(_location, _size));

            g.DrawImage(_current.Image, new Rectangle(new Point(0, 0), this.Size));
        }

        #endregion

        #region << Events >>

        public event EventHandler StatusChanged;
        private void OnStatusChanged()
        {
            if (StatusChanged != null)
                StatusChanged(this, new EventArgs());
        }

        #endregion

        //private void _items_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public enum DisplayStyle
    {
        Image,
        Color
    }

    public class StatusCollection : IEnumerable<StatusItem>, INotifyPropertyChanged
    {
        #region << Private Fields >>

        private Dictionary<string, StatusItem> _dicItems;

        #endregion

        #region << ctor >>

        public StatusCollection()
        {
            _dicItems = new Dictionary<string, StatusItem>();
        }

        public StatusCollection(IEnumerable<StatusItem> items)
            : this()
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
            {
                _dicItems.Add(item.Name, item);
            }
        }

        #endregion

        public int Count
        {
            get { return _dicItems.Count; }
        }

        public void Add(StatusItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (_dicItems.ContainsKey(item.Name))
                throw new Exception("");

            _dicItems.Add(item.Name, item);

            NotifyPropertyChanged("Count");
        }

        public bool Remove(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentOutOfRangeException("name");

            if (!_dicItems.ContainsKey(name))
                throw new Exception("");

            bool result = _dicItems.Remove(name);

            if (!result)
                return false;

            NotifyPropertyChanged("Count");

            return result;
        }

        public bool Contains(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentOutOfRangeException("name");

            return _dicItems.ContainsKey(name);
        }

        #region << IEnumerable<StatusItem> Members >>

        public IEnumerator<StatusItem> GetEnumerator()
        {
            return _dicItems.Values.GetEnumerator();
        }

        #endregion

        #region << IEnumerable Members >>

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dicItems.Values.GetEnumerator();
        }

        #endregion

        #region << INotifyPropertyChanged Members >>

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class StatusItem
    {
        private string _name;
        private Color _color;

        public StatusItem(string name, Color color)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentOutOfRangeException("name");

            if (color == null)
                throw new ArgumentNullException("color");

            Name = name;
            Color = color;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentOutOfRangeException("");

                _name = value;
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("");

                _color = value;
            }
        }

        private Image _image;
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public string Description { get; set; }
    }
}
