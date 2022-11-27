using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Mobile.Pages.QuickMatchPage
{
    public class QMTimeControl
    {
        public int Minutes { get; set; }
        public int Increment { get; set; }

        public override string ToString()
        {
            return $"{Minutes} + {Increment}";
        }
    }
}
