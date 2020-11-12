using System;
using Xunit;
using IIG.BinaryFlag;
using IIG.PasswordHashingUtils;
using IIG.DatabaseConnectionUtils;
using System.Data.SqlTypes;
using IIG.CoSFE.DatabaseUtils;
using System.Collections.Generic;

namespace IntegrationTesting
{
    public class TestCheckCredentials
    {
        private const string Server = @"DESKTOP-4UTFR7T\MSSQL_SERVER";
        private const string Database = @"IIG.CoSWE.AuthDB";
        private const bool IsTrusted = true;
        private const string Login = @"NewSA";
        private const string Password = @"12345";
        private const int ConnectionTime = 75;

        AuthDatabaseUtils authDatabaseUtils = new AuthDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTime);

        string[] logins = new string[10] { "login_1", "login_2", "login_3", "login_4", "login_5",
                                           "login_6", "login_7", "login_8", "login_9", "login_10"};
        string[] hash_passwords = new string[10] { "hash_pass_1", "hash_pass_2", "hash_pass_3", "hash_pass_4", "hash_pass_5",
                                                   "hash_pass_6", "hash_pass_7", "hash_pass_7", "hash_pass_9", "hash_pass_10"};

        [Fact]
        public void CheckAllUsers()
        {
            for (int i = 0; i < 10; i++)
            {
                Assert.True(authDatabaseUtils.CheckCredentials(logins[i], PasswordHasher.GetHash(hash_passwords[i])));
            }
            Assert.True(authDatabaseUtils.CheckCredentials("someLogin", PasswordHasher.GetHash(hash_passwords[0])));
            Assert.True(authDatabaseUtils.CheckCredentials("test", "1111B7BFB77DED53B6AE803759D31E48F6F26D6220D79B4E24BD1E35C3930506"));
        }

        [Fact]
        public void CheckNull()
        {
            Assert.False(authDatabaseUtils.CheckCredentials(null, null));
        }

        [Fact]
        public void CheckNonExistentUser()
        {
            Assert.False(authDatabaseUtils.CheckCredentials("Non-existent-user", PasswordHasher.GetHash("Non-existent-user")));
        }

        [Fact]
        public void CheckUserWithWrongLogin()
        {
            Assert.False(authDatabaseUtils.CheckCredentials("Wrong-user-name", PasswordHasher.GetHash(hash_passwords[0])));
        }

        [Fact]
        public void CheckUserWithWrongPass()
        {
            Assert.False(authDatabaseUtils.CheckCredentials(logins[0], PasswordHasher.GetHash("Wrong-user-password")));
        }
    }
}
