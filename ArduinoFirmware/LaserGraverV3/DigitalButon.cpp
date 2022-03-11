#include <Arduino.h>

#ifndef DBUTTONCPP
#define DBUTTONCPP

class DigitalButton{ 
private: 
  int pinBut;
  bool lastData;
public :
  DigitalButton(int _pinBut){
    pinBut = _pinBut;
    lastData = false;
    pinMode(_pinBut, INPUT);
  }
  boolean isPressed(){ 
    return getDigitalBool(pinBut);
  }
  boolean isDown(){
    bool value = isPressed();
    bool result = value && !lastData;
    if(result){
      Serial.print(F("Button ")); Serial.print(String(pinBut)); Serial.println(F(" down."));
    }
    lastData = value;
    return result;
  }
  boolean isUp(){
    bool value = isPressed();
    bool result = !value && lastData;
    if(result){
      Serial.print(F("Button ")); Serial.print(String(pinBut)); Serial.println(F(" up."));
    }
    lastData = value;
    return result;
  }
  
  bool getDigitalBool(int pin)
  {
    long total = 0;
    long cnt = 0;
    long timeRead = 100;
    long time = micros();
    while(micros() - time < timeRead){
      if(digitalRead(pin) == HIGH)
        cnt ++;
      total ++;
    }
    return cnt * 3 > total * 2; 
  }
  
};
#endif
