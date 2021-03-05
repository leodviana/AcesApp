using System;
using AcesApp.Interfaces;
using Acr.UserDialogs;
using Prism.Navigation;
using Prism.Services;

namespace AcesApp.ViewModels
{
    public class RankingPageViewModel : ViewModelBase
    {
        public RankingPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService, IApiService ApiService, IUserDialogs userDialogs) : base(navigationService, pageDialogService)
        {

        }
    }
}
