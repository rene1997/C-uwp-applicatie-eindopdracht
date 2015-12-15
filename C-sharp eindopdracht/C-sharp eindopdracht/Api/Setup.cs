using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace C_sharp_eindopdracht.Api
{
    public class Setup
    {
        private async Task<string> request(string requested)
        {
            var cts = new CancellationTokenSource();
                cts.CancelAfter(5000);

            try
            {
                HttpClient client = new HttpClient();
                Uri uri = new Uri($"https://api.9292.nl/0.1/{requested}");
                var response = await client.GetAsync(uri).AsTask(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }

                string StringResponse = await response.Content.ReadAsStringAsync();
                return StringResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
