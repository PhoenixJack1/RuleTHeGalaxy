using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public class ServerClan
    {
        public int ID { get; private set; }
        public GSString Name { get; private set; }
        public long Image { get; private set; }
        public List<GSPlayer> Players;
        public byte[] BasicArray;
        public static byte[] NullBasicArray = CreateNullBasicArray();
        public List<int> LeadersID;
        public List<GSPlayer> Requests;
        int LastDefensePlayer = 0;
        public bool HasFleets = false;
        public ChatMessageArray ClanMessages = new ChatMessageArray(10000);
        public ServerClan(int id, GSString name, long image)
        {
            ID = id; Name = name; Image = image;
            Players = new List<GSPlayer>();
            LeadersID = new List<int>();
            Requests = new List<GSPlayer>();
            ServerLinks.GSClansByName.Add(name.ToString(), this);
            ServerLinks.GSClanByID.Add(ID, this);
            CreateBasicArray();
        }
       /* public ServerFleet GetDefenseFleet(GSPlayer player) //метод возвращает флот для защиты игрока у которого нет флотов
        {
            if (HasFleets == false) return null;
            ServerFleet fleet = null;
            for (int i = 0; i < Players.Count; i++)
            {
                LastDefensePlayer = (LastDefensePlayer + 1) % Players.Count;
                GSPlayer nextplayer = Players[LastDefensePlayer];
                if (nextplayer == player) continue;
                fleet = nextplayer.SelectFleetForDefense();
                if (fleet != null)
                    return fleet;
            }
            HasFleets = false;
            return null;
        }*/
        public bool LeavePlayer(GSPlayer player) //удаляет игрока из клана
        {
            if (LeadersID.Contains(player.ID))
                return false;
            Players.Remove(player);
            player.Clan = null;
            player.CreateArrayForPlayer();
            return true;
        }
        public bool InvitePlayer(GSPlayer player) //принимаиет игрока в клан
        {
            if (Players.Count >= 40) return false;
            Players.Add(player);
            Requests.Remove(player);
            player.Clan = this;
            player.CreateArrayForPlayer();
            player.RequestClan = null;
            return true;
        }
        public void RemoveInvite(GSPlayer player) //удаляет запрос на приём в клан
        {
            Requests.Remove(player);
            player.RequestClan = null;
        }
        public void RecieveRequest(GSPlayer player) //добавление реквеста на вход в клан
        {
            if (!Requests.Contains(player))
            {
                Requests.Add(player);
                player.RequestClan = this;
                player.ClanChangesTime = DateTime.Now;
            }
        }
        public void RemoveRequest(GSPlayer player) //удаление реквеста на вход в клан
        {
            if (Requests.Contains(player))
            {
                Requests.Remove(player);
                player.RequestClan = null;
            }
        }
        public void RemovePlayer(GSPlayer player) //выход игрока из клана
        {
            if (LeadersID.Contains(player.ID))
                LeadersID.Remove(player.ID);
            if (Players.Contains(player))
                Players.Remove(player);
            player.Clan = null;
            player.ClanChangesTime = DateTime.Now;
            player.CreateArrayForPlayer();
            if (Players.Count == 0)
            {
                ServerLinks.GSClanByID.Remove(ID);
                ServerLinks.GSClansByName.Remove(Name.ToString());
                foreach (GSPlayer rec in Requests)
                    rec.RequestClan = null;
                Requests.Clear();
            }
        }
        public static void SelectLeadersAll() //метод выбирающий лидеров во всех кланах
        {
            foreach (ServerClan clan in ServerLinks.GSClanByID.Values)
                clan.SelectLeaders();
            Console.WriteLine("Leaders Selected");
        }
        public void SelectLeaders() //метод выбирающий трёх лидеров в клане
        {
            int[] MoneyProd = new int[Players.Count];
            for (int i = 0; i < Players.Count; i++)
                foreach (ServerLand land in Players[i].Lands.Values)
                    MoneyProd[i] += land.Add.Money;

            int maxvalue, curindex;
            if (Players.Count == 0) return;
            //GSPlayer player = Players[0];

            for (int i = 0; i < Players.Count; i++)
            {
                maxvalue = MoneyProd[i];
                curindex = i;
                //player = Players[0];
                for (int j = i + 1; j < Players.Count; j++)
                {
                    if (MoneyProd[j] > maxvalue)
                    {
                        maxvalue = MoneyProd[j];
                        curindex = j;
                    }
                }
                GSPlayer temp1 = Players[i];
                Players[i] = Players[curindex];
                Players[curindex] = temp1;
                int temp2 = MoneyProd[i];
                MoneyProd[i] = MoneyProd[curindex];
                MoneyProd[curindex] = temp2;
            }
            LeadersID.Clear();
            for (int i = 0; i < 3 && i < Players.Count; i++)
                LeadersID.Add(Players[i].ID);
        }
        /*public byte[] GetTotalInfo() //информация об участниках клана
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(LeadersID.Count));
            foreach (int leaderid in LeadersID)
                list.AddRange(BitConverter.GetBytes(leaderid));
            foreach (GSPlayer player in Players)
                list.AddRange(player.GetInfoForClan());
            return list.ToArray();
        }
        public byte[] GetRequestsInfo() //информация о заявках в клан
        {
            List<byte> list = new List<byte>();
            foreach (GSPlayer player in Requests)
                list.AddRange(player.GetInfoForClan());
            return list.ToArray();
        }*/
        public byte[] GetClanShortInfo() //краткая информация о клане
        {
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(ID));
            list.AddRange(Name.Array);
            list.AddRange(BitConverter.GetBytes(Image));
            list.AddRange(BitConverter.GetBytes(Players.Count));
            int lands = 0;
            foreach (GSPlayer player in Players)
                lands += player.Lands.Count;
            list.AddRange(BitConverter.GetBytes(lands));
            return list.ToArray();
        }
        public static bool CreateNewClan(GSString name, long image) //создаёт новый клан
        {
            if (ServerLinks.GSClansByName.ContainsKey(name.ToString()))
                return false;
            int id = ClanID.GetID();
            ServerClan clan = new ServerClan(id, name, image);
            return true;
        }
        public static byte[] GetRequestClan(ServerClan clan) //метод возвращает информацию о клане в которой подана заявка
        {
            List<byte> list = new List<byte>();
            list.Add(1); //флаг что угрока отправлен запрос на вступление в клан
            list.AddRange(clan.GetClanShortInfo());
            return list.ToArray();
        }
        public static byte[] GetClanList(int clanid) //даёт список из 12 кланов, по возрастанию или убыванию 
        {
            List<byte> list = new List<byte>();
            list.Add(0); //флаг что у игрока НЕ отрпвлен запрос на вступление в клан
            int index = 0;
            if (clanid > 0)
            {
                if (!ServerLinks.GSClanByID.ContainsKey(clanid)) index = 0;
                else
                {
                    index = ServerLinks.GSClanByID.IndexOfKey(clanid) + 1;
                    if (index + 12 > ServerLinks.GSClanByID.Count)
                        index = ServerLinks.GSClanByID.Count - 12;
                }
            }
            else if (clanid < 0)
            {
                int curid = -clanid;
                if (!ServerLinks.GSClanByID.ContainsKey(curid)) index = 0;
                else
                {
                    index = ServerLinks.GSClanByID.IndexOfKey(curid) - 12;
                    if (index < 0) index = 0;
                }
            }
            for (int i = index; i < ServerLinks.GSClanByID.Count; i++)
            {
                if (i > index + 11) break;
                ServerClan clan = ServerLinks.GSClanByID.Values[i];
                list.AddRange(clan.GetClanShortInfo());
            }
            return list.ToArray();
        }
        public void CreateBasicArray() //информация о клане в базовой информации игрока
        {
            List<byte> list = new List<byte>();
            list.Add(0); list.Add(9);
            list.AddRange(BitConverter.GetBytes(Name.Array.Length + 4 + 8));
            list.AddRange(BitConverter.GetBytes(ID));
            list.AddRange(BitConverter.GetBytes(Image));
            list.AddRange(Name.Array);
            BasicArray = list.ToArray();
        }
        public static byte[] CreateNullBasicArray() //информация о нулевом клане в базовой информации игрока
        {
            List<byte> list = new List<byte>();
            list.Add(0); list.Add(9);
            list.AddRange(BitConverter.GetBytes(4));
            list.AddRange(BitConverter.GetBytes(-1));
            return list.ToArray();
        }
        public void Show() //выводит информацию о клане в консоль
        {
            Console.WriteLine("ID={0} Name: {1} Invite requests: {2}", ID, Name.ToString(), Requests.Count);
            foreach (GSPlayer player in Players)
                Console.WriteLine(player.Name);
            Console.WriteLine("--------------------------");
        }
    }
    class ClanID
    {
        public static int ID = -1;
        public static int GetID()
        {
            ID++;
            return ID;
        }
        public static void CheckID(int id)
        {
            if (ID < id)
                ID = id;
        }
    }
    public class ChatMessageArray
    {
        int MaxLength;
        int HalfLength;
        public List<byte> CurList = new List<byte>();
        int MiddlePos = 0;

        public ChatMessageArray(int maxlength)
        {
            MaxLength = maxlength;
            HalfLength = maxlength / 2;
        }
        public void AddMessage(byte[] message)
        {
            if (CurList.Count >= MaxLength)
            {
                CurList.RemoveRange(0, MiddlePos);
                MiddlePos = CurList.Count;
            }
            if (MiddlePos < HalfLength)
            {
                MiddlePos += message.Length;
            }
            CurList.AddRange(message);
        }

    }
}
