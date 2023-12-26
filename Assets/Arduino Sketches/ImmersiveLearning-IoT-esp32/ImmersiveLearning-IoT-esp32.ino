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
      
        Serial.println("Waiting for client to send a message...");
        WebsocketsMessage msg = client.readBlocking();//readBlocking(removes the need for calling poll()) will return the first message or event received. readBlocking can also return Ping, Pong and Close messages.
       
        // log
        Serial.print("Got Message: ");
        Serial.println(msg.data());
       
        if (msg.data().equalsIgnoreCase("Need Force")) {
          digitalWrite(11, HIGH);  //Notify user to use force sensor
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
    // close the connection
    client.close();
  }
  //delay(500);
}
