using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace CodeEditor
{
    public class Consola
    {
        private const string grammer = "libtree-sitter-c_sharp";

        static Consola()
        {
            LibraryChooser();
        }

        public static TSLanguage lang = new TSLanguage(tree_sitter_c_sharp());

        [DllImport(grammer, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr tree_sitter_c_sharp();

        public static void ShowContentOfFile(string[] file, int currentLine, bool fastTravelMode, int startingLine = 0, int startingColumn = 0)
        {
            Console.CursorVisible = false;
            int visibleAreaWidth = Console.WindowWidth;
            int visibleAreaHight = Console.WindowHeight - 3;
            int lineNumber;
            string line;
            string lineIndex;
            int currentEndColumn;
            int currentStartColumn;
            var theme = new Theme();
            using var parser = new TSParser();

            NullOrEmptyCases.ArgumentNullException(file);

            string maximumNumberOfLines = Convert.ToString(file.Length - 1);

            ClearConsole();


            for (int i = startingLine; i < Math.Min(file.Length, startingLine + visibleAreaHight); i++)
            {
                line = file[i];
                lineNumber = i;
                lineIndex = GenerateLineIndex(fastTravelMode, currentLine, lineNumber, Convert.ToString(file.Length)) + " ";
                currentStartColumn = Math.Max(0, Math.Min(startingColumn, line.Length));
                currentEndColumn = line.Length - currentStartColumn <= visibleAreaWidth - lineIndex.Length ? line.Length - currentStartColumn : visibleAreaWidth - lineIndex.Length;
                if (i < Math.Min(file.Length, startingLine + visibleAreaHight) - 1)
                {
                    WriteIndex(lineNumber, lineIndex, currentLine, fastTravelMode);
                    ParseTree(line.Substring(currentStartColumn, currentEndColumn), parser, theme);
                    Console.WriteLine();
                }
                else
                {
                    WriteIndex(lineNumber, lineIndex, currentLine, fastTravelMode);
                    ParseTree(line.Substring(currentStartColumn, currentEndColumn), parser, theme);
                }
            }

            Console.CursorVisible = true;
        }

        public static void ClearConsole()
        {
            for (int i = Console.WindowHeight - 3; i >= 0; i--)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, 0);
        }

        public static string GenerateLineIndex(bool fastTravelMode, int currentLine, int lineNumber, string maximumNumberOfLines)
        {
            NullOrEmptyCases.ArgumentNullException(maximumNumberOfLines);
            string lineIndex = Convert.ToString(lineNumber);
            if (fastTravelMode)
            {
                if (lineNumber != currentLine)
                {
                    lineIndex = Convert.ToString(Math.Abs(currentLine - lineNumber));

                    while (maximumNumberOfLines.Length > lineIndex.Length)
                    {
                        lineIndex = " " + lineIndex;
                    }

                    return lineIndex;
                }

                while (maximumNumberOfLines.Length > lineIndex.Length)
                {
                    if (lineIndex.Length == maximumNumberOfLines.Length)
                    {
                        lineIndex = lineIndex + " ";
                        break;
                    }

                    lineIndex = " " + lineIndex;
                }

                return lineIndex;
            }

            lineIndex = Convert.ToString(lineNumber + 1);
            while (maximumNumberOfLines.Length > lineIndex.Length)
            {
                lineIndex = " " + lineIndex;
            }

            return lineIndex;
        }

        public static void ShowDirectoryContent(string[] fileFromDirectory)
        {
            const int searchBarDim = 4;
            SearchContour();
            int startingLine = Console.WindowHeight - (searchBarDim + 1);
            if (fileFromDirectory == null)
            {
                return;
            }

            ClearResultsWindow();
            for (int i = 0; i < fileFromDirectory.Length && startingLine != 0; i++)
            {
                Console.Write("  " + Path.GetFileName(fileFromDirectory[i]));
                startingLine--;
                Console.SetCursorPosition(1, startingLine);
            }
        }

        public static void ShowValidResults(List<string> validFiles, int numberOfValidFiles, string search, string[] totalNumberOfFiles)
        {
            NullOrEmptyCases.ArgumentNullException(validFiles);
            NullOrEmptyCases.ArgumentNullException(totalNumberOfFiles);
            NullOrEmptyCases.ArgumentNullException(search);

            int corsorLeftPosition;
            const int searchBarDim = 4;
            const int spaceBeforeName = 3;
            int startingLine = Console.WindowHeight - (searchBarDim + 1);

            ClearResultsWindow();

            for (int i = 0; i < numberOfValidFiles && startingLine != 0; i++)
            {
                int startingIndex = Path.GetFileName(validFiles[i]).IndexOf(search, StringComparison.OrdinalIgnoreCase);
                if (startingIndex >= 0)
                {
                    string firstSection = Path.GetFileName(validFiles[i]).Substring(0, startingIndex);
                    string colored = Path.GetFileName(validFiles[i]).Substring(firstSection.Length, search.Length);
                    string lastSection = Path.GetFileName(validFiles[i]).Substring(startingIndex + search.Length);
                    Console.Write("  " + firstSection);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(colored);
                    Console.ResetColor();
                    Console.Write(lastSection);
                    startingLine--;
                    Console.SetCursorPosition(1, startingLine);
                }
                else
                {
                    Console.SetCursorPosition(spaceBeforeName, Console.CursorTop);
                    HilightChar(validFiles[i], search);
                    startingLine--;
                    Console.SetCursorPosition(spaceBeforeName, startingLine);
                }
            }

            corsorLeftPosition = Console.WindowWidth - (Convert.ToString(totalNumberOfFiles.Length).Length + Convert.ToString(numberOfValidFiles).Length + 2);
            Console.SetCursorPosition(search.Length + 1, Console.WindowHeight - 2);
            Console.Write(new string(' ', Console.WindowWidth - (2 + search.Length)));
            Console.SetCursorPosition(corsorLeftPosition, Console.WindowHeight - 2);
            Console.Write(numberOfValidFiles + "/" + totalNumberOfFiles.Length);
        }

        public static void ClearResultsWindow()
        {
            const int searchBarDim = 4;
            int startingLine = Console.WindowHeight - (searchBarDim + 1);
            Console.SetCursorPosition(0, startingLine);
            for (int i = startingLine; i > 0; i--)
            {
                Console.SetCursorPosition(1, i);
                Console.Write(new string(' ', Console.WindowWidth - 2));
            }

            Console.SetCursorPosition(1, startingLine);
        }

        public static void SearchContour()
        {
            const int lowerLineResults = 3;
            for (int i = 0; i <= Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            Console.SetCursorPosition(0, 0);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth - 1, 0);
            Console.Write("┐");
            Console.SetCursorPosition(1, 0);
            Console.Write(new string('─', Console.WindowWidth - 2));
            Console.SetCursorPosition(0, Console.WindowHeight - (1 + lowerLineResults));
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - (1 + lowerLineResults));
            Console.Write("┘");
            Console.SetCursorPosition(1, Console.WindowHeight - (1 + lowerLineResults));
            Console.Write(new string('─', Console.WindowWidth - 2));
            Console.SetCursorPosition(0, Console.WindowHeight - lowerLineResults);
            Console.Write("┌");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - lowerLineResults);
            Console.Write("┐");
            Console.SetCursorPosition(1, Console.WindowHeight - lowerLineResults);
            Console.Write(new string('─', Console.WindowWidth - 2));
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("└");
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            Console.Write("┘");
            Console.SetCursorPosition(1, Console.WindowHeight - 1);
            Console.Write(new string('─', Console.WindowWidth - 2));
        }

        public static void CommandModeContour()
        {
            const int topLane = 10;
            int bottomLane = Console.WindowHeight - 10;
            const int leftLane = 20;
            int rightLane = Console.WindowWidth - 20;
            const string header = "Command Line";

            for (int i = topLane; i <= bottomLane; i++)
            {
                Console.SetCursorPosition(leftLane, i);
                Console.Write("│");
                Console.SetCursorPosition(leftLane + 1, i);
                Console.Write(new string(' ', rightLane - leftLane));
                Console.SetCursorPosition(rightLane, i);
                Console.Write("│");
            }

            Console.SetCursorPosition(leftLane, topLane);
            Console.Write("┌");
            Console.SetCursorPosition(rightLane, topLane);
            Console.Write("┐");
            Console.SetCursorPosition(leftLane + 1, topLane);
            Console.Write(header);
            Console.SetCursorPosition(leftLane + header.Length + 1, topLane);
            Console.Write(new string('─', rightLane - leftLane - 1 - header.Length));
            Console.SetCursorPosition(leftLane, bottomLane - 3);
            Console.Write("└");
            Console.SetCursorPosition(rightLane, bottomLane - 3);
            Console.Write("┘");
            Console.SetCursorPosition(leftLane + 1, bottomLane - 3);
            Console.Write(new string('─', rightLane - leftLane - 1));

            Console.SetCursorPosition(leftLane, bottomLane - 2);
            Console.Write("┌");
            Console.SetCursorPosition(rightLane, bottomLane - 2);
            Console.Write("┐");
            Console.SetCursorPosition(leftLane + 1, bottomLane - 2);
            Console.Write(new string('─', rightLane - leftLane - 1));
            Console.SetCursorPosition(leftLane, bottomLane);
            Console.Write("└");
            Console.SetCursorPosition(rightLane, bottomLane);
            Console.Write("┘");
            Console.SetCursorPosition(leftLane + 1, bottomLane);
            Console.Write(new string('─', rightLane - leftLane - 1));

            Console.SetCursorPosition(leftLane + 1, bottomLane - 1);
        }

        public static void Status(bool editMode, int horizontalPosition, int verticalPosition, int lineCounting, int startingColumn, string[] fileContent, string originalPath)
        {
            NullOrEmptyCases.ArgumentNullException(fileContent);

            bool fastTravelMode = Config.FastTravel;

            string lineIndex = Consola.GenerateLineIndex(fastTravelMode, lineCounting, lineCounting, Convert.ToString(fileContent.Length)) + " ";
            int currentEndColumn = NullOrEmptyCases.CurrentEndColumn(lineCounting, startingColumn, fileContent).Item2;

            if (horizontalPosition >= currentEndColumn + lineIndex.Length)
            {
                horizontalPosition = currentEndColumn + lineIndex.Length;
            }

            int topLane = Console.WindowHeight - 3;
            int currentColumn = horizontalPosition + startingColumn;
            string horizontal = Convert.ToString(currentColumn);
            string vertical = Convert.ToString(lineCounting);

            Console.SetCursorPosition(1, topLane + 1);

            if (editMode)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write(new string(' ', Console.WindowWidth - 1));
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(1, topLane + 1);
                Console.Write("INS " + Path.GetFileName(originalPath));
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(new string(' ', Console.WindowWidth - 1));
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(1, topLane + 1);
                Console.Write("NOR " + Path.GetFileName(originalPath));
            }

            Console.SetCursorPosition((Console.WindowWidth - 1) - (horizontal.Length + vertical.Length + 1), topLane + 1);
            Console.Write(horizontal + '/' + vertical);

            Console.ResetColor();
            Console.SetCursorPosition(horizontalPosition, verticalPosition);
        }

        private static void HilightChar(string file, string search)
        {
            int subSearchLength;
            string restOfSearch = "";
            string subSearch;
            if (search.Length == 1)
            {
                subSearchLength = search.Length;
                subSearch = search.Substring(0, subSearchLength);
            }
            else
            {
                subSearchLength = search.Length - 1;
                subSearch = search.Substring(0, subSearchLength);
                restOfSearch = search.Substring(search.Length - 1);
            }

            int startingIndex = Path.GetFileName(file).IndexOf(subSearch, StringComparison.OrdinalIgnoreCase);
            string restOfValid = Path.GetFileName(file).Substring(startingIndex + subSearch.Length);
            bool presentOfChar = ContainAllChar(restOfSearch, restOfValid);
            while (startingIndex < 0 || !presentOfChar)
            {
                restOfSearch = subSearch.Substring(subSearch.Length - 1) + restOfSearch;
                subSearch = subSearch.Substring(0, subSearch.Length - 1);
                startingIndex = Path.GetFileName(file).IndexOf(subSearch, StringComparison.OrdinalIgnoreCase);
                restOfValid = Path.GetFileName(file).Substring(startingIndex + subSearch.Length);
                presentOfChar = ContainAllChar(restOfSearch, restOfValid);
            }

            string firstSection = Path.GetFileName(file).Substring(0, startingIndex);
            string colored = Path.GetFileName(file).Substring(firstSection.Length, subSearch.Length);
            Console.Write(firstSection);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(colored);
            Console.ResetColor();

            if (search.Length == 1)
            {
                Console.Write(Path.GetFileName(file).Substring(startingIndex + subSearch.Length));
                return;
            }

            if (search.Length > 1)
            {
                file = Path.GetFileName(file).Substring(startingIndex + subSearch.Length);
                search = search.Substring(subSearch.Length);
            }

            HilightChar(file, search);
        }

        private static bool ContainAllChar(string search, string fileName)
        {
            int charIndex = 0;

            foreach (char c in fileName)
            {
                if (charIndex < search.Length && char.ToLower(c) == char.ToLower(search[charIndex]))
                {
                    charIndex++;
                }
            }

            return charIndex == search.Length;
        }

        private static void WriteIndex(int lineNumber, string lineIndex, int currentLine, bool fastTravelMode)
        {
            if (lineNumber == currentLine && fastTravelMode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(lineIndex);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(lineIndex);
            }

            Console.ResetColor();
        }

        private static bool ParseTree(string filetext, TSParser parser, Theme theme)
        {
            parser.set_language(Consola.lang);

            using var tree = parser.parse_string(null!, filetext);

            if (tree == null)
            {
                return false;
            }

            using var cursor = new TSCursor(tree.root_node(), lang);

            var rootNode = tree.root_node();
            var nodes = new List<HighlightedNode>();

            SyntaxHighlight(rootNode, filetext, theme);

            return true;
        }

        public static void SyntaxHighlight(TSNode rootNode, string code, Theme theme)
        {
            var themeColors = theme.ThemeColors;
            var nodes = new List<HighlightedNode>();
            AddNodes(rootNode, nodes, code);

            nodes.Sort((a, b) => a.StartByte.CompareTo(b.StartByte));

            HighlightChooser(code, nodes, themeColors);
        }

        private static void AddNodes(TSNode node, List<HighlightedNode> nodes, string fileContent)
        {
            string nodeType = node.type();

            if (Theme.HighlightedNodeTypes.Contains(nodeType))
            {
                nodes.Add(new HighlightedNode
                {
                    Type = nodeType,
                    StartByte = node.start_offset(),
                    EndByte = node.end_offset()
                });
            }

            uint childCount = node.child_count();
            for (uint i = 0; i < childCount; i++)
            {
                TSNode child = node.child(i);
                AddNodes(child, nodes, fileContent);
            }
        }

        private static void HighlightChooser(string fileText, List<HighlightedNode> nodes, Dictionary<string, ConsoleColor> themeColors)
        {
            int currentPos = 0;

            foreach (var node in nodes)
            {
                if (node.StartByte > currentPos)
                {
                    string unhighlighted = fileText.Substring(currentPos, (int)(node.StartByte - currentPos));
                    Console.Write(unhighlighted);
                    currentPos += unhighlighted.Length;
                }

                string highlighted = fileText.Substring((int)node.StartByte, (int)(node.EndByte - node.StartByte));
                HighlightApplier(node.Type, highlighted, themeColors);
                currentPos = (int)node.EndByte;
            }

            if (currentPos < fileText.Length)
            {
                string remaining = fileText.Substring(currentPos);
                Console.Write(remaining);
            }
        }

        private static void HighlightApplier(string nodeType, string codeFragment, Dictionary<string, ConsoleColor> themeColors)
        {

            string themeKey = GetColorForNodeType(nodeType);

            if (themeColors.TryGetValue(themeKey, out var color))
            {
                Console.ForegroundColor = color;
                Console.Write(codeFragment);
                Console.ResetColor();
            }
            else
            {
                Console.Write(codeFragment);
            }
        }

        private static string GetColorForNodeType(string nodeType)
        {
            switch (nodeType)
            {
                case "class":
                case "struct_declaration":
                case "interface_declaration":
                case "enum_declaration":
                case "predefined_type":
                case "implicit_type":
                case "modifier":
                case "namespace":
                case "using":
                    return "Keywords";
                case "type_identifier":
                case "if":
                case "continue":
                case "foreach":
                case "for":
                case "else":
                case "while":
                case "new":
                case "return":
                case "break":
                case "do":
                case "in":
                    return "Statement";
                case "number_literal":
                case "integer_literal":
                case "float_literal":
                case "string_literal":
                case "verbatim_string_literal":
                    return "Characters/Strings";
                case "comment":
                    return "Comments";
                case "identifier":
                    return "Identifiers";
                default:
                    return "default";
            }
        }

        private static void LibraryChooser()
        {
            string treeSitterLib;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                treeSitterLib = $"{grammer}.dll"; ;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                treeSitterLib = $"{grammer}.so";
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not supported.");
            }
        }

        public static void WriteIndexWithoutLine(string[] file, int currentLine, bool fastTravelMode, int startingLine)
        {
            string lineIndex;

            for (int i = startingLine; i < Math.Min(file.Length, startingLine + Console.WindowHeight - 3); i++)
            {
                lineIndex = GenerateLineIndex(fastTravelMode, currentLine, i, Convert.ToString(file.Length)) + " ";

                if (i == currentLine && fastTravelMode)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(lineIndex);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(lineIndex);
                }

                if (i < Math.Min(file.Length, startingLine + Console.WindowHeight - 3) - 1)
                {
                    Console.WriteLine();
                }

            }

            Console.ResetColor();
        }
    }
}