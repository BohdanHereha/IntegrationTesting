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
    public class TestFileWorker
    {

        [Fact]
        public void GetFileNameWithoutFileName()
        {
            Assert.Null(BaseFileWorker.GetFileName("D:\\Lab4"));
            Assert.Null(BaseFileWorker.GetFileName(""));
            Assert.Null(BaseFileWorker.GetFileName(null));
        }

        [Fact]
        public void GetNotExistingFileName()
        {
            Assert.Null(BaseFileWorker.GetFileName("D:\\Lab4\\NotExistingFileName"));
        }

        [Fact]
        public void GetFileWithSpecificName()
        {
            Assert.Equal(".....txt", BaseFileWorker.GetFileName("D:\\Lab4\\.....txt"));
            Assert.Equal("A12#@!#$%^&_+.txt", BaseFileWorker.GetFileName("D:\\Lab4\\A12#@!#$%^&_+.txt"));
            Assert.Equal("Їюяй.txt", BaseFileWorker.GetFileName("D:\\Lab4\\Їюяй.txt"));
        }

        string[] fileNames = new string[14] { "Новий Microsoft Access База данных.accdb",
                                                  "Новий Архив WinRAR.rar", "Новий Документ Microsoft Word.DOCX",
                                                  "Новий Електронна таблиця OpenDocument.ODS",
                                                  "Новий Лист Microsoft Excel.XLSX",
                                                  "Новий Презентация Microsoft PowerPoint.PPTX",
                                                  "Новий Презентація OpenDocument.ODP",
                                                  "Новий Рисунок OpenDocument.ODG",
                                                  "Створити точковий рисунок.bmp",
                                                  "1.png",
                                                  "1.jpeg",
                                                  "1.svg",
                                                  "1.md",
                                                  "1.mp4"};
        [Fact]
        public void GetDiffetentFileName()
        {
            for (int i = 0; i < 14; i++) 
            {
                Assert.Equal(fileNames[i], BaseFileWorker.GetFileName("D:\\Lab4\\" + fileNames[i]));
            }
        }

        [Fact]
        public void GetFilePathWithoutPath()
        {
            Assert.Null(BaseFileWorker.GetPath(""));
            Assert.Null(BaseFileWorker.GetPath(null));
        }

        [Fact]
        public void GetPathForNotExistingFile()
        {
            Assert.Null(BaseFileWorker.GetPath("D:\\Lab4\\NotExistingFileName1.txt"));
        }

        [Fact]
        public void RightGetPath()
        {
            for (int i = 0; i < 14; i++)
            {
                Assert.Equal("D:\\Lab4", BaseFileWorker.GetPath("D:\\Lab4\\" + fileNames[i]));
            }
        }

        [Fact]
        public void GetFullPathWithoutFileName()
        {
            Assert.Null(BaseFileWorker.GetFullPath("D:\\Lab4"));
            Assert.Null(BaseFileWorker.GetPath(""));
            Assert.Null(BaseFileWorker.GetPath(null));
        }

        [Fact]
        public void GetFullPathForNotExistingFile()
        {
            Assert.Null(BaseFileWorker.GetFullPath("D:\\Lab4\\NotExistingFileName2.txt"));
        }

        [Fact]
        public void RightGetFullPath()
        {
            for (int i = 0; i < 14; i++)
            {
                Assert.Equal("D:\\Lab4\\" + fileNames[i], BaseFileWorker.GetFullPath("D:\\Lab4\\" + fileNames[i]));
            }
        }

        [Fact]
        public void MkDirWithoutPath()
        {
            Assert.Throws<System.ArgumentException>(() => BaseFileWorker.MkDir(""));
            Assert.Throws<System.ArgumentNullException>(() => BaseFileWorker.MkDir(null));
        }

        [Fact]
        public void RightMkDir()
        {
            Assert.Equal("D:\\Lab4\\NewMkDir", BaseFileWorker.MkDir("D:\\Lab4\\NewMkDir"));
            Assert.Equal("D:\\Lab4", BaseFileWorker.MkDir("D:\\Lab4"));
        }

        [Fact]
        public void MkDirWithSpecialSymbols()
        {
            string[] fileNames = new string[7] { ":", "*", "?", "<", ">", "|", "\"" };
            for (int i = 0; i < 7; i++)
            {
                Assert.Throws<System.IO.IOException>(() => BaseFileWorker.MkDir("D:\\Lab4\\NewMkDir" + fileNames[i]));
            }
        }

        [Fact]
        public void MkDirWithSpacesName()
        {
            Assert.Equal("D:\\Lab4\\", BaseFileWorker.MkDir("D:\\Lab4\\      "));
            Assert.Equal("D:\\Lab4\\    NewMkDir1", BaseFileWorker.MkDir("D:\\Lab4\\    NewMkDir1    "));
            Assert.Equal("D:\\Lab4\\    \\NewMkDir2", BaseFileWorker.MkDir("D:\\Lab4\\    \\NewMkDir2"));
        }


        [Fact]
        public void MkDirWithForbiddenNames()
        {
            string[] forbiddenFileNames = new string[22] { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5",
                                                          "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5",
                                                          "LPT6", "LPT7", "LPT8", "LPT9" };
            for (int i = 0; i < 22; i++)
            {
                Assert.Throws<System.IO.DirectoryNotFoundException>(() => BaseFileWorker.MkDir(forbiddenFileNames[i]));
            }
        }

        [Fact]
        public void ReadLinesWithoutPath()
        {
            Assert.Null(BaseFileWorker.ReadLines(""));
            Assert.Null(BaseFileWorker.ReadLines(null));
        }

        [Fact]
        public void ReadLinesWithWrongPath()
        {
            Assert.Null(BaseFileWorker.ReadAll("D:\\Lab44\\NotExistingFileName.txt"));
        }

        [Fact]
        public void ReadLinesFromFile()
        {
            Assert.Equal(
                new string[] {
                    "When I am dead, bury me",
                    "In my beloved Ukraine,",
                    "My tomb upon a grave mound high",
                    "Amid the spreading plain,",
                    "So that the fields, the boundless steppes,",
                    "The Dnieper's plunging shore",
                    "My eyes could see, my ears could hear",
                    "The mighty river roar"
                }, BaseFileWorker.ReadLines("D:\\Lab4\\Testament (Zapovit) Taras Shevchenko.txt"));
            Assert.Equal(
                new string[] {
                    "Як умру, то поховайте",
                    "Мене на могилі,",
                    "Серед степу широкого,",
                    "На Вкраїні милій,",
                    "Щоб лани широкополі,",
                    "І Дніпро, і кручі",
                    "Було видно, було чути,",
                    "Як реве ревучий."
                }, BaseFileWorker.ReadLines("D:\\Lab4\\Заповіт.txt"));
        }

        [Fact]
        public void ReadLinesFromDiffetentFileName()
        {
            for (int i = 0; i < 14; i++)
            {
                Assert.NotNull(BaseFileWorker.ReadLines("D:\\Lab4\\" + fileNames[i]));
            }
        }

        [Fact]
        public void ReadAllWithoutPath()
        {
            Assert.Null(BaseFileWorker.ReadAll(""));
            Assert.Null(BaseFileWorker.ReadAll(null));
        }

        [Fact]
        public void ReadAllWithWrongPath()
        {
            Assert.Null(BaseFileWorker.ReadAll("D:\\Lab44\\NotExistingFileName.txt"));
        }

        [Fact]
        public void ReadAllFromFile()
        {
            Assert.Equal(
                new string[] {
                    "When I am dead, bury me",
                    "In my beloved Ukraine,",
                    "My tomb upon a grave mound high",
                    "Amid the spreading plain,",
                    "So that the fields, the boundless steppes,",
                    "The Dnieper's plunging shore",
                    "My eyes could see, my ears could hear",
                    "The mighty river roar"
                }, BaseFileWorker.ReadLines("D:\\Lab4\\Testament (Zapovit) Taras Shevchenko.txt"));
            Assert.Equal(
                new string[] {
                    "Як умру, то поховайте",
                    "Мене на могилі,",
                    "Серед степу широкого,",
                    "На Вкраїні милій,",
                    "Щоб лани широкополі,",
                    "І Дніпро, і кручі",
                    "Було видно, було чути,",
                    "Як реве ревучий."
                }, BaseFileWorker.ReadLines("D:\\Lab4\\Заповіт.txt"));
        }

        [Fact]
        public void ReadAllFromFromDiffetentFileName()
        {
            for (int i = 0; i < 14; i++)
            {
                Assert.NotNull(BaseFileWorker.ReadLines("D:\\Lab4\\" + fileNames[i]));
            }
        }

        [Fact]
        public void TryWriteWithoutPath()
        {
            Assert.False(BaseFileWorker.TryWrite("text", ""));
        }

        [Fact]
        public void TryWriteWithoutFileName()
        {
            Assert.False(BaseFileWorker.TryWrite("text", "D:\\Lab4"));
        }

        [Fact]
        public void TryWriteToNotExistingFile()
        {
            Assert.True(BaseFileWorker.TryWrite("someText", "D:\\Lab4\\someText.txt"));
        }

        [Fact]
        public void TryWriteToFileWithMissingLinkInPath()
        {
            Assert.False(BaseFileWorker.TryWrite("text", "D:\\Lab4\\non\\someText.txt"));
        }

        [Fact]
        public void TryWriteToExistingFile()
        {
            for (int i = 0; i < 14; i++)
            {
                Assert.True(BaseFileWorker.TryWrite("someText", "D:\\Lab4\\" + fileNames[i]));
            }
            for (int i = 0; i < 14; i++)
            {
                Assert.False(BaseFileWorker.TryWrite("someText", "D:\\Lab4\\" + fileNames[i], -1));
            }
            for (int i = 0; i < 14; i++)
            {
                Assert.False(BaseFileWorker.TryWrite("someText", "D:\\Lab4\\" + fileNames[i], 0));
            }
            for (int i = 0; i < 14; i++)
            {
                Assert.True(BaseFileWorker.TryWrite("someText", "D:\\Lab4\\" + fileNames[i], 1));
            }
        }

        [Fact]
        public void WriteWithoutPath()
        {
            Assert.False(BaseFileWorker.Write("someText", ""));
        }

        [Fact]
        public void WriteWithoutFileName()
        {
            Assert.False(BaseFileWorker.Write("someText", "D:\\Lab4"));
        }

        [Fact]
        public void WriteEmptyText()
        {
            Assert.True(BaseFileWorker.Write("", "D:\\Lab4\\someTest.txt"));
        }

        [Fact]
        public void WriteToNotExistingFile()
        {
            Assert.True(BaseFileWorker.Write("someText", "D:\\Lab4\\File.txt"));
        }

        [Fact]
        public void WriteToExistingFile()
        {
            for (int i = 0; i < 14; i++)
            {
                Assert.True(BaseFileWorker.Write("someText", "D:\\Lab4\\" + fileNames[i]));
            }
        }

        [Fact]
        public void WriteToFileWithMissingLinkInPath()
        {
            Assert.False(BaseFileWorker.Write("someText", "D:\\Lab4\\non\\someText.txt"));
        }


        [Fact]
        public void TryCopyWithoutPath()
        {
            Assert.False(BaseFileWorker.TryCopy("", "", true));
        }
        
        [Fact]
        public void TryCopyWithoutFileName()
        {
            Assert.Throws<System.IO.IOException>(() => BaseFileWorker.TryCopy("D:\\Lab4", "D:\\Lab4", true));
        }
        
        [Fact]
        public void TryCopyNotExistingFile()
        {
            Assert.Throws<System.IO.FileNotFoundException>(() => BaseFileWorker.TryCopy("D:\\Lab4\\non.md", "D:\\Lab4\\someText1.md", true));
        }
        
        [Fact]
        public void TryCopyExistingFile()
        {
            for (int i = 0; i < 14; i++)
            {
                Assert.True(BaseFileWorker.TryCopy("D:\\Lab4\\" + fileNames[i], "D:\\Lab4\\Copy\\" + fileNames[i], true));
            }
            for (int i = 0; i < 14; i++)
            {
                Assert.False(BaseFileWorker.TryCopy("D:\\Lab4\\" + fileNames[i], "D:\\Lab4\\Copy\\" + fileNames[i], true, -1));
            }
            for (int i = 0; i < 14; i++)
            {
                Assert.False(BaseFileWorker.TryCopy("D:\\Lab4\\" + fileNames[i], "D:\\Lab4\\Copy\\" + fileNames[i], true, 0));
            }
            for (int i = 0; i < 14; i++)
            {
                Assert.True(BaseFileWorker.TryCopy("D:\\Lab4\\" + fileNames[i], "D:\\Lab4\\Copy\\" + fileNames[i], true, 1));
            }
            for (int i = 0; i < 14; i++)
            {
                Assert.Throws<System.IO.IOException>(() => BaseFileWorker.TryCopy("D:\\Lab4\\" + fileNames[i], "D:\\Lab4\\Copy\\" + fileNames[i], false));
            }
            Assert.True(BaseFileWorker.TryCopy("D:\\Lab4\\test.txt", "D:\\Lab4\\Copy\\text.txt", false));
        }
        

    }
}
