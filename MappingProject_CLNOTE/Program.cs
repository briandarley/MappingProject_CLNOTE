using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MappingProject_CLNOTE
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.ReadLinesForCLNOT1();
            //program.ReadLinesForCLNOT2();
            //program.ReadLinesForCLNOT3();
        }

        private const string ExcelPathForCLNOT1 = @"E:\Temp\CLNOT1C.csv";
        public void ReadLinesForCLNOT1()
        {
            List<string> dumpBox = new List<string>();

            var fileLines = File.ReadLines(ExcelPathForCLNOT1);


            foreach (var fileLine in fileLines)
            {

                var filtered = Regex.Replace(fileLine, @"[^\u0000-\u007F]", string.Empty);
                filtered = filtered.Replace("\0", "");
                
                if (Regex.Match(filtered, "^\\d+,\\d+,\\d+,\\d+,\\d+,\"[a-zA-Z0-9]*\",").Success)
                {
                    dumpBox.Add(filtered);
                }
                else if (string.IsNullOrEmpty(filtered.Trim()))
                {
                    continue;
                }
                else
                {
                    var lastItem = dumpBox.Last();
                    lastItem = lastItem + filtered;
                    dumpBox.RemoveAt(dumpBox.Count - 1);
                    dumpBox.Add(lastItem);

                }
                //Console.WriteLine(filtered);
            }
            var sr = new StreamWriter(@"E:\Temp\CLNOT1C_EDITED.csv", false);
            var regexp1 = new Regex("^(\\d+),(\\d+),(\\d+),(\\d+),(\\d+),\"(.+?(?=\",))\",\"(.+?(?=\",))\",\"(.+?(?=\",))\",(\\d+),(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"([a-zA-Z0-9]*)\",(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",(\\d+)$");
            var regexp2 = new Regex("^(\\d+),(\\d+),(\\d+),(\\d+),(\\d+),\"()\",\"(.+?(?=\",))\",\"()\",(\\d+),(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"([a-zA-Z0-9]*)\",(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",(\\d+)$");
            var regexp3 = new Regex("^(\\d+),(\\d+),(\\d+),(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"([a-zA-Z0-9]*)\",\"()\",(\\d+),(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"([a-zA-Z0-9]*)\",(\\d+),(\\d+),\"()\",(\\d+)$");
            var regexp4 = new Regex("^(\\d+),(\\d+),(\\d+),(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"([a-zA-Z0-9]*)\",\"(.+?(?=\",))\",(\\d+),(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"([a-zA-Z0-9]*)\",(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",(\\d+)$");
            var regexp5 = new Regex("^(\\d+),(\\d+),(\\d+),(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"([a-zA-Z0-9]*)\",\"()\",(\\d+),(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"([a-zA-Z0-9]*)\",(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",(\\d+)$");
            //1,940203,940203,0,0,"N","NMCD","",835,80,10342,"CSTUDER","7778350800010342",1140112,92436,"",1140112
            // 445,1140527,1140527,144438,0,"","CLCCO","MED NOTES RX COMPRESSION HOSE",835,85,11031,"CREID","7778350850011031",1140528,110428,"TBROWN",1140527
            //2658,1141229,1141229,143835,0,"N","NDW35","",835,98,2892,"GLIVENGO","7778350980002892",1141229,154424,"GLIVENGO",1141229


            foreach (var item in dumpBox)
            {
                //,835,114,15264,"7778351140015264",1140630,"",0,0,""
                //if (!regexp1.IsMatch(item))
                //{
                //    Console.WriteLine(item);
                //}
                //if (!Regex.Match(item, "\\d+,\\d+,\\d+,\"\\d+\",\\d+,\"[a-zA-Z0-9]+\",\\d+,\\d+,\"[a-zA-Z0-9]*\"$").Success)
                //{
                //    Console.WriteLine(item);
                //}

                if (!regexp1.IsMatch(item) && !regexp2.IsMatch(item) && !regexp3.IsMatch(item) && !regexp4.IsMatch(item) && !regexp5.IsMatch(item))
                {
                    Console.WriteLine(item);
                }
                else
                {
                    GroupCollection groups = null;
                    if (regexp1.IsMatch(item))
                    {
                        groups = regexp1.Match(item).Groups;
                    }
                    else if (regexp2.IsMatch(item))
                    {
                        groups = regexp2.Match(item).Groups;
                    }
                    else if (regexp3.IsMatch(item))
                    {
                        groups = regexp3.Match(item).Groups;
                    }
                    else if (regexp4.IsMatch(item))
                    {
                        groups = regexp4.Match(item).Groups;
                    }
                    else if (regexp5.IsMatch(item))
                    {
                        groups = regexp5.Match(item).Groups;
                    }


                    var cleanedCols = new List<string>();
                    var i = 0;
                    if (groups.Count != 18)
                    {
                        throw new Exception("Count doesn't match");
                    }
                    foreach (Group group in groups)
                    {
                        if (i > 0)
                        {
                            var value = group.Value;
                            value = Regex.Replace(value, "\\s+", " ");
                            value = Regex.Replace(value, "^'|'$", "");
                            cleanedCols.Add(value);
                        }
                        i++;
                    }
                    i = 0;
                    var resultRecord = string.Join("\t", cleanedCols);
                    sr.WriteLine(resultRecord);

                }
            }
            sr.Flush();
            sr.Close();

        }
        private const string ExcelPathForCLNOT2 = @"E:\Temp\CLNOT2C.csv";
        public void ReadLinesForCLNOT2()
        {
            List<string> dumpBox = new List<string>();

            var fileLines = File.ReadLines(ExcelPathForCLNOT2);


            foreach (var fileLine in fileLines)
            {

                var filtered = Regex.Replace(fileLine, @"[^\u0000-\u007F]", string.Empty);
                filtered = filtered.Replace("\0", "");
                //if (!filtered.Contains("7778350980002616"))
                //{
                //    //continue;
                //    //Console.ReadLine();
                //}
                if (Regex.Match(filtered, "^\\d+,\\d+,\"[a-zA-Z0-9]*\",\"").Success )
                {
                    dumpBox.Add(filtered);
                }
                else if (string.IsNullOrEmpty(filtered.Trim()))
                {
                    continue;
                }
                else 
                {
                    var lastItem = dumpBox.Last();
                    lastItem = lastItem + filtered;
                    dumpBox.RemoveAt(dumpBox.Count - 1);
                    dumpBox.Add(lastItem);

                }
                //Console.WriteLine(filtered);
            }
            var sr = new StreamWriter(@"E:\Temp\CLNOT2C_EDITED.csv",false);
            var regexp1 = new Regex("^(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"(.+?(?=\",))\",(\\d+),(\\d+),(\\d+),\"([^\"]+)\",(\\d+),\"([a-zA-Z0-9]+)\",(\\d+),(\\d+),\"([a-zA-Z0-9]*)\"$");
            var regexp2 = new Regex("^(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"()\",(\\d+),(\\d+),(\\d+),\"([^\"]+)\",(\\d+),\"([a-zA-Z0-9]+)\",(\\d+),(\\d+),\"([a-zA-Z0-9]*)\"$");

            foreach (var item in dumpBox)
            {
                //,835,114,15264,"7778351140015264",1140630,"",0,0,""
                if (!Regex.Match(item, "^\\d+,\\d+,\"[a-zA-Z0-9]*\"").Success)
                {
                    Console.WriteLine(item);
                }
                if (!Regex.Match(item, "\\d+,\\d+,\\d+,\"\\d+\",\\d+,\"[a-zA-Z0-9]+\",\\d+,\\d+,\"[a-zA-Z0-9]*\"$").Success)
                {
                    Console.WriteLine(item);
                }
              
                if (!regexp1.IsMatch(item) && !regexp2.IsMatch(item))
                {
                    Console.WriteLine(item);
                }
                else
                {
                    GroupCollection groups = null;
                    if (regexp1.IsMatch(item))
                    {
                        groups = regexp1.Match(item).Groups;
                    }
                    else
                    {
                        groups = regexp2.Match(item).Groups;
                    }
                    var cleanedCols = new List<string>();
                    var i = 0;
                    if (groups.Count != 14)
                    {
                        throw new Exception("Count doesn't match");
                    }
                    foreach (Group group in groups)
                    {
                        if (i > 0)
                        {
                            var value = group.Value;
                            value = Regex.Replace(value, "\\s+", " ");
                            value = Regex.Replace(value, "^'|'$", "");
                            cleanedCols.Add(value);
                        }
                        i++;
                    }
                    i = 0;
                    var resultRecord = string.Join("\t", cleanedCols);
                    sr.WriteLine(resultRecord);
                    
                }
            }
            sr.Flush();
            sr.Close();
            
        }
        private const string ExcelPathForCLNOT3 = @"E:\Temp\CLNOT3C.csv";
        public void ReadLinesForCLNOT3()
        {
            List<string> dumpBox = new List<string>();

            var fileLines = File.ReadLines(ExcelPathForCLNOT3);


            foreach (var fileLine in fileLines)
            {
                
                var filtered = Regex.Replace(fileLine, @"[^\u0000-\u007F]", string.Empty);
                filtered = filtered.Replace("\0","" );

                if (Regex.Match(filtered, "^\\d+,\\d+,\\d+,\"[a-zA-Z0-9]*\"").Success)
                {
                    dumpBox.Add(filtered);
                }
                else if (string.IsNullOrEmpty(filtered.Trim()))
                {
                    continue;
                }
                else
                {
                    var lastItem = dumpBox.Last();
                    lastItem = lastItem + filtered;
                    dumpBox.RemoveAt(dumpBox.Count-1);
                    dumpBox.Add(lastItem);

                }
                //Console.WriteLine(filtered);
            }
            var finalList = new List<string>();
            foreach (var item in dumpBox)
            {
                //,835,114,15264,"7778351140015264",1140630,"",0,0,""
                if (!Regex.Match(item, "^\\d+,\\d+,\\d+,\"[a-zA-Z0-9]*\"").Success)
                {
                    Console.WriteLine(item);
                }
                if (!Regex.Match(item, "\\d+,\\d+,\\d+,\"\\d+\",\\d+,\"\",\\d+,\\d+,\"\"$").Success)
                {
                    Console.WriteLine(item);
                }
                //var regexp1 = new Regex("(\\d+),(\\d+),(\\d+),\"([a-zA-Z]+)\",\"([^\"]+)\",(\\d+),(\\d+),(\\d+),\"([^\"]+)\",(\\d+),\"()\",(\\d+),(\\d+),\"()\"");
                var regexp1 = new Regex("(\\d+),(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"(.+?(?=\",))\",(\\d+),(\\d+),(\\d+),\"([^\"]+)\",(\\d+),\"()\",(\\d+),(\\d+),\"()\"");
                var regexp2 = new Regex("(\\d+),(\\d+),(\\d+),\"([a-zA-Z0-9]*)\",\"([^\"]?)\",(\\d+),(\\d+),(\\d+),\"([^\"]+)\",(\\d+),\"()\",(\\d+),(\\d+),\"()\"");

                

                if (!regexp1.IsMatch(item) && !regexp2.IsMatch(item))
                {
                    Console.WriteLine(item);
                }
                else
                {
                    GroupCollection groups = null;
                    if (regexp1.IsMatch(item))
                    {
                        groups = regexp1.Match(item).Groups;
                    }
                    else 
                    {
                        groups = regexp2.Match(item).Groups;
                    }
                    var cleanedCols = new List<string>();
                    var i = 0;
                    foreach (Group group in groups)
                    {
                        if(i > 0)
                        {
                            var value = group.Value;
                            value = Regex.Replace(value, "\\s+", " ");
                            value = Regex.Replace(value, "^'|'$", "");
                            cleanedCols.Add(group.Value);
                        }
                        i++;
                    }
                    i = 0;
                    var resultRecord = string.Join("\t", cleanedCols);
                    finalList.Add(resultRecord);
                }
            }

            File.WriteAllLines(@"E:\Temp\CLNOT3C_EDITED.csv", finalList);
        }
    }
}
//(\\d+),(\\d+),(\\d+),\"([a-zA-Z]+)\",\"([^\",]+)\",' From: system@mymatrixx.com [mailto:system@mymatrixx.com]  Sent: Monday, February 03, 2014 8:00 AM To: Van Cort, Alexis Subject: Authorization Completed for Claim # 835-085-0011031 (SHARON GABRIEL-LONG)     Thank you for completing authorization number 67",835,85,11031,"7778350850011031",1140203,"",0,0,""