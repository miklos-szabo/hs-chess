using HSC.Transfer.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Groups
{
    public class GroupDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserContextMenuDto> Users { get; set; }
        public bool IsInGroup { get; set; }
        public int UserCount { get; set; }
    }
}
