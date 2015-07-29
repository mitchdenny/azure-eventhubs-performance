using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSimulator
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var simulatorId = Guid.NewGuid();
            var devices = new List<Device>();

            for (var deviceIndex = 0; deviceIndex < 100; deviceIndex++)
            {
                var publisher = string.Format("{0}-device{1}", simulatorId, deviceIndex);
                var url = Helper.BuildUrl("azure-eventhubs-performance", "readings", publisher);
                var token = Helper.GenerateToken(
                    "azure-eventhubs-performance",
                    "readings",
                    publisher,
                    "Send",
                    "nFe/qIwesKYJmGte0AHeDLZzmIlHCgB21qDRV+ZALUU=",
                    DateTimeOffset.Now.AddDays(2)
                    );

                var device = new Device("azure-eventhubs-performance", "readings", publisher, token);
                devices.Add(device);
            }

            foreach (var device in devices)
            {
                Console.Write("{0}...", device.Publisher);

                Task.Run(device.SendTelemetryAsync).Wait();

                Console.WriteLine("done!");
            }
        }
    }
}
