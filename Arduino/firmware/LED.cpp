#include <Arduino.h>
#include "LED.h"
#include "Serial.h"

LED::LED(int ledPin){
  pinLed = ledPin;
  pinMode(pinLed, OUTPUT);
  off();
}  
void LED::set(bool value){
  if(value != relayState){
    relayState = value;
    digitalWrite(pinLed, value?HIGH:LOW);     
  }
}
void LED::on(){
  set(true);
}
void LED::off(){
  set(false);
}
void LED::blink(int times){
  for(int i=0; i<times; i++){
    onFor(30);
    delay(100);
  }
}
void LED::dim(int value, int ms){
  int lightTime = ms;
  int darkTime = 255-ms;
  long begin = millis();
  while(millis() - begin < ms){
    digitalWrite(pinLed, HIGH);
    delayMicroseconds(lightTime); 
    digitalWrite(pinLed, LOW); 
    delayMicroseconds(darkTime); 
  }
}
void LED::fadeOff(int ms){
  int stageTime = ms/255;
  for(int i=255; i>0; i--)
    dim(i, stageTime);
}

void LED::error(){
  for(;isCont();){
    onFor(500);
    if(!isCont()) return;
    delay(500);
  }
}

void LED::onFor(long ms){
    on();
    delay(ms);
    off();
}

void LED::onForMks(long ms){
    on();
    delayMicroseconds(ms);
    off();
}
