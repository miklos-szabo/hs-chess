using System.ComponentModel.DataAnnotations;

namespace HSC.Dal.Entities
{
    public class User
    {
        public User()
        {
            Groups = new HashSet<Group>();
            Friends = new HashSet<User>();
            FriendsOf = new HashSet<User>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int RatingBullet { get; set; }
        public int RatingBlitz { get; set; }
        public int RatingRapid { get; set; }
        public int RatingClassical { get; set; }
        public decimal Balance { get; set; }
        public decimal PlayMoneyBalance { get; set; }
        public DateTimeOffset LastPlayMoneyRedeemDate { get; set; }
        public bool IsUsingPlayMoney { get; set; }
        public bool LightTheme { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<User> Friends { get; set; }
        public virtual ICollection<User> FriendsOf { get; set; }

    }
}
