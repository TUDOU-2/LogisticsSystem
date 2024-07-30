using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using LogisticsSystem.Services;
using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogisticsSystem.ViewModels
{
    public partial class AddExpressTransportViewModel : ViewModelBase
    {
        private readonly IExpressTransportDetailService expressTransportDetailService;
        private readonly IExpressTransportService expressTransportService;

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

        [Required(ErrorMessage = "渠道为必填项")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "柜型只能包含字母和数字")]
        [ObservableProperty]
        private string channel;

        [Required(ErrorMessage = "单号为必填项")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "柜号只能包含字母和数字")]
        [ObservableProperty]
        private string orderNumber;


        partial void OnTargetPlaceChanged(string value)
        {
            ValidateProperty(value, nameof(TargetPlace));
        }

        partial void OnChannelChanged(string value)
        {
            ValidateProperty(value, nameof(Channel));
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
        private ExpressTransportDto expressTransport = new ExpressTransportDto();

        // 构造函数
        public AddExpressTransportViewModel(IExpressTransportDetailService expressTransportDetailService, IExpressTransportService expressTransportService)
        {
            this.expressTransportDetailService = expressTransportDetailService;
            this.expressTransportService = expressTransportService;
            Messenger.Register<ValueChangedMessage<List<CustomerDto>>, string>(this, "AddExpress", (recipient, message) =>
            {
                CustomerList = message.Value;
            });
        }

        // 添加详情命令
        [RelayCommand]
        private void Add()
        {
            ExpressTransport.ExpressTransportDetailsList.Add(new ExpressTransportDetailDto
            {
                ExpressTransportId = ExpressTransport.Id,
                ReceiveDate = DateTime.Now,
                InsertDate = DateTime.Now,
                IsModified = true
            });
        }

        // 删除详情命令
        [RelayCommand]
        private void Delete(ExpressTransportDetailDto expressTransportDetail)
        {
            ExpressTransport.ExpressTransportDetailsList.Remove(expressTransportDetail);
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

                ExpressTransport.CustomerId = SelectedCustomer.Id;
                ExpressTransport.SourcePlace = SourcePlace;
                ExpressTransport.TargetPlace = TargetPlace;
                ExpressTransport.OrderNumber = OrderNumber;
                ExpressTransport.Channel = Channel;

                var expressTransportResult = await expressTransportService.AddAsync(ExpressTransport);
                if (expressTransportResult.Status)
                {
                    foreach (var item in ExpressTransport.ExpressTransportDetailsList)
                    {
                        item.ExpressTransportId = expressTransportResult.Result.Id;
                        await expressTransportDetailService.AddAsync(item);
                    }

                    MessageBox.Show("数据添加成功！");
                    Messenger.Send(new ValueChangedMessage<ExpressTransportDto>(ExpressTransport), new string("RefreshExpressTransport"));
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
