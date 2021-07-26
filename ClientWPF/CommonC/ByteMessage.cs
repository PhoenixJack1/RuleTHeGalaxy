using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum MessageResult { Error, Yes, No, Responce };
    class ByteMessage
    {

        public MessageResult Type = MessageResult.Error;
        public byte Field1; //первое поле для определения способа расшифровки
        public byte Field2; //второе поле для определения способа расшифровки
        public byte[] Message; //сообщение
        public ByteMessage(MessageResult type, byte field1, byte field2, byte[] message)
        {
            Type = type; Field1 = field1; Field2 = field2; Message = message;
        }
        public ByteMessage(byte field1, byte field2, byte[] message)
        {
            Type = MessageResult.Responce;
            Field1 = field1;
            Field2 = field2;
            Message = message;
        }
        public ByteMessage(bool type, byte[] message)
        {
            if (type)
                Type = MessageResult.Yes;
            else
                Type = MessageResult.No;
            Message = message;

        }
        public ByteMessage(byte[] array)
        {
            if (array.Length < 3) return;
            switch (array[0])
            {
                case 1: Type = MessageResult.Yes; break;
                case 2: Type = MessageResult.No; break;
                case 3: Type = MessageResult.Responce; break;
                default: return;
            }
            Field1 = array[1];
            Field2 = array[2];
            Message = Move(array, 3);
        }
        public static ByteMessage SaveReconMessage(byte[] array)
        {
            if (array.Length < 3) return null;
            if (array[0] == 0 || array[0] > 3) return null;
            return new ByteMessage((MessageResult)array[0], array[1], array[2], Move(array, 3));

        }
        static byte[] Move(byte[] from, int pos)
        {
            byte[] result = new byte[from.Length - pos];
            int k = 0;
            for (int i = pos; i < from.Length; i++)
            {
                result[k] = from[i];
                k++;
            }
            return result;
        }
        public ByteMessage(MessageResult result)
        {
            if (result == MessageResult.Yes || result == MessageResult.No) Type = result;
        }
        public ByteMessage(string text)
        {
            Type = MessageResult.Responce;
            Message = Common.StringToByte(text);
        }
        public byte[] GetBytes()
        {
            if (Type == MessageResult.Error) return null;
            List<byte> result = new List<byte>();
            result.Add((byte)Type);
            result.Add(Field1);
            result.Add(Field2);
            if (Message != null) result.AddRange(Message);
            return result.ToArray();
        }
        public byte[] AddCurTime(byte[] array)
        {
            byte[] curtime = BitConverter.GetBytes(ServerLinks.NowTime.Ticks);
            array[0] = curtime[0]; array[1] = curtime[1]; array[2] = curtime[2]; array[3] = curtime[3];
            array[4] = curtime[4]; array[5] = curtime[5]; array[6] = curtime[6]; array[7] = curtime[7];
            return array;
        }
        public byte[] ResponseToInformationRequest(GSPlayer Player)
        {
            switch (Field1)
            {
                case 0:
                    switch (Field2)
                    {
                        case 0: return AddCurTime(Player.ArrayStep); //полный список информации игрока
                        case 8: return Player.Notice.Array.ToArray();
                        case 2: return Player.Resources.Resources; //Ресурсы
                        case 4: return Player.Sciences.ByteArray; //список исследований
                        //case 5: return ServerLinks.GameData.GameArchive1Hash; //Архив №1
                        case 6: return BitConverter.GetBytes(ServerLinks.Science.MaxScinecesNeedsToBeKnownToLearnNextlevel);
                        //case 7: return SciencePrice.BasesCodes; //основы для цен исследований
                        case 15: return GSTimer.Statictic;
                    }
                    break;
                case 3:
                    switch (Field2)
                    {
                        case 2: return Player.Schemas; //список корабельных схем
                        case 4: return Player.ShipsArray; //список кораблей
                    }
                    break;
                case 4:
                    switch (Field2)
                    {
                        case 1:
                            Player.CreateCryptLand();
                            return Player.CryptLands;
                    }
                    break;
                case 5:
                    switch (Field2)
                    {
                        case 1: return ServerLinks.Market.Code;
                    }
                    break;
                case 6:
                    switch (Field2)
                    {
                        case 1: return Player.Pilots.AcademyPilots;
                        case 4: return Player.Pilots.FreePilotsArray;
                    }
                    break;
                case 7:
                    switch (Field2)
                    {
                        case 2: return Player.FleetsArray;
                    }
                    break;
                case 9: //НОВОЕ ДОБАВЛЕНИЕ
                    switch (Field2)
                    {
                        case 2: return ServerStar.FreeLandsArray;
                    }
                    break;
            }
            throw new Exception();
        }
    }
}

