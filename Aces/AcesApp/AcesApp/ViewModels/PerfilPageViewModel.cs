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
    public class PerfilPageViewModel : ViewModelBase
    {
        public PerfilPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IApiService ApiService) : base(navigationService, pageDialogService)
        {

        }
    }
}
