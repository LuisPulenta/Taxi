﻿using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taxi.Common.Helpers;
using Taxi.Common.Models;
using Taxi.Common.Services;
using Taxi.Prism.Helpers;
using Taxi.Prism.Views;
using Xamarin.Forms.Maps;

namespace Taxi.Prism.ViewModels
{
    public class ReportPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IGeolocatorService _geolocatorService;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _reportCommand;

        public ReportPageViewModel(
            INavigationService navigationService,
            IApiService apiService,
            IGeolocatorService geolocatorService)
            : base(navigationService)
        {
            Title = "Reporte un incidente";
            IsEnabled = true;
            _navigationService = navigationService;
            _apiService = apiService;
            _geolocatorService = geolocatorService;
        }

        public DelegateCommand ReportCommand => _reportCommand ?? (_reportCommand = new DelegateCommand(ReportAsync));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public string Plaque { get; set; }

        

        public string Remark { get; set; }

        private async void ReportAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Error de conexión", "Aceptar");
                return;
            }

            await _geolocatorService.GetLocationAsync();
            var address = string.Empty;

            if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
            {
                var position = new Position(_geolocatorService.Latitude, _geolocatorService.Longitude);
                Geocoder geoCoder = new Geocoder();
                IEnumerable<string> sources = await geoCoder.GetAddressesForPositionAsync(position);
                List<string> addresses = new List<string>(sources);

                if (addresses.Count > 0)
                {
                    address = addresses[0];
                }
            }

            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var request = new IncidentRequest
            {
                Address = address,
                Latitude = _geolocatorService.Latitude,
                Longitude = _geolocatorService.Longitude,
                Plaque = Plaque,
                UserId = new Guid(user.Id),
                Remarks = Remark
            };

            await _apiService.AddIncident(url, "api", "/Trips/AddIncident", request, "bearer", token.Token);
            IsRunning = false;
            IsEnabled = true;
            await _navigationService.NavigateAsync($"/TaxiMasterDetailPage/NavigationPage/{nameof(HomePage)}");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Plaque))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Esta Patente no existe", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(Remark))
            {
                await App.Current.MainPage.DisplayAlert("Error", "No hay incidentes reportados", "Aceptar");
                return false;
            }

            return true;
        }
    }
}
