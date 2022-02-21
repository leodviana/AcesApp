﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AcesApp.Helpers;
using AcesApp.Interfaces;
using AcesApp.Models;
using AcesApp.Utils;
using Acr.UserDialogs;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AcesApp.ViewModels
{
    public class PerfilProfessorViewModel : ViewModelBase
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
                CachedImage.InvalidateCache(_photo, CacheType.All, true);
            }
        }

        public PerfilProfessorViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IApiService ApiService, IUserDialogs userDialogs) : base(navigationService, pageDialogService)
        {
            _userDialogs = userDialogs;
            apiService = ApiService;
            Photo = App.usuariologado.ImagePath;
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
                //await CarregarEventosAsync();
                atribuivalores(App.usuariologado);
            }


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
                    catch (Exception ex)
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

        private async void atualizar()
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
                    await PageDialogService.DisplayAlertAsync(TitleAlert, "Por favor Verifique sua conexao!", "Ok");
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
            /* if (string.IsNullOrEmpty(Email))
             {
                 await PageDialogService.DisplayAlertAsync(TitleAlert, "Prencha o campo Email!", "OK");
                 IsRunning = false;
                 return;
             }


             if (string.IsNullOrEmpty(Senha))
             {
                 await PageDialogService.DisplayAlertAsync(TitleAlert, "Prencha o campo Senha!", "OK");
                 IsRunning = false;
                 return;
             }
             if (string.IsNullOrEmpty(Login))
             {
                 await PageDialogService.DisplayAlertAsync(TitleAlert, "Prencha o campo Login!", "OK");
                 IsRunning = false;
                 return;
             }

             _userDialogs.ShowLoading("Salvando");

             Usuario dentistaatualizado = new Usuario();
             dentistaatualizado.Id = id;
             dentistaatualizado.nome = Nome;
             dentistaatualizado.Email = Email;
             dentistaatualizado.logon = Login;
             dentistaatualizado.status = "0";
             dentistaatualizado.senha = Senha;
             dentistaatualizado.ImagePath = "";
             dentistaatualizado.ImageArray = imageArray;

             //Lista = await apiService.getDentistas();
             // await _dialogService.DisplayAlertAsync("Painel Studio - Perboyre Castelo", _dentista.senha, "Ok");
             var response = await apiService.PutDentista(dentistaatualizado);
             if (!response.IsSuccess)
             {
                 _userDialogs.HideLoading();
                 await PageDialogService.DisplayAlertAsync(TitleAlert, response.Message, "OK");
                 //?await PageDialogService.DisplayAlertAsync("Painel Studio - Perboyre Castelo", response.Message, "Ok");
                 return;
             }
             var result = (Dentista)response.Result;

             // await _dialogService.DisplayAlertAsync("Painel Studio - Perboyre Castelo", result.senha, "Ok");
             limpaFormulario();
             gravaUsuarioLogado(result);
             Photo = "";
             atribuivalores(result);
             _userDialogs.HideLoading();
             await PageDialogService.DisplayAlertAsync(TitleAlert, response.Message, "OK");
             //await PageDialogService.DisplayAlertAsync("Painel Studio - Perboyre Castelo", response.Message, "OK");
             IsRunning = false;

             /*  Settings.ClearAllData();
               App.usuariologado = null;*/
            //  Page nova = Navegacao_aux.GetMainPage();
            // App.Current.MainPage = nova;

        }
        private async void atribuivalores(Usuario _dentista)
        {
            //id = _dentista.Id;
            Nome = _dentista.nome;
            Email = _dentista.Login;
            Senha = _dentista.senha_sem;
           // ContratoId = _dentista.contratoId.ToString();

           // Inicio = DateTime.Parse(_dentista.inicio.ToString()).ToString("dd/MM/yyyy");
           // Renova = DateTime.Parse(_dentista.Renovacao.ToString()).ToString("dd/MM/yyyy");
           // NomeProfessor = _dentista.professor;
           // Plano = _dentista.plano;
           // Num_Aulas = _dentista.num_aulas;
            /*Login = _dentista.logon;
            status = _dentista.status;
            Photo = _dentista.ImagePath;
            await FFImageLoading.Forms.CachedImage.InvalidateCache(Photo, CacheType.Memory);
            _dentista.ImageArray = null;*/

        }
        /* private void gravaUsuarioLogado(Dentista User)
         {
             if (User.Id == 999999999)
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
             }
         }*/
    }
}
