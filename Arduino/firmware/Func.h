#include <Arduino.h>
#ifndef MY_GLOBAL_FUN
#define MY_GLOBAL_FUN

String log(String text); 
bool getAnalogBool(int pin);
void commandReceived(String cmd);
void processCommand(String cmd);
String getValue(String data, char separator, int index);
long toLong(String data);
void checkButtons();
int freeRam ();

#endif
