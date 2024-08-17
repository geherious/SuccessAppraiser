using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.UnitTests.TestObjects
{
    public class AuthTestObjects
    {
        public static ApplicationUser GetApplicationUser()
        {
            ApplicationUser user = new ApplicationUser();
            user.Id = Guid.Parse("4ad5e212-8476-48ba-bdf1-8d468c37c7ad");
            user.UserName = "Username";
            return user;
        }

        public static RefreshToken GetRefreshToken()
        {
            RefreshToken token = new RefreshToken();
            token.UserId = GetApplicationUser().Id;
            token.Token = Guid.Parse("077f3c62-aad8-462b-9a5d-5d942d337d09").ToString();
            token.Expires = DateTime.Now.AddDays(2);
            return token;
        }
    }
}
