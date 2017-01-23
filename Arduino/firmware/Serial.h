#include <Arduino.h>

#ifndef MY_SERIAL
#define MY_SERIAL

void checkSerial();
String getAnalogsData();
void monitorAnalogs();
void tic();
long tac();
long toc();
bool isCont();
bool isStop();
void stop();
String getLine();
bool getBool(String str);

#endif
