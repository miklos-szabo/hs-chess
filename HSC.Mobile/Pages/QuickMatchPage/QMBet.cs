using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Mobile.Pages.QuickMatchPage
{
    public class QMBet
    {
        public decimal MinimumBet { get; set; }
        public decimal MaximumBet { get; set; }

        public override string ToString()
        {
            return $"{MinimumBet:F2}$ - {MaximumBet:F2}$";
        }
    }
}
