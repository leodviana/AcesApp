using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Prism;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace AcesApp.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible, IActiveAware
    {
        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService PageDialogService { get; private set; }
        private string _title;

        public event EventHandler IsActiveChanged;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, RaiseIsActiveChanged);
        }

        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        protected ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;

            this.Title = $"Default";
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }


        public  string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        public string base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        public string base64Decode2(string sData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(sData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        public static bool InternetConnectivity()
        {

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                return true;
            }

            return false;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }
        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }
        public async Task exibeErro(string _mensagem)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("mensagem", _mensagem);
           // await NavigationService.NavigateAsync("PopupMensagemPage", navigationParams, useModalNavigation: true);
            await NavigationService.NavigateAsync("PopupMensagemPage", navigationParams, true, true);
        }
        public virtual void Destroy()
        {

        }
    }
}
