using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JobSearch.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void GoRegister(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Register());
        }

        private void GoStart(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Start());
        }
    }
}