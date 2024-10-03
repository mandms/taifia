using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    internal class Program
    {
        void CreateHeader(List<string> uniqueTransactions)
        {
            for (int i = 0; i < uniqueTransactions.Count(); i++)
            {
                string transaction = uniqueTransactions[i];
                string output = ';' + transaction.Split('/')[1];
                File.AppendAllText("output.csv", output);
            }

            File.AppendAllText("output.csv", "\n");

            for (int i = 0; i < uniqueTransactions.Count(); i++)
            {
                string state = $";S{i}";
                File.AppendAllText("output.csv", state);
            }

            File.AppendAllText("output.csv", "\n");
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.csv");

            List<string> states = new List<string>();

            List<List<string>> transactions = new List<List<string>>();

            List<string> uniqueTransactions = new List<string>();

            foreach (string line in lines)
            {
                string[] splitLines = line.Split(';');

                if (lines[0] == line)
                {
                    states.AddRange(splitLines);
                    continue;
                }

                transactions.Add(splitLines.Skip(1).ToList());

                uniqueTransactions.AddRange(splitLines.Skip(1));
            }

            uniqueTransactions = (from l in uniqueTransactions let n = l.Split('/') select $"{n[0]}/{n[1]}").Distinct().ToList();

            File.WriteAllText("output.csv", string.Empty);

            new Program().CreateHeader(uniqueTransactions);

            InitialStateToFront(states, uniqueTransactions);
            //создание таблицы мура
            //List<List<string>> muraTransactions = new List<List<string>>();
            //List<string> tmp = new List<string>();
            string[,] tmp = new string[transactions.Count(), uniqueTransactions.Count()];
            for (int i = 0; i < uniqueTransactions.Count(); i++)
            {
                string transaction = uniqueTransactions[i].Split('/')[0];
                int stateIndex = states.FindIndex(x => x == transaction) - 1;

                transactions.ForEach((x) =>
                {
                    int transactionIndex = uniqueTransactions.FindIndex(s => s == x[stateIndex]);
                    tmp[transactions.IndexOf(x), i] = $"S{transactionIndex}";
                });
            }

            //for (int i = 0; i < transactions.Count(); i++)
            //{
            //    List<string> transaction = transactions[i];

            //    uniqueTransactions.ForEach(uTransaction =>
            //    {
            //        uTransaction = uTransaction.Split('/')[0];
            //        int stateIndex = states.FindIndex(x => x == uTransaction) - 1;
            //        int transactionIndex = uniqueTransactions.FindIndex(s => s == transaction[stateIndex]);
            //        tmp.Add($"S{transactionIndex}");
            //    });
            //}

            // вывод
            int rows = tmp.GetUpperBound(0) + 1;    // количество строк
            int columns = tmp.Length / rows;
            for (int i = 0; i < rows; i++)
            {
                File.AppendAllText("output.csv", $"z{i + 1}");
                for (int j = 0; j < columns; j++)
                {
                    File.AppendAllText("output.csv", $";{tmp[i, j]}");
                }
                File.AppendAllText("output.csv", "\n");
            }
            //for (int i = 0; i < transactions.Count(); i++)
            //{
            //    List<string> transaction = muraTransactions[i];

            //    File.AppendAllText("output.csv", $"z{i} {transaction}\n");
            //}
        }

        private static void InitialStateToFront(List<string> states, List<string> uniqueTransactions)
        {
            var t = uniqueTransactions[uniqueTransactions.FindIndex(x => x.Contains(states[1]))];
            var first = uniqueTransactions[0];

            uniqueTransactions[uniqueTransactions.FindIndex(x => x.Contains(states[1]))] = first;
            uniqueTransactions[0] = t;
        }
    }
}
