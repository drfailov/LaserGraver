#include <Arduino.h>
#include "Stepper.h"
#include "Relay.h"
#include "func.h"

class MyStepper{ 
  public:
  bool invert = false;
  Stepper stepper; 
  Relay powerRelay;
  
  MyStepper(int p1, int p2, int p3, int p4, int _powerRelay, bool _invert) : stepper(200, p1, p2, p3, p4), powerRelay(_powerRelay){
    invert = _invert;
  }  
  void stepF(){
    if(!powerRelay.relayState){
      powerRelay.on();
      delay(100);  
    }
    registerTime();
    stepper.step(invert?-1:1);
  }  
  void stepB(){
    if(!powerRelay.relayState){
      powerRelay.on();
      delay(100);  
    }
    registerTime();
    stepper.step(invert?1:-1);
  } 
  void stepF(int times){
    while(times--)
      stepF();
  }
  void stepB(int times){
    while(times--)
      stepB();
  }
  void hold(){
    powerRelay.off();
    log("Mot " + toString() + " hold.");
  } 
  void release(){
    hold();
    digitalWrite(stepper.motor_pin_1, LOW);
    digitalWrite(stepper.motor_pin_2, LOW);
    digitalWrite(stepper.motor_pin_3, LOW);
    digitalWrite(stepper.motor_pin_4, LOW);
    log("Motor " + toString() + " released.");
  }  
  String toString(){
    return "(" + String(stepper.motor_pin_1) + ", " + String(stepper.motor_pin_2) + ", " + String(stepper.motor_pin_3) + ", " + String(stepper.motor_pin_4) + ")";
  }
  
  
  private:
  long stepDelay = 2000;
  const long delayMax = 2500;
  const long delayMin = 2500;
  long lastStepTime = micros();
  void registerTime(){
    if(dif() > delayMax)
      stepDelay = delayMax;
    while(dif() < stepDelay);
    if(stepDelay > delayMin)
      stepDelay -= 4;
    lastStepTime = micros();    
  }
  long dif(){
    return micros() - lastStepTime;
  }
};
