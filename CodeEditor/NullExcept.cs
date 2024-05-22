namespace CodeEditor
{
    public class NullExcept
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
    }
}
