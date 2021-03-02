using AcesApp.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcesApp.ViewModels
{
    public class MainPage2ViewModel : ViewModelBase
    {
        public MainPage2ViewModel(INavigationService navigationService, IPageDialogService pageDialogService
        , IApiService ApiService) : base(navigationService, pageDialogService)
        {
           
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
