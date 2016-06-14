using System;
using System.Text.RegularExpressions;

namespace MappingProject_CLNOTE
{
    /// <summary>
    /// Process imports records from flat file into clnote tables in SQL
    /// </summary>
    public class ClNoteProcessor
    {

        public void ProcessClNote1()
        {
            var appcontext = new AppContext();
            var flatFileReader = new FlatFileReader(@"E:\Temp\Test Notes thru 05312016\Test Notes thru 05312016\CLNOT1C.txt");
            var records = flatFileReader.ReadFile<CLNOTE1>();

            var count = 0;
            var saveCount = 0;
            foreach (var clnote1 in records)
            {
                count++;
                var claimNumber = Regex.Match(clnote1.NTKEY.Substring(7), "0+([1-9][0-9]+$)").Groups[1].Value;
                clnote1.ClaimNumber = int.Parse(claimNumber);


                appcontext.Clnote1s.Add(clnote1);
                if (count % 1000 == 0)
                {
                    saveCount++;
                    appcontext.SaveChanges();
                    appcontext = new AppContext();
                    Console.WriteLine($"save count : {saveCount}");
                }
            }

            appcontext.SaveChanges();
        }
        public void ProcessClNote2()
        {
            var appcontext = new AppContext();
            var flatFileReader = new FlatFileReader(@"E:\Temp\Test Notes thru 05312016\Test Notes thru 05312016\CLNOT2C.txt");
            var records = flatFileReader.ReadFile<CLNOTE2>();

            var count = 0;
            var saveCount = 0;
            foreach (var clnote2 in records)
            {
                var claimNumber = Regex.Match(clnote2.ARKEY.Substring(7), "0+([1-9][0-9]+$)").Groups[1].Value;
                clnote2.ClaimNumber = int.Parse(claimNumber);
                count++;
                appcontext.Clnote2s.Add(clnote2);
                if (count % 1000 == 0)
                {
                    saveCount++;
                    appcontext.SaveChanges();
                    appcontext = new AppContext();
                    Console.WriteLine($"save count : {saveCount}");
                }
            }

            appcontext.SaveChanges();
        }
        public void ProcessClNote3()
        {
            var appcontext = new AppContext();
            var flatFileReader = new FlatFileReader(@"E:\Temp\Test Notes thru 05312016\Test Notes thru 05312016\CLNOT3C.txt");
            var records = flatFileReader.ReadFile<CLNOTE3>();

            var count = 0;
            var saveCount = 0;
            foreach (var clnote3 in records)
            {
                var claimNumber = Regex.Match(clnote3.CLKEY.Substring(7), "0+([1-9][0-9]+$)").Groups[1].Value;
                clnote3.ClaimNumber = int.Parse(claimNumber);
                count++;
                appcontext.Clnote3s.Add(clnote3);
                if (count % 1000 == 0)
                {
                    saveCount++;
                    appcontext.SaveChanges();
                    appcontext = new AppContext();
                    Console.WriteLine($"save count : {saveCount}");
                }
            }

            appcontext.SaveChanges();
        }
    }
}
