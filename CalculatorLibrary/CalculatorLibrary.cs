// OK! Create a functionality that will count the amount of times the calculator was used.
// Ok! Store a list with the latest calculations. And give the users the ability to delete that list.
// OK! Allow the users to use the results in the list above to perform new calculations.
// OK! Add extra calculations: Square Root, Taking the Power, 10x, Trigonometry functions.

using System.Text.Json;
using System.IO;

namespace CalculatorLibrary
{
    public class Calculator
    {
        const string logPath = "../../../calculatorlog.json";
        private CalculatorLog log;

        public Calculator()
        {
            string jsonString = File.Exists(logPath) ? File.ReadAllText(logPath) : "{}";
            log = JsonSerializer.Deserialize<CalculatorLog>(jsonString) ?? new CalculatorLog();
            log.openedCount++;
            WriteLog();
        }

        public int getOpenedCount()
        {
            return log.openedCount;
        }

        public List<Operation> GetHistory()
        {
            return log.history;
        }

        void WriteLog()
        {
            int len = log.history.Count;
            // Only write the last 5 elements, both to avoid cluttering the screen and logs too large
            if (len > 5)
                log.history = log.history.GetRange(len - 6, len - 1);
            Console.WriteLine(JsonSerializer.Serialize(log));
            Console.WriteLine(log.openedCount);
            File.WriteAllText(logPath, JsonSerializer.Serialize<CalculatorLog>(log));
        }

        public double DoOperation(double num1, double num2, string op)
        {
            Operation operand = new();
            operand.result = double.NaN;
            operand.Num1 = num1;
            operand.Num2 = num2;
            log.history.Add(operand);

            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    operand.result = num1 + num2;
                    operand.Operand = "Add";
                    break;
                case "s":
                    operand.result = num1 - num2;
                    operand.Operand = "Subtract";
                    break;
                case "m":
                    operand.result = num1 * num2;
                    operand.Operand = "Multiply";
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        operand.result = num1 / num2;
                        operand.Operand = "Divide";
                    }
                    else
                    {
                        log.history.RemoveAt(log.history.Count - 1); // remove from history
                    }
                    break;
                case "p":
                    operand.result = Math.Pow(num1, num2);
                    operand.Operand = "Power";
                    break;
                case "q":
                    if (num1 > 0)
                    {
                        operand.result = Math.Sqrt(num1);
                        operand.Operand = "Square Root";
                    }
                    else
                    {
                        log.history.RemoveAt(log.history.Count - 1);
                    }
                    break;
                case "c":
                    operand.result = Math.Cos(num1);
                    operand.Operand = "Consine";
                    break;
                case "i":
                    operand.result = Math.Sin(num1);
                    operand.Operand = "Consine";
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }
            WriteLog();

            return operand.result;
        }
    }
    public class Operation
    {
        public double Num1 { get; set; }
        public double Num2 { get; set; }
        public string? Operand { get; set; }
        public double result { get; set; }
    }
    public class CalculatorLog
    {
        public int openedCount { get; set; }
        public List<Operation> history { get; set; } = [];
    }
}