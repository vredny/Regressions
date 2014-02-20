using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Linear_regression
{
    class Program
    {

        static double Goal(double[] theta, double[,] data, int vectors, int components) //целевая функция
        {
            double s = 0;
            for (int i = 0; i < vectors; i++)
            {
                double sum = 0;
                for (int j = 0; j < components - 1; j++)
                {
                    sum += theta[j] * data[i, j];
                }
                double func = 1 / (1 + Math.Exp(-sum));

                s += data[i, components - 1] * Math.Log(func) + (1 - data[i, components - 1]) * Math.Log(1 - func);
            }
            return (-s) / vectors;
        }


        static double Grad(double[] theta, double[,] data, int vectors, int components, int i) //i-ая компонента градиента
        {
            double gr = 0;

            for (int j = 0; j < vectors; j++)
            {
                double sum = 0;
                for (int k = 0; k < components - 1; k++)
                {
                    sum += theta[k] * data[j, k];
                }
                gr += (data[j, components - 1] - (1 / (1 + Math.Exp(-sum)))) * data[j, i];
            }

            gr = (-1) * (gr / vectors);

            return gr;
        }


        static void Main(string[] args)
        {
            double[,] data, input;
            double[] theta, newtheta, vec;

            int components, MAXvectors, vectors;


            Console.WriteLine("number of components?");
            components = int.Parse(Console.ReadLine());
            components++; //еще одна координата нужна для ЛР (в нулевой коорденате будут единицы);


            MAXvectors = 10000000;/////////////
            data = new double[MAXvectors, components];



            int i = 0, j = 0;
            StreamReader infile = new StreamReader(@"C:\LinearRegression\training.txt"); // тренировочный сет
            StreamWriter outfile = new StreamWriter(@"C:\LinearRegression\output.txt");
            while (!infile.EndOfStream)
            {
                string str;
                str = infile.ReadLine();
                string[] numbers = str.Split(',');
                for (j = 1; j < components - 1; j++) data[i, j] = int.Parse(numbers[j]);

                data[i, 0] = 1;

                i++;

            }
            vectors = i;

            theta = new double[components];
            newtheta = new double[components];

            Random r = new Random();

            for (i = 0; i < components; i++)
                theta[i] = r.NextDouble();


            int iter = 0;
            double goal;
            while (true)
            {
                iter++;

                goal = Goal(theta, data, vectors, components);
                Console.WriteLine(goal);



                // lambda = lambda / iter;
                for (i = 0; i < components - 2; i++)
                {
                    newtheta[i] = theta[i] - 0.0001 * Grad(theta, data, vectors, components, i);

                }
                theta = newtheta;
                if (Math.Abs(goal - Goal(theta, data, vectors, components)) < 0.0001) break;

            }

            vec = new double[components];


            StreamReader testfile = new StreamReader(@"C:\LinearRegression\testing.txt"); // тренировочный сет
            i = 0;
            input = new double[10000, components];

            while (!testfile.EndOfStream)
            {
                string str;
                str = testfile.ReadLine();
                string[] numbers = str.Split(',');
                for (j = 0; j < components - 1; j++) input[i, j] = int.Parse(numbers[j]);
                input[i, 0] = 1;
                i++;
            }

            int n = i;

            int accuracy = 0;

            for (i = 0; i < n; i++)
            {
                double s = 0;
                for (j = 0; j < components - 2; j++) s += input[i, j] * theta[j];
                if ((s < -0.2) && (input[i, components - 2] == 4)) accuracy++;
                if ((s >= 0.2) && (input[i, components - 2] == 2)) accuracy++;
                outfile.WriteLine(s + "|" + input[i, components - 2]);
            }

            accuracy = (accuracy * 100) / n;
            Console.WriteLine(accuracy);
            Console.WriteLine("Done");
            outfile.Close();
            Console.ReadLine();
        }
    }
}
