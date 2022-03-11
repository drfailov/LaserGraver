#include <Arduino.h>
#include "Commands.h"
#include "Global.h"
#include "Serial.h"


void commandReceived(String cmd) {
  Serial.print(F("Command received: ")); Serial.println(cmd);
  if (cmd.equals(F("selftest"))) selftest();
  if (cmd.equals(F("selftestquick"))) selftestquick();
  if (cmd.equals(F("home"))) home();
  if (cmd.equals(F("status"))) status();
  if (cmd.equals(F("version"))) version();
  if (cmd.equals(F("size"))) size();
  if (cmd.equals(F("pos"))) posForce();
  if (cmd.equals(F("rightslow"))) rightslow();
  if (cmd.equals(F("rightfast"))) rightfast();
  if (cmd.equals(F("leftslow"))) leftslow();
  if (cmd.equals(F("leftfast"))) leftfast();
  if (cmd.equals(F("upslow"))) upslow();
  if (cmd.equals(F("upfast"))) upfast();
  if (cmd.equals(F("downslow"))) downslow();
  if (cmd.equals(F("downfast"))) downfast();
  if (cmd.equals(F("ledoff"))) ledoff();
  if (cmd.equals(F("ledon"))) ledon();
  if (cmd.equals(F("laseron"))) laseron();
  if (cmd.equals(F("release"))) release();
  if (cmd.equals(F("upload"))) upload();
  if (cmd.equals(F("execute"))) execute();
  if (cmd.equals(F("goto"))) {
    int x = getLong();
    int y = getLong();
    goTo(x, y);
  }
  if (cmd.equals(F("burntest"))) burntest(getLong());
}

void home() {
  lineX.home();
  lineY.home();
  answer(F("OK"));
  pos();
}
void status() {
  answer(F("STATUSOK"));
}
void version() {
  Serial.print(F("!["));
  Serial.print(F("FP myCNC v3, Compile date: "));
  Serial.print(__DATE__);
  Serial.print(F(" "));
  Serial.print(__TIME__);
  Serial.print(F(", Free RAM: "));
  Serial.print(freeRam());
  Serial.print(F(" bytes."));
  Serial.println(F("]!"));
}
void selftest() {
  bool pass1 = lineX.test();
  bool pass2 = lineY.test();
  Serial.print(F("![TEST;"));
  if (pass1) Serial.print(F("PASS;"));
  else Serial.print(F("FAIL;"));
  if (pass2) Serial.print(F("PASS;"));
  else Serial.print(F("FAIL;"));
  Serial.println(F("]!"));
}
void selftestquick() {
  bool pass1 = lineX.testquick();
  bool pass2 = lineY.testquick();
  Serial.print(F("![TEST;"));
  if (pass1) Serial.print(F("PASS;"));
  else Serial.print(F("FAIL;"));
  if (pass2) Serial.print(F("PASS"));
  else Serial.print(F("FAIL"));
  Serial.println(F("]!"));
}

void size() {
  Serial.print(F("![SIZE;"));
  Serial.print(X_SIZE);
  Serial.print(F(";"));
  Serial.print(Y_SIZE);
  Serial.println(F("]!"));
}

void rightslow() {
  answer(F("OK"));
  pos(); //send current position
  while (isCont()) {
    lineX.stepFSlow(6000);
    pos();
  }
  answer(F("OK"));
  posForce(); //send current position
}
void rightfast() {
  answer(F("OK"));
  pos(); //send current position
  while (isCont()) {
    lineX.stepF();
    pos();
  }
  answer(F("OK"));
  posForce(); //send current position
}
void leftslow() {
  answer(F("OK"));
  pos(); //send current position
  while (isCont()) {
    lineX.stepBSlow(6000);
    pos();
  }
  answer(F("OK"));
  posForce(); //send current position
}
void leftfast() {
  answer(F("OK"));
  pos(); //send current position
  while (isCont()) {
    lineX.stepB();
    pos();
  }
  answer(F("OK"));
  posForce(); //send current position
}
void upslow() {
  answer(F("OK"));
  pos(); //send current position
  while (isCont()) {
    lineY.stepBSlow(6000);
    pos();
  }
  answer(F("OK"));
  posForce(); //send current position
}
void upfast() {
  answer(F("OK"));
  pos(); //send current position
  while (isCont()) {
    lineY.stepB();
    pos();
  }
  answer(F("OK"));
  posForce(); //send current position
}
void downslow() {
  answer(F("OK"));
  pos(); //send current position
  while (isCont()) {
    lineY.stepFSlow(6000);
    pos();
  }
  answer(F("OK"));
  posForce(); //send current position
}
void downfast() {
  answer(F("OK"));
  pos(); //send current position
  while (isCont()) {
    lineY.stepF();
    pos();
  }
  answer(F("OK"));
  posForce(); //send current position
}

