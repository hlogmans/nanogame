using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Messages;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace NanoGameClient
{
    internal class GameClient
    {
        private MqttClient _broker;

        private Encoding _encoding = Encoding.UTF8;

        public GameClient(string server, string username, string password)
        {
            _broker = new MqttClient(server);
            _broker.ConnectionOpened += (_, _) => Debug.WriteLine("Connection OPEN");
            _broker.ConnectionClosed += (_, _) => Debug.WriteLine("connection CLOSED");

            var connectResult =_broker.Connect(Guid.NewGuid().ToString(), username, password);
            if (connectResult != MqttReasonCode.Success)
            {
                Debug.WriteLine($"Failed to connect to the server, code = {connectResult}");
            }
            else Debug.WriteLine("Connected to the MQTT server");
        }

        internal void Run()
        {
            // subscribe to topic "game/welcome"

            _broker.Subscribe(new string[] { "game/welcome" }, new MqttQoSLevel[] { MqttQoSLevel.AtLeastOnce });
            _broker.MqttMsgPublishReceived += _broker_MqttMsgPublishReceived;
            //_broker.Publish("game/information", Encoding.UTF8.GetBytes("********************************"));
        }

        private void _broker_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Debug.WriteLine($"{e.Topic} -> {_encoding.GetString(e.Message, 0, e.Message.Length)}");
        }
    }
}
