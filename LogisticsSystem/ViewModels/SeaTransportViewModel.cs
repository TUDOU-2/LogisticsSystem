using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using LogisticsSystem.Messages;
using LogisticsSystem.Services;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;
using LogisticsSystem.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogisticsSystem.ViewModels
{
    public partial class SeaTransportViewModel : ViewModelBase
    {
        private readonly ICustomerService customerService;
        private readonly ISeaTransportDetailService seaTransportDetailService;
        private readonly ISeaTransportService seaTransportService;

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

        // 所有海运单号列表
        private ObservableCollection<SeaTransportDto> seaTransportList = new ObservableCollection<SeaTransportDto>();
        public ObservableCollection<SeaTransportDto> SeaTransportList
        {
            get { return seaTransportList; }
            set { seaTransportList = value; }
        }

        // 构造函数
        public SeaTransportViewModel(ICustomerService customerService, ISeaTransportDetailService seaTransportDetailService, ISeaTransportService seaTransportService)
        {
            this.customerService = customerService;
            this.seaTransportDetailService = seaTransportDetailService;
            this.seaTransportService = seaTransportService;
            Messenger.Register<ValueChangedMessage<SeaTransportDto>, string>(this, "RefreshSeaTransport", (recipient, message) =>
            {
                GotoPage(0);
                view?.Close();
            });
            InitializeData();
        }

        // 添加海运单号命令
        [RelayCommand]
        private void AddSeaTransport()
        {
            var view = App.Current.Services.GetRequiredService<AddSeaTransportView>();
            Messenger.Send(new ValueChangedMessage<List<CustomerDto>>(CustomerList), new string("AddSea"));
            view.Owner = App.Current.MainWindow;
            this.view = view;
            view.ShowDialog();
        }

        // 编辑海运单号命令
        [RelayCommand]
        private void EditSeaTransport(SeaTransportDto seaTransport)
        {
            if (seaTransport == null) return;

            var message = new EditTransportMessage<SeaTransportDto>(seaTransport, CustomerList);
            var view = App.Current.Services.GetRequiredService<EditSeaTransportView>();
            Messenger.Send(new ValueChangedMessage<EditTransportMessage<SeaTransportDto>>(message), new string("EditSea"));
            view.Owner = App.Current.MainWindow;
            view.ShowDialog();

        }

        // 删除海运单号命令
        [RelayCommand]
        private async void DeleteSeaTransport(SeaTransportDto seaTransport)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("确定要删除吗？删除后将无法恢复！", "提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;

                var deleteResult = await seaTransportService.DeleteAsync(seaTransport.Id);
                if (deleteResult.Status)
                {
                    var seaTransportDetails = await seaTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = seaTransport.Id.ToString() });
                    var deleteTasks = seaTransportDetails.Result.Items.Select(detail => seaTransportDetailService.DeleteAsync(detail.Id));
                    await Task.WhenAll(deleteTasks);

                    SeaTransportList.Remove(seaTransport);
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
        private async void SearshSeaTransport()
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

                var seaTransportResult = await seaTransportService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 20, Search = Search });
                if (seaTransportResult.Status)
                {
                    SeaTransportList.Clear();
                    foreach (var item in seaTransportResult.Result.Items)
                    {
                        item.Customer = CustomerList.FirstOrDefault(c => c.Id == item.CustomerId); // 根据客户Id获取客户

                        var seaTransportDetailResult = await seaTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = item.Id.ToString() });
                        foreach (var sea in seaTransportDetailResult.Result.Items)
                        {
                            item.SeaTransportDetailsList.Add(sea);
                        }
                        SeaTransportList.Add(item);
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
                var seaTransportResult = await seaTransportService.GetAllAsync(new QueryParameter() { PageIndex = value, PageSize = 5 });
                if (seaTransportResult.Status)
                {
                    SeaTransportList.Clear();
                    int startIndex = value * 5;
                    foreach (var item in seaTransportResult.Result.Items)
                    {
                        item.Customer = CustomerList.FirstOrDefault(c => c.Id == item.CustomerId); // 根据客户Id获取客户

                        var seaTransportDetailResult = await seaTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = item.Id.ToString() });
                        foreach (var sea in seaTransportDetailResult.Result.Items)
                        {
                            item.SeaTransportDetailsList.Add(sea);
                        }

                        item.IndexNumber = ++startIndex;
                        SeaTransportList.Add(item);
                    }
                }
                MaxPageCount = seaTransportResult.Result.TotalPages;
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
