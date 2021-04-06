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
            _horarioFinal.Subject = _horarioInicial.Subject;
            _horarioFinal.contrato = _horarioInicial.contrato;
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
            try
            {

                _userDialogs.ShowLoading("Salvando...", MaskType.Black);
                var response = new Response();
                //IsRunning = true;
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)

                {
                    response = await apiService.saveHorarios(_horarioInicial.EventID,_horarioFinal);

                    if (!response.IsSuccess)
                    {
                        await exibeErro(response.Message);
                        _userDialogs.HideLoading();
                        
                        return;
                    }
                    var _evento = (Events)response.Result;
                   // await abrewhattsapp();
                    await NavigationService.GoBackAsync();
                    //duvida se posso colocar isso ou so goback asyc
                    //await NavigationService.NavigateAsync("AulasPage", null);
                }
                else
                {
                    await exibeErro("Dispositivo não está conectado a internet!");
                    _userDialogs.HideLoading();
                    //await PageDialogService.DisplayAlertAsync("Erro", "Dispositivo sem Conexâo", "OK");
                    //await dialogServices.ShowMessage("Erro", response.Message);
                    // IsRunning = false;
                    return;
                }
                    
            }
            catch(Exception ex)
            {
                await exibeErro(ex.Message);
                _userDialogs.HideLoading();
            }


        }
        private async Task abrewhattsapp()
        {
            try
            {
                string texto = "A aula com o professor" + _horarioInicial.nome_professor + "no horario: " + _horarioInicial.data_inicio + " " + _horarioInicial.descricao +
                          "foi alterada para " + _horarioFinal.data_inicio + " " + _horarioFinal.Description + " com o profesor " + _horarioFinal.nome_professor;


                //var uriString = "whatsapp://send?phone=" + fone + " teste de mensagen";
               // var uriString = "whatsapp://send?phone=" + fone + " teste de mensagen";
                var uriString = "whatsapp://send?phone=8599954175" + texto;
                await Launcher.OpenAsync(uriString);
            }
            catch(Exception ex)
            {
                await exibeErro(ex.Message);
            }
           
        }
    }
}
