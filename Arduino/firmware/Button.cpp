#include <Arduino.h>
#include "Button.h"
#include "func.h"

Button :: Button(int butPin){
  pinBut = butPin;
  lastData = false;
}
boolean Button :: isPressed(){ 
  return getAnalogBool(pinBut);
}
boolean Button :: isDown(){
  bool value = isPressed();
  bool result = value && !lastData;
  if(result){
    log(String("Butt " + String(pinBut) + " down."));
  }
  lastData = value;
  return result;
}
boolean Button :: isUp(){
  bool value = isPressed();
  bool result = !value && lastData;
  if(result){
    log("Butt " + String(pinBut) + " up.");
  }
  lastData = value;
  return result;
}
