using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace lab1
{
    public class MiliToMura
    {
        List<string> _states = new List<string>();
        List<List<string>> _transactions = new List<List<string>>();
        List<string> _uniqueTransactions = new List<string>();

        private string _inputFilename;
        private string _outputFilename;

        public MiliToMura(string inputFilename, string outputFilename)
        {
            _inputFilename = inputFilename;
            _outputFilename = outputFilename;
        }

        private void Read()
        {
            string[] lines = File.ReadAllLines(_inputFilename);

            foreach (string line in lines)
            {
                string[] splitLines = line.Split(';');

                if (lines[0] == line)
                {
                    _states.AddRange(splitLines.Skip(1));
                    continue;
                }

                _transactions.Add(splitLines.Skip(1).ToList());
                _uniqueTransactions.AddRange(splitLines.Skip(1));
            }

            _uniqueTransactions = (
                from line in _uniqueTransactions 
                let transactions = line.Split('/')
                select $"{transactions[0]}/{transactions[1]}"
                                   ).Distinct().ToList();
        }

        private void CreateHeader()
        {
            for (int i = 0; i < _uniqueTransactions.Count(); i++)
            {
                string transaction = _uniqueTransactions[i];
                string output = ';' + transaction.Split('/')[1];
                File.AppendAllText(_outputFilename, output);
            }

            File.AppendAllText(_outputFilename, "\n");

            for (int i = 0; i < _uniqueTransactions.Count(); i++)
            {
                string state = $";S{i}";
                File.AppendAllText(_outputFilename, state);
            }

            File.AppendAllText(_outputFilename, "\n");
        }

        private void InitialStateToFront()
        {
            int firstItemIdx = _uniqueTransactions.FindIndex(x => x.Contains(_states[0]));
            var first = _uniqueTransactions[0];

            if (firstItemIdx == -1)
            {
                string initState = _states[0];
                _uniqueTransactions[0] = $"{initState}/-";
                _uniqueTransactions.Add(first);
                return;
            }
            var tmp = _uniqueTransactions[firstItemIdx];

            _uniqueTransactions[firstItemIdx] = first;
            _uniqueTransactions[0] = tmp;
        }

        public void MoveToMura()
        {
            Read();
            InitialStateToFront();

            string[,] muraTransactions = new string[_transactions.Count(), _uniqueTransactions.Count()];
            for (int i = 0; i < _uniqueTransactions.Count(); i++)
            {
                string transaction = _uniqueTransactions[i].Split('/')[0];
                int stateIndex = _states.FindIndex(x => x == transaction);

                _transactions.ForEach(x =>
                {
                    int transactionIndex = _uniqueTransactions.FindIndex(s => s == x[stateIndex]);
                    muraTransactions[_transactions.IndexOf(x), i] = $"S{transactionIndex}";
                });
            }

            WriteToFile(muraTransactions);
        }

        void ClearFile() => File.WriteAllText(_outputFilename, string.Empty);

        private void WriteToFile(string[,] muraTransactions)
        {
            ClearFile();
            CreateHeader();

            int rows = muraTransactions.GetUpperBound(0) + 1;
            int columns = muraTransactions.Length / rows;
            for (int i = 0; i < rows; i++)
            {
                File.AppendAllText(_outputFilename, $"z{i + 1}");
                for (int j = 0; j < columns; j++)
                {
                    File.AppendAllText(_outputFilename, $";{muraTransactions[i, j]}");
                }
                File.AppendAllText(_outputFilename, "\n");
            }
        }
    }
}
