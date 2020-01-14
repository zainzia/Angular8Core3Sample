using Angular8Core3Sample.Models.DataModels.Catalog;

namespace Angular8Core3Sample.Models.Identity
{
    public class Address
    {

        public int AddressID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public Province Province { get; set; }

        public State State { get; set; }

        public string Province2 { get; set; }

        public string PostalCode { get; set; }

        public string ZipCode { get; set; }

        public Country Country { get; set; }

        public string PhoneNumber { get; set; }
    }
}