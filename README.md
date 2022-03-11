# FP_LaserEngraver_v3
 Набор программ для реализации лазерного гравировщика на основе Arduino Nano.

 
## Использованные компоненты:
- Arduino Nano (https://www.banggood.com/Geekcreit-ATmega328P-Nano-V3-Controller-Board-Improved-Version-Module-Development-Board-p-940937.html)
- Плата соединяющая Arduino и драйверы двигателей CNC Shield V4 Expansion Board With Nano (https://www.banggood.com/CNC-Shield-V4-Expansion-Board-With-Nano-and-3Pcs-Red-A4988-For-3D-Printer-p-1343033.html)
- 12V 20W 450nm Лазер (https://aliexpress.ru/item/1005003148619218.html)
- Драйверы двигателей TMC2208, тихие (https://aliexpress.ru/item/4000869320068.html)
- Набор для ЧПУ на профилях 2040 (https://aliexpress.ru/item/1005002058668194.html)
- Концевые выключатели MakerBot (https://aliexpress.ru/item/4000602312490.html)
- Шаговые моторы NEMA17 (https://aliexpress.ru/item/32665922113.html)

<img src="Photos/unnamed.png" width="120"/> <img src="Photos/CNC_board_DP0.jpg" width="120"/> <img src="Photos/Mechanical-Endstop-2-500x500.jpg" width="120"/> <img src="Photos/HTB1M3Qvi8DH8KJjSspnq6zNAVXaz.jpg" width="120"/> <img src="Photos/1Screenshot 2021-12-12 152449.png" width="120"/> <img src="Photos/Screenshot 2021-12-12 153955.png" width="120"/>

<img src="Photos/20211022_120119.jpg" width="300"/> <img src="Photos/IMG_20181010_213621.jpg" width="300"/> 
<img src="Photos/photo_2021-12-11_14-15-36 (2).jpg" width="300"/> <img src="Photos/photo_2021-12-11_14-15-36 (3).jpg" width="300"/>



## Схема подключения модулей устройства
  <img src="Photos/Схема включения.jpg" width="500"/>
  Обратите внимание: эта схема не включает в себя подключения питания
<img src="Photos/IMG_20210111_175132.jpg" width="300"/>  <img src="Photos/IMG_20210111_175144.jpg" width="300"/>  
 
 
 
## Прошивка Arduino:
Язык: C++, Arduino IDE \
Дополнительных библиотек не требуется. \
Основные параметры настройки вынесены в `Config.h`, но многие константы прописаны в коде. \
Последний раз успешно собиралась в среде Arduino 1.8.16.




## Программа для ПК: FP MyCNC v3
Язык: C#, Windows Forms, Microsoft Visual Studio. \
Для работы требуется .NET Framework. 

<img src="Photos/image_2021-12-11_13-53-48.png" width="300"/>
<img src="Photos/image_2021-12-11_13-54-16.png" width="300"/>
<img src="Photos/image_2021-12-11_13-54-36.png" width="300"/>
<img src="Photos/image_2021-12-11_13-56-15.png" width="300"/>
<img src="Photos/image_2021-12-11_13-56-57.png" width="300"/>

 
 
## Как пользоваться программой:
Демонстрация работы: https://youtu.be/yXsoJGkvpNk
1. Подключаем гравер по USB (драйвера для Arduino должны быть установлены)
2. Обновляем список портов и выбираем порт ардуинки
3. Подключаемся. 
	На этом этапе на гравер отправляется команда о самотестировании. 
	Если самотестирование не будет пройдено, подключение будет разорвано.
	Самотестирование проверяет правильность отклика концевиков на команды моторам.
	Если тестирование не проходит, следует проверить правильность подключения концевиков и моторов.
4. Когда подключение установлено, создаём новый проект. 
	Размер холста выбираем в соответствии с размером требуемой гравировки. 
	Чем больше холст, тем сложнее с ним работать.
5. Добавляем на холст нужное изображение или текст.
6. С помощью стрелок перемещаем каретку с лазером.
	Прицеливаемся в нужное место, позиционируем рисунок на холсте.
7. Задаём параметры гравировки (количество слоёв и время прожига одного пикселя).
	Параметры для каждого конкретного гравера, материала и лазера индивидуальны,
	и их подбор требует больного количества экспериментов.
	Для быстрого подбора параметров под материал можно воспользоваться тестом на панели слева.
8. Выполняем рендеринг. Проект при этом преобразуется в готовый набор инструкций для гравера. 
	Проверяем их корректность.
9. Если всё верно, запускаем гравировку и ожидаем.


## Примеры гравировки и порезки
<img src="Photos/photo_2021-12-11_14-15-36.jpg" width="300"/> <img src="Photos/20211007_010526.jpg" width="300"/> 
<img src="Photos/IMG_20210214_140938.jpg" width="300"/> <img src="Photos/IMG_20210215_143131.jpg" width="300"/> 
<img src="Photos/IMG_20210228_012807.jpg" width="300"/>
