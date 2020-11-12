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
    public class TestUpdateCredentials
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
        public void UpdateExistingUser()
        {
            Assert.True(authDatabaseUtils.UpdateCredentials(logins[0], PasswordHasher.GetHash(hash_passwords[0]), "updateLogin_1", PasswordHasher.GetHash("NewPass")));
        }

        [Fact]
        public void UpdateUserWithNullLogin()
        {
            Assert.False(authDatabaseUtils.UpdateCredentials(logins[1], PasswordHasher.GetHash(hash_passwords[1]), null, PasswordHasher.GetHash("newPass")));
        }

        [Fact]
        public void UpdateUserWithNullPass()
        {
            Assert.False(authDatabaseUtils.UpdateCredentials(logins[0], PasswordHasher.GetHash(hash_passwords[0]), "update" + logins[0], null));
        }
        
        [Fact]
        public void UpdateUserWithEmptyLogin()
        {
            Assert.False(authDatabaseUtils.UpdateCredentials(logins[2], PasswordHasher.GetHash(hash_passwords[2]), "", PasswordHasher.GetHash("NEWPass")));
        }

        [Fact]
        public void UpdateUserWithWrongLogin()
        {
            Assert.False(authDatabaseUtils.UpdateCredentials("wrongLogin", PasswordHasher.GetHash(hash_passwords[3]), logins[3], PasswordHasher.GetHash(hash_passwords[3])));
        }
        
        [Fact]
        public void UpdateUserWithWrongPass()
        {
            Assert.False(authDatabaseUtils.UpdateCredentials(logins[4], PasswordHasher.GetHash("WrongPass"), logins[4], PasswordHasher.GetHash("NewPass")));
        }
        
        [Fact]
        public void UpdateUserWithPassLengthLessThan64()
        {
            Assert.False(authDatabaseUtils.UpdateCredentials(logins[5], PasswordHasher.GetHash(hash_passwords[5]), logins[5], "PassLengthLessThan64"));
        }
        
        [Fact]
        public void CheckSqlQueries2()
        {
            string selectSqlRequest = "SELECT [Password] FROM [dbo].[Credentials] WHERE [Login]=";
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'updateLogin_1'"), PasswordHasher.GetHash("NewPass"));
        }
    }
}
