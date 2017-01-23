#include <Arduino.h>

#ifndef MY_COMMANDS
#define MY_COMMANDS

void monitor(String in); //show analogs data
void home(String in);    //calibrate both lines
void gomax(String in);   //go both lines max
void gomin(String in);   //go both lines min
void error(String in);   //test error visualisation
void release(String in); //release both motors
void center(String in);  //go both lines to center
void memory(String in);  //show amount of available memory
void gx(String in);      //go X line to index (gx5055)
void gy(String in);      //go Y line to index (gy5055)
void gxy(String in);     //go X and Y line to index (gxy 5055 228)
void xmin(String in);    //go X line to min
void xmax(String in);    //go Y line to max
void ymin(String in);    //go X line to min
void ymax(String in);    //go Y line to max
void brn(String in);     //burn by laser for ... ms. (brn500)
void xtest(String in);   //test X line for the infinite time until stop
void ytest(String in);   //test Y line for the infinite time until stop
void xytest(String in);  //test X and Y line for the infinite time until stop
void laseron(String in); //turn on laser
void laseroff(String in); //turn off laser
void lasertest(String in); //test laser for the infinite time until stop
void getxresolution(String in); //get X resolution 
void getyresolution(String in); //get Y resolution 
void xshift(String in);  //shift X line for ... steps
void yshift(String in);  //shift Y line for ... steps
void xhold(String in);   //switch X line to 16v
void yhold(String in);   //switch Y line to 16v
void bx(String in);       //command burns line on X from(2) to (3) with (1) time per step
void by(String in);       //command burns line on Y from(2) to (3) with (1) time per step
void getxy(String in);   //returns current position of the X and Y lines

#endif
