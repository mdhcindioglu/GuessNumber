using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuessNumber
{
    public class MainService
    {
        //Human Number Possibilities
        public List<string> Possibilities;
        //Human Selected Number
        public string HumanSelectedNumber { get; }
        //Pc Selected Number
        public string PcSelectedNumber { get; }
        //Current Turn
        public Turn CurrentTurn { get; set; }
        //Who Win
        public Win WhoWin { get; set; }

        /// <summary>
        ///MainService Constructor
        /// </summary>
        /// <param name="myNumber">Human Selected Number</param>
        public MainService(string myNumber)
        {
            Possibilities = GetPossibilities();
            HumanSelectedNumber = myNumber;
            PcSelectedNumber = GetPcRandumNumber();
            CurrentTurn = GetHowStart;
        }

        /// <summary>
        /// Determine who will start
        /// </summary>
        /// <returns>Turn.Pc or Turn.Human</returns>
        private Turn GetHowStart => (Turn)new Random().Next(0, 2);

        /// <summary>
        /// Compare possibility number and PcSelectedNumber
        /// </summary>
        /// <param name="possibility">possibility number</param>
        /// <returns>Result of comparation (ط ت ط ط)</returns>
        public string GetResultFromPc(string possibility)
        {
            return GetResult(possibility, PcSelectedNumber);
        }

        /// <summary>
        /// Compare possibility number and HumanSelectedNumber
        /// </summary>
        /// <param name="possibility"></param>
        /// <returns>Result of comparation (ط ت ط ط)</returns>
        public string GetResultFromHuman(string possibility)
        {
            return GetResult(possibility, HumanSelectedNumber);
        }

        /// <summary>
        /// Compare possibility number and target number
        /// </summary>
        /// <param name="possibility">possibility number</param>
        /// <param name="target">target number</param>
        /// <returns>Result of comparation (ط ت ط ط)</returns>
        private string GetResult(string possibility, string target)
        {
            var result = string.Empty;

            if (possibility[0] == target[0])
                result += "A";
            else if (possibility[0] == target[1] || possibility[0] == target[2] || possibility[0] == target[3])
                result += "Y";

            if (possibility[1] == target[1])
                result += "A";
            else if (possibility[1] == target[0] || possibility[1] == target[2] || possibility[1] == target[3])
                result += "Y";

            if (possibility[2] == target[2])
                result += "A";
            else if (possibility[2] == target[0] || possibility[2] == target[1] || possibility[2] == target[3])
                result += "Y";

            if (possibility[3] == target[3])
                result += "A";
            else if (possibility[3] == target[0] || possibility[3] == target[1] || possibility[3] == target[2])
                result += "Y";

            return result == string.Empty ? "-" : result;
        }

        
        /// <summary>
        /// Remove UnAccepted Possibilities from list
        /// </summary>
        /// <param name="possibility">Pc Possibility</param>
        /// <param name="result">Result of Pc Possibility</param>
        public void RemoveUnAcceptedPossibilities(string possibility, string result)
        {
            var acceptedPossibilities = new List<string>();

            foreach (var item in Possibilities)
                if (GetResult(possibility, item) == result)
                    acceptedPossibilities.Add(item);

            Possibilities = acceptedPossibilities;
        }

        /// <summary>
        /// Let Pc select random acceptable number (0123-9876)
        /// </summary>
        /// <returns>String Pc Number (4 Number UnRepeated)</returns>
        private string GetPcRandumNumber()
        {
            string pcNumber;

            do
            {
                pcNumber = string.Format("{0:0000}", new Random().Next(0123, 9877));
            } while (IsRepeated(pcNumber.ToString()));

            return pcNumber;
        }

        /// <summary>
        /// Get Randum Number From Pc Possibilities
        /// </summary>
        /// <returns>Randum number</returns>
        public string GetNumberFromPcPossibilities()
        {
            if (Possibilities.Count > 0)
                return Possibilities[new Random().Next(0, Possibilities.Count)];
            else
                return "1234";
        }

        /// <summary>
        /// Get Acceptable Possibilities
        /// </summary>
        /// <returns>A list of acceptable string values</returns>
        private static List<string> GetPossibilities()
        {
            var allPossibilities = new List<string>();

            //Fill all possibilities
            for (int a = 0; a < 10; a++)
                for (int b = 0; b < 10; b++)
                    for (int c = 0; c < 10; c++)
                        for (int d = 0; d < 10; d++)
                            allPossibilities.Add($"{a}{b}{c}{d}");

            //Remove unsutable values
            var possibilities = new List<string>();
            foreach (var choice in allPossibilities)
            {
                if (!IsRepeated(choice))
                    possibilities.Add(choice);
            }

            return possibilities;
        }

        /// <summary>
        /// Test if number has repeated number
        /// </summary>
        /// <param name="possibility">Possibility to test</param>
        /// <returns>True if has repeated and False if not</returns>
        private static bool IsRepeated(string possibility)
        {
            if (possibility[0] == possibility[1])
                return true;
            if (possibility[0] == possibility[2])
                return true;
            if (possibility[0] == possibility[3])
                return true;
            if (possibility[1] == possibility[2])
                return true;
            if (possibility[1] == possibility[3])
                return true;
            if (possibility[2] == possibility[3])
                return true;

            return false;
        }
    }

    public enum Turn
    {
        Human,
        Pc,
    }
    public enum Win
    {
        NoOne,
        Human,
        Pc,
    }
}
