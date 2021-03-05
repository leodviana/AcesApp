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
            return "calendar";
        }

        public string GetSelectedIcon()
        {
            return "calendarblack";
        }
    }
}
