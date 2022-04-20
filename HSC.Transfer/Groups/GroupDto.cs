using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Groups
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }
        public bool IsInGroup { get; set; }
    }
}
