using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
using System.Windows;

namespace LogisticsSystem.ViewModels
{
    public partial class AirMoneySummaryViewModel : ViewModelBase
    {
        private readonly IAirTransportService airTransportService;
        private readonly IAirTransportDetailService airTransportDetailService;

        [ObservableProperty]
        private CustomerDto selectedCustomer;

        [ObservableProperty]
        private int maxPageCount = 1; // 最大页码

        [ObservableProperty]
        private int pageIndex; // 当前页码

        [ObservableProperty]
        private int sumCount; // 汇总数量

        [ObservableProperty]
        private double sumNeedPayMoney; // 汇总应付金额

        [ObservableProperty]
        private double sumPayMoney; // 汇总已付金额

        [ObservableProperty]
        private List<CustomerDto> customerList = new List<CustomerDto>();

        [ObservableProperty]
        private ObservableCollection<AirTransportDto> airTransportList = new ObservableCollection<AirTransportDto>();

        partial void OnSelectedCustomerChanged(CustomerDto value)
        {
            GotoPageAsync(0);
        }

        partial void OnPageIndexChanged(int value)
        {
            GotoPageAsync(value - 1);
        }

        public AirMoneySummaryViewModel(IAirTransportService airTransportService, IAirTransportDetailService airTransportDetailService)
        {
            this.airTransportService = airTransportService;
            this.airTransportDetailService = airTransportDetailService;
            Messenger.Register<ValueChangedMessage<CustomerDto>, string>(this, "MoneySummary", (recipient, messange) =>
            {
                SelectedCustomer = messange.Value;
            });
        }

        [RelayCommand]
        private async void Save()
        {
            try
            {
                if (AirTransportList.Count == 0) return;
                foreach (var item in AirTransportList)
                {
                    var result = await airTransportService.UpdateAsync(item);
                    if (!result.Status)
                    {
                        MessageBox.Show("保存失败！");
                        return;
                    }
                }
                MessageBox.Show("保存成功！");
                await GotoPageAsync(PageIndex - 1);
            }
            catch (Exception)
            {
                MessageBox.Show("通信异常，请检查网络！");
            }
        }

        private async Task GotoPageAsync(int value)
        {
            try
            {
                if (value < 0 || value > MaxPageCount) return;
                if (SelectedCustomer == null) return;
                var airTransportResult = await airTransportService.GetAllAsync(new QueryParameter { PageIndex = value, PageSize = 5, CustomerId = SelectedCustomer.Id });

                if (airTransportResult.Status)
                {
                    AirTransportList.Clear();
                    SumCount = 0;
                    SumNeedPayMoney = 0;
                    SumPayMoney = 0;
                    int startIndex = value * 5;
                    foreach (var item in airTransportResult.Result.Items)
                    {
                        item.Customer = SelectedCustomer;

                        var airTransportDetailResult = await airTransportDetailService.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 1000, Search = item.Id.ToString() });
                        foreach (var air in airTransportDetailResult.Result.Items)
                        {
                            item.AirTransportDetailsList.Add(air); // 将空运单号详情添加到空运单号中，Dto会自动计算件数、体积、重量
                        }
                        SumCount += item.SumCount;
                        SumNeedPayMoney += item.NeedPayMoney;
                        SumPayMoney += item.PayMoney;
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
    }
}
