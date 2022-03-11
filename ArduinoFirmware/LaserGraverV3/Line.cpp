#include <Arduino.h>
#include "Line.h"






Line::Line(int _size, int _step_pin, int _dir_pin, int _en_pin, bool _invert, int _endstop_apin) : motor(_step_pin, _dir_pin, _en_pin, _invert), endstop(_endstop_apin){
    size = _size;
    motor.disable();
}

int Line::home(){
    Serial.print(F("Homing line ")); Serial.print(motor.toString()); Serial.println(F("..."));
    long cnt = 0; 
    //go to beginning (not precise)
    for(int i=0; i<size+(precise_homing_steps*2); i++){
      if(endstop.isPressed())
        break;
      for(int s=0; s<MOTOR_DIVIDER; s++)
        motor.stepBackward();
      cnt ++;
    }  
    delay(300); 
    //retry (precise)
    motor.goForward(precise_homing_steps * MOTOR_DIVIDER);//forward
    delay(100); 
    for(int i=0; i<precise_homing_steps*2; i++){ //back to endstop
      if(endstop.isPressed())
        break;
      long stepDelay = 5000 / MOTOR_DIVIDER;
      for(int s=0; s<MOTOR_DIVIDER; s++){
        motor.stepBackward();
        delayMicroseconds(stepDelay);
      }
    } 
    //go away from endstop
    for(int i=0; i<precise_homing_steps; i++){      
      long stepDelay = 3000 / MOTOR_DIVIDER;
      for(int s=0; s<MOTOR_DIVIDER; s++){
        motor.stepForward();
        delayMicroseconds(stepDelay);
      }
      cnt --;
    }
    position = 0; 
    Serial.print(F("Homing line ")); Serial.print(motor.toString()); Serial.println(F(" complete!"));
    delay(500);
    return cnt;
}
int Line::pos(){
  //инициализация, если ещё не была выполнена, и возврат на то же место
  if(position == -1){
    int oldPosition = home();
    goTo(oldPosition);
  }    
  return position;
}
  
bool Line::test(){
  Serial.print(F("Testing line ")); Serial.print(motor.toString()); Serial.println(F("..."));
  home();
  delay(500);
  end();
  int result2 = home();
  Serial.print(F("Testing line ")); Serial.print(motor.toString()); Serial.print(F(" complete!  Result = ")); Serial.println(result2);
  int error = abs(size - result2);
  Serial.print(F("                          Error = ")); Serial.println(error);
  if(error > 10){
    Serial.print(F("                          Line ")); Serial.print(motor.toString()); Serial.println(F(" FAILED!"));
    return false;
  }
  else{
    Serial.print(F("                          Line ")); Serial.print(motor.toString()); Serial.println(F(" PASSED!"));
    return true;
  }
}
bool Line::testquick(){
  Serial.print(F("Testing line ")); Serial.print(motor.toString()); Serial.println(F("..."));
  home();
  delay(200);
  int testDistance = 200;
  goTo(testDistance);
  int result2 = home();
  Serial.print(F("Testing line ")); Serial.print(motor.toString()); Serial.print(F(" complete!  Result = ")); Serial.println(result2);
  int error = abs(testDistance - result2);
  Serial.print(F("                          Error = ")); Serial.println(error);
  if(error > 10){
    Serial.print(F("                          Line ")); Serial.print(motor.toString()); Serial.println(F(" FAILED!"));
    return false;
  }
  else{
    Serial.print(F("                          Line ")); Serial.print(motor.toString()); Serial.println(F(" PASSED!"));
    return true;
  }
}

  
void Line::stepF(){
    //инициализация, если ещё не была выполнена, и возврат на то же место
    if(position == -1){
      int oldPosition = home();
      goTo(oldPosition);
    }
    if(position < size-1){
      for(int i=0; i<MOTOR_DIVIDER; i++)
        motor.stepForward();
      position ++;
    }
}
void Line::stepB(){
    //инициализация, если ещё не была выполнена, и возврат на то же место
    if(position == -1){
      int oldPosition = home();
      goTo(oldPosition);
    }
    if(position > 0){
      for(int i=0; i<MOTOR_DIVIDER; i++)
        motor.stepBackward();
      position--;
    }
}

