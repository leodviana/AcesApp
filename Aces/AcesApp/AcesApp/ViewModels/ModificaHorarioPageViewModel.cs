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
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;

namespace AcesApp.ViewModels
{
    public class ModificaHorarioPageViewModel : ViewModelBase
    {

        private ICommand _MudarHorarioCommand;
        IApiService apiService;
        private readonly IUserDialogs _userDialogs;
        public EventCollection Events { get; set; }

        public Events _horarioInicial;
        public Professor _professor;
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

        private ICommand _voltar;

        public ICommand voltarCommand
        {
            get
            {
                return _voltar ?? (_voltar = new Command(objeto =>
                {
                    NavigationService.GoBackAsync();
                }));
            }
        }
        public ModificaHorarioPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService, IApiService ApiService, IUserDialogs userDialogs) : base(navigationService, pageDialogService)
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
        public ICommand MudarHorarioCommand => new Command(async (item) => await MudarHorario(item));
        private async Task MudarHorario(object item)
        {

            var navigationParams = new NavigationParameters();
            navigationParams.Add("final", item);
            navigationParams.Add("inicial", _horarioInicial);
            //await NavigationService.NavigateAsync("ModificaHorarioPage", navigationParams);
            await NavigationService.NavigateAsync("PopMudaHorario", navigationParams, true, true);
            //Selection = null;
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
            if (_professor == null)
                parametro.professor = App.usuariologado.id_professor;
            else
                parametro.professor = _professor.id_grldentista;
            parametro.Start = DateTime.Now;
            Culture = CultureInfo.CreateSpecificCulture("pt-BR");
            var current = Connectivity.NetworkAccess;

            var response = new Response();
            IsRunning = true;

            _userDialogs.ShowLoading("Carregando", MaskType.Black);
            if (current == NetworkAccess.Internet)

            {
                response = await apiService.getEventosfree(parametro);
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
            var groupedlist = lista_eventos
               .GroupBy(x => Convert.ToDateTime(x.Start).ToString("d"))
               .Select(grp => grp.ToList())
               .ToList();
            foreach (var item in groupedlist)
            {
                var datakey = Convert.ToDateTime(item[0].Start);
                var converteditem = item.ToList();
                Events.Add(datakey, converteditem);
            }
           
           /* foreach (var item in lista_eventos)
            {

                var horario = (item.Start.Hour.ToString().Length == 1 ? "0" : "") + item.Start.Hour.ToString() + ":" + item.Start.Minute.ToString() + (item.Start.Minute.ToString().Length == 1 ? "0" : "") + " - " + (item.End.Hour.ToString().Length == 1 ? "0" : "") + item.End.Hour.ToString() + ":" + item.End.Minute.ToString() + (item.End.Minute.ToString().Length == 1 ? "0" : "");
                var lista_eventos_preenchida = GeraLista(item.nome_professor.ToString(), horario);
                Events.Add(item.Start, lista_eventos_preenchida);

            }*/
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
            if (_horarioInicial == null)
                _horarioInicial = (Events)parameters["aluno"];

            //if (parameters.)
            //if (_professor == null)
           // {
                
                
                _professor = (Professor)parameters["professor"];
                await CarregarEventosAsync();
            //}
                
           /* var navigationMode = parameters.GetNavigationMode();

            if (navigationMode != Prism.Navigation.NavigationMode.Back)
                Task.Run(() => InitializeAsync()).ConfigureAwait(false);*/
        }

        public ICommand ProfessorCommand => new Command(async () => await ProfessorHorario());
        private async Task ProfessorHorario()
        {


            await NavigationService.NavigateAsync("ProfessorPage", null, true, true);
            //Selection = null;
        }


    }
}
