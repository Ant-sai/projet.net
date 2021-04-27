using System;
using System.Device.Gpio;

namespace sonnette
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GpioController controller = new GpioController(PinNumberingScheme.Board);
            var pin = 11;
            var buttonPin = 13;
            var etatBouton = false;

            controller.OpenPin(pin, PinMode.Output);
            controller.OpenPin(buttonPin, PinMode.InputPullUp);

            try
            {
                while (true)
                {
                    if (controller.Read(buttonPin) == false && etatBouton == true)
                    {
                        etatBouton =false;
                        controller.Write(pin, PinValue.High);
                        CallWebApi(etatBouton);
                    }
                    else if (etatBouton == false && controller.Read(buttonPin)==true)
                    {
                        etatBouton = true;
                        controller.Write(pin, PinValue.Low);
                        CallWebApi(etatBouton);
                    }
                }
            }
            finally
            {
                controller.ClosePin(pin);
            }
        }
    static private bool CallWebApi(Boolean _status)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("192.168.43.241:44308");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var sonetteStatus = new sonnetteModele{
                status = _status
            };
            string StatusJSON = JsonConvert.SerializeObject(sonetteStatus);

            HttpResponseMessage message = client.PostAsync("/api/Sonnette", new StringContent(StatusJSON, Encoding.UTF8, "application/json")).Result;

            return message.IsSuccessStatusCode;
        }
    }
}
