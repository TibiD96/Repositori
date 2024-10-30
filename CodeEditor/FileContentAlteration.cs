using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CodeEditor
{
    public class FileContentAlteration
    {
        public static void EditText(
                                    ref int lineCounting,
                                    ref int horizontalPosition,
                                    ref int verticalPosition,
                                    ref int startingLine,
                                    ref int startingColumn,
                                    ref string[] fileContent,
                                    ConsoleKeyInfo action)
        {
            bool fastTravelMode = Config.FastTravel;
            int charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

            switch (action.Key)
            {
                case ConsoleKey.Backspace:
                    DeleteLine(
                                ref lineCounting,
                                ref horizontalPosition,
                                ref verticalPosition,
                                ref startingLine,
                                ref startingColumn,
                                ref fileContent,
                                charIndex);
                    break;

                case ConsoleKey.Enter:
                    AddLine(
                             ref lineCounting,
                             ref horizontalPosition,
                             ref verticalPosition,
                             ref startingLine,
                             ref startingColumn,
                             ref fileContent,
                             charIndex);
                    break;

                default:

                    Variables.Undo.Peek().Push((lineCounting, fileContent[lineCounting]));
                    Variables.UndoDeleteLine.Peek().Push(false);
                    Variables.UndoAddLine.Peek().Push(false);

                    if (!Variables.EditAfterCursor && charIndex + 1 != fileContent[lineCounting].Length)
                    {
                        CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        Variables.EditAfterCursor = true;
                    }

                    fileContent[lineCounting] = charIndex == fileContent[lineCounting].Length
                        ? fileContent[lineCounting].Substring(charIndex) + action.KeyChar
                        : fileContent[lineCounting].Substring(0, charIndex + 1) + action.KeyChar + fileContent[lineCounting].Substring(charIndex + 1);

                    CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                    Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                    Console.SetCursorPosition(horizontalPosition, verticalPosition);
                    break;
            }
        }

        public static void DeleteLine(
                                       ref int lineCounting,
                                       ref int horizontalPosition,
                                       ref int verticalPosition,
                                       ref int startingLine,
                                       ref int startingColumn,
                                       ref string[] fileContent,
                                       int charIndex)
        {
            bool fastTravelMode = Config.FastTravel;

            if (charIndex >= 0)
            {
                Variables.UndoDeleteLine.Peek().Push(false);
                Variables.Undo.Peek().Push((lineCounting, fileContent[lineCounting]));
                fileContent[lineCounting] = fileContent[lineCounting].Remove(charIndex, 1);
                CursorMovement.NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                Console.SetCursorPosition(horizontalPosition, verticalPosition);
            }
            else
            {
                if (lineCounting > 0)
                {
                    Variables.UndoDeleteLine.Peek().Push(true);
                    string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting - 1, lineCounting - 1, Convert.ToString(fileContent.Length)) + " ";
                    int indexOfLastChar = fileContent[lineCounting - 1].Length - 1 + lineIndex.Length;
                    if (fileContent[lineCounting].Length > 0)
                    {
                        Variables.Undo.Peek().Push((lineCounting - 1, fileContent[lineCounting - 1]));
                        fileContent[lineCounting - 1] = fileContent[lineCounting - 1] + fileContent[lineCounting];
                    }

                    for (int i = lineCounting; i < fileContent.Length - 1; i++)
                    {
                        Variables.Undo.Peek().Push((i, fileContent[i]));
                        fileContent[i] = fileContent[i + 1];
                        Variables.UndoDeleteLine.Peek().Push(false);
                    }

                    Variables.Undo.Peek().Push((fileContent.Length - 1, fileContent[fileContent.Length - 1]));
                    Variables.UndoDeleteLine.Peek().Push(false);

                    string[] newfileContent = fileContent.Take(fileContent.Length - 1).ToArray();

                    fileContent = (string[])newfileContent.Clone();

                    CursorMovement.FileParameter(fastTravelMode, fileContent);
                    CursorMovement.NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                    lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";

                    startingColumn = 0;
                    horizontalPosition = lineIndex.Length;

                    while (horizontalPosition + startingColumn < indexOfLastChar + 1)
                    {
                        CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                    }

                    Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                    Console.SetCursorPosition(horizontalPosition, verticalPosition);
                }
            }
        }

        public static void AddLine(
                                      ref int lineCounting,
                                      ref int horizontalPosition,
                                      ref int verticalPosition,
                                      ref int startingLine,
                                      ref int startingColumn,
                                      ref string[] fileContent,
                                      int charIndex)
        {
            bool fastTravelMode = Config.FastTravel;
            string[] newFileContent = new string[fileContent.Length + 1];
            string newLine = fileContent[lineCounting].TakeWhile(c => c == ' ').Aggregate("", (current, c) => current + c);
            int emptySpacesLength = newLine.Length;

            Variables.UndoAddLine.Peek().Push(true);

            if (charIndex >= 0)
            {
                Variables.Undo.Peek().Push((lineCounting, fileContent[lineCounting]));
                newLine = newLine + fileContent[lineCounting].Substring(charIndex + 1);
                fileContent[lineCounting] = fileContent[lineCounting].Substring(0, charIndex + 1);
            }
            else
            {
                Variables.Undo.Peek().Push((lineCounting, fileContent[lineCounting]));
                newLine = newLine + fileContent[lineCounting];
                fileContent[lineCounting] = fileContent[lineCounting].Remove(0);
            }

            Array.Copy(fileContent, 0, newFileContent, 0, lineCounting + 1);
            newFileContent[lineCounting + 1] = newLine;

            if (lineCounting < fileContent.Length)
            {
                for (int i = lineCounting + 2; i < newFileContent.Length; i++)
                {
                    Variables.Undo.Peek().Push((i - 1, fileContent[i - 1]));
                    newFileContent[i] = fileContent[i - 1];
                }
            }

            fileContent = (string[])newFileContent.Clone();

            CursorMovement.FileParameter(fastTravelMode, fileContent);
            CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
            startingColumn = 0;
            horizontalPosition = emptySpacesLength + lineIndex.Length;
            Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition, verticalPosition);
        }

        public static void AutoDelete(
                                    ref int lineCounting,
                                    ref int horizontalPosition,
                                    ref int verticalPosition,
                                    ref int startingLine,
                                    ref int startingColumn,
                                    ref string[] fileContent)
        {
            string numberOfMoves = "";
            bool fastTravelMode = Config.FastTravel;
            int charIndex;
            int originalHorizotalPosition;
            int startingChar;
            char? character = ' ';
            int endCharIndex;
            ConsoleKeyInfo keyInfo = Controller.ReadKey(ref numberOfMoves);

            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow:

                    for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                    {
                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        DeleteLine(
                                    ref lineCounting,
                                    ref horizontalPosition,
                                    ref verticalPosition,
                                    ref startingLine,
                                    ref startingColumn,
                                    ref fileContent,
                                    charIndex);
                    }

                    break;

                case ConsoleKey.RightArrow:

                    CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                    for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                    {
                        CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        DeleteLine(
                                       ref lineCounting,
                                       ref horizontalPosition,
                                       ref verticalPosition,
                                       ref startingLine,
                                       ref startingColumn,
                                       ref fileContent,
                                       charIndex);
                    }

                    CursorMovement.NavigateLeft(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                    break;

                case ConsoleKey.D:

                    Variables.UndoDeleteLine.Peek().Push(false);
                    Variables.Undo.Peek().Push((lineCounting, fileContent[lineCounting]));
                    fileContent[lineCounting] = fileContent[lineCounting].Remove(0);
                    CursorMovement.HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
                    Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                    Console.SetCursorPosition(horizontalPosition, verticalPosition);
                    charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                    if (lineCounting == 0)
                    {
                        CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                    }

                    DeleteLine(
                                       ref lineCounting,
                                       ref horizontalPosition,
                                       ref verticalPosition,
                                       ref startingLine,
                                       ref startingColumn,
                                       ref fileContent,
                                       charIndex);

                    break;

                case ConsoleKey.UpArrow:

                    originalHorizotalPosition = horizontalPosition;
                    CursorMovement.NavigateUp(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                    for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                    {
                        Variables.UndoDeleteLine.Peek().Push(false);
                        Variables.Undo.Peek().Push((lineCounting, fileContent[lineCounting]));
                        fileContent[lineCounting] = fileContent[lineCounting].Remove(0);
                        CursorMovement.HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
                        Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                        Console.SetCursorPosition(horizontalPosition, verticalPosition);
                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                        if (lineCounting == 0)
                        {
                            CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        }

                        DeleteLine(
                                           ref lineCounting,
                                           ref horizontalPosition,
                                           ref verticalPosition,
                                           ref startingLine,
                                           ref startingColumn,
                                           ref fileContent,
                                           charIndex);

                    }

                    horizontalPosition = originalHorizotalPosition;

                    CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                    break;

                case ConsoleKey.DownArrow:

                    originalHorizotalPosition = horizontalPosition;

                    for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                    {
                        CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        Variables.UndoDeleteLine.Peek().Push(false);
                        Variables.Undo.Peek().Push((lineCounting, fileContent[lineCounting]));
                        fileContent[lineCounting] = fileContent[lineCounting].Remove(0);
                        CursorMovement.HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
                        Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
                        Console.SetCursorPosition(horizontalPosition, verticalPosition);
                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                        if (lineCounting == 0)
                        {
                            CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        }

                        DeleteLine(
                                           ref lineCounting,
                                           ref horizontalPosition,
                                           ref verticalPosition,
                                           ref startingLine,
                                           ref startingColumn,
                                           ref fileContent,
                                           charIndex);

                    }

                    horizontalPosition = originalHorizotalPosition;
                    Console.SetCursorPosition(horizontalPosition, verticalPosition);

                    break;

                case ConsoleKey.End:

                    Variables.Undo.Push(new Stack<(int, string)>());
                    Variables.UndoDeleteLine.Push(new Stack<bool>());
                    Variables.UndoAddLine.Push(new Stack<bool>());
                    Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                    Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                    DeleteTilTheEnd(
                          ref lineCounting,
                          ref horizontalPosition,
                          ref verticalPosition,
                          ref startingLine,
                          ref startingColumn,
                          ref fileContent);

                    break;

                case ConsoleKey.Home:

                    Variables.Undo.Push(new Stack<(int, string)>());
                    Variables.UndoDeleteLine.Push(new Stack<bool>());
                    Variables.UndoAddLine.Push(new Stack<bool>());
                    Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                    Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                    string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";

                    while (horizontalPosition > lineIndex.Length)
                    {
                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        DeleteLine(
                                    ref lineCounting,
                                    ref horizontalPosition,
                                    ref verticalPosition,
                                    ref startingLine,
                                    ref startingColumn,
                                    ref fileContent,
                                    charIndex);
                    }

                    break;

                    case ConsoleKey.PageUp:

                    Variables.Undo.Push(new Stack<(int, string)>());
                    Variables.UndoDeleteLine.Push(new Stack<bool>());
                    Variables.UndoAddLine.Push(new Stack<bool>());
                    Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                    Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                    for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                    {
                        while (verticalPosition + 1 < Console.WindowHeight - 3)
                        {
                            CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        }

                        Variables.UndoDeleteLine.Peek().Push(false);
                        Variables.Undo.Peek().Push((lineCounting, fileContent[lineCounting]));

                        while (verticalPosition > 0)
                        {
                            fileContent[lineCounting] = fileContent[lineCounting].Remove(0);
                            CursorMovement.HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
                            charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                            if (lineCounting == 0)
                            {
                                CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                            }

                            DeleteLine(
                                               ref lineCounting,
                                               ref horizontalPosition,
                                               ref verticalPosition,
                                               ref startingLine,
                                               ref startingColumn,
                                               ref fileContent,
                                               charIndex);
                        }

                        fileContent[lineCounting] = fileContent[lineCounting].Remove(0);
                        CursorMovement.HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                        if (lineCounting == 0)
                        {
                            CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        }

                        DeleteLine(
                                           ref lineCounting,
                                           ref horizontalPosition,
                                           ref verticalPosition,
                                           ref startingLine,
                                           ref startingColumn,
                                           ref fileContent,
                                           charIndex);

                        CursorMovement.PageUpBehaviour(ref lineCounting, horizontalPosition, verticalPosition, ref startingLine, ref startingColumn);
                    }

                    break;

                case ConsoleKey.PageDown:

                    Variables.Undo.Push(new Stack<(int, string)>());
                    Variables.UndoDeleteLine.Push(new Stack<bool>());
                    Variables.UndoAddLine.Push(new Stack<bool>());
                    Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                    Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                    for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                    {
                        while (verticalPosition + 1 < Console.WindowHeight - 3)
                        {
                            CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        }

                        Variables.UndoDeleteLine.Peek().Push(false);
                        Variables.Undo.Peek().Push((lineCounting, fileContent[lineCounting]));

                        while (verticalPosition > 0)
                        {
                            fileContent[lineCounting] = fileContent[lineCounting].Remove(0);
                            CursorMovement.HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
                            charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                            if (lineCounting == 0)
                            {
                                CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                            }

                            DeleteLine(
                                               ref lineCounting,
                                               ref horizontalPosition,
                                               ref verticalPosition,
                                               ref startingLine,
                                               ref startingColumn,
                                               ref fileContent,
                                               charIndex);
                        }

                        fileContent[lineCounting] = fileContent[lineCounting].Remove(0);
                        CursorMovement.HomeButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                        if (lineCounting == 0)
                        {
                            CursorMovement.NavigateDown(ref lineCounting, horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);
                        }

                        DeleteLine(
                                           ref lineCounting,
                                           ref horizontalPosition,
                                           ref verticalPosition,
                                           ref startingLine,
                                           ref startingColumn,
                                           ref fileContent,
                                           charIndex);

                    }

                    break;

                case ConsoleKey.W:

                    Variables.Undo.Push(new Stack<(int, string)>());
                    Variables.UndoDeleteLine.Push(new Stack<bool>());
                    Variables.UndoAddLine.Push(new Stack<bool>());
                    Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                    Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                    startingChar = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                    for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                    {
                        CursorMovement.MoveWordRight(keyInfo.KeyChar, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                    }

                    charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                    while (startingChar != charIndex)
                    {
                        DeleteLine(
                                    ref lineCounting,
                                    ref horizontalPosition,
                                    ref verticalPosition,
                                    ref startingLine,
                                    ref startingColumn,
                                    ref fileContent,
                                    charIndex);
                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                    }

                    break;

                case ConsoleKey.B:

                    Variables.Undo.Push(new Stack<(int, string)>());
                    Variables.UndoDeleteLine.Push(new Stack<bool>());
                    Variables.UndoAddLine.Push(new Stack<bool>());
                    Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                    Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                    startingChar = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                    for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                    {
                        CursorMovement.MoveWordLeft(keyInfo.KeyChar, ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                    }

                    endCharIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                    charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                    while (startingChar != charIndex)
                    {
                        CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                    }

                    while (charIndex != endCharIndex)
                    {
                        DeleteLine(
                                     ref lineCounting,
                                     ref horizontalPosition,
                                     ref verticalPosition,
                                     ref startingLine,
                                     ref startingColumn,
                                     ref fileContent,
                                     charIndex);
                        charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                    }

                    break;

                case ConsoleKey.F:

                    Variables.Undo.Push(new Stack<(int, string)>());
                    Variables.UndoDeleteLine.Push(new Stack<bool>());
                    Variables.UndoAddLine.Push(new Stack<bool>());
                    Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                    Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                    startingChar = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                    for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                    {
                        character = Controller.ReadChar(ref character);

                        CursorMovement.GoOnDesiredCharacter(keyInfo.KeyChar, lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);

                    }

                    charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                    if (startingChar < charIndex)
                    {
                        while (startingChar != charIndex - 1)
                        {
                            DeleteLine(
                                        ref lineCounting,
                                        ref horizontalPosition,
                                        ref verticalPosition,
                                        ref startingLine,
                                        ref startingColumn,
                                        ref fileContent,
                                        charIndex);
                            charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        }
                    }
                    else
                    {
                        endCharIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        while (startingChar != charIndex)
                        {
                            CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        }

                        while (charIndex != endCharIndex)
                        {
                            DeleteLine(
                                         ref lineCounting,
                                         ref horizontalPosition,
                                         ref verticalPosition,
                                         ref startingLine,
                                         ref startingColumn,
                                         ref fileContent,
                                         charIndex);
                            charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        }
                    }

                    break;

                case ConsoleKey.T:

                    Variables.Undo.Push(new Stack<(int, string)>());
                    Variables.UndoDeleteLine.Push(new Stack<bool>());
                    Variables.UndoAddLine.Push(new Stack<bool>());
                    Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                    Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                    startingChar = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                    for (int i = 1; i <= Convert.ToInt32(numberOfMoves); i++)
                    {
                        character = Controller.ReadChar(ref character);

                        CursorMovement.GoTillDesiredCharacter(keyInfo.KeyChar, lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn, character);

                    }

                    charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                    if (startingChar < charIndex)
                    {
                        while (startingChar != charIndex - 1)
                        {
                            DeleteLine(
                                        ref lineCounting,
                                        ref horizontalPosition,
                                        ref verticalPosition,
                                        ref startingLine,
                                        ref startingColumn,
                                        ref fileContent,
                                        charIndex);
                            charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        }
                    }
                    else
                    {
                        endCharIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        while (startingChar != charIndex)
                        {
                            CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                            charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        }

                        while (charIndex != endCharIndex)
                        {
                            DeleteLine(
                                         ref lineCounting,
                                         ref horizontalPosition,
                                         ref verticalPosition,
                                         ref startingLine,
                                         ref startingColumn,
                                         ref fileContent,
                                         charIndex);
                            charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                        }
                    }

                    break;

            }

            if (keyInfo.KeyChar == '^')
            {
                Variables.Undo.Push(new Stack<(int, string)>());
                Variables.UndoDeleteLine.Push(new Stack<bool>());
                Variables.UndoAddLine.Push(new Stack<bool>());
                Variables.InfoToShowUndo.Push((lineCounting, startingLine, startingColumn));
                Variables.CursorPositionUndo.Push((horizontalPosition, verticalPosition));
                startingChar = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                CursorMovement.MoveToFirstCharacter(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                endCharIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

                while (startingChar != charIndex)
                {
                    CursorMovement.NavigateRight(ref lineCounting, ref horizontalPosition, ref verticalPosition, ref startingLine, ref startingColumn);

                    charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                }

                while (charIndex != endCharIndex)
                {
                    DeleteLine(
                                 ref lineCounting,
                                 ref horizontalPosition,
                                 ref verticalPosition,
                                 ref startingLine,
                                 ref startingColumn,
                                 ref fileContent,
                                 charIndex);
                    charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);
                }
            }
        }

        public static void DeleteTilTheEnd(
                                    ref int lineCounting,
                                    ref int horizontalPosition,
                                    ref int verticalPosition,
                                    ref int startingLine,
                                    ref int startingColumn,
                                    ref string[] fileContent)
        {
            bool fastTravelMode = Config.FastTravel;
            int charIndex = Controller.GetCursorCharIndex(lineCounting, ref horizontalPosition, startingColumn, fileContent);

            Variables.UndoDeleteLine.Peek().Push(false);
            Variables.Undo.Peek().Push((lineCounting, fileContent[lineCounting]));
            fileContent[lineCounting] = fileContent[lineCounting].Remove(charIndex + 1);
            CursorMovement.EndButtonBehaviour(lineCounting, ref horizontalPosition, verticalPosition, startingLine, ref startingColumn);
            Consola.ShowContentOfFile(fileContent, lineCounting, fastTravelMode, startingLine, startingColumn);
            Console.SetCursorPosition(horizontalPosition, verticalPosition);
        }
    }
}
