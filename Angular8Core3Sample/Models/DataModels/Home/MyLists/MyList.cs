
using System.Collections.Generic;

namespace Angular8Core3Sample.Models.DataModels.Home.MyLists
{
    public class MyList
    {

        public int myListId { get; set; }

        public string userAccountId { get; set; }

        public string myListName { get; set; }

        public List<MyListItem> myListItems { get; set; }

    }
}
