using AcesApp.Interfaces;
using AcesApp.Models;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;

namespace AcesApp.ViewModels
{
    public class AulasPageViewModel : ViewModelBase

    {
        private ICommand _MudarHorarioCommand;
        IApiService apiService;
        private readonly IUserDialogs _userDialogs;
        public EventCollection Events { get; set; }
        public List<Professor> Professores { get; set; }
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

        private Professor professor;

        public Professor Professor
        {
            get { return professor; }
            set
            {
                SetProperty(ref professor, value);

            }
        }
        public AulasPageViewModel(INavigationService navigationService, 
            IPageDialogService pageDialogService, IApiService ApiService , IUserDialogs userDialogs) : base(navigationService, pageDialogService)
        {
             _userDialogs = userDialogs;
            apiService = ApiService;
            Events = new EventCollection();
           // Professores = new ObservableCollection<Professor>();
            inicializa = false;

            IsActiveChanged += HandleIsActiveTrue;
            IsActiveChanged += HandleIsActiveFalse;

        }
        // public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));
        public ICommand MudarHorarioCommand => new Command(async (item) => await MudarHorario(item));
      

        private async Task MudarHorario(object item)
        {

            var evento = (Events)item;  
            var inicio = evento.Start;
            var final = DateTime.Now;

            TimeSpan resultado =  inicio- final;
            
             var numero_dias  = resultado.Days;
            var numero_horas  = resultado.TotalHours;

            if(App.usuariologado.Renovacao<DateTime.Now)
            {
                _userDialogs.HideLoading();
                await exibeErro("Contrato sem vigência!");

                return;
            }
           

            if (numero_horas < 6)
            {
                _userDialogs.HideLoading();
                await exibeErro("Periodo de Remarcação Expirado!");

                return;
                // await PageDialogService.DisplayAlertAsync("app", "Aula já Realizada não é possivel Remarcação!", "Ok");
            }
            var navigationParams = new NavigationParameters();
            navigationParams.Add("aluno", item);

           
            await NavigationService.NavigateAsync("ModificaHorarioPage",navigationParams);
            //await NavigationService.NavigateAsync("PopMudaHorario", navigationParams, true, true);
            //Selection = null;
        }

        private void HandleIsActiveFalse(object sender, EventArgs e)
        {
            if (IsActive == true) return;

        }

        private async void HandleIsActiveTrue(object sender, EventArgs e)
        {
            if (IsActive == false) return;
            if (!inicializa)
            {
                await CarregarEventosAsync();
                //saber se e aqui mesmo pois vai oura vez no servidor 
               // await CarregaProfessoresAsync();

            }
                

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

        private async Task CarregaProfessoresAsync()
        {
            var parametro = new Events();
            
            Culture = CultureInfo.CreateSpecificCulture("pt-BR");
            var current = Connectivity.NetworkAccess;

            var response = new Response();
            IsRunning = true;

            _userDialogs.ShowLoading("Carregando", MaskType.Black);
            if (current == NetworkAccess.Internet)

            {
                response = await apiService.getprofessores(parametro);
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
            var lista_eventos = (List<Professor>)response.Result;
            /*  Events = new EventCollection();*/
            Professores = lista_eventos;


            _userDialogs.HideLoading();

        }


        private async Task CarregarEventosAsync()
        {
            var parametro = new Events();
            parametro.contrato = App.usuariologado.contratoId;
            Culture = CultureInfo.CreateSpecificCulture("pt-BR");
            var current = Connectivity.NetworkAccess;
           
            var response = new Response();
            IsRunning = true;
            
            _userDialogs.ShowLoading("Carregando",MaskType.Black);
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

                var horario = (item.Start.Hour.ToString().Length == 1 ? "0" : "") + item.Start.Hour.ToString() +":" +item.Start.Minute.ToString()+ (item.Start.Minute.ToString().Length == 1 ? "0" : "") + " - " + (item.End.Hour.ToString().Length == 1 ? "0" : "") + item.End.Hour.ToString()+ ":" +item.End.Minute.ToString() + (item.End.Minute.ToString().Length==1?"0":"");
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
