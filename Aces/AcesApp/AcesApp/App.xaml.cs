using AcesApp.Interfaces;
using AcesApp.Services;
using AcesApp.ViewModels;
using AcesApp.Views;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace AcesApp
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await this.NavigationService.NavigateAsync("LoginPage");
           // await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
           // containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
           // containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<PopupMensagemPage, PopMensagemViewModel>();
            containerRegistry.RegisterSingleton<IApiService, ApiService>();
        }
    }
}
