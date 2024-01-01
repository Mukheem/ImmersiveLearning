void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(13, OUTPUT);
}

void loop() {
    // Variable to store ADC value ( 0 to 1023 )
    int forceSensorValue;
    // analogRead function returns the integer 10 bit integer (0 to 1023)
    forceSensorValue = analogRead(A0);


  if (forceSensorValue > 1) {

    Serial.print(forceSensorValue, DEC);
    Serial.print("\n"); // Sending New Line character is important to read data in unity
    Serial.flush();
    digitalWrite(13, HIGH);
  } else {
    Serial.println(00,DEC);
    Serial.flush();
    digitalWrite(13, LOW);
  }
  delay(700);
}