long lastPosSent = millis();
bool pos() {
  pos(POS_UPDATE_FREQUENCY_MS);
}
bool pos(long periodMs) {
  if (millis() - lastPosSent > periodMs) {
    posForce();
    lastPosSent = millis();
    return true;
  }
  return false;
}
void posForce() {
  //answer("POS;" + String(lineX.pos()) + ";" + String(lineY.pos()));
  int x = lineX.pos();
  int y = lineY.pos();
  Serial.print(F("![POS;"));
  Serial.print(x);
  Serial.print(F(";"));
  Serial.print(y);
  Serial.println(F("]!"));
}
void ledoff() {
  digitalWrite(LED_PIN, LOW);
  answer(F("OK"));
}
void ledon() {
  digitalWrite(LED_PIN, HIGH);
  answer(F("OK"));
}
void laseron() {
  answer(F("OK"));
  long timeStart = millis();
  digitalWrite(LASER_PIN, HIGH);
  while (isCont());
  digitalWrite(LASER_PIN, LOW);
  answer(F("OK"));
}
void release() {
  lineX.release();
  lineY.release();
  answer(F("OK"));
}
void goTo(int x, int y) {
  //goto;5;5;
  lineY.goTo(y);
  lineX.goTo(x);
  answer(F("OK"));
  pos();
}

void burntest(long time) {
  //burntest;5;
    int steps = 50;
    pos();
    for(int i=0; i<steps; i++){
      lineX.burnF(time);
    }
    answer(F("OK"));
    pos();
}


//временно - тест механики
//void burntest(long time) {
  //burntest;5;
    
//  int steps = 4000;
//  pos();
//  answer(F("OK"));
//  for (int y = 0; y < steps; y++) {
//    digitalWrite(LASER_PIN, HIGH);
//    delayMicroseconds(400);
//    digitalWrite(LASER_PIN, LOW);
//    delay(10);
//    //move X forward
//    for (int i = 0; i < steps; i++) {
//      digitalWrite(X_DIR_PIN, LOW);
//      digitalWrite(X_STP_PIN,HIGH);
//      delayMicroseconds(5);
//      digitalWrite(X_STP_PIN,LOW);  
//      delayMicroseconds(400);
//    }
//    pos();
//    delay(10);
//    digitalWrite(LASER_PIN, HIGH);
//    delayMicroseconds(800);
//    digitalWrite(LASER_PIN, LOW);
//    delay(10);
//
//    //step Y forward
//    for (int i = 0; i < 10; i++) {
//      digitalWrite(Y_DIR_PIN, LOW);
//      digitalWrite(Y_STP_PIN,HIGH);
//      delayMicroseconds(5);
//      digitalWrite(Y_STP_PIN,LOW);  
//      delayMicroseconds(1000);
//    }
//
//    //move X backward
//    for (int i = 0; i < steps; i++) {
//      digitalWrite(X_DIR_PIN, HIGH);
//      digitalWrite(X_STP_PIN,HIGH);
//      delayMicroseconds(5);
//      digitalWrite(X_STP_PIN,LOW);  
//      delayMicroseconds(400);
//    }
//    pos();
//    
//  }
//  answer(F("OK"));
//}






typedef struct {
  int time;
  int X1;
  int X2;
  int Y;
} BurnMark;

BurnMark commandCache[MAX_INSTRUCTIONS_COUNT];
int commandCount = 0;
int retries = 0;
bool isEngravingNow = false;

