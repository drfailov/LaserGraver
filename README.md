# FP LaserEngraver v3
<b>Project is now in process translating from russian to English.</b>
<p align="center"><img src="Photos/2022-03-11 Promo for repository Laser Engraver.png" width="600"/></p>
PC-software and Arduino-firmware can work together to run laser engraver for englave 2D raster images.

## Overview > List of features
- Easy to use software.
- Support JPG, PNG, GIF, BMP image formats.
- Stable USB connection, error-correction algorythm.
- Full kinematic test can check if engraver works ok.
- Saving and restoring projects for engraving.
- Auto-save projects before engraving.
- Alows to continue engraving in case if process interrupted.
- Manual laser position control.
- Realtime laser position indicator.
- Easy material testing.

## Overview > Photos of device
<p align="center">
<img src="Photos/DSC_7819.jpg" height="300"/>
</p>
<p align="center">
<img src="Photos/DSC_7831.jpg" height="100"/>
<img src="Photos/IMG_20210126_233233.jpg" height="100"/>
<img src="Photos/IMG_20210117_022328.jpg" height="100"/>
<img src="Photos/IMG_20210208_222245.jpg" height="100"/>
<img src="Photos/photo_2021-12-11_14-15-36 (2).jpg" height="100"/>
<img src="Photos/photo_2021-12-11_14-15-36 (3).jpg" height="100"/>
</p>

## Overview > Examples of engraving
<p align="center">
<img src="Photos/20211207_002439.jpg" height="300"/>
</p>
<p align="center">
<img src="Photos/IMG_20210228_012807.jpg" height="100"/>
<img src="Photos/IMG_20210215_143131.jpg" height="100"/>
<img src="Photos/IMG_20210214_140938.jpg" height="100"/>
<img src="Photos/20211007_010526.jpg" height="100"/>
<img src="Photos/photo_2021-12-11_14-15-36.jpg" height="100"/>
<img src="Photos/IMG_20210223_230900.jpg" height="100"/>
<img src="Photos/IMG_20210315_000848.jpg" height="100"/>
<img src="Photos/IMG_20210130_121652.jpg" height="100"/>
<img src="Photos/IMG_20210214_210026.jpg" height="100"/>
<img src="Photos/IMG_20210217_160130.jpg" height="100"/>
</p>

