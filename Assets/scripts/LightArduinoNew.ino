#include <Adafruit_NeoPixel.h>

#define PIN            11   // Change this to the pin connected to your Neopixel data line
#define NUMPIXELS      24  // Change this to the number of Neopixels in your LED ring
const int fsrPin = A1;  // Connect the force sensor to analog pin A0
int fsrValue;           // Variable to store FSR reading

Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);

bool lightsOn = false;

void setup() {
  Serial.begin(9600);
  strip.begin();
  strip.show();  // Initialize all pixels to 'off'

  Serial.println("Arduino is ready");
}

void loop() {
int fsrValue = analogRead(fsrPin);  // Read the FSR sensor value
 Serial.println(" Value of force sensor "); 
  Serial.println(fsrValue); 
 
  delay(800);                     // Adjust the delay based on your 

  

  if (Serial.available() > 0) {
    char data = Serial.read();
    Serial.print("Received data: ");
    Serial.println(data);

    if (data == 'L') {
      // Toggle lights on and off
      lightsOn = !lightsOn;
      updateLights();
      Serial.println(lightsOn ? "Lights turned on." : "Lights turned off.");
    }
  }
}

void updateLights() {
  for (int i = 0; i < NUMPIXELS; i++) {
    if (lightsOn) {
      // Turn on all the lights with white color
      strip.setPixelColor(i, strip.Color(255, 255, 255)); // Set color to white
    } else {
      // Turn off all the lights
      strip.setPixelColor(i, strip.Color(0, 0, 0)); // Set color to off (black)
    }
  }

  strip.show();
}
