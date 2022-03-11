
#ifndef CONFIGH
#define CONFIGH

//SIZE
  //размеры рабочей области. Слишком большие размеры будут приводить к тому, что каретка будет упираться в край
  const int X_SIZE = 5300;
  const int Y_SIZE = 4800;
//MOTORS
  //максимальная скорость мотора, 10000 адекватная для делителя шагов 1\32;
  const long speed = 10000;  
  //Скорость разгона мотора. 0 = не разгонять. чем больше число, тем быстрее разгон.
  const float startAccel = 0.05; 
  //Скорость торможения перед подходом к цели. 0 = весь путь на минимальной скорости. Чем больше число, тем меньше будет путь торможения. 
  //Этот параметр никак не связан с скоростью разгона, они считаются вообще по разному.
  const float stopAccel = 0.5; 
//MEMORY AND PERFORMANCE
  //Определяет количество памяти выделяемое под массив инструкций прожига
  //1 instruction is 10 bytes
  const int MAX_INSTRUCTIONS_COUNT = 130;
  //определяет частоту обновления позиции на экране программы
  //delay in ms between "POS" data sent
  const int POS_UPDATE_FREQUENCY_MS = 100;
//PINS
  //Пин Direction для оси X
  const int X_DIR_PIN = 2;
  //Пин Step для оси X
  const int X_STP_PIN = 5;
  //Пин (аналоговый) с концевиком оси Х
  const int X_END_APIN = A7;
  //Пин Enabled для оси X
  const int X_EN_PIN = 8;
  //Пин Direction для оси Y
  const int Y_DIR_PIN = 3;
  //Пин Step для оси Y
  const int Y_STP_PIN = 6;
  //Пин (аналоговый) с концевиком оси Y
  const int Y_END_APIN = A6;
  //Пин Enabled для оси Y
  const int Y_EN_PIN = 8;
  //Пин GPIO куда подключена подсветка стола
  const int LED_PIN = 9;
  //Пин GPIO куда подключен лазер
  const int LASER_PIN = 12;

#endif
