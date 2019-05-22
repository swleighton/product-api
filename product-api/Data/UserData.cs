using System;
using System.Collections.Generic;
using productapi.Helpers;
using productapi.Models;

namespace productapi.Data
{
    public static class UserData
    {
        private static Dictionary<string, string> Cache;
        private static object CacheLock = new object();
        public static Dictionary<string, string> User
        {
            get
            {
                lock (CacheLock)
                {
                    if (Cache == null)
                    {
                        Cache = new Dictionary<string, string>
                        {
                            { "admin", "$MYHASH$V1$10000$Lm2xhfu/K0/1Pk1BeFeWdVz4YNhXnTwt4fk2fjJVWYK/iHlV" }
                        };
                    }
                    return Cache;
                }
            }
        }

        public static bool ValidateUser(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                if (User.ContainsKey(userName))
                {
                    return SecurePasswordHasher.Verify(password, User[userName]);
                }
            }

            return false;

        }
    }
}
