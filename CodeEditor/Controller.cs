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
                        int currentLine = Console.WindowHeight - 1;
                        string fullPath = PathToFile();
                        if (fullPath != "")
                        {
                            bool fastTravelMode = FastTravel();
                            string[] lines = File.ReadAllLines(fullPath);
                            Consola.KeysForMovement();
                            Consola.ShowContentOfFile(lines, currentLine, fastTravelMode);
                            NavigateInConsole(lines, fastTravelMode);
                        }

                        exitApp = true;
                        break;
                    case 2:
                        Consola.Menu();
                        break;
                }
            }
        }

        private static void NavigateInConsole(string[] lines, bool fastTravelMode)
        {
            int startingLine = 0;
            int startingColumn = 0;
            string numberOfMoves = "";
            char? character = ' ';
            int lineCounting = Console.CursorTop;
            int verticalPosition = Console.CursorTop;
            int horizontalPosition = Console.CursorLeft;
            ConsoleKeyInfo navigationDirection = ReadKey(ref numberOfMoves);
            CursorMovement.FileParameter(fastTravelMode, lines);
            while (navigationDirection.Key != ConsoleKey.Escape)
            {
                for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                {
                    switch (navigationDirection.Key)
                    {
                        case ConsoleKey.UpArrow:

                            CursorMovement.NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.DownArrow:

                            CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.LeftArrow:

                            CursorMovement.NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.RightArrow:

                            CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.End:

                            CursorMovement.EndButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.Home:

                            CursorMovement.HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.PageDown:

                            CursorMovement.PageDownBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.PageUp:

                            CursorMovement.PageUpBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.W:

                            CursorMovement.MoveWordRight(navigationDirection.KeyChar, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.B:

                            CursorMovement.MoveWordLeft(navigationDirection.KeyChar, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            break;

                        case ConsoleKey.F:

                            character = ReadChar(ref character, i);

                            CursorMovement.FindCharacter(navigationDirection.KeyChar, lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);

                            break;

                        case ConsoleKey.M:

                            CursorMovement.SeeKeys(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            break;
                    }
                }

                if (navigationDirection.KeyChar == '^')
                {
                    CursorMovement.CaretBehaviour(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                }

                character = ' ';
                navigationDirection = ReadKey(ref numberOfMoves);
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
            Console.WriteLine("File don't exist or wrong path");
            Console.WriteLine("Do you want to add the path again?\nPress 1 for \"yes\" \nPress 2 for \"no\"");
            Console.ResetColor();
            answer = ReadOption(validOptions);

            return answer == 1 ? PathToFile() : "";
        }

        private static ConsoleKeyInfo ReadKey(ref string numberOfMoves)
        {
            numberOfMoves = "";
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (char.IsDigit(keyInfo.KeyChar))
            {
                if (keyInfo.KeyChar == '0' && numberOfMoves.Length == 0)
                {
                    break;
                }

                numberOfMoves = numberOfMoves + keyInfo.KeyChar;
                keyInfo = Console.ReadKey(true);
            }

            if (numberOfMoves.Length == 0)
            {
                numberOfMoves = "1";
            }

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

        private static bool FastTravel()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Do you want to activate Fast Travel Mode?\nPress 1 for \"yes\" \nPress 2 for \"no\"");
            Console.ResetColor();
            int[] validOptions = new[] { 1, 2 };
            int answer = ReadOption(validOptions);

            return answer == 1;
        }

        private static char? ReadChar(ref char? character, int numberOfMoves)
        {
            if (character == ' ')
            {
                ConsoleKeyInfo baseInput = Console.ReadKey(true);

                character = baseInput.KeyChar;

                return character;
            }

            return character;
        }
    }
}