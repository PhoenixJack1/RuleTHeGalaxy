using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    #region Enums
    public enum ItemSize { Small, Medium, Large, Any};
    
    #endregion
    public class Item
    {
        public ushort ID;
        public string Name;
        public ItemSize Size;
        public int Consume;
        public ItemPrice Price;

        public Item(ushort id, string name, ItemSize size, int consume, ItemPrice price)
        {
            ID = id; Name = name; Size = size; Consume = consume; Price = price;
        }
        /*
        public static Item GetItem(byte[] array, ref int startindex)
        {
           
            int id = BitConverter.ToInt32(array, startindex);
            startindex += 4;
            ItemPrice price = ItemPrice.GetPrice(array, startindex);
            startindex += 16;
            ItemSize size = (ItemSize)BitConverter.ToInt32(array, startindex);
            startindex += 4;
            int consume = BitConverter.ToInt32(array, startindex);
            startindex += 4;
            int nameLength = BitConverter.ToInt32(array, startindex);
            startindex += 4;
            string name = Common.GetStringFromByteArray(array, startindex, nameLength);
            startindex += nameLength * 2;
            Item item = new Item(id, name, size, consume, price);
            return item;
        }
         */
        public byte[] GetCode()
        {
            List<byte> Code = new List<byte>();
            Code.AddRange(BitConverter.GetBytes(ID));
            Code.AddRange(Price.GetCode());
            Code.AddRange(BitConverter.GetBytes(((int)Size)));
            Code.AddRange(BitConverter.GetBytes(Consume));
            Code.AddRange(BitConverter.GetBytes(Name.Length));
            Code.AddRange(Common.StringToByte(Name));

            return Code.ToArray();
        }
        public override string ToString()
        {
            return Name;
        }
        public ItemSize GetSize()
        {
            return Size;
        }
        public WeaponGroup GetWeaponGroup()
        {
            return WeaponGroup.Any;
        }
        protected Inline AddInline(string caption, Inline value)
        {
            Span Line = new Span(); Line.Inlines.Add(new Run(caption + ": "));
            Line.Inlines.Add(value); Line.Inlines.Add(new LineBreak());
            return Line;
        }
        
    }
    /*
    interface ItemInterface
    {
        ItemSize GetSize();
        WeaponGroup GetWeaponGroup();
        void SetStats(ShipParamsClass schema);
        //void SetPrice(ItemPrice price);

    }
     */
    /*
    struct RepairPrice
    {
        public int Money;
        public int Metal;
        public int Chips;
        public int Anti;
        public RepairPrice(int money, int metal, int chips, int anti)
        {
            Money = money;
            Metal = metal;
            Chips = chips;
            Anti = anti;
        }
        public bool CheckPrice()
        {
            if (Money > GSGameInfo.Money) return false;
            if (Metal > GSGameInfo.Metals) return false;
            if (Chips > GSGameInfo.Chips) return false;
            if (Anti > GSGameInfo.Anti) return false;
            return true;
        }
    }
     */
    public struct ItemPrice
    {
        public int Money;
        public int Metall;
        public int Chips;
        public int Anti;
        public ItemPrice(ItemPrice price)
        {
            Money = price.Money;
            Metall = price.Metall;
            Chips = price.Chips;
            Anti = price.Anti;
        }
        public static ItemPrice GetUpgradePrice(ItemPrice building, ItemPrice version, byte count)
        {
            int money = version.Money * count - building.Money * count / 2;
            int metal = version.Metall * count - building.Metall * count / 2;
            int chips = version.Chips * count - building.Chips * count / 2;
            int anti = version.Anti * count - building.Anti * count / 2;
            return new ItemPrice(money, metal, chips, anti);
        }
        public ItemPrice Mult(byte count)
        {
            return new ItemPrice(Money * count, Metall * count, Chips * count, Anti * count);
        }
        public ItemPrice(int money, int metall, int chips, int anti)
        {
            Money = money; Metall = metall; Chips = chips; Anti = anti;
        }
        public ItemPrice GetHalfPrice()
        {
            return new ItemPrice(Money / 2, Metall / 2, Chips / 2, Anti / 2);
        }
        public void Add(ItemPrice price)
        {
            Money += price.Money; Metall += price.Metall;
            Chips += price.Chips; Anti += price.Anti;
        }
        public void Remove(ItemPrice price)
        {
            Money -= price.Money; Metall -= price.Metall;
            Chips -= price.Chips; Anti -= price.Anti;
        }
        public byte[] GetCode()
        {
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter.GetBytes(Money));
            result.AddRange(BitConverter.GetBytes(Metall));
            result.AddRange(BitConverter.GetBytes(Chips));
            result.AddRange(BitConverter.GetBytes(Anti));
            return result.ToArray();
        }
        public static ItemPrice GetPrice(byte[] array, int z)
        {
            ItemPrice price = new ItemPrice();
            price.Money = BitConverter.ToInt32(array, z);
            price.Metall = BitConverter.ToInt32(array, z + 4);
            price.Chips = BitConverter.ToInt32(array, z + 8);
            price.Anti = BitConverter.ToInt32(array, z + 12);
            return price;
        }
        public bool CheckPrice(int money, int metal, int chips, int anti)
        {
            if (Money < money) return false;
            if (Metall < metal) return false;
            if (Chips < chips) return false;
            if (Anti < anti) return false;
            return true;
        }
        public bool CheckPrice()
        {
            if (GSGameInfo.Money < Money) return false;
            if (GSGameInfo.Metals < Metall) return false;
            if (GSGameInfo.Chips < Chips) return false;
            if (GSGameInfo.Anti < Anti) return false;
            return true;
        }
        public bool CheckPrice(byte starthealth)
        {
            starthealth = (byte)(100 - starthealth);
            if (GSGameInfo.Money < (Money*starthealth/100.0)) return false;
            if (GSGameInfo.Metals < (Metall*starthealth/100.0)) return false;
            if (GSGameInfo.Chips < (Chips*starthealth/100.0)) return false;
            if (GSGameInfo.Anti < (Anti*starthealth/100.0)) return false;
            return true;
        }
        public bool CheckPrice(ItemPrice price)
        {
            return CheckPrice(price.Money, price.Metall, price.Chips, price.Anti);
        }
        public override string ToString()
        {
            return String.Format("Money={0} Metals={1} Chips={2} Anti={3}", Money, Metall, Chips, Anti);
        }
        public StackPanel GetStackPanel()
        {
            
        Rectangle MoneyRectangle;
        Rectangle MetalRectangle;
        Rectangle ChipRectangle;
        Rectangle AntiRectangle;
        Label MoneyLabel;
        Label MetalLabel;
        Label ChipLabel;
        Label AntiLabel;
        
            MoneyRectangle = new Rectangle(); MoneyRectangle.Width = 20; MoneyRectangle.Height = 20;
            MoneyRectangle.Fill = Links.Brushes.MoneyImageBrush; MoneyRectangle.VerticalAlignment = VerticalAlignment.Center;
            MetalRectangle = new Rectangle(); MetalRectangle.Width = 20; MetalRectangle.Height = 20;
            MetalRectangle.Fill = Links.Brushes.MetalImageBrush; MetalRectangle.VerticalAlignment = VerticalAlignment.Center;
            ChipRectangle = new Rectangle(); ChipRectangle.Width = 20; ChipRectangle.Height = 20;
            ChipRectangle.Fill = Links.Brushes.ChipsImageBrush; ChipRectangle.VerticalAlignment = VerticalAlignment.Center;
            AntiRectangle = new Rectangle(); AntiRectangle.Width = 20; AntiRectangle.Height = 20;
            AntiRectangle.Fill = Links.Brushes.AntiImageBrush; AntiRectangle.VerticalAlignment = VerticalAlignment.Center;
            MoneyLabel = new Label(); MoneyLabel.Content = Money; MoneyLabel.Style = Links.SmallTextStyle;
            MetalLabel = new Label(); MetalLabel.Content = Metall; MetalLabel.Style = Links.SmallTextStyle;
            ChipLabel = new Label(); ChipLabel.Content = Chips; ChipLabel.Style = Links.SmallTextStyle;
            AntiLabel = new Label(); AntiLabel.Content = Anti; AntiLabel.Style = Links.SmallTextStyle;
            MoneyLabel.VerticalAlignment = VerticalAlignment.Center; MetalLabel.VerticalAlignment = VerticalAlignment.Center;
            ChipLabel.VerticalAlignment = VerticalAlignment.Center; AntiLabel.VerticalAlignment = VerticalAlignment.Center;
            StackPanel pricePanel = new StackPanel();
            pricePanel.Orientation = Orientation.Horizontal;
            if (Money != 0)
            {
                pricePanel.Children.Add(MoneyRectangle);
                pricePanel.Children.Add(MoneyLabel);
            }
            if (Metall != 0)
            {
                pricePanel.Children.Add(MetalRectangle);
                pricePanel.Children.Add(MetalLabel);
            }
            if (Chips != 0)
            {
                pricePanel.Children.Add(ChipRectangle);
                pricePanel.Children.Add(ChipLabel);
            }
            if (Anti != 0)
            {
                pricePanel.Children.Add(AntiRectangle);
                pricePanel.Children.Add(AntiLabel);
            }
            return pricePanel;
    
        }
    }
}
