using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 *КОММЕНТАРИИ К СООБЩЕНИЯМ ОБМЕНА ИНФОРМАЦИЕЙ МЕЖДУ СЕРВЕРОМ И КЛИЕНТОМ
 *сообщение имеет поля Type - позволяет быстро определись информацию сообщения
 * Если Type равно Y - то это ответ на другое сообщения что информация принята
 * Если Type равно N - то это ответ на другое сообщение, что сообщение не принято, в тексте сообщения есть комментарий.
 * Если Type равно R - то в тексте сообщения содержится системная информация, сообщение надо расшифровывать.
 * Сообщение содержит поля Field1 и Field2 типа байт, в которых зашифрован способ расшифровки сообщения
 * 
 *
 */
namespace Client
{/*
    class GSMessage
    {
        /// <summary>
        /// Y - принято к исполнению, N - не принято, есть комментарий, R - надо рассшифровывать
        /// </summary>
        public char Type;
        public byte Field1; //первое поле для определения способа расшифровки
        public byte Field2; //второе поле для определения способа расшифровки
        public string Message; //сообщение
        public bool IsMessage; //истина, если сообщение верно создано
        public GSMessage(bool result)
        {
            if (result)
            {
                Type = 'Y';
                Field1 = Field2 = 0;
                Message = "";
                IsMessage = true;
            }
            else
            {
                Type = 'N';
                Field1 = Field2 = 0;
                Message = "";
                IsMessage = true;
            }
        }
        public GSMessage(bool result, string message)
        {
            if (result)
            {
                Type = 'Y';
                Field1 = Field2 = 0;
                Message = message;
                IsMessage = true;
            }
            else
            {
                Type = 'N';
                Field1 = Field2 = 0;
                Message = message;
                IsMessage = true;
            }
        }
        public GSMessage(byte field1, byte field2, string message)
        {
            Type = 'R';
            Field1 = field1;
            Field2 = field2;
            Message = message;
            IsMessage = true;
        }
        public override string ToString()
        {
            if (!IsMessage)
                return false.ToString();
            else
                return Type.ToString() + ((char)(Field2 * 256 + Field1)).ToString() + Message;
        }
        public byte[] GetBytes()
        {
            string line = ToString();
            return Common.StringToByte(line);
        }
        public static GSMessage GetMessageFromBytes(byte[] array)
        {
            string crypt = Common.GetStringFromByteArray(array, 0);
            GSMessage message = new GSMessage(crypt);
            return message;
        }
        public GSMessage(string crypt)
        {
            if (crypt.Length < 2)
            {
                IsMessage = false;
                return;
            }
            if (crypt[0] == 'Y')
            {
                Type = 'Y'; Field1 = Field2 = 0;
                Message = crypt.Substring(2);
                IsMessage = true;
            }
            else if (crypt[0] == 'N')
            {
                Type = 'N'; Field1 = Field2 = 0;
                Message = crypt.Substring(2);
                IsMessage = true;
            }
            else if (crypt[0] == 'R')
            {
                Type = 'R';
                char second = crypt[1];
                Field2 = (byte)(second / 256);
                Field1 = (byte)(second - Field2 * 256);
                Message = crypt.Substring(2);
                IsMessage = true;
            }
            else
            {
                IsMessage = false;
            }

        }
        public static string Send(bool result, string message)
        {
            if (result)
                return "Y0" + message;
            else
                return "N0" + message;
        }
    }
  */
}
