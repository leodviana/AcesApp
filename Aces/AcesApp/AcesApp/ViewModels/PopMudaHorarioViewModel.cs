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
        
        // public Command voltarCommand; 
        public PopMudaHorarioViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IUserDialogs userDialogs, IApiService ApiService) : base(navigationService, pageDialogService)
        {
            
            _userDialogs = userDialogs;
            apiService = ApiService;
           // Professores = new List<Professor>();
           
        }

        

        
        

       

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
           
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
