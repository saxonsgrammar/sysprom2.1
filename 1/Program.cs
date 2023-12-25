using System;
using System.IO;
using System.Text;
using System.Threading;

namespace _1
{
    class Program
    {
        static AutoResetEvent waitHandler = new AutoResetEvent(true);
        static string path1 = $"..\\Debug\\1.txt";
        static string path2 = $"..\\Debug\\2.txt";
        static string path3 = $"..\\Debug\\3.txt";

        class Obj
        {
            public string[] Numbers { get; set; }
            public string Path { get; set; }
        }

        static void Main(string[] args)
        {
            Obj obj = new Obj();

            Thread thread1 = new Thread(Generate);
            thread1.Start(obj);
            Thread thread2 = new Thread(Sum);
            thread2.Start(obj);
            Thread thread3 = new Thread(Mul);
            thread3.Start(obj);
        }

        static void Generate(object obj)
        {
            waitHandler.WaitOne();

            Obj obj1 = obj as Obj;
            obj1.Path = path1;
            Random random = new Random();
            string line = "";

            for (int i = 0; i < 10; i++)
            {
                line += random.Next(1, 9).ToString() + " ";
            }

            using (FileStream fstream = new FileStream(obj1.Path, FileMode.Create))
            {
                byte[] buffer = Encoding.Default.GetBytes(line);
                fstream.Write(buffer, 0, buffer.Length);
            }

            waitHandler.Set();
        }

        static void Sum(object obj)
        {
            waitHandler.WaitOne();

            Obj obj1 = obj as Obj;
            obj1.Path = path1;

            using (StreamReader reader = new StreamReader(obj1.Path))
            {
                string line = reader.ReadToEnd();
                char[] space = { ' ' };
                obj1.Numbers = line.Split(space);
            }

            obj1.Path = path2;

            using (StreamWriter writer = new StreamWriter(obj1.Path, false))
            {
                for (int i = 0; i < obj1.Numbers.Length / 2; i++)
                {
                    writer.Write(Convert.ToInt32(obj1.Numbers[2 * i]) + Convert.ToInt32(obj1.Numbers[(2 * i) + 1]) + " ");

                }
            }

            waitHandler.Set();
        }

        static void Mul(object obj)
        {
            waitHandler.WaitOne();

            Obj obj1 = obj as Obj;
            obj1.Path = path1;

            using (StreamReader reader = new StreamReader(obj1.Path))
            {
                string line = reader.ReadToEnd();
                char[] space = { ' ' };
                obj1.Numbers = line.Split(space);
            }

            obj1.Path = path3;

            using (StreamWriter writer = new StreamWriter(obj1.Path, false))
            {
                for (int i = 0; i < obj1.Numbers.Length / 2; i++)
                {
                    writer.Write(Convert.ToInt32(obj1.Numbers[2 * i]) * Convert.ToInt32(obj1.Numbers[(2 * i) + 1]) + " ");
                }

            }

            waitHandler.Set();
        }
    }
}