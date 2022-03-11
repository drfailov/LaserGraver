#include <Arduino.h>
#include "Motor.h"


  Motor::Motor(int _step_pin, int _dir_pin, int _en_pin, bool _invert){
    invert = _invert;
    step_pin = _step_pin;
    dir_pin = _dir_pin;
    en_pin = _en_pin;
    pinMode(step_pin, OUTPUT);
    pinMode(dir_pin, OUTPUT);
    pinMode(en_pin, OUTPUT);
    disable();
    digitalWrite(step_pin, LOW);
    digitalWrite(dir_pin, LOW);   
  }  
  void Motor::stepForward(){
    registerTime(true);
    enable();
    digitalWrite(dir_pin, (invert?LOW:HIGH));
    digitalWrite(step_pin,HIGH);
    delayMicroseconds(5);
    digitalWrite(step_pin,LOW);  
  }
  void Motor::stepForwardQuick(){ //КОСТЫЛЬ - функция шага вперёд без задержки
    enable();
    digitalWrite(dir_pin, (invert?LOW:HIGH));
    digitalWrite(step_pin,HIGH);
    delayMicroseconds(5);
    digitalWrite(step_pin,LOW);  
  }
  void Motor::stepBackward(){
    registerTime(false);
    enable();
    digitalWrite(dir_pin, (!invert?LOW:HIGH));
    digitalWrite(step_pin,HIGH);
    delayMicroseconds(5);
    digitalWrite(step_pin,LOW);  
  }
  void Motor::goForward(long count){
    for(long i=0; i<count; i++)
    {
      stepForward();
      fadeStop(count - i);
    }
  }
  void Motor::goBackward(long count){
    for(long i=0; i<count; i++)
    {
      stepBackward();
      fadeStop(count - i);
    }
  }
  void Motor::enable(){
    digitalWrite(en_pin, LOW);
  }
  void Motor::disable(){
    digitalWrite(en_pin, HIGH);
  }
  String Motor::toString(){
    return "M(" + String(step_pin) + ", " + String(dir_pin) + ")";
  }
  
  
  void Motor::fadeStop(float remaining){
    //чем больше задержка тем меньше скорость
    //чем больше коэффициент тем меньше задержка
    //чем блице к цели, тем меньше остаток
    //чем меньше остаток, тем больше задержка
    //приближение к цели: delay = 0 ... (delayMax - delayMin)
    //remaining обычно больше задержки
    //Коэффициент 0 = весь путь на минимальной скорости
    //Чем больше коэффициент, тем быстрее будет торможение
    float delay = (delayMax - delayMin) - (remaining*stopAccel); 
    if(delay > 0)
      delayMicroseconds(delay);
  }
  //вызывать эту функцию при каждом шаге. Она сама считает и соблюдает задержки.
  void Motor::registerTime(bool direction){
    if(dif() < 0) //при долгом простое программа виснет 
      lastStepTime = micros() - delayMax;
    if(dif() > delayMax)
      stepDelay = delayMax;
    if(direction != lastStepDirection){
      delay(50);
      stepDelay = delayMax;
    }
    while(dif() < stepDelay);  //delay here
    if(stepDelay > delayMin)
      stepDelay -= startAccel;
    lastStepTime = micros();  
    lastStepDirection = direction;
  }
  
  long Motor::dif(){
    return micros() - lastStepTime;
  }
