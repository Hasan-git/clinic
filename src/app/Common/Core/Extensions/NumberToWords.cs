using System.Collections.Generic;

namespace Clinic.Common.Core.Extensions
{
    public class NumberToWords
    {

        public static string ConvertToWords(int number)
        {
            //Split the string into 3 part pieces
            string numStr = number.ToString();
            List<string> numParts = new List<string>();

            string currentStr = "";

            int t = 0;
            for (int i = numStr.Length - 1; i >= 0; i--)
            {
                t++;
                currentStr = numStr[i] + currentStr;

                if (t == 3)
                {
                    currentStr = "," + currentStr;
                    t = 0;
                }
            }

            if (currentStr.StartsWith(","))
                currentStr = currentStr.Remove(0, 1);

            string[] vals = currentStr.Split(',');

            numParts.AddRange(vals);

            List<string> outstring = new List<string>();

            List<string> delims = new List<string>();

            delims.Add("");

            if (numParts.Count >= 2)
                delims.Add("Thousand");

            if (numParts.Count >= 3)
                delims.Add("Million");

            if (numParts.Count >= 4)
                delims.Add("Billion");

            if (numParts.Count >= 5)
                delims.Add("Trillion");

            int j = delims.Count - 1;

            for (int i = 0; i < numParts.Count; i++)
            {
                int num = int.Parse(numParts[i]);

                string temp = "";


                if (num >= 100)
                    temp = ConvertThreeDigits(num);
                else if (num >= 10)
                    temp = ConvertTwoDigits(num);
                else
                {
                    if (i == 0)
                    {
                        temp = ConvertOneDigit(num);
                    }
                }


                temp += " " + delims[j];
                j--;

                outstring.Add(temp);
            }

            string retstring = string.Join(" ", outstring.ToArray());

            return retstring;
        }

        private static string ConvertThreeDigits(int Number)
        {
            int firstDigit = Number / 100;
            int lastDigits = Number - (firstDigit * 100);

            if (lastDigits == 0)
                return ConvertOneDigit(firstDigit) + " Hundred";
            else if (lastDigits < 9)
                return ConvertOneDigit(firstDigit) + " Hundred " + ConvertOneDigit(lastDigits);
            else
                return ConvertOneDigit(firstDigit) + " Hundred " + ConvertTwoDigits(lastDigits);
        }

        private static string ConvertTwoDigits(int Number)
        {
            int firstDigit = Number / 10;
            int secondDigit = Number - (firstDigit * 10);

            if (Number >= 10 && Number < 20)
            {
                if (secondDigit == 4 || secondDigit == 6 ||
                    secondDigit >= 7)
                {
                    return ConvertOneDigit(secondDigit) + "teen";
                }
                else
                {
                    switch (secondDigit)
                    {
                        case 0:
                            return "Ten";
                        case 1:
                            return "Eleven";
                        case 2:
                            return "Twelve";
                        case 3:
                            return "Thirteen";
                        case 5:
                            return "Fifteen";
                        default:
                            return "ERROR";
                    }
                }
            }
            else
            {
                string firstPart = "";

                switch (firstDigit)
                {
                    case 2:
                        firstPart = "Twenty";
                        break;
                    case 3:
                        firstPart = "Thirty";
                        break;
                    case 4:
                        firstPart = "Fourty";
                        break;
                    case 5:
                        firstPart = "Fifty";
                        break;
                    case 6:
                        firstPart = "Sixty";
                        break;
                    case 7:
                        firstPart = "Seventy";
                        break;
                    case 8:
                        firstPart = "Eighty";
                        break;
                    case 9:
                        firstPart = "Ninty";
                        break;
                    default:
                        return "ERROR";
                }

                if (secondDigit > 0)
                    return firstPart + "-" + ConvertOneDigit(secondDigit);
                else
                    return firstPart;
            }
        }

        private static string ConvertOneDigit(int Number)
        {
            switch (Number)
            {
                case 0:
                    return "Zero";
                case 1:
                    return "One";
                case 2:
                    return "Two";
                case 3:
                    return "Three";
                case 4:
                    return "Four";
                case 5:
                    return "Five";
                case 6:
                    return "Six";
                case 7:
                    return "Seven";
                case 8:
                    return "Eight";
                case 9:
                    return "Nine";
                default:
                    return "ERROR";
            }
        }

        public static string ConvertToSymbol(decimal number)
        {
            var symbol = "";
            if (IsInThousands(number))
            {
                symbol = (number / 1000M).ToString("#.##K");
            }
            else if (IsInMillions(number))
            {
                symbol = (number / 1000000M).ToString("#.#M");
            }
            else if (IsInBillions(number))
            {
                symbol = (number / 1000000000M).ToString("#.#B");
            }
            else
                symbol = number.ToString();

            return symbol;
        }

        private static bool IsInThousands(decimal number)
        {
            var rounded = number / 1000;
            return rounded >= 1.0M && rounded < 999.5M;
        }
        private static bool IsInMillions(decimal number)
        {
            var rounded = number / 1000000;
            return rounded >= 0.95M && rounded < 1000.0M;
        }
        private static bool IsInBillions(decimal number)
        {
            var rounded = number / 1000000000;
            return rounded >= 0.95M && rounded < 1000.0M;
        }

    }
}
