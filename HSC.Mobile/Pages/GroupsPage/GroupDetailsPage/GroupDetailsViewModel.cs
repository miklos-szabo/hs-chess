using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Mobile.Pages.GroupsPage.GroupDetailsPage
{
    public class GroupDetailsViewModel: BaseViewModel
    {
        private int _groupId;

        public int GroupId
        {
            get => _groupId;
            set => SetField(ref _groupId, value);
        }
    }
}
