using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        static int[,] matrix;
        static int[] arrayRnd;
        static object l = new object();
        static int[] oddNumbers;

        static void Matrix()
        {
            
            lock (l)
            {
                
                matrix = new int[100, 100];
                
                while (arrayRnd == null)
                {
                    Monitor.Wait(l);
                }
                                int k = 0;
                
                while (k < arrayRnd.Length)
                {
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            matrix[i, j] = arrayRnd[k];
                            k++;
                        }
                    }
                }
            }
        }

        static void RandomNumbers()
        {
             
            lock (l)
            {
                arrayRnd = new int[10000];
                Random random = new Random();
                for (int i = 0; i < arrayRnd.Length; i++)
                {
                    arrayRnd[i] = random.Next(10, 100);
                }
                
                Monitor.Pulse(l);
            }
        }
        static void OddNumbers()
        {

        }

        static void OddNumbersPrint()
        {

        }
        static void Main(string[] args)
        {
            Thread matrix = new Thread(Matrix);
            matrix.Start();
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
