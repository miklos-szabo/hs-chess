using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class Block
    {
        public int Id { get; set; }
        public string BlockerUsername { get; set; }
        public string BlockedUsername { get; set; }
    }
}
