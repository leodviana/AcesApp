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
    public class ProfessorPageViewModel : ViewModelBase 
    {
        private ICommand _refresh;
        private ICommand _voltar;
        IApiService apiService;
        private readonly IUserDialogs _userDialogs;


        private string mensagem;

        public string Mensagem
        {
            get { return mensagem; }
            set
            {
                SetProperty(ref mensagem, value);

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
        private bool _isVisible;

        public bool isVisible
        {
            get { return _isVisible; }
            set
            {
                SetProperty(ref _isVisible, value);

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

        private bool mostra_mensagem;

        public bool Mostramensagem
        {
            get { return mostra_mensagem; }
            set
            {
                SetProperty(ref mostra_mensagem, value);

            }
        }


        private Professor _Selection;
        public Professor Selection
        {
            get { return _Selection; }
            set
            {
                SetProperty(ref _Selection, value);

                Navega();
            }
        }

        
        private bool isRunning2;

        //active indicator do rodape 
        public bool IsRunning2
        {
            get { return isRunning2; }
            set
            {
                SetProperty(ref isRunning2, value);

            }
        }
        private bool _isVisible2;
        public bool isVisible2
        {
            get { return _isVisible2; }
            set
            {
                SetProperty(ref _isVisible2, value);

            }
        }

        private ObservableCollection<Professor> _dentistas;
        public ObservableCollection<Professor> dentistas
        {
            get { return _dentistas; }
            set
            {
                SetProperty(ref _dentistas, value);

            }
        }

        List<Professor> Lista;
        ObservableCollection<Professor> Lista_filtrada;
        private string _DentistaFilter = "";

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
                    dentistas = new ObservableCollection<Professor>(Lista);
                }
                else if (DentistaFilter.Trim().Length > 0)
                {

                    Lista_filtrada = new ObservableCollection<Professor>(Lista.Where(x => x.Idgrlbasic.nome.ToUpper().Contains(DentistaFilter.ToUpper())));
                    if (Lista_filtrada.Count == 0)
                        Mostra = true;
                    dentistas = Lista_filtrada;
                    //dentistas = new ObservableCollection<Dentista>(Lista.Where(x => x.nome.ToUpper().Contains(DentistaFilter.ToUpper())));
                }
            }
            catch(Exception ex)
            {
                exibeErro(ex.Message.ToString());
            }
            finally
            {
                taskThro?.Dispose();
                taskThro = null;
            }



        }
        // public Command voltarCommand; 
        public ProfessorPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IUserDialogs userDialogs, IApiService ApiService) : base(navigationService, pageDialogService)
        {

            _userDialogs = userDialogs;
            apiService = ApiService;
            dentistas = new ObservableCollection<Professor>();
            Lista_filtrada = new ObservableCollection<Professor>();
            Lista = new List<Professor>();
            mostra = false;
            // Professores = new List<Professor>();

        }

        private async Task CarregaProfessoresAsync()
        {

            isVisible = true;
            isRunning = true;
           
            Mostra = true;
            Mostramensagem = false;
            var parametro = new Events();

           // Culture = CultureInfo.CreateSpecificCulture("pt-BR");
            var current = Connectivity.NetworkAccess;

            var response = new Response();
            IsRunning = true;

            _userDialogs.ShowLoading("Carregando", MaskType.Black);
            if (current == NetworkAccess.Internet)

            {
               if (Lista!=null)
                {
                    if (Lista.Any())
                        Lista.Clear();
                }
                response = await apiService.getprofessores(parametro);
                if (!response.IsSuccess)
                {
                    _userDialogs.HideLoading();
                    await exibeErro(response.Message);

                    return;
                }
                //var lista_eventos = new List<Events>();
                 Lista = (List<Professor>)response.Result;
                /*  Events = new EventCollection();*/
                //Professores = lista_eventos;
                if (Lista.Count==0)
                {
                    Mostra = false;
                    Mostramensagem = true;
                    Mensagem = "Não há Profesores";
                    isRunning = false;
                    _isVisible = false;
                }
                else

                  dentistas = new ObservableCollection<Professor>(Lista);
            }
            else
            {
                _userDialogs.HideLoading();
                await exibeErro("Dispositivo não está conectado a internet!");

                IsRunning = false;
                return;
            }
            IsRunning = false;

            isVisible = false;
            _userDialogs.HideLoading();

        }

       /* private async Task GetDentistasasync()
        {
            isVisible = true;
            IsRunning = true;

            var parametro = new Events();
            try
            {
                if (InternetConnectivity())
                {
                    if (Lista != null)
                    {
                        if (Lista.Any())
                            Lista.Clear();
                    }

                    Lista = await apiService.getprofessores(parametro);
                    dentistas = new ObservableCollection<Professor>(Lista);


                }
                else
                {
                    // dentistas = new ObservableCollection<Dentista>();
                    mostra = true;
                    /* await PageDialogService.DisplayAlertAsync("app", "Sem conexao!", "Ok");

                     return;*/
            /*    }
          / }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("app", ex.ToString(), "Ok");
                return;
            }
            IsRunning = false;
            isVisible = false;
            isVisible2 = false;

        }*/



        

        private async Task InitializeAsync()
        {

            await CarregaProfessoresAsync();

        }


        private async void Navega()
        {
            if (Selection!=null)
            {
                var navigationParams = new Prism.Navigation.NavigationParameters();
                navigationParams.Add("professor", Selection);
                // await NavigationService.NavigateAsync("ProfessorPage", navigationParams);
                await NavigationService.GoBackAsync(navigationParams);
                Selection = null;
            }
        }


        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                Task.Run(() => InitializeAsync()).ConfigureAwait(false);
            }
            catch(Exception ex)
            {

            }
            
        }


        public ICommand voltarCommand
        {
            get
            {
                return _voltar ?? (_voltar = new Command(async objeto =>
                {



                    await NavigationService.GoBackAsync();


                }));
            }
        }

        
        public ICommand RefreshCommand
        {
            get
            {
                return _refresh ?? (_refresh = new Command(async objeto =>
                {



                    await CarregaProfessoresAsync();


                }));
            }
        }
    }
    
}
