#include <Arduino.h>

#ifndef MOTORH
#define MOTORH
#include "Config.h"

class Motor{ 
  public:
  bool invert = false;
  
  Motor(int _step_pin, int _dir_pin, int _en_pin, bool _invert);  
  void stepForward();
  void stepForwardQuick();
  void stepBackward();
  void goForward(long count);
  void goBackward(long count);
  void enable();
  void disable();
  String toString();
  
  
  private:  
  int step_pin = 0;
  int dir_pin = 0;
  int en_pin = 0;
  const float delayMin = 1000000 / speed; //минимальная задержка (самая большая скорость) = шагов в секунду
  const float delayMax = delayMin * 7; //максимальная задержка (самая малая скорость)
  long stepDelay = delayMax; //какая задержка сейчас
  long lastStepTime = micros();
  boolean lastStepDirection = true;

  //блокирующая функция которая на основе оставшегося маршрута выполняет замедление шагов
  void fadeStop(float remaining);
  //вызывать эту функцию при каждом шаге. Она сама считает и соблюдает задержки.
  void registerTime(bool direction);
  long dif();
};

#endif
