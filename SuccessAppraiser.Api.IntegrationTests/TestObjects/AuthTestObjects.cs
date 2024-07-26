using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.Api.IntegrationTests.TestObjects
{
    public class AuthTestObjects
    {
        public static ApplicationUser getBaseUser()
        {
            ApplicationUser user = new();
            user.UserName = "BaseUser";
            user.Email = "base@gmail.com";
            return user;
        }
    }
}
