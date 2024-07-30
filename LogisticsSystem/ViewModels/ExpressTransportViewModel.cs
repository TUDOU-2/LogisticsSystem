using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using LogisticsSystem.Services;
using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using LogisticsSystem.Shared.Parameters;
using Microsoft.Extensions.DependencyInjection;
using LogisticsSystem.Views;
using LogisticsSystem.Messages;

namespace LogisticsSystem.ViewModels
{
    public partial class ExpressTransportViewModel : ViewModelBase
    {
        private readonly ICustomerService customerService;
        private readonly IExpressTransportDetailService expressTransportDetailService;
        private readonly IExpressTransportService expressTransportService;

        private int pageNumber;
        private int pageSize;
        private Window view;

        [ObservableProperty]
        private string search; // 搜索关键字

        [ObservableProperty]
        private int maxPageCount = 1; // 最大页码

        [ObservableProperty]
        private int pageIndex; // 当前页码

        partial void OnPageIndexChanged(int value)
        {
            GotoPage(value - 1);
        }


        // 所有客户列表
        private List<CustomerDto> customerList = new List<CustomerDto>();
        public List<CustomerDto> CustomerList
        {
            get { return customerList; }
            set { customerList = value; }
        }

        // 所有空运单号列表
        private ObservableCollection<ExpressTransportDto> expressTransportList = new ObservableCollection<ExpressTransportDto>();
        public ObservableCollection<ExpressTransportDto> ExpressTransportList
        {
            get { return expressTransportList; }
            set { expressTransportList = value; }
        }

        // 构造函数
        public ExpressTransportViewModel(ICustomerService customerService, IExpressTransportDetailService expressTransportDetailService, IExpressTransportService expressTransportService)
        {
            this.customerService = customerService;
            this.expressTransportDetailService = expressTransportDetailService;
            this.expressTransportService = expressTransportService;
            Messenger.Register<ValueChangedMessage<ExpressTransportDto>, string>(this, "RefreshExpressTransport", (recipient, message) =>
            {
                GotoPage(0);
                view?.Close();
            });
            InitializeData();
        }

        // 添加空运单号命令
        [RelayCommand]
        private void AddExpressTransport()
        {
            var view = App.Current.Services.GetRequiredService<AddExpressTransportView>();
            Messenger.Send(new ValueChangedMessage<List<CustomerDto>>(CustomerList), new string("AddExpress"));
            view.Owner = App.Current.MainWindow;
            this.view = view;
            view.ShowDialog();
        }

        // 编辑空运单号命令
        [RelayCommand]
        private void EditExpressTransport(ExpressTransportDto expressTransport)
        {
            if (expressTransport == null) return;

            var message = new EditTransportMessage<ExpressTransportDto>(expressTransport, CustomerList);
            var view = App.Current.Services.GetRequiredService<EditExpressTransportView>();
            Messenger.Send(new ValueChangedMessage<EditTransportMessage<ExpressTransportDto>>(message), new string("EditExpress"));
            view.Owner = App.Current.MainWindow;
            view.ShowDialog();

        }

        // 删除空运单号命令
        [RelayCommand]
        private async void DeleteExpressTransport(ExpressTransportDto expressTransport)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("确定要删除吗？删除后将无法恢复！", "提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;

                var deleteResult = await expressTransportService.DeleteAsync(expressTransport.Id);
                if (deleteResult.Status)
                {
                    var expressTransportDetails = await expressTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = expressTransport.Id.ToString() });
                    var deleteTasks = expressTransportDetails.Result.Items.Select(detail => expressTransportDetailService.DeleteAsync(detail.Id));
                    await Task.WhenAll(deleteTasks);

                    ExpressTransportList.Remove(expressTransport);
                    MessageBox.Show("删除成功！");
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("通信异常，请检查网络连接！", "错误");
            }
        }

        // 根据单号搜索
        [RelayCommand]
        private async void SearshExpressTransport()
        {
            try
            {
                ShowProgress(true);
                if (string.IsNullOrWhiteSpace(Search))
                {
                    // 如果搜索关键字为空，则重新加载所有数据
                    await InitializeData();
                    return;
                }

                var expressTransportResult = await expressTransportService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 20, Search = Search });
                if (expressTransportResult.Status)
                {
                    ExpressTransportList.Clear();
                    foreach (var item in expressTransportResult.Result.Items)
                    {
                        item.Customer = CustomerList.FirstOrDefault(c => c.Id == item.CustomerId); // 根据客户Id获取客户

                        var expressTransportDetailResult = await expressTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = item.Id.ToString() });
                        foreach (var express in expressTransportDetailResult.Result.Items)
                        {
                            item.ExpressTransportDetailsList.Add(express); // 将空运单号详情添加到空运单号中，Dto会自动计算件数、体积、重量
                        }
                        ExpressTransportList.Add(item);
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                ShowProgress(false);
            }
        }

        // 跳转到指定页码
        private async Task GotoPage(int value)
        {
            try
            {
                if (value < 0 || value > MaxPageCount) return;
                var expressTransportResult = await expressTransportService.GetAllAsync(new QueryParameter() { PageIndex = value, PageSize = 5 });
                if (expressTransportResult.Status)
                {
                    ExpressTransportList.Clear();
                    int startIndex = value * 5;
                    foreach (var item in expressTransportResult.Result.Items)
                    {
                        item.Customer = CustomerList.FirstOrDefault(c => c.Id == item.CustomerId); // 根据客户Id获取客户

                        var expressTransportDetailResult = await expressTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = item.Id.ToString() });
                        foreach (var express in expressTransportDetailResult.Result.Items)
                        {
                            item.ExpressTransportDetailsList.Add(express); // 将空运单号详情添加到空运单号中，Dto会自动计算件数、体积、重量
                        }

                        item.IndexNumber = ++startIndex;
                        ExpressTransportList.Add(item);
                    }
                }
                MaxPageCount = expressTransportResult.Result.TotalPages;
            }
            catch (Exception)
            {

            }
        }

        // 初始化数据
        private async Task InitializeData()
        {
            try
            {
                ShowProgress(true);
                // 获取所有客户
                var customerResult = await customerService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000 });
                if (customerResult.Status)
                {
                    CustomerList.Clear();
                    foreach (var item in customerResult.Result.Items)
                    {
                        CustomerList.Add(item);
                    }
                }

                await GotoPage(0);
            }
            catch (Exception)
            {
                MessageBox.Show("初始化数据失败,请检查网络！");
            }
            finally
            {
                ShowProgress(false);
            }
        }
    }
}
