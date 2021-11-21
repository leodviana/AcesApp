using AcesApp.Interfaces;
using AcesApp.Models;
using AcesApp.Services;
using AcesApp.ViewModels;
using AcesApp.Views;
using Acr.UserDialogs;
using Newtonsoft.Json;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Plugin.Popups;
using System;
using Xamarin.Essentials;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace AcesApp
{
    public partial class App : PrismApplication
    {
        public static Usuario usuariologado { get; set; }
        public App() : base(null) { }
        
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
           Application.Current.UserAppTheme = OSAppTheme.Light;
            string usuario_logado = Preferences.Get("dentistaserializado", "");
            App.usuariologado = JsonConvert.DeserializeObject<Usuario>(usuario_logado);


            if (App.usuariologado == null)
            {

                await this.NavigationService.NavigateAsync("LoginPage");
            }
            else

            {
                try
                {

                
                if (usuariologado.ImagePath==null)
                {
                    usuariologado.ImagePath = "profile";
                }
               
                /*if (usuariologado.Id == 999999999)
                {

                    usuariologado.tipo = "Administrador";
                    */
                    var mainPage = $"{nameof(NavigationPage)}/{nameof(MainPage2)}";
                    await this.NavigationService.NavigateAsync(mainPage);

                    /* }
                     else
                     {
                         App.usuariologado.tipo = "Dentista";
                         var navigationParams = new NavigationParameters();
                         navigationParams.Add("paciente", App.usuariologado);


                         var mainPage = $"{nameof(NavigationPage)}/{nameof(MainPage2)}";
                         await NavigationService.NavigateAsync(mainPage);

                     }*/
                }
                catch(Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("",ex.ToString(),"");
                }
            }
            
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterInstance(UserDialogs.Instance);
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterForNavigation<MainPage2, MainPage2ViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<PopupMensagemPage, PopMensagemViewModel>();
            containerRegistry.RegisterForNavigation<RankingPage, RankingPageViewModel>();
            containerRegistry.RegisterForNavigation<PopMudaHorario, PopMudaHorarioViewModel>();
            containerRegistry.RegisterForNavigation<ProfessorPage, ProfessorPageViewModel>();
            containerRegistry.RegisterSingleton<IApiService, ApiService>();

            containerRegistry.RegisterForNavigation<AulasPage, AulasPageViewModel>();
            containerRegistry.RegisterForNavigation<PopAulasPage, PopAulasViewModel>();
            containerRegistry.RegisterForNavigation<ModificaHorarioPage, ModificaHorarioPageViewModel>();
            containerRegistry.RegisterForNavigation<RankingCategoriaPage, RankingCategoriaViewModel>();
        }
    }
}
