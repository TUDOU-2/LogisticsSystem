using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LogisticsSystem.Messages;
using LogisticsSystem.Services;
using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogisticsSystem.ViewModels
{
    public partial class AddAirTransportViewModel : ViewModelBase
    {
        private readonly IAirTransportDetailService airTransportDetailService;
        private readonly IAirTransportService airTransportService;

        private CustomerDto selectedCustomer;
        [Required(ErrorMessage = "客户为必选项")]
        public CustomerDto SelectedCustomer
        {
            get => selectedCustomer;
            set => SetProperty(ref selectedCustomer, value, true);
        }

        [Required(ErrorMessage = "始发地为必填项")]
        [ObservableProperty]
        private string sourcePlace;


        [Required(ErrorMessage = "目的地为必填项")]
        [ObservableProperty]
        private string targetPlace;

        [Required(ErrorMessage = "单号为必填项")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "单号只能包含字母和数字")]
        [ObservableProperty]
        private string orderNumber;

        partial void OnTargetPlaceChanged(string value)
        {
            ValidateProperty(value, nameof(TargetPlace));
        }

        partial void OnOrderNumberChanged(string value)
        {
            ValidateProperty(value, nameof(OrderNumber));
        }

        partial void OnSourcePlaceChanged(string value)
        {
            ValidateProperty(value, nameof(SourcePlace));
        }

        [ObservableProperty]
        private List<CustomerDto> customerList = new List<CustomerDto>();

        [ObservableProperty]
        private AirTransportDto airTransport = new AirTransportDto();

        // 构造函数
        public AddAirTransportViewModel(IAirTransportDetailService airTransportDetailService, IAirTransportService airTransportService)
        {
            this.airTransportDetailService = airTransportDetailService;
            this.airTransportService = airTransportService;
            Messenger.Register<ValueChangedMessage<List<CustomerDto>>, string>(this, "AddAir", (recipient, message) =>
            {
                CustomerList = message.Value;
            });
        }

        // 添加详情命令
        [RelayCommand]
        private void Add()
        {
            AirTransport.AirTransportDetailsList.Add(new AirTransportDetailDto
            {
                AirTransportId = AirTransport.Id,
                ReceiveDate = DateTime.Now,
                InsertDate = DateTime.Now,
                IsModified = true
            });
        }

        // 删除详情命令
        [RelayCommand]
        private void Delete(AirTransportDetailDto airTransportDetail)
        {
            AirTransport.AirTransportDetailsList.Remove(airTransportDetail);
        }

        // 保存命令
        [RelayCommand]
        private async Task SaveAsync()
        {

            try
            {
                ValidateAllProperties();
                if (HasErrors)
                {
                    MessageBox.Show("输入信息有误，请重新检查输入信息！", "错误");
                    return;
                }

                AirTransport.CustomerId = SelectedCustomer.Id;
                AirTransport.SourcePlace = SourcePlace;
                AirTransport.TargetPlace = TargetPlace;
                AirTransport.OrderNumber = OrderNumber;

                var airTransportResult = await airTransportService.AddAsync(AirTransport);
                if (airTransportResult.Status)
                {
                    foreach (var item in AirTransport.AirTransportDetailsList)
                    {
                        item.AirTransportId = airTransportResult.Result.Id;
                        await airTransportDetailService.AddAsync(item);
                    }

                    MessageBox.Show("数据添加成功！");
                    Messenger.Send(new ValueChangedMessage<AirTransportDto>(AirTransport), new string("RefreshAirTransport"));
                }
                else
                {
                    MessageBox.Show("数据添加失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("通信异常，请检查网络连接！", "错误");
            }
        }
    }
}
