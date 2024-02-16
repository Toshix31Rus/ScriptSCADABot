using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Configuration;

namespace ConsoleApp2
{
    class Program
    {
        public class InlineKeyboardMarkup
        {
            public InlineKeyboardButton[][] inline_keyboard;
            public InlineKeyboardMarkup(InlineKeyboardButton[][] _KeyboardButton)
            {
                this.inline_keyboard = _KeyboardButton;
            }
        }

        public class ReplyKeyboardMarkup
        {
            public KeyboardButton[][] keyboard;
            public bool resize_keyboard { get; set; }
            public ReplyKeyboardMarkup(KeyboardButton[][] _Button)
            {
                this.keyboard = _Button;
                this.resize_keyboard = true;
            }
        }

        public class KeyboardButton
        {
            public string text { get; set; }
            public bool request_contact { get; set; }
        }

        public class CallbackQuery
        {
            public string id { get; set; }
            public Message message { get; set; }
            public string data { get; set; }
        }

        public class InlineKeyboardButton
        {
            public string callback_data { get; set; }
            public string text { get; set; }
        }

        public class Field
        {
            public bool ok { get; set; }
            public My result { get; set; }
        }

        public class My
        {
            public long id { get; set; }
            public bool is_bot { get; set; }
            public string first_name { get; set; }
            public string username { get; set; }
            public bool can_join_groups { get; set; }
            public bool can_read_all_group_messages { get; set; }
            public bool supports_inline_queries { get; set; }
        }

        public class Rootobject
        {
            public bool ok { get; set; }
            public Result[] result { get; set; }
        }

        public class Result
        {
            public long update_id { get; set; }
            public Message message { get; set; }
            public CallbackQuery callback_query { get; set; }
        }

        public class Message
        {
            public long message_id { get; set; }
            public From from { get; set; }
            public Chat chat { get; set; }
            public int date { get; set; }
            public string text { get; set; }
            public Contact contact { get; set; }
        }

        public class From
        {
            public long id { get; set; }
            public bool is_bot { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string username { get; set; }
            public string language_code { get; set; }
        }

        public class Chat
        {
            public long id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string username { get; set; }
            public string type { get; set; }
        }

        public class Contact
        { 
            public string phone_number { get; set; }
            public string first_name { get; set; }
        }

        public class Tele_Bot
        {
            private long numbMessage;
            private string _mytoken;
            private static string url = "https://api.telegram.org/bot";
            private static HttpClient Client;

            public Tele_Bot(string _mtok)
            {
                _mytoken = _mtok;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                Client = new HttpClient();
            }
            /// <summary>
            /// Отправка сообщения абоненту
            /// </summary>
            /// <param name="_id"></param>
            /// <param name="mes"></param>
            public async void SendTextMessage(long _id, string mes)
            {
                string set_url = url + _mytoken + "/sendMessage?chat_id=" + _id.ToString() + "&text=" + mes;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, set_url);
                HttpResponseMessage response = await Client.SendAsync(request);
                byte[] Content = await response.Content.ReadAsByteArrayAsync();
                string json = Encoding.UTF8.GetString(Content);
            }


