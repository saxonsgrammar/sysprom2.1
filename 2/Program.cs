using System;
using System.IO;
using System.Threading;

namespace _2
{
    class Program
    {
        public static object locker = new object();

        static void Main(string[] args)
        {
            string path = $"..\\Debug\\2.txt";

            Thread thread1 = new Thread(Count);
            thread1.Start(path);
            Thread thread2 = new Thread(Replace);
            thread2.Start(path);
        }

        static void Count(object path)
        {
            bool acquiredLock = false;

            try
            {
                Monitor.Enter(locker, ref acquiredLock);

                using (StreamReader stream = new StreamReader(path as string))
                {
                    string line = stream.ReadToEnd();
                    Console.WriteLine($"Количество элементов в файле: {line.Length}");
                }
            }
            finally
            {
                if (acquiredLock) Monitor.Exit(locker);
            }
        }

        static void Replace(object path)
        {
            string line;
            bool acquiredLock = false;

            try
            {
                Monitor.Enter(locker, ref acquiredLock);

                using (StreamReader stream = new StreamReader(path as string))
                {
                    line = stream.ReadToEnd();
                };

                using (StreamWriter writer = new StreamWriter(path as string))
                {
                    line = line.Replace('#', '!');
                    writer.Write(line);
                }
            }
            finally
            {
                if (acquiredLock) Monitor.Exit(locker);
            }
        }
    }
}