using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AcesApp.Interfaces;
using AcesApp.Models;
using Acr.UserDialogs;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace AcesApp.ViewModels
{
    public class ProfessorPageViewModel : ViewModelBase
    {
        private ICommand _refresh;
        IApiService apiService;
        private readonly IUserDialogs _userDialogs;
        public Events _horarioInicial;
        public Events _horarioFinal;

        // public Command voltarCommand; 
        public ProfessorPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IUserDialogs userDialogs, IApiService ApiService) : base(navigationService, pageDialogService)
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
