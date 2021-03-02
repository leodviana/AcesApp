using System;
using Xamarin.Forms;

namespace AcesApp.Utils
{
    public static class navegacaoAux
    {
        public static INavigation Navigation { get; private set; }

        public static Page GetMainPage()
        {
            var rootPage = new Views.LoginPage();
            Navigation = rootPage.Navigation;
            return rootPage;
        }
    }
}
