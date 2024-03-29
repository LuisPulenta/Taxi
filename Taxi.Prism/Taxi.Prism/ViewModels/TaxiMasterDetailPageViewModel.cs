﻿using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Taxi.Common.Helpers;
using Taxi.Common.Models;
using Taxi.Common.Services;
using Taxi.Prism.Views;

namespace Taxi.Prism.ViewModels
{
    public class TaxiMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private DelegateCommand _modifyUserCommand;
        private static TaxiMasterDetailPageViewModel _instance;


        public TaxiMasterDetailPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            _apiService = apiService;
            LoadUser();
            LoadMenus();
        }

        public DelegateCommand ModifyUserCommand => _modifyUserCommand ?? (_modifyUserCommand = new DelegateCommand(ModifyUserAsync));

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_airport_shuttle",
                    PageName = "HomePage",
                    Title = "Nuevo viaje",
                    IsLoginRequired=false
                },
                new Menu
                {
                    Icon = "ic_local_taxi",
                    PageName = "TaxiHistoryPage",
                    Title = "Ver historial del Taxi",
                    IsLoginRequired=false
                },
                new Menu
                {
                    Icon = "ic_location_on",
                    PageName = "MyTripsPage",
                    Title = "Mis Viajes",
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "ic_supervised_user_circle",
                    PageName = "GroupPage",
                    Title = "Administrar mi grupo de usuarios",
                    IsLoginRequired=true
                },
                new Menu
                {
                    Icon = "ic_account_circle",
                    PageName = "ModifyUserPage",
                    Title = "Modificar Usuario",
                    IsLoginRequired=true
                },
                new Menu
                {
                    Icon = "ic_report",
                    PageName = "ReportPage",
                    Title = "Informar un incidente",
                    IsLoginRequired=false
                },
                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? "Cerrar sesión" : "Iniciar sesión",
                    IsLoginRequired=false
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title,
                    IsLoginRequired=m.IsLoginRequired
                }).ToList());


            
        }

        private async void ModifyUserAsync()
        {
            await _navigationService.NavigateAsync($"/TaxiMasterDetailPage/NavigationPage/{nameof(ModifyUserPage)}");
        }

        public static TaxiMasterDetailPageViewModel GetInstance()
        {
            return _instance;
        }

        public async void ReloadUser()
        {
            string url = App.Current.Resources["UrlAPI"].ToString();
            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                return;
            }

            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            EmailRequest emailRequest = new EmailRequest
            {
                Email = user.Email
            };

            Response response = await _apiService.GetUserByEmail(url, "api", "/Account/GetUserByEmail", "bearer", token.Token, emailRequest);
            UserResponse userResponse = (UserResponse)response.Result;
            Settings.User = JsonConvert.SerializeObject(userResponse);
            LoadUser();
        }



    }
}