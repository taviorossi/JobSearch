using JobSearch.App.Models;
using JobSearch.App.Services;
using JobSearch.Domain.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JobSearch.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Start : ContentPage
    {
        private JobService _service;
        private ObservableCollection<Job> _listOfJobs;
        private SearchParams _searchParams;
        public Start()
        {
            InitializeComponent();

            _service = new JobService();
        }

        private void GoVisualizer(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Visualizer());
        }

        private void GoRegisterJob(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterJob());
        }

        private void FocusWord(object sender, EventArgs e)
        {
            TxtWord.Focus();
        }

        private void FocusCityState(object sender, EventArgs e)
        {
            TxtCityState.Focus();
        }

        private void Logout(object sender, EventArgs e)
        {
            App.Current.Properties.Remove("User");
            App.Current.SavePropertiesAsync();
            App.Current.MainPage = new Login();
        }

        private async void Search(object sender, EventArgs e)
        {

            string word = TxtWord.Text;
            string cityState = TxtCityState.Text;

            _searchParams = new SearchParams() { Word = word, CityState = cityState, NumberOfPage = 1 };

            ResponseService<List<Job>> responseService = await _service.GetJobs(_searchParams.Word, _searchParams.CityState, _searchParams.NumberOfPage);

            if (responseService.IsSuccess)
            {
                //Colocar dentro da CollectionView
                _listOfJobs = new ObservableCollection<Job>(responseService.Data);
                ListOfJobs.ItemsSource = _listOfJobs;
                ListOfJobs.RemainingItemsThreshold = 1;
            }
            else
            {
                await DisplayAlert("Erro!", "Ops! Ocorreu um erro inserperado! Tente novamente mais tarde.", "OK");
            }
        }

        private async void InfinitySearch(object sender, EventArgs e)
        {
            _searchParams.NumberOfPage++;

            ResponseService<List<Job>> responseService = await _service.GetJobs(_searchParams.Word, _searchParams.CityState, _searchParams.NumberOfPage);

            if (responseService.IsSuccess)
            {
                var listOfJobsFromService = responseService.Data;
                foreach (var item in listOfJobsFromService)
                {
                    _listOfJobs.Add(item);
                }
            }
            else
            {
                await DisplayAlert("Erro!", "Ops! Ocorreu um erro inserperado! Tente novamente mais tarde.", "OK");
            }
        }
    }
}