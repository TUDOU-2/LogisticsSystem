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
    public partial class EditCustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService service;

        [ObservableProperty]
        private CustomerDto customer;


        public EditCustomerViewModel(ICustomerService service)
        {
            this.service = service;
            Messenger.Register<ValueChangedMessage<CustomerDto>, string>(this, "EditCustomer", (recipient, message) =>
            {
                Customer = message.Value;
            });
        }

        // 保存内容
        [RelayCommand]
        private async void Save()
        {
            try
            {
                var customerResult = await service.UpdateAsync(Customer);
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
