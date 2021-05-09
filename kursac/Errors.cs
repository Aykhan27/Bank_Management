using System;
using System.Collections.Generic;

namespace kursac
{
    class Errors
    {
        static List<string> str_error = new List<string>();

        public static Rect rect = new Rect();
    
        public static void Add(string error)
        {
            str_error.Add(error);
        }

        public static void Draw()
        {
            if (str_error.Count > 0)
            {            
                Window.Draw_List(rect.ChangeHeight(str_error.Count - 1),str_error, ConsoleColor.Red);
                str_error.Clear();
            }
        }

        public static void Draw(string error , ConsoleColor color = ConsoleColor.Red)
        {  
            Window.Draw_List(rect, new List<string> {error}, color);           
        }
    }
}