#include <Arduino.h>


#ifndef LINEH
#define LINEH

#include "Config.h"
#include "Motor.h"
#include "AnalogButton.cpp"

class Line{ 
public:
  const long MOTOR_DIVIDER = 7; //сколько физических шагов двигателя считать одним пикселем
  int precise_homing_steps = 100;//на сколько шагов назад будет отходить каретка после срабатывания датчика для точной перекалибровки
  Motor motor;
  AnalogButton endstop;
  long size = -1;
  long position = -1;
  
  Line(int _size, int _step_pin, int _dir_pin, int _en_pin, bool _invert, int _endstop_apin);
  int home();
  int pos();
  boolean test();
  boolean testquick();
  void stepF();
  void stepB();
  void burnF(long burnTimeMs);
  void burnB(long burnTimeMs);
  void stepFSlow(long additionalMicrosecondsDelay);
  void stepBSlow(long additionalMicrosecondsDelay);
  void goTo(long aim);
  void release();
  void center();
  void end();
  void begin();

private:
  void delayMks(long mks);
  
};
#endif
