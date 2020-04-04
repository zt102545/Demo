using System;

namespace Redis
{
    class Program
    {
        static RedisHelper redis = RedisHelper.Current;

        static void Main(string[] args)
        {
            Test();
            Console.ReadKey();
        }

        public async static void Test()
        {
            var result = await redis.Get("test");
            Console.WriteLine(result);

        }
    }
}
