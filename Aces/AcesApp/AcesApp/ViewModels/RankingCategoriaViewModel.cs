using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    
        public class RankingCategoriaViewModel : ViewModelBase
        {
            IApiService apiService;

            private readonly IUserDialogs _userDialogs;
            private ICommand _navegar;

            List<Ranking> Lista;
            ObservableCollection<Ranking> Lista_filtrada;
            private string _DentistaFilter = "";
  

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

            
        private string _title;
        public string title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);

            }
        }
        private Ranking _ranking;
        public Ranking ranking
        {
            get { return _ranking; }
            set
            {
                SetProperty(ref _ranking, value);

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

                    //Navega();
                }
            }


        private bool mostra;

        public bool Mostra
        {
            get { return mostra; }
            set
            {
                SetProperty(ref mostra, value);

            }
        }

        public string DentistaFilter
        {
            get { return _DentistaFilter; }
            set
            {
                SetProperty(ref _DentistaFilter, value);

                SearchProfessores();
            }
        }

        Task taskThro;
        private async void SearchProfessores()
        {
            if (taskThro is null || taskThro.IsCompleted)
            {
                taskThro = Task.Run(async () =>
                {
                    await Task.Delay(500);
                    GetDentistas();
                });
            }
        }
        private void GetDentistas()
        {
            try
            {


                if (string.IsNullOrEmpty(DentistaFilter))
                {
                    pacs = new ObservableCollection<Ranking>(Lista.Where(x => x.categoria == ranking.categoria).OrderBy(x => x.posicao));
                }
                else if (DentistaFilter.Trim().Length > 0)
                {

                    Lista_filtrada = new ObservableCollection<Ranking>(Lista.Where(x=>x.categoria==ranking.categoria &&  x.jogador.ToUpper().Contains(DentistaFilter.ToUpper())).OrderBy(x=>x.posicao));
                    if (Lista_filtrada.Count == 0)
                        Mostra = true;
                    pacs = Lista_filtrada;
                    //dentistas = new ObservableCollection<Dentista>(Lista.Where(x => x.nome.ToUpper().Contains(DentistaFilter.ToUpper())));
                }
            }
            catch (Exception ex)
            {
                exibeErro(ex.Message.ToString());
            }
            finally
            {
                taskThro?.Dispose();
                taskThro = null;
            }



        }
        private bool mostra_mensagem;

        public bool Mostramensagem
        {
            get { return mostra_mensagem; }
            set
            {
                SetProperty(ref mostra_mensagem, value);

            }
        }
        public RankingCategoriaViewModel(INavigationService navigationService,
                IPageDialogService pageDialogService, IApiService ApiService, IUserDialogs userDialogs)
                : base(navigationService, pageDialogService)
            {

                _userDialogs = userDialogs;
                inicializa = false;
                apiService = ApiService;
                IsActiveChanged += HandleIsActiveTrue;
                IsActiveChanged += HandleIsActiveFalse;
                pacs = new ObservableCollection<Ranking>();
                mostra = true;
                Lista_filtrada = new ObservableCollection<Ranking>();
                Lista = new List<Ranking>();
             }


            private void HandleIsActiveFalse(object sender, EventArgs e)
            {
                if (IsActive == true) return;

            }


            public ICommand navegar
            {
                get
                {
                    return _navegar ?? (_navegar = new Command<Ranking>(objeto =>
                    {
                        Ranking posicao = objeto;
                        //_navigationService.NavigateAsync("/MasterPage/NavigationPage/DentistaPage");
                        // Login();
                        // _navigationService.NavigateAsync("PermissaoPage");
                    }));
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
                   // await CarregarEventosAsync();
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
              try
              {



                if (ranking.categoria == "1")
                {
                    title = "Ranking Masculino";
                }
                else if (ranking.categoria == "2")
                {
                    title = "Ranking Feminino";
                }
                else if (ranking.categoria == "3")
                {
                    title = "Ranking Duplas";
                }
                var parametro = new Ranking();

                parametro.categoria = ranking.categoria;
                var current = Connectivity.NetworkAccess;

                var response = new Response();
                //IsRunning = true;

                _userDialogs.ShowLoading("Carregando", MaskType.Black);
                if (current == NetworkAccess.Internet)

                {
                    response = await apiService.getRanking(parametro);
                }
                else
                {
                    _userDialogs.HideLoading();
                    await exibeErro("Dispositivo não está conectado a internet!");

                    //IsRunning = false;
                    return;
                }
                //IsRunning = false;

                if (!response.IsSuccess)
                {
                    _userDialogs.HideLoading();
                    await exibeErro(response.Message);

                    return;
                }
                //var lista_eventos = new List<Events>();
                 Lista = (List<Ranking>)response.Result;
                if (Lista.Count == 0)
                {
                    mostra = true;
                }
                pacs.Clear();
                pacs = new ObservableCollection<Ranking>(Lista.Where(x=>x.categoria==ranking.categoria).OrderBy(x=>x.posicao));
                /*  Events = new EventCollection();*/


                _userDialogs.HideLoading();
              }
              catch(Exception ex)
              {
                await exibeErro(ex.Message.ToString());
                _userDialogs.HideLoading();
            }

            }
            public override async void OnNavigatedFrom(INavigationParameters parameters)
            {

            }

            public override async void OnNavigatedTo(INavigationParameters parameters)
            {
                ranking = (Ranking)parameters["ranking"];
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

