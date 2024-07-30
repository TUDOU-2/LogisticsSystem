using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using HandyControl.Tools.Extension;
using LogisticsSystem.Messages;
using LogisticsSystem.Services;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;
using LogisticsSystem.Views;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LogisticsSystem.ViewModels
{
    public partial class AirTransportViewModel : ViewModelBase
    {
        private readonly ICustomerService customerService;
        private readonly IAirTransportDetailService airTransportDetailService;
        private readonly IAirTransportService airTransportService;

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
        private ObservableCollection<AirTransportDto> airTransportList = new ObservableCollection<AirTransportDto>();
        public ObservableCollection<AirTransportDto> AirTransportList
        {
            get { return airTransportList; }
            set { airTransportList = value; }
        }

        // 构造函数
        public AirTransportViewModel(ICustomerService customerService, IAirTransportDetailService airTransportDetailService, IAirTransportService airTransportService)
        {
            this.customerService = customerService;
            this.airTransportDetailService = airTransportDetailService;
            this.airTransportService = airTransportService;
            Messenger.Register<ValueChangedMessage<AirTransportDto>, string>(this, "RefreshAirTransport", (recipient, message) =>
            {
                GotoPage(0);
                view?.Close();
            });
            InitializeData();
        }

        // 添加空运单号命令
        [RelayCommand]
        private void AddAirTransport()
        {
            var view = App.Current.Services.GetRequiredService<AddAirTransportView>();
            Messenger.Send(new ValueChangedMessage<List<CustomerDto>>(CustomerList), new string("AddAir"));
            view.Owner = App.Current.MainWindow;
            this.view = view;
            view.ShowDialog();
        }

        // 编辑空运单号命令
        [RelayCommand]
        private void EditAirTransport(AirTransportDto airTransport)
        {
            if (airTransport == null) return;

            var message = new EditTransportMessage<AirTransportDto>(airTransport, CustomerList);
            var view = App.Current.Services.GetRequiredService<EditAirTransportView>();
            Messenger.Send(new ValueChangedMessage<EditTransportMessage<AirTransportDto>>(message), new string("EditAir"));
            view.Owner = App.Current.MainWindow;
            view.ShowDialog();

        }

        // 删除空运单号命令
        [RelayCommand]
        private async void DeleteAirTransport(AirTransportDto airTransport)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("确定要删除吗？删除后将无法恢复！", "提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;

                var deleteResult = await airTransportService.DeleteAsync(airTransport.Id);
                if (deleteResult.Status)
                {
                    var airTransportDetails = await airTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = airTransport.Id.ToString() });
                    var deleteTasks = airTransportDetails.Result.Items.Select(detail => airTransportDetailService.DeleteAsync(detail.Id));
                    await Task.WhenAll(deleteTasks);

                    AirTransportList.Remove(airTransport);
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
        private async void SearshAirTransport()
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

                var airTransportResult = await airTransportService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 20, Search = Search });
                if (airTransportResult.Status)
                {
                    AirTransportList.Clear();
                    foreach (var item in airTransportResult.Result.Items)
                    {
                        item.Customer = CustomerList.FirstOrDefault(c => c.Id == item.CustomerId); // 根据客户Id获取客户

                        var airTransportDetailResult = await airTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = item.Id.ToString() });
                        foreach (var air in airTransportDetailResult.Result.Items)
                        {
                            item.AirTransportDetailsList.Add(air); // 将空运单号详情添加到空运单号中，Dto会自动计算件数、体积、重量
                        }
                        AirTransportList.Add(item);
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
                var airTransportResult = await airTransportService.GetAllAsync(new QueryParameter() { PageIndex = value, PageSize = 5 });
                if (airTransportResult.Status)
                {
                    AirTransportList.Clear();
                    int startIndex = value * 5;
                    foreach (var item in airTransportResult.Result.Items)
                    {
                        item.Customer = CustomerList.FirstOrDefault(c => c.Id == item.CustomerId); // 根据客户Id获取客户

                        var airTransportDetailResult = await airTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = item.Id.ToString() });
                        foreach (var air in airTransportDetailResult.Result.Items)
                        {
                            item.AirTransportDetailsList.Add(air); // 将空运单号详情添加到空运单号中，Dto会自动计算件数、体积、重量
                        }

                        item.IndexNumber = ++startIndex;
                        AirTransportList.Add(item);
                    }
                }
                MaxPageCount = airTransportResult.Result.TotalPages;
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
