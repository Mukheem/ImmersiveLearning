/*
  Minimal Esp32 Websockets Server

  This sketch:
        1. Connects to a WiFi network
        2. Starts a websocket server on port 80
        3. Waits for client connections. Checks for every 1 Second.
        4. Once a client is available,it connects with it.
        5. Until the client is connected, it checks for any message or event from the client. This happens every 500 milliseconds.
        6. When there is a message from the client, it lits small led and waits for force sensor value. 
        7. When force is applied on the sensor, it reads the value and  messageSends an "echo" message to the client
        8. closes the connection and when client is unavailable.
 
  Base Example By Gil Maimon, Modifications & additional code by Mukheem.
  https://github.com/gilmaimon/ArduinoWebsockets
*/

#include <ArduinoWebsockets.h>
#include <WiFi.h>

const char* ssid = "M";
const char* password = "123mukhee";

using namespace websockets;

WebsocketsServer server;
WebsocketsClient client;
// Variable to store ADC value ( 0 to 1023 )
int forceSensorValue = 0;
void setup() {
  //Setting pin modes
  pinMode(11, OUTPUT);  // TO notify user that he/she can use force sensor
  pinMode(13, OUTPUT);  // To notifu user when he/she is using force sensor
  Serial.begin(115200);
  // Connect to wifi
  WiFi.begin(ssid, password);

  // Wait some time to connect to wifi
  for (int i = 0; i < 15 && WiFi.status() != WL_CONNECTED; i++) {
    Serial.print(".");
    delay(1000);
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());  //You can get IP address assigned to ESP

  server.listen(81);
  Serial.print("Is server live? ");
  Serial.println(server.available());
}

void loop() {

  if (server.poll()) {  //server.poll() checks if any client is waiting to connect
    Serial.println("Client is available to connect...");
    client = server.accept();  // Accept() --> what server.accept does, is: "server, please wait until there is a client knocking on the door. when there is a client knocking, let him in and give me it's object".
    Serial.println("Client connected...");

    while (client.available()) {
      if (client.poll()) {  //client.poll() checks if there are any new messages or events
        Serial.println("Client sent a message...");
        WebsocketsMessage msg = client.readBlocking();
        digitalWrite(11, HIGH);  //Notify user to use force sensor
        // log
        Serial.print("Got Message: ");
        Serial.println(msg.data());
       
        if (msg.data().equalsIgnoreCase("Need Force")) {
          Serial.println("Reading value from Force Sensor...");
          while (forceSensorValue <= 20) {

            analogReadResolution(10);  // This statement tells in how many bits the AnalogRead should happen.
            // analogRead function returns the integer 10 bit integer (0 to 1023)
            forceSensorValue = analogRead(A1);

            if (forceSensorValue > 20) {
              digitalWrite(13, HIGH);
              Serial.print(forceSensorValue, DEC);
              Serial.print("\n");  // Sending New Line character is important to read data in unity
              Serial.flush();

              // return echo
              client.send(String(forceSensorValue));
              digitalWrite(11, LOW);  //Notify user NOT to use force sensor
              digitalWrite(13, LOW);  // Turn off built-in LED to notify user that value reading is done.
              forceSensorValue = 0;
              break;
            }
          }
        }
      }
      delay(500);
    }
    // close the connection
    client.close();
  }
  delay(500);
}
// #include <ArduinoWebsockets.h>

// #include <WiFi.h>


// const char* ssid     = "M";
// const char* password = "123mukhee";

// using namespace websockets;
// WiFiServer server(80);
// WebSocketServer webSocket = WebSocketServer(81);


// void setup()
// {
//     Serial.begin(115200);
//     pinMode(13, OUTPUT);      // set the LED pin mode

//     delay(10);

//     // We start by connecting to a WiFi network
//     Serial.println();
//     Serial.println();
//     Serial.print("Connecting to: ");
//     Serial.println(ssid);

//     WiFi.begin(ssid, password);

//     while (WiFi.status() != WL_CONNECTED) {
//         delay(500);
//         Serial.print(".");
//     }

//     Serial.println("");
//     Serial.println("WiFi connected.");
//     Serial.println("IP address: ");
//     Serial.println(WiFi.localIP());
//     //Beginning WifiServer
//     server.begin();
//     //Beginning Websocket Server
//     webSocket.begin();
//     webSocket.onEvent(webSocketEvent);
// }

// void loop(){

//   webSocket.loop(); // This keeps the websocket connection open between client and server
//  WiFiClient client = server.available();   // listen for incoming clients

//   if (client) {                             // if you get a client,
//     Serial.println("New Client.");           // print a message out the serial port
//     String currentLine = "";                // make a String to hold incoming data from the client
//     // loop while the client's connected - client.connected()
//     // if there's bytes to read from the client, - client.available()

//     // close the connection:
//     client.stop();
//     Serial.println("Client Disconnected.");
//   }
//    // Variable to store ADC value ( 0 to 1023 )
//     int forceSensorValue;
//     analogReadResolution(10);
//     // analogRead function returns the integer 10 bit integer (0 to 1023)
//     forceSensorValue = analogRead(A1);


//   if (forceSensorValue > 7) {

//     Serial.print(forceSensorValue,DEC);
//     Serial.print("\n"); // Sending New Line character is important to read data in unity
//       Serial.flush();
//     digitalWrite(13, HIGH);
//   } else {
//     Serial.println(00,DEC);
//     Serial.flush();
//     digitalWrite(13, LOW);
//   }
//   delay(700);
// }
