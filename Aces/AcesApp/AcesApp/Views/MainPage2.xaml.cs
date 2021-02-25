using Xamarin.Forms;

namespace AcesApp.Views
{
    public partial class MainPage2 : TabbedPage
    {
        public MainPage2()
        {
            InitializeComponent();

            Children.Add(new PerfilPage());
            Children.Add(new AulasPage());

            //Children.Add(new Localizacao());

            //if (Device.RuntimePlatform == Device.iOS)
            //    Children.Add(new TrainingView());

            // Children.Add(new MainPage());
            //Children.Add(new MainPage());
        }
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            /*if (CurrentPage is IDynamicTitle page)
            {

                NavigationPage.SetHasNavigationBar(this, true);

                NavigationPage.SetTitleView(this, page.GetTitle());
                return;
            }*/

            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
