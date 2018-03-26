using System;
using System.Threading;
using System.Collections;

namespace PCVC
{
    class Program
    {
        ManualResetEvent can_produce = new ManualResetEvent(false);
        ManualResetEvent can_consume = new ManualResetEvent(false);
        Queue myQ = new Queue();
        Mutex mtx = new Mutex(false);
        int i = 0;


        public void Producer()
        {
            while (true)
            {
                Thread.Sleep(1000);

                mtx.WaitOne();

                while (myQ.Count == 5)
                {
                    mtx.ReleaseMutex();
                    can_produce.WaitOne();
                    mtx.WaitOne();
                }

                Console.WriteLine("Produced an element");
                myQ.Enqueue(i++);
                mtx.ReleaseMutex();
                can_consume.Set();
                
            }
        }

        public void Consumer()
        {
            while (true)
            {
                mtx.WaitOne();

                while (myQ.Count == 0)
                {
                    mtx.ReleaseMutex();
                    can_consume.WaitOne();
                    mtx.WaitOne();
                }

                Console.WriteLine("(Consumed)\t{0}", myQ.Dequeue());
                mtx.ReleaseMutex();
                can_produce.Set();
                

                Thread.Sleep(2000);
            }
        }
    }

    class Test
    {
        static void Main(string[] args)
        {

            Program program = new Program();


            Thread p = new Thread(new ThreadStart(program.Producer));
            Thread c = new Thread(new ThreadStart(program.Consumer));

            p.Start();
            c.Start();

            p.Join();
            c.Join();

        }
    }
}


