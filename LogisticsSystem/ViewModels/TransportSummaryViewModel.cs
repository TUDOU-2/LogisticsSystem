using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LogisticsSystem.Services;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LogisticsSystem.ViewModels
{
    public partial class TransportSummaryViewModel : ViewModelBase
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ICustomerService customerService;

        [ObservableProperty]
        private object currentSummaryView;

        [ObservableProperty]
        private CustomerDto selectedItem;

        partial void OnSelectedItemChanged(CustomerDto value)
        {
            Messenger.Send(new ValueChangedMessage<CustomerDto>(SelectedItem), new string("TransportSummary"));
        }

        [ObservableProperty]
        private ObservableCollection<CustomerDto> customerList = new ObservableCollection<CustomerDto>();

        public TransportSummaryViewModel(IServiceProvider serviceProvider, ICustomerService customerService)
        {
            this.serviceProvider = serviceProvider;
            this.customerService = customerService;
            Initialize();
            NavigeteToView("Air");
        }

        [RelayCommand]
        private void NavigeteToView(string viewName)
        {
            var view = GetOrCreateView(viewName);
            CurrentSummaryView = view;
            Messenger.Send(new ValueChangedMessage<CustomerDto>(SelectedItem), new string("TransportSummary"));
        }

        private object GetOrCreateView(string viewName)
        {
            try
            {
                string typeName = $"LogisticsSystem.Views.{viewName}TransportSummaryView";

                Type viewType = Type.GetType(typeName);

                object view = serviceProvider.GetService(viewType);

                return view;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private async void Initialize()
        {
            try
            {
                var customerResult = await customerService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000 });
                if (customerResult.Status)
                {
                    foreach (var item in customerResult.Result.Items)
                    {
                        CustomerList.Add(item);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
