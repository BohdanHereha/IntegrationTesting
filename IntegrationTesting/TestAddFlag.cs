using System;
using Xunit;
using IIG.BinaryFlag;
using IIG.DatabaseConnectionUtils;
using System.Data.SqlTypes;
using IIG.CoSFE.DatabaseUtils;
using System.Collections.Generic;

namespace IntegrationTesting
{
    public class TestAddFlag
    {
        private const string Server = @"DESKTOP-4UTFR7T\MSSQL_SERVER";
        private const string Database = @"IIG.CoSWE.FlagpoleDB";
        private const bool IsTrusted = true;
        private const string Login = @"NewSA";
        private const string Password = @"12345";
        private const int ConnectionTime = 75;

        FlagpoleDatabaseUtils flagpoleDatabaseUtils = new FlagpoleDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTime);
        DatabaseConnection databaseConnection = new DatabaseConnection(Server, Database, IsTrusted, Login, Password, ConnectionTime);

        MultipleBinaryFlag multipleBinaryFlag_1 = new MultipleBinaryFlag(6, true);
        MultipleBinaryFlag multipleBinaryFlag_2 = new MultipleBinaryFlag(40, true); 
        MultipleBinaryFlag multipleBinaryFlag_3 = new MultipleBinaryFlag(90, false);
        MultipleBinaryFlag multipleBinaryFlag_4 = new MultipleBinaryFlag(150, false);

        [Fact]
        public void AddFlagSuccessfully()
        {
            Assert.True(flagpoleDatabaseUtils.AddFlag("T", multipleBinaryFlag_1.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("t", multipleBinaryFlag_2.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("F", multipleBinaryFlag_3.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("f", multipleBinaryFlag_4.GetFlag()));
        }

        [Fact]
        public void AddFlagNotSuccessful()
        {
            Assert.False(flagpoleDatabaseUtils.AddFlag("T", multipleBinaryFlag_3.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("t", multipleBinaryFlag_4.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("F", multipleBinaryFlag_1.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("f", multipleBinaryFlag_2.GetFlag()));
        }

        [Fact]
        public void AddFlagWithFlagValue()
        {
            Assert.True(flagpoleDatabaseUtils.AddFlag("f", false));
            Assert.True(flagpoleDatabaseUtils.AddFlag("t", true));
            Assert.True(flagpoleDatabaseUtils.AddFlag("F", false));
            Assert.True(flagpoleDatabaseUtils.AddFlag("T", true));
        }
        
        [Fact]
        public void AddFlagWithFlagValueOf0()
        {
            Assert.True(flagpoleDatabaseUtils.AddFlag("TF", multipleBinaryFlag_4.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("TF", multipleBinaryFlag_1.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("FT", multipleBinaryFlag_4.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("FT", multipleBinaryFlag_1.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("tF", multipleBinaryFlag_4.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("tF", multipleBinaryFlag_1.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("fT", multipleBinaryFlag_4.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("fT", multipleBinaryFlag_1.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("TFtttttffffftftftft", multipleBinaryFlag_4.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("TFtttttffffftftftft", multipleBinaryFlag_1.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("FftftftFTFTFfTFFTTF", multipleBinaryFlag_4.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("FftftftFTFTFfTFFTTF", multipleBinaryFlag_1.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("TTTTTTTTTTTTTTTTF", multipleBinaryFlag_4.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("TTTTTTTTTTTTTTTTF", multipleBinaryFlag_1.GetFlag()));
        }

        [Fact]
        public void AddFlagWithNullFlagView()
        {
            Assert.False(flagpoleDatabaseUtils.AddFlag(null, multipleBinaryFlag_1.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag(null, multipleBinaryFlag_4.GetFlag()));
        }

        [Fact]
        public void AddFlagWithWrongFlagView()
        {
            Assert.False(flagpoleDatabaseUtils.AddFlag("True", multipleBinaryFlag_1.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("true", multipleBinaryFlag_2.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("False", multipleBinaryFlag_3.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("false", multipleBinaryFlag_4.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("qT", multipleBinaryFlag_1.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("1t", multipleBinaryFlag_2.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("WF", multipleBinaryFlag_3.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("_f", multipleBinaryFlag_4.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("T@", multipleBinaryFlag_1.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("t/", multipleBinaryFlag_2.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("F.", multipleBinaryFlag_3.GetFlag()));
            Assert.False(flagpoleDatabaseUtils.AddFlag("f+", multipleBinaryFlag_4.GetFlag()));
        }
       
        [Fact]
        public void AddFlagWithRightFlagView()
        {
            string t = "TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT";
            Assert.True(flagpoleDatabaseUtils.AddFlag(t+t+t+t+t+t+t+t+t, multipleBinaryFlag_1.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("ttttt", multipleBinaryFlag_1.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("FFFFFFF", multipleBinaryFlag_3.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("fffffff", multipleBinaryFlag_4.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("Tt", multipleBinaryFlag_1.GetFlag()));
            Assert.True(flagpoleDatabaseUtils.AddFlag("Ff", multipleBinaryFlag_4.GetFlag()));
        }

        [Fact]
        public void CheckSqlQueries4()
        {
            string selectSqlRequest = "SELECT TOP 1 [MultipleBinaryFlagValue] FROM[dbo].[MultipleBinaryFlags] " +
                                      "WHERE [MultipleBinaryFlagView]=";
            Assert.True(databaseConnection.ExecSql("SELECT [MultipleBinaryFlagID], [MultipleBinaryFlagView], " +
                                                  "[MultipleBinaryFlagValue] FROM[dbo].[MultipleBinaryFlags]"));

            Assert.True(databaseConnection.GetBoolBySql(selectSqlRequest + "'T'"));
            Assert.False(databaseConnection.GetBoolBySql(selectSqlRequest + "'F'"));
            Assert.True(databaseConnection.GetBoolBySql(selectSqlRequest + "'t'"));
            Assert.False(databaseConnection.GetBoolBySql(selectSqlRequest + "'f'"));
        }
    }
}
