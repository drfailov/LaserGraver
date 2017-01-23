#include <Arduino.h>
#include "Line.h"
#include "Serial.h"

Line::Line(int _motorPin1, int _motorPin2, int _motorPin3, int _motorPin4, int _powerRelayPin, int _endstopHomePin, long _lineSize, bool _invert, int _backShift, bool _clearDirection): 
  endstop(_endstopHomePin),
  motor(_motorPin1, _motorPin2, _motorPin3, _motorPin4, _powerRelayPin, _invert){
    lineSize = _lineSize;
    clearDirection = _clearDirection;
    backShift = _backShift;
    calibrated = false;
    clearDirection = FORWARD;
    currentPosition = 0;
}  


void Line::stepF(){
  if(!calibrated)
    calibrate();
  if(currentPosition < lineSize){
    motor.stepF();
    currentPosition ++;
    if(clearDirection == FORWARD)
      correctSteps ++;
    else
      correctSteps = 0;
  }
} 


void Line::stepB(){
  if(!calibrated)
    calibrate();
  if(currentPosition > 0){
    motor.stepB();
    currentPosition --;
    if(clearDirection == BACKWARD)
      correctSteps ++;
    else
      correctSteps = 0;
  }
}  


bool Line::isCorrect(){
  return correctSteps >= backShift;
}


void Line::calibrate(){
  calibrated = false;
  log("Calibrating " + motor.toString());
  long cnt = 0;
  while(!endstop.isDown() && isCont()){
    motor.stepB();
    cnt ++;
  }
  if(!isCont()){ motor.hold();  return;}
  delay(400);
  if(clearDirection == FORWARD)
    shiftUp();
  motor.hold();
  currentPosition = 0;
  calibrated = true;
  log("Line " + motor.toString() + " homed, dst = "+String(cnt));
  delay(201);
} 


long Line::getPosition(){
  return currentPosition;
}


long Line::getSize(){
  return lineSize;
}


void Line::goShift(long shift){
  goindex(getPosition() + shift);
}


void Line::goMax(){
  if(!calibrated)
    calibrate();
  log("Line " + motor.toString() + " to max");
  while(currentPosition++ < lineSize && isCont())
    motor.stepF();
  if(!isCont()){ motor.hold();  return;}
  delay(300);
  if(clearDirection == BACKWARD){
    shiftDown();
    delay(300);
    shiftUp();
  }
  motor.hold();
  //log("Line " + motor.toString() + " max, index = "+String(currentPosition));
}  


void Line::goMin(){
  if(!calibrated)
    calibrate();
  log("Line " + motor.toString() + " to min");
  while(currentPosition-- > 0 && isCont())
    motor.stepB();
  if(!isCont()){ motor.hold();  return;}
  delay(300);
  if(clearDirection == FORWARD){
    shiftDown();
    delay(300);
    shiftUp();
  }
  motor.hold();
  //log("Line " + motor.toString() + " mined, index = "+String(currentPosition));
}


void Line::release(){
  motor.release();
}


void Line::hold(){
  motor.hold();
}


void Line::center(){
  goindex(lineSize / 2);
}


void Line::goindex(long index){
  if(!calibrated)
    calibrate();
  log("Line " + motor.toString() + " to "+String(index));
  if(index < currentPosition){
    while(currentPosition > index && currentPosition > 0 && isCont())
      stepB();   
  }
  else if(index > currentPosition){    
    while(currentPosition < index && currentPosition < lineSize && isCont())
      stepF();    
  }
  if(!isCont()){ motor.hold();  return;}
  if(!isCorrect()){
    shiftDown();
    delay(60);
    shiftUp();
  }
  //motor.hold();
  //log("Line " + motor.toString() + " moved to "+String(index));
}



void Line::shiftDown(){
  log("Shift down " + motor.toString());
  for(int i=0; i<backShift; i++){      
    if(clearDirection == BACKWARD){
      motor.stepF();
    }
    if(clearDirection == FORWARD){
      motor.stepB();
    }
  }
  correctSteps = 0;
}
void Line::shiftUp(){
  log("Shift up " + motor.toString());
  for(int i=0; i<backShift; i++){      
    if(clearDirection == BACKWARD){
      motor.stepB();
    }
    if(clearDirection == FORWARD){
      motor.stepF();
    }
  }
  correctSteps = backShift;
}
  
