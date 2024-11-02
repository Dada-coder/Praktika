using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dz_praktika2
{

    class TransportTask
    {
        
        static void Main()
        {
            int ech = 0;    
            Console.WriteLine("Введите количество поставщиков:");
            int rows = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите количество потребителей:");
            int cols = int.Parse(Console.ReadLine());

            int[,] costs = new int[rows, cols];
            Console.WriteLine("Введите матрицу стоимости перевозок:");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"Стоимость перевозки от поставщика {i + 1} к потребителю {j + 1}: ");
                    costs[i, j] = int.Parse(Console.ReadLine());
                }
            }

            
            int[] supply = new int[rows];
            Console.WriteLine("Введите запасы для каждого поставщика:");
            for (int i = 0; i < rows; i++)
            {
                Console.Write($"Запас поставщика {i + 1}: ");
                supply[i] = int.Parse(Console.ReadLine());
            }

            
            int[] demand = new int[cols];
            Console.WriteLine("Введите потребности для каждого потребителя:");
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"Потребность потребителя {j + 1}: ");
                demand[j] = int.Parse(Console.ReadLine());
            }

          
            int[,] plan = new int[rows, cols];
            InitializePlan(plan, supply, demand);

            
            while (true)
            {
                ech++;
                int[] u = new int[rows];
                int[] v = new int[cols];
                bool[] uFound = new bool[rows];
                bool[] vFound = new bool[cols];
                u[0] = 0;
                uFound[0] = true;
                bool potentialsFound;

                
                do
                {
                    potentialsFound = false;
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            if (plan[i, j] > 0)
                            {
                                if (uFound[i] && !vFound[j])
                                {
                                    v[j] = costs[i, j] - u[i];
                                    vFound[j] = true;
                                    potentialsFound = true;
                                }
                                else if (!uFound[i] && vFound[j])
                                {
                                    u[i] = costs[i, j] - v[j];
                                    uFound[i] = true;
                                    potentialsFound = true;
                                }
                            }
                        }
                    }
                } while (potentialsFound);

                
                int minDelta = 0;
                int minRow = -1, minCol = -1;
                bool isOptimal = true;

                Console.WriteLine("Оценки для небазисных клеток:");
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (plan[i, j] == 0)
                        {
                            int delta = costs[i, j] - (u[i] + v[j]);
                            Console.WriteLine($"Клетка ({i + 1},{j + 1}): Δ = {delta}");

                            if (delta < minDelta)
                            {
                                minDelta = delta;
                                minRow = i;
                                minCol = j;
                                isOptimal = false;
                            }
                        }
                    }
                }

                if (isOptimal)
                {
                    Console.WriteLine("Текущее решение оптимально.");
                    break;
                }

                Console.WriteLine("Текущее решение не оптимально. Улучшаем план...");
                ImprovePlan(plan, minRow, minCol);
                if (ech == 5)
                {
                    break;
                }
            }

           
            Console.WriteLine("Оптимальный план перевозок:");
            PrintMatrix(plan);
            Console.ReadLine();
        }

        
        static void InitializePlan(int[,] plan, int[] supply, int[] demand)
        {
            int rows = supply.Length;
            int cols = demand.Length;
            int i = 0, j = 0;

            while (i < rows && j < cols)
            {
                int quantity = Math.Min(supply[i], demand[j]);
                plan[i, j] = quantity;
                supply[i] -= quantity;
                demand[j] -= quantity;

                if (supply[i] == 0) i++;
                if (demand[j] == 0) j++;
            }
        }

        
        static void ImprovePlan(int[,] plan, int stRow, int stCol)
        {
            int minValue = int.MaxValue;

            
            if (stRow > 0) minValue = Math.Min(minValue, plan[stRow - 1, stCol]);
            if (stCol > 0) minValue = Math.Min(minValue, plan[stRow, stCol - 1]);

            if (stRow > 0) plan[stRow - 1, stCol] -= minValue;
            if (stCol > 0) plan[stRow, stCol - 1] -= minValue;
            plan[stRow, stCol] += minValue;

            Console.WriteLine("План после перераспределения:");
            PrintMatrix(plan);
        }

        
        static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}