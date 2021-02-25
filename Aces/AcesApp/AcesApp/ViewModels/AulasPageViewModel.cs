using AcesApp.Interfaces;
using AcesApp.Models;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Plugin.Calendar.Models;

namespace AcesApp.ViewModels
{
    public class AulasPageViewModel : ViewModelBase

    {
        IApiService apiService;
        private readonly IUserDialogs _userDialogs;
        public EventCollection Events { get; set; }

        private CultureInfo _culture = CultureInfo.InvariantCulture;
        public CultureInfo Culture
        {
            get => _culture;
            set => SetProperty(ref _culture, value);
        }

        private bool _inicializa;

        public bool inicializa
        {
            get { return _inicializa; }
            set
            {
                SetProperty(ref _inicializa, value);

            }
        }

        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                SetProperty(ref isRunning, value);

            }
        }
        public AulasPageViewModel(INavigationService navigationService, 
            IPageDialogService pageDialogService, IApiService ApiService , IUserDialogs userDialogs) : base(navigationService, pageDialogService)
        {
             _userDialogs = userDialogs;
            apiService = ApiService;
            Events = new EventCollection();
            inicializa = false;

            IsActiveChanged += HandleIsActiveTrue;
            IsActiveChanged += HandleIsActiveFalse;

        }



        private void HandleIsActiveFalse(object sender, EventArgs e)
        {
            if (IsActive == true) return;

        }

        private async void HandleIsActiveTrue(object sender, EventArgs e)
        {
            if (IsActive == false) return;
            if (!inicializa)
                await CarregarEventosAsync();

        }


        private async Task InitializeAsync()
        {
            //await PageDialogService.DisplayAlertAsync("app", "Sem conexao!", "Ok");
            try
            {
                await CarregarEventosAsync();
                inicializa = true;
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("app", ex.ToString(), "Ok");
                return;
            }
        }
        public override void Destroy()
        {
            IsActiveChanged -= HandleIsActiveTrue;
            IsActiveChanged -= HandleIsActiveFalse;
        }


        private async Task CarregarEventosAsync()
        {
            var parametro = new Events();
            parametro.contrato = App.usuariologado.contratoId;
            Culture = CultureInfo.CreateSpecificCulture("pt-BR");
            var current = Connectivity.NetworkAccess;
           
            var response = new Response();
            IsRunning = true;
            _userDialogs.ShowLoading("Carregando");
            if (current == NetworkAccess.Internet)

            {
                response = await apiService.getEventos(parametro);
            }
            else
            {
                _userDialogs.HideLoading();
                await exibeErro("Dispositivo não está conectado a internet!");

                IsRunning = false;
                return;
            }
            IsRunning = false;

            if (!response.IsSuccess)
            {
                _userDialogs.HideLoading();
                await exibeErro(response.Message);

                return;
            }
            //var lista_eventos = new List<Events>();
            var lista_eventos = (List<Events>)response.Result;
            /*  Events = new EventCollection();*/
            Events.Clear();
            foreach (var item in lista_eventos)
            {

                var horario = (item.Start.Hour.ToString().Length == 1 ? "0" : "") + item.Start.Hour.ToString() +":" +item.Start.Minute.ToString()+ (item.Start.Minute.ToString().Length == 1 ? "0" : "") + " - " + (item.End.Hour.ToString().Length == 1 ? "0" : "") + item.End.Hour.ToString()+ ":" +item.End.Minute.ToString() + (item.End.Minute.ToString().Length==1?"0":"");
                var lista_eventos_preenchida = GeraLista(item.nome_professor.ToString(), horario);
                Events.Add(item.Start, lista_eventos_preenchida);

            }
            _userDialogs.HideLoading();

        }
        private List<Evento> GeraLista(string nome, string descricao)
        {
            return new List<Evento>
                 {
                     new Evento { Nome = nome, Descricao = descricao }
                 };


        }
        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var navigationMode = parameters.GetNavigationMode();

            if (navigationMode != Prism.Navigation.NavigationMode.Back)
                Task.Run(() => InitializeAsync()).ConfigureAwait(false);
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {

            // await InitializeAsync();
        }
    }
}
