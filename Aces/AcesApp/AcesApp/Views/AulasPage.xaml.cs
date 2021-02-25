using AcesApp.Interfaces;
using Xamarin.Forms;

namespace AcesApp.Views
{
    public partial class AulasPage : ContentPage, ITabPageIcons
    {
        public AulasPage()
        {
            InitializeComponent();
        }

        public string GetIcon()
        {
            return "Email36";
        }

        public string GetSelectedIcon()
        {
            return "Email36";
        }
    }
}
