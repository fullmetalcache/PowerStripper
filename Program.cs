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

        static void stripComments( string inFile, string outFile )
        {
            using (StreamWriter fso = new System.IO.StreamWriter(outFile))
            {
                bool startToken = false;

                foreach (string line in File.ReadLines(inFile))
                {
                    if (!startToken)
                    {
                        if (line.Contains("<#"))
                        {
                            startToken = true;
                            continue;
                        }
                        else
                        {
                            if (line.Length > 0)
                            {
                                string outLine = line;

                                if (outLine.Contains("#"))
                                {
                                    int idx = outLine.IndexOf("#");
                                    outLine = outLine.Substring(0, idx);
                                }

                                if (outLine.Length > 0)
                                {
                                    outLine = outLine.Replace("Write-Host", "Write-Output");

                                    fso.WriteLine(outLine);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (line.Contains("#>"))
                        {
                            startToken = false;
                        }
                    }
                }
            }
        }

        static void b64Encode( string inFile, string outFile )
        {
            using (StreamWriter fso = new StreamWriter(outFile))
            {
                fso.Write(Convert.ToBase64String(Encoding.UTF8.GetBytes(File.ReadAllText(inFile))));
            }
        }
    }
}
