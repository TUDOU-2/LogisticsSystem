using LogisticsSystem.Services;
using LogisticsSystem.ViewModels;
using LogisticsSystem.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace LogisticsSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }


        public App()
        {
            Services = ConfigureServices();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginView = Services.GetRequiredService<LoginView>();
            loginView.ShowDialog();
            
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // 注册Http请求服务
            services.AddSingleton<string>(new Uri("http://localhost:5107/").ToString());
            services.AddSingleton<HttpRestClient>();

            // 注册视图
            services.AddSingleton<ProgressView>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginView>();
            services.AddTransient<CustomerViewModel>();
            services.AddTransient<CustomerView>(sp => new CustomerView(sp.GetRequiredService<CustomerViewModel>()));
            services.AddTransient<UserViewModel>();
            services.AddTransient<UserView>(sp => new UserView(sp.GetRequiredService<UserViewModel>()));
            services.AddTransient<AirTransportViewModel>();
            services.AddTransient<AirTransportView>(sp => new AirTransportView(sp.GetRequiredService<AirTransportViewModel>()));
            services.AddTransient<SeaTransportViewModel>();
            services.AddTransient<SeaTransportView>(sp => new SeaTransportView(sp.GetRequiredService<SeaTransportViewModel>()));
            services.AddTransient<ExpressTransportViewModel>();
            services.AddTransient<ExpressTransportView>(sp => new ExpressTransportView(sp.GetRequiredService<ExpressTransportViewModel>()));

            services.AddTransient<TransportSummaryViewModel>();
            services.AddTransient<TransportSummaryView>(sp => new TransportSummaryView(sp.GetRequiredService<TransportSummaryViewModel>()));
            services.AddTransient<AirTransportSummaryViewModel>();
            services.AddTransient<AirTransportSummaryView>(sp => new AirTransportSummaryView(sp.GetRequiredService<AirTransportSummaryViewModel>()));
            services.AddTransient<SeaTransportSummaryViewModel>();
            services.AddTransient<SeaTransportSummaryView>(sp => new SeaTransportSummaryView(sp.GetRequiredService<SeaTransportSummaryViewModel>()));
            services.AddTransient<ExpressTransportSummaryViewModel>();
            services.AddTransient<ExpressTransportSummaryView>(sp => new ExpressTransportSummaryView(sp.GetRequiredService<ExpressTransportSummaryViewModel>()));

            services.AddTransient<MoneySummaryViewModel>();
            services.AddTransient<MoneySummaryView>(sp => new MoneySummaryView(sp.GetRequiredService<MoneySummaryViewModel>()));
            services.AddTransient<AirMoneySummaryViewModel>();
            services.AddTransient<AirMoneySummaryView>(sp => new AirMoneySummaryView(sp.GetRequiredService<AirMoneySummaryViewModel>()));
            services.AddTransient<SeaMoneySummaryViewModel>();
            services.AddTransient<SeaMoneySummaryView>(sp => new SeaMoneySummaryView(sp.GetRequiredService<SeaMoneySummaryViewModel>()));
            services.AddTransient<ExpressMoneySummaryViewModel>();
            services.AddTransient<ExpressMoneySummaryView>(sp => new ExpressMoneySummaryView(sp.GetRequiredService<ExpressMoneySummaryViewModel>()));

            // 注册弹窗
            services.AddTransient<AddCustomerViewModel>();
            services.AddTransient<AddCustomerView>(sp => new AddCustomerView(sp.GetRequiredService<AddCustomerViewModel>()));
            services.AddTransient<EditCustomerViewModel>();
            services.AddTransient<EditCustomerView>(sp => new EditCustomerView(sp.GetRequiredService<EditCustomerViewModel>()));
            services.AddTransient<AddAirTransportViewModel>();
            services.AddTransient<AddAirTransportView>(sp => new AddAirTransportView(sp.GetRequiredService<AddAirTransportViewModel>()));
            services.AddTransient<EditAirTransportViewModel>();
            services.AddTransient<EditAirTransportView>(sp => new EditAirTransportView(sp.GetRequiredService<EditAirTransportViewModel>()));
            services.AddTransient<AddSeaTransportViewModel>();
            services.AddTransient<AddSeaTransportView>(sp => new AddSeaTransportView(sp.GetRequiredService<AddSeaTransportViewModel>()));
            services.AddTransient<EditSeaTransportViewModel>();
            services.AddTransient<EditSeaTransportView>(sp => new EditSeaTransportView(sp.GetRequiredService<EditSeaTransportViewModel>()));
            services.AddTransient<AddExpressTransportViewModel>();
            services.AddTransient<AddExpressTransportView>(sp => new AddExpressTransportView(sp.GetRequiredService<AddExpressTransportViewModel>()));
            services.AddTransient<EditExpressTransportViewModel>();
            services.AddTransient<EditExpressTransportView>(sp => new EditExpressTransportView(sp.GetRequiredService<EditExpressTransportViewModel>()));

            // 注册服务
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ILoginService, LoginSerivce>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IAirTransportService, AirTransportService>();
            services.AddTransient<IAirTransportDetailService, AirTransportDetailService>();
            services.AddTransient<ISeaTransportService, SeaTransportService>();
            services.AddTransient<ISeaTransportDetailService, SeaTransportDetailService>();
            services.AddTransient<IExpressTransportService, ExpressTransportService>();
            services.AddTransient<IExpressTransportDetailService, ExpressTransportDetailService>();

            return services.BuildServiceProvider();
        }
    }

}
