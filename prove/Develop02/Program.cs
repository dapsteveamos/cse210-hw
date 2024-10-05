// EXCEEDING REQUIREMEN, I was able to save my file and let user know journal saved successfully and i seperated all files 
// I let user know option to choice from from "What do you want to do?" i added an extra questionhave you you made your entry

        

using System;
using System.Collections.Generic;

public class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();
        List<string> prompts = new List<string>
        {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "have you you made your entry"
        };

        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a file");
            Console.WriteLine("4. Load the journal from a file");
            Console.WriteLine("5. Exit");
            // let user know option available
            Console.WriteLine("What do you want to do? Choice from (1-5)");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Random random = new Random();
                    string prompt = prompts[random.Next(prompts.Count)];
                    Console.WriteLine(prompt);
                    string response = Console.ReadLine();
                    journal.AddEntry(prompt, response);
                    break;
                case "2":
                    journal.DisplayEntries();
                    break;
                case "3":
                    Console.Write("Enter filename (without path, saved in the project folder): ");
                    string fileName = Console.ReadLine();
                    string savePath = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
                    journal.SaveJournal(savePath);
                    break;
                case "4":
                    Console.Write("Enter filename to load from (in the project folder): ");
                    string loadFile = Console.ReadLine();
                    string loadPath = System.IO.Path.Combine(Environment.CurrentDirectory, loadFile);
                    journal.LoadJournal(loadPath);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice, please select again.");
                    break;
            }
        }
    }
}
