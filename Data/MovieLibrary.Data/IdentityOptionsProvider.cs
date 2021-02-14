﻿namespace MovieLibrary.Data
{
    using System;

    using Microsoft.AspNetCore.Identity;
    public static class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions options)
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;
            options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 15, 0);
            options.User.RequireUniqueEmail = true;
        }
    }
}
