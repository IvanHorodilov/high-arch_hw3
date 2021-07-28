using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleApp
{
    public class GoogleAnalyticsHelper
    {
        private readonly string endpoint = "https://www.google-analytics.com/collect";
        private readonly string googleVersion = "1";
        private readonly string googleTrackingId; // UA-XXXXXXXXX-XX

        private readonly int categoriesCount = 5;
        private readonly int actionsCount = 20;
        private readonly int labelsCount = 100;
        private readonly List<string> categories;
        private readonly List<string> actions;
        private readonly List<string> labels;

        public GoogleAnalyticsHelper(string trackingId)
        {
            this.googleTrackingId = trackingId;
            categories = Enumerable.Range(1,categoriesCount).Select(i => $"Test category {i}").ToList();
            actions = Enumerable.Range(1, actionsCount).Select(i => $"Test action {i}").ToList();
            labels = Enumerable.Range(1, labelsCount).Select(i => $"Test label {i}").ToList();
        }

        public async Task GenerateEvent()
        {
            var rand = new Random();
            await TrackEvent(
                categories[rand.Next(categoriesCount)], 
                actions[rand.Next(actionsCount)], 
                labels[rand.Next(labelsCount)], 
                rand.Next(500, 800).ToString());    
        }

        public async Task<HttpResponseMessage> TrackEvent(string category, string action, string label, string clientId, int? value = null)
        {
            if (string.IsNullOrEmpty(category))
              throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrEmpty(action))
              throw new ArgumentNullException(nameof(action));

            using (var httpClient = new HttpClient())
            {
                var postData = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("v", googleVersion),
                    new KeyValuePair<string, string>("t", "event"),
                    new KeyValuePair<string, string>("tid", googleTrackingId),
                    new KeyValuePair<string, string>("cid", clientId),
                    new KeyValuePair<string, string>("ec", category),
                    new KeyValuePair<string, string>("ea", action)
                };

                if (label != null)
                {
                    postData.Add(new KeyValuePair<string, string>("el", label));
                }

                if (value != null)
                {
                    postData.Add(new KeyValuePair<string, string>("ev", value.ToString()));
                }


                return await httpClient.PostAsync(endpoint, new FormUrlEncodedContent(postData));
            }
        }
    }
}
