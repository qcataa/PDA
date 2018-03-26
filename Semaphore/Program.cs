using System;
using System.Threading;

namespace Semaphore
{
    class Program
    {
        static Semafor semafor;
        
        public void doSomething()
        {
            semafor.Down();

            //face ceva
            Thread.Sleep(1000);



            semafor.Up();
        } 



        static void Main(string[] args)
        {
            Program prog = new Program();
            semafor = new Semafor(3);
            Thread[] p = new Thread[10];

            for(int i=0; i <p.Length; i++)
            {
                p[i] = new Thread(new ThreadStart(prog.doSomething));
                p[i].Start();
            }
            foreach(var thread in p)
            {
                thread.Join();
            }
            Console.ReadLine();
        }
    }
}
