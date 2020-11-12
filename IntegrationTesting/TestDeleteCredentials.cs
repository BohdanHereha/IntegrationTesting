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
    public class TestDeleteCredentials
    {
        private const string Server = @"DESKTOP-4UTFR7T\MSSQL_SERVER";
        private const string Database = @"IIG.CoSWE.AuthDB";
        private const bool IsTrusted = true;
        private const string Login = @"NewSA";
        private const string Password = @"12345";
        private const int ConnectionTime = 75;

        AuthDatabaseUtils authDatabaseUtils = new AuthDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTime);
        DatabaseConnection databaseConnection = new DatabaseConnection(Server, Database, IsTrusted, Login, Password, ConnectionTime);

        string[] logins = new string[10] { "login_1", "login_2", "login_3", "login_4", "login_5",
                                           "login_6", "login_7", "login_8", "login_9", "login_10"};
        string[] hash_passwords = new string[10] { "hash_pass_1", "hash_pass_2", "hash_pass_3", "hash_pass_4", "hash_pass_5",
                                                   "hash_pass_6", "hash_pass_7", "hash_pass_7", "hash_pass_9", "hash_pass_10"};

        [Fact]
        public void DeleteExistingUser()
        {
            Assert.True(authDatabaseUtils.DeleteCredentials(logins[9], PasswordHasher.GetHash(hash_passwords[9])));
        }


        [Fact]
        public void DeleteUserWithNullLogin()
        {
            Assert.False(authDatabaseUtils.DeleteCredentials(null, PasswordHasher.GetHash(hash_passwords[6])));
        }

        [Fact]
        public void DeleteUserWithNullPass()
        {
            Assert.False(authDatabaseUtils.DeleteCredentials(logins[6], null));
        }

        [Fact]
        public void DeleteNonExistingUser()
        {
            Assert.False(authDatabaseUtils.DeleteCredentials("NonExistingUser", PasswordHasher.GetHash("NonExistingUserPass")));
        }

        [Fact]
        public void DeleteExistingUserWithWrongLogin()
        {
            Assert.False(authDatabaseUtils.DeleteCredentials("wrongLogin", PasswordHasher.GetHash(hash_passwords[5])));
        }

        [Fact]
        public void DeleteExistingUserWithWrongPass()
        {
            Assert.False(authDatabaseUtils.DeleteCredentials(logins[6], PasswordHasher.GetHash("wrongPass")));
        }

        [Fact]
        public void CheckSqlQueries3()
        {
            string selectSqlRequest = "SELECT [Password] FROM [dbo].[Credentials] WHERE [Login]=";
            Assert.False(databaseConnection.ExecSql(selectSqlRequest+ logins[9]));
        }
    }
}
