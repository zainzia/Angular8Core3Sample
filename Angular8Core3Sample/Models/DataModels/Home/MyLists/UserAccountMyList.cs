

using Angular8Core3Sample.Models.Identity;
using System.Collections.Generic;

namespace Angular8Core3Sample.Models.DataModels.Home.MyLists
{
    public class UserAccountMyList
    {

        public int UserAccountMyListId { get; set; }

        public ApplicationUser UserAccount { get; set; }

        public string MyListName { get; set; }

        public IList<UserAccountMyListItem> MyListItems { get; set; }

    }
}
