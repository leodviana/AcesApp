using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AcesApp.ViewModels
{
    public class PopMensagemViewModel : ViewModelBase
    {
        private ICommand _refresh;

        private string _Mensagem;

        public string Mensagem
        {
            get { return _Mensagem; }
            set
            {
                SetProperty(ref _Mensagem, value);


            }
        }

        // public Command voltarCommand; 
        public PopMensagemViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {

        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Mensagem = parameters["mensagem"].ToString();
        }

       

        public ICommand voltarCommand
        {
            get
            {
                return _refresh ?? (_refresh = new Command(async objeto =>
                {



                    await NavigationService.GoBackAsync();


                }));
            }
        }
    }
}
