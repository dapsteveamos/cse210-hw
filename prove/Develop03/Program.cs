// Exceeded requirements by I implementing a library of scriptures that loads from a text file and randomly selects one for display, 
// enhancing user experience and providing variety in scripture memorization.


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        // Load scriptures from a file
        List<Scripture> scriptures = LoadScripturesFromFile("scriptures.txt");
        
        if (scriptures.Count == 0)
        {
            Console.WriteLine("No scriptures found. Exiting.");
            return;
        }

        // Select a random scripture
        Random random = new Random();
        Scripture scripture = scriptures[random.Next(scriptures.Count)];

        while (true)
        {
            Console.Clear();
            scripture.Display();
            Console.WriteLine("Press Enter to hide words or type 'quit' to exit.");

            string input = Console.ReadLine();
            if (input.ToLower() == "quit")
                break;

            scripture.HideRandomWords(1); // Hide one word at a time
        }
    }

    // Method to load scriptures from a text file
  private static List<Scripture> LoadScripturesFromFile(string filePath)
{
    List<Scripture> scriptures = new List<Scripture>();

    try
    {
        // Read all lines from the file
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            // Each line should be formatted as "Reference|Text"
            var parts = line.Split('|');
            if (parts.Length == 2)
            {
                // Create Reference and Scripture objects
                Reference reference = new Reference(parts[0]);
                scriptures.Add(new Scripture(reference, parts[1]));
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error reading file: {ex.Message}");
    }

    return scriptures;
}

}
