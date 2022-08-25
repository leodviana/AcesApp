using AcesApp.Interfaces;
using AcesApp.Models;
using AcesApp.Services;
using AcesApp.Views;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AcesApp.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private ICommand _navegar;
        private ICommand _ForgotPasswordCommand;

        IApiService apiService;

        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                SetProperty(ref isRunning, value);

            }
        }


        private bool _mostra_mensagem;

        public bool mostra_mensagem
        {
            get { return _mostra_mensagem; }
            set
            {
                SetProperty(ref _mostra_mensagem, value);

            }
        }

        private string _mensagem;
        public string mensagem
        {
            get { return _mensagem; }
            set
            {
                SetProperty(ref _mensagem, value);

            }
        }

        private string _usuarioid;
        public string Usuarioid
        {
            get { return _usuarioid; }
            set
            {
                SetProperty(ref _usuarioid, value);

            }
        }



        private string _senha;
        public string Senha
        {
            get { return _senha; }
            set
            {
                SetProperty(ref _senha, value);

            }
        }

        // (INavigationService navigationService,
        //       IPageDialogService pageDialogService, IMarvelApiService marvelApiService) : base(navigationService, pageDialogService)
        
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,IApiService ApiService) : base(navigationService, pageDialogService)
        {              
             apiService = ApiService;
             mostra_mensagem = false;
             mensagem = "";
         }

         public ICommand navegar
         {
             get
             {
                 return _navegar ?? (_navegar = new Command(objeto =>
                 {
                     //_navigationService.NavigateAsync("/MasterPage/NavigationPage/DentistaPage");
                     Login();
                     // _navigationService.NavigateAsync("PermissaoPage");
                 }));
             }
         }

         public ICommand ForgotPasswordCommand
         {
             get
             {
                 return _ForgotPasswordCommand ?? (_ForgotPasswordCommand = new Command(objeto =>
                 {
                     //_navigationService.NavigateAsync("/MasterPage/NavigationPage/DentistaPage");
                     // navigationService.NavigateAsync
                     NavigationService.NavigateAsync("LoginPacientePage");
                 }));
             }
         }

        private async void Login()
         {
             
             if (string.IsNullOrEmpty(Usuarioid))
             {
                 await exibeErro("Prencha o campo Email!");

                 return;
             }


             if (string.IsNullOrEmpty(Senha))
             {
                 await exibeErro("Prencha o campo Senha!");
                 //await PageDialogService.DisplayAlertAsync("Erro", "Prencha o campo Senha!", "OK");
                 return;
             }
            //var senhaNovaDigitadaCript =GetMd5Hash(MD5.Create(), Senha);
            var senhaNovaDigitadaCript =base64Encode(Senha);

            var usuario = new Usuario();
            usuario.Login = Usuarioid;
            usuario.Senha = senhaNovaDigitadaCript;
             var response = new Response();
             IsRunning = true;
             var current = Connectivity.NetworkAccess;

             if (current == NetworkAccess.Internet)

             {
                 response = await apiService.getUsuario(usuario);
             }
             else
             {
                 await exibeErro("Dispositivo não está conectado a internet!");
                 
                 IsRunning = false;
                 return;
             }
             IsRunning = false;

             if (!response.IsSuccess)
             {
                 await exibeErro(response.Message);
                 
                 return;
             }

             var User = (Usuario)response.Result;
            if (User.ImagePath == null)
            {
                User.ImagePath = "profile";
            }
            App.usuariologado = User;

             Preferences.Set("dentistaserializado", JsonConvert.SerializeObject(User));
             var mainPage = $"/{nameof(NavigationPage)}/{nameof(MainPage2)}";
             await NavigationService.NavigateAsync(mainPage);
                 
            
        }


       
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }
    }
}
