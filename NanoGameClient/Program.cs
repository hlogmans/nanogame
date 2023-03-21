using nanoFramework.Networking;
using System;
using System.Device.Wifi;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace NanoGameClient
{
    public class Program
    {
        const string MQTT_Username = "device";
        const string MQTT_Password = "device-user";
        const string MQTT_Server = "178.128.245.0";

        // To edit the WiFi information, go to the 'Device Explorer' and press 'Edit Network Configuration'.
        // The icons might be blank due to a recent bug, they are in the top of the view, just above 'Devices'.

        public static void Main()
        {
            try
            {
                Debug.WriteLine("Starting game app");

                Debug.WriteLine("Connecting to Wifi network");
                if (!SetupAndConnectNetwork())
                {
                    Debug.WriteLine("Failed to connect to the network.");
                    return;
                }

                var client = new GameClient(MQTT_Server, MQTT_Username, MQTT_Password);
                client.Run();
            }
            finally
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }

        /// <summary>
        /// This is a helper function to pick up first available network interface and use it for communication.
        /// </summary>
        private static bool SetupAndConnectNetwork()
        {
            CancellationTokenSource cs = new(TimeSpan.FromSeconds(10));
            if (WifiNetworkHelper.Reconnect(true, token: cs.Token)) return true;

            Debug.WriteLine($"Error connecting to network: {NetworkHelper.Status}.");

            return false;
        }
    }
}
