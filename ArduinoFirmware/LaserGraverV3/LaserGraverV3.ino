#include "Serial.h"
#include "Global.h"

void setup() {
  Serial.begin(115200);
  Serial.println(F("---=== FP myCNC v3 ===---"));
  Serial.println(F("by Dr. Failov."));
  Serial.print(F("Build date: "));
  Serial.println(__DATE__);
  Serial.print(F("Free RAM: "));
  Serial.print(freeRam());
  Serial.println(F(" bytes"));
  
  pinMode(LED_PIN, OUTPUT);
  pinMode(LASER_PIN, OUTPUT);
  pinMode(X_EN_PIN, OUTPUT); 
  
  digitalWrite(LED_PIN, HIGH);
  digitalWrite(LASER_PIN, LOW);
  digitalWrite(X_EN_PIN, HIGH);

  Serial.println(F("![Ready]!"));
}

void loop() {
  checkSerial();
}
