using System;

namespace ConsoleLab
{
    internal class Apple
    {
        internal void GetTypeParameter<T>() where T : class
        {
            Type c = typeof(T);
            Console.WriteLine(c.ToString());
        }

        internal bool IsType<T>(object o) where T : class
        {
            return o.GetType().IsSubclassOf(typeof(T));
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            RedApple redApple = new RedApple();
            Apple apple = new Apple();

            Apple a = redApple;
            RedApple ra = (RedApple)apple;
            Console.WriteLine(a);
            Console.WriteLine(ra);
        }
    }

    internal class RedApple : Apple
    {
    }
}