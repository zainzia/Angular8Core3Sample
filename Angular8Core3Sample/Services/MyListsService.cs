
using Angular8Core3Sample.Data;
using Angular8Core3Sample.Models.DataModels.Home.MyLists;

using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;


namespace Angular8Core3Sample
{
    public class MyListsService
    {

        private ApplicationDbContext DbContext;


        public MyListsService(ApplicationDbContext context)
        {
            DbContext = context;
        }


        public MyList CreateMyList(string listName, string userAccountId)
        {

            var newList = new UserAccountMyList
            {
                MyListName = listName,
                UserAccount = DbContext.Users.FirstOrDefault(x => x.Id == userAccountId)
            };

            DbContext.UserAccountMyLists.Add(newList);
            DbContext.SaveChanges();

            var newMyList = new MyList
            {
                myListId = newList.UserAccountMyListId,
                myListName = newList.MyListName,
                userAccountId = newList.UserAccount.Id
            };

            return newMyList;
        }


        public bool MoveMyListItem(MoveMyListItem moveMyListItem)
        {
            if(moveMyListItem == null || moveMyListItem.ProductId < 1 || moveMyListItem.OriginalMyListId < 1)
            {
                return false;
            }

            if(moveMyListItem.MoveToMyListId == null)
            {
                var myListName = DbContext.UserAccountMyLists.FirstOrDefault(x => x.UserAccountMyListId == moveMyListItem.OriginalMyListId).UserAccount.FirstName + "'s List";
                var userAccountId = DbContext.UserAccountMyLists.FirstOrDefault(x => x.UserAccountMyListId == moveMyListItem.OriginalMyListId).UserAccount.Id;
                moveMyListItem.MoveToMyListId = CreateMyList(myListName, userAccountId).myListId;
            }

            if(DeleteMyListItem(moveMyListItem.ProductId, moveMyListItem.OriginalMyListId))
            {
                var addMyListItem = new AddMyListItem
                {
                    MyListId = moveMyListItem.MoveToMyListId.Value,
                    ProductId = moveMyListItem.ProductId
                };
                return AddMyListItem(addMyListItem).Value;
            }

            return false;
        }


        public bool? AddMyListItem(AddMyListItem addMyListItem)
        {
            if (addMyListItem == null || addMyListItem.ProductId < 1 || addMyListItem.MyListId < 1)
            {
                return null;
            }

            var myList = DbContext.UserAccountMyLists
                                    .Include(x => x.MyListItems)
                                        .ThenInclude(y => y.Item)
                                    .FirstOrDefault(z => z.UserAccountMyListId == addMyListItem.MyListId);

            if(myList.MyListItems.Any(x => x.Item.ProductID == addMyListItem.ProductId))
            {
                return false;
            }

            var product = DbContext.Products.FirstOrDefault(x => x.ProductID == addMyListItem.ProductId);

            var item = new UserAccountMyListItem
            {
                Item = product
            };

            myList.MyListItems.Add(item);
            DbContext.SaveChanges();

            return true;
        }


        public bool DeleteMyListItem(int productId, int myListId)
        {
            if(productId < 1)
            {
                return false;
            }

            var myList = DbContext.UserAccountMyLists
                                    .Include(x => x.MyListItems)
                                        .ThenInclude(y => y.Item)
                                    .FirstOrDefault(z => z.UserAccountMyListId == myListId);

            var product = myList.MyListItems.FirstOrDefault(x => x.Item.ProductID == productId);

            myList.MyListItems.Remove(product);
            DbContext.SaveChanges();

            return true;
        }

        public bool DeleteMyList(int myListId)
        {
            if (myListId < 1)
            {
                return false;
            }

            var myList = DbContext.UserAccountMyLists.FirstOrDefault(x => x.UserAccountMyListId == myListId);

            DbContext.UserAccountMyLists.Remove(myList);
            DbContext.SaveChanges();

            return true;
        }


        public List<MyList> GetMyListNames(string userAccountId, int? languageId, int? countryId)
        {
            if (userAccountId == null || languageId == null || countryId == null)
            {
                return null;
            }

            var myListNames = DbContext.UserAccountMyLists
                                        .Where(x => x.UserAccount.Id == userAccountId)
                                        .Select(y => new MyList
                                        {
                                            myListId = y.UserAccountMyListId,
                                            myListName = y.MyListName,
                                            userAccountId = y.UserAccount.Id
                                        }).ToList();

            return myListNames;
        }


        public List<MyList> GetMyLists(string userAccountId, int? languageId, int? countryId)
        {

            if (userAccountId == null || languageId == null || countryId == null)
            {
                return null;
            }

            var myLists = DbContext.UserAccountMyLists
                                    .Include(m => m.UserAccount)
                                    .Include(m => m.MyListItems)
                                        .ThenInclude(p => p.Item)
                                            .ThenInclude(n => n.Names)
                                                .ThenInclude(l => l.Language)
                                    .Include(m => m.MyListItems)
                                        .ThenInclude(p => p.Item)
                                            .ThenInclude(p => p.Descriptions)
                                                .ThenInclude(d => d.Language)
                                    .Include(m => m.MyListItems)
                                        .ThenInclude(p => p.Item)
                                            .ThenInclude(p => p.Descriptions)
                                                .ThenInclude(d => d.Description)
                                    .Include(m => m.MyListItems)
                                        .ThenInclude(p => p.Item)
                                            .ThenInclude(p => p.Images)
                                    .Include(m => m.MyListItems)
                                        .ThenInclude(p => p.Item)
                                            .ThenInclude(p => p.Prices)
                                                .ThenInclude(c => c.Country)
                                    .Include(m => m.MyListItems)
                                        .ThenInclude(p => p.Item)
                                            .ThenInclude(p => p.Category)
                                                .ThenInclude(c => c.Names)
                                                    .ThenInclude(l => l.Language)
                                    .Where(y => y.UserAccount.Id == userAccountId)
                                    .Select(z => new MyList
                                    {
                                        userAccountId = z.UserAccount.Id,
                                        myListId = z.UserAccountMyListId,
                                        myListName = z.MyListName,
                                        myListItems = z.MyListItems.Select(x => new MyListItem
                                        {
                                            ItemId = x.UserAccountMyListItemId,
                                            ProductId = x.Item.ProductID.Value,
                                            CategoryId = x.Item.Category.CategoryID.Value,
                                            CategoryName = x.Item.Category.Names.FirstOrDefault(y => y.Language.LanguageID == languageId).Name,
                                            Description = x.Item.Descriptions.FirstOrDefault(d => d.Language.LanguageID == languageId).Description.FirstOrDefault().Description,
                                            Image = x.Item.Images.FirstOrDefault().Path1,
                                            Name = x.Item.Names.FirstOrDefault(y => y.Language.LanguageID == languageId).Name,
                                            Price = x.Item.Prices.FirstOrDefault(p => p.Country.CountryID == countryId).Price.Value,
                                            Currency = x.Item.Prices.FirstOrDefault(p => p.Country.CountryID == countryId).Country.Currency,
                                            Shipping = x.Item.Prices.FirstOrDefault(p => p.Country.CountryID == countryId).Shipping.Value,
                                        }).ToList()
                                    }).ToList();

            return myLists;
        }
    }
}
