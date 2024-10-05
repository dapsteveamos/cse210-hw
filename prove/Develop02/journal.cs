using System;
using System.Collections.Generic;
using System.IO;

public class Journal
{
    private List<Entry> _entries = new List<Entry>(); // Adjusted to _underscoreCamelCase

    public void AddEntry(string prompt, string response)
    {
        Entry newEntry = new Entry
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            Prompt = prompt,
            Response = response
        };
        _entries.Add(newEntry);
    }

    public void DisplayEntries()
    {
        if (_entries.Count == 0)
        {
            Console.WriteLine("No entries to display.");
            return;
        }

        foreach (var entry in _entries)
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
                foreach (Entry entry in _entries)
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
                _entries.Clear();
                using (StreamReader inputFile = new StreamReader(filename))
                {
                    string line;
                    while ((line = inputFile.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 3)
                        {
                            Entry loadedEntry = new Entry
                            {
                                Date = parts[0].Trim(),
                                Prompt = parts[1].Trim(),
                                Response = parts[2].Trim()
                            };
                            _entries.Add(loadedEntry);
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
