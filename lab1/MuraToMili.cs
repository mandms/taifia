using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace lab1
{
    public class MuraToMili
    {
        private Dictionary<string, string> _statesOutputsPairs = new Dictionary<string, string>();
        private List<List<string>> _transactions = new List<List<string>>();
        private string _inputFilename;
        private string _outputFilename;

        public MuraToMili(string inputFilename, string outputFilename)
        {
            _inputFilename = inputFilename;
            _outputFilename = outputFilename;
        }

        private void ReadFile()
        {
            string[] lines = File.ReadAllLines(_inputFilename);

            string[] outputData = lines[0].Split(';').Skip(1).ToArray();
            string[] states = lines[1].Split(';').Skip(1).ToArray();

            for (int i = 0; i < outputData.Length; i++)
            {
                if (!string.IsNullOrEmpty(states[i]) && i < outputData.Length)
                {
                    _statesOutputsPairs[states[i]] = outputData[i];
                }
            }

            foreach (string line in lines.Skip(2))
            {
                string[] splitLines = line.Split(';');
                _transactions.Add(splitLines.Skip(1).ToList());
            }
        }

        void ClearFile() => File.WriteAllText(_outputFilename, string.Empty);

        public void MoveToMili()
        {
            ReadFile();

            for (int i = 0; i < _transactions.Count; i++)
            {
                for (int j = 0; j < _transactions[i].Count; j++)
                {
                    string output = "";
                    if (_statesOutputsPairs.TryGetValue(_transactions[i][j], out output))
                    {
                        _transactions[i][j] = $"{_transactions[i][j]}/{output}";
                    }
                }
            }

            ClearFile();
            WriteHeader();
            WriteTransactions();
        }

        private void WriteHeader()
        {
            foreach (string output in _statesOutputsPairs.Keys)
            {
                File.AppendAllText(_outputFilename, $";{output}");
            }
            File.AppendAllText(_outputFilename, "\n");
        }

        private void WriteTransactions()
        {
            for (int i = 0; i < _transactions.Count; i++)
            {
                List<string> transactionsLine = _transactions[i];
                File.AppendAllText(_outputFilename, $"z{i + 1}");

                foreach (string transaction in transactionsLine)
                {
                    File.AppendAllText(_outputFilename, $";{transaction}");
                }
                File.AppendAllText(_outputFilename, "\n");
            }
        }
    }
}
