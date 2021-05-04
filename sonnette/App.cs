using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using sonnette.Configuration;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace sonnette
{
    class App
    {
        private readonly AppConfig config;
        public App(IOptions<AppConfig> options)
        {
            this.config = options.Value;
        }

        public void Run()
        {
            Console.WriteLine("Hello World");
            GpioController controller = new GpioController(PinNumberingScheme.Board);
            var etatBouton = false;

            controller.OpenPin(config.LEDPin, PinMode.Output);
            controller.OpenPin(config.BtnPin, PinMode.InputPullUp);

            try
            {
                while (true)
                {
                    if (controller.Read(config.BtnPin) == false && etatBouton == true)
                    {
                        etatBouton = false;
                        controller.Write(config.LEDPin, PinValue.High);
                        CallWebApi(!etatBouton);
                    }
                    else if (etatBouton == false && controller.Read(config.BtnPin) == true)
                    {
                        etatBouton = true;
                        controller.Write(config.LEDPin, PinValue.Low);
                        CallWebApi(!etatBouton);
                    }
                }
            }
            finally
            {
                controller.ClosePin(config.BtnPin);
                controller.ClosePin(config.LEDPin);
            }
        }

        private bool CallWebApi(Boolean _status)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"http://{config.UrlServer}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var sonetteStatus = new sonnetteModele
            {
                status = _status
            };
            string StatusJSON = JsonConvert.SerializeObject(sonetteStatus);

            HttpResponseMessage message = client.PostAsync("/api/Sonnette", new StringContent(StatusJSON, Encoding.UTF8, "application/json")).Result;

            return message.IsSuccessStatusCode;
        }

    }
}
