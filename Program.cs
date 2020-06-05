using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace SD_18_ADDS_TO_HUNDRED
{
    class Program
    {
        static void Main()
        {
            string str = "12+-";

            var res = FindPermutations(str);
            WriteInRed("Wszystkie");
            PrintNumberOfElements(res);
            PrintAllElementsOfTheListThenWaitForKey(res, false, true);

            List<string> equations = ParseToEquationsStrings(res);
            WriteInRed("Zrownaniownione");
            PrintNumberOfElements(equations);
            PrintAllElementsOfTheListThenWaitForKey(equations, false, true);

            List<string> eqSolvesToHundred = TakeThoseEquationsThatSolvesTo(3, equations);
            WriteInRed("Te ktorych rozwiazaniem jest pozadana liczba");
            PrintNumberOfElements(eqSolvesToHundred);
            PrintAllElementsOfTheListThenWaitForKey(eqSolvesToHundred);
        }

        private static void WriteInRed(string msg)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine($"\t{msg}");
            ResetColor();
        }

        private static List<string> TakeThoseEquationsThatSolvesTo(int solvesTo, List<string> @possibleEquations)
        {
            List<string> solves = new List<string>();
            List<string> currentEquation = new List<string>();
            foreach (string equation in possibleEquations)
            {
                StringBuilder sb = new StringBuilder();
                char[] chars = equation.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    if (char.IsDigit(chars[i]))
                    {
                        sb.Append(chars[i]);
                        if (i == chars.Length - 1) { currentEquation.Add(sb.ToString()); }
                    }
                    else
                    {
                        if (sb.Length != 0)
                        {
                        currentEquation.Add(sb.ToString());
                        sb.Clear();
                        }
                        currentEquation.Add(chars[i].ToString());
                    }
                }

                int result = 0;
                char sign = '0';
                for (int i = 0; i < currentEquation.Count; i++)
                {

                    if (currentEquation[i].Contains("+") ||
                       currentEquation[i].Contains("-"))
                    {
                        sign = currentEquation[i].ToCharArray()[0];
                    }
                    else if (result == 0) result = int.Parse(currentEquation[i]);
                    else
                    {
                       result = Calculate(result, sign, int.Parse(currentEquation[i]));
                    }

                }

                if (result == solvesTo) { solves.Add(equation); }
                currentEquation.Clear();
            }


            return solves;
        }

        private static int Calculate(int result, char sign, int value)
        {
            switch (sign)
            {
                case '+':
                    result += value;
                    break;
                case '-':
                    result -= value;
                    break;
            }


            return result;
        }

        /// <summary>
        /// Parses passed permutations in a manner that returned strings look like written equations. Ready to be solved.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private static List<string> ParseToEquationsStrings(List<string> @res)
        {
            List<string> equations = new List<string>();
            foreach (var item in @res)
            {
                var test = ParseToEquationString(item);
                if (test.Item1)
                {
                    equations.Add(test.Item2);
                }
            }

            return equations;
        }

        private static void PrintNumberOfElements(List<string> res)
        {
            WriteLine(string.Concat("\tLiczba elementow = ", res.Count));
        }

        private static void PrintAllElementsOfTheListThenWaitForKey(List<string> @list, bool @wait = true, bool @carridgeReturn = true)
        {
            if (@carridgeReturn)
                @list.ForEach(x => { Write(x); WriteLine(); });
            else
                @list.ForEach(x => { Write(x); });
            Console.WriteLine();
            if (@wait)
                ReadKey();
        }

        private static (bool, string) ParseToEquationString(string @string)
        {
            List<char> equation = new List<char>();
            char[] chars = @string.ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < chars.Length; i++)
            {
                if (int.TryParse(chars[i].ToString(), out int parsed))
                {
                    sb.Append(parsed);
                    if (i == chars.Length - 1) equation.AddRange(sb.ToString().ToCharArray());
                }
                else
                {
                    if (sb.Length != 0)
                    {
                        equation.AddRange(sb.ToString().ToCharArray());
                        sb.Clear();
                    }

                    if (equation.Count == 0 && chars[i] == '-') { equation.Add(chars[i]); }
                    else
                    {
                        if (i != chars.Length - 1)
                        {
                            if (equation.Count > 0 && char.IsDigit(equation.Last()))
                            {
                                equation.Add(chars[i]);
                            }
                        }
                    }

                }
            }
            if (char.IsDigit(equation.Last())) { }
            else { equation.RemoveAt(equation.Count - 1); }



            sb.Clear();
            equation.ForEach(x => sb.Append(x));
            return (true, sb.ToString());
        }

        /// <summary>
        /// Generate all permutations
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        private static List<string> FindPermutations(string @string)
        {
            var output = new List<string>();
            if (@string.Length == 1)
            {
                output.Add(@string);
            }
            else
            {
                foreach (char c in @string)
                {
                    // Remove one occurrence of the char (not all)
                    var tail = @string.Remove(@string.IndexOf(c), 1);
                    foreach (var tailPerms in FindPermutations(tail))
                    {
                        output.Add(c + tailPerms);
                    }
                }
            }
            return output;
        }

    }
}