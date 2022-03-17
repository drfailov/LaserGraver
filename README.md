# FP LaserEngraver v3
<p align="center"><img src="Photos/2022-03-11 Promo for repository Laser Engraver.png" width="600"/></p>
PC-software and Arduino-firmware can work together to run laser engraver for englave 2D raster images.

## List of features
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

## Photos of device
<p align="center">
<img src="Photos/DSC_7819.jpg" height="150"/>
<img src="Photos/DSC_7831.jpg" height="150"/>
<img src="Photos/IMG_20210126_233233.jpg" height="150"/>
<img src="Photos/IMG_20210117_022328.jpg" height="150"/>
<img src="Photos/IMG_20210208_222245.jpg" height="150"/>
<img src="Photos/photo_2021-12-11_14-15-36 (2).jpg" height="150"/>
<img src="Photos/photo_2021-12-11_14-15-36 (3).jpg" height="150"/>
</p>

## Examples of engraving
<p align="center">
<img src="Photos/IMG_20210228_012807.jpg" height="150"/>
<img src="Photos/IMG_20210215_143131.jpg" height="150"/>
<img src="Photos/IMG_20210214_140938.jpg" height="150"/>
<img src="Photos/20211007_010526.jpg" height="150"/>
<img src="Photos/photo_2021-12-11_14-15-36.jpg" height="150"/>
<img src="Photos/20211207_002439.jpg" height="150"/>
<img src="Photos/IMG_20210223_230900.jpg" height="150"/>
<img src="Photos/IMG_20210315_000848.jpg" height="150"/>
<img src="Photos/IMG_20210130_121652.jpg" height="150"/>
<img src="Photos/IMG_20210214_210026.jpg" height="150"/>
<img src="Photos/IMG_20210217_160130.jpg" height="150"/>
</p>

## Video demo of usage on YouTube
[<p align="center"><img alt="FP LaserEngraver V3 Overview" width="300px" src="Photos/Screenshot 2022-03-13 220049.jpg" /> </p>](https://youtu.be/yXsoJGkvpNk) 
<p align="center">https://youtu.be/yXsoJGkvpNk </p>

## This repository contains
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

## How to run
If you don't want to build or edit sources of PC software, you can just run it with following steps:
- Check if you have installed .NET Framework 3.5
- Open application from: `PC Software\LaserDrawerApp v.3\LaserDrawerApp\bin\Release\LaserDrawerApp.exe` 

## How to build
To build software, you need such environment:
- Use Microsoft Windows
- Visual Studio 2019 installed
- C# Windows Forms installed

## Typical usage
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

## Menu overview
### Engraver - Full self-test
### Engraver - Turn ON Lights
### Engraver - Turn OFF Lights
### Engraver - Get Status
### Engraver - Disable motors
### Render - No trace optimizer
### Render - Zig-Zag way trace optimizer
### Render - Shortest way trace optimizer
### Render - Random way trace
### Render - Use semitones
### Help - Show log window
### Help - Log debug messages
### Help - About
### New project
### Open project
### Save project
### Refresh port list
### Connect
### Move head
### Material test
### Project tab
### Engrave tab

# Hardware

## Used components
### Arduino Nano
Arduino Nano, main controller based on Atmega 328p.\
Link: https://www.banggood.com/Geekcreit-ATmega328P-Nano-V3-Controller-Board-Improved-Version-Module-Development-Board-p-940937.html
<p align="center">
<img src="Photos/unnamed.png" height="120"/>
</p>

### CNC Shield V4 Expansion Board With Nano
board allows connect Arduino and motor drivers: CNC Shield V4 Expansion Board With Nano \
Link: https://www.banggood.com/CNC-Shield-V4-Expansion-Board-With-Nano-and-3Pcs-Red-A4988-For-3D-Printer-p-1343033.html
<p align="center">
<img src="Photos/CNC_board_DP0.jpg" height="120"/>
<img src="Photos/IMG_20181010_213621.jpg" height="120"/>
</p>

### Laser
12V 20W 450nm Laser module\
Link: https://aliexpress.ru/item/1005003148619218.html
<p align="center">
<img src="Photos/20211022_120119.jpg" height="120"/>
<img src="Photos/1Screenshot 2021-12-12 152449.png" height="120"/>
</p>

### Motor drivers
- Motor drivers TMC2208, silent.\
Link: https://aliexpress.ru/item/4000869320068.html
<p align="center">
<img src="Photos/HTB1M3Qvi8DH8KJjSspnq6zNAVXaz.jpg" height="120"/>
</p>

### CNC Set
CNC DIY Set with 2040 aluminium profiles\
Link: https://aliexpress.ru/item/1005002058668194.html

### Endstops
MakerBot Endstops\
Link: https://aliexpress.ru/item/4000602312490.html
<p align="center">
<img src="Photos/Mechanical-Endstop-2-500x500.jpg" height="120"/>
</p>

### Stepper motors
NEMA17 Stepper motors\
Link: https://aliexpress.ru/item/32665922113.html
<p align="center">
<img src="Photos/Screenshot 2021-12-12 153955.png" height="120"/>
</p>


## Schematic
<p align="center">
  <img src="Photos/Схема включения.png" width="400"/>
</p>
<p align="center">
<img src="Photos/IMG_20210111_175132.jpg" height="150"/>  
<img src="Photos/IMG_20210111_175144.jpg" height="150"/>  
</p>
Motor drivers have to be set for smallest possible microstep (64 microstep).

- <b>D2: X DIR: </b> Direction signal for X stepper motor driver. 
Can be inverted in `Global.h` in line definition.
- <b>D3: X DIR: </b> Direction signal for Y stepper motor driver. 
Can be inverted in `Global.h` in line definition.
- <b>D4: Z DIR: </b> Direction signal for Z stepper motor driver.
<i>Not used in project. Needed only as CNC shield schematic.</i>
- <b>D5: X STEP: </b> Step signal for X stepper motor driver.
One pulse is 1 step. It's recommended to use smallest possible steps for your driver.
Steps speed can be set in `Config.h`.
- <b>D6: Y STEP: </b> Step signal for Y stepper motor driver.
One pulse is 1 step. It's recommended to use smallest possible steps for your driver.
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



## Troubleshooting

# Firmware
## How to build and flash
Язык: C++, Arduino IDE \
Дополнительных библиотек не требуется. \
Основные параметры настройки вынесены в `Config.h`, но многие константы прописаны в коде. \
Последний раз успешно собиралась в среде Arduino 1.8.16.
## Configuration
## Protocol description


--------------

# Project is now in process translating from russian to English.
 

  




