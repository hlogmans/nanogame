# nanogame
An example repository using MQTT on NanoFramework for playing a simple trade game

## context
This code is used in a small workshop combining MQTT and the NanoFramework (Dotnet on microcontrollers). The workshop focuses on giving some explanation about MQTT because the participants probably have some knowledge about MQTT. Knowledge level of the NanoFramework and C# is probably zero.  In this workshop the participants use an ESP32 device with WiFi capabilities to connect to a RabbitMQ server using the MQTT protocol (so not AMQP). And then they extent the program with subscribing to topics and publishing messages.

## game
A small minigame is implemented  in the server using MQTT to communicate. The goal of the game is to have the most money in the end of the game. That player wins. A game takes less than 1 minute because it is fully automated. At every game start, all players are invited to register for the game, and start with 1000 credits. Then 10 rounds of buying tokens is initiated: the highest bidder gets a token. Then 10 selling rounds occur: the lowest bidder sells his token back to the server. Leftover tokens have no value.

## prerequisites
- use Visual Studio 2022 on a Windows machine (Windows for ARM won't work yet)
- install the Cross Platform feature
- install the NanoFramework plugin
- have a device ready (e.g. most ESP32 devices), you can see a recommendation list here: https://docs.nanoframework.net/content/getting-started-guides/where-to-buy-devices.html
- you might need to install COM port drivers for your laptop

## prepare device
- install dotnet tool nanoff: 'dotnet tool install --global nanoff'
- attach device and list ports: 'nanoff --listports'
- nanoff --platform ESP32 --serialport COM7 --update (use correct COM port of course)
- check if the device is listed as ESP32_REV3: 'nanoff --listdevices'


## getting started
The first steps are preparing a project:
- open Visual Studio, and choose to create a new project/solution. Project type: Console (nanoframework).
- rightclick on 'references' in the project tree, and choose 'manage nuget packages'.
- install package nanoFramework.System.Device.Wifi and nanoFramework.M2Mqtt
- open the Device Explorer (view | other windows | device explorer), choose the connected device and in the top click on Edit Network information (icons might be blank). Enter the Wifi SSID and password.
- in Main() write a method to connect to Wifi (use WifiNetworkHelper.Reconnect). If the result is true, then a connection has been made.
- then create a class to listen to MQTT topics. The MQQT class to use is 'MqttClient' and it has a connect method that uses a unique ID ('(Guid.NewGuid().ToString() or your name') and username and password to gain access to the MQTT server.
- use the subscribe method to subscribe to topics, and with Quality of Service level you want, use 'new MqttQoSLevel[] { MqttQoSLevel.AtLeastOnce }' for now.
- use the MqttMsgPublishReceived event to listen for messages (_broker.MqttMsgPublishedReceived += (and press tab to create a new method)).
- in this new method, use Debug.WriteLine to write the results to the debug console.
- run the program on the device and watch the logs
- make changes according to the information provided, rerun an look for the results...

## solving problems
- if the device is not visible in Visual Device device pane, disconnect and reconnect the ESP.
- if the device is not programmable anymore, reflash it with bare firmware (see prepare device)
- data is transfered as byte arrays. You can (un)serialize data with the Encoding.UTF8.GetBytes and GetString functions.