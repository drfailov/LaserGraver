#include "func.h"
#include "Commands.h"
#include "Global.h"
#include "Serial.h"


String log(String text){
  Serial.println(text);
  return text;
}  


bool getAnalogBool(int pin)
{
  long sum = 0;
  long cnt = 0;
  long timeRead = 100;
  long time = micros();
  while(micros() - time < timeRead){
    sum += analogRead(pin);
    cnt ++;
  }
  long avg = sum / cnt;    
  return avg > 800;
}


void commandReceived(String cmd){
  log("Commands received: " + cmd);
  for(int i=0; ;i++){
    String ccmd = getValue(cmd, '|', i);
    if(ccmd.equals(""))
      break;
    else
      processCommand(ccmd);
  }
}


void processCommand(String cmd){
  log("Command received: " + cmd);
  if(cmd.equals("monitor")) monitor(cmd);
  else if(cmd.startsWith("bx") && cmd.indexOf(".") > 0) bx(cmd);
  else if(cmd.startsWith("by") && cmd.indexOf(".") > 0) by(cmd);
  else if(cmd.equals("home")) home(cmd);
  else if(cmd.equals("error")) error(cmd);
  else if(cmd.equals("gomax")) gomax(cmd);
  else if(cmd.equals("gomin")) gomin(cmd);
  else if(cmd.equals("release")) release(cmd);
  else if(cmd.equals("center")) center(cmd);
  else if(cmd.equals("memory")) memory(cmd);  
  else if(cmd.equals("xmin")) xmin(cmd);
  else if(cmd.equals("xmax")) xmax(cmd);
  else if(cmd.equals("ymin")) ymin(cmd);
  else if(cmd.equals("ymax")) ymax(cmd);
  else if(cmd.equals("xtest")) xtest(cmd);
  else if(cmd.equals("ytest")) ytest(cmd);
  else if(cmd.equals("xytest")) xytest(cmd);  
  else if(cmd.equals("laseron")) laseron(cmd);
  else if(cmd.equals("laseroff")) laseroff(cmd);
  else if(cmd.equals("lasertest")) lasertest(cmd); 
  else if(cmd.equals("getxresolution")) getxresolution(cmd); 
  else if(cmd.equals("getyresolution")) getyresolution(cmd); 
  else if(cmd.equals("xhold")) xhold(cmd);
  else if(cmd.equals("yhold")) yhold(cmd);
  else if(cmd.equals("getxy")) getxy(cmd);
  
  else if(cmd.startsWith("gx")) gx(cmd);
  else if(cmd.startsWith("gy")) gy(cmd);
  else if(cmd.startsWith("brn")) brn(cmd);
  else if(cmd.startsWith("gxy ")) gxy(cmd);
  else if(cmd.startsWith("xshift ")) xshift(cmd);
  else if(cmd.startsWith("yshift ")) yshift(cmd);
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


long toLong(String data){  
    char argc[10];
    data.toCharArray(argc, 10);
    long result = atol(argc);
    return result;
}


void checkButtons(){  
   if(button4.isDown()){
      statusLed.blink(3);
      statusLed.fadeOff(500);
      x.release();
      y.release();
   }
   if(button3.isDown()){
      statusLed.blink(4);
      home("");
   }
}

int freeRam () {
  extern int __heap_start, *__brkval; 
  int v; 
  return (int) &v - (__brkval == 0 ? (int) &__heap_start : (int) __brkval); 
}
