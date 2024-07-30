using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LogisticsSystem.Messages;
using LogisticsSystem.Services;
using LogisticsSystem.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogisticsSystem.ViewModels
{
    public partial class EditAirTransportViewModel : ViewModelBase
    {
        private readonly ICustomerService customerService;
        private readonly IAirTransportService airTransportService;
        private readonly IAirTransportDetailService airTransportDetailService;

        [ObservableProperty]
        private AirTransportDto airTransport;

        [ObservableProperty]
        private List<CustomerDto> customerList = new List<CustomerDto>();

        // 构造函数
        public EditAirTransportViewModel(ICustomerService customerService, IAirTransportService airTransportService, IAirTransportDetailService airTransportDetailService)
        {
            this.customerService = customerService;
            this.airTransportService = airTransportService;
            this.airTransportDetailService = airTransportDetailService;
            Messenger.Register<ValueChangedMessage<EditTransportMessage<AirTransportDto>>, string>(this, "EditAir", (recipient, message) =>
            {
                var editAirTransportMessage = message.Value;
                customerList = editAirTransportMessage.CustomerList;
                AirTransport = editAirTransportMessage.entityDto;
                Initialize();
            });
        }

        private void Initialize()
        {
            try
            {
                if (AirTransport != null)
                {
                    foreach (var item in AirTransport.AirTransportDetailsList)
                    {
                        item.IsModified = false;
                    }
                }
            }
            catch (Exception)
            {

            }
        }


        // 添加空运详情
        [RelayCommand]
        private async void Add()
        {
            var entity = new AirTransportDetailDto
            {
                AirTransportId = AirTransport.Id,
                ReceiveDate = DateTime.Now,
                InsertDate = DateTime.Now,
                IsNew = true
            };

            AirTransport.AirTransportDetailsList.Add(entity);
        }

        // 删除空运详情
        [RelayCommand]
        private async void Delete(AirTransportDetailDto entity)
        {

            MessageBoxResult result = MessageBox.Show("确定要删除吗？删除后将无法恢复！", "提示", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;

            var deleteResult = await airTransportDetailService.DeleteAsync(entity.Id);
            if (deleteResult.Status)
            {
                AirTransport.AirTransportDetailsList.Remove(entity);
                MessageBox.Show("删除成功", "提示");
                Save();
            }
            else
            {
                MessageBox.Show("删除失败", "提示");
            }            
        }

        // 保存内容
        [RelayCommand]
        private async void Save()
        {
            try
            {
                var customer = CustomerList.FirstOrDefault(c => c.Name == AirTransport.Customer.Name);
                AirTransport.CustomerId = customer.Id;

                var airTransportResult = await airTransportService.UpdateAsync(AirTransport);

                var saveResult = await SaveAirTransportDetailAsync();

                if (airTransportResult.Status && saveResult)
                {
                    MessageBox.Show("数据保存成功！", "提示");
                }
                else
                {
                    MessageBox.Show("数据保存失败！", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生异常: {ex.Message}", "错误");
            }
        }

        private async Task<bool> SaveAirTransportDetailAsync()
        {
            bool saveResult = true;

            try
            {
                foreach (var item in AirTransport.AirTransportDetailsList.Where(a => a.IsModified == true && (a.IsNew == false || a.IsNew == null)))
                {
                    var updateResult = await airTransportDetailService.UpdateAsync(item);
                    if (updateResult.Status)
                    {
                        item.IsModified = false; // 标记为非修改状态
                    }
                    else
                    {
                        saveResult = false;
                    }
                }

                foreach (var item in AirTransport.AirTransportDetailsList.Where(a => a.IsNew == true))
                {
                    var addResult = await airTransportDetailService.AddAsync(item);
                    if (addResult.Status)
                    {
                        item.Id = addResult.Result.Id;
                        item.IsNew = false; // 标记为非新增状态
                    }
                    else
                    {
                        saveResult = false;
                    }
                }
            }
            catch (Exception)
            {

            }

            return saveResult;
        }


    }
}
