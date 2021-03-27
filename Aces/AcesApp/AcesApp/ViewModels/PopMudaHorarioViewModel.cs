using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class PopMudaHorarioViewModel : ViewModelBase
    {
        private ICommand _refresh;
        IApiService apiService;
        private readonly IUserDialogs _userDialogs;
        public Events _horarioInicial;
        public Events _horarioFinal;

        // public Command voltarCommand; 
        public PopMudaHorarioViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IUserDialogs userDialogs, IApiService ApiService) : base(navigationService, pageDialogService)
        {
            
            _userDialogs = userDialogs;
            apiService = ApiService;
           // Professores = new List<Professor>();
           
        }


        private string _inicio;
        public string inicio
        {
            get { return _inicio; }
            set
            {
                SetProperty(ref _inicio, value);

            }
        }

        private string _final;
        public string final
        {
            get { return _final; }
            set
            {
                SetProperty(ref _final, value);

            }
        }





        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            _horarioInicial = (Events)parameters["inicial"];
            _horarioFinal = (Events)parameters["final"];

            inicio = _horarioInicial.EventID + " " + _horarioInicial.descricao + " " + _horarioInicial.Start;
            final = _horarioFinal.EventID + " " + _horarioFinal.descricao+ " " + _horarioFinal.Start;
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
