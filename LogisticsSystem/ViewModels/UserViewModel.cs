using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class UserViewModel : ViewModelBase
    {
        private readonly IUsersService service;

        [ObservableProperty]
        private ObservableCollection<UsersDto> usersList = new ObservableCollection<UsersDto>();

        public UserViewModel(IUsersService service)
        {
            this.service = service;
            GetUserDataAsync();
        }

        [RelayCommand]
        private void Add()
        {
            UsersList.Add(new UsersDto() { IsNew = true });
        }

        [RelayCommand]
        private async void Save()
        {
            var result = await UpdateAndAdd();
            if (result)
            {
                MessageBox.Show("保存成功");
                await GetUserDataAsync();
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        private async Task<bool> UpdateAndAdd()
        {
            bool result = true;
            foreach (var item in UsersList.Where(u => u.IsModified == true && !u.IsNew))
            {
                var saveResult = await service.UpdateAsync(item);
                if (saveResult.Status)
                {
                    item.IsModified = false;
                }
                else
                {
                    result = false;
                }
            }

            foreach (var item in UsersList.Where(u => u.IsNew))
            {
                var queryResult = await service.GetAllAsync(new QueryParameter { PageIndex = 0, PageSize = 1000 ,Search = item.Account});
                if (queryResult.Status && queryResult.Result.Items.Count > 0)
                {
                    MessageBox.Show("账号已存在");
                    result = false;
                    continue;
                }

                var addResult = await service.AddAsync(item);
                if (addResult.Status)
                {
                    item.IsNew = false;
                    item.IsModified = false;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        [RelayCommand]
        private async void CancelChanges()
        {
            await GetUserDataAsync();
        }

        private async Task GetUserDataAsync()
        {
            try
            {
                var usersResult = await service.GetAllAsync(new QueryParameter() { PageIndex = 0, PageSize = 100 });
                if (usersResult.Status)
                {
                    UsersList.Clear();
                    foreach (var item in usersResult.Result.Items)
                    {
                        item.IsModified = false;
                        UsersList.Add(item);
                    }
                }
            }
            catch (Exception)
            {

            }

        }
    }
}
