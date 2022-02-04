using AcesApp.Interfaces;
using Xamarin.Forms;

namespace AcesApp.Views
{
    public partial class PerfilProfessorPage : ContentPage, ITabPageIcons
    {
        public PerfilProfessorPage()
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