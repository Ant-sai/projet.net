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
                        controller.Write(pin, PinValue.Low);
                    }
                    else if (etatBouton == false && controller.Read(buttonPin)==true)
                    {
                        etatBouton = true;
                        controller.Write(pin, PinValue.High);
                    }
                }
            }
            finally
            {
                controller.ClosePin(pin);
            }
        }
    }
}
