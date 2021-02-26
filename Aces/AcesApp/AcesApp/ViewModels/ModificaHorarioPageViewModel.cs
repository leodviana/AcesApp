using AcesApp.Interfaces;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcesApp.ViewModels
{
    public class ModificaHorarioPageViewModel  : ViewModelBase
    {

        IApiService apiService;
        private readonly IUserDialogs _userDialogs;
        public ModificaHorarioPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService, IApiService ApiService, IUserDialogs userDialogs) : base(navigationService, pageDialogService)
        {
            _userDialogs = userDialogs;
            apiService = ApiService;
        }
        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {

            // await InitializeAsync();
        }
    }
}
