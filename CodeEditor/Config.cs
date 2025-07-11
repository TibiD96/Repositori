﻿namespace CodeEditor
{
    public class Config
    {
        public static bool FastTravel { get; } = true;

        public static List<string> Commands { get; } = ["edit", "quit", "write"];

        public static bool TabCompletion { get; set; } = false;
    }
}
