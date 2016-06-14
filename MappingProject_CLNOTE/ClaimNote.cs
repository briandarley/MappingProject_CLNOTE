using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MappingProject_CLNOTE
{
    public class ClaimNote : ICloneable
    {
        [Key]
        public int Id { get; set; }
        private string _text;
        /// <summary>
        /// NTFILETYPE
        /// </summary>

        public string FileType { get; set; }
        /// <summary>
        /// NTCLAIMID
        /// </summary>
        public int ClaimKey { get; set; }
        /// <summary>
        /// NTDTTM
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// NTCLASS
        /// </summary>
        public string NoteClass { get; set; }
        /// <summary>
        /// NTTYPE
        /// </summary>
        public string NoteType { get; set; }
        /// <summary>
        /// NTWHOINIT
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// NTKEYWORD
        /// </summary>
        public string KeyWork { get; set; }
        /// <summary>
        /// NTTEXT
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                var result = value;
                result = result.Replace("'", "''");
                result = System.Text.RegularExpressions.Regex.Replace(result, @"\s", " ");
                _text = result;
            }
        }
        /// <summary>
        /// NTBILLHRS
        /// </summary>
        public int BillableHours { get; set; }
        /// <summary>
        /// NTBILLMILES
        /// </summary>
        public int BillableMiles { get; set; }

        public int NoteId { get; set; }


        public bool HasNotes()
        {
            return Text.Trim().Length > 0;
        }

        public bool HasMultipleNotes()
        {
            if (!HasNotes()) return false;
            return (Text.Trim().Length >= 1900);
        }

        private IEnumerable<string> OldSplit(string str, int chunkSize)
        {
            var range = (Int32)(Math.Ceiling(str.Length / (decimal)chunkSize));
            var substring = string.Empty;
            var totalLength = str.Length;
            return Enumerable.Range(0, range)
                .Select(i =>
                {
                    var begin = i * chunkSize;
                    var length = chunkSize;
                    if ((begin + length) > totalLength)
                    {
                        length = totalLength - begin;

                        substring = str.Substring(begin, length);
                    }
                    else
                    {
                        substring = str.Substring(begin, length);
                    }
                    if (begin > 0)
                        return " " + substring;

                    return substring;
                }



                    );
        }
        public static IEnumerable<string> SplitOn(string initial, int MaxCharacters)
        {

            if (string.IsNullOrEmpty(initial) == false)
            {
                string targetGroup = "Line";
                string theRegex = string.Format(@"(?<{0}>.{{1,{1}}})(?:\W|$)", targetGroup, MaxCharacters);

                MatchCollection matches = Regex.Matches(initial, theRegex, RegexOptions.IgnoreCase
                                                                          | RegexOptions.Multiline
                                                                          | RegexOptions.ExplicitCapture
                                                                          | RegexOptions.CultureInvariant
                                                                          | RegexOptions.Compiled);
                if (matches != null)
                    if (matches.Count > 0)
                        foreach (Match m in matches)
                            yield return m.Groups[targetGroup].Value;

            }

            yield break;
        }


        public IEnumerable<ClaimNote> GetNotes()
        {
            if (!HasNotes()) yield break;
            if (!HasMultipleNotes())
            {
                yield return this;
                yield break;
            }

            var listOfNotes = SplitOn(Text, 1900);
            var curSecond = 0;
            var previousText = string.Empty;
            foreach (var item in listOfNotes)
            {
                var curNote = (ClaimNote)Clone();
                if (previousText == curNote.Text) continue;
                curNote.CreateDate = curNote.CreateDate.AddSeconds(curSecond);
                curNote.Text = item;
                curSecond += 1;
                previousText = curNote.Text;
                yield return curNote;
            }


            yield break;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public string PrevTpaClaimKey { get; set; }

    }
}
