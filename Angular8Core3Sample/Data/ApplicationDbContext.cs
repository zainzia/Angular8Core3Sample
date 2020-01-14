
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


using Angular8Core3Sample.Models.DataModels.Catalog;
using Angular8Core3Sample.Models.DataModels.Catalog.Product;


using Angular8Core3Sample.Models.Identity;
using Angular8Core3Sample.Models.DataModels.Home.MyLists;


namespace Angular8Core3Sample.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        

        //Catalog
        public DbSet<Language> Languages { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<State> States { get; set; }


        //Categories
        public DbSet<Models.DataModels.Catalog.Category.Category> Categories { get; set; }

        // Products
        public DbSet<Models.DataModels.Catalog.Product.Product> Products { get; set; }


        //Identity
        public DbSet<Token> Tokens { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //UserAccount 
        public DbSet<UserAccountMyList> UserAccountMyLists { get; set; }
        public DbSet<UserAccountMyListItem> UserAccountMyListItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            if (builder == null)
            {
                throw new System.ArgumentNullException("Builder is null");
            }

            //Products
            builder.Entity<Models.DataModels.Catalog.Product.Product>().HasKey(c => c.ProductID);
            builder.Entity<Models.DataModels.Catalog.Product.Product>().HasIndex(c => c.ProductID).IsUnique();
            builder.Entity<Models.DataModels.Catalog.Product.Product>().Property(u => u.ProductID).UseIdentityColumn();
            builder.Entity<Models.DataModels.Catalog.Product.Product>().HasMany(u => u.Names).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.DataModels.Catalog.Product.Product>().HasMany(u => u.Descriptions).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.DataModels.Catalog.Product.Product>().HasMany(u => u.Images).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.DataModels.Catalog.Product.Product>().HasMany(u => u.Prices).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.DataModels.Catalog.Product.Product>().HasMany(u => u.Specifics).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.DataModels.Catalog.Product.Product>().HasMany(u => u.Options).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.DataModels.Catalog.Product.Product>().HasMany(u => u.Filters).WithOne().OnDelete(DeleteBehavior.Cascade);
            

            //Category
            builder.Entity<Models.DataModels.Catalog.Category.Category>().HasKey(c => c.CategoryID);
            builder.Entity<Models.DataModels.Catalog.Category.Category>().HasIndex(c => c.CategoryID).IsUnique();
            builder.Entity<Models.DataModels.Catalog.Category.Category>().Property(u => u.CategoryID).UseIdentityColumn();
            builder.Entity<Models.DataModels.Catalog.Category.Category>().HasMany(u => u.Descriptions).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.DataModels.Catalog.Category.Category>().HasMany(u => u.Names).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.DataModels.Catalog.Category.Category>().HasMany(u => u.Images).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.DataModels.Catalog.Category.Category>().HasMany(u => u.Filters).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.DataModels.Catalog.Category.Category>().HasMany(u => u.Keywords).WithOne().OnDelete(DeleteBehavior.Cascade);

           
            //Language
            builder.Entity<Language>().HasKey(c => c.LanguageID);
            builder.Entity<Language>().HasIndex(c => new { c.LanguageID, c.Name } ).IsUnique();
            builder.Entity<Language>().Property(u => u.LanguageID).UseIdentityColumn();
            builder.Entity<Language>().Property(u => u.Name).IsRequired();
            builder.Entity<Language>().Property(u => u.Culture).IsRequired();


            //Country
            builder.Entity<Country>().HasKey(c => c.CountryID);
            builder.Entity<Country>().HasIndex(c => c.CountryID).IsUnique();
            builder.Entity<Country>().Property(u => u.CountryID).UseIdentityColumn();
            builder.Entity<Country>().Property(u => u.Name).IsRequired();


            //Token
            builder.Entity<Token>().HasKey(t => t.TokenId);
            builder.Entity<Token>().HasIndex(t => new { t.TokenId, t.UserId }).IsUnique();
            builder.Entity<Token>().HasOne(i => i.User).WithMany(u => u.Tokens);


            //MyList
            builder.Entity<UserAccountMyList>().HasKey(m => m.UserAccountMyListId);
            builder.Entity<UserAccountMyList>().HasIndex(c => c.UserAccountMyListId).IsUnique();
            builder.Entity<UserAccountMyList>().HasOne(m => m.UserAccount).WithMany().IsRequired();
            builder.Entity<UserAccountMyList>().HasMany(m => m.MyListItems).WithOne().IsRequired();


            //MyListItem
            builder.Entity<UserAccountMyListItem>().HasKey(m => m.UserAccountMyListItemId);
            builder.Entity<UserAccountMyListItem>().HasIndex(c => c.UserAccountMyListItemId).IsUnique();
            builder.Entity<UserAccountMyListItem>().HasOne(m => m.Item).WithMany().IsRequired();


            //UserProfile
            builder.Entity<UserProfile>().HasKey(m => m.UserProfileID);
            builder.Entity<UserProfile>().HasIndex(c => c.UserProfileID).IsUnique();
            builder.Entity<UserProfile>().Property(u => u.UserProfileID).UseIdentityColumn();
            builder.Entity<UserProfile>().Property(u => u.FirstName).IsRequired();
            builder.Entity<UserProfile>().Property(m => m.LastName).IsRequired();
            builder.Entity<UserProfile>().Property(m => m.Email).IsRequired();
            builder.Entity<UserProfile>().Property(m => m.Username).IsRequired();


            //Address
            builder.Entity<Address>().HasKey(m => m.AddressID);
            builder.Entity<Address>().HasIndex(c => c.AddressID).IsUnique();
            builder.Entity<Address>().Property(u => u.AddressID).UseIdentityColumn();
            builder.Entity<Address>().Property(m => m.Address1).IsRequired();
            builder.Entity<Address>().Property(m => m.City).IsRequired();
            builder.Entity<Address>().Property(m => m.PhoneNumber).IsRequired();
            builder.Entity<Address>().HasOne(x => x.Country).WithMany().IsRequired();


            //Province
            builder.Entity<Province>().HasKey(m => m.ProvinceID);
            builder.Entity<Province>().HasIndex(c => c.ProvinceID).IsUnique();
            builder.Entity<Province>().Property(u => u.ProvinceID).UseIdentityColumn();
            builder.Entity<Province>().Property(m => m.Name).IsRequired();
            builder.Entity<Province>().Property(m => m.Code).IsRequired();


            //State
            builder.Entity<State>().HasKey(m => m.StateID);
            builder.Entity<State>().HasIndex(c => c.StateID).IsUnique();
            builder.Entity<State>().Property(u => u.StateID).UseIdentityColumn();
            builder.Entity<State>().Property(m => m.Name).IsRequired();
            builder.Entity<State>().Property(m => m.Code).IsRequired();
        }

    }
}

