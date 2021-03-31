using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AcesApp.Interfaces;
using AcesApp.Models;
using Acr.UserDialogs;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AcesApp.ViewModels
{
    public class PopMudaHorarioViewModel : ViewModelBase
    {
        private ICommand _refresh;
        private ICommand _reagendar;
        IApiService apiService;
        private readonly IUserDialogs _userDialogs;
        public ObservableCollection<Events> lsthorarioInicial { get; set; }
        public ObservableCollection<Events> lsthorarioFinal { get; set; }
        public Events _horarioInicial { get; set; }
        public Events _horarioFinal;

        // public Command voltarCommand; 
        public PopMudaHorarioViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IUserDialogs userDialogs, IApiService ApiService) : base(navigationService, pageDialogService)
        {
            
            _userDialogs = userDialogs;
            apiService = ApiService;
            lsthorarioInicial = new ObservableCollection<Events>();
            lsthorarioFinal = new ObservableCollection<Events>();
            // Professores = new List<Professor>();

        }


        private string _inicio;
        public string inicio
        {
            get { return _inicio; }
            set
            {
                SetProperty(ref _inicio, value);

            }
        }

        private string _final;
        public string final
        {
            get { return _final; }
            set
            {
                SetProperty(ref _final, value);

            }
        }





        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            _horarioInicial = (Events)parameters["inicial"];
            _horarioFinal = (Events)parameters["final"];
            lsthorarioInicial.Add(_horarioInicial);
            lsthorarioFinal.Add(_horarioFinal);
        }



        public ICommand voltarCommand
        {
            get
            {
                return _refresh ?? (_refresh = new Command(async objeto =>
                {



                    await NavigationService.GoBackAsync();


                }));
            }
        }

        public ICommand reagendarCommand
        {
            get
            {
                return _reagendar ?? (_reagendar = new Command(async objeto =>
                {



                    await reagendar();


                }));
            }
        }

        private async Task reagendar()
        {
            var response = new Response();
           // IsRunning = true;
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)

            {
                response = await apiService.saveHorarios(_horarioInicial.EventID,_horarioFinal);
            }
            else
            {
                await exibeErro("Dispositivo não está conectado a internet!");
                //await PageDialogService.DisplayAlertAsync("Erro", "Dispositivo sem Conexâo", "OK");
                //await dialogServices.ShowMessage("Erro", response.Message);
               // IsRunning = false;
                return;
            }
            //IsRunning = false;

            if (!response.IsSuccess)
            {
                await exibeErro(response.Message);
                //await PageDialogService.DisplayAlertAsync("Erro", response.Message, "OK");
                //await dialogServices.ShowMessage("Erro", response.Message);
                return;
            }
           var _evento =  (Events)response.Result;
        }
    }
}
