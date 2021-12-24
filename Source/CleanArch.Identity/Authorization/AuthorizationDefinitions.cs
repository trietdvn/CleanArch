﻿using System.ComponentModel;

namespace CleanArch.Identity.Authorization
{
    public static class Actions
    {
        public const string Create = nameof(Create);
        public const string Read = nameof(Read);
        public const string Update = nameof(Update);
        public const string Delete = nameof(Delete);
    }

    public static class Permissions
    {
        public static class Role
        {
            [Description("Create a new Role")]
            public const string Create = nameof(Role) + "." + nameof(Actions.Create);

            [Description("Read roles data (permissions, etc.")]
            public const string Read = nameof(Role) + "." + nameof(Actions.Read);

            [Description("Edit existing Roles")]
            public const string Update = nameof(Role) + "." + nameof(Actions.Update);

            [Description("Delete any Role")]
            public const string Delete = nameof(Role) + "." + nameof(Actions.Delete);
        }

        public static class User
        {
            [Description("Create a new User")]
            public const string Create = nameof(User) + "." + nameof(Actions.Create);

            [Description("Read Users data (Names, Emails, Phone Numbers, etc.)")]
            public const string Read = nameof(User) + "." + nameof(Actions.Read);

            [Description("Edit existing users")]
            public const string Update = nameof(User) + "." + nameof(Actions.Update);

            [Description("Delete any user")]
            public const string Delete = nameof(User) + "." + nameof(Actions.Delete);
        }
    }
}