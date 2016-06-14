using System;

namespace MappingProject_CLNOTE
{
    public class NoteFileAttribute: Attribute
    {
        public int Start { get; set; }
        public int Length { get; set; }

        public NoteFileAttribute(int start, int length)
        {
            Start = start;
            Length = length;
        }
    }
}
