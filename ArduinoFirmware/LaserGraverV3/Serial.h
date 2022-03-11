#include <Arduino.h>
#ifndef SERIALH
#define SERIALH

void checkSerial();
String getString();
String waitString();
long getLong();
String getAnalogsData();
void monitorAnalogs();
void tic();
long tac();
long toc();
bool isCont();
bool isStop();
void stop();
String log(String text);
String answer(String text);
int freeRam();
String getValue(String data, char separator, int index);
int getValueInt(String data, char separator, int index);
long toLong(String data);
int toInt(String data);

#endif
