using AcesApp.Helpers;
using AcesApp.Interfaces;
using AcesApp.Models;
using AcesApp.Utils;
using Acr.UserDialogs;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AcesApp.ViewModels
{
    public class PerfilPageViewModel : ViewModelBase
    {
        private const string TitleAlert = "Aces APP";
        private ICommand _logout;
        private ICommand _versao;

        public byte[] imageArray;
        private ICommand _abrircameraCommand;
        private ICommand _navegarCommand;
        private ICommand _salvarCommand;
        private readonly IUserDialogs _userDialogs;
        IApiService apiService;

        //aula_dada

        private Int64 aula_nao_dada;

        public Int64 aula_dada;
        
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

        private string _plano;

        public string Plano
        {
            get { return _plano; }
            set
            {
                SetProperty(ref _plano, value);

            }
        }
        private string _nomeProfessor;

        public string NomeProfessor
        {
            get { return _nomeProfessor; }
            set
            {
                SetProperty(ref _nomeProfessor, value);

            }
        }
        private string _nome;

        public string Nome
        {
            get { return _nome; }
            set
            {
                SetProperty(ref _nome, value);

            }
        }

        private string _inicio;

        public string Inicio
        {
            get { return _inicio; }
            set
            {
                SetProperty(ref _inicio, value);

            }
        }

        private string _renova;

        public string Renova
        {
            get { return _renova; }
            set
            {
                SetProperty(ref _renova, value);

            }
        }

        private int _num_aulas;

        public int Num_Aulas
        {
            get { return _num_aulas; }
            set
            {
                SetProperty(ref _num_aulas, value);

            }
        }

        private string _contratoId;

        public string ContratoId
        {
            get { return _contratoId; }
            set
            {
                SetProperty(ref _contratoId, value);

            }
        }
        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);

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


        private string _login;

        public string Login
        {
            get { return _login; }
            set
            {
                SetProperty(ref _login, value);

            }
        }


        private Int64 _id;

        public Int64 id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);

            }
        }
        private string _status;

        public string status
        {
            get { return _status; }
            set
            {
                SetProperty(ref _status, value);

            }
        }

        private string _exibeaulas;

        public string exibeaulas
        {
            get { return _exibeaulas; }
            set
            {
                SetProperty(ref _exibeaulas, value);

            }
        }

        private ImageSource _photo;

        public ImageSource Photo
        {
            get { return _photo; }
            set
            {
                SetProperty(ref _photo, value);
                modificaCache();
            }
        }

        private async Task modificaCache()
        {
            await CachedImage.InvalidateCache(_photo, CacheType.All, true);
        }
        public PerfilPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IApiService ApiService, IUserDialogs userDialogs) : base(navigationService, pageDialogService)
        {
            _userDialogs = userDialogs;
            apiService = ApiService;
            //Photo = App.usuariologado.ImagePath;
             atribuivalores(App.usuariologado);
            
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
            {
               
                await CarregarEventosAsync();
               // await atribuivalores(App.usuariologado);
            }
                

        }

        private async Task CarregarEventosAsync()
        {
            aula_nao_dada = 0;
            aula_dada = 0;
            var parametro = new Events();
            parametro.contrato = App.usuariologado.contratoId;
           // Culture = CultureInfo.CreateSpecificCulture("pt-BR");
            var current = Connectivity.NetworkAccess;

            var response = new Response();
            IsRunning = true;

            _userDialogs.ShowLoading("Carregando", MaskType.Black);
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
            foreach (var item in lista_eventos)
            {
                if (DateTime.Now >= Convert.ToDateTime(item.Start))
                    aula_dada = aula_dada + 1;
                else
                    aula_nao_dada = aula_nao_dada + 1;
            }
            if (lista_eventos.Count==0)
            {
                if (current == NetworkAccess.Internet)

                {
                    parametro.contrato = App.usuariologado.contrato_dupla;
                    response = await apiService.getEventos(parametro);
                    lista_eventos = (List<Events>)response.Result;
                    foreach (var item in lista_eventos)
                    {
                        if (DateTime.Now >= Convert.ToDateTime(item.Start))
                            aula_dada = aula_dada + 1;
                        else
                            aula_nao_dada = aula_nao_dada + 1;
                    }
                }
                else
                {
                    _userDialogs.HideLoading();
                    await exibeErro("Dispositivo não está conectado a internet!");

                    IsRunning = false;
                    return;
                }
            }
            /*  Events = new EventCollection();*/

            exibeaulas = aula_dada + " de " + App.usuariologado.num_aulas;
            _userDialogs.HideLoading();

        }

        public override void Destroy()
        {
            IsActiveChanged -= HandleIsActiveTrue;
            IsActiveChanged -= HandleIsActiveFalse;
        }

        public ICommand LogoutCommand
        {
            get
            {
                return _logout ?? (_logout = new Command(async objeto =>
                {

                    try
                    {
                        Preferences.Clear();
                        App.usuariologado = null;
                        Page nova = navegacaoAux.GetMainPage();
                        App.Current.MainPage = nova;
                    }
                    catch(Exception ex)
                    {
                        await exibeErro(ex.Message.ToString());
                    }
                    //await PageDialogService.DisplayAlertAsync("app", "Sem conexao!", "Ok");
                    

                }));
            }
        }
        public ICommand VersaoCommand
        {
            get
            {
                return _versao ?? (_versao = new Command(async () =>
                {

                    await exibeErro(App.Current.Resources["versao"].ToString());

                }));
            }
        }
        

        public ICommand abrircameraCommand
        {
            get
            {
                return _abrircameraCommand ?? (_abrircameraCommand = new Command(objeto =>
                {

                    selecionartipodeImagem();
                }));
            }
        }

        private async void selecionartipodeImagem()
        {
            var action = await PageDialogService.DisplayActionSheetAsync("Selecione Imagem:", "Cancel", null, "Camera", "Galeria");
            if (action == "Camera")
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                }
                abrircamera();
            }
            else if (action == "Galeria")
            {
                selecionargaleria();
            }
            else
            {
                Photo = null;
                return;

            }
        }
        private async void abrircamera()
        {

            // var action = await _dialogService.DisplayActionSheetAsync("" , "Cancel", null, "Tirar nova foto", "Galeria");

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await exibeErro("Sua Camera não esta Disponivel");
                // await _dialogService.DisplayAlertAsync("Painel Studio - Perboyre", "Sua Camera não esta Disponivel", "Ok");
                //DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                // Name = _dentista.nome,
                AllowCropping = false,
                Directory = "Photos",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front,
                //RotateImage = DisplayRotation.Rotation270

            });
            // Plugin.Media.Abstractions.StoreCameraMediaOptions teste = new Plugin.Media.Abstractions.StoreCameraMediaOptions();

            if (file == null)
                return;

            // DisplayAlert("File Location", file.Path, "OK");


            Photo = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();

                if (file != null)
                {
                    imageArray = FilesHelper.ReadFully(file.GetStream());


                }
                file.Dispose();
                return stream;
            });

        }

        private async void selecionargaleria()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await exibeErro("Arquivo não Suportado"); //_dialogService.DisplayAlertAsync("Painel Studio - Perboyre", "Arquivo não Suportado", "Ok");
                //DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });


            if (file == null)
                return;

            Photo = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                if (file != null)
                {
                    imageArray = FilesHelper.ReadFully(file.GetStream());

                }
                file.Dispose();
                return stream;
            });
          
        }


        public ICommand salvarCommand
        {
            get
            {
                return _salvarCommand ?? (_salvarCommand = new Command(objeto =>
                {
                    atualizar();
                }));
            }
        }



        public ICommand navegarCommand
        {
            get
            {
                return _navegarCommand ?? (_navegarCommand = new Command(objeto =>
                {

                    atualizar();
                }));
            }
        }

        private async void mostrar()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            await exibeErro("Densidade " + mainDisplayInfo.Density.ToString() + " largura" + mainDisplayInfo.Width.ToString() + " altura " + mainDisplayInfo.Height.ToString());
        }

        private async Task atualizar()
        {
            try
            {
                IsRunning = true;
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    await salvaPerfil();
                }
                else
                {
                    await exibeErro( "Por favor Verifique sua conexao!");
                    IsRunning = false;
                    return;
                }

                IsRunning = false;
            }
            catch (Exception ex)
            {
                _userDialogs.HideLoading();
                _userDialogs.Toast("Erro ao salvar perfil.");
            }
        }

        private void limpaFormulario()
        {
            id = 0;
            Nome = "";
            Email = "";
            Senha = "";
            Login = "";
            status = "";
            Photo = "";

        }
        private async Task salvaPerfil()
        {
            IsRunning = true;
            if (string.IsNullOrEmpty(Email))
            {
                await exibeErro("Prencha o campo Email!");
                IsRunning = false;
                return;
            }


            if (string.IsNullOrEmpty(Senha))
            {
                await exibeErro("Prencha o campo Senha!");
                IsRunning = false;
                return;
            }
           
            _userDialogs.ShowLoading("Salvando");

            
            //var senhaNovaDigitadaCript = GetMd5Hash(MD5.Create(), Senha);
            var senhaNovaDigitadaCript = base64Encode(Senha);
            Usuario dentistaatualizado = new Usuario();
           

            dentistaatualizado.contratoId = Convert.ToInt32(ContratoId);
            dentistaatualizado.nome = Nome;
            dentistaatualizado.Login = Email;
            dentistaatualizado.UsuarioId = App.usuariologado.UsuarioId;
            
            dentistaatualizado.Senha = senhaNovaDigitadaCript;
            dentistaatualizado.ImagePath = "";
            dentistaatualizado.ImageArray = imageArray;

            //Lista = await apiService.getDentistas();
            // await _dialogService.DisplayAlertAsync("Painel Studio - Perboyre Castelo", _dentista.senha, "Ok");
            var response = await apiService.PutUsuario(dentistaatualizado);
            if (!response.IsSuccess)
            {
                _userDialogs.HideLoading();
                await exibeErro (response.Message);
                //?await PageDialogService.DisplayAlertAsync("Painel Studio - Perboyre Castelo", response.Message, "Ok");
                return;
            }
            var result = (Usuario)response.Result;

            // await _dialogService.DisplayAlertAsync("Painel Studio - Perboyre Castelo", result.senha, "Ok");
            limpaFormulario();
            gravaUsuarioLogado(result);
            Photo = "";
            Photo = App.usuariologado.ImagePath;
            await atribuivalores(result);
            _userDialogs.HideLoading();
            await exibeErro (response.Message);
            //await PageDialogService.DisplayAlertAsync("Painel Studio - Perboyre Castelo", response.Message, "OK");
            IsRunning = false;

            /*  Settings.ClearAllData();
              App.usuariologado = null;*/
            //  Page nova = Navegacao_aux.GetMainPage();
            // App.Current.MainPage = nova;

        }
        private async Task atribuivalores(Usuario _dentista)
        {

            try
            {
                //id = _dentista.Id;
                Nome = _dentista.nome;
                Email = _dentista.Login;


                //Senha = _dentista.Senha;//_dentista.senha_sem;
                Senha = base64Decode2(_dentista.Senha);
                
                
                ContratoId = _dentista.contratoId.ToString();

                Inicio = DateTime.Parse(_dentista.inicio.ToString()).ToString("dd/MM/yyyy");
                Renova = DateTime.Parse(_dentista.Renovacao.ToString()).ToString("dd/MM/yyyy");
                NomeProfessor = _dentista.professor;
                Plano = _dentista.plano;
                Num_Aulas = _dentista.num_aulas;
                

                Photo = _dentista.ImagePath;

                
                await FFImageLoading.Forms.CachedImage.InvalidateCache(_photo, CacheType.Memory);
                _dentista.ImageArray = null;
                /*Login = _dentista.logon;
                status = _dentista.status;

                await FFImageLoading.Forms.CachedImage.InvalidateCache(Photo, CacheType.Memory);
                _dentista.ImageArray = null;*/
            }
            catch(Exception ex)
            {
                await exibeErro(ex.Message.ToString());
            }



        }
        private async void gravaUsuarioLogado(Usuario User)
        {
           
            string usuario_logado = Preferences.Get("dentistaserializado", "");
            var _usuario = JsonConvert.DeserializeObject<Usuario>(usuario_logado);
            _usuario.Senha = User.Senha;
            _usuario.ImagePath = User.ImagePath;
            Preferences.Set("dentistaserializado", JsonConvert.SerializeObject(_usuario));
            /*if (User.Tipo_usuario == 999999999)
            {
                User.tipo = "Administrador";
                App.usuariologado = User;
                Preferences.Set("dentistaserializado", JsonConvert.SerializeObject(User));
                // Preferences.Get("dentistaserializado", JsonConvert.SerializeObject(User));
                //Settings.Grava_Settings(JsonConvert.SerializeObject(User));
                // await _navigationService.NavigateAsync("/MasterPage/NavigationPage/DentistaPage");
            }
            else
            {
                User.tipo = "Dentista";
                App.usuariologado = User;
                Preferences.Set("dentistaserializado", JsonConvert.SerializeObject(User));
                var navigationParams = new NavigationParameters();
                navigationParams.Add("paciente", User);

                // await _navigationService.NavigateAsync("/MasterPage/NavigationPage/ExamesPage", navigationParams);
            }*/
        }
    }
}
