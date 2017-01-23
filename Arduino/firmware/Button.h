#include <Arduino.h>

#ifndef MY_BUTTON
#define MY_BUTTON

class Button{ 
  public: 
  int pinBut;
  bool lastData;
  
  Button(int butPin);  
  boolean isPressed();  
  boolean isDown();  
  boolean isUp();
};
#endif
