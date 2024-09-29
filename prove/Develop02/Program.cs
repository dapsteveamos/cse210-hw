// EXCEEDING REQUIREMEN, I was able to save my file and let user know journal saved successfully
// I let user know option to choice from from "What do you want to do?"

using System;
using System.Collections.Generic;
using System.IO;

public class Entry
{
    public string Date { get; set; }
    public string Prompt { get; set; }
    public string Response { get; set; }
}

public class Journal
{
    private List<Entry> entries = new List<Entry>();

    public void AddEntry(string prompt, string response)
    {
        Entry newEntry = new Entry
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"), // Format the date as needed
            Prompt = prompt,
            Response = response
        };
        entries.Add(newEntry); // Add the new entry to the list
    }

    public void DisplayEntries()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("No entries to display.");
            return;
        }

        foreach (var entry in entries)
        {
            Console.WriteLine($"{entry.Date} | {entry.Prompt} | {entry.Response}");
        }
    }

    public void SaveJournal(string filename)
    {
        try
        {
            using (StreamWriter outputFile = new StreamWriter(filename))
            {
                foreach (Entry entry in entries)
                {
                    outputFile.WriteLine($"{entry.Date} | {entry.Prompt} | {entry.Response}");
                }
            }
            Console.WriteLine("Journal saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving: {ex.Message}");
        }
    }
// loading journal
    public void LoadJournal(string filename)
    {
        try
        {
            if (File.Exists(filename))
            {
                entries.Clear(); // Clear current entries before loading
                using (StreamReader inputFile = new StreamReader(filename))
                {
                    string line;
                    while ((line = inputFile.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|'); // Assuming the format is Date | Prompt | Response
                        if (parts.Length == 3)
                        {
                            Entry loadedEntry = new Entry
                            {
                                Date = parts[0].Trim(),
                                Prompt = parts[1].Trim(),
                                Response = parts[2].Trim()
                            };
                            entries.Add(loadedEntry);
                        }
                    }
                }
                Console.WriteLine("Journal loaded successfully!");
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading: {ex.Message}");
        }
    }
}

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
            "If I had one thing I could do over today, what would it be?"
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
            Console.WriteLine("What do you want to do? choice from (1-5)");
            string choice = Console.ReadLine();

// Answers and response to all choice choosen
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
                    Console.Write("Enter filename to save to: ");
                    string saveFile = Console.ReadLine();
                    journal.SaveJournal(saveFile);
                    break;
                case "4":
                    Console.Write("Enter filename to load from: ");
                    string loadFile = Console.ReadLine();
                    journal.LoadJournal(loadFile);
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
