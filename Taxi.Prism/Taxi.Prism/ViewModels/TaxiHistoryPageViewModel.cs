﻿using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Taxi.Common.Models;
using Taxi.Common.Services;

namespace Taxi.Prism.ViewModels
{
    public class TaxiHistoryPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private TaxiResponse _taxi;
        private DelegateCommand _checkPlaqueCommand;
        private bool _isRunning;
        private List<TripItemViewModel> _trips;

        public List<TripItemViewModel> Trips
        {
            get => _trips;
            set => SetProperty(ref _trips, value);
        }


        public TaxiHistoryPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Historia del Taxi";
        }

        public TaxiResponse Taxi
        {
            get => _taxi;
            set => SetProperty(ref _taxi, value);
        }

        private string _plaque;

        public string Plaque
        {
            get => _plaque;
            set => SetProperty(ref _plaque, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }


        public DelegateCommand CheckPlaqueCommand => _checkPlaqueCommand ?? (_checkPlaqueCommand = new DelegateCommand(CheckPlaqueAsync));

        private async void CheckPlaqueAsync()
        {
            Plaque = Plaque.ToUpper();
            if (string.IsNullOrEmpty(Plaque))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar una Patente.",
                    "Aceptar");
                return;
            }

            Regex regex = new Regex(@"^([A-Za-z]{3}\d{3})$");
            if (!regex.IsMatch(Plaque))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "La Patente debe empezar con 3 letras y finalizar con 3 números.",
                    "Accept");
                return;
            }

            IsRunning = true;
            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Chequee su conexión a Internet.", "Accept");
                return;
            }

            Response response = await _apiService.GetTaxiAsync(Plaque, url, "api", "/Taxis");
            IsRunning = false;

            
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            Taxi = (TaxiResponse)response.Result;

            if (Taxi.Trips==null)
            {
                Trips = null;
            }
            else
            {
                Trips = Taxi.Trips.Select(t => new TripItemViewModel(_navigationService)
                {
                    EndDate = t.EndDate,
                    Id = t.Id,
                    Qualification = t.Qualification,
                    Remarks = t.Remarks,
                    Source = t.Source,
                    SourceLatitude = t.SourceLatitude,
                    SourceLongitude = t.SourceLongitude,
                    StartDate = t.StartDate,
                    Target = t.Target,
                    TargetLatitude = t.TargetLatitude,
                    TargetLongitude = t.TargetLongitude,
                    TripDetails = t.TripDetails,
                    User = t.User
                }).ToList();
            }


            


        }
    }
}
