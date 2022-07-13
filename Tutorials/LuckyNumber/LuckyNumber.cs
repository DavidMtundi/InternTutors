using System.Text.RegularExpressions;

namespace Tutorials.LuckyNumber
{
    public class LuckyNumber
    {
        public static void GetLuckyNumber()
        {
            string? input = "";
            string? concatstring = "";
            double convertedNumbers;
            double[] dummyresult = Array.Empty<double>();
            double[] providedvalues = Array.Empty<double>();
            string resultfound = "";

            do
            {
                Console.WriteLine("");
                Console.Write("Please Enter a Valid String, it should be only letters :  ");
                input = Console.ReadLine();
                concatstring = input.Replace(" ", string.Empty);
            }
            while (!IsValidString(concatstring));


            //convert the string to a list of numbers with indexpoints;
            convertedNumbers = ConvertStringToDouble(concatstring);
            providedvalues = ConvertStringToDoubleArray(concatstring);
            resultfound = GetResult(providedvalues)[0].ToString();
            //this is for the purpose of understanding
            for (int i = 0; i < concatstring.Length; i++)
            {
                char character = concatstring[i];
                Console.WriteLine($"the item at Location {i} is {character} and It's index is {GetCharacterPosition(character)}");
            }

            Console.WriteLine($"The Lucky Number for this String {input} is {resultfound}");
            //  return resultfound;

        }
        //get lucky number
        private static double[] GetResult(double[] providedvalues)
        {
            double providedvaluelength = providedvalues.Length;
            int intlength = providedvalues.Length;
            double midlength = providedvaluelength / 2;
            string convertedarray = "";

            int midvalue = (int)Math.Ceiling(midlength);
            double[] dummyresult = new double[midvalue];
            for (int i = 0; i < midvalue; i++)
            {
                int subtractresult = intlength - i - 1;
                if (subtractresult <= i)
                {
                    dummyresult[i] = providedvalues[i];
                }
                else
                {
                    dummyresult[i] = providedvalues[i] + providedvalues[subtractresult];
                }
            }
            if (dummyresult.Length == 1)
            {
                return dummyresult;
            }
            else
            {
                convertedarray = string.Join(" ", dummyresult).Replace(" ", string.Empty);
                //convert the string back to an array of doubles
                //i have to do this since i realized that some addition results to a value greater than 10
                //thus an array element contains a value greater than 10
                //so this method convertstringarraytodoublearray converts the array to a string then back to a
                //double
                dummyresult = ConvertStringArrayToDoubleArray(convertedarray);
                return GetResult(dummyresult);
            }
            // return dummyresult;

        }

        private static double ConvertStringToDouble(string value)
        {
            double converted = 0;
            for (int i = 0; i < value.Length; i++)
            {
                converted += GetCharacterPosition(value[i]) * Math.Pow(10, double.Parse(i.ToString()));
            }
            return converted;
        }
        private static double[] ConvertStringArrayToDoubleArray(string value)
        {
            double[] converted = new double[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                converted[i] = double.Parse(value[i].ToString());
            }
            return converted;
        }
        private static double[] ConvertStringToDoubleArray(string value)
        {
            double[] converted = new double[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                converted[i] = GetCharacterPosition(char.Parse(value[i].ToString()));
            }
            return converted;
        }
        private static bool IsValidString(string value)
        {
            Regex regex = new Regex("[A-Za-z ]+");
            if (regex.IsMatch(value))
            {
                if (regex.Matches(value)[0].Groups[0].Length == value.Length)
                {
                    return true;
                }
            }
            return false;
        }
        private static int GetCharacterPosition(char character)
        {
            return char.ToUpper(character) - 64;//index == 1
        }

    }
}
