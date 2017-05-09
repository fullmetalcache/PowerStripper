//Author: fullmetalcache
//Date: 2017-03-27

using System;
using System.Text;
using System.IO;

namespace PowerStripper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please provide the to a file...");
                return;
            }

            string strippedFile = "";
            string encodedFile = "";

            if (args[0].Contains("."))
            {
                strippedFile = args[0].Substring(0, args[0].IndexOf(".")) + "_stripped.ps1";
                encodedFile = args[0].Substring(0, args[0].IndexOf(".")) + "_encoded.txt";
            }
            else
            {
                strippedFile = args[0] + "_stripped.ps1";
                encodedFile = args[0] + "_encoded.txt";
            }

            stripComments(args[0], strippedFile);
            b64Encode(strippedFile, encodedFile);
        }

        static void stripComments(string inFile, string outFile)
        {
            string data = "";
            bool enter = true;
            using (StreamWriter fso = new System.IO.StreamWriter(outFile))
            {
                foreach (string line in File.ReadLines(inFile))
                {
                    string temp = line.Replace(" ", "");
                    if (line.IndexOf("<#") != -1)
                    {
                        enter = false;
                        for (int i = 0; i < line.IndexOf("<#"); i++)
                        {
                            data = data + line[i];
                        }
                        data = data + "\n";

                    }
                    else if (line.IndexOf("#>") != -1)
                    {
                        enter = true;
                        for (int i = line.IndexOf("#>") + 2; i < line.Length; i++)
                        {
                            if (i < line.Length)
                                data = data + line[i];
                        }
                        data = data + "\n";
                    }
                    else if (enter == true && temp != "" && temp[0] != '#')
                    {
                        data = data + line + "\n";
                    }
                }
                fso.Write(data);
            }

        }

        static void b64Encode(string inFile, string outFile)
        {
            using (StreamWriter fso = new StreamWriter(outFile))
            {
                fso.Write(Convert.ToBase64String(Encoding.UTF8.GetBytes(File.ReadAllText(inFile))));
            }
        }
    }
}