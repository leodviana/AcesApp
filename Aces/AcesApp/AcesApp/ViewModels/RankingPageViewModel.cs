using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AcesApp.Interfaces;
using AcesApp.Models;
using Acr.UserDialogs;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AcesApp.ViewModels
{
    public class RankingPageViewModel : ViewModelBase
    {
        IApiService apiService;
        
        private readonly IUserDialogs _userDialogs;
        private ICommand _navegar;

        public ObservableCollection<Ranking> _pacs;
        public ObservableCollection<Ranking> pacs
        {
            get { return _pacs; }
            set
            {
                SetProperty(ref _pacs, value);

            }
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

        private bool _mostra;
        public bool mostra
        {
            get { return _mostra; }
            set
            {
                SetProperty(ref _mostra, value);

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

        private Ranking _Selection;
        public Ranking Selection
        {
            get { return _Selection; }
            set
            {
                SetProperty(ref _Selection, value);

                 Navega(_Selection);
            }
        }
        public RankingPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService, IApiService ApiService, IUserDialogs userDialogs)
            : base(navigationService, pageDialogService)
        {

            _userDialogs = userDialogs;
            inicializa = false;
            apiService = ApiService;
            IsActiveChanged += HandleIsActiveTrue;
            IsActiveChanged += HandleIsActiveFalse;
            pacs = new ObservableCollection<Ranking>();
            mostra = false;
        }


        private void HandleIsActiveFalse(object sender, EventArgs e)
        {
            if (IsActive == true) return;

        }


        public ICommand navegar
        {
            get
            {
                return _navegar ?? (_navegar = new Command<Ranking>(async objeto =>
                {
                    Ranking posicao = objeto;

                    await Navega(posicao);

                }));
            }
        }

        private async Task Navega (Ranking ranking)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("ranking", ranking);

            await NavigationService.NavigateAsync("RankingCategoriaPage", navigationParams);
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

        public override void Destroy()
        {
            IsActiveChanged -= HandleIsActiveTrue;
            IsActiveChanged -= HandleIsActiveFalse;
        }
        

        private async Task CarregarEventosAsync()
        {
            var parametro = new Ranking();
            
            
            var current = Connectivity.NetworkAccess;

            var response = new Response();
            IsRunning = true;

            _userDialogs.ShowLoading("Carregando", MaskType.Black);
            if (current == NetworkAccess.Internet)

            {
                response = await apiService.getRanking(parametro);
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
            var lista_eventos = (List<Ranking>)response.Result;
            if (lista_eventos.Count==0)
            {
                mostra = true;
            }
            pacs.Clear();
            pacs = new ObservableCollection<Ranking>( lista_eventos.Where(x=>x.posicao==1).OrderBy(x=>x.categoria));
            /*  Events = new EventCollection();*/

            
            _userDialogs.HideLoading();

        }
        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            /*var navigationMode = parameters.GetNavigationMode();

            if (navigationMode != Prism.Navigation.NavigationMode.Back)
                Task.Run(() => InitializeAsync()).ConfigureAwait(false);*/
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {

            // await InitializeAsync();
        }
    }
}
