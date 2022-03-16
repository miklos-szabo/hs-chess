namespace HSC.Dal.Entities
{
    public class User
    {
        public User()
        {
            Groups = new HashSet<Group>();
            Friends = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int RatingBullet { get; set; }
        public int RatingBlitz { get; set; }
        public int RatingRapid { get; set; }
        public int RatingClassical { get; set; }
        public double Balance { get; set; }
        public double PlayMoneyBalance { get; set; }
        public DateTimeOffset LastPlayMoneyRedeemDate { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<User> Friends { get; set; }

    }
}