## Overview > Video demo of usage on YouTube
[<p align="center"><img alt="FP LaserEngraver V3 Overview" width="350px" src="Photos/Screenshot 2022-03-13 220049.jpg" /> </p>](https://youtu.be/yXsoJGkvpNk) 
<p align="center">https://youtu.be/yXsoJGkvpNk </p>

## Overview > This repository contains
- C# software to control engraver
- Arduino firmware to control engraver
- Schematics how to connect modules to arduino to make firmware work
<p align="center"><img src="Photos/2022-03 topology.png" width="700"/></p>



# PC Software
<p align="center"><img src="Photos/software.png" width="700"/></p>
<p align="center">
<img src="Photos/image_2021-12-11_13-53-48.png" height="70"/>
<img src="Photos/image_2021-12-11_13-54-16.png" height="70"/>
<img src="Photos/image_2021-12-11_13-54-36.png" height="70"/>
<img src="Photos/image_2021-12-11_13-56-15.png" height="70"/>
<img src="Photos/image_2021-12-11_13-56-57.png" height="70"/>
</p>

## PC Software > How to run
If you don't want to build or edit sources of PC software, you can just run it with following steps:
- Check if you have installed .NET Framework 3.5
- Open application from: `PC Software\LaserDrawerApp v.3\LaserDrawerApp\bin\Release\LaserDrawerApp.exe` 

## PC Software > How to build
To build software, you need such environment:
- Use Microsoft Windows
- Visual Studio 2019 installed
- C# Windows Forms installed

## PC Software > Typical usage
Usage demo: https://youtu.be/yXsoJGkvpNk
- Before usage engraver already assembled, drivers installed, firmware flashed and configured.
- Connect USB cable from arduino. 
- Open PC Software, update list of COM ports, select engraver COM port and press connect.
Engraver will execute self-testing command to check if CNC works correctly.
System will step some steps forward from home point and measure how much steps needed to trigger endstop again. 
Based on this measurement engraver will connect successfully, or throw an error.
If you have errors on this step and can`t figure how to fix it, you always can send commands manually by arduino port monitor to debug device.
- Create new project. Project size is measured from top-left corner of working zone. If you not sure about project size, select 25% for first tests.
- Add images or text on project. JPG, PNG, GIF and BMP formats are supported.
- You can move laser head with arrows on left panel. 
Actual head position will be displayed on phoject area.
You can move and resize objects in project.
- Set material paramenets: burn time for 1 pixel and number of repeatings.
Material settings is very individual and optimal way to get best results is a lot of practice.
- If you don't know which burn time is best for you, you can use burn test block on left panel.
It will burn short line (about 5mm) with selected settings and you can check if it's normal settings for your material.
- When objects are positioned, you can render your project. While rendering software will prepare instructions for engraving.
- If everything fine, press Engrave. System will initialize motors (if needed) and start engraving.
Don't look at working laser while engraving, use safety glasses!
If something goes wrong, press stop button. Engraver will stop executing commands and move head to 0. If needed, disconnect motors and laser from power.

## PC Software > Menu overview
### Engraver - Full self-test
Move laser head to maxumim and minimum position or table. 
As a result, system is measuring number of steps needed to trigger endstop from maximum position.

### Engraver - Turn ON Lights
Turn ON device backlight.

### Engraver - Turn OFF Lights
Turn OFF device backlight.

### Engraver - Get Status
Shows system status or engraver. For debugging.

### Engraver - Disable motors
When motors disabled, you can easily move laser head.

### Render - No trace optimizer
The easiest way to engrave. 
If your engraver not perfectly precise, use this mode.

### Render - Zig-Zag way trace optimizer
Enables burning when laser head moves back. 
Your engraver has to be assembled very precisely, 
othervise image will deform if you enable this optimization;

### Render - Shortest way trace optimizer
Aligns all instructions to minimize laser head movement. 
Optimal for cutting (<i>but notice, begerally this software is not optimized for cutting at all</i>).
Also breaks every instruction into 3px length. 
It needed to get better results in case if engraver is used for cutting.
Your engraver has to be assembled very precisely, 
othervise image will deform if you enable this optimization;

### Render - Random way trace
Just for fun, scrambles all the instructions. Very inefficient way to engrave:)

### Render - Use semitones
If semitones enabled, burning time for every pixel is calculated based on pixel darkness and burning time value set by user.
If semitones disabled, burning time for every pixal can be only 0ms or burning time value set by user.
### Help - Show log window
Open one more window with log. Useful for debugging.

### Help - Log debug messages
If enabled, more information will be printed to log.

### Help - About
Show info about developer and credits.

### New project

### Open project
Opens previously rendered and saved set of instructions.
Notice, that every time you render your project, it automatically saved in temporary folder.
So, if engraving process is interrupted, 
you can open autosaved project and continue from last engraved position.
Just don't move objects on table.

### Save project
Saves rendered set of instructions.

### Refresh port list
Get list of available COM port list to ComboBox and tries to select one with connected Arduino.

### Connect
Try to connect to COM port and send command for self-testing.
If self-testing passed, connection successful.

### Move head block
You can:
- Manually move head. Slow or quick.
- Manually enable laser (dangerous!!!).
- Put laser head to zero coordinates.
If motors is disabled before command for move, 
firstly motors will be initialized to home point.

### Material test
It will burn <b>50px</b> (usually about 5mm length) line with selected burn time.
It can help you detect optimal burning time for your material.
The longer the burn time, the more contrast the resulting image will be. For each material, this value can vary greatly.
To find out what burn time is suitable for a particular material, find a small fragment on the material on which you can test by drawing a few lines.\
Pressing the "Burn Line" button will start drawing a line with the specified settings a few centimeters to the right of the current laser position.\
- If the line is invisible, you can try more time.\
- If the line burns through the material, use less time.

### Project tab
Here you can arrange objects in project.
You can add text, images or empty frame.
Empty frame can help positioning image if needed.
Next step is render it to get set of instructions.
Rendered set of instructions can't be changed, moved.

### Engrave tab
When you rendered your project, review reconstruction for your set of instructions. 
Red lines if burning zones, blue lines is laser head movement.
If everything OK, you can start engraving.
Left panel shows progress of engraving.


# Hardware

## Hardware > Used components
### Hardware > Used components > Arduino Nano
Arduino Nano, main controller based on Atmega 328p.\
Link: https://www.banggood.com/Geekcreit-ATmega328P-Nano-V3-Controller-Board-Improved-Version-Module-Development-Board-p-940937.html
<p align="center">
<img src="Photos/unnamed.png" height="120"/>
</p>

### Hardware > Used components > CNC Shield V4 Expansion Board With Nano
board allows connect Arduino and motor drivers: CNC Shield V4 Expansion Board With Nano \
Link: https://www.banggood.com/CNC-Shield-V4-Expansion-Board-With-Nano-and-3Pcs-Red-A4988-For-3D-Printer-p-1343033.html
<p align="center">
<img src="Photos/CNC_board_DP0.jpg" height="200"/>
<img src="Photos/IMG_20181010_213621.jpg" height="200"/>
</p>

### Hardware > Used components > Laser
12V 20W 450nm Laser module\
Link: https://aliexpress.ru/item/1005003148619218.html
<p align="center">
<img src="Photos/20211022_120119.jpg" height="200"/>
<img src="Photos/1Screenshot 2021-12-12 152449.png" height="200"/>
</p>

### Hardware > Used components > Motor drivers
- Motor drivers TMC2208, silent.\
Link: https://aliexpress.ru/item/4000869320068.html
<p align="center">
<img src="Photos/HTB1M3Qvi8DH8KJjSspnq6zNAVXaz.jpg" height="200"/>
</p>

### Hardware > Used components > CNC Set
CNC DIY Set with 2040 aluminium profiles\
Link: https://aliexpress.ru/item/1005002058668194.html

### Hardware > Used components > Endstops
MakerBot Endstops\
Link: https://aliexpress.ru/item/4000602312490.html
<p align="center">
<img src="Photos/Mechanical-Endstop-2-500x500.jpg" height="200"/>
</p>

### Hardware > Used components > Stepper motors
NEMA17 Stepper motors\
Link: https://aliexpress.ru/item/32665922113.html
<p align="center">
<img src="Photos/Screenshot 2021-12-12 153955.png" height="200"/>
</p>


## Hardware > Schematic
<p align="center">
  <img src="Photos/Схема включения.png" width="400"/>
</p>
<p align="center">
<img src="Photos/IMG_20210111_175132.jpg" height="150"/>  
<img src="Photos/IMG_20210111_175144.jpg" height="150"/>  
</p>

- <b>D2: X DIR: </b> Direction signal for X stepper motor driver. 
Can be inverted in `Global.h` in line definition.
- <b>D3: X DIR: </b> Direction signal for Y stepper motor driver. 
Can be inverted in `Global.h` in line definition.
- <b>D4: Z DIR: </b> Direction signal for Z stepper motor driver.
<i>Not used in project. Needed only as CNC shield schematic.</i>
- <b>D5: X STEP: </b> Step signal for X stepper motor driver.
One pulse is 1 step. It's recommended to use smallest possible steps for your driver (64 microstep).
Steps speed can be set in `Config.h`.
- <b>D6: Y STEP: </b> Step signal for Y stepper motor driver.
One pulse is 1 step. It's recommended to use smallest possible steps for your driver (64 microstep).
Steps speed can be set in `Config.h`.
- <b>D7: Z STEP: </b> Step signal for Z stepper motor driver.
<i>Not used in project. Needed only as CNC shield schematic.</i>
- <b>D8: EN: </b> Enabled signal for X, Y, Z stepper motor driver. 
Sets when motors have to be enabled or disabled.
HIGH is enabled, LOW is disabled.
When motor enabled, it hold its position, you can't move it manually.
When motor disabled, it allows you to manually move laser head.
- <b>D9: LED: </b> controls engraver backlight. HIGH is enabled, LOW is disabled.
- <b>D12: LASER: </b> controls laser. Uses PWM. HIGH is enabled, LOW is disabled.
- <b>A7: END X-: </b> reads endstop signal from X axis. 
Used to detect home position of laser head.
Endstop is have to be installed in LEFT side of X axis (this is zero point).
When HIGH, endstop is acticated. 
When LOW, endstop is released.
- <b>A6: END Y-: </b> reads endstop signal from X axis. 
Used to detect home position of laser head.
Endstop is have to be installed in TOP side of Y axis  (this is zero point).
When HIGH, endstop is acticated. 
When LOW, endstop is released.



## Hardware > Troubleshooting
### Hardware > Troubleshooting > Image deformated
Check if all axises of your CNC is aligned and moves easily.

### Hardware > Troubleshooting > No device connected or Unrecognized device on PC when USB connected
Check or replace your Arduino and USB cable.

### Hardware > Troubleshooting > No response from board when trying to flash firmware.
Check if your arduino is alive, replace Arduino and check if you selected correct bootloader for your Arduino.



# Firmware
## Firmware > How to build and flash
- Install Arduino IDE.
I used Arduino 1.8.16.
Link: https://www.arduino.cc/en/software ;
- Because Arduino IDE don't have drivers for CH340, you need to install drivers manually.
You can find drivers enywhere over Internet.
I found here: https://drive.google.com/file/d/1BqUo6f5WEd1vqdxt1UWzDh4OgjdQ7Mx1/view ;
- Additional libraries not needed.
- Open project and try to build it. If all steps done correctly, project will build successfully.
After successful build you can start configuring firmware by changing values in `Config.h`.
When make first configurations, be ready to disconnect power immediately 
in case of unexpeted behaviour.


## Firmware > Configuration
After successful build you can start configuring firmware by changing values in `Config.h`.\
Here are list of available configurations:
- `Line.h` > `long MOTOR_DIVIDER`: Defines how much physical motor <b>steps</b> is counted as 1 <b>pixel</b>.
Using this value you can adjust physical resolution of engraved image.\
For example:\
If divider for my engraver is 1 (1 pixel equals 1 step), resolution of my table is <b>36400px X 33600px</b>.
This is too much, so I adjusted resolution by dividing it by 7:\
If divider for my engraver is 7 (1 pixel equels 7 steps), resolution of my table is <b>4800px X 5200px</b>, giving me resolution of <b>583 DPI</b>.\
Value `7` is optimal if you are using NEMA7 motors with 1/32 microstep.
- `Line.h` > `int precise_homing_steps`: Used only when laser head is going home. 
How much <b>steps</b> go forward after endstop is triggered.\
Value has to be small, but enough to unclick endstop.
- `Config.h` > `int X_SIZE`: Defines width of working table in <b>pixels</b>.
- `Config.h` > `int Y_SIZE`: Defines height of working table in <b>pixels</b>.
- `Config.h` > `long speed`: Max speed of laser head movement.
Unit is steps per second.\
More is quicker, less is slower.\
Value 10000 is acceptable to NEMA17 motor and 1\32 microstep.
- `Config.h` > `float startAccel`: Acceleration of motor.\
The greater number - the quicker the acceleration.
There is no measurement unit and maximum value for this number.\
0 is no acceleration (all movement on minimal speed).\
0.05 is optimal value for my engraver (NEMA17 motor and 1\32 microstep).
- `Config.h` > `float stopAccel`: Speed of breaking. 
Works differently from startAccel, optimal ranges also different.\
There is no measurement unit and maximum value for this number.\
The greater stopAccel - the quicker braking.\
0 is moving laser head on minimum speed all the time.\
0.5 is optimal value for my engraver (NEMA17 motor and 1\32 microstep).
- `Config.h` > `const int MAX_INSTRUCTIONS_COUNT`: Number of instructions stored in RAM memory of Arduino.
This number is used only for `upload` command.
This number have to be the same in PC Software.\
One instruction tooks 10 bytes of memory.\
In my case I used 130 instructions.
- `Config.h` > `const int POS_UPDATE_FREQUENCY_MS`: Defines time (in ms) between sending position data to PC Software.\
Very frequent updating leads to channel overloading and unstable motors movement.\
In my case optimal value is 100ms.
- `Config.h` pin definitions:\
`int X_DIR_PIN` - Where direction pin from X motor driver is connected.\
`int X_STP_PIN` - Where step pin from X motor driver is connected.\
`int X_END_APIN` - Number of analog pin where X endstop is connected. (A0=0, A1=1, A2=2...)\
`int X_EN_PIN` -  Where enable pin from X motor driver is connected.\
`int Y_DIR_PIN` - Where direction pin from Y motor driver is connected.\
`int Y_STP_PIN` - Where step pin from Y motor driver is connected.\
`int Y_END_APIN` - Number of analog pin where Y endstop is connected. (A0=0, A1=1, A2=2...)\
`int Y_EN_PIN` - Where enable pin from Y motor driver is connected.\
`int LED_PIN` - Where connected switch for engraver backlight.\
`int LASER_PIN` - Signal pin for laser.
- `Global.h` - `Line lineX` - `Invert direction` X motor direction inversion.
- `Global.h` - `Line lineY` - `Invert direction` Y motor direction inversion.



## Firmware > Protocol description
Commands separated by `;`. Command arguments separated by `;`.
Engraver answers starts with `!`, separated by `\n`. Engraver arguments separated by `;`.
Baud is 115200.

### Firmware > Protocol description > Pause
Send from PC to Engraver to pause any process: \
`pause;`.\
Send from PC to Engraver to resume paused process:\
`continue;`

### Firmware > Protocol description > Status
Command needed to check if engraver ready.\
Send from PC to Engraver:\
`status;`\
Answer from Engraver to PC:\
`![STATUSOK]!`

### Firmware > Protocol description > Self-test
Run full-range mechanical self-test.
Laser head will move from home point to max point and back.\
Send from PC to Engraver:\
`selftest;`\
Answer from Engraver to PC:\
`![TEST;PASS;PASS]!`\
`![TEST;PASS;FAIL]!`\
"TEST", "PASS"/"FAIL" result for X axis, "PASS"/"FAIL" result for Y axis.

### Firmware > Protocol description > Quick Self-test
Run quick mechanical self-test.
Laser head will move from home point to some point and back.\
Send from PC to Engraver:\
`selftestquick;`\
Answer from Engraver to PC:\
`![TEST;PASS;PASS]!`\
`![TEST;PASS;FAIL]!`\
"TEST", "PASS"/"FAIL" result for X axis, "PASS"/"FAIL" result for Y axis.

### Firmware > Protocol description > Home
Initialize engraver to starting point.
Send from PC to Engraver:\
`home;`\
Answer from Engraver to PC:\
`![OK]!`

### Firmware > Protocol description > Version
Get information about firmware version.
Send from PC to Engraver:\
`version;`\
Answer from Engraver to PC:\
`![FP myCNC v3, Compile date: Mar 18 2022 23:15:31, Free RAM: 503 bytes.]!`

### Firmware > Protocol description > Size
Get size of wirking zone in pixels. Data is collected from `Config.h` file.\
Answer contains width and height. This data is used to select project size.\
Send from PC to Engraver:\
`size;`\
Answer from Engraver to PC:\
`![SIZE;2100;2100]!`

### Firmware > Protocol description > LEDOFF, LEDON
Enable or disable engraver backlight.\
Send from PC to Engraver:\
`ledoff;` or `ledon;`\
Answer from Engraver to PC:\
`![OK]!`


### Firmware > Protocol description > Manually moving laser head
This commands sent form PC to Engraver start laser head movement.\
Send from PC to Engraver:\
`rightslow;` - Start <b>slow</b> moving <b>right</b>.\
`rightfast;` - Start <b>fast</b> moving <b>right</b>.\
`leftslow;` - Start <b>slow</b> moving <b>left</b>.\
`leftfast;` - Start <b>fast</b> moving <b>left</b>.\
`upslow;` - Start <b>slow</b> moving <b>up</b>.\
`upfast;` - Start <b>fast</b> moving <b>up</b>.\
`downslow;` - Start <b>slow</b> moving <b>down</b>.\
`downfast;` - Start <b>fast</b> moving <b>down</b>.\
While head is moving, engraver keep sending its actual position:\
`![POS;868;500]!`\
To stop moving head, send from PC to Engraver stop command:\
`stop;`\
Answer (from Engraver to PC) for stop command is:\
`![OK]!`

### Firmware > Protocol description > Get position
To get current actual position of laser head.
Send from PC to Engraver:\
`pos;`\
Answer from Engraver to PC:\
`![POS;868;500]!`

### Firmware > Protocol description > Disable motors
Disconnect motors from power.
Send from PC to Engraver:\
`release;`\
Answer from Engraver to PC:\
`![OK]!`

### Firmware > Protocol description > Laser ON
Enable laser (DANGEROUS!)\
Send from PC to Engraver:\
`laseron;`\
Answer from Engraver to PC:\
`![OK]!`\
To disable laser, send from PC to Engraver stop command:\
`stop;`\
Answer (from Engraver to PC) for stop command is:\
`![OK]!`


### Firmware > Protocol description > Burn test
Burn short line with selected burn time for pixel.
Send from PC to Engraver:\
`burntest;5;`\
Answer from Engraver to PC:\
`![OK]!`

### Firmware > Protocol description > Go to coordinates
Manually move head to selected coordinates\
Send from PC to Engraver:\
`goto;5;5;`\
Answer from Engraver to PC:\
`![OK]!`\
`![POS;868;500]!`

### Firmware > Protocol description > Upload
First stage of engraving: send commands to engraver and check if delivered normally.
Maximum number of commands for one upload: 130 (can be configured in `Config.h`.\
Send from PC to Engraver:\
`upload;5_20_600_50_600;end;`\
Description: Upload;command1;command2;...;end;\
Command: burnTime_x1_y1_x2_y2 (Y2 is ignored)\
Answer from Engraver to PC:\
`![CHKSUM;130;465;934;123]!`\
Description: ![chksum;number_of_commads;Ys_sum_mod1000;lengths_sum_mod1000;times_sum_mod1000]!\
Examples:\
`upload;11_1400_1100_1600_1100;11_1400_1100_1600_1100;11_1400_1200_1600_1200;end;execute;`\
`![CHKSUM;130;465;934;123]!`

### Firmware > Protocol description > Execute
Second stage of engraving: execute commangs that was sent by `upload` command.\
While engraving is in progress, engraver is senging `progress` and `pos` answers.
When execution finished, engraver sends `complete` answer and wait for next upload.
Send from PC to Engraver:\
`execute;`\
Answer from Engraver to PC:\
`![ENGRAVING]!   ...   ![PROGRESS;30]!  ...  ![POS;868;500]!  ...  ![COMPLETE;1]!`


### Examlpes of scripts
`goto;2100;2100;home;`\
`goto;1000;1000;goto;1200;1200;`

  




