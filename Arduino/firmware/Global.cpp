#include "Global.h"

extern Button button1(2);//stop
extern Button button2(3);//pause
extern Button button3(4);//home\continue
extern Button button4(5);//release

extern LED statusLed(13);
extern LED laser(11);

extern Line y(2, 3, 4, 5, //motor pins
              12,         //motorPowerRelay
              1,          //endstopPin
              240,      //lineSize
              true,       //invert
              5,          //backShift
              FORWARD);   //clearDirection  
              
extern Line x(6, 7, 8, 9, //motor pins
              10,         //motorPowerRelay
              0,          //endstopPin
              210,      //lineSize
              false,      //invert
              5,          //backShift
              FORWARD);   //clearDirection  
