using System;
using System.Threading;


namespace Semaphore
{
    public class Semafor
    {
        int slots;
        Mutex mutex;
        ManualResetEvent conditionVar;

        //release la slot
        public void Up() {
            mutex.WaitOne();

            slots++;
            conditionVar.Set();

            Console.WriteLine("UP! number of slots available {0}", slots);

            mutex.ReleaseMutex();
        }
        

        //slot lock
        public void Down() {

            mutex.WaitOne();

            while (slots == 0)
            {
                // pun while pentru ca daca pun if scade continuu
                // motivul e ca wait one face wait many si trezeste thread-urile
                // fiecare thread trebuie sa verifice contidita dupa trezire
                // dau release la mutex pentru ca thread-ul care face up asteapta dupa mutex-ul blocat de mine
                // si eu astept ca el sa dea up ( deadlock )
                mutex.ReleaseMutex();
                conditionVar.WaitOne();
                mutex.WaitOne();
            }

            slots--;
            Console.WriteLine("DOWN! number of slots available {0}", slots);
            mutex.ReleaseMutex();

        }

        public Semafor(int slots = 1)
        {
            this.slots = slots;
            mutex = new Mutex();
            conditionVar = new ManualResetEvent(false);
        }
    }
}
