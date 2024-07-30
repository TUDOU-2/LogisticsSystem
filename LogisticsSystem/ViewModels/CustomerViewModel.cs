using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
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
    public partial class CustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService service;

        [ObservableProperty]
        private int maxPageCount = 1; // 最大页码

        [ObservableProperty]
        private int pageIndex; // 当前页码

        [ObservableProperty]
        private ObservableCollection<CustomerDto> customerList = new ObservableCollection<CustomerDto>();        

        public CustomerViewModel(ICustomerService service)
        {
            this.service = service;
            GotoPage(0);

            Messenger.Register<ValueChangedMessage<CustomerDto>>(this, (recipient, message) =>
            {
                // 查找客户，-1表示不存在
                var existingCustomerIndex = CustomerList.IndexOf(CustomerList.FirstOrDefault(c => c.Id == message.Value.Id));
                if (existingCustomerIndex != -1)
                {
                    CustomerList[existingCustomerIndex] = message.Value; // 更新客户信息
                }
                else
                {
                    CustomerList.Add(message.Value); // 添加客户成功后，刷新客户列表
                }                               
            });
        }

        // 跳转到指定页码
        private async Task GotoPage(int value)
        {
            try
            {
                if (value < 0 || value > MaxPageCount) return;
                var airTransportResult = await service.GetAllAsync(new QueryParameter() { PageIndex = value, PageSize = 5 });
                if (airTransportResult.Status)
                {
                    CustomerList.Clear();
                    int startIndex = value * 100;
                    foreach (var item in airTransportResult.Result.Items)
                    {
                        item.IndexNumber = ++startIndex;
                        CustomerList.Add(item);
                    }
                }
                MaxPageCount = airTransportResult.Result.TotalPages;
            }
            catch (Exception)
            {

            }
        }

        // 添加客户命令
        [RelayCommand]
        private void AddCustomer()
        {
            var view = App.Current.Services.GetRequiredService<AddCustomerView>();
            view.Owner = App.Current.MainWindow;
            view.ShowDialog();
        }

        // 编辑客户命令
        [RelayCommand]
        private void EditCustomer(CustomerDto customer)
        {
            var view = App.Current.Services.GetRequiredService<EditCustomerView>();
            view.Owner = App.Current.MainWindow;
            Messenger.Send(new ValueChangedMessage<CustomerDto>(customer), new string("EditCustomer"));
            view.ShowDialog();
        }

        // 删除客户命令
        [RelayCommand]
        private async void DeleteCustomer(CustomerDto customer)
        {
            try
            {
                if (customer == null) return;

                MessageBoxResult result = MessageBox.Show($"确定要删除该客户吗？", "提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;
                var customerResult = await service.DeleteAsync(customer.Id);
                if (customerResult.Status)
                {
                    CustomerList.Remove(customer);
                    
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
