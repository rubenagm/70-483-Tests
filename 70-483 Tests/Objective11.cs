using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        #region listing 1.7
        /*
         * QueueUserWorkItem Genera automáticamente hilos los cuales se auto-administran para no generar una sobrecarga en los procesos
         * Los procesos se encolan para ser atendidos cuando un hilo es desocupado.
         * Cómo cambiar estas configuraciones?
         */
        public static void main17()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine($"Proceso {i} ha sido agregado.");
                ThreadPool.QueueUserWorkItem((obj) => {
                    Console.WriteLine($"Trabajando en hilo: {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"Trabajo terminado: {Thread.CurrentThread.ManagedThreadId}");
                });
            }
        }
        #endregion

        #region listing 1.10
        public static void main110()
        {
            Task<int> task = Task.Run(() => {
                Console.WriteLine("Corriendo Task");
                Thread.Sleep(3000);
                return 2;
            }).ContinueWith<int>((p) => {
                Console.WriteLine("Entra continue");
                return p.Result * 4;
            });

            Console.WriteLine($"El resultado es: {task.Result}");
        }
        #endregion

        #region listing 1.11 y 1.13
        public static void main111()
        {
            Task<Int32[]> task = Task.Run(() => {
                var tasks = new int[3];
                Task[] obTasks = new Task[3];
                new Task<Int32>(() => {
                    tasks[0] = 1;
                    Console.WriteLine($"Trabajando en el proceso: {tasks[0]}");
                    Thread.Sleep(3000);
                    return 1;
                }, TaskCreationOptions.AttachedToParent).Start();
                new Task<Int32>(() => {
                    tasks[1] = 2;
                    Console.WriteLine($"Trabajando en el proceso: {tasks[0]}");
                    Thread.Sleep(5000);
                    return 2;
                }, TaskCreationOptions.AttachedToParent).Start();
                new Task<Int32>(() => {
                    tasks[2] = 3;
                    Console.WriteLine($"Trabajando en el proceso: {tasks[0]}");
                    Thread.Sleep(3000);
                    return 3;
                }, TaskCreationOptions.AttachedToParent).Start();

                Task.WaitAll();
                return tasks;
            });

            var finalTask = task.ContinueWith(firstTask => {
                foreach (var item in firstTask.Result)
                {
                    Console.WriteLine($"Imprimiendo: {item}");
                }
            });

            finalTask.Wait();
        }
        #endregion

        #region listing 1.14
        public static void main114()
        {
            Task task = Task.Run(() => {
                Task[] obTasks = new Task[3];

                obTasks[0] = Task.Run(() => {
                    Console.WriteLine($"Trabajando en el proceso: 1");
                    Thread.Sleep(3000);
                    Console.WriteLine("Proceso 1 terminado.");
                    return 1;
                });

                obTasks[1] = Task.Run(() => {
                    Console.WriteLine($"Trabajando en el proceso: 2");
                    Thread.Sleep(5000);
                    Console.WriteLine("Proceso 2 terminado.");
                    return 2;
                });
                obTasks[2] = Task.Run(() => {
                    Console.WriteLine($"Trabajando en el proceso: 3");
                    Thread.Sleep(3000);
                    Console.WriteLine("Proceso 3 terminado.");
                    return 3;
                });

                Task.WaitAll(obTasks);
            });

            task.ContinueWith(t => {
                Console.WriteLine("Todos los procesos terminados.");
            });
            //task.Wait();
            //Console.WriteLine("Todos los procesos terminados.");
        }
        #endregion


    }
}
