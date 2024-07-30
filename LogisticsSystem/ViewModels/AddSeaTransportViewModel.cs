using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LogisticsSystem.Services;

namespace LogisticsSystem.ViewModels
{
    public partial class AddSeaTransportViewModel : ViewModelBase
    {
        private readonly ISeaTransportDetailService seaTransportDetailService;
        private readonly ISeaTransportService seaTransportService;

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

        [Required(ErrorMessage = "柜型为必填项")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "柜型只能包含字母和数字")]
        [ObservableProperty]
        private string boxModel;

        [Required(ErrorMessage = "柜号为必填项")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "柜号只能包含字母和数字")]
        [ObservableProperty]
        private string boxNumber;


        partial void OnTargetPlaceChanged(string value)
        {
            ValidateProperty(value, nameof(TargetPlace));
        }

        partial void OnBoxModelChanged(string value)
        {
            ValidateProperty(value, nameof(BoxModel));
        }
        partial void OnBoxNumberChanged(string value)
        {
            ValidateProperty(value, nameof(BoxNumber));
        }

        partial void OnSourcePlaceChanged(string value)
        {
            ValidateProperty(value, nameof(SourcePlace));
        }

        [ObservableProperty]
        private List<CustomerDto> customerList = new List<CustomerDto>();

        [ObservableProperty]
        private SeaTransportDto seaTransport = new SeaTransportDto();

        // 构造函数
        public AddSeaTransportViewModel(ISeaTransportDetailService seaTransportDetailService, ISeaTransportService seaTransportService)
        {
            this.seaTransportDetailService = seaTransportDetailService;
            this.seaTransportService = seaTransportService;
            Messenger.Register<ValueChangedMessage<List<CustomerDto>>, string>(this, "AddSea", (recipient, message) =>
            {
                CustomerList = message.Value;
            });
        }

        // 添加详情命令
        [RelayCommand]
        private void Add()
        {
            SeaTransport.SeaTransportDetailsList.Add(new SeaTransportDetailDto
            {
                SeaTransportId = SeaTransport.Id,
                ReceiveDate = DateTime.Now,
                InsertDate = DateTime.Now,
                IsModified = true
            });
        }

        // 删除详情命令
        [RelayCommand]
        private void Delete(SeaTransportDetailDto seaTransportDetail)
        {
            SeaTransport.SeaTransportDetailsList.Remove(seaTransportDetail);
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

                SeaTransport.CustomerId = SelectedCustomer.Id;
                SeaTransport.SourcePlace = SourcePlace;
                SeaTransport.TargetPlace = TargetPlace;
                SeaTransport.BoxModel = BoxModel;
                SeaTransport.BoxNumber = BoxNumber;

                var seaTransportResult = await seaTransportService.AddAsync(SeaTransport);
                if (seaTransportResult.Status)
                {
                    foreach (var item in SeaTransport.SeaTransportDetailsList)
                    {
                        item.SeaTransportId = seaTransportResult.Result.Id;
                        await seaTransportDetailService.AddAsync(item);
                    }

                    MessageBox.Show("数据添加成功！");
                    Messenger.Send(new ValueChangedMessage<SeaTransportDto>(SeaTransport), new string("RefreshSeaTransport"));
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
