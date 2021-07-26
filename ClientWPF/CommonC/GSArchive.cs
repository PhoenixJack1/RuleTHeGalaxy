using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Client
{
    class GSArchive
    {
        public string ArchiveName { get; private set; }
        public SortedList<string, ObjectToAdd> Objects { get; private set; }
        public bool Status;
        public GSArchive(string FileName, bool IsCreate)
        {
            ArchiveName = FileName;
            Objects = new SortedList<string, ObjectToAdd>();
            if (IsCreate)
            {
                File.Delete(ArchiveName);
            }
            else
            {
                try
                {
                    Status = GetFiles();
                }
                catch (Exception)
                { Status = false; }
            }
        }
        public static byte[] GetHash(string FileName)
        {
            byte[] array = File.ReadAllBytes(FileName);
            return System.Security.Cryptography.SHA256.Create().ComputeHash(array);
            /*
            string line = File.ReadAllText(FileName);
            long result = 0;
            for (int i = 0; i < line.Length; i++)
                result += ((int)line[i] + 17) * ((i % 7 + 1) * 23);
            for (int i = 1; ; i++)
            {
                result = result << 1;
                result += (int)line[i % line.Length];
                if (result > Int64.MaxValue >> 1) break;
            }
            return result;
            */
        }
        bool GetFiles()
        {
            if (!File.Exists(ArchiveName)) return false;
            byte[] array = File.ReadAllBytes(ArchiveName);
            for (int i = 0; i < array.Length; )
            {
                ObjectToAdd file = new ObjectToAdd();
                int FileNameLength = BitConverter.ToInt32(array, i);
                i += 4;
                Queue<char> FileNameQueue = new Queue<char>();
                for (int j = 0; j < FileNameLength; j++, i += 2)
                    FileNameQueue.Enqueue(BitConverter.ToChar(array, i));
                string FileName = new String(FileNameQueue.ToArray());
                file.Name = FileName;
                long TextLength = BitConverter.ToInt64(array, i);
                i += 8;
                byte FileType = array[i++];
                switch (FileType)
                {
                    case 0: file.File = GetText(array, ref i, TextLength); file.Type = EObjectToAddType.String; break;
                    case 1: file.File = GetBitmap(array, ref i, TextLength); file.Type = EObjectToAddType.ImageMemory; break;
                    case 2: file.File = GetByteArray(array, ref i, TextLength); file.Type = EObjectToAddType.ByteArray; break;
                        
                }
                Objects.Add(file.Name, file);

            }
            return true;
        }
        BitmapSource GetBitmap(byte[] array, ref int startIndex, long FileLength)
        {
            List<byte> list = new List<byte>();
            long EndOfImage = startIndex + FileLength;
            for (long i = startIndex; i < EndOfImage; startIndex++, i++)
                list.Add(array[i]);
            MemoryStream stream = new MemoryStream(list.ToArray());
            Bitmap bmp = new Bitmap(stream);

            return loadBitmap(bmp);
        }
        byte[] GetByteArray(byte[] array, ref int startindex, long Length)
        {
            Queue<byte> result = new Queue<byte>();
            for (int i = 0; i < Length; i++)
            {
                result.Enqueue(array[startindex]);
                startindex++;
            }
            return result.ToArray();
        }
        string GetText(byte[] array, ref int startIndex, long Length)
        {
            Queue<char> Text = new Queue<char>();
            for (int i = 0; i < Length; i += 2)
            {
                Text.Enqueue(BitConverter.ToChar(array, startIndex));
                startIndex += 2;
            }
            return new string(Text.ToArray());
        }
        BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {

            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception) { }
            return bs;
        }

        public enum EObjectToAddType { TextFile, Directory, String, ImageMemory, ImageFile, ByteArray }
        public class ObjectToAdd
        {
            public EObjectToAddType Type;
            public string Name;
            public object File;
        }
        public static void GetSounds()
        {
            Links.GameData.Sounds = new GSArchive("Sounds.db", false);

        }
    }
}
