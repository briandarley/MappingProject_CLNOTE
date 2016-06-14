using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingProject_CLNOTE
{
    public class AppContext : DbContext
    {
        
        public DbSet<CLNOTE1> Clnote1s { get; set; }
        public DbSet<CLNOTE2> Clnote2s { get; set; }
        public DbSet<CLNOTE3> Clnote3s { get; set; }

        public DbSet<Note> Notes { get; set; }
        public AppContext() : base("sql:local")
        {
            
        }
    }
}
