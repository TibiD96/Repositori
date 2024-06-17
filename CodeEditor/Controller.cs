using System;
using static System.Collections.Specialized.BitVector32;

namespace CodeEditor
{
    public class Controller
    {
        public static void OpenFile()
        {
            string currentDirectory = Environment.CurrentDirectory;
            string[] filesFromDirectory = Directory.GetFiles(currentDirectory);
            bool fastTravelMode = Config.FastTravel;
            List<string> allFiles = new List<string>();

            GetAllFiles(ref allFiles, currentDirectory);
            string filePathToOpen = FuzzySearchLogic.FuzzySearch(filesFromDirectory, allFiles.ToArray());
            string[] fileContent = File.ReadAllLines(filePathToOpen);

            Consola.ShowContentOfFile(fileContent, Console.WindowHeight - 1, fastTravelMode);
            InFileActions(fileContent, fastTravelMode);
        }

        private static void GetAllFiles(ref List<string> allFiles, string directory)
        {
            allFiles.AddRange(Directory.GetFiles(directory));

            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                GetAllFiles(ref allFiles, subDirectory);
            }
        }

        private static void InFileActions(string[] fileContent, bool fastTravelMode)
        {
            int startingLine = 0;
            int startingColumn = 0;
            string numberOfMoves = "";
            int lineCounting = Console.CursorTop;
            int verticalPosition = Console.CursorTop;
            int horizontalPosition = Console.CursorLeft;
            string[] originalFile = fileContent;

            ConsoleKeyInfo action = ReadKey(ref numberOfMoves);
            CursorMovement.FileParameter(fastTravelMode, fileContent);

            while (action.KeyChar != ':')
            {
                if (action.Key != ConsoleKey.I)
                {
                    Movements(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, numberOfMoves, action);
                }
                else
                {
                    EditMode(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, ref fileContent);
                }

                action = ReadKey(ref numberOfMoves);
            }

            Console.SetCursorPosition(0, Console.WindowWidth);
        }

        private static void Movements(
                                      ref int lineCounting,
                                      ref int horizontalPosition,
                                      ref int verticalPosition,
                                      ref int startingLine,
                                      ref int startingColumn,
                                      string numberOfMoves,
                                      ConsoleKeyInfo action)
        {
            char? character = ' ';

            for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
            {
                switch (action.Key)
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

                        CursorMovement.MoveWordRight(action.KeyChar, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.B:

                        CursorMovement.MoveWordLeft(action.KeyChar, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                        break;

                    case ConsoleKey.F:

                        character = ReadChar(ref character);

                        CursorMovement.FindCharacter(action.KeyChar, lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);

                        break;

                    case ConsoleKey.M:

                        character = ReadChar(ref character);

                        char key = Convert.ToChar(character);

                        CursorMovement.MarkLine(lineCounting, key);

                        break;
                }
            }

            if (action.KeyChar == '^')
            {
                CursorMovement.CaretBehaviour(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            }

            if (action.KeyChar == '\'')
            {
                character = ReadChar(ref character);

                char key = Convert.ToChar(character);

                CursorMovement.GoToMarkedLine(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn, key);
            }
        }

        private static void EditMode(ref int lineCounting, ref int horizontalPosition, ref int verticalPosition, ref int startingLine, ref int startingColumn, ref string[] fileContent)
        {
            bool arrowButton = false;
            ConsoleKeyInfo action = Console.ReadKey(true);
            bool fastTravelMode = Config.FastTravel;
            while (action.Key != ConsoleKey.Escape)
            {
                switch (action.Key)
                {
                    case ConsoleKey.UpArrow:

                        CursorMovement.NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        arrowButton = true;
                        break;

                    case ConsoleKey.DownArrow:

                        CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        arrowButton = true;
                        break;

                    case ConsoleKey.LeftArrow:

                        CursorMovement.NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        arrowButton = true;
                        break;

                    case ConsoleKey.RightArrow:

                        CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        arrowButton = true;
                        break;
                }

                if (!arrowButton)
                {
                    EditText(
                              ref lineCounting,
                              ref horizontalPosition,
                              ref verticalPosition,
                              ref startingLine,
                              ref startingColumn,
                              ref fileContent,
                              action);
                }

                action = Console.ReadKey(true);
                arrowButton = false;
            }
        }

        private static void EditText(
                                     ref int lineCounting,
                                     ref int horizontalPosition,
                                     ref int verticalPosition,
                                     ref int startingLine,
                                     ref int startingColumn,
                                     ref string[] fileContent,
                                     ConsoleKeyInfo action)
        {
            bool fastTravelMode = Config.FastTravel;
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";

            int charIndex = horizontalPosition + startingColumn - lineIndex.Length - 1;
            if (action.Key == ConsoleKey.Backspace && charIndex >= 0)
            {
                fileContent[lineCounting] = fileContent[lineCounting].Remove(charIndex, 1);
                CursorMovement.NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                Console.SetCursorPosition(horizontalPosition, verticalPosition);
            }
            else if (action.Key != ConsoleKey.Backspace)
            {
                fileContent[lineCounting] = charIndex == fileContent[lineCounting].Length
                    ? fileContent[lineCounting].Substring(charIndex) + action.KeyChar
                    : fileContent[lineCounting].Substring(0, charIndex + 1) + action.KeyChar + fileContent[lineCounting].Substring(charIndex + 1);

                CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                Console.SetCursorPosition(horizontalPosition, verticalPosition);
            }
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

        private static char? ReadChar(ref char? character)
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