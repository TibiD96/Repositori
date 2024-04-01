using System;
using System.Collections.Generic;

namespace CodeEditor
{
    public class Controller
    {
        public static void RunMenu()
        {
            int[] validOptions = new[] { 0, 1, 2 };
            Consola.Menu();
            bool exitApp = false;
            while (!exitApp)
            {
                switch (ReadOption(validOptions))
                {
                    case 0:
                        exitApp = true;
                        break;
                    case 1:
                        string fullPath = PathToFile();
                        if (fullPath != "")
                        {
                            string[] lines = File.ReadAllLines(fullPath);
                            Consola.ShowContentOfFile(lines);
                            NavigateInConsole(lines);
                        }

                        exitApp = true;
                        break;
                    case 2:
                        Consola.Menu();
                        break;
                }
            }
        }

        private static void NavigateInConsole(string[] lines)
        {
            int startingLine = 0;
            int startingColumn = 0;
            int lineCounting = Console.CursorTop;
            int verticalPosition = Console.CursorTop;
            int horizontalPosition = Console.CursorLeft;
            ConsoleKeyInfo navigationDirection = ReadKey();
            while (navigationDirection.Key != ConsoleKey.Escape)
            {
                switch (navigationDirection.Key)
                {
                    case ConsoleKey.UpArrow:

                        CursorMovement.NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.DownArrow:

                        CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.LeftArrow:

                        CursorMovement.NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.RightArrow:

                        CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.End:

                        CursorMovement.EndButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.Home:

                        CursorMovement.HomeButtonBehaviour(ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.PageDown:

                        CursorMovement.PageDownBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;

                    case ConsoleKey.PageUp:

                        CursorMovement.PageUpBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn, lines);

                        break;
                }

                navigationDirection = ReadKey();
            }

            Console.SetCursorPosition(0, Console.WindowWidth);
        }

        private static int ReadOption(int[] validOption)
        {
            try
            {
                int answer = Convert.ToInt32(Console.ReadLine());
                if (validOption.Contains(answer))
                {
                    return answer;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Choose a valid option");
                Console.ResetColor();

                return ReadOption(validOption);
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Choose a valid option");
                Console.ResetColor();
                return ReadOption(validOption);
            }
        }

        private static string PathToFile()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Give the path to the file like in the next example!\nExample:\nC:\\Users\\danit\\OneDrive\\Desktop\\Fisier.TXT");
            Console.ResetColor();
            int[] validOptions = new[] { 1, 2 };
            string? pathOfFile = Console.ReadLine();
            int answer;

            if (File.Exists(pathOfFile))
            {
                return pathOfFile;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("File don't exist");
            Console.ResetColor();
            Console.WriteLine("Have you added a wrong path?\nPress 1 for \"yes\" or 2 for \"no\"");
            answer = ReadOption(validOptions);
            return answer == 1 ? PathToFile() : "";
        }

        private static ConsoleKeyInfo ReadKey()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Dictionary<ConsoleKey, ConsoleKey> keyValue = new Dictionary<ConsoleKey, ConsoleKey>()
            {
                { ConsoleKey.K, ConsoleKey.UpArrow },
                { ConsoleKey.J, ConsoleKey.DownArrow },
                { ConsoleKey.H, ConsoleKey.LeftArrow },
                { ConsoleKey.L, ConsoleKey.RightArrow },
                { ConsoleKey.D0, ConsoleKey.Home }
            };

            foreach (var key in keyValue)
            {
                if (keyInfo.Key == key.Key)
                {
                    return new ConsoleKeyInfo((char)0, keyValue[keyInfo.Key], false, false, false);
                }
            }

            if (keyInfo.KeyChar == '$')
            {
                return new ConsoleKeyInfo((char)0, ConsoleKey.End, false, false, false);
            }

            return keyInfo;
        }
    }
}