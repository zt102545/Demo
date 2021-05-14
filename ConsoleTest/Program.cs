using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {

        static void Main(string[] args)
        {
            #region IEnumerable测试
            //int count = 0;
            //eJobExperience[] JobExperience1 =
            //{
            //    new eJobExperience { BeginDate=DateTime.Now,EndDate=DateTime.Now.AddDays(31) }
            //};
            //Test T = new Test();
            //int sum = T.A(JobExperience1, out count);
            //Console.WriteLine(sum + "；" + count);
            //Console.ReadLine();
            #endregion

            #region try catch测试
            //Console.WriteLine("1");
            //try
            //{
            //    Console.WriteLine("2");
            //    throw new ArgumentException();
            //}
            //catch (NotImplementedException)
            //{
            //    Console.WriteLine("3");
            //}
            //catch (ArgumentException)
            //{
            //    Console.WriteLine("4");
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine("5");
            //    throw new NotImplementedException();
            //}
            //finally
            //{
            //    Console.WriteLine("6");
            //    throw new Exception();
            //}
            //Console.WriteLine("7");
            //Console.Read();
            #endregion

            #region 虚方法重写测试
            //A b = new B();
            //A c = new C();
            //A d = new D();
            //A e = new E();
            //b.Func1();
            //c.Func1();
            //d.Func1();
            //b.Func2();
            //c.Func2();
            //d.Func2();
            //e.Func2();
            ////总结：记住A b = new B();的类型是A，就不难理解了。
            ////1、子类使用new隐藏父类方法后，是独立于父类的方法。
            ////2、子类使用override重写父类的方法后，只是在调用时不再调用父类方法而调用子类方法。
            ////3、子类也可以使用new隐藏父类的虚方法。
            #endregion

            #region 并行测试
            //object locks = new object();
            //int sum = 0;
            //ParallelLoopResult result = Parallel.For(0, 10, i =>
            //{
            //    lock (locks)//锁住后是并行变成串行了
            //    {
            //        sum += CalcAsync(i);
            //        Console.WriteLine(i);
            //    }
            //});
            //Console.WriteLine(sum);
            //Console.Read();
            #endregion

            #region 静态构造函数测试
            //Sc o1 = new Sc();
            //Sc o2 = new Sc();
            //Console.WriteLine(Sc.Count);
            #endregion

            #region ReferenceEquals测试
            //object i = 5;
            //int j = 5;
            //if (object.Equals(i, j))
            //    Console.WriteLine("Equal");
            //else
            //    Console.WriteLine("Not Euqal");

            //string f = "1";
            //string k = "5";
            //string l = k;
            //if (object.ReferenceEquals(k + f, l + f))
            //    Console.WriteLine("Equal");
            //else
            //    Console.WriteLine("Not Euqal");

            ////总结：
            ////1."=="：值类型时比较值是否相等，引用类型时比较两对象的引用是否相等。（string就重载==，使其比较的不是两个字符串的引用，而是比较的两个字符串字面量是否相等）。
            ////2.Equals：值类型时类型相同（不会进行类型自动转换），并且数值相同(对于struct的每个成员都必须相同)。引用类型时比较两个对象的引用是否相等。(string同上)
            ////3.ReferenceEquals：无论值类型还是引用类型都比较两对象的引用是否相等。
            #endregion

            #region IEnumerable和list性能测试
            //EnumberableTest et = new EnumberableTest();
            //et.Goo();
            //网上结论：
            //按照功能排序：List<T> > IList<T> > ICollection<T> > IEnumerable<T>
            //按照性能排序：IEnumerable<T> > ICollection<T> > IList<T> > List<T>(自己测试结果相反，可能测试有问题吧)
            #endregion

            #region ThreadAbortException
            //ThreadStart start = new ThreadStart(M);
            //Thread thread = new Thread(start);
            //thread.Start();
            //Thread.Sleep(1000);
            //thread.Abort();
            #endregion

            #region string类型赋值测试
            //string s1 = "qwe";
            //string s2 = s1;
            //s1 = "ewq";
            //Console.WriteLine(s1);
            //Console.WriteLine(s2);
            //Console.WriteLine($"{s1}+{s2}");
            //Console.WriteLine(2 + 6 + "8");

            //string mes = "taobao";
            //string a = "tao" + "bao";
            //string b = "tao";
            //string c = "bao";
            //Console.WriteLine(mes == a);
            //Console.WriteLine((b + c) == mes);
            //Console.WriteLine(ReferenceEquals(mes, a));
            //Console.WriteLine(ReferenceEquals((b + c), mes));
            #endregion

            #region ParameterizedThreadStart
            //Thread th = new Thread(new ParameterizedThreadStart(BBB));
            //th.Start(5);
            #endregion

            #region 静态构造函数
            //Console.WriteLine(Z.strText);
            //Console.WriteLine(T.strText);
            //Console.WriteLine(T.x);
            //Console.WriteLine(T.strText);
            ////结论：子类直接调用父类静态成员时不会执行自身的构造函数。
            #endregion

            #region 递归函数
            //Console.WriteLine("输入位数：");
            //int num = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine("计算结果：" + GetNum(num));
            #endregion

            #region 冒泡排序 
            //Console.WriteLine("输入一组数组，逗号分隔");
            //string str = Console.ReadLine();
            //string[] list = str.Split(',');
            //for (int i = 0; i < list.Length; i++)
            //{
            //    for (int j = i; j < list.Length; j++)
            //    {
            //        if (Convert.ToInt32(list[i]) > Convert.ToInt32(list[j]))
            //        {
            //            string t = list[i];
            //            list[i] = list[j];
            //            list[j] = t;
            //        }
            //    }
            //}
            //Console.WriteLine("排序结果：");
            //Console.WriteLine(string.Join(",", list));
            #endregion

            #region Encoding.Default.GetBytes("")
            //string stringstrTmp = "abcdefg某某某";
            ////Encoding.Default.GetBytes(stringstrTmp)将字符串转为字节数组，UTF-8编码的中文字符占三个字节，GetBytes是将字符串的每个字符转成ascii码后返回byte数组
            //int i = Encoding.Default.GetBytes(stringstrTmp).Length;
            //Console.WriteLine(i);
            #endregion

            #region async和await
            //Console.WriteLine("step1,线程ID:{0}", Thread.CurrentThread.ManagedThreadId);

            //AsyncDemo demo = new AsyncDemo();
            //demo.AsyncSleep().Wait();//Wait会阻塞当前线程直到AsyncSleep返回
            ////demo.AsyncSleep();//不会阻塞当前线程

            //Console.WriteLine("step5,线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
            #endregion

            #region Task
            //var t1 = new Task(() => TaskMethod("Task 1"));
            //var t2 = new Task(() => TaskMethod("Task 2"));
            //t2.Start();
            //t1.Start();
            //Task.WaitAll(t1, t2);
            //Task.Run(() => TaskMethod("Task 3"));
            //Task.Factory.StartNew(() => TaskMethod("Task 4"));
            ////标记为长时间运行任务,则任务不会使用线程池,而在单独的线程中运行。
            //Task.Factory.StartNew(() => TaskMethod("Task 5"), TaskCreationOptions.LongRunning);

            //#region 常规的使用方式
            //Console.WriteLine("主线程执行业务处理.");
            ////创建任务
            //Task task = new Task(() =>
            //{
            //    Console.WriteLine("使用System.Threading.Tasks.Task执行异步操作.");
            //    for (int i = 0; i < 10; i++)
            //    {
            //        Console.WriteLine(i);
            //    }
            //});
            ////启动任务,并安排到当前任务队列线程中执行任务(System.Threading.Tasks.TaskScheduler)
            //task.Start();
            //Console.WriteLine("主线程执行其他处理");
            //task.Wait();
            //#endregion
            //Thread.Sleep(TimeSpan.FromSeconds(1));
            #endregion

            #region async/await实现方式
            //Console.WriteLine("主线程执行业务处理.");
            //AsyncFunction();
            //Console.WriteLine("主线程执行其他处理");
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine(string.Format("Main:i={0}", i));
            //}
            #endregion

            #region Convert.ToInt32
            //对于x.5这种数，使用Convert.ToInt32转换时，返回的是其两个边界整数中为偶数的那个
            //double a = 19.5;
            //double b = 20.5;
            //while (true)
            //{
            //    double c = Convert.ToDouble(Console.ReadLine());
            //    Console.WriteLine(Convert.ToInt32(c));
            //}
            #endregion

            #region 通过反射能获取类的哪些类型成员
            //Type t = typeof(Person);

            ////GetMembers()默认返回公共成员 包括父类的成员
            //MemberInfo[] minfos = t.GetMembers();
            //foreach (MemberInfo m in minfos)
            //{
            //    Console.WriteLine(m.Name);
            //}
            //Console.WriteLine("GetMembers不带参数时只能获取得到共有成员");
            //Console.WriteLine("GetMembers带参数：");
            ////包含非公共成员 包含实例成员 包含公共成员
            //MemberInfo[] minfos2 = t.GetMembers(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            //foreach (MemberInfo m in minfos2)
            //{
            //    Console.WriteLine(m.Name);
            //}
            //Console.WriteLine("GetMembers带参数时能指定获取哪些访问级别的成员");
            ////结论:都能获取的到
            #endregion

            #region finally在return中间返回，断点调试 
            //int i = 1;
            //int s = finallyTest(ref i);
            //Console.WriteLine("ref的结果：" + i);
            //Console.WriteLine("返回的结果：" + s);
            #endregion

            #region 创建一个int数组，长度为100，并向其中随机插入1-100，并不重复。
            //var list = new List<int>();
            //var newlist = new int[100];
            //for (int i = 0; i < 100; i++)
            //{
            //    list.Add(i + 1);
            //}

            //Random rm = new Random();
            //for (int j = 0; j < newlist.Length; j++)
            //{
            //    int index = rm.Next(0, list.Count);
            //    newlist[j] = list[index];
            //    list.RemoveAt(index);
            //    Console.WriteLine(newlist[j]);
            //}
            #endregion

            #region 把一个Array复制到ArrayList里
            //string[] array = new string[] { "1", "2", "3", "4" };

            ////方法一
            //Console.WriteLine("方法一：for循环");
            //ArrayList list1 = new ArrayList();
            //for (int i = 0; i < array.Length; i++)
            //{
            //    list1.Add(array[i]);
            //}
            //Console.WriteLine(string.Join(",", (string[])list1.ToArray(typeof(string))));

            ////方法二
            //Console.WriteLine("方法二：使用ArrayList的Adapter()方法");
            //ArrayList list2 = ArrayList.Adapter(array);
            //Console.WriteLine(string.Join(",", (string[])list2.ToArray(typeof(string))));

            ////方法三
            //Console.WriteLine("方法三：直接使用构造方法传入，因为Array实现了ICollection：");
            //ArrayList list3 = new ArrayList(array);
            //Console.WriteLine(string.Join(",", (string[])list3.ToArray(typeof(string))));
            #endregion

            #region 字符串单词倒序
            //string str = "I  am a good man";
            //string[] list = str.Split(' ');
            //string newStr = null;
            //foreach (string item in list)
            //{
            //    newStr = item + " " + newStr;
            //}
            //Console.WriteLine(newStr);
            #endregion

            #region 线程安全问题
            //ThreadA t = new ThreadA();
            //ThreadA.obj.i = 10;

            //Thread[] threads = new Thread[2];
            //for (int i = 0; i < 2; i++)
            //{
            //    Thread th = new Thread(new ThreadStart(t.hhh));
            //    threads[i] = th;
            //}

            //for (int j = 0; j < 2; j++)
            //{
            //    threads[j].Start();
            //}
            #endregion

            #region 线程调用问题
            //    //线程调用带参数的方法不能用ThreadStart()
            //    Thread th = new Thread(new ThreadStart(ThreadC));
            //    th.Start();

            //    #region 改进，两种方法，推荐lambda表达式
            //Thread th = new Thread(() => AAA(5));
            //th.Start();
            //    Thread th = new Thread(new ParameterizedThreadStart(ThreadC));
            //    th.Start(5);
            //    #endregion
            #endregion

            #region B继承A，C继承B，怎样是A的方法B能调用而C不能
            //JCC c = new JCC();
            //c.test();
            #endregion

            #region float double decimal的区别
            //float:单精度，32位，4字节的浮点数，有效位数7位
            //double:双精度，64位，8字节的浮点数，有效位数16位
            //decimal:高精度，128位，16字节的浮点数，有效数字28位
            double s = 12345.123456789012345678901234567890123;
            s = s + 1;
            Console.WriteLine(s);
            #endregion

            Console.Read();
        }

        #region ParameterizedThreadStart
        static void BBB(object o)
        {
            Console.WriteLine($"{o}+7");
        }
        #endregion

        #region 并行测试
        public static int CalcAsync(int i)
        {
            Thread.Sleep(1000);
            return i;
        }
        #endregion

        #region ThreadAbortException
        static void M()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadAbortException)
            {
                Thread.Sleep(1000);
                Console.WriteLine("ThreadAbortException");
            }
            catch (Exception)
            {
                Console.WriteLine("Exception");
            }
            catch
            {
                Console.WriteLine("catch");
            }
            finally
            {
                Console.WriteLine("finally");
            }
        }
        #endregion

        #region 1、1、2、3、5、8、13、21、34...... 求第30位数是多少，用递归算法实现
        public static int GetNum(int i)
        {
            if (i == 1 || i == 2)
            {
                return 1;
            }
            else
            {
                return GetNum(i - 1) + GetNum(i - 2);
            }
        }
        #endregion

        #region Task
        static void TaskMethod(string name)
        {
            Console.WriteLine("Task {0} is running on a thread id {1}. Is thread pool thread: {2}",
                name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
        }
        #endregion

        #region async/await实现方式
        async static void AsyncFunction()
        {
            await Task.Delay(1);
            Console.WriteLine("使用System.Threading.Tasks.Task执行异步操作.");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(string.Format("AsyncFunction:i={0}", i));
            }
        }
        #endregion

        #region finally在return中间返回，断点调试 
        static int finallyTest(ref int i)
        {
            try
            {
                Console.WriteLine("try里面的i : " + i);
                return i;
            }
            finally
            {
                Console.WriteLine("进入finally...");
                i++;
                Console.WriteLine("finally里面的i : " + i);
            }
        }
        #endregion

        #region 线程调用问题
        static void ThreadC(object o)
        {
            Console.WriteLine($"{0}+7");
        }
        #endregion

    }

    #region IEnumerable测试
    public class eJobExperience
    {
        public DateTime BeginDate;
        public DateTime EndDate;
    }

    public class Test
    {
        public int A(IEnumerable<eJobExperience> JobExperience, out int workNumber)
        {
            workNumber = JobExperience.Count();
            return JobExperience.Sum(e => (e.EndDate - e.BeginDate).Days / 30);
        }
    }
    #endregion

    #region 接口测试
    public interface IBese
    {
        int func();
    }

    public class Base1
    {
        public int func() { return 0; }
    }

    public class myClass : Base1, IBese
    {
    }
    #endregion

    #region 虚方法重写测试
    public abstract class A
    {
        public virtual void Func1() { Console.Out.WriteLine("A1"); }
        public virtual void Func2() { Console.Out.WriteLine("A2"); }
    }
    public class B : A
    {
        public override void Func1() { Console.Out.WriteLine("B1"); }
        public new void Func2() { Console.Out.WriteLine("B2"); }
    }
    public class C : A
    {
        public override void Func1() { Console.Out.WriteLine("C1"); }
        public new virtual void Func2() { Console.Out.WriteLine("C2"); }
    }
    public class D : B
    {
        public override void Func1() { Console.Out.WriteLine("D1"); }
        public new void Func2() { Console.Out.WriteLine("D2"); }
    }
    public class E : A
    {
        public override void Func1() { Console.Out.WriteLine("E1"); }
        public new void Func2() { base.Func1(); }
    }
    #endregion

    #region 静态构造函数测试
    public class Sc
    {
        public static int Count = 0;
        static Sc()
        {
            Count++;
        }
        public Sc()
        {
            Count++;
        }

    }
    #endregion

    #region IEnumerable和list性能测试
    public class EnumberableTest
    {
        private void Foo1(IEnumerable<string> sList)
        {
            foreach (var s in sList)
            {

            }
        }

        private void Foo2(List<string> sList)
        {
            foreach (var s in sList)
            {

            }
        }


        public void Goo()
        {
            var list = new List<string>();
            for (int i = 0; i < 10000000; i++)
            {
                list.Add("1");
            }


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //  开始监视代码运行时间  
            //Foo1(list);
            Foo2(list);
            stopwatch.Stop(); //  停止监视 
            TimeSpan timespan = stopwatch.Elapsed;
            double milliseconds = timespan.TotalMilliseconds;
            Console.WriteLine(milliseconds);

        }
    }
    #endregion

    #region 静态构造函数
    public class Z
    {
        public static string strText;
        static Z()
        {
            strText = "zzz";
        }
    }

    public class T : Z
    {
        public static string x;
        static T()
        {
            x = "22";
            strText = "ttt";
        }
    }
    #endregion

    #region async和await
    public class AsyncDemo
    {

        public async Task AsyncSleep()
        {
            Console.WriteLine("step2,线程ID:{0}", Thread.CurrentThread.ManagedThreadId);

            //await关键字表示等待Task.Run传入的逻辑执行完毕，此时(等待时)AsyncSleep的调用方能继续往下执行
            //Task.Run将开辟一个新线程执行指定逻辑
            await Task.Run(() => Sleep(2));

            Console.WriteLine("step4,线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        private void Sleep(int second)
        {
            Console.WriteLine("step3,线程ID:{0}", Thread.CurrentThread.ManagedThreadId);

            Thread.Sleep(second * 1000);
        }

    }
    #endregion

    #region 通过反射能获取类的哪些类型成员
    /// <summary>
    /// 结论:都能获取的到
    /// </summary>
    public class Person
    {
        private int _private;
        private int _private_s { get; set; }

        protected int _protected;
        protected int _protected_s { get; set; }

        public int _public;
        public int _public_s { get; set; }

        internal int _internal;
        internal int _internal_s { get; set; }

        private void PrivateFuc() { }
        protected void ProtectedFuc() { }
        public void PublicFuc() { }
        internal void InternalFuc() { }


        private static void PrivateStatic() { }
        protected static void ProtectedStatic() { }
        public static void PublicStatic() { }
        internal static void InternalStatic() { }
    }
    #endregion

    #region 线程安全问题
    class ThreadA
    {
        public static ThreadB obj = new ThreadB();
        //不加锁会有线程安全问题
        public static object Lock = new object();
        public void hhh()
        {
            lock (Lock)
            {
                for (int m = 0; m < 7; m++)
                {
                    if (obj.i > 0)
                    {
                        obj.i--;
                        Console.WriteLine("ojb.i=" + obj.i + "---当前线程：" + Thread.CurrentThread.ManagedThreadId.ToString());
                    }
                }
            }
        }
    }

    class ThreadB
    {
        public int i;
    }
    #endregion

    #region B继承A，C继承B，怎样是A的方法B能调用而C不能
    public class JCA
    {
        public virtual void test()
        {
            Console.WriteLine("AAAA");
        }

    }

    public class JCB : JCA
    {
        /// <summary>
        /// 重载A的方法，这样C调用的时候只能调到B的重载方法
        /// </summary>
        public override void test()
        {
            Console.WriteLine("BBBB");
        }

    }

    public class JCC : JCB
    {
    }
    #endregion 

    #region 单例模式

    #region 懒汉式：单例类的实例在第一次被引用时候才被初始化。线程不安全，需要加锁。
    public class LSingleton
    {
        private static LSingleton _LSingleton = null;
        private static object Lock = new object();
        private LSingleton()
        { }

        public static LSingleton CreateInstance()
        {
            if (_LSingleton == null)
            {
                lock (Lock)
                {
                    if (_LSingleton == null)
                        _LSingleton = new LSingleton();
                }
            }
            return _LSingleton;
        }
    }
    #endregion

    #region 饿汉式：单例类的实例在加载的时候就被初始化。线程安全
    public class ESingleton
    {
        private static ESingleton _ESingletonSecond = null;

        private ESingleton()
        { }
        static ESingleton()
        {
            _ESingletonSecond = new ESingleton();
        }

        public static ESingleton CreateInstance()
        {
            return _ESingletonSecond;
        }
    }
    #endregion
    #endregion

}
