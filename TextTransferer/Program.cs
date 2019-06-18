using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextTransferer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKey answer = ConsoleKey.NoName;
            while(answer != ConsoleKey.N)
            {
                //taking a path from user
                Console.WriteLine("Enter a file path:");
                string originPath = Console.ReadLine();
                

                if (File.Exists(originPath))
                {
                    //if file exists taking another file path
                    Console.WriteLine("Choose where to save formated file:");
                    string newFilePath = Console.ReadLine();
                    if (File.Exists(newFilePath))
                    {
                        //if everything is correct reading text and lines from an origin file
                        string text = File.ReadAllText(originPath);
                        string[] lines = File.ReadAllLines(originPath);

                        //formatting our input
                        string[] array = FormatString(text);
                        Dictionary<string, List<int>> dict = ProccessWords(array, lines);

                        List<string> tempArr = new List<string>();
                        //looping through the dictionary to form a string for a new file
                        foreach (var i in dict.OrderBy(x => x.Key))
                        {
                            string temp = "";

                            foreach (var x in i.Value)
                            {
                                temp += x + "; ";
                            }
                            tempArr.Add(String.Format("Word {0} found on line(s): {1}", i.Key, temp));

                            Console.WriteLine("Word {0} found on line(s): {1}", i.Key, temp);
                        }
                        File.WriteAllLines(newFilePath, tempArr);
                    }
                    else
                    {
                        Console.WriteLine("File does not exisits");
                    }
                    Console.WriteLine("File Successfully saved!");
                    Console.WriteLine("Want to try again? (Y\\N)");
                    answer = Console.ReadKey().Key;
                }
            }
            Console.WriteLine("Thank you, bye!");
            Console.ReadKey();

        }

        static private Dictionary<string, List<int>> ProccessWords(string[] input, string[] lines)
        {
            var dict = new Dictionary<string, List<int>>();
            //looping through our lines and words to find on which line it was found
            for (int i = 0; i < lines.Length; i++)
            {
                foreach (string s in input)
                {
                    List<int> temp = new List<int>();

                    //check for whole word match
                    if (Regex.IsMatch(lines[i], String.Format(@"\b{0}\b", s)))
                    {

                        //if there is no such a word in dict add new one
                        if (!dict.ContainsKey(s))
                        {
                            temp.Add(i + 1);
                            dict.Add(s, temp);


                        }
                        else //else get integer list from dict and add new value to it
                        {
                            dict.TryGetValue(s, out temp);
                            temp.Add(i + 1);
                            dict[s] = temp;
                        }
                        //remove word that has already been found from lines
                        int x = lines[i].IndexOf(s);
                        lines[i] = lines[i].Remove(x, s.Length);

                    }

                }

            }
            return dict;
        }

        static private string[] FormatString(string input)
        {
            string[] stringArray = new string[] { };
            //setting regex params for our word
            Regex regex = new Regex("[^A-Za-z0-9 -]");

            //if file is not empty formatting string
            if (input != "")
            {
                //splitting words by a space and storing into a list
                stringArray = input.Split(' ');
                //for each word removing any unnececcary symbols and transfering to lower case
                for (int i = 0; i < stringArray.Length; i++)
                {
                    stringArray[i] = regex.Replace(stringArray[i].ToLower(), "");
                    if (stringArray[i] == "")
                    {
                        List<string> tmp = new List<string>(stringArray);
                        tmp.Remove("");
                        stringArray = tmp.ToArray();
                        i--;
                    }
                    
                }
                return stringArray;
            }
            else
            {
                return stringArray;
            }
        }
    }
}
