using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MappingProject_CLNOTE
{
    [Table("CLNOT2C")]
    public class CLNOTE2
    {
        [Key]
        public int Id { get; set; }
        [NoteFile(0, 6)]
        public int N2SEQNUM { get; set; }
        [NoteFile(7, 5)]
        public int N2MINNUM { get; set; }
        [NoteFile(12, 5)]
        public string N2CODE { get; set; }
        [NoteFile(17, 75)]
        public string N2NOTE { get; set; }
        [NoteFile(93, 3)]
        public string N2FND { get; set; }
        [NoteFile(96, 3)]
        public string N2YR { get; set; }
        [NoteFile(101, 7)]
        public string N2CASE { get; set; }
        [NoteFile(107, 40)]
        public string ARKEY { get; set; }
        [NoteFile(149, 7)]
        public int ARCDAT { get; set; }
        [NoteFile(156, 10)]
        public string ARIUSR { get; set; }
        [NoteFile(167, 7)]
        public int ARDT { get; set; }
        [NoteFile(175, 7)]
        public string ARIDT { get; set; }
        [NoteFile(182, 10)]
        public string ARUSER { get; set; }
        public int ClaimNumber { get; set; }
    }
}
