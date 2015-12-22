using System;
using System.Collections.Generic;
using AxisEngine;

namespace DoubleKeyMapTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DoubleKeyMap<string, int> map = new DoubleKeyMap<string, int>();
            map.Set("Cameron", "Taylor", 21);
            map.Set("Anita", "Savell", 20);
            map.Set("Jule", "Taylor", 49);
            map.Set("Brian", "Taylor", 57);
            Console.WriteLine("Cameron Taylor -> " + map.Get("Cameron", "Taylor"));
            Console.WriteLine("Taylor Cameron -> " + map.Get("Taylor", "Cameron"));

            map.Set("Zachary", "Taylor", 19);
            Console.WriteLine("Zachary Taylor -> " + map.Get("Zachary", "Taylor"));
            Console.WriteLine("Taylor Zachary -> " + map.Get("Taylor", "Zachary"));

            try
            {
                Console.WriteLine("Cameron Zachary -> " + map.Get("Cameron", "Zachary"));
            }
            catch(KeyNotFoundException)
            {
                Console.WriteLine("Cameron Zachary -> " + "Key not found!");
            }

            Console.WriteLine("Contents:");
            foreach(Tuple<string, string> entry in map.Keys)
            {
                Console.WriteLine("\t- " + entry.ToString() + " -> " + map.Get(entry));
            }
            Console.WriteLine("Paired with 'Taylor'");
            foreach(string s in map.GetPairedKeys("Taylor"))
            {
                Console.WriteLine("\t- " + s);
            }
            Console.WriteLine();

            Console.WriteLine("Removing 'Taylor'");
            map.RemoveKey("Taylor");
            try
            {
                Console.WriteLine("Cameron Taylor -> ", map.Get("Cameron", "Taylor"));
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Cameron Taylor -> Key not found!");
            }
            try
            {
                Console.WriteLine("Zachary Taylor -> ", map.Get("Zachary", "Taylor"));
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Zachary Tayor -> Key not found!");
            }


            Console.ReadKey();
        }
    }
}
