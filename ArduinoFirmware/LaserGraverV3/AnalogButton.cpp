#include <Arduino.h>

#ifndef ABUTTONCPP
#define ABUTTONCPP
class AnalogButton{ 
private: 
  int apinBut;
  bool lastData;
public :
  AnalogButton(int _apinBut){
    apinBut = _apinBut;
    lastData = false;
  }
  boolean isPressed(){ 
    return getAnalogBool(apinBut);
  }
  boolean isDown(){
    bool value = isPressed();
    bool result = value && !lastData;
    if(result){
      Serial.print(F("Button A")); Serial.print(String(apinBut)); Serial.println(F(" down."));
    }
    lastData = value;
    return result;
  }
  boolean isUp(){
    bool value = isPressed();
    bool result = !value && lastData;
    if(result){
      Serial.print(F("Button A")); Serial.print(String(apinBut)); Serial.println(F(" up."));
    }
    lastData = value;
    return result;
  }
  
  bool getAnalogBool(int pin)
  {
    long sum = 0;
    long cnt = 0;
    long timeRead = 100;
    long time = micros();
    while(micros() - time < timeRead){
      sum += analogRead(pin);
      cnt ++;
    }
    long avg = sum / cnt;    
    return avg < 400;
  }
  
};

#endif
