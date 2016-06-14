using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MappingProject_CLNOTE
{
    [Table("CLNOT3C")]
    public class CLNOTE3
    {
        [Key]
        public int Id { get; set; }
        [NoteFile(0, 7)]
        public int N3SEQNUM { get; set; }
        [NoteFile(7, 5)]
        public int N3ASEQ { get; set; }
        [NoteFile(13, 5)]
        public int N3NSEQ { get; set; }
        [NoteFile(18, 5)]
        public string N3CODE { get; set; }
        [NoteFile(23, 256)]
        public string N3NOTE { get; set; }
        [NoteFile(279, 3)]
        public string N3FND { get; set; }
        [NoteFile(283, 3)]//modified
        public string N3FYR { get; set; }
        [NoteFile(288, 7)]
        public string N3CASE { get; set; }
        [NoteFile(295, 40)]
        public string CLKEY { get; set; }
        [NoteFile(336, 7)]
        public int CLCDAT { get; set; }
        [NoteFile(345, 10)]
        public string CLIUSR { get; set; }
        [NoteFile(355, 7)]
        public int CLDT { get; set; }
        [NoteFile(362, 7)]
        public int CLIDT { get; set; }
        [NoteFile(369, 10)]
        public string CLUSER { get; set; }
        public int ClaimNumber { get; set; }
    }
}
