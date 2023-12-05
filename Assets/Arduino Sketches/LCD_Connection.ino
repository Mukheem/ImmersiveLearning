#include <LiquidCrystal.h>


LiquidCrystal lcd(1, 2, 4, 5, 6, 7);

void setup() {
  Serial.begin(9600);
  pinMode(8, INPUT);
}

void loop() {
  if (digitalRead(8) == HIGH) {
    Serial.print("TOUCH_DETECTED");
    Serial.print("/n");
    Serial.flush();
    lcd.setCursor(0, 0);
    lcd.print("Sensor is touched");
    delay(2000);
    lcd.clear();
  }
}

