﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Dal.Entities;

namespace HSC.Bll.RatingService
{
    public class RatingService: IRatingService
    {
        public int GetRatingOfUserFromTimeControl(User user, int timeLimitMinutes)
        {
            if (timeLimitMinutes < 3) return user.RatingBullet;
            if (timeLimitMinutes < 10) return user.RatingBlitz;
            if (timeLimitMinutes < 20) return user.RatingRapid;
            return user.RatingClassical;
        }

        public void ModifyRating(User user, int timeLimitMinutes, int byAmount)
        {
            if (timeLimitMinutes < 3) user.RatingBullet += byAmount;
            else if (timeLimitMinutes < 10) user.RatingBlitz += byAmount;
            else if (timeLimitMinutes < 20) user.RatingRapid += byAmount;
            else user.RatingClassical += byAmount;
        }
    }
}
