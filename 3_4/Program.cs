using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace _3_4
{
    class Program
    {
        static string path1 = $"..\\Debug\\1.txt";
        static string path2 = $"..\\Debug\\2.txt";
        static string path3 = $"..\\Debug\\3.txt";
        static string path4 = $"..\\Debug\\4.txt";

        static Mutex mutexobj = new Mutex();

        private class Obj
        {
            public string[] Numbers { get; set; }
            public string Path { get; set; }
        }

        static bool Prime_Detector(int n)
        {
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        static void Main(string[] args)
        {
            Obj obj = new Obj();

            Thread thread = new Thread(Num_Generator);
            thread.Start(obj);
            Thread thread2 = new Thread(Primes);
            thread2.Start(obj);
            Thread thread3 = new Thread(Primes7);
            thread3.Start(obj);
            Thread thread4 = new Thread(Info);
            thread4.Start(obj);
        }

        static void Num_Generator(object obj)
        {
            mutexobj.WaitOne();

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

            mutexobj.ReleaseMutex();
        }

        static void Primes(object obj)
        {
            mutexobj.WaitOne();

            Obj obj1 = obj as Obj;
            string line;
            obj1.Path = path1;

            using (StreamReader reader = new StreamReader(obj1.Path))
            {
                line = reader.ReadToEnd();
            }

            obj1.Numbers = line.Split(' ');
            obj1.Path = path2;

            using (StreamWriter writer = new StreamWriter(obj1.Path, false))
            {
                for (int i = 0; i < obj1.Numbers.Length; i++)
                {
                    if (Prime_Detector(Convert.ToInt32(obj1.Numbers[i])))
                    {
                        writer.Write(obj1.Numbers[i] + " ");
                    }
                }
            }

            mutexobj.ReleaseMutex();
        }

        static void Primes7(object obj)
        {
            mutexobj.WaitOne();

            Obj obj1 = obj as Obj;
            obj1.Path = path1;

            using (StreamReader reader = new StreamReader(obj1.Path))
            {
                string line = reader.ReadToEnd();
                obj1.Numbers = line.Split(' ');
            }

            obj1.Path = path3;

            using (StreamWriter writer = new StreamWriter(obj1.Path, false))
            {
                for (int i = 0; i < obj1.Numbers.Length; i++)
                {
                    if (obj1.Numbers[i].EndsWith("7"))
                    {
                        writer.Write(obj1.Numbers[i] + " ");
                    }

                }
            }

            mutexobj.ReleaseMutex();
        }

        static void Info(object obj)
        {
            mutexobj.WaitOne();

            Obj obj1 = obj as Obj;

            using (FileStream fstream = new FileStream(path4, FileMode.Create))
            {
                int count = 0;
                string line;

                obj1.Path = path1;
                using (StreamReader stream = new StreamReader(obj1.Path, false))
                {
                    line = stream.ReadToEnd();
                    obj1.Numbers = line.Split(' ');
                    count += obj1.Numbers.Length;
                }

                obj1.Path = path2;
                using (StreamReader stream = new StreamReader(obj1.Path, false))
                {
                    line = stream.ReadToEnd();
                    obj1.Numbers = line.Split(' ');
                    count += obj1.Numbers.Length;
                }

                obj1.Path = path3;
                using (StreamReader stream = new StreamReader(obj1.Path, false))
                {
                    line = stream.ReadToEnd();
                    obj1.Numbers = line.Split(' ');
                    count += obj1.Numbers.Length;
                }

                line = $"Количество чисел в файлах: {count}";
                byte[] buffer = Encoding.Default.GetBytes(line);
                fstream.Write(buffer, 0, buffer.Length);

                line = "Размеры файлов в байтах: ";
                buffer = Encoding.Default.GetBytes(line);
                fstream.Write(buffer, 0, buffer.Length);

                obj1.Path = path1;
                byte[] bytes = File.ReadAllBytes(obj1.Path);
                line = $"{bytes.Length}  ";
                buffer = Encoding.Default.GetBytes(line + "\n");
                fstream.Write(buffer, 0, buffer.Length);

                obj1.Path = path2;
                bytes = File.ReadAllBytes(obj1.Path);
                line = $"{bytes.Length}  ";
                buffer = Encoding.Default.GetBytes(line + "\n");
                fstream.Write(buffer, 0, buffer.Length);

                obj1.Path = path3;
                bytes = File.ReadAllBytes(obj1.Path);
                line = $"{bytes.Length}  ";
                buffer = Encoding.Default.GetBytes(line + "\n");
                fstream.Write(buffer, 0, buffer.Length);

                obj1.Path = path1;
                using (StreamReader stream = new StreamReader(obj1.Path, false))
                {
                    line = stream.ReadToEnd();
                    buffer = Encoding.Default.GetBytes($"Содержимое файла{1}: " + line + "\n");
                    fstream.Write(buffer, 0, buffer.Length);
                }

                obj1.Path = path2;
                using (StreamReader stream = new StreamReader(obj1.Path, false))
                {
                    line = stream.ReadToEnd();
                    buffer = Encoding.Default.GetBytes($"Содержимое файла{2}: " + line + "\n");
                    fstream.Write(buffer, 0, buffer.Length);
                }

                obj1.Path = path3;
                using (StreamReader stream = new StreamReader(obj1.Path, false))
                {
                    line = stream.ReadToEnd();
                    buffer = Encoding.Default.GetBytes($"Содержимое файла{3}: " + line + "\n");
                    fstream.Write(buffer, 0, buffer.Length);
                }
            }

            mutexobj.ReleaseMutex();
        }                
    }
}