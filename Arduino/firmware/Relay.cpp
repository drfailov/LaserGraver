#include <Arduino.h>
#include "Relay.h"
  
Relay::Relay(int pin){
  relayPin = pin;
  pinMode(relayPin, OUTPUT);
  off();
}
void Relay::setRelay(boolean state){
  if(state != relayState){
    relayState = state;
    digitalWrite(relayPin, state?HIGH:LOW);   
  }
}
void Relay::on(){
  setRelay(true);
}
void Relay::off(){
  setRelay(false);
}
