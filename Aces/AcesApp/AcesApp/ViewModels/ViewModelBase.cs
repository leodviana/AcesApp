using System;
using System.Threading.Tasks;
using Prism;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;



namespace AcesApp.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible, IActiveAware
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

        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;

            this.Title = $"Default";
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

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
