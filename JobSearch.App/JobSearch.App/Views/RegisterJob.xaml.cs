﻿using JobSearch.App.Models;
using JobSearch.App.Services;
using JobSearch.App.Utility.Converters;
using JobSearch.App.Utility.Load;
using JobSearch.Domain.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
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
    public partial class RegisterJob : ContentPage
    {
        private JobService _service;
        public RegisterJob()
        {
            InitializeComponent();
            _service = new JobService();
        }

        private void GoBack(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void Save(object sender, EventArgs e)
        {
            try
            {
                TxtMessages.Text = String.Empty;
                await ValidacaoCampos();
                User user = JsonConvert.DeserializeObject<User>(App.Current.Properties["User"].ToString());

                Job job = new Job()
                {
                    Company = TxtCompany.Text,
                    JobTitle = TxtJobTitle.Text,
                    CityState = TxtCityState.Text,
                    InitialSalary = TextToDoubleConverter.ToDouble(TxtInitialSalary.Text),
                    FinalSalary = TextToDoubleConverter.ToDouble(TxtFinalSalary.Text),
                    ContractType = (RBCLT.IsChecked) ? "CLT" : "PJ",
                    TecnologyTools = TxtTecnologyTools.Text,
                    CompanyDescription = TxtCompanyDescription.Text,
                    JobDescription = TxtJobDescription.Text,
                    Benefits = TxtBenefits.Text,
                    InterestedSendEmailTo = TxtInterestedSendEmailTo.Text,
                    PublicationDate = DateTime.Now,
                    UserId = user.Id
                };

                await Navigation.PushPopupAsync(new Loading());
                ResponseService<Job> responseService = await _service.AddJob(job);
                if (responseService.IsSuccess)
                {
                    await Navigation.PopAllPopupAsync();
                    await DisplayAlert("Vaga Cadastrada!", "Vaga cadastrada com sucesso", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    if (responseService.StatusCode == 400)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var dicKey in responseService.Errors)
                        {
                            foreach (var message in dicKey.Value)
                            {
                                sb.AppendLine(message);
                            }
                        }
                        TxtMessages.Text = sb.ToString();
                    }
                    else
                    {
                        await DisplayAlert("Erro!", "Ops! Ocorreu um erro inserperado! Tente novamente mais tarde.", "OK");
                    }
                }
                await Navigation.PopAllPopupAsync();
            }
            catch (Exception ex)
            {

                await DisplayAlert("Ocorreu um erro", ex.Message, "OK");
            }
        }

        private async Task ValidacaoCampos()
        {
            if (string.IsNullOrWhiteSpace(TxtCompany.Text) || 
                string.IsNullOrWhiteSpace(TxtJobTitle.Text) || 
                string.IsNullOrWhiteSpace(TxtCityState.Text) || 
                string.IsNullOrWhiteSpace(TxtInitialSalary.Text) || 
                string.IsNullOrWhiteSpace(TxtFinalSalary.Text)
            || string.IsNullOrWhiteSpace(TxtTecnologyTools.Text) || 
            string.IsNullOrWhiteSpace(TxtJobDescription.Text) || 
            string.IsNullOrWhiteSpace(TxtInterestedSendEmailTo.Text))
            {
                throw new Exception("Campos obrigatórios");
            }
            if (!TxtInterestedSendEmailTo.Text.Contains("@"))
            {
                throw new Exception("Email inválido");
            }
        }
    }
}