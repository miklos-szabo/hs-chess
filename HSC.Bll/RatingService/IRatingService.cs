using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Dal.Entities;

namespace HSC.Bll.RatingService
{
    public interface IRatingService
    {
        int GetRatingOfUserFromTimeControl(User user, int timeLimitMinutes);
        void ModifyRating(User user, int timeLimitMinutes, int byAmount);
    }
}