void upload() {
  if (isEngravingNow) {
    log(F("Can't upload while engraving is running."));
    return;
  }
  //upload;t_x1_y1_x2_y2;5_20_600_50_600;end;

  commandCount = 0;
  for (int i = 0; i < MAX_INSTRUCTIONS_COUNT; i++) {
    String next = waitString();
    if (next.equals(F("end")) || next.indexOf('_') == -1)
      break;
    int time = getValueInt(next, '_', 0);
    int x1 = getValueInt(next, '_', 1);
    int y1 = getValueInt(next, '_', 2);
    int x2 = getValueInt(next, '_', 3);
    int y2 = getValueInt(next, '_', 4);
    commandCache[commandCount].time = time;
    commandCache[commandCount].X1 = x1;
    commandCache[commandCount].X2 = x2;
    commandCache[commandCount].Y = y1;
    commandCount++;
  }
  //print report
  delay(10 + (retries * 20));
  Serial.print(F("Parsed ")); Serial.print(commandCount); Serial.println(F(" commands."));
  Serial.print(F("Free RAM = ")); Serial.print(freeRam()); Serial.println(F(" bytes."));
  //В ответ отправляет контрольные суммы:
  //- общее количество команд принятых
  //- Сумма всех координат У по модулю 1000
  //- сумма всех длин отрезков по модулю 1000
  //- сумма всего времени обжига по модулю 1000
  //![CHKSUM;130;465;934;123]!

  long sumCommands = 0;
  long sumX = 0;
  long sumY = 0;
  long sumTime = 0;
  for (int i = 0; i < commandCount; i++) {
    sumCommands ++;
    sumX += (commandCache[i].X2 - commandCache[i].X1);
    sumY += (commandCache[i].Y);
    sumTime += (commandCache[i].time);
  }
  sumCommands = sumCommands % 1000;
  sumX = sumX % 1000;
  sumY = sumY % 1000;
  sumTime = sumTime % 1000;
  //if(rand()%3==0)sumY = 0;   //генератор случайных ошибок для тестов
  Serial.print(F("![CHKSUM"));
  Serial.print(F(";")); Serial.print(sumCommands);
  Serial.print(F(";")); Serial.print(sumY);
  Serial.print(F(";")); Serial.print(sumX);
  Serial.print(F(";")); Serial.print(sumTime);
  Serial.println(F("]!"));
  retries ++;
}

void execute() {
  // if(rand()%8 == 0) return;     //errors for debugging
  //Чтобы минимизировать вероятность того что команда не дойдёт, отправим её 3 раза з задержками в 100мс
  for (int i = 0; i < 3; i++) {
    answer(F("ENGRAVING"));
    delay(100 + (retries * 20));
  }
  if (isEngravingNow) {
    log(F("Can't srart engraving while engraving is running."));
    return;
  }
  isEngravingNow = true;
  retries = 0;

  pos();
  int executedCnt = 0;
  for (int i = 0; i < commandCount && isCont(); i++) {
    Serial.print(F("Executing command ")); Serial.print(i); Serial.print(F(" of ")); Serial.println(commandCount);
    Serial.print(F("![PROGRESS;")); Serial.print(i); Serial.println(F("]!"));
    BurnMark current = commandCache[i];
    lineY.goTo(current.Y);
    lineX.goTo(current.X1);
    pos();
    if (current.X2 < 0) current.X2 = 0;
    if (current.X1 < 0) current.X1 = 0;
    if (current.X1 >= X_SIZE) current.X1 = X_SIZE - 1;
    if (current.X2 >= X_SIZE) current.X2 = X_SIZE - 1;

    if (current.X2 > current.X1) { //вперед
      while (lineX.pos() < current.X2) {
        lineX.burnF(current.time);
        pos();
      }
    }
    if (current.X2 < current.X1) { //назад
      while (lineX.pos() > current.X2) {
        lineX.burnB(current.time);
        pos();
      }
    }
    executedCnt ++;
  }

  isEngravingNow = false;
  commandCount = 0;
  Serial.print(F("![COMPLETE;")); Serial.print(executedCnt); Serial.println(F("]!"));
}
