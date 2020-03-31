using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Taxi.Common.Helpers;
using Taxi.Common.Models;
using Taxi.Common.Services;
using Taxi.Prism.Views;
using Xamarin.Forms.Maps;

namespace Taxi.Prism.ViewModels
{
    public class StartTripPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IGeolocatorService _geolocatorService;
        private readonly ApiService _apiService;
        private string _source;
        private string _buttonLabel;
        private bool _isSecondButtonVisible;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _getAddressCommand;
        private DelegateCommand _startTripCommand;
        private TripResponse _tripResponse;
        private Position _position;

        public StartTripPageViewModel(INavigationService navigationService, IGeolocatorService geolocatorService, ApiService apiService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _geolocatorService = geolocatorService;
            _apiService = apiService;
            Title = "Comenzar Viaje";
            ButtonLabel = "Iniciar Viaje";
            IsEnabled = true;
            LoadSourceAsync();
        }

        public DelegateCommand GetAddressCommand => _getAddressCommand ?? (_getAddressCommand = new DelegateCommand(LoadSourceAsync));

        public DelegateCommand StartTripCommand => _startTripCommand ?? (_startTripCommand = new DelegateCommand(StartTripAsync));

        public string Plaque { get; set; }

        public bool IsSecondButtonVisible
        {
            get => _isSecondButtonVisible;
            set => SetProperty(ref _isSecondButtonVisible, value);
        }

        public string ButtonLabel
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

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

        public string Source
        {
            get => _buttonLabel;
            set => SetProperty(ref _buttonLabel, value);
        }

        private async void LoadSourceAsync()
        {
            await _geolocatorService.GetLocationAsync();
            if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
            {
                _position = new Position(_geolocatorService.Latitude, _geolocatorService.Longitude);
                Geocoder geoCoder = new Geocoder();
                IEnumerable<string> sources = await geoCoder.GetAddressesForPositionAsync(_position);
                List<string> addresses = new List<string>(sources);

                if (addresses.Count > 0)
                {
                    Source = addresses[0];
                }
            }
        }

        private async void StartTripAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }
            IsRunning = true;
            IsEnabled = false;

            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Revise su conexión a Internet",
                    "Aceptar");
                return;
            }

            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            TripRequest tripRequest = new TripRequest
            {
                Address = Source,
                Latitude = _geolocatorService.Latitude,
                Longitude = _geolocatorService.Longitude,
                Plaque = Plaque,
                UserId = new Guid(user.Id)
            };

            Response response = await _apiService.NewTripAsync(url, "api", "/Trips", tripRequest, "bearer", token.Token);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            _tripResponse = (TripResponse)response.Result;
            IsSecondButtonVisible = true;
            ButtonLabel = "Finalizar viaje";
            StartTripPage.GetInstance().AddPin(_position, Source, "Iniciar Viaje", PinType.Place);
            IsRunning = false;
            IsEnabled = true;

        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Plaque))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar una Patente",
                    "Aceptar");
                return false;
            }

            Regex regex = new Regex(@"^([A-Za-z]{3}\d{3})$");
            if (!regex.IsMatch(Plaque))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "La Patente debe tener 3 Letras y 3 Números",
                    "Aceptar");
                return false;
            }

            return true;
        }
    }
}