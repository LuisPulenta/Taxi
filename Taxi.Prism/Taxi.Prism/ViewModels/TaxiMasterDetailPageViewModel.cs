using Newtonsoft.Json;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Taxi.Common.Helpers;
using Taxi.Common.Models;

namespace Taxi.Prism.ViewModels
{
    public class TaxiMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserResponse _user;

        public TaxiMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadUser();
            LoadMenus();
        }

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
                    Icon = "ic_action_airport_shuttle",
                    PageName = "HomePage",
                    Title = "Nuevo viaje"
                },
                new Menu
                {
                    Icon = "ic_action_local_taxi",
                    PageName = "TaxiHistoryPage",
                    Title = "Ver historial del Taxi"
                },
                new Menu
                {
                    Icon = "ic_action_people",
                    PageName = "GroupPage",
                    Title = "Administrar mi grupo de usuarios"
                },
                new Menu
                {
                    Icon = "ic_action_account_circle",
                    PageName = "ModifyUserPage",
                    Title = "Modificar Usuario"
                },
                new Menu
                {
                    Icon = "ic_action_report",
                    PageName = "ReportPage",
                    Title = "Informar un incidente"
                },
                new Menu
                {
                    Icon = "ic_action_exit_to_app",
                    PageName = "LoginPage",
                    Title = Settings.IsLogin ? "Cerrar sesión" : "Iniciar sesión"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}