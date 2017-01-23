#include <Arduino.h>

#ifndef MY_RELAY
#define MY_RELAY

class Relay{ 
  public: 
  int relayPin;
  bool relayState;
      
  Relay(int pin);
  void setRelay(boolean state);
  void on();
  void off();
};
#endif
