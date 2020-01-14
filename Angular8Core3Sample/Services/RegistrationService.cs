
using Angular8Core3Sample.Data;
using Angular8Core3Sample.Models.Identity;

using Microsoft.AspNetCore.Identity;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Angular8Core3Sample.Models.DataModels.Catalog;

namespace Angular8Core3Sample.Services
{

    public class RegistrationService
    {

        private UserManager<ApplicationUser> _userManager { get; set; }

        private ApplicationDbContext _dbContext { get; set; }

        private IEnumerable<IdentityError> errors { get; set; }


        public RegistrationService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _dbContext = context;
        }


        private async Task<ApplicationUser> RegisterUser(RegistrationRequest registrationRequest)
        {
            var now = DateTime.Now;

            // create a new Item with the client-sent json data
            var user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registrationRequest.UserProfile.Username,
                Email = registrationRequest.UserProfile.Email,
                FirstName = registrationRequest.UserProfile.FirstName,
                LastName = registrationRequest.UserProfile.LastName,
                CreatedDate = now,
                LastModifiedDate = now,
                PhoneNumber = registrationRequest.UserProfile.PhoneNumber,
                EmailConfirmed = false,
                LockoutEnabled = false,
                Id = Guid.NewGuid().ToString()
            };

            // Add the user to the Db with the choosen password
            var result = await _userManager.CreateAsync(user, registrationRequest.Password).ConfigureAwait(false);

            errors = result.Errors;

            if (result.Succeeded)
            {
                // Assign the user to the 'RegisteredUser' role.
                await _userManager.AddToRoleAsync(user, "RegisteredUser").ConfigureAwait(false);

                // persist the changes into the Database.
                _dbContext.SaveChanges();

                return user;
            }

            return null;
        }


        private UserProfile RegisterUserProfile(RegistrationRequest registrationRequest)
        {

            Province province = null;
            if(registrationRequest.UserProfile.Address.Province != null)
            {
                province = _dbContext.Provinces.FirstOrDefault(x => x.ProvinceID == registrationRequest.UserProfile.Address.Province.ProvinceID);
            }

            State state = null;
            if (registrationRequest.UserProfile.Address.State != null)
            {
                state = _dbContext.States.FirstOrDefault(x => x.StateID == registrationRequest.UserProfile.Address.State.StateID);
            }

            var address = new Address
            {
                Address1 = registrationRequest.UserProfile.Address.Address1,
                Address2 = registrationRequest.UserProfile.Address.Address2,
                City = registrationRequest.UserProfile.Address.City,
                Country = _dbContext.Countries.FirstOrDefault(x => x.CountryID == registrationRequest.UserProfile.Address.Country.CountryID),
                FirstName = registrationRequest.UserProfile.Address.FirstName,
                LastName = registrationRequest.UserProfile.Address.LastName,
                PhoneNumber = registrationRequest.UserProfile.Address.PhoneNumber,
                PostalCode = registrationRequest.UserProfile.Address.PostalCode,
                Province = province,
                Province2 = registrationRequest.UserProfile.Address.Province2,
                State = state,
                ZipCode = registrationRequest.UserProfile.Address.ZipCode
            };

            _dbContext.Addresses.Add(address);

            var userProfile = new UserProfile
            {
                Address = _dbContext.Addresses.FirstOrDefault(x => x.AddressID == address.AddressID),
                Email = registrationRequest.UserProfile.Email,
                FirstName = registrationRequest.UserProfile.FirstName,
                LastName = registrationRequest.UserProfile.LastName,
                Language = _dbContext.Languages.FirstOrDefault(x => x.LanguageID == registrationRequest.UserProfile.Language.LanguageID),
                PhoneNumber = registrationRequest.UserProfile.PhoneNumber,
                Username = registrationRequest.UserProfile.Username,
                User = registrationRequest.UserProfile.User
            };

            _dbContext.UserProfiles.Add(userProfile);
            _dbContext.SaveChanges();

            return userProfile;
        }
        

        public async Task<RegistraionResult> RegisterNewUser(RegistrationRequest registrationRequest)
        {

            if(registrationRequest == null)
            {
                return new RegistraionResult
                {
                    Result = RegistraionResultEnum.Failed,
                    UserProfile = null
                };
            }


            if(!registrationRequest.ClientId.Equals("northZ.club", StringComparison.OrdinalIgnoreCase))
            {
                return new RegistraionResult
                {
                    Result = RegistraionResultEnum.Failed,
                    UserProfile = null
                };
            }


            // check if the Username/Email already exists
            ApplicationUser user = await _userManager.FindByNameAsync(registrationRequest.UserProfile.Username).ConfigureAwait(false);
            if (user != null)
            {
                return new RegistraionResult 
                {
                    Result = RegistraionResultEnum.UsernameAlreadyExists,
                    UserProfile = null,
                    Errors = new List<string>
                    {
                        "Username is already registered"
                    }
                };
            }

            user = await _userManager.FindByEmailAsync(registrationRequest.UserProfile.Email).ConfigureAwait(false);
            if (user != null)
            {
                return new RegistraionResult
                {
                    Result = RegistraionResultEnum.EmailAlreadyRegistered,
                    UserProfile = null,
                    Errors = new List<string>
                    {
                        "Email Address is already registered"
                    }
                };
            }

            try
            {
                var applicationUser = await RegisterUser(registrationRequest).ConfigureAwait(false);

                if(errors.Any())
                {
                    var errorStrings = new List<string>();

                    foreach(var error in errors)
                    {
                        errorStrings.Add(error.Description);
                    }

                    return new RegistraionResult
                    {
                        Result = RegistraionResultEnum.Failed,
                        UserProfile = null,
                        Errors = errorStrings
                    };
                }

                registrationRequest.UserProfile.User = applicationUser;

                var userProfile = RegisterUserProfile(registrationRequest);

                var userProfileUser = new ApplicationUser
                {
                    FirstName = userProfile.User.FirstName,
                    LastName = userProfile.User.LastName,
                    Id = userProfile.User.Id,
                    UserName = userProfile.User.UserName,
                    Email = userProfile.User.Email,
                    SecurityStamp = null,
                    ConcurrencyStamp = null
                };

                userProfile.User = userProfileUser;

                return new RegistraionResult
                {
                    Result = RegistraionResultEnum.Succeeded,
                    UserProfile = userProfile,
                    Errors = null
                };

            }
            catch (Exception ex)
            {
                return new RegistraionResult
                {
                    Result = RegistraionResultEnum.Failed,
                    UserProfile = null,
                    Errors = new List<string>
                    {
                        ex.ToString()
                    }
                };
            }
        }

    }

}

