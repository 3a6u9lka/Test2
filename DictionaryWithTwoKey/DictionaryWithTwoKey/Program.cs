using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace DictionaryWithTwoKey
{
    class Program
    {
        static void Main(string[] args)
        {
            var dicV1 = new MyDictionaryV1<int, string, int>();
            var dicV2 = new MyDictionaryV2<int, string, int>();
            var dicV3 = new MyDictionaryV3<int, string, int>();

            SimpleTest(dicV1);
            SimpleTest(dicV2);
            SimpleTest(dicV3);


            var dicV11 = new MyDictionaryV1<int, UserType, string>();
            var dicV12 = new MyDictionaryV2<int, UserType, string>();
            var dicV13 = new MyDictionaryV3<int, UserType, string>();

            UserTypeTest(dicV11);
            UserTypeTest(dicV12);
            UserTypeTest(dicV13);

            dicV11.Clear();
            dicV12.Clear();
            dicV13.Clear();

            SpeedTest(dicV11);
            SpeedTest(dicV12);
            SpeedTest(dicV13);


            Console.WriteLine("========================================");

            var dic = new MyConcurrentDictionaryV3<int, UserType, string>();

            var tt = new Thread(AddItem);
            tt.Start(dic);
            //AddItem(dic, 100000);
            //tt.Join();

            var t = 0;
            foreach (var value in dic.GetValues())
            {
                t++;
            }
            Console.WriteLine("колличество значений: " + t);

            tt.Join();
            t = 0;
            foreach (var value in dic.GetValues())
            {
                t++;
            }
            Console.WriteLine("колличество значений: " + t);
            Console.ReadKey();
        }

        public static void AddItem(object dic)
        {
            if (dic is MyConcurrentDictionaryV3<int, UserType, string>)
                AddItem((MyConcurrentDictionaryV3<int, UserType, string>)dic);
        }

        public static void AddItem(MyConcurrentDictionaryV3<int, UserType, string> dic, int start = 0)
        {
            var count = 100000 + start;
            for (int i = start; i < count; i++)
            {
                var userType = new UserType {Age = i / 10, Name = $"Name{i / 500}"};
                dic[i % 10, userType] = i.ToString();
            }
        }

        public static void SpeedTest(MyDictionary<int, UserType, string> dic)
        {
            Console.WriteLine(dic.GetType().Name);
            var timer = System.Diagnostics.Stopwatch.StartNew();

            var keys1 = new List<int>();
            var keys2 = new List<UserType>();
            var count = 100000;
            timer.Start();

            for (int i = 0; i < count; i++)
            {
                var userType = new UserType {Age = i / 10, Name = $"Name{i / 500}"};
                keys1.Add(i % 10);
                keys2.Add(userType);
                dic[i % 10, userType] = i.ToString();
            }
            timer.Stop();
            keys1 = keys1.Distinct().ToList();
            keys2 = keys2.Distinct().ToList();
            Console.WriteLine($"Время добавления {count} элементов: {timer.Elapsed:g}");

            timer.Start();
            var t = 0;
            foreach (var value in dic.GetValues())
            {
                t++;
            }
            timer.Stop();
            Console.WriteLine("Время вывода всех значений: " + timer.Elapsed.ToString("g"));
            Console.WriteLine("колличество значений: " + t);

            timer.Start();
            t = 0;

            foreach (var key1 in keys1)
            {
                t += dic.GetValues(key1).Count();
            }
            timer.Stop();
            Console.WriteLine("Время поиска по первому ключу: " + timer.Elapsed.ToString("g"));
            Console.WriteLine("колличество значений: " + t);

            timer.Start();
            t = 0;

            foreach (var key2 in keys2)
            {
                t += dic.GetValues(key2).Count();
            }
            timer.Stop();
            Console.WriteLine("Время поиска по второму ключу: "+timer.Elapsed.ToString("g"));
            Console.WriteLine("колличество значений: " + t);
            Console.WriteLine("========================================");

            //dic.Printe();
        }

        public static void UserTypeTest(MyDictionary<int, UserType, string> dic)
        {
            var name1 = new UserType {Name = "Name1", Age = 31};
            var name2 = new UserType {Name = "Name2"};
            var name3 = new UserType {Name = "Name3", Age = 33 };

            dic[1, name1] = "1, Name1";
            dic[2, name1] = "2, Name1";
            dic[3, name2] = "3, Name2";
            dic[1, name2] = "1, Name2";

            dic.Printe();
            Console.WriteLine("========================================");

            Console.WriteLine("обновляем первый элемент");
            dic[1, new UserType {Name = "Name1", Age = 31 }] = "UPDATE 1, Name1";
            dic.Printe();
            Console.WriteLine("========================================");
        }

        public static void SimpleTest(MyDictionary<int, string, int> dic)
        {
            dic[1, "Name1"] = 1;
            dic[2, "Name1"] = 2;
            dic[3, "Name1"] = 3;
            dic[5, "Name1"] = 5;
            dic[2, "Name2"] = 22;
            dic[2, "Name3"] = 23;

            dic.Printe();
            Console.WriteLine("========================================");
            Console.WriteLine("значение по первому ключу = 2");
            foreach (var value in dic.GetValues(2))
            {
                Console.WriteLine(value);
            }

            Console.WriteLine("========================================");
            Console.WriteLine("значение по второму ключу = Name1");
            foreach (var value in dic.GetValues("Name1"))
            {
                Console.WriteLine(value);
            }
            Console.WriteLine("========================================");
            Console.WriteLine("Обновление и удаление");

            dic.Remove(5, "Name1");
            dic.Remove(6, "Name1");
            dic[1, "Name1"] = 10001;

            dic.Printe();
            Console.WriteLine("========================================");
        }
    }
}
