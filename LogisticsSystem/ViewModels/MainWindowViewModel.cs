using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogisticsSystem.Messages;
using CommunityToolkit.Mvvm.Messaging;
using LogisticsSystem.Services;
using LogisticsSystem.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Windows.Threading;

namespace LogisticsSystem.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IServiceProvider serviceProvider; // 服务提供者
        private readonly ProgressView progressView;

        [ObservableProperty]
        private string userName; // 用户名

        [ObservableProperty]
        private string currentTime; // 当前时间

        [ObservableProperty]
        private object currentView; // 当前视图

        //[ObservableProperty]
        private bool isProgressVisible;
        public bool IsProgressVisible
        {
            get => isProgressVisible;
            set => SetProperty(ref isProgressVisible, value);
        }

        // 构造函数
        public MainWindowViewModel(IServiceProvider serviceProvider, ProgressView progressView)
        {
            this.serviceProvider = serviceProvider;
            this.progressView = progressView;
            GetTime();
            Messenger.Register<ProgressMessage>(this, OnProgressMessageReceived);
            Messenger.Register<ValueChangedMessage<string>, string>(this, "Login", (recipient, message) =>
            {
                UserName = message.Value;
            });
        }

        private void GetTime()
        {
            // 初始化当前时间
            CurrentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, args) =>
            {
                CurrentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            };
            timer.Start();
        }

        private void OnProgressMessageReceived(object recipient, ProgressMessage message)
        {
            IsProgressVisible = message.IsVisible;
        }

        [RelayCommand]
        private void NavigeteToView(string viewName)
        {
            var view = GetOrCreateView(viewName);
            CurrentView = view;
        }


        /// <summary>
        /// 获取视图窗口
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns></returns>
        private object GetOrCreateView(string viewName)
        {
            try
            {
                string typeName = $"LogisticsSystem.Views.{viewName}View";

                Type viewType = Type.GetType(typeName);

                object view = serviceProvider.GetService(viewType);

                return view;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
