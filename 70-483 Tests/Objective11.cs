using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _70_483_Tests
{
    class Objective11
    {
        #region listing 1.1
        public static void ThreadMethod11()
        {
            for (int x = 0; x < 10; x++)
            {
                Console.WriteLine("Hilo: {0}", x);
                Thread.Sleep(0);
            }
        }

        public static void main11()
        {
            Thread thread = new Thread(new ThreadStart(ThreadMethod11));
            thread.Start();

            for (int x = 0; x < 5; x++)
            {
                Console.WriteLine("Programa Principal");
                Thread.Sleep(0);
            }

            thread.Join();

            Console.WriteLine("El programa finalizó.");

            Console.ReadLine();
        }
        #endregion

        #region listing 1.2
        public static void ThreadMethod12()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("Hilo: {0}", i);
                Thread.Sleep(1000);
            }
        }

        public static void main12()
        {
            Thread thread = new Thread(new ThreadStart(ThreadMethod12));
            /*
             * Diferencia al poner IsBakcground en "true"
             * IsBackground corre la aplicación en un hilo interno por separado. Cuando es false, la aplicación finaliza junto con la aplicación principal
             * (En este caso el main)
             */
            thread.IsBackground = true;
            thread.Start();
            Console.ReadLine();
        }
        #endregion

        #region listing 1.5
        [ThreadStatic] //Toma de manera distinta el count dentro de los hilos
        public static int count;
        public static void main15()
        {
            Thread thread1 = new Thread(() => {
                for (int i = 0; i < 10; i++)
                {
                    count++;
                    Console.WriteLine("Hilo: {0}", count);
                    Thread.Sleep(300);
                }
            });

            Thread thread2 = new Thread(() => {
                for (int i = 0; i < 10; i++)
                {
                    count++;
                    Console.WriteLine("Hilo 2: {0}", count);
                    Thread.Sleep(300);
                }
            });

            thread1.Start();
            thread2.Start();

            Console.ReadLine();
        }
        #endregion

        #region listing 1.6
        /*Toma la variable de manera global pero las toma como variables independientes en cada hilo.
         * A diferencia del programa anterior en este se pueden inicializar las variables dependiendo el hilo.
         * (Se puede tomar información mediantte "Thread.CurrentThread")
         */
        public static ThreadLocal<int> count16 = new ThreadLocal<int>(() => {
            return Thread.CurrentThread.ManagedThreadId;
        });

        public static void main16()
        {
            Thread thread1 = new Thread(() => {
                for (int i = 0; i < 10; i++)
                {
                    
                    Console.WriteLine("Hilo: {0}", count16.Value);
                    Thread.Sleep(300);
                }
            });

            Thread thread2 = new Thread(() => {
                for (int i = 0; i < 10; i++)
                {
                    
                    Console.WriteLine("Hilo: {0}", count16.Value);
                    Thread.Sleep(300);
                }
            });

            thread1.Start();
            thread2.Start();

            Console.ReadLine();
        }
        #endregion
    }
}
