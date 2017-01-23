#include "Global.h"
#include "func.h"
#include "Serial.h" 


void setup() {
  Serial.begin(38400);
  statusLed.blink(3);
  log("OK.");
  log("---------------");
}

void loop() { 
   checkSerial();
   checkButtons();
}
