using System;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace NitsoAsset_Maui.Pages.Base
{
    public class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = ((Color)App.Current.Resources["BlueColor"]);
            BarTextColor = ((Color)App.Current.Resources["White"]);
        }

        protected override void OnAppearing()
        {
            this.Popped += Page_Popped;
            base.OnAppearing();
        }

        void Page_Popped(object sender, NavigationEventArgs e)
        {
            var page = e.Page;

            if (page is ICustomPage)
                ((ICustomPage)page).OnPageClosing();
        }

        protected override void OnDisappearing()
        {
            this.Popped -= Page_Popped;
            base.OnDisappearing();
        }
    }
}