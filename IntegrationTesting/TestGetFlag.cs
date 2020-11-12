using System;
using Xunit;
using IIG.BinaryFlag;
using IIG.DatabaseConnectionUtils;
using System.Data.SqlTypes;
using IIG.CoSFE.DatabaseUtils;
using System.Collections.Generic;

namespace IntegrationTesting
{
    public class TestGetFlag
    {
        private const string Server = @"DESKTOP-4UTFR7T\MSSQL_SERVER";
        private const string Database = @"IIG.CoSWE.FlagpoleDB";
        private const bool IsTrusted = true;
        private const string Login = @"NewSA";
        private const string Password = @"12345";
        private const int ConnectionTime = 75;

        FlagpoleDatabaseUtils flagpoleDatabaseUtils = new FlagpoleDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTime);


        [Fact]
        public void GetFlagSuccessfully()
        {
            string flagView_1;
            bool? flagValue_1;
            Assert.True(flagpoleDatabaseUtils.GetFlag(95, out flagView_1, out flagValue_1));
            Assert.True(flagpoleDatabaseUtils.GetFlag(102, out flagView_1, out flagValue_1));
            Assert.True(flagpoleDatabaseUtils.GetFlag(115, out flagView_1, out flagValue_1));
        }

        [Fact]
        public void GetFlagNotSuccessful()
        {
            string flagView_1;
            bool? flagValue_1;
            Assert.False(flagpoleDatabaseUtils.GetFlag(-1, out flagView_1, out flagValue_1));
            Assert.False(flagpoleDatabaseUtils.GetFlag(0, out flagView_1, out flagValue_1));
            Assert.False(flagpoleDatabaseUtils.GetFlag(94, out flagView_1, out flagValue_1));
            Assert.False(flagpoleDatabaseUtils.GetFlag(116, out flagView_1, out flagValue_1));
        }

    }
}