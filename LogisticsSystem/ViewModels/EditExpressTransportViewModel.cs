using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
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
    public partial class EditExpressTransportViewModel : ViewModelBase
    {
        private readonly ICustomerService customerService;
        private readonly IExpressTransportService expressTransportService;
        private readonly IExpressTransportDetailService expressTransportDetailService;

        [ObservableProperty]
        private ExpressTransportDto expressTransport;

        [ObservableProperty]
        private List<CustomerDto> customerList = new List<CustomerDto>();

        // 构造函数
        public EditExpressTransportViewModel(ICustomerService customerService, IExpressTransportService expressTransportService, IExpressTransportDetailService expressTransportDetailService)
        {
            this.customerService = customerService;
            this.expressTransportService = expressTransportService;
            this.expressTransportDetailService = expressTransportDetailService;
            Messenger.Register<ValueChangedMessage<EditTransportMessage<ExpressTransportDto>>, string>(this, "EditExpress", (recipient, message) =>
            {
                var editExpressTransportMessage = message.Value;
                customerList = editExpressTransportMessage.CustomerList;
                ExpressTransport = editExpressTransportMessage.entityDto;
                Initialize();
            });
        }

        private void Initialize()
        {
            try
            {
                if (ExpressTransport != null)
                {
                    foreach (var item in ExpressTransport.ExpressTransportDetailsList)
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
            var entity = new ExpressTransportDetailDto
            {
                ExpressTransportId = ExpressTransport.Id,
                ReceiveDate = DateTime.Now,
                InsertDate = DateTime.Now,
                IsNew = true
            };

            ExpressTransport.ExpressTransportDetailsList.Add(entity);
        }

        // 删除空运详情
        [RelayCommand]
        private async void Delete(ExpressTransportDetailDto entity)
        {

            MessageBoxResult result = MessageBox.Show("确定要删除吗？删除后将无法恢复！", "提示", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;

            var deleteResult = await expressTransportDetailService.DeleteAsync(entity.Id);
            if (deleteResult.Status)
            {
                ExpressTransport.ExpressTransportDetailsList.Remove(entity);
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
                var customer = CustomerList.FirstOrDefault(c => c.Name == ExpressTransport.Customer.Name);
                ExpressTransport.CustomerId = customer.Id;

                var expressTransportResult = await expressTransportService.UpdateAsync(ExpressTransport);

                var saveResult = await SaveExpressTransportDetailAsync();

                if (expressTransportResult.Status && saveResult)
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

        private async Task<bool> SaveExpressTransportDetailAsync()
        {
            bool saveResult = true;

            try
            {
                foreach (var item in ExpressTransport.ExpressTransportDetailsList.Where(a => a.IsModified == true && (a.IsNew == false || a.IsNew == null)))
                {
                    var updateResult = await expressTransportDetailService.UpdateAsync(item);
                    if (updateResult.Status)
                    {
                        item.IsModified = false; // 标记为非修改状态
                    }
                    else
                    {
                        saveResult = false;
                    }
                }

                foreach (var item in ExpressTransport.ExpressTransportDetailsList.Where(a => a.IsNew == true))
                {
                    var addResult = await expressTransportDetailService.AddAsync(item);
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
