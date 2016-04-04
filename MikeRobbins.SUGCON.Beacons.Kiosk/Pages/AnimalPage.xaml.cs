﻿using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MikeRobbins.SUGCON.Beacons.Kiosk.Data;
using MikeRobbins.SUGCON.Beacons.Kiosk.Entities;
using MikeRobbins.SUGCON.Beacons.Kiosk.Services;

namespace MikeRobbins.SUGCON.Beacons.Kiosk.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AnimalPage : Page
    {
        public Animal Animal { get; set; }

        public AnimalPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Animal = (Animal)e.Parameter;
        }

        private void SponsorAnimal_OnClick(object sender, RoutedEventArgs e)
        {
           RegisterGoal();
        }

        private async void RegisterGoal()
        {
            var sitecoreApi = new SitecoreAuthenticationService();

            var authCookie = await sitecoreApi.AuthenticateAsync();

            var goalService = new SitecoreGoalService(authCookie);

            var person = LoadCurrentPerson();

            goalService.RegisterGoalAsync(Goal.SponsorAnimal, person.UniqueIdentifier, "Sponsored" + Animal.Name);
        }

        private Person LoadCurrentPerson()
        {
            return ((App)Application.Current).CurrentPerson;
        }
    }
}
