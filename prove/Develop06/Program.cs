// Exceeding requirement addedDeadline/Expiry Feature: An optional due date can now be assigned to each goal. If a due date is set, 
// the program will notify the user when a goal is approaching its deadline (within 3 days) or has expired.

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        GoalManager manager = new GoalManager();
        manager.Start();
    }
}

public class GoalManager
{
    private List<Goal> _goals = new List<Goal>();
    private int _score;

    public GoalManager()
    {
        _score = 0;
    }

    public void Start()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nEternal Quest Program");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. List Goals");
            Console.WriteLine("3. Save Goals");
            Console.WriteLine("4. Load Goals");
            Console.WriteLine("5. Record Event");
            Console.WriteLine("6. Quit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateGoal();
                    break;
                case "2":
                    ListGoalDetails();
                    break;
                case "3":
                    SaveGoals();
                    break;
                case "4":
                    LoadGoals();
                    break;
                case "5":
                    RecordEvent();
                    break;
                case "6":
                    running = false;
                    Console.WriteLine("Exiting the program. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public void CreateGoal()
    {
        Console.WriteLine("Choose a goal type:");
        Console.WriteLine("1. Simple Goal");
        Console.WriteLine("2. Eternal Goal");
        Console.WriteLine("3. Checklist Goal");
        string choice = Console.ReadLine();

        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();

        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();

        Console.Write("Enter goal points: ");
        int points = int.Parse(Console.ReadLine());

        Console.Write("Enter due date (yyyy-mm-dd) or press Enter to skip: ");
        string dueDateInput = Console.ReadLine();
        DateTime? dueDate = null;
        if (!string.IsNullOrEmpty(dueDateInput))
        {
            dueDate = DateTime.Parse(dueDateInput);
        }

        switch (choice)
        {
            case "1":
                _goals.Add(new SimpleGoal(name, description, points, dueDate));
                break;
            case "2":
                _goals.Add(new EternalGoal(name, description, points, dueDate));
                break;
            case "3":
                Console.Write("Enter target completions: ");
                int target = int.Parse(Console.ReadLine());
                Console.Write("Enter bonus points: ");
                int bonus = int.Parse(Console.ReadLine());
                _goals.Add(new ChecklistGoal(name, description, points, target, bonus, dueDate));
                break;
            default:
                Console.WriteLine("Invalid choice. Goal creation failed.");
                break;
        }
    }

    public void ListGoalDetails()
    {
        Console.WriteLine("Goals:");
        foreach (var goal in _goals)
        {
            Console.WriteLine(goal.GetDetailsString());
        }
    }

    public void SaveGoals()
    {
        Console.Write("Enter filename to save goals: ");
        string filename = Console.ReadLine();

        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine(_score); // Save the current score
            foreach (var goal in _goals)
            {
                writer.WriteLine(goal.GetStringRepresentation());
            }
        }
        Console.WriteLine("Goals successfully saved!");
    }

    public void LoadGoals()
    {
        Console.Write("Enter filename to load goals: ");
        string filename = Console.ReadLine();

        if (File.Exists(filename))
        {
            _goals.Clear(); // Clear existing goals before loading new ones

            using (StreamReader reader = new StreamReader(filename))
            {
                _score = int.Parse(reader.ReadLine()); // Load the score first

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');
                    string goalType = parts[0];
                    string name = parts[1];
                    string description = parts[2];
                    int points = int.Parse(parts[3]);
                    DateTime? dueDate = string.IsNullOrEmpty(parts[4]) ? (DateTime?)null : DateTime.Parse(parts[4]);

                    if (goalType == "SimpleGoal")
                    {
                        bool isComplete = bool.Parse(parts[5]);
                        _goals.Add(new SimpleGoal(name, description, points, dueDate, isComplete));
                    }
                    else if (goalType == "EternalGoal")
                    {
                        _goals.Add(new EternalGoal(name, description, points, dueDate));
                    }
                    else if (goalType == "ChecklistGoal")
                    {
                        int amountCompleted = int.Parse(parts[5]);
                        int target = int.Parse(parts[6]);
                        int bonus = int.Parse(parts[7]);
                        _goals.Add(new ChecklistGoal(name, description, points, target, bonus, dueDate, amountCompleted));
                    }
                }
            }
            Console.WriteLine("Goals successfully loaded!");
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }

    public void RecordEvent()
    {
        Console.WriteLine("Select a goal to record an event:");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }

        int choice = int.Parse(Console.ReadLine());

        if (choice > 0 && choice <= _goals.Count)
        {
            Goal goal = _goals[choice - 1];
            goal.RecordEvent();
            _score += goal.Points;
        }
        else
        {
            Console.WriteLine("Invalid goal selection.");
        }
    }
}

