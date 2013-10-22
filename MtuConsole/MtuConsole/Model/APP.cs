using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;

using System.ComponentModel.Design;

namespace MtuConsole
{
    public interface IApplication : IServiceProvider, IDisposable
    {
       
        TServiceType GetService<TServiceType>() where TServiceType : class;
    }
    public class App : IApplication, IDisposable
    {
        #region << Fields >>

        /// <summary>
        /// Service Container
        /// </summary>
        private IServiceContainer _serviceContainer = null;


        private IBasicDataService _basicDataService = null;



        #endregion

        #region << ctor >>

        public App()
        {

            App.CurrentApp = this;

            Init();
        }

        #endregion

        #region << Public Property >>

        /// <summary>
        /// Instance
        /// </summary>
        public static IApplication CurrentApp
        {
            get;
            private set;
        }

        #endregion

        #region << IApplication Members >>

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <typeparam name="TServiceType">服务类型</typeparam>
        /// <returns></returns>
        public TServiceType GetService<TServiceType>() where TServiceType : class
        {
            return _serviceContainer == null ? null : _serviceContainer.GetService(typeof(TServiceType)) as TServiceType;
        }

        #endregion

        #region << IServiceProvider Members >>

        public object GetService(Type serviceType)
        {
            return _serviceContainer == null ? null : _serviceContainer.GetService(serviceType);
        }

        #endregion

        #region << Private Methods >>

        /// <summary>
        /// Init 
        /// </summary>
        private void Init()
        {
           

            // 初始化服务容器，及服务
            _serviceContainer = new ServiceContainer();

 


            LoadBasicDataService();

        }

        #region << Load Service >>
        /// <summary>
        /// 加载基础数据提供服务
        /// </summary>
        private void LoadBasicDataService()
        {
            ///TODO:加载部分重写，实现BasicDataServiceImp 中央核心部分
            _basicDataService = new BasicDataServiceImp();

            _serviceContainer.AddService(typeof(IBasicDataService), _basicDataService);

           
        }

   



        #endregion

        #endregion

        #region << Release resources >>

        #region << IDisposable Members >>

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        private bool _isDisposed = false;
        protected virtual void Dispose(bool isDisposing)
        {
           
            if (_isDisposed)
                return;

            if (isDisposing)
            {


        

                _serviceContainer.RemoveService(typeof(IBasicDataService));
                _basicDataService.Dispose();

                ((ServiceContainer)_serviceContainer).Dispose();
            }

            _isDisposed = true;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~App()
        {
            Dispose(true);
        }

        #endregion
    }
}
