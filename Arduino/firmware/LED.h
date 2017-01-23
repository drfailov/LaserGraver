#include <Arduino.h>

#ifndef MY_LED
#define MY_LED

class LED{ 
  public: 
  int pinLed;
  bool relayState;
      
  LED(int ledPin);
  void on();
  void off();
  void set(bool value);
  void blink(int times);
  void error();
  void dim(int value, int ms);
  void fadeOff(int ms);
  void onFor(long ms);
  void onForMks(long ms);
};
#endif
