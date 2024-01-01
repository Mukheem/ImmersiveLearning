#include <LiquidCrystal.h>
#include <Adafruit_NeoPixel.h>


LiquidCrystal lcd(1, 2, 4, 5, 6, 7);

#define PIN            11   
#define NUMPIXELS      24  
const int fsrPin = A1;  // Connecting the force sensor to analog pin A1
int fsrValue;           // Variable to store FSR reading

Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);
bool lightsOn = false;

void setup() {
  Serial.begin(9600);
  pinMode(8, INPUT);

  strip.begin();
  strip.show();  // Initialize all pixels to 'off'

  Serial.println("Arduino is ready");
}

void loop() {

  int fsrValue;  // Read the FSR sensor value
  
  //First check if the touch sensor is touched
  if (digitalRead(8) == HIGH) {
    Serial.print("TOUCH_DETECTED");
    Serial.print("\n");
    Serial.flush();
    lcd.setCursor(0, 0);
    lcd.print("Sensor is touched");
    // delay(2000);
    lcd.clear();
  }

  //If the touch sensor is free, read the force sensor value
  else if(digitalRead(8) == LOW){
    Serial.println("Touch is free");
    fsrValue =  analogRead(fsrPin);
    Serial.println(fsrValue); 

      // delay(800);                    

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
  



