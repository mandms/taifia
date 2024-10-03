namespace lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string cmd = args[0];
            string inputFileName = args[1];
            string outputFileName = args[2];

            switch(cmd)
            {
                case "mealy-to-moore":
                    {
                        new MiliToMura(inputFileName, outputFileName).MoveToMura();
                        break;
                    }
                case "moore-to-mealy":
                    {
                        new MuraToMili(inputFileName, outputFileName).MoveToMili();
                        break;
                    }
            }
        }
    }
}
