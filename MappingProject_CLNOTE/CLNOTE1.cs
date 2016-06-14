using System.ComponentModel.DataAnnotations.Schema;

namespace MappingProject_CLNOTE
{
    [Table("CLNOT1C")]
    public class CLNOTE1
    {
        public int Id { get; set; }
        [NoteFile(0, 7)]
        public int NTSEQNUM { get; set; }
        [NoteFile(7, 7)]
        public int NTDATE { get; set; }
        [NoteFile(15, 7)]
        public int NTTRDT { get; set; }
        [NoteFile(24, 6)]
        public int NTTRTM { get; set; }
        [NoteFile(30, 7)]
        public string NTCMPL { get; set; }

        [NoteFile(37, 1)]
        public string NTDIAR { get; set; }
        [NoteFile(38, 5)]
        public string NTCODE { get; set; }
        [NoteFile(43, 275)]
        public string NTNOTE { get; set; }
        [NoteFile(319, 3)]
        public string NTFND { get; set; }
        [NoteFile(323, 3)]
        public string NTYR { get; set; }
        [NoteFile(327, 7)]
        public string NTCASE { get; set; }
        [NoteFile(334, 10)]
        public string NTUSER { get; set; }
        [NoteFile(344, 40)]
        public string NTKEY { get; set; }
        [NoteFile(385, 7)]
        public int NTDT { get; set; }
        [NoteFile(393, 6)]
        public string NTTIME { get; set; }
        [NoteFile(399, 10)]
        public string NTIUSR { get; set; }
        [NoteFile(410, 7)]
        public string NTIDT { get; set; }
        public int ClaimNumber { get; set; }
    }
}
