using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;
using System.ComponentModel;
namespace MtuConsole
{
    // Summary:
    //     Notifies clients that a property value has changed.
    public interface INotifyPropertyChanged
    {
        // Summary:
        //     Occurs when a property value changes.
        event PropertyChangedEventHandler PropertyChanged;
    }
    public abstract class Model<T> : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性发生变化时间触发
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        protected int _itemCount;
        /// <summary>
        /// 项目数量
        /// </summary>
        public int ItemCount
        {
            get { return _itemCount; }
            set { _itemCount = value; NotifyPropertyChanged("ItemCount"); }
        }

        protected IEnumerable<T> _items;
        /// <summary>
        /// 所有项目
        /// </summary>
        public virtual IEnumerable<T> Items
        {
            get { return GetItems(); }
            set { _items = value; NotifyPropertyChanged("Items"); }
        }

        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<T> GetItems();

        private T _selectedItem;
        /// <summary>
        /// 选中项
        /// </summary>
        public virtual T SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; NotifyPropertyChanged("SelectedItem"); }
        }

        /// <summary>
        /// 重置
        /// </summary>
        protected void Reset()
        {
            Items = null;
            SelectedItem = default(T);
        }

    }
    public class RtuModel : Model<RtuItem>
    {
        #region << Private Fileds >>

        private IBasicDataService _basicDataService = null;

        //private IRtuModelProvider _service = new RtuModelProvider();

        private string _searchText;

        #endregion

        #region << ctor >>

        /// <summary>
        /// 构造函数
        /// </summary>
        public RtuModel()
        {
            _basicDataService = App.CurrentApp.GetService<IBasicDataService>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="service">基础数据服务实例</param>
        public RtuModel(IBasicDataService service)
        {
            _basicDataService = service;
        }

        #endregion

        #region << Public Property >>

        /// <summary>
        /// Filter
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                NotifyPropertyChanged("Items");
                NotifyPropertyChanged("ItemCount");
            }
        }

        #endregion

        #region << Override Base Members >>

        /// <summary>
        /// 覆盖父类成员, 获取所有
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<RtuItem> GetItems()
        {
            //Option 1: Retrieve rtu collection from service all the time
            //if (items == null)
            //{
            //    items = string.IsNullOrEmpty(SearchText) ?
            //        service.GetAllRtu() :
            //        service.GetSearchRtus(SearchText);

            //    ItemCount = items.Count();
            //}
            //return items;

            //Option 2: Cache rtu collection on client side
            //if (_items == null)
            //{
            //_items = _service.GetAllRtu();

            //_basicDataService.SyncRtus();
            _items = new List<RtuItem>(_basicDataService.Rtus);

            if (_items == null)
                return null;
            //}

            string ptid = string.Empty, ctid = string.Empty,
                   rtuid = string.Empty, rtuname = string.Empty;

            if (SearchText != null && SearchText.Trim() != string.Empty)
            {
                string[] tmpstrs = SearchText.Split(',');

                if (tmpstrs.Length == 4)
                {
                    ptid = tmpstrs[0];
                    ctid = tmpstrs[1];
                    rtuid = tmpstrs[2];
                    rtuname = tmpstrs[3];
                }
            }

            //var newitems = from rtu in _items
            //               where string.IsNullOrEmpty(ptid) ? true: rtu.ProductTypeId.ToLower().Equals(ptid.ToLower())
            //                  && string.IsNullOrEmpty(ctid) ? true : rtu.CommunicationTypeId.ToLower().Equals(ctid.ToLower())
            //                  && string.IsNullOrEmpty(rtuid) ? true : rtu.Id.Equals(rtuid)
            //                  && string.IsNullOrEmpty(rtuname) ? true : rtu.Name.Equals(rtuname)
            //               select rtu;

            var newitems = _items.Where((rtu) =>
            {

                string tmpctid = rtu.CommunicationTypeId == null ? string.Empty : rtu.CommunicationTypeId.ToLower();
                string tmpptid = rtu.ProductTypeId == null ? string.Empty : rtu.ProductTypeId.ToLower();

                // RTU名称模糊查询实现细节:
                //   1. 不区分大小写;
                //   2. 不区分区域特性;
                //   3. 检查字符串是否包含查询字符串
                return (string.IsNullOrEmpty(ptid) ? true : tmpptid == ptid.ToLower())
                    & (string.IsNullOrEmpty(ctid) ? true : tmpctid == ctid.ToLower())
                    & (string.IsNullOrEmpty(rtuid) ? true : rtu.Id == rtuid)
                    & (string.IsNullOrEmpty(rtuname) || string.IsNullOrEmpty(rtu.Name)
                        ? true : rtu.Name.ToLower().Contains(rtuname.ToLower()));
            });

            _itemCount = newitems.Count();

            return newitems.ToList();
        }

        #endregion
    }
        public class RtuItem
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductTypeId { get; set; }

        public string CommunicationId { get; set; }

        public string CommunicationId1 { get; set; }

        public string CommunicationId2 { get; set; }

        /// <summary>
        /// 通讯协议类型
        /// </summary>
        public string CommunicationTypeId { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string SName { get; set; }

        /// <summary>
        /// 安装地址
        /// </summary>
        public string InstallAddress { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        public string InstallLocation { get; set; }

        /// <summary>
        /// 安装日期
        /// </summary>
        public DateTime InstallDatetime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 警戒线
        /// </summary>
        public decimal SafetyLevel { get; set; }

        /// <summary>
        /// 溢流线
        /// </summary>
        public decimal AlertLevel { get; set; }

        /// <summary>
        /// 回差值
        /// </summary>
        public decimal BackValue { get; set; }

        /// <summary>
        /// 设备状态（开机 1，停机 0, 通讯终端 2, 设备故障 3）
        /// </summary>
        public int DeviceStatus { get; set; }

        /// <summary>
        /// 当前使用的周期(1,2,3)
        /// </summary>
        public int CurrentCycle { get; set; }
    }
        public class MeasureMessage
        {
            /// <summary>
            /// 检测量Id
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// 检测量名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 检测量值
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            /// 时间戳
            /// </summary>
            public DateTime Time { get; set; }
        }

        public abstract class AlertMessage
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 时间
            /// </summary>
            public DateTime Time { get; set; }
        }
        public class LimitAlertMessage : AlertMessage
        {
            /// <summary>
            /// 值
            /// </summary>
            public object Value1 { get; set; }
        }

        public class MutationAlertMessage : AlertMessage
        {
            /// <summary>
            /// 值1
            /// </summary>
            public object Value1 { get; set; }

            /// <summary>
            /// 值2
            /// </summary>
            public object Value2 { get; set; }
        }

    
}
