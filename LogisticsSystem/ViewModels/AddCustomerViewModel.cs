using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LogisticsSystem.Services;
using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.ViewModels
{
    public partial class AddCustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService service;

        [ObservableProperty]
        private CustomerDto customer;


        public AddCustomerViewModel(ICustomerService service)
        {
            this.service = service;

            Customer = new CustomerDto();
        }

        // 保存内容
        [RelayCommand]
        private async void Save()
        {
            try
            {
                if (string.IsNullOrEmpty(Customer.Name) && string.IsNullOrEmpty(Customer.Telephone) &&
                    string.IsNullOrEmpty(Customer.Address) && string.IsNullOrEmpty(Customer.Address) &&
                    string.IsNullOrEmpty(Customer.Nation) && string.IsNullOrEmpty(Customer.Description))
                {
                    // 请填写完整信息
                    return;
                }
                    var customerResult = await service.AddAsync(Customer);
                if (customerResult.Status)
                {
                    Messenger.Send(new ValueChangedMessage<CustomerDto>(Customer));
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
