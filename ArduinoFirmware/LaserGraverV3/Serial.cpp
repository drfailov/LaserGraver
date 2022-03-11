#include <Arduino.h>
#include "Serial.h"
#include "Commands.h"

/*==============================================
////    SERIAL
==============================================*/
long time = 0;
long timeStop = 0;

void checkSerial(){
    if(Serial.available() > 0) {
        String str = Serial.readStringUntil(';');
        str.trim();
        if(str.length() > 0){
          commandReceived(str);
          Serial.println(F("---------------"));
        }
    }
}
String getString(){
  if(Serial.available() > 0) {
      String str = Serial.readStringUntil(';');
      Serial.println(F("Got string: ")); Serial.println(str);
      return str;
  }
  return "";
}
String waitString(){
  const long timeout = 2000;
  long start = millis();
  while (millis() - start < timeout){
    if(Serial.available() > 0) {
      String str = Serial.readStringUntil(';');
      Serial.println(F("Got string: ")); Serial.println(str);
      return str;
    }
  }
  return "";
}
long getLong(){
  long lng = toLong(getString());
  Serial.print(F("Got long: ")); Serial.println(lng);
  return lng;
}
String getAnalogsData(){
  String result = "analogs: ";
  for(int i=0; i<=7; i++){
    result += "A" + String(i) + "=" + String(analogRead(i)) + "   ";
  }
  return result;
}
void monitorAnalogs(){
  while(isCont())
    Serial.println(getAnalogsData());
}
void tic(){
  time = millis();
}
long tac(){
  long now = millis();
  return now - time;
}
long toc(){
    Serial.print(F(" time = "));
    long res = tac();
    Serial.println(res);
    return res;
}
bool isCont(){  
  while(Serial.available() > 0)
  {
      String str = Serial.readStringUntil(';');
      //stopping
      if(str.equals(F("stop"))){
        log("Stopping...");
        timeStop = millis();
        return false;
      }
      //pausing
      else if(str.equals(F("pause"))){
        log(F("Paused. send \"continue\" to continue."));
        while(!Serial.readStringUntil(';').equals("continue"));
          log(F("Continue."));
      }
      else{
        if(str.length() > 0){
          commandReceived(str);
          Serial.println(F("---------------"));
        }
      }
  }
  return true;
}
bool isStop(){
  return !isCont();
}
void stop(){
  timeStop = millis();
}
String log(String text){
  Serial.println(text);
  return text;
} 
String answer(String text){
  Serial.print(F("!["));
  Serial.print(text);
  Serial.println(F("]!"));
  return text;
} 
int freeRam () {
  extern int __heap_start, *__brkval; 
  int v; 
  return (int) &v - (__brkval == 0 ? (int) &__heap_start : (int) __brkval); 
}
String getValue(String data, char separator, int index){
    int found = 0;
    int strIndex[] = {0, -1 };
    int maxIndex = data.length()-1;
    for(int i=0; i<=maxIndex && found<=index; i++){
      if(data.charAt(i)==separator || i==maxIndex){
        found++;
        strIndex[0] = strIndex[1]+1;
        strIndex[1] = (i == maxIndex) ? i+1 : i;
      }
   }
   return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}
int getValueInt(String data, char separator, int index){
  return toInt(getValue(data, separator, index));
}
long toLong(String data){  
    char argc[10];
    data.toCharArray(argc, 10);
    long result = atol(argc);
    return result;
}

int toInt(String data){  
    char argc[10];
    data.toCharArray(argc, 10);
    int result = atoi(argc);
    return result;
} 



