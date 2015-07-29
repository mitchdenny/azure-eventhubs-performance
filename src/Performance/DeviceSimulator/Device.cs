using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator
{
    public class Device
    {
        public Device(string @namespace, string eventHub, string publisher, string sharedAccessToken)
        {
            this.@namespace = @namespace;
            this.eventHub = eventHub;
            this.Publisher = publisher;
            this.sharedAccessToken = sharedAccessToken;
        }

        private string @namespace;
        private string eventHub;
        public string Publisher { get; private set; }
        private string sharedAccessToken;

        public async Task SendTelemetryAsync()
        {
            var url = Helper.BuildUrl(this.@namespace, this.eventHub, this.Publisher);

            var reading = new Reading();
            var serializedReading = JsonConvert.SerializeObject(reading);

            var content = new StringContent(serializedReading);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", this.sharedAccessToken);
            var response = await client.PostAsync(url, content);
            Console.WriteLine("{0} {1}", response.StatusCode, response.ReasonPhrase);
        }
    }
}