            public async void SendTextMessage_new(long _id, string mes)
            {
                string set_url = url + _mytoken + "/sendMessage?chat_id=" + _id.ToString() + "&text=" + mes;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, set_url);
                HttpResponseMessage response = await Client.SendAsync(request);
                byte[] Content = await response.Content.ReadAsByteArrayAsync();
                string json = Encoding.UTF8.GetString(Content);
            }
            /// <summary>
            /// Отправка сообщения абоненту с включением кнопок в тексте
            /// </summary>
            /// <param name="_id"></param>
            /// <param name="mes"></param>
            /// <param name="keyboard"></param>
            public async void SendTextMessage_newBut(long _id, string mes, InlineKeyboardMarkup keyboard)
            {
                string json_reply = Newtonsoft.Json.JsonConvert.SerializeObject(keyboard);
                string set_url = url + _mytoken + "/sendMessage?chat_id=" + _id.ToString() + "&text=" + mes + "&reply_markup= " + json_reply;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, set_url);
                HttpResponseMessage response = await Client.SendAsync(request);
                byte[] Content = await response.Content.ReadAsByteArrayAsync();
                string json = Encoding.UTF8.GetString(Content);
            }
            /// <summary>
            /// Отправка сообщения абоненту с включением кнопок внизу экрана
            /// </summary>
            /// <param name="_id"></param>
            /// <param name="mes"></param>
            /// <param name="keyboard"></param>
            public async void SendTextMessage_newBut(long _id, string mes, ReplyKeyboardMarkup keyboard)
            {
                string json_reply = Newtonsoft.Json.JsonConvert.SerializeObject(keyboard);
                string set_url = url + _mytoken + "/sendMessage?chat_id=" + _id.ToString() + "&text=" + mes + "&reply_markup= " + json_reply;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, set_url);
                HttpResponseMessage response = await Client.SendAsync(request);
                byte[] Content = await response.Content.ReadAsByteArrayAsync();
                string json = Encoding.UTF8.GetString(Content);
            }

            public async void GetBot()
            {
                string set_url = url + _mytoken + "/getMe";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, set_url);
                HttpResponseMessage response = await Client.SendAsync(request);
                byte[] Content = await response.Content.ReadAsByteArrayAsync();
                string json = Encoding.UTF8.GetString(Content);
                var rootobject = JsonConvert.DeserializeObject<Field>(json);
                numbMessage = rootobject.result.id;
            }

            public async Task<Result[]> OnMessage_new()
            {
                string set_url = url + _mytoken + "/getUpdates?timeout=1000&offset=" + (numbMessage + 1).ToString();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, set_url);
                HttpResponseMessage response = await Client.SendAsync(request);
                byte[] Content = await response.Content.ReadAsByteArrayAsync();
                string json = Encoding.UTF8.GetString(Content);
                var rootobject = JsonConvert.DeserializeObject<Rootobject>(json);
                if (rootobject.result != null)
                    if (rootobject.result.Length != 0)
                    {
                        var rt = rootobject.result[rootobject.result.Length - 1];
                        numbMessage = rt.update_id;
                        Result[] rtd = new Result[rootobject.result.Length];
                        for (int i = 0; i < rootobject.result.Length; i++)
                        {
                            rtd[i] = rootobject.result[i];
                        }
                        return rtd;
                    }
                    else
                        return null;
                else
                    return null;
            }
        }

        public static InlineKeyboardButton[][] GetInlineKeyboard(string[] stringArray1, string[] stringArray2)
        {
            var keyboardInline = new InlineKeyboardButton[2][];
            var keyboardButtons = new InlineKeyboardButton[stringArray1.Length];
            for (var i = 0; i < stringArray1.Length; i++)
            {
                keyboardButtons[i] = new InlineKeyboardButton
                {
                    callback_data = i.ToString(),
                    text = stringArray1[i]
                };
            }
            keyboardInline[0] = keyboardButtons;
            keyboardButtons = new InlineKeyboardButton[stringArray2.Length];
            for (var i = 0; i < stringArray2.Length; i++)
            {
                keyboardButtons[i] = new InlineKeyboardButton
                {
                    callback_data = (i + stringArray1.Length).ToString(),
                    text = stringArray2[i]
                };
            }
            keyboardInline[1] = keyboardButtons;
            return keyboardInline;
        }
        //Функции для создания кнопок
        public static KeyboardButton[][] GetReplyKeyboard(string[] stringArray1, string[] stringArray2)
        {
            var keyboardInline = new KeyboardButton[2][];
            var keyboardButtons = new KeyboardButton[stringArray1.Length];
            for (var i = 0; i < stringArray1.Length; i++)
            {
                keyboardButtons[i] = new KeyboardButton
                {
                    text = stringArray1[i],
                };
            }
            keyboardInline[0] = keyboardButtons;
            keyboardButtons = new KeyboardButton[stringArray2.Length];
            for (var i = 0; i < stringArray2.Length; i++)
            {
                keyboardButtons[i] = new KeyboardButton
                {
                    text = stringArray2[i],
                };
            }
            keyboardInline[1] = keyboardButtons;
            return keyboardInline;
        }
        public static KeyboardButton[][] GetReplyKeyboard(string[] stringArray1)
        {
            var keyboardInline = new KeyboardButton[1][];
            var keyboardButtons = new KeyboardButton[stringArray1.Length];
            for (var i = 0; i < stringArray1.Length; i++)
            {
                keyboardButtons[i] = new KeyboardButton
                {
                    text = stringArray1[i],
                    request_contact = true,
                };
            }
            keyboardInline[0] = keyboardButtons;
            return keyboardInline;
        }
        //Класс для хранения запросов боту
        public class TelMes
        {
            public long Id;
            public string Text;
            public string Phone;
        }

        public class Person
        {
            public long Id;
            public string Phone;
            public Person(long _id)
            {
                Id = _id;
                Phone = " ";
            }
            public Person(long _id, string _ph)
            {
                Id = _id;
                Phone = _ph;
            }
        }

        //Это класс - в котором производится счет.
        private class ClassCounter
        {
            private long i = 0;
            public delegate void MethodContainer(); //создаем делегат для метода
            public delegate void MethodContainer_one(string _fileName); //создаем делегат для метода

            public event MethodContainer OnCount;   //согдаем событие сработал таймер

            public event MethodContainer_one OnCount_one;   //согдаем событие сработал таймер2


            public void Count(bool _tik, string _tempF)
            {
                if (_tik == true)
                    i++;

                if (i == 50000 && ArrMessage.Count!=0)
                {
                    //block1 = true;
                    OnCount_one(_tempF);                    
                    i++;
                }

                if (i > 100000)
                {
                    //block = true;
                    OnCount();                    
                    i = 0;
                }
            }
        }

        static void Send_Tele()
        {
            block = true;
            Task t = Task.Factory.StartNew(() => addNewTelegramMessage());
        }

        static void Set_TeleMes(long _Id, string _ms)
        {
            Task t = Task.Factory.StartNew(() => TeleСlient.SendTextMessage(_Id, _ms));
        }

        static void Set_TeleMesBut(long _Id, string _ms, ReplyKeyboardMarkup _btn)
        {
            Task t = Task.Factory.StartNew(() => TeleСlient.SendTextMessage_newBut(_Id, _ms, _btn));
        }
        /// <summary>
        /// Функция добавления запросов телеграмм боту
        /// </summary>
        static void addNewTelegramMessage()
        {
            if (!block1)
            {
                block = true;
                TelMes tlm;
                using (var e = TeleСlient.OnMessage_new())
                {
                    if (e != null)
                    {
                        if (e.Result != null)
                        {
                            if (e.Result[0].message != null)
                            {
                                for (int i = 0; i < e.Result.Length; i++)
                                {
                                    var msg = e.Result[i].message;
                                    var number = msg.chat.id;
                                    var _text = e.Result[i].message.text;
                                    if (TecId != msg.message_id)
                                    {
                                        tlm = new TelMes();
                                        TecId = msg.message_id;
                                        tlm.Id = number;
                                        tlm.Text = _text;
                                        if (msg.contact != null)
                                            if (msg.contact.phone_number != null)
                                                tlm.Phone = msg.contact.phone_number;
                                        ArrMessage.Add(tlm);
                                        string ffr = "";
                                        foreach (var temp in ArrPers)
                                        {
                                            if (temp.Id == tlm.Id)
                                                ffr = temp.Phone;
                                        }

                                        Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss ") +" Входящее сообщение от Id: " + tlm.Id.ToString() + ", Телефон: " + ffr);
                                        Console.WriteLine(tlm.Text);
                                    }
                                }
                                block = false;
                            }
                            block = false;
                        }
                        block = false;
                    }
                    block = false;
                }
                block = false;
            }
            block = false;
        }

        static void Worker(string _fileName)
        {
            var button = new[] { "Давление", "Температура" };
            var button1 = new[] { "Аргон", "Состояние ВКС" };
            var button2 = new[] { "Предоставить номер" };
            var keyboard = new ReplyKeyboardMarkup(GetReplyKeyboard(button, button1));
            var keyboard2 = new ReplyKeyboardMarkup(GetReplyKeyboard(button2));
            //Вот так делается смайлик
            //var smile = char.ConvertFromUtf32(0x1F4A3);
            while (ArrMessage.Count != 0)
            {
                if (!block)
                {
                    block1 = true;
                    for (int i = 0; i < ArrMessage.Count; i++)
                    {
                        bool init = false;
                        bool init1 =false;
                        switch (ArrMessage[i].Text)
                        {
                            case "/start":
                                if (ArrPers.Count == 0)
                                {
                                    long tmp = ArrMessage[i].Id;
                                    ArrPers.Add(new Person(ArrMessage[i].Id));
                                    TeleСlient.SendTextMessage_newBut(ArrMessage[i].Id, "Предоставьте номер:", keyboard2);
                                    init = false;
                                    init1 = false;
                                }
                                else
                                {
                                    for (int yy = 0; yy < ArrPers.Count; yy++)
                                    {
                                        if (ArrMessage[i].Id == ArrPers[yy].Id)
                                        {
                                            if (ArrPers[yy].Phone == " " && ArrMessage[i].Phone != null)
                                            {
                                                ArrPers.Add(new Person(ArrMessage[i].Id, ArrMessage[i].Phone));
                                                WriteFile(ArrPers, _fileName);
                                                TeleСlient.SendTextMessage_newBut(ArrMessage[i].Id, "Вы успешно внесены в базу!", keyboard);
                                                init = false;
                                                init1 = false;
                                            }
                                            else
                                                init = true;
                                            break;
                                        }
                                        else
                                        {
                                            init = false;
                                            init1 = true;
                                        }

                                    }
                                }
                                if (init)
                                {
                                    TeleСlient.SendTextMessage(ArrMessage[i].Id, "Привет! \nВы уже подписаны на сообщения!)\nИдите в ж...у, не засоряйте меня)");
                                }

                                if (!init && init1)
                                {
                                    long tmp = ArrMessage[i].Id;
                                    ArrPers.Add(new Person(ArrMessage[i].Id));
                                    TeleСlient.SendTextMessage_newBut(ArrMessage[i].Id, "Предоставьте номер:", keyboard2);
                                }
                                break;

                            case "Давление":
                                try
                                {
                                    /*
                                    string message_pres = "";
                                    if (SendExess(Dopusk, ArrPers, ArrMessage[i].Id))
                                        message_pres = "Вот тебе и давление :-)";
                                    else
                                        message_pres = "Пошел в ж..пу! У Вас нет допуска! :-)";
                                    Set_TeleMes(ArrMessage[i].Id, message_pres);
                                    */
                                    PrintMessage(ArrMessage[i].Id, "Вот тебе и давление :-)");
                                }
                                catch (Exception EX)
                                {
                                    Console.WriteLine("Косяк в блоке давление!: " + EX.Message);
                                    block1 = false;
                                }
                                break;

                            case "Температура":
                                try
                                {
                                    PrintMessage(ArrMessage[i].Id, "Вот тебе и температура :-)");
                                }
                                catch (Exception EX)
                                {
                                    Console.WriteLine("Косяк в блоке температура!: " + EX.Message);
                                    block1 = false;
                                }
                                break;

                            case "Аргон":
                                try
                                {
                                    PrintMessage(ArrMessage[i].Id, "Вот тебе и показания аргона :-)");
                                }
                                catch (Exception EX)
                                {
                                    Console.WriteLine("Косяк в блоке показания аргона!: " + EX.Message);
                                    block1 = false;
                                }
                                break;

                            case "Состояние ВКС":
                                try
                                {
                                    PrintMessage(ArrMessage[i].Id, "Вот тебе и показания ВКС :-)");
                                }
                                catch (Exception EX)
                                {
                                    Console.WriteLine("Косяк в блоке Состояние ВКС!: " + EX.Message);
                                    block1 = false;
                                }
                                break;

                            default:
                                if (ArrMessage[i].Text == null && ArrMessage[i].Phone != null)
                                {
                                    for (int yy = 0; yy < ArrPers.Count; yy++)
                                    {
                                        if (ArrPers[yy].Id == ArrMessage[i].Id)
                                        {
                                            if (ArrPers[yy].Phone == " " && ArrMessage[i].Phone != null)
                                            {
                                                ArrPers[yy].Phone = ArrMessage[i].Phone;
                                                WriteFile(ArrPers, _fileName);
                                                TeleСlient.SendTextMessage_newBut(ArrMessage[i].Id, "Вы успешно внесены в базу!", keyboard);
                                                break;
                                            }
                                            else
                                                TeleСlient.SendTextMessage(ArrMessage[i].Id, "Привет! \nВы уже подписаны на сообщения!)\nИдите в ж...у, не засоряйте меня)");
                                        }
                                    }
                                }
                                else
                                    Set_TeleMesBut(ArrMessage[i].Id, "Неизвестная команда!", keyboard);
                                    block1 = false;
                                break;
                        }
                    }
                    ArrMessage.Clear();
                    block1 = false;
                }
                block1 = false;
            }
            block1 = false;
        }

        //Функция вывода сообщения по запросу с проверкой доступа
        static void PrintMessage(long _id, string _ms)
        {
            string message_vks = "";
            if (SendExess(Dopusk, ArrPers, _id))
                message_vks = _ms;
            else
                message_vks = "Пошел в ж..пу! У Вас нет допуска! :-)";
            Set_TeleMes(_id, message_vks);
        }
        static void WorkerAsync(string _fn)
        {
            block1 = true;
            Task.Factory.StartNew(() => Worker(_fn));                // выполняется асинхронно
        }

        //Сохранение списка подписчиков в файл
        static void WriteFile(List<Person> _message, string _fP)
        {
            try
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(_fP, false);
                for (int i = 0; i < _message.Count; i++)
                {
                    writer.WriteLine(_message[i].Id.ToString() + ',' + _message[i].Phone.ToString());
                }
                writer.Close();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
        }
        //Получение списка допущенных пользователей
        static bool SendExess(List<String> _dopusk, List<Person> _arrPers, long _id)
        {
            foreach (var pn in _arrPers)
            {
                if (pn.Id == _id)
                {
                    foreach (var dop in _dopusk)
                    {
                        if (pn.Phone == dop)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        static List<Person> ReadFile(string path)
        {
            List<string> file_in = new List<string>();
            List<Person> pers = new List<Person>();
            try
            {
                System.IO.StreamReader rider = new System.IO.StreamReader(path);
                while (true)
                {
                    string s = rider.ReadLine();
                    if (s != null)
                        file_in.Add(s);
                    else
                        break;
                }
                for (int i = 0; i < file_in.Count; i++)
                {
                    //string tty = file_in[i].Trim('+');
                    int ind = file_in[i].IndexOf(',');
                    string _id = file_in[i].Remove(ind);
                    string _phn = file_in[i].Remove(0,ind+1);
                    _phn = _phn.Trim('+'); 
                    Person prs = new Person(Convert.ToInt64(_id), _phn);
                    pers.Add(prs);
                }
                rider.Close();
                return pers;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return pers;
            }
        }

        //Чтение файла с допущенными
        static List<string> ReadaktFile(string path)
        {
            List<string> file_in = new List<string>();
            try
            {
                System.IO.StreamReader rider = new System.IO.StreamReader(path);
                while (true)
                {
                    string s = rider.ReadLine();
                    if (s != null && s.Length == 11)
                        file_in.Add(s);
                    else
                        break;
                }
                rider.Close();              
                return file_in;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return file_in;
            }
        }

        static string Token = " ";
        //переменная для хранения номера последнего входящего сообщения
        static long TecId = 0;

        static bool block, block1;
        //
        static Tele_Bot TeleСlient;
        static ClassCounter Counter;

        //список подписчиков
        static List<Person> ArrPers = new List<Person>();

        //список номеров телефоном допущенных подписчиков
        static List<String> Dopusk = new List<string>();

        //список полученных запрособ боту
        static List<TelMes> ArrMessage = new List<TelMes>();

        static string FileNamePers = ConfigurationManager.ConnectionStrings["FilePers"].ConnectionString;
        static string FileNameaktPers = ConfigurationManager.ConnectionStrings["FileaktPers"].ConnectionString;
        static void Main(string[] args)
        {
            
            // клиент для телеграмм бота
            TeleСlient = new Tele_Bot(Token);
            Counter = new ClassCounter();
            Counter.OnCount += Send_Tele;
            Counter.OnCount_one += WorkerAsync;
            TeleСlient.GetBot();
            if (ArrPers != null)
            {
                if (ArrPers.Count == 0)
                {
                    //ArrPers = new List<Person>(ReadFile(@"C:\\Users\\BurmaginAI\\Desktop\\ConsoleApp2\\ConsoleApp2\\bin\\Debug\\Person.txt"));
                    ArrPers = new List<Person>(ReadFile(@FileNamePers));
                }
            }
            //Dopusk = ReadaktFile(@"C:\\Users\\BurmaginAI\\Desktop\\ConsoleApp2\\ConsoleApp2\\bin\\Debug\\aktPerson.txt");
            Dopusk = ReadaktFile(@FileNameaktPers);
            string EnRun = " ";
            while (EnRun != "Yes")
            {
                Counter.Count(!block && !block1, FileNamePers);
                //Task.Delay(1000);
            }
        }
    }
}
