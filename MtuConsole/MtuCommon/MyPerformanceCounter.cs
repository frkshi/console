using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace MtuConsole.Common
{
    public class MyPerformanceCounter
    {
        PerformanceCounter _counter;
        private void Create(string classname, string countername)
        {
            try
            {
                //判断是否已经存在
                if (PerformanceCounterCategory.Exists(classname))
                    return;

                //创建CounterCreationData
                var ccd = new CounterCreationData();
                //名称为：我的计数器，类型是：NumberOfItems64
                ccd.CounterName = countername;
                ccd.CounterType = PerformanceCounterType.NumberOfItems32;

                PerformanceCounterCategory.Create(classname,           //类型名称
                    countername,                                 //类型描述
                    PerformanceCounterCategoryType.SingleInstance,     //类型的实例种类
                    new CounterCreationDataCollection() { ccd });      //创建性能计数器数据
                _counter = new PerformanceCounter(classname, countername, false);
            }
            catch
            { }


        }


        public void AddValue(string classname, string countername, int addvalue)
        {
            try
            {
                if (!PerformanceCounterCategory.Exists(classname))
                {
                    Create(classname, countername);
                }
                if (_counter == null)
                {
                    _counter = new PerformanceCounter(classname, countername, false);
                }

                // PerformanceCounterCategory[] c= PerformanceCounterCategory.GetCategories();
                // PerformanceCounterCategory a = c.First(x => x.CategoryName == classname);
                //PerformanceCounter[] counters= a.GetCounters();
                //PerformanceCounter counter = counters.First(x => x.CounterName == countername);
                // PerformanceCounter counter = new PerformanceCounter(classname, countername);

                _counter.IncrementBy(addvalue);

            }
            catch
            { }

        }
    }
}
