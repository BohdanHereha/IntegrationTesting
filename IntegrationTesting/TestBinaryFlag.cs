using System;
using Xunit;
using IIG.BinaryFlag;
using IIG.DatabaseConnectionUtils;
using System.Data.SqlTypes;
using IIG.CoSFE.DatabaseUtils;
using System.Collections.Generic;
using IIG.FileWorker;

namespace IntegrationTesting
{
    public class TestBinaryFlag
    {
        MultipleBinaryFlag multipleBinaryFlag_1 = new MultipleBinaryFlag(6, true);
        MultipleBinaryFlag multipleBinaryFlag_2 = new MultipleBinaryFlag(40, true);
        MultipleBinaryFlag multipleBinaryFlag_3 = new MultipleBinaryFlag(90, false);
        MultipleBinaryFlag multipleBinaryFlag_4 = new MultipleBinaryFlag(150, false);

        [Fact]
        public void AddFlagSuccessfullyToFile()
        {
            Assert.True(BaseFileWorker.Write(multipleBinaryFlag_1.GetFlag().ToString(), "D:\\Lab4\\Flagpole.txt"));
            Assert.Equal(multipleBinaryFlag_1.GetFlag().ToString(), BaseFileWorker.ReadAll("D:\\Lab4\\Flagpole.txt"));
            Assert.True(BaseFileWorker.Write(multipleBinaryFlag_2.GetFlag().ToString(), "D:\\Lab4\\Flagpole.txt"));
            Assert.Equal(multipleBinaryFlag_2.GetFlag().ToString(), BaseFileWorker.ReadAll("D:\\Lab4\\Flagpole.txt"));
            Assert.True(BaseFileWorker.Write(multipleBinaryFlag_3.GetFlag().ToString(), "D:\\Lab4\\Flagpole.txt"));
            Assert.Equal(multipleBinaryFlag_3.GetFlag().ToString(), BaseFileWorker.ReadAll("D:\\Lab4\\Flagpole.txt"));
            Assert.True(BaseFileWorker.Write(multipleBinaryFlag_4.GetFlag().ToString(), "D:\\Lab4\\Flagpole.txt"));
            Assert.Equal(multipleBinaryFlag_4.GetFlag().ToString(), BaseFileWorker.ReadAll("D:\\Lab4\\Flagpole.txt"));
        }
    }
}
