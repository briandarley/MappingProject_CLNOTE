using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingProject_CLNOTE
{
    [Table("JE_NOTES")]
    public class Note
    {
        [Key]
        public int Id { get; set; }
        [Column("NTFILETYPE"), MaxLength(1)]
        public string FileType { get; set; }
        [Column("NTCLAIMID")]
        public int ClaimId { get; set; }
        [Column("NTDTTM"), Required]
        public DateTime? NoteCreateTime { get; set; }
        [Column("NTCLASS")]
        public int NoteClass { get; set; }
        [Column("NTTYPE"), MaxLength(1)]
        public string NoteType { get; set; }
        [Column("NTWHOINIT"), MaxLength(10)]
        public string NoteAuthor { get; set; }
        [Column("NTKEYWORD"), MaxLength(30)]
        public string KeyWord { get; set; }
        [Column("NTTEXT"), MaxLength(2000)]
        public string NoteText { get; set; }
        [Column("NTBILLHRS")]
        public decimal BillableHours { get; set; }
        [Column("NTBILLMILES")]
        public decimal BillableMiles { get; set; }
        [Column("PREV_TPA_CLAIMKEY"), MaxLength(50)]
        public string PrevTpaClaimKey { get; set; }
    }
}
