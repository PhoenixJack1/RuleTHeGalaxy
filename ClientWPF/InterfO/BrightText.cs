using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;

namespace Client
{
    class BrightText : Viewbox
    {
        static SortedList<char, int> Steps = SetSteps();
        public Color CurColor { get; private set; }
        public Color BlurColor { get; private set; }
        public Color Back { get; private set; }
        int Blur;
        List<TextBlock> Blocks = new List<TextBlock>();
        public BrightText(string text, Color color, Color blur_color, Color background, int blur)
        {
            Canvas canvas = new Canvas();
            VerticalAlignment = VerticalAlignment.Center;
            Child = canvas;
            Back = background;
            CurColor = color;
            BlurColor = blur_color;
            Blur = blur;
            int pos = 10;
            for (int i = 0; i < text.Length; i++)
            {
                TextBlock b = GetBack(text[i]);
                canvas.Children.Add(b);
                Canvas.SetLeft(b, pos);
                Blocks.Add(b);
                TextBlock f = GetForward(text[i]);
                canvas.Children.Add(f);
                Canvas.SetLeft(f, pos + 3);
                Canvas.SetTop(f, 6);
                pos += Steps[text[i]];
                Blocks.Add(f);
            }
            canvas.Width = pos + 20;
            canvas.Height = 70;
            canvas.Background = new SolidColorBrush(background);
            Width = pos + 20;
            Height = 70;
        }
        public BrightText(string text, Color color, Color background, int blur)
        {
            Canvas canvas = new Canvas();
            VerticalAlignment = VerticalAlignment.Center;
            Child = canvas;
            Back = background;
            CurColor = color;
            BlurColor = color;
            Blur = blur;
            int pos = 10;
            for (int i = 0; i < text.Length; i++)
            {
                TextBlock b = GetBack(text[i]);
                canvas.Children.Add(b);
                Canvas.SetLeft(b, pos);
                Blocks.Add(b);
                TextBlock f = GetForward(text[i]);
                canvas.Children.Add(f);
                Canvas.SetLeft(f, pos + 3);
                Canvas.SetTop(f, 6);
                pos += Steps[text[i]];
                Blocks.Add(f);
            }
            canvas.Width = pos + 20;
            canvas.Height = 70;
            canvas.Background = new SolidColorBrush(background);
            Width = pos + 20;
            Height = 70;
        }
        public void ChangeColor(Color color, TimeSpan time)
        {
            ColorAnimation anim = new ColorAnimation(color, time);
            foreach (TextBlock block in Blocks)
                block.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim);
        }
        public void Resize(double d)
        {
            Width = Width * d;
            Height = Height * d;
        }
        TextBlock GetBack(Char c)
        {
            TextBlock b = new TextBlock();
            b.FontFamily = new FontFamily("Calibri");
            b.Foreground = new SolidColorBrush(BlurColor);
            b.FontWeight = FontWeights.Bold;
            BlurEffect e = new BlurEffect();
            e.Radius = Blur;
            b.Effect = e;
            b.FontSize = 50;
            b.Text = c.ToString();
            return b;
        }
        TextBlock GetForward(Char c)
        {
            TextBlock b = new TextBlock();
            b.FontFamily = new FontFamily("Calibri");
            b.Foreground = new SolidColorBrush(CurColor);
            b.FontSize = 40;
            b.Text = c.ToString();
            return b;
        }
        static SortedList<char, int> SetSteps()
        {
            SortedList<char, int> steps = new SortedList<char, int>();
            steps.Add('А', 22);
            steps.Add('Б', 20);
            steps.Add('В', 20);
            steps.Add('Г', 16);
            steps.Add('Д', 25);
            steps.Add('Е', 18);
            steps.Add('Ё', 18);
            steps.Add('Ж', 32);
            steps.Add('З', 16);
            steps.Add('И', 22);
            steps.Add('Й', 22);
            steps.Add('К', 22);
            steps.Add('Л', 20);
            steps.Add('М', 30);
            steps.Add('Н', 22);
            steps.Add('О', 24);
            steps.Add('П', 23);
            steps.Add('Р', 20);
            steps.Add('С', 22);
            steps.Add('Т', 22);
            steps.Add('У', 20);
            steps.Add('Ф', 26);
            steps.Add('Х', 20);
            steps.Add('Ц', 25);
            steps.Add('Ч', 20);
            steps.Add('Ш', 32);
            steps.Add('Щ', 36);
            steps.Add('Ъ', 22);
            steps.Add('Ы', 26);
            steps.Add('Ь', 20);
            steps.Add('Э', 20);
            steps.Add('Ю', 32);
            steps.Add('Я', 20);
            steps.Add('а', 16);
            steps.Add('б', 20);
            steps.Add('в', 16);
            steps.Add('г', 14);
            steps.Add('д', 20);
            steps.Add('е', 18);
            steps.Add('ё', 18);
            steps.Add('ж', 25);
            steps.Add('з', 15);
            steps.Add('и', 20);
            steps.Add('й', 18);
            steps.Add('к', 17);
            steps.Add('л', 18);
            steps.Add('м', 24);
            steps.Add('н', 19);
            steps.Add('о', 20);
            steps.Add('п', 18);
            steps.Add('р', 18);
            steps.Add('с', 16);
            steps.Add('т', 15);
            steps.Add('у', 17);
            steps.Add('ф', 22);
            steps.Add('х', 16);
            steps.Add('ц', 20);
            steps.Add('ч', 16);
            steps.Add('ш', 26);
            steps.Add('щ', 30);
            steps.Add('ъ', 20);
            steps.Add('ы', 24);
            steps.Add('ь', 18);
            steps.Add('э', 16);
            steps.Add('ю', 26);
            steps.Add('я', 16);
            steps.Add('A', 20);
            steps.Add('B', 20);
            steps.Add('C', 20);
            steps.Add('D', 22);
            steps.Add('E', 18);
            steps.Add('F', 16);
            steps.Add('G', 22);
            steps.Add('H', 22);
            steps.Add('I', 8);
            steps.Add('J', 10);
            steps.Add('K', 20);
            steps.Add('L', 16);
            steps.Add('M', 30);
            steps.Add('N', 22);
            steps.Add('O', 24);
            steps.Add('P', 20);
            steps.Add('Q', 26);
            steps.Add('R', 20);
            steps.Add('S', 18);
            steps.Add('T', 20);
            steps.Add('U', 22);
            steps.Add('V', 22);
            steps.Add('W', 34);
            steps.Add('X', 20);
            steps.Add('Y', 18);
            steps.Add('Z', 18);
            steps.Add('a', 18);
            steps.Add('b', 18);
            steps.Add('c', 16);
            steps.Add('d', 18);
            steps.Add('e', 18);
            steps.Add('f', 12);
            steps.Add('g', 18);
            steps.Add('h', 18);
            steps.Add('i', 8);
            steps.Add('j', 8);
            steps.Add('k', 18);
            steps.Add('l', 8);
            steps.Add('m', 30);
            steps.Add('n', 18);
            steps.Add('o', 18);
            steps.Add('p', 18);
            steps.Add('q', 24);
            steps.Add('r', 14);
            steps.Add('s', 14);
            steps.Add('t', 12);
            steps.Add('u', 18);
            steps.Add('v', 16);
            steps.Add('w', 26);
            steps.Add('x', 18);
            steps.Add('y', 16);
            steps.Add('z', 14);
            steps.Add('1', 18);
            steps.Add('2', 18);
            steps.Add('3', 18);
            steps.Add('4', 18);
            steps.Add('5', 18);
            steps.Add('6', 18);
            steps.Add('7', 18);
            steps.Add('8', 18);
            steps.Add('9', 18);
            steps.Add('0', 18);
            steps.Add('.', 8);
            steps.Add(',', 8);
            steps.Add(' ', 8);
            steps.Add('!', 12);
            steps.Add('?', 16);
            steps.Add(':', 8);
            steps.Add(')', 12);
            steps.Add('(', 16);
            steps.Add('-', 12);
            steps.Add('\'', 8);
            steps.Add('\"', 10);
            steps.Add('%', 18);
            return steps;
        }
    }
}