public abstract class Goal
{
    protected string _shortName;
    protected string _description;
    protected int _points;
    protected DateTime? _dueDate;

    public Goal(string name, string description, int points, DateTime? dueDate = null)
    {
        _shortName = name;
        _description = description;
        _points = points;
        _dueDate = dueDate;
    }

    public abstract void RecordEvent();
    public abstract bool IsComplete();
    public abstract string GetStringRepresentation();

    // Set to protected so derived classes can access it
    protected string GetDeadlineAlert()
    {
        if (_dueDate.HasValue)
        {
            int daysRemaining = (int)(_dueDate.Value - DateTime.Now).TotalDays;

            if (daysRemaining < 0)
            {
                return " (Expired)";
            }
            else if (daysRemaining <= 3)
            {
                return $" (Due in {daysRemaining} days)";
            }
        }
        return "";
    }

    public virtual string GetDetailsString()
    {
        string deadlineAlert = GetDeadlineAlert();
        return $"{(IsComplete() ? "[X]" : "[ ]")} {_shortName} - {_description} : {_points} points {deadlineAlert}";
    }

    public int Points => _points;
}

public class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string name, string description, int points, DateTime? dueDate = null, bool isComplete = false)
        : base(name, description, points, dueDate)
    {
        _isComplete = isComplete;
    }

    public override void RecordEvent()
    {
        if (!_isComplete)
        {
            _isComplete = true;
            Console.WriteLine($"{_shortName} completed! +{_points} points.");
        }
        else
        {
            Console.WriteLine($"{_shortName} is already complete.");
        }
    }

    public override bool IsComplete()
    {
        return _isComplete;
    }

    public override string GetStringRepresentation()
    {
        return $"SimpleGoal|{_shortName}|{_description}|{_points}|{_dueDate?.ToString("yyyy-MM-dd")}|{_isComplete}";
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points, DateTime? dueDate = null)
        : base(name, description, points, dueDate)
    {
    }

    public override void RecordEvent()
    {
        Console.WriteLine($"Eternal Goal recorded! +{_points} points.");
    }

    public override bool IsComplete()
    {
        return false; // Eternal goals never complete
    }

    public override string GetStringRepresentation()
    {
        return $"EternalGoal|{_shortName}|{_description}|{_points}|{_dueDate?.ToString("yyyy-MM-dd")}";
    }
}

public class ChecklistGoal : Goal
{
    private int _amountCompleted;
    private int _target;
    private int _bonus;

    public ChecklistGoal(string name, string description, int points, int target, int bonus, DateTime? dueDate = null, int amountCompleted = 0)
        : base(name, description, points, dueDate)
    {
        _amountCompleted = amountCompleted;
        _target = target;
        _bonus = bonus;
    }

    public override void RecordEvent()
    {
        _amountCompleted++;
        Console.WriteLine($"Progress made! Completed {_amountCompleted}/{_target}.");

        if (_amountCompleted >= _target)
        {
            Console.WriteLine($"Checklist goal completed! +{_bonus} bonus points!");
            _points += _bonus;
        }
    }

    public override bool IsComplete()
    {
        return _amountCompleted >= _target;
    }

    public override string GetDetailsString()
    {
        string deadlineAlert = GetDeadlineAlert();
        return $"{(IsComplete() ? "[X]" : "[ ]")} {_shortName} - {_description} : Completed {_amountCompleted}/{_target}, {_points} points, Bonus: {_bonus} {deadlineAlert}";
    }

    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal|{_shortName}|{_description}|{_points}|{_dueDate?.ToString("yyyy-MM-dd")}|{_amountCompleted}|{_target}|{_bonus}";
    }
}
