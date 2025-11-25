using CalculatorLibrary;
using System.Text.RegularExpressions;

namespace CalculatorProgram
{
    class Program
    {
        public Calculator calculator;
        Program()
        {
            calculator = new();
        }

        void printHistory()
        {
            var history = calculator.GetHistory();
            for (int i = 0; i < history.Count; i++)
            {
                Operation op = history.ElementAt(i);
                Console.WriteLine($"[{i}]- {op.Num1} {op.Operand} {op.Num2} = {op.result}");
            }

            Console.WriteLine("Obs: type uN (where N is a integer) to use the results above in your operations");
            Console.WriteLine("\n");
        }

        bool ParseInput(out double value)
        {
            value = 0;
            string input = Console.ReadLine() ?? "";
            if (input.Length == 0) return false;
            if (input[0] == 'h')
            {
                printHistory();
                return false;
            }
            else if (input[0] == 'u')
            {
                if (int.TryParse(input.Substring(1, 1), out int index))
                {
                    var history = calculator.GetHistory();
                    if (index < history.Count)
                    {
                        value = calculator.GetHistory().ElementAt(index).result;
                        Console.WriteLine($"...used value {value} from history.");
                        return true;
                    }
                    else return false;
                }
            }
            else if (double.TryParse(input, out value))
            {
                return true;
            }
            return false;
        }

        static public void Main(string[] args)
        {
            Program app = new();

            bool endApp = false;
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine($"Calculator has been used {app.calculator.getOpenedCount()} times");
            Console.WriteLine("------------------------\n");

            while (!endApp)
            {
                double result = 0;

                // Ask the user to type the first number.
                Console.Write("Type a number, or type 'h' to see history.");
                double cleanNum1 = 0;
                while (!app.ParseInput(out cleanNum1))
                {
                    Console.Write("This is not valid input. Please enter an integer value: ");
                }

                // Ask the user to type the second number.
                Console.Write("Type another number, and then press Enter: ");
                double cleanNum2 = 0;
                while (!app.ParseInput(out cleanNum2))
                {
                    Console.Write("This is not valid input. Please enter an integer value: ");
                }

                // Ask the user to choose an operator.
                Console.WriteLine("Choose an operator from the following list:");
                Console.WriteLine("\ta - Add");
                Console.WriteLine("\ts - Subtract");
                Console.WriteLine("\tm - Multiply");
                Console.WriteLine("\td - Divide");
                Console.Write("Your option? ");

                string? op = Console.ReadLine();

                // Validate input is not null, and matches the pattern
                if (op == null || !Regex.IsMatch(op, "[a|s|m|d]"))
                {
                    Console.WriteLine("Error: Unrecognized input.");
                }
                else
                {
                    try
                    {
                        result = app.calculator.DoOperation(cleanNum1, cleanNum2, op);
                        if (double.IsNaN(result))
                        {
                            Console.WriteLine("This operation will result in a mathematical error.\n");
                        }
                        else Console.WriteLine("Your result: {0:0.##}\n", result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                    }
                }
                Console.WriteLine("------------------------\n");

                // Wait for the user to respond before closing.
                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                if (Console.ReadLine() == "n") endApp = true;

                Console.WriteLine("\n"); // Friendly linespacing.
            }
            return;
        }
    }
}