//прожечь один пиксель (это MOTOR_DIVIDER шагов мотора)  вперёд
void Line::burnF(long burnTimeMs){
    //инициализация, если ещё не была выполнена, и возврат на то же место
    if(position == -1){
      int oldPosition = home();
      goTo(oldPosition);
    }
    //проверка, есть ли куда гравировать один пиксель
    if(position >= size-1)
      return;

    
    //нормальное время прохода 10 мс на пиксель
    //Это получается 10 000 мкс на пиксель
    //За этот пиксель делается 7 шагов
    //Получается проход 1 428 мкс на 1 шаг 
    //если надо гравировать за этот шаг 1 мс лазером
    //Это 1 000 мкс на пиксель
    //За этот пиксель делается 7 шагов
    //Получается гравировка 142 мкс на 1 шаг
    //Соответственно, 142мс гравируем, оставшееся вреся чилим.

    long moveTimeMks = burnTimeMs * 100; //чем меньше число, тем "слабее" будет гравировать. Полезно уменьшить если лазер слишком мощный
    if(moveTimeMks < 5000) moveTimeMks = 5000; //Чем меньше число, тем быстрее будет производиться гравировка на малых мощностях
    long burnTimeMks = burnTimeMs * 100; //чем меньше число, тем "слабее" будет гравировать. Полезно уменьшить если лазер слишком мощный
    long moveTimeMksPerStep = moveTimeMks / MOTOR_DIVIDER;
    long burnTimeMksPerStep = burnTimeMks / MOTOR_DIVIDER;
    long restTimeMksPerStep = moveTimeMksPerStep - burnTimeMksPerStep;
    if(restTimeMksPerStep < 0) restTimeMksPerStep = 0;

    motor.stepForward();//первый шаг может содержать задержку на смену направления вращения. Поэтому он делается до включения лазера
    
    for(int i=0; i<MOTOR_DIVIDER-1; i++){
      long start = micros();
      motor.stepForwardQuick();
      //delayMks(restTimeMksPerStep);
      while(micros() - start < restTimeMksPerStep);
      digitalWrite(LASER_PIN, HIGH);
      delayMks(burnTimeMksPerStep);
      digitalWrite(LASER_PIN, LOW);
    }
    position ++;
}
void Line::delayMks(long mks){
  if(mks > 16000)
    delay(mks / 1000);
  else
    delayMicroseconds(mks);//max delay value is 16383
}
// Эта функция практически не используется
//прожечь один пиксель (это MOTOR_DIVIDER шагов мотора)  назад
void Line::burnB(long burnTimeMs){
    //инициализация, если ещё не была выполнена, и возврат на то же место
    if(position == -1){
      int oldPosition = home();
      goTo(oldPosition);
    }
    //проверка, есть ли куда гравировать один пиксель
    if(position > 0){
      motor.stepBackward();//первый шаг может содержать задержку на смену направления вращения. Поэтому он делается до включения лазера
      long startedTime = millis();
      digitalWrite(LASER_PIN, HIGH);
      long stepDelay = 1000 * burnTimeMs / MOTOR_DIVIDER;
      for(int i=0; i<MOTOR_DIVIDER-1; i++){
        motor.stepBackward();
        if(stepDelay > 16000)
          delay(stepDelay / 1000);
        else
          delayMicroseconds(stepDelay);//max delay value is 16383
        //если прожгли достаточно, выключить лазер
        if(millis() - startedTime > burnTimeMs) 
          digitalWrite(LASER_PIN, LOW);
      }
      digitalWrite(LASER_PIN, LOW);
      position --;
    }
}
void Line::stepFSlow(long additionalMicrosecondsDelay){
    //инициализация, если ещё не была выполнена, и возврат на то же место
    if(position == -1){
      int oldPosition = home();
      goTo(oldPosition);
    }
    if(position < size-1){
      long stepDelay = additionalMicrosecondsDelay / MOTOR_DIVIDER;
      for(int i=0; i<MOTOR_DIVIDER; i++){
        motor.stepForward();
        if(stepDelay > 16000)
          delay(stepDelay / 1000);
        else
          delayMicroseconds(stepDelay);//max delay value is 16383
      }
      position ++;
    }
}
void Line::stepBSlow(long additionalMicrosecondsDelay){
    //инициализация, если ещё не была выполнена, и возврат на то же место
    if(position == -1){
      int oldPosition = home();
      goTo(oldPosition);
    }
    if(position > 0){
      long stepDelay = additionalMicrosecondsDelay / MOTOR_DIVIDER;
      for(int i=0; i<MOTOR_DIVIDER; i++){
        motor.stepBackward();
        if(stepDelay > 16000)
          delay(stepDelay / 1000);
        else
          delayMicroseconds(stepDelay);//max delay value is 16383
      }
      position--;
    }
}
  
void Line::goTo(long aim){
  //ограничение чтобы не выйти за поле
  if(aim < 0) aim = 0;
  if(aim >= size) aim = size - 1;
  //инициализация, если ещё не была выполнена
  if(position == -1) home();
  //move
  if(position < aim) {
    long defference = (aim - position) * MOTOR_DIVIDER;
    motor.goForward(defference);
  }
  else {
    long defference = (position - aim) * MOTOR_DIVIDER;
    motor.goBackward(defference);
  }
  {//move back and forward if direction changed
    long defference = precise_homing_steps * 2;
    if(aim < position && position - aim > defference/MOTOR_DIVIDER){
      motor.goBackward(defference);
      motor.goForward(defference);
    }
  }
  //update position
  position = aim;
}
  
//void Line::goToOld(long aim){
//  //long treshold = size / 150;
//  long treshold = 0;
//  long stage1aim = aim - treshold;
//  //long stage2aim = aim;
//  //ограничение чтобы не выйти за поле
//  if(stage1aim < 0) stage1aim = 0;
//  if(stage1aim >= size) stage1aim = size - 1;
////  if(stage2aim < 0) stage2aim = 0;
////  if(stage2aim >= size) stage2aim = size - 1;
////  //если это маленькое перемещение вперед, просто перемещаемся вперед, без первого этапа
////  if(stage1aim < position && stage2aim >= position) stage1aim = position;
////  //если это маленькое перемещение назад, просто перемещаемся назад, без второго этапа
////  if(stage2aim - position < 0 && abs(stage2aim - position) < treshold*2) stage1aim = stage2aim;
//  //инициализация, если ещё не была выполнена
//  if(position == -1) home();
//  //stage 1 - go fast
//  if(position < stage1aim) {
//    long defference = (stage1aim - position) * MOTOR_DIVIDER;
//    motor.goForward(defference);
//  }
//  else {
//    long defference = (position - stage1aim) * MOTOR_DIVIDER;
//    motor.goBackward(defference);
//  }
//  position = stage1aim;
////  //stage 2 - go slow
////  while(position < stage2aim)
////     stepFSlow(7000);
//}
  
void Line::release(){
  motor.disable();
  position = -1;
  Serial.print(F("Line ")); Serial.print(motor.toString()); Serial.println(F(" released."));
}

void Line::center(){
    goTo(size/2);
  }
  
void Line::end(){
    goTo(size);
}
  
void Line::begin(){
  goTo(0);
}

   
