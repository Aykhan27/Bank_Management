using System;
using System.Collections.Generic;
using System.Linq;

namespace kursac
{
    class Window
    {
        public static void Clear(Rect rect)
        {
            Console.CursorVisible = false;
            for (int i = rect.top; i <= rect.top + rect.height; i++)
            {
                for (int j = rect.left; j <= rect.left + rect.width; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(' ');
                }
            }

        }

        public static void Draw(Rect rect, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.CursorVisible = false;
            for (int i = rect.left + 1; i < rect.left + rect.width; i++)
            {
                Console.SetCursorPosition(i, rect.top);
                Console.Write('─');
                Console.SetCursorPosition(i, rect.top + rect.height);
                Console.Write('─');
            }
            for (int i = rect.top + 1; i < rect.top + rect.height; i++)
            {
                Console.SetCursorPosition(rect.left, i);
                Console.Write('│');
                Console.SetCursorPosition(rect.left + rect.width, i);
                Console.Write('│');
            }
            Console.SetCursorPosition(rect.left, rect.top);
            Console.WriteLine('┌');
            Console.SetCursorPosition(rect.left + rect.width, rect.top);
            Console.WriteLine('┐');
            Console.SetCursorPosition(rect.left, rect.top + rect.height);
            Console.WriteLine('└');
            Console.SetCursorPosition(rect.left + rect.width, rect.top + rect.height);
            Console.WriteLine('┘');
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Draw_At_Center(Rect rect, string str = "", ConsoleColor color = ConsoleColor.Gray)
        {
            Draw(rect, color);
            if (str != "")
            {
                Console.SetCursorPosition(rect.left + (rect.left + rect.width - rect.left - str.Length + 1) / 2, rect.top + (rect.top + rect.height - rect.top) / 2);
                Console.ForegroundColor = color;
                Console.Write(str);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void Draw_At_Top(Rect rect, string str = "")
        {
            Draw(rect);
            if (str != "")
            {
                Console.SetCursorPosition(rect.left + 1, rect.top - 1);
                Console.Write(str);
            }
        }

        public static string Draw_Read_Line(Rect rect, string str = "")
        {
            Draw_At_Top(rect, str);
            Console.CursorVisible = true;
            Console.SetCursorPosition(rect.left + 1, rect.top + 1);
            return Console.ReadLine();
        }

        public static bool Draw_Bool(Rect rect, string str = "")
        {
            Draw(rect);
            if (str != "")
            {
                Console.SetCursorPosition(rect.left + (rect.left + rect.width - rect.left - str.Length + 1) / 2, rect.top + 1);
                Console.WriteLine(str);
            }
            int choose = 0;
            Rect tab = new Rect(0, 0, 15, 4);
            while (true)
            {
                Draw_At_Center(new Rect(rect.left + 2, rect.top + rect.height - tab.height - 1, tab.width, tab.height), "Yes", 0 == choose ? ConsoleColor.Yellow : ConsoleColor.Gray);

                Draw_At_Center(new Rect(rect.left + rect.width - tab.width - 2, rect.top + rect.height - tab.height - 1, tab.width, tab.height), "No", 1 == choose ? ConsoleColor.Yellow : ConsoleColor.Gray);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.RightArrow:
                        if (choose < 1) choose++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (choose > 0) choose--;
                        break;
                    case ConsoleKey.Enter:
                        return choose == 0;
                }
            }
        }

        public static int Draw_Choose(Rect rect, IEnumerable<string> category, ConsoleColor color = ConsoleColor.Gray)
        {
            Draw(rect, color);
            int choose = 0;
            Console.CursorVisible = false;
            while (true)
            {
                for (int i = 0; i < category.Count(); i++)
                {
                    string item = category.ElementAt(i);
                    Console.SetCursorPosition(rect.left + 1, rect.top + 1 + i);
                    if (choose == i) Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(item);
                    Console.ResetColor();
                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (choose > 0) choose--;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (choose + 1 < category.Count()) choose++;
                        break;
                    case ConsoleKey.Enter:
                        return choose;
                }
            }
        }

        public static void Draw_List(Rect rect, List<string> category, ConsoleColor color = ConsoleColor.Gray)
        {
            Draw(rect);
            for (int i = 0; i < category.Count(); i++)
            {
                Console.ForegroundColor = color;
                string item = category.ElementAt(i);
                Console.SetCursorPosition(rect.left + 1, rect.top + 1 + i);
                Console.Write(item);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}