using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            if (StringIsJSONValid(input) && input == "\"\"")
            {
                return true;
            }
            else if (StringIsJSONValid(input))
            {
                return ValidateString(input);
            }

            return false;
        }

        static bool ValidateString(string input)
        {
            if (!DontContainControlCharacter(input))
            {
                return false;
            }

            if (CheckEscapedCharacters(input))
            {
                return true;
            }

            if (CanContainLargeUnicodeCharacters(input))
            {
                return true;
            }

            if (!CheckToNotFinishWithAnUnifinishedHexNumber(input))
            {
                return false;
            }

            return input.StartsWith('"') && input.EndsWith('"');
        }

        static bool DontContainControlCharacter(string input)
        {
            string[] controlCharacters = { "\b", "\f", "\n", "\r", "\t" };
            const int asciiVerification = 32;
            for (int j = 0; j < controlCharacters.Length; j++)
            {
                    if (input.Contains(controlCharacters[j]))
                    {
                        return false;
                    }
            }

            foreach (char character in input)
            {
                if (character < asciiVerification)
                {
                    return false;
                }
            }

            return true;
        }

        static bool CheckEscapedCharacters(string input)
        {
            string[] escapedCharactersToCheck = { @"\""", @"\\", @"\/", @"\b", @"\f", @"\n", @"\r", @"\t", @"\u" };
            int i = input.IndexOf('\\');
            int nextElementAfterBackSlach = i + 1;
            int lengthOfUnicode = 0;
            const int corectLengthOfUnicode = 5;
            for (int j = 0; j < escapedCharactersToCheck.Length; j++)
            {
               if (input.Contains(escapedCharactersToCheck[j]) && nextElementAfterBackSlach != input.Length - 1 && j < escapedCharactersToCheck.Length - 1)
               {
                 return true;
               }
               else if (input.Contains(escapedCharactersToCheck[j]) && j == escapedCharactersToCheck.Length - 1)
                {
                    i++;
                    while (input[i] != ' ' && input[i] != '"')
                    {
                        i++;
                        lengthOfUnicode++;
                    }

                    if (lengthOfUnicode == corectLengthOfUnicode)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static bool CheckToNotFinishWithAnUnifinishedHexNumber(string input)
        {
            bool isHex;
            const int largeUnicodeAscii = 127;
            foreach (char element in input)
            {
                if (element < largeUnicodeAscii)
                {
                    return true;
                }
            }

            for (int i = input.Length - 2; i < input.Length; i++)
            {
                isHex = input[i] >= '0' && input[i] <= '9' || input[i] >= 'a' && input[i] <= 'f';
                if (!isHex)
                {
                    isHex = input[i] >= 'A' && input[i] <= 'F';

                    if (!isHex)
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }

            return true;
        }

        static bool CanContainLargeUnicodeCharacters(string input)
        {
            const int largeUnicodeAscii = 127;
            foreach (char element in input)
            {
                if (element < largeUnicodeAscii)
                {
                    return true;
                }
            }

            return false;
        }

        static bool StringIsJSONValid(string input)
        {
            try
            {
                JToken.Parse(input);
                return true;
            }
            catch (JsonReaderException wrong)
            {
                Console.WriteLine(wrong);
                return false;
            }
        }
    }
}
