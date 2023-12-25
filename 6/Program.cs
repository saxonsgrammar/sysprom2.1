using System;
using System.Threading;

namespace _6
{
    class Program
    {
        class Generator
        {
            Thread thread;
            Random random;
            static Semaphore semaphore = new Semaphore(3, 10);
            int count = 3;

            public Generator(int i)
            {
                thread = new Thread(Generate);
                thread.Name = $"Тред №{i + 1}";
                thread.Start();
            }

            public void Generate()
            {
                while (count > 0)
                {
                    semaphore.WaitOne();

                    string line = "";
                    random = new Random();

                    for (int i = 0; i < 6; i++)
                    {
                        line += random.Next(1, 9).ToString() + " ";
                    }

                    Console.WriteLine(Thread.CurrentThread.Name + ": " + line);
                    Thread.Sleep(500);

                    count--;
                    semaphore.Release();
                }
            }
        }

        static void Main(string[] args)
        {
            for (int i = 0; i < 9; i++)
            {
                Generator generator = new Generator(i);
            }
        }
    }
}