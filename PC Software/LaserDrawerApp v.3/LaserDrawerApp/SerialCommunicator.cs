using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace LaserDrawerApp
{
    class SerialCommunicator
    {
        public bool debug = false;
        private SerialPort port = null;
        private LogDelegate statusDelegate = null;
        private LogDelegate messageBoxDelegate = null;
        private ActionDelegate onConnected = null;
        private ActionDelegate onDisconnected = null;
        private PositionDelegate onPositionUpdated = null;
        private ProgressDelegate onProgressUpdated = null;
        public delegate string LogDelegate(string text);
        public delegate void ActionDelegate();
        public delegate void PositionDelegate(int x, int y);
        public delegate void ProgressDelegate(int count);
        public string deviceName = null;
        public long lastAnswerTime = 0;
        public int lastAnswerIndex = 0;
        public string[] lastAnswerText = { "", "", "", "", "", "", "", "", "", "" };
        public Point lastPosition = new Point(-1, -1);
        public long lastPositionTime = 0;
        //buffering begin
        public char[] inputBuffer = new char[100];
        public int lastIndex = 0;
        public int prelastIndex(int howMuchBack)
        {
            int result = lastIndex - howMuchBack;
            while (result < 0)
                result += inputBuffer.Length;
            return result;
        }
        public void bufferChar(char c)
        {
            lastIndex++;
            if (lastIndex >= inputBuffer.Length)
                lastIndex = 0;
            inputBuffer[lastIndex] = c;
        }
        public char lastChar(int howMuchBack)
        {
            return inputBuffer[prelastIndex(howMuchBack)];
        }
        //buffering end

        public void connect(string name)
        {
            if (port == null)
            {
                deviceName = name;
                connect();
                //new Thread(connect).Start();
            }
        }
        private void connect()
        {
            log("Connecting to port: " + deviceName + "...");
            try
            {
                port = new SerialPort(deviceName, 115200, Parity.None, 8, StopBits.One);
                port.DataReceived += DataReceivedHandler;
                port.DtrEnable = true;
                //port.RtsEnable = true;
                port.Open();
                log("Connected: " + deviceName);

                log("Checking connection: " + deviceName);
                string welcomeMessage = receiveAnswer("Ready", 10000);
                if (status())
                {
                    log("Engraver connected, self-testing...");
                    if (selftestquick())
                    {
                        log("Connected, engraver ready.");
                        notifyConnected();
                        startWatchdog();
                    }
                    else
                    {
                        log("Test not passed, engraver malfunction.");
                        releare();
                        messageBox("Test failed, engraver has mechanical malfunction.");
                        port.Close();
                        port = null;
                        notifyDisconnected();
                        log("Порт закрыт.");
                    }
                }
                else
                {
                    log("Engraver not responding.");
                    port.Close();
                    port = null;
                    notifyDisconnected();
                    log("Port closed.");
                }
            }
            catch (Exception e)
            {
                log("Connection error: " + e.Message);
                port = null;
                notifyDisconnected();
                throw e;
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            while (sp.IsOpen && sp.BytesToRead > 0)
            {
                char inbyte = (char)sp.ReadByte();
                //log("Byte received: " + inbyte);
                bufferChar(inbyte);
                //когда нашли конец строки
                if (lastChar(0) == '\n')
                {
                    for (int i = 1; i < inputBuffer.Length; i++)//поиск начала строки
                    {
                        if (lastChar(i + 1) == '\n' || lastChar(i + 1) == '\0')//тут мы нашли начало строки
                        {
                            string line = "";   //получаем команду двигаясь от ячеек раньше к ячейкам новее
                            for (int j = i; j > 0; j--)
                                line += lastChar(j);
                            if (debug)
                                log(". " + line);
                            break;
                        }
                    }
                }
                if (lastChar(0) == '!' && lastChar(1) == ']')//тут мы нашли конец команды
                {
                    int indexEnd = 2;
                    //log("Command end at: " + indexEnd);
                    int indexStart = indexEnd;
                    for (int i = 0; i < inputBuffer.Length; i++)//поиск начала команды
                    {
                        if (lastChar(i + 2) == '!' && lastChar(i + 1) == '[')//тут мы нашли начало команды
                        {
                            indexStart = i;
                            //log("Command start at: " + indexStart);
                            break;
                        }
                    }
                    string command = "";   //получаем команду двигаясь от ячеек раньше к ячейкам новее
                    for (int i = indexStart; i >= indexEnd; i--)
                        command += lastChar(i);
                    commandReceived(command);//отправляем данные в адекватную среду!
                }
            }
        }
        private void commandReceived(string command)
        {
            if (debug)
                log("Answer received: " + command);
            lastAnswerIndex++;
            if (lastAnswerIndex >= lastAnswerText.Length) lastAnswerIndex = 0;
            lastAnswerText[lastAnswerIndex] = command;
            lastAnswerTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            if (command.Contains("POS"))
            {
                try
                {
                    string[] parts = command.Split(';');
                    int x = int.Parse(parts[1]);
                    int y = int.Parse(parts[2]);
                    notifyPositionUpdated(x, y);
                }
                catch (Exception e)
                {
                    log("Error updating position: " + e.Message);
                }
            }
            if (command.Contains("PROGRESS"))
            {
                try
                {
                    string[] parts = command.Split(';');
                    int count = int.Parse(parts[1]);
                    notifyProgressUpdated(count);
                }
                catch (Exception e)
                {
                    log("Error updating progress: " + e.Message);
                }
            }
        }

        private void startWatchdog()
        {
            new Thread(watchdog).Start();
        }
        private void watchdog()
        {
            //функция каждую минуту отправляет на гравер какое нибудь сообщение, чтобы убедиться, что с ним всё хорошо
            while (port != null)
            {
                Thread.Sleep(10000);
                if (!isConnected())
                    disconnect();
            }
        }
        public bool isConnected()
        {
            if (port == null || !port.IsOpen)
                return false;
            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long ago = now - lastAnswerTime;
            if (debug)
                log("Last answer was " + ago + " ms ago.");
            if (ago < 60000) //меньше минуты\
                return true;
            return status();
        }
        //вызывать перед отправкой на гравер новой порции данных, чтобы быть уверенным в том, что все предыдущие пакеты были приняты
        public void waitUntilBufferEmpty()
        {
            long timeout = 30000;
            long threshold = 500;
            if (port == null || !port.IsOpen)
                return;
            long startedWaiting = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startedWaiting < timeout    //ожидаем пока не дошли до таймаута
                && (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - lastAnswerTime < threshold) ;  //и пока не прошло секунды с последнего сообщения от гравера
        }
        public void disconnect()
        {
            log("Отключение: " + deviceName);
            if (port != null)
            {
                try { releare(); } catch (Exception) { };
                try { port.DiscardInBuffer(); } catch (Exception) { };
                try { port.Dispose(); } catch (Exception) { };
                try { port.DiscardInBuffer(); } catch (Exception) { };
                notifyDisconnected();
            }
            log("Отключено.");
        }

        public int upload(List<BurnMark> list)
        {
            //возвращает количество ошибок
            int errors = 0;
            while (errors < 10)
            {
                if (port != null && port.IsOpen)
                {
                    log("Engraving: checking connection...");
                    status();
                    Thread.Sleep(10 + (errors * 20));
                    if (errors == 0)
                        log("Engraving: отправка команд...");
                    else
                        log("Engraving: повторная отправка команд (" + errors + ")...");
                    send("upload;");
                    Thread.Sleep(10 + (errors * 20));
                    for (int i = 0; i < list.Count; i++)
                    {
                        BurnMark b = list[i];
                        Thread.Sleep(5 + (errors * 20));
                        send(b.burnTimeMs + "_" + b.Xfrom + "_" + b.Yfrom + "_" + b.Xto + "_" + b.Yto + ";");
                    }
                    send("end;");
                    //проверка
                    int totalCommands = list.Count;
                    int errorsFound = 0;

                    log("Engraving: проверка правильности команд...");
                    //В ответ отправляет контрольные суммы:
                    //- общее количество команд принятых
                    //- Сумма всех координат У по модулю 1000
                    //- сумма всех длин отрезков по модулю 1000
                    //- сумма всего времени обжига по модулю 1000
                    //![CHKSUM;130;465;934;123]!
                    long totalControlSum = 0;
                    long yControlSum = 0;
                    long xControlSum = 0;
                    long timeControlSum = 0;
                    //вичислим суммы какими они должны быть
                    for (int i = 0; i < totalCommands; i++)
                    {
                        BurnMark burnMark = list[i];
                        totalControlSum++;
                        yControlSum += burnMark.Yfrom;
                        xControlSum += burnMark.Xto - burnMark.Xfrom;
                        timeControlSum += burnMark.burnTimeMs;
                    }
                    totalControlSum = totalControlSum % 1000;
                    yControlSum = yControlSum % 1000;
                    xControlSum = xControlSum % 1000;
                    timeControlSum = timeControlSum % 1000;
                    string answer = receiveAnswer("CHKSUM;", 5000);
                    if (answer.Length == 0)
                    {
                        log("Ошибка при проверке данных: проверочный ответ не был получен.");
                        Thread.Sleep(1000);
                        errorsFound++;
                    }
                    else
                    {
                        try
                        {
                            string[] parts = answer.Split(';');
                            long total = long.Parse(parts[1]);
                            long ySum = long.Parse(parts[2]);
                            long xSum = int.Parse(parts[3]);
                            int timeSum = int.Parse(parts[4]);
                            if (total != totalControlSum)
                            {
                                log("Есть ошибки в количестве команд. Ответ от arduino: " + total + ", правильный ответ: " + totalControlSum + ". Текст полученный от Arduino: " + answer);
                                errorsFound++;
                            }
                            if (ySum != yControlSum)
                            {
                                log("Есть ошибки в составе координат У. Ответ от arduino: " + ySum + ", правильный ответ: " + yControlSum + ". Текст полученный от Arduino: " + answer);
                                errorsFound++;
                            }
                            if (xSum != xControlSum)
                            {
                                log("Есть ошибки в составе координат Х. Ответ от arduino: " + xSum + ", правильный ответ: " + xControlSum + ". Текст полученный от Arduino: " + answer);
                                errorsFound++;
                            }
                            if (timeSum != timeControlSum)
                            {
                                log("Есть ошибки в составе времени обжига. Ответ от arduino: " + timeSum + ", правильный ответ: " + timeControlSum + ". Текст полученный от Arduino: " + answer);
                                errorsFound++;
                            }
                        }
                        catch (Exception e)
                        {
                            log("Ошибка разбора ответа при проверке данных: " + e.Message + ", текст ответа " + answer);
                            errorsFound++;
                        }
                    }
                    if (errorsFound > 0)
                    {
                        errors++;
                        waitUntilBufferEmpty();
                    }
                    else
                        return errors;
                }
            }
            return errors;
        }

        public int execute()
        {
            //возвращает количество ошибок
            int errors = 0;
            while (errors < 10)
            {
                if (port != null && port.IsOpen)
                {
                    log("Engraving: Check connection...");
                    status();
                    Thread.Sleep(10 + (errors * 20));
                    if (errors == 0) log("Engraving: sending EXECUTE...");
                    else log("Engraving: re-sending EXECUTE (" + errors + ")...");
                    send("execute;");

                    string engraving = receiveAnswer("ENGRAVING", 5000);
                    if (!engraving.Contains("ENGRAVING"))
                    {
                        errors++;
                        log("Error: EXECUTE command is not delivered.");
                        waitUntilBufferEmpty();
                        continue;
                    }

                    log("Engraving...");

                    string result = receiveAnswer("COMPLETE", 3600000);//1 час
                    Thread.Sleep(20 + (errors * 20));
                    int executedCnt = 0;
                    if (!result.Contains(';'))
                    {
                        errors++;
                        log("Error: ответ на EXECUTE некорректный.");
                        waitUntilBufferEmpty();
                        continue;
                    }
                    try
                    {
                        executedCnt = int.Parse(result.Split(';')[1]);
                    }
                    catch (Exception e)
                    {
                        log("Ошибка рзбора ответа гравера на команду execute: " + e.Message + ", текст ответа " + result);
                        errors++;
                        waitUntilBufferEmpty();
                        continue;
                    }
                    return errors;
                }
            }
            return errors;
        }




        public bool status()
        {
            if (port != null && port.IsOpen)
            {
                send("status;");
                string result = receiveAnswer("STATUS", 5000);
                bool resultBool = result.Equals("STATUSOK");
                if (resultBool)
                    log(System.DateTime.Now.ToString("HH:mm:ss") + " Engraver online.");
                return resultBool;
            }
            return false;
        }
        public void releare()
        {
            if (port != null && port.IsOpen)
            {
                send("release;");
            }
        }
        public void home()
        {
            if (port != null && port.IsOpen)
            {
                send("home;");
            }
        }
        public void go0()
        {
            if (port != null && port.IsOpen)
            {
                send("goto;0;0;");
                receiveAnswer("OK", 5000);
            }
        }
        public bool selftestquick()
        {
            send("selftestquick;");
            string answer = receiveAnswer("TEST", 60000);
            try
            {
                log("Parsing self-test result: " + answer);
                string[] parts = answer.Split(';');
                log("parts: " + parts.Length);
                bool x = parts[1].Equals("PASS");
                log("x: " + x);
                bool y = parts[2].Equals("PASS");
                log("y: " + y);
                return x && y;
            }
            catch (Exception e)
            {
                log("Self-testing error: " + e.Message);
            }
            return false;
        }
        public bool selftest()
        {
            send("selftest;");
            string answer = receiveAnswer("TEST", 120000);
            try
            {
                log("Parsing self-test result: " + answer);
                string[] parts = answer.Split(';');
                log("parts: " + parts.Length);
                bool x = parts[1].Equals("PASS");
                log("x: " + x);
                bool y = parts[2].Equals("PASS");
                log("y: " + y);
                return x && y;
            }
            catch (Exception e)
            {
                log("Error parsing self-test result: " + e.Message);
            }
            return false;
        }
        public Size size()
        {
            send("size;");
            string answer = receiveAnswer("SIZE", 5000);
            try
            {
                log("Paring size: " + answer);
                string[] parts = answer.Split(';');
                log("parts: " + parts.Length);
                int x = int.Parse(parts[1]);
                log("x: " + x);
                int y = int.Parse(parts[2]);
                log("y: " + y);
                return new Size(x, y);
            }
            catch (Exception e)
            {
                log("Error updating position: " + e.Message);
            }
            return new Size(0, 0);
        }
        public string version()
        {
            send("version;");
            string answer = receiveAnswer("Compile date", 5000);
            if (answer.Length == 0)
                answer = "Engraver not responded.";
            return answer;
        }
        public void pause()
        {
            if (port != null && port.IsOpen)
            {
                send("pause;");
            }
        }
        public void Continue()
        {
            if (port != null && port.IsOpen)
            {
                send("continue;");
            }
        }
        public void ledoff()
        {
            if (port != null && port.IsOpen)
            {
                send("ledoff;");
            }
        }
        public void ledon()
        {
            if (port != null && port.IsOpen)
            {
                send("ledon;");
            }
        }
        public void rightslow()
        {
            if (port != null && port.IsOpen)
            {
                send("rightslow;");
            }
        }
        public void rightfast()
        {
            if (port != null && port.IsOpen)
            {
                send("rightfast;");
            }
        }
        public void leftslow()
        {
            if (port != null && port.IsOpen)
            {
                send("leftslow;");
            }
        }
        public void leftfast()
        {
            if (port != null && port.IsOpen)
            {
                send("leftfast;");
            }
        }
        public void upslow()
        {
            if (port != null && port.IsOpen)
            {
                send("upslow;");
            }
        }
        public void upfast()
        {
            if (port != null && port.IsOpen)
            {
                send("upfast;");
            }
        }
        public void downslow()
        {
            if (port != null && port.IsOpen)
            {
                send("downslow;");
            }
        }
        public void downfast()
        {
            if (port != null && port.IsOpen)
            {
                send("downfast;");
            }
        }
        public void pos()
        {
            if (port != null && port.IsOpen)
            {
                send("pos;");
            }
        }
        public void laseron()
        {
            if (port != null && port.IsOpen)
            {
                send("laseron;");
            }
        }
        public void stop()
        {
            if (port != null && port.IsOpen)
            {
                send("stop;");
            }
        }
        public void burntest(int time)
        {
            if (port != null && port.IsOpen)
            {
                send("burntest;" + time + ";");
            }
        }
        public void goTo(int x, int y)
        {
            if (port != null && port.IsOpen)
            {
                send("goto;" + x + ";" + y + ";");
            }
        }






        public bool send(string text)
        {
            if (port != null && port.IsOpen)
            {
                if (debug)
                    log("|#| <-- " + text);
                port.Write(text);
                return true;
            }
            else
            {
                if (port != null)
                    notifyDisconnected();
                if (debug)
                    log("|X| <-- " + text);
                return false;
            }
        }
        private void clearQueue()
        {
            for (int i = 0; i < lastAnswerText.Length; i++)
                lastAnswerText[i] = "";
        }

        public string receiveAnswer(string textToFind, long timeout)
        { //получить херню типа ![текст]!
            if (port == null)
                return "";
            clearQueue();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < timeout)
            {
                for (int i = 0; i < lastAnswerText.Length; i++)
                {
                    if (lastAnswerText[i].Contains(textToFind))
                    {
                        if (debug)
                            log("Answer for " + textToFind + " received in " + stopwatch.ElapsedMilliseconds + " ms.");
                        return lastAnswerText[i];
                    }
                }
                if (Application.OpenForms.Count == 0)
                    return "";
                if (!Application.OpenForms[0].InvokeRequired) //если мы в главном потоке
                    Application.DoEvents();
                else
                    Thread.Sleep(1);
            }
            log("Answer timeout for " + textToFind + "!");
            return "";

        }

        private void notifyProgressUpdated(int count)
        {
            if (onProgressUpdated != null)
                onProgressUpdated.Invoke(count);
        }
        private void notifyPositionUpdated(int x, int y)
        {
            if (onPositionUpdated != null)
                onPositionUpdated.Invoke(x, y);
        }
        private void notifyConnected()
        {
            if (onConnected != null)
                onConnected.Invoke();
        }
        private void notifyDisconnected()
        {
            if (onDisconnected != null)
                onDisconnected.Invoke();
            if (port != null)
            {
                try
                {
                    port.Close();
                }
                catch (Exception e)
                {
                    log("Closing port: " + e.Message);
                }
            }
            port = null;
        }
        public void setStatusDelegate(LogDelegate _statusDelegate)
        {
            statusDelegate = _statusDelegate;
        }
        public void setMessageBoxDelegate(LogDelegate _messageBoxDelegate)
        {
            messageBoxDelegate = _messageBoxDelegate;
        }
        public void setOnConnected(ActionDelegate _onConnected)
        {
            onConnected = _onConnected;
        }
        public void setOnDisconnected(ActionDelegate _onDisconnected)
        {
            onDisconnected = _onDisconnected;
        }
        public void setOnPositionUpdated(PositionDelegate _onPositionUpdated)
        {
            onPositionUpdated = _onPositionUpdated;
        }
        public void setOnProgressUpdated(ProgressDelegate _onProgressUpdated)
        {
            onProgressUpdated = _onProgressUpdated;
        }
        private string log(string text)
        {
            if (statusDelegate != null)
                statusDelegate.Invoke(text);
            return text;
        }
        private string messageBox(string text)
        {
            if (messageBoxDelegate != null)
                messageBoxDelegate.Invoke(text);
            return text;
        }
    }
}
