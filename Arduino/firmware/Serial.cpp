#include "Serial.h"
#include "func.h"
#include "Global.h"
/*==============================================
////    SERIAL
==============================================*/
long time = 0;
long timeStop = 0;

void checkSerial(){
    if(Serial.available() > 0) {
        String str = Serial.readStringUntil('\n');
        commandReceived(str);
        log("---------------");
    }
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
    log(getAnalogsData());
}
void tic(){
  time = millis();
}
long tac(){
  long now = millis();
  return now - time;
}
long toc(){
    Serial.print(" time = ");
    long res = tac();
    Serial.println(res);
    return res;
}
bool isCont(){  
  //pausing
  if(button2.isDown()){
    log("Paused. button3 to continue.");
    x.hold();
    y.hold();
    while(!button3.isDown());
    log("Continue.");
  }
  //stopDelay
  if(millis() - timeStop < 1000)
    return false;   
  //stopButton 
  if(button1.isDown()){
    statusLed.onFor(300);
    timeStop = millis();
    return false;
  }
  if(Serial.available() > 0)
  {
      String str = Serial.readStringUntil('\n');
      //stopping
      if(str.equals("stop")){
        timeStop = millis();
        return false;
      }
      //pausing
      if(str.equals("pause")){
        log("Paused. send \"continue\" to continue.");
        x.hold();
        y.hold();
        while(!Serial.readStringUntil('\n').equals("continue"));
          log("Continue.");
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

String getLine(){
  while(true){    
    if(Serial.available() > 0)
    {
      String str = Serial.readStringUntil('\n');
      return str;
    }
  }
}

bool getBool(String str){//0 1
    if(str.equals("1"))
      return true;
    else
      return false;
}
