﻿using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using MikeRobbins.SUGCON.Beacons.Kiosk.Contracts;
using MikeRobbins.SUGCON.Beacons.Kiosk.Data;
using MikeRobbins.SUGCON.Beacons.Kiosk.Services.Base;

namespace MikeRobbins.SUGCON.Beacons.Kiosk.Services
{
    public class SitecoreGoalService : SitecoreServiceBase, ISitecoreGoalService
    {
        private readonly HttpCookie _authCookie;

        public SitecoreGoalService(HttpCookie authCookie)
        {
            _authCookie = authCookie;
        }

        public async Task RegisterGoalAsync(Goal goal, string userUniqueIdentifier, string goalText)
        {
            await ThreadPool.RunAsync(async delegate { await RegisterGoalAsyncTask(goal, userUniqueIdentifier, goalText); });
        }

        private async Task RegisterGoalAsyncTask(Goal goal, string userUniqueIdentifier, string goalText)
        {
            var filter = new HttpBaseProtocolFilter();

            filter.CookieManager.SetCookie(_authCookie);

            using (var client = new HttpClient(filter))
            {
                var goalJson = BuildJsonGoal(goal, userUniqueIdentifier, goalText);

                var itemResult = await client.PostAsync(new Uri(BaseUrl + "/MikeRobbins-SUGCON-Beacons-Website-Analytics-Controllers/Goal/"), goalJson);

                itemResult.EnsureSuccessStatusCode();
            }
        }

        private IHttpContent BuildJsonGoal(Goal goal, string userUniqueIdentifier, string goalText)
        {
            var goalName = GetGoalName(goal);
            var goalId = GetGoalId(goal);

            IHttpContent content = new HttpStringContent(
                    "{ \"Id\": \"" + goalId +
                    "\" , \"Name\": \"" + goalName +
                    "\" , \"ContactUniqueIdentifier\": \"" + userUniqueIdentifier +
                    "\" , \"GoalText\": \"" + goalText + "\"",
                    UnicodeEncoding.Utf8,
                    "application/json");

            return content;
        }

        private static string GetGoalName(Goal goal)
        {
            string goalName;
            switch (goal)
            {
                case Goal.SponsorAnimal:
                    goalName = "Sponsor Animal";
                    break;
                case Goal.BuyToy:
                    goalName = "Buy Toy";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
            }

            return goalName;
        }

        private static string GetGoalId(Goal goal)
        {
            string goalId;
            switch (goal)
            {
                case Goal.SponsorAnimal:
                    goalId = "{94FD9EB7-1AB8-462F-A330-A784274071FB}";
                    break;
                case Goal.BuyToy:
                    goalId = "{1904F50A-8FDA-4E5E-B7EF-3916DEAADC84}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
            }

            return goalId;
        }
    }
}