using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.User
{
    public class UserFullDetailsDto
    {
        public string UserName { get; set; }
        public decimal PlayMoneyBalance { get; set; }
        public decimal Balance { get; set; }
        public bool IsUsingPlayMoney { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RatingBullet { get; set; }
        public int RatingBlitz { get; set; }
        public int RatingRapid { get; set; }
        public int RatingClassical { get; set; }
    }
}
