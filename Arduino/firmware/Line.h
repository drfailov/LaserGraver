#include <Arduino.h>

#ifndef MY_LINE
#define MY_LINE

#include "Button.h"
#include "MyStepper.cpp"
#define FORWARD true
#define BACKWARD false 

class Line{
  public:
  long lineSize;
  long currentPosition = 0;
  long correctSteps = 0;
  long backShift = 0;
  bool clearDirection; 
  bool calibrated;
  
  public:
  Line(int _motorPin1, int _motorPin2, int _motorPin3, int _motorPin4, int _powerRelayPin, int _endstopHomePin, long _lineSize, bool _invert, int _backShift, bool _clearDirection); 
  void stepF(); 
  void stepB(); 
  bool isCorrect();
  void calibrate(); 
  long getPosition();
  long getSize();
  void goShift(long shift);
  void goMax();
  void goMin();
  void release();
  void hold();
  void center();
  void goindex(long index);
  
  private:
  Button endstop;
  MyStepper motor;
  void shiftDown();
  void shiftUp();
};
#endif
