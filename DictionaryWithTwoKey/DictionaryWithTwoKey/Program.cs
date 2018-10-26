using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryWithTwoKey
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleTest();

            UserTypeTest();

            Console.ReadKey();
        }

        public static void UserTypeTest()
        {
            var dic = new MyDictionary<int, UserType, string>();

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

            Console.WriteLine($"проверяем есть ли ключ {name3} в колекции");
            Console.WriteLine(dic.ContainsKey(name3));
            Console.WriteLine("========================================");


        }

        public static void SimpleTest()
        {
            var dic = new MyDictionary<int, string, int>
            {
                [1, "Name1"] = 1,
                [2, "Name1"] = 2,
                [3, "Name1"] = 3,
                [5, "Name1"] = 5,
                [2, "Name2"] = 22,
                [2, "Name3"] = 23
            };


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
