using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum EChatMessageType { ForAll, Private, Clan, System };
    class ChatMessage
    {
        public uint ID;
        public string Text;
        public string Creator;
        public string Target;
        public EChatMessageType Type;
        public bool IsCorrect;
        public ChatMessage(EChatMessageType type, string creator, string target, string text, uint id)
        {
            ID = id;
            Target = target;
            Creator = creator;
            Text = text;
            Type = type;
            IsCorrect = true;
        }
        public byte[] GetText()
        {
            return new GSString(Text).Array;
        }
        public byte[] GetTargetText()
        {
            List<byte> result = new List<byte>();
            result.AddRange(new GSString(Target).Array);
            result.AddRange(new GSString(Text).Array);
            return result.ToArray();

        }
        public static ChatMessage GetChatMessage(byte[] array, ref int pos)
        {
            int type = BitConverter.ToInt32(array, pos); pos += 4;
            GSString CreatorName = GSString.GetGSString(array, pos);
            pos += CreatorName.Array.Length;
            GSString TargetName = GSString.GetGSString(array, pos);
            pos += TargetName.Array.Length;
            GSString text = GSString.GetGSString(array, pos);
            pos += text.Array.Length;
            uint id = BitConverter.ToUInt32(array, pos); pos += 4;
            ChatMessage message = new ChatMessage((EChatMessageType)type, CreatorName.ToString(), TargetName.ToString(), text.ToString(), id);
            return message;
           
        }
        /*
        public ChatMessage(string CryptedText)
        {
            string[] Items = CryptedText.Split(Links.InternalSeparator);
            if (Items.Length != 5) return;
            int inttype;
            if (!Int32.TryParse(Items[0], out inttype)) return;
            if (inttype > 3 || inttype < 0) return;
            Type = (EChatMessageType)inttype;
            if (!Int32.TryParse(Items[4],out ID)) return;
            if (ID < 0) return;

            Creator = Common.CheckString(Items[1]);
            Target = Common.CheckString(Items[2]);
            Text = Common.CheckString(Items[3]);
            IsCorrect = true;
        }
        public override string ToString()
        {
            return ((int)Type).ToString() + Links.InternalSeparator + Creator + Links.InternalSeparator + Target + Links.InternalSeparator + Text;
        }
        */
    }
}
