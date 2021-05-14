using AcesApp.Interfaces;
using Xamarin.Forms;

namespace AcesApp.Views
{
    public partial class PerfilPage : ContentPage, ITabPageIcons
    {
        public PerfilPage()
        {
            InitializeComponent();
        }

        public string GetIcon()
        {
            return "profile";
        }

        public string GetSelectedIcon()
        {
            return "profileblack";
        }

       
    }
}
