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
    public class TestAddCredentials
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
        public void Add10Users()
        {
            for (int i = 0; i < 10; i++) 
            {
                Assert.True(authDatabaseUtils.AddCredentials(logins[i], PasswordHasher.GetHash(hash_passwords[i])));
            }
        }
        
        [Fact]
        public void AddUserWithIdenticalLogin()
        {
            Assert.False(authDatabaseUtils.AddCredentials(logins[0], PasswordHasher.GetHash("somePass")));
        }

        [Fact]
        public void AddUserWithIdenticalPass()
        {
            Assert.True(authDatabaseUtils.AddCredentials("someLogin", PasswordHasher.GetHash(hash_passwords[0])));
        }

        [Fact]
        public void AddUserWithNullLogin()
        {
            Assert.False(authDatabaseUtils.AddCredentials(null, PasswordHasher.GetHash(hash_passwords[0])));
        }

        [Fact]
        public void AddUserWithEmptyLogin()
        {
            Assert.False(authDatabaseUtils.AddCredentials("", PasswordHasher.GetHash(hash_passwords[0])));
        }

        [Fact]
        public void AddUserWithNullPass()
        {
            Assert.False(authDatabaseUtils.AddCredentials("someLogin2", null));
        }

        [Fact]
        public void AddUserWithPassLengthLessThan64()
        {
            Assert.False(authDatabaseUtils.AddCredentials("someLogin3", "PassLengthLessThan64"));
        }
        
        [Fact]
        public void CheckSqlQueries()
        {
            string selectSqlRequest = "SELECT [Password] FROM [dbo].[Credentials] WHERE [Login]=";
            Assert.True(databaseConnection.ExecSql("SELECT [Login] ,[Password] FROM [dbo].[Credentials]"));
            Assert.True(databaseConnection.ExecSql("INSERT INTO [dbo].[Credentials] ([Login] ,[Password]) " +
                        "VALUES ('test', '1111B7BFB77DED53B6AE803759D31E48F6F26D6220D79B4E24BD1E35C3930506')"));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'login_1'"), PasswordHasher.GetHash(hash_passwords[0]));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'login_2'"), PasswordHasher.GetHash(hash_passwords[1]));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'login_3'"), PasswordHasher.GetHash(hash_passwords[2]));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'login_4'"), PasswordHasher.GetHash(hash_passwords[3]));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'login_5'"), PasswordHasher.GetHash(hash_passwords[4]));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'login_6'"), PasswordHasher.GetHash(hash_passwords[5]));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'login_7'"), PasswordHasher.GetHash(hash_passwords[6]));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'login_8'"), PasswordHasher.GetHash(hash_passwords[7]));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'login_9'"), PasswordHasher.GetHash(hash_passwords[8]));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'login_10'"), PasswordHasher.GetHash(hash_passwords[9]));
            Assert.Equal(databaseConnection.GetStrBySql(selectSqlRequest + "'someLogin'"), PasswordHasher.GetHash(hash_passwords[0]));
            Assert.Equal("1111B7BFB77DED53B6AE803759D31E48F6F26D6220D79B4E24BD1E35C3930506", databaseConnection.GetStrBySql(selectSqlRequest + "'test'"));
        }
    }
}
