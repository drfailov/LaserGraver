#include "Global.h"

extern Button button1(2);
extern Button button2(3);
extern Button button3(4);
extern Button button4(5);

extern LED statusLed(13);
extern LED laser(11);

extern Line y(2, 3, 4, 5, //motor pins
              12,         //motorPowerRelay
              1,          //endstopPin
              17000,      //lineSize
              true,       //invert
              2000,       //backShift
              FORWARD);   //clearDirection  
              
extern Line x(6, 7, 8, 9, //motor pins
              10,         //motorPowerRelay
              0,          //endstopPin
              15000,      //lineSize
              false,      //invert
              400,       //backShift
              FORWARD);   //clearDirection  
