using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LogisticsSystem.Services;
using LogisticsSystem.Shared.Dtos;
using LogisticsSystem.Shared.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LogisticsSystem.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly ILoginService service;
        private readonly IUsersService usersService;
        private readonly IServiceProvider serviceProvider;

        private bool isRegistering;
        public bool IsRegistering
        {
            get => isRegistering;
            set
            {
                SetProperty(ref isRegistering, value);
                OnPropertyChanged(nameof(LoginPanelVisibility));
                OnPropertyChanged(nameof(RegisterPanelVisibility));
            }
        }

        public Visibility LoginPanelVisibility => IsRegistering ? Visibility.Collapsed : Visibility.Visible;
        public Visibility RegisterPanelVisibility => IsRegistering ? Visibility.Visible : Visibility.Collapsed;


        [ObservableProperty]
        [Required(ErrorMessage = "请输入账号")]
        private string account;

        [ObservableProperty]
        [Required(ErrorMessage = "请输入密码")]
        private string password;


        [ObservableProperty]
        [Required(ErrorMessage = "请输入账号")]
        [RegularExpression(@"^\w+$", ErrorMessage = "账号只能包含字母和数字")]
        private string newAccount;

        [ObservableProperty]
        [Required(ErrorMessage = "请输入用户名")]
        private string newName;

        [ObservableProperty]
        [Required(ErrorMessage = "请输入密码")]
        [RegularExpression(@"^\w+$", ErrorMessage = "密码只能包含字母和数字")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6到20个字符之间")]
        private string newPassword;


        private string confirmPassword;
        [Required(ErrorMessage = "请确认密码")]
        [Compare(nameof(NewPassword), ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value, true);
        }

        public LoginViewModel(ILoginService service, IUsersService usersService, IServiceProvider serviceProvider)
        {
            this.service = service;
            this.usersService = usersService;
            this.serviceProvider = serviceProvider;
            GotoLogin();
        }

        [RelayCommand]
        public async Task LoginAsync()
        {

            try
            {
                ValidateAllProperties();
                if (HasErrors)
                {
                    return;
                }
                if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Password))
                {
                    MessageBox.Show("账户或密码不能为空");
                    return;
                }

                var loginResult = await service.LoginAsync(new UsersDto
                {
                    Account = Account,
                    Password = Password
                });

                if (loginResult.Status)
                {
                    var view = serviceProvider.GetRequiredService<MainWindow>();
                    view.Show();
                    Messenger.Send(new ValueChangedMessage<string>(loginResult.Result.Name), new string("Login"));

                    App.Current.MainWindow.Close();
                }
                else
                {
                    MessageBox.Show("登录失败");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("通信异常,请检查网络！");
            }
        }

        [RelayCommand]
        private void GotoRegister()
        {
            IsRegistering = true;
            Account = "1";
            Password = "1";
            NewAccount = string.Empty;
            NewName = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }

        [RelayCommand]
        private void GotoLogin()
        {
            IsRegistering = false;  
            Account= string.Empty;
            Password = string.Empty;
            NewAccount = "1";
            NewName = "1";
            NewPassword = "1";
            ConfirmPassword = "1";
        }

        [RelayCommand]
        private async void Register()
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                return;
            }
            var result = await usersService.GetAllAsync(new QueryParameter {PageIndex = 0,PageSize = 1000,Search = NewAccount });
            if (result.Status && result.Result.Items.Count > 0)
            {
                MessageBox.Show("账号已存在");
                return;
            }

            var user = new UsersDto
            {
                Account = NewAccount,
                Name = NewName,
                Password = NewPassword
            };

            var addResult = await usersService.AddAsync(user);
            if (addResult.Status)
            {
                MessageBox.Show("注册成功");
                GotoLogin();
            }
            else
            {
                MessageBox.Show("注册失败");
            }
        }
    }
}
