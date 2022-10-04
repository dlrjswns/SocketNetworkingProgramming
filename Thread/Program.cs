using System;
using System.Threading;

namespace Thread_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(threadFunc);
            t.IsBackground = true; // 
            t.Start();
            // 메인스레드가 이외의 스레드보다 먼저 죽는 상황은 바람직하지않음
            // 위와 같은 경우를 위한 코드, IsBackground, Join
            t.Join();

            ThreadParam param = new ThreadParam();
            param.value1 = 10;
            param.calue2 = 20;
            t.Start(param);
        }

        static void threadFunc()
        {
            Console.WriteLine("60초 후에 프로그램 종료");
            Thread.Sleep(1000 * 60);
            Console.WriteLine("스레드 종료!");
        }
        // t.Start(10) 이런식으로 전달하여 threadFunc로 전달 가능 이때 인자의 타입은 반드시 object이므로 
        // 추후에 다시 타입캐스팅해줘야함
        static void threadFunc(object initialValue)
        {
            // int intValue = (int)initialValue;
            // Console.WriteLine(intValue);


            ThreadParam intValue = (ThreadParam)initialValue;
            Console.WriteLine("{0}, {1}", intValue.value1, intValue.calue2);
        }

        class ThreadParam
        {
            public int value1;
            public int calue2;
        }

        class ThreadWithParam
        {
            public int value1;
            public int value2;
            public ThreadWithParam(int val1, int val2)
            {
                this.value1 = val1;
                this.value2 = val2;
            }

            static void threadFunc()
            {
                Console.WriteLine("");
            }
        }

    }
}
