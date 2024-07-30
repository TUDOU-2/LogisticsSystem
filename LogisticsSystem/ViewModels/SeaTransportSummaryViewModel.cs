using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using LogisticsSystem.Services;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.ViewModels
{
    public partial class SeaTransportSummaryViewModel : ViewModelBase
    {
        private readonly ISeaTransportService seaTransportService;
        private readonly ISeaTransportDetailService seaTransportDetailService;

        [ObservableProperty]
        private CustomerDto selectedCustomer;

        [ObservableProperty]
        private int maxPageCount = 1; // 最大页码

        [ObservableProperty]
        private int pageIndex; // 当前页码

        [ObservableProperty]
        private int sumCount; // 汇总数量

        [ObservableProperty]
        private double sumWeight; // 汇总重量

        [ObservableProperty]
        private double sumVolume; // 汇总体积

        [ObservableProperty]
        private List<CustomerDto> customerList = new List<CustomerDto>();

        [ObservableProperty]
        private ObservableCollection<SeaTransportDto> seaTransportList = new ObservableCollection<SeaTransportDto>();

        partial void OnSelectedCustomerChanged(CustomerDto value)
        {
            GotoPage(0);
        }

        partial void OnPageIndexChanged(int value)
        {
            GotoPage(value - 1);
        }

        public SeaTransportSummaryViewModel(ISeaTransportService seaTransportService, ISeaTransportDetailService seaTransportDetailService)
        {
            this.seaTransportService = seaTransportService;
            this.seaTransportDetailService = seaTransportDetailService;
            Messenger.Register<ValueChangedMessage<CustomerDto>, string>(this, "TransportSummary", (recipient, messange) =>
            {
                SelectedCustomer = messange.Value;
            });
        }

        private async void GotoPage(int value)
        {
            try
            {
                if (value < 0 || value > MaxPageCount) return;
                if (SelectedCustomer == null) return;
                var seaTransportResult = await seaTransportService.GetAllAsync(new QueryParameter { PageIndex = value, PageSize = 5, CustomerId = SelectedCustomer.Id });

                if (seaTransportResult.Status)
                {
                    SeaTransportList.Clear();
                    SumCount = 0;
                    SumWeight = 0;
                    SumVolume = 0;
                    int startIndex = value * 5;
                    foreach (var item in seaTransportResult.Result.Items)
                    {
                        item.Customer = SelectedCustomer;

                        var seaTransportDetailResult = await seaTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = item.Id.ToString() });
                        foreach (var sea in seaTransportDetailResult.Result.Items)
                        {
                            item.SeaTransportDetailsList.Add(sea); // 将空运单号详情添加到空运单号中，Dto会自动计算件数、体积、重量
                        }
                        SumCount += item.SumCount;
                        SumWeight += item.SumWeight;
                        SumVolume += item.SumVolume;
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
    }
}
