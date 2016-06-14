using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace MappingProject_CLNOTE
{
    //**
    //** Don't forget to add the appropriate indexes, otherwist it'll take forever to import
    public class ClaimNotes
    {
        public List<ClaimNote> Notes { get; set; }
        public int ClaimKey { get; set; }


        private DateTime ParseDate(int dateValue)
        {
            var regExp1 = new Regex(@"(\d{2})(\d{2})(\d{2})");
            var regExp2 = new Regex(@"1(\d{2})(\d{2})(\d{2})");
            var enteredDate = dateValue.ToString();
            var result = DateTime.MinValue;

            try
            {
                var badDates = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("2810730", "1140609"),
                    new KeyValuePair<string, string>("2811010", "1140430"),
                    new KeyValuePair<string, string>("2820920", "1141211"),
                    new KeyValuePair<string, string>("1000229", "1000228"),
                    new KeyValuePair<string, string>("1120229", "1120228"),
                    new KeyValuePair<string, string>("6150405", "1130516"),
                    new KeyValuePair<string, string>("5141209", "1141001"),
                    new KeyValuePair<string, string>("1080229", "1080228"),
                    new KeyValuePair<string, string>("1160229", "1160228"),
                    new KeyValuePair<string, string>("2300821", "1300821"),
                    new KeyValuePair<string, string>("6150813", "1150813"),
                    new KeyValuePair<string, string>("6150922", "1150922")
                };
                if (badDates.Exists(c => c.Key == enteredDate))
                {
                    enteredDate = badDates.FirstOrDefault(c => c.Key == enteredDate).Value;
                }
                //970326
                switch (enteredDate.Length)
                {
                    case 6:
                    {
                        var year = "19" + regExp1.Match(enteredDate).Groups[1].Value;
                        var month = regExp1.Match(enteredDate).Groups[2].Value;
                        var day = regExp1.Match(enteredDate).Groups[3].Value;
                        result = DateTime.Parse($"{month}/{day}/{year}");

                    }
                        break;
                    case 7:
                    {
                        var year = "20" + regExp2.Match(enteredDate).Groups[1].Value;
                        var month = regExp2.Match(enteredDate).Groups[2].Value;
                        var day = regExp2.Match(enteredDate).Groups[3].Value;
                        result = DateTime.Parse($"{month}/{day}/{year}");


                    }
                        break;
                }
                if (result == DateTime.MinValue)
                {
                    Debugger.Break();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        private class AgragatedNote
        {
            public CLNOTE1 Clnote1 { get; set; }
            public string PrevTpaClaimKey { get; set; }
            public DateTime CreateDate { get; set; }
            public string FullNotes { get; set; }
            public int TotalNotes { get; set; }
        }
        public void ProcessClientNotes(List<int> claimKeys, string noteType)
        {

            foreach (var claimKey in claimKeys)
            {
                IEnumerable<AgragatedNote> unprocessedNotes = null;


                switch (noteType)
                {
                    case "CLNOTE1AND2":
                        unprocessedNotes = UnprocessedNotesForClNote1AndClNote2(claimKey);
                        break;
                    case "CLNOTE1AND3":
                        unprocessedNotes = UnprocessedNotesForClNote1AndClNote3(claimKey);
                        break;
                    case "JUSTNOTES1":
                        unprocessedNotes = UnprocessedNotesForJustNotes1(claimKey);
                        break;
                }

                if(unprocessedNotes== null) continue;

                foreach (var clientNote in unprocessedNotes)
                {
                    //foreach (var clientNote in context.ClaimNotes.Where(c => c.PrevTpaClaimKey == clmkey).OrderBy(c => c.NOTE_NOTE_DATE.Value))
                    //{
                    if (string.IsNullOrEmpty(clientNote.FullNotes)) continue;

                    var claimNote = new ClaimNote();
                    claimNote.Author = "PrevTPA";
                    claimNote.BillableHours = 0;
                    claimNote.BillableMiles = 0;
                    claimNote.PrevTpaClaimKey = clientNote.PrevTpaClaimKey;
                    claimNote.CreateDate = clientNote.CreateDate;
                    claimNote.FileType = "W";
                    claimNote.KeyWork = "CR";
                    claimNote.NoteClass = "70";
                    claimNote.Text = clientNote.FullNotes;
                    claimNote.NoteType = "a";// noteTypes.Where(c => c.NOTESUBJECT == clientNote.NOTESUBJECT)
                                             //.Select(c => c.NTNOTETYPE).DefaultIfEmpty("a").First();

                    if (claimNote.HasMultipleNotes())
                    {
                        foreach (var multinote in claimNote.GetNotes())
                        {
                            InsertNote(multinote);
                        }
                    }
                    else
                    {
                        InsertNote(claimNote);
                    }

                }


            }
        }

        private IEnumerable<AgragatedNote> UnprocessedNotesForClNote1AndClNote2(int claimKey)
        {
            try
            {


                using (var context = new AppContext())
                {
                    var list = context.Clnote1s
                        .Where(c => c.ClaimNumber == claimKey)
                        //.Where(c=>c.NTNOTE == "08500___06870-001: Batter, Charles")
                        .Join(
                            context.Clnote2s,
                            cl1 => new { ClaimKey = cl1.ClaimNumber, SeqNum = cl1.NTSEQNUM, EntrdDt = cl1.NTDT },
                            cl2 => new { ClaimKey = cl2.ClaimNumber, SeqNum = cl2.N2SEQNUM, EntrdDt = cl2.ARDT },
                            (cl1, cl2) => new { cl1, cl2 })

                        .OrderBy(c => c.cl1.NTDATE)
                        .ThenBy(c => c.cl2.N2MINNUM)
                        .ToList();
                    var unprocessedNotes = list
                        .GroupBy(c => c.cl1, res => new { res.cl1, res.cl2 })
                        .Select(c =>
                        {
                            var notes = new List<string>();
                            notes.Add(c.Key.NTNOTE);
                            notes.AddRange(c.Select(d => d.cl2.N2NOTE));

                            var fullNotes = string.Join(" ", notes);
                            var prevTpaClaimKey = $"{c.Key.NTFND}{c.Key.NTYR.PadLeft(3, '0')}{c.Key.NTCASE.PadLeft(7, '0')}";
                            var enteredDate = ParseDate(c.Key.NTDATE);
                            return new AgragatedNote()
                            {
                                TotalNotes = notes.Count,
                                Clnote1 = c.Key,
                                CreateDate = enteredDate,
                                FullNotes = fullNotes,
                                PrevTpaClaimKey = prevTpaClaimKey
                            };
                        }
                        )
                        .ToList();

                    //var totalNotes = unprocessedNotes.ToList().Sum(c => c.TotalNotes);
                    return unprocessedNotes;
                }
            }
            catch (Exception ex)
            {
                using (var wr = new System.IO.StreamWriter(@"c:\temp\import_error_logs.log", true))
                {
                    wr.WriteLine("Error processing claim {0}", claimKey);
                    wr.Flush();
                }

            }
            return null;
        }
        private IEnumerable<AgragatedNote> UnprocessedNotesForClNote1AndClNote3(int claimKey)
        {
            using (var context = new AppContext())
            {
                var unprocessedNotes = context.Clnote1s
                    .Where(c => c.ClaimNumber == claimKey)
                    .Join(
                        context.Clnote3s,
                        cl1 => new { cl1.ClaimNumber, Key = cl1.NTKEY, SeqNum = cl1.NTSEQNUM, EntrdDt = cl1.NTDT },
                        cl3 => new { cl3.ClaimNumber, Key = cl3.CLKEY, SeqNum = cl3.N3SEQNUM, EntrdDt = cl3.CLCDAT },
                        (cl1, cl3) => new { cl1, cl3 })
                    .OrderBy(c => c.cl1.NTDATE)
                    .ThenBy(c => c.cl3.N3ASEQ)
                    .ThenBy(c => c.cl3.N3NSEQ)
                    .ToList()
                    .GroupBy(c => c.cl1, res => new { res.cl1, res.cl3 })
                    .Select(c =>
                    {
                        var notes = new List<string>();
                        notes.Add(c.Key.NTNOTE);
                        notes.AddRange(c.Select(d => d.cl3.N3NOTE));

                        var fullNotes = string.Join(" ", notes);
                        var prevTpaClaimKey = $"{c.Key.NTFND}{c.Key.NTYR.PadLeft(3, '0')}{c.Key.NTCASE.PadLeft(7, '0')}";
                        var enteredDate = ParseDate(c.Key.NTDATE);
                        return new AgragatedNote()
                        {
                            Clnote1 = c.Key,
                            CreateDate = enteredDate,
                            FullNotes = fullNotes,
                            PrevTpaClaimKey = prevTpaClaimKey
                        };
                    }
                    )
                    .ToList();
                return unprocessedNotes;
            }
        }

        private IEnumerable<AgragatedNote> UnprocessedNotesForJustNotes1(int claimKey)
        {
            using (var context = new AppContext())
            {
                var unprocessedNotes = context.Clnote1s
                    .Where(c => c.ClaimNumber == claimKey)
                    .OrderBy(c => c.NTDATE)
                    .ToList()
                    .Select(c =>
                    {
                        var notes = new List<string>();
                        notes.Add(c.NTNOTE);
                        
                        var fullNotes = string.Join(" ", notes);
                        var prevTpaClaimKey = $"{c.NTFND}{c.NTYR.PadLeft(3, '0')}{c.NTCASE.PadLeft(7, '0')}";
                        var enteredDate = ParseDate(c.NTDATE);
                        return new AgragatedNote()
                        {
                            Clnote1 = c,
                            CreateDate = enteredDate,
                            FullNotes = fullNotes,
                            PrevTpaClaimKey = prevTpaClaimKey
                        };
                    }
                    )
                    .ToList();
                return unprocessedNotes;
            }
        }


        private static List<KeyValuePair<DateTime, int>> _previouslyExecuted;

        private void InsertNote(ClaimNote note)
        {
            //return;
            if (_previouslyExecuted == null) _previouslyExecuted = new List<KeyValuePair<DateTime, int>>();

            using (var context = new AppContext())
            {

                //lock (obj)
                //{
                if (!_previouslyExecuted.Any(c => c.Key.Date == note.CreateDate.Date && c.Value == note.ClaimKey))
                {


                    //var entities = new PolkLbEntites();
                    var minDate = note.CreateDate.Date;
                    var maxDate = note.CreateDate.Date.AddDays(1);
                    var maxCreateDate = context.Notes.Where(c => c.PrevTpaClaimKey == note.PrevTpaClaimKey && c.NoteCreateTime >= minDate && c.NoteCreateTime < maxDate).Max(c => c.NoteCreateTime);
                    //var maxCreateDate = entities.JE_NOTES.Where(c => c.NTCLAIMID == note.ClaimKey && c.NTDTTM >= minDate && c.NTDTTM < maxDate).Max(c => c.NTDTTM);
                    if (maxCreateDate.HasValue)
                    {

                        note.CreateDate = maxCreateDate.Value.AddSeconds(1);
                    }
                }
                else
                {
                    var maxCreateDate = _previouslyExecuted.Where(c => c.Key.Date == note.CreateDate.Date && c.Value == note.ClaimKey).Max(c => c.Key).AddSeconds(1);
                    note.CreateDate = maxCreateDate.AddSeconds(1);
                }

                while (_previouslyExecuted.Any(c => c.Key == note.CreateDate && c.Value == note.ClaimKey))
                {
                    note.CreateDate = note.CreateDate.AddSeconds(1);
                }
                _previouslyExecuted.Add(new KeyValuePair<DateTime, int>(note.CreateDate, note.ClaimKey));

                var newNote = context.Notes.Create();
                newNote.NoteCreateTime = note.CreateDate;
                newNote.PrevTpaClaimKey = note.PrevTpaClaimKey;
                newNote.BillableHours = note.BillableHours;
                newNote.ClaimId = note.ClaimKey;
                newNote.BillableMiles = note.BillableMiles;
                newNote.FileType = note.FileType;
                newNote.KeyWord = note.KeyWork;
                newNote.NoteAuthor = note.Author;
                newNote.NoteType = note.NoteType;
                newNote.NoteText = note.Text;
                context.Notes.Add(newNote);
                context.SaveChanges();

            }
          


        }


        
    }
}
