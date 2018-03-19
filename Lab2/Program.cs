using System;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        private static SemaphoreSlim semaphore;

        public static void Main()
        {
            semaphore = new SemaphoreSlim(0, 3);

            Console.WriteLine("{0} tasks can enter the semaphore.",semaphore.CurrentCount);
            Task[] tasks = new Task[7];
            
            for (int i = 0; i <= 6; i++)
            {
                tasks[i] = Task.Run(() => {
                    Console.WriteLine("Task {0} begins and waits for the semaphore.",Task.CurrentId);
                    semaphore.Wait();
                    Console.WriteLine("Task {0} enters the semaphore.",Task.CurrentId);
                    Thread.Sleep(1000);
                    Console.WriteLine("Task {0} releases the semaphore; previous count: {1}.",Task.CurrentId, semaphore.Release());
                });
            }
            
            Thread.Sleep(500);
            
            Console.Write("Main thread calls Release(3) --> ");
            semaphore.Release(3);
            Console.WriteLine("{0} tasks can enter the semaphore.",semaphore.CurrentCount);

            Task.WaitAll(tasks);

            Console.WriteLine("Main thread exits.");
            Console.ReadLine();
        }
    }
}
