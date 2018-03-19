using System;
using System.Threading;
using System.Collections;

namespace Lab1
{
    class Program
    {
        Queue myQ = new Queue();

        Mutex mtx = new Mutex(false);

        int i = 0;

        public void Producer()
        {
            while (true)
            {
                Thread.Sleep(1000);

                mtx.WaitOne();

                while(myQ.Count == 5)
                {
                    Console.WriteLine("Producer Waiting");
                    mtx.ReleaseMutex();

                    Thread.Sleep(10);
                    mtx.WaitOne();
                }

                myQ.Enqueue(i++);
                mtx.ReleaseMutex();
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

                    Thread.Sleep(10);

                    mtx.WaitOne();
                }

                Console.WriteLine("(Consumed)\t{0}", myQ.Dequeue());

                mtx.ReleaseMutex();

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

        }
    }
}
