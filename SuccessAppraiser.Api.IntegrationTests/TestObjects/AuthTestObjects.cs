using Microsoft.AspNetCore.Identity;
using SuccessAppraiser.Data.Entities;
using System;

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
