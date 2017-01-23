#include "Commands.h"
#include "Global.h"
#include "func.h"
#include "Serial.h"


void monitor(String in){
  log ("analogs:");
  monitorAnalogs();
}


void home(String in){
  x.calibrate();
  x.release();
  y.calibrate();
  y.release();
}


void gomax(String in){
  x.goMax();
  y.goMax();
}


void gomin(String in){
  x.goMin();
  y.goMin();
}


void error(String in){
  log ("testing error state.");
  statusLed.error();
}


void release(String in){ 
  x.release();
  y.release();
}


void center(String in){
  x.center();
  y.center();
  
  x.hold();
  y.hold();
}


void memory(String in){
  log("Free memory: " + String(freeRam())); 
}


void gx(String in){  
    in.replace("gx","");
    x.goindex(toLong(in));
}


void gy(String in){
    in.replace("gy","");
    y.goindex(toLong(in));
}


void gxy(String in){
    long coordx = toLong(getValue(in, ' ', 1));    
    long coordy = toLong(getValue(in, ' ', 2));    
    x.goindex(coordx);
    y.goindex(coordy);
}


void xmin(String in){  
    x.goMin();
}


void xmax(String in){  
    x.goMax();
}


void ymin(String in){  
    y.goMin();
}


void ymax(String in){  
    y.goMax();
}


void brn(String in){
    in.replace("brn","");
    long time = toLong(in);
    log("Burn for " + String(time) + "ms.");
    laser.onFor(time);
}


void xtest(String in){
  log("test X.");
  while(isCont()){
    x.goMin();
    if(!isCont()) return;
    delay(500);
    x.goMax(); 
    if(!isCont()) return;
    delay(500);   
  }
}


void ytest(String in){
  log("test Y.");
  while(isCont()){
    y.goMin();
    if(!isCont()) return;
    delay(500);
    y.goMax(); 
    if(!isCont()) return;
    delay(500);   
  }
}


void xytest(String in){
  log("test XY.");
  while(isCont()){
    x.goMin();
    if(!isCont()) return;
    delay(500);
    y.goMin();
    if(!isCont()) return;
    delay(500);
    x.goMax(); 
    if(!isCont()) return;
    delay(500); 
    y.goMax(); 
    if(!isCont()) return;
    delay(500);   
  }
}


void laseron(String in){
  log("laser on");
  laser.on();
}


void laseroff(String in){
  log("laser off");
  laser.off();
}


void lasertest(String in){
  log("test laser");
  laser.error();
}


void getxresolution(String in){
  log("X resolution = " + String(x.getSize()));
}


void getyresolution(String in){
  log("Y resolution = " + String(y.getSize()));
}


void xshift(String in){
    long coord = toLong(getValue(in, ' ', 1));    
    log("shift X to " + String(coord) + " st.");
    x.goShift(coord);
}

void yshift(String in){
    long coord = toLong(getValue(in, ' ', 1));    
    log("shift Y to " + String(coord) + " st.");
    y.goShift(coord);
}
void xhold(String in){
  log("hold X.");
  x.hold();
}
void yhold(String in){
  log("hold Y.");
  y.hold();
}
void bx(String in){
  in.replace("bx","");
  String timeS = getValue(in, '.', 0);
  String beginS = getValue(in, '.', 1);
  String endS = getValue(in, '.', 2);
  long time = toLong(timeS);   
  long begin = toLong(beginS); 
  long end = 0;   
  if(endS.equals(""))  
    end = begin;
  else
    end = toLong(endS); 
  log("BX T=" + String(time) + " B="+String(begin)+" E=" + String(end));
  x.goindex(begin);
  log("Burning...");
  laser.onForMks(time);
  for(int xc=begin; xc<=end; xc++){
    x.stepF();
    laser.onForMks(time);
  }
}
void by(String in){
  in.replace("by","");
  String timeS = getValue(in, '.', 0);
  String beginS = getValue(in, '.', 1);
  String endS = getValue(in, '.', 2);
  long time = toLong(timeS);   
  long begin = toLong(beginS); 
  long end = 0;   
  if(endS.equals(""))  
    end = begin;
  else
    end = toLong(endS); 
  log("BY T=" + String(time) + " B="+String(begin)+" E=" + String(end));
  y.goindex(begin);
  log("Burning...");
  laser.onForMks(time);
  for(int yc=begin; yc<=end; yc++){
    y.stepF();
    laser.onForMks(time);
  }
}
void getxy(String in){
  log("X = " + String(x.currentPosition));
  log("Y = " + String(y.currentPosition));
}
