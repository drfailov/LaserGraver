#include <Arduino.h>

#ifndef COMMANDSH
#define COMMANDSH

void commandReceived(String cmd);
void home();
void selftest();
void selftestquick();
void size();
void status();
void version();
bool pos(long periodMs);
bool pos();
void posForce();
void rightslow();
void rightfast();
void leftslow();
void leftfast();
void upslow();
void upfast();
void downslow();
void downfast();
void ledoff();
void ledon();
void laseron();
void release();
void upload();
void execute();
void goTo(int x, int y);
void burntest(long time);

#endif
