namespace CodeEditor
{
    public class NullOrEmptyCases
    {
        public static void ArgumentNullException(string input)
        {
            if (input != null)
            {
                return;
            }

            throw new ArgumentNullException(input);
        }

        public static void ArgumentNullException(string[] input)
        {
            if (input != null)
            {
                return;
            }

            throw new ArgumentNullException(nameof(input));
        }

        public static void ArgumentNullException(List<string> input)
        {
            if (input != null)
            {
                return;
            }

            throw new ArgumentNullException(nameof(input));
        }

        public static (int, int) CurrentEndColumn(int lineCounting, int startingColumn, string[] fileContent)
        {
            int currentStartColumn;

            if (fileContent.Length == 0)
            {
                return (0, 0);
            }

            return (currentStartColumn = Math.Max(0, Math.Min(startingColumn, fileContent[lineCounting].Length)), fileContent[lineCounting].Length - currentStartColumn < Console.WindowWidth ? fileContent[lineCounting].Length - currentStartColumn : Console.WindowWidth - 1);
        }
    }
}
