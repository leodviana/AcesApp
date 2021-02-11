using AcesApp.Interfaces;
using AcesApp.iOS.renderers;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace AcesApp.iOS.renderers
{
    public class CustomTabbedPageRenderer : TabbedRenderer
    {
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();


            foreach (var item in TabBar.Items)
            {
                item.ImageInsets = new UIEdgeInsets(6, 5, -6, 0);
                // UIImage teste = ;
                //teste.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);

                // item.Image = GetImageFromFile("home");


            }
        }



        protected override async Task<Tuple<UIImage, UIImage>> GetIcon(Page page)
        {
            //if (page?.Navigation?.NavigationStack?.FirstOrDefault() is ITabPageIcons tabPage)
            if (page is ITabPageIcons tabPage)
                return await Task.FromResult(
                    new Tuple<UIImage, UIImage>(
                        GetImageFromFile(tabPage.GetIcon()),
                        GetImageFromFile(tabPage.GetSelectedIcon())
                    )
                );

            return await base.GetIcon(page);
        }

        private UIImage GetImageFromFile(string fileName)
        {
            return UIImage
                .FromFile(fileName)
                .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
        }
    }
}