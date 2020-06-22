using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        static object l = new object();
        public static Random random = new Random();
        static int[,] matrix;
        static int[] arrayRnd;
        static int[] oddNumbers;


        /// <summary>
        ///  Matrix creation method
        /// </summary>
        static void Matrix()
        {
            // code lock
             lock (l)
            {
                matrix = new int[100, 100];
                //Waiting for the second thread to generate numbers
                while (arrayRnd == null)
                {
                    Monitor.Wait(l);
                }
                int m = 0;
                
                while (m < arrayRnd.Length)
                {
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            matrix[i, j] = arrayRnd[m];
                            m++;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///Method that generates random numbers
        /// </summary>
        static void RandomNumbers()
        {
            Thread.Sleep(20);
            lock (l)
            {
                arrayRnd = new int[10000];
                
                for (int i = 0; i < arrayRnd.Length; i++)
                {
                    arrayRnd[i] = random.Next(10, 100);
                }
                
                Monitor.Pulse(l); //sending another thread signal that generating is done
            }
        }

        /// <summary>
        ///  A method that writes all odd elements of a matrix to a file
        /// </summary>
        static void OddNumbers()
        {
            lock (l)
            {
                int count = 0;
                
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] % 2 == 1)
                        {
                            count++; // adding odd numbers

                        }
                    }
                }
                
                oddNumbers = new int[count];
                int a = 0;
                
                while (a < count)
                {
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            if (matrix[i, j] % 2 == 1)
                            {
                                oddNumbers[a] = matrix[i, j];
                                a++;
                            }
                        }
                    }
                }
                //Write to file
                StreamWriter sw = new StreamWriter(@"../../OddNumbersArray.txt");
                foreach (int number in oddNumbers)
                {
                    sw.WriteLine(number);
                }
                sw.Close();
                
                Monitor.Pulse(l); //sending another thread signal that generating is done
            }
        }

        /// <summary>
        /// Print from a file on the console
        /// </summary>
        static void OddNumbersPrint()
        {

            // locking method
            lock (l)
            {
                
                while (oddNumbers == null)
                {
                    Monitor.Wait(l); //Waiting for array to be created
                }
                
                string[] red = File.ReadAllLines(@"../../OddNumbersArray.txt");
                foreach (string number in red)
                {
                    Console.WriteLine(number);
                }
            }
        }

        static void Main(string[] args)
        {
            Thread matrix = new Thread(Matrix);//Creatig thread
            matrix.Start();//thread start

            Thread rndNumbers = new Thread(RandomNumbers);
            rndNumbers.Start();
            matrix.Join();
            rndNumbers.Join();
            Thread oddNumbers = new Thread(OddNumbers);
            Thread print = new Thread(OddNumbersPrint);
            print.Start();
            oddNumbers.Start();
            Console.ReadLine();
        }
    }
}
