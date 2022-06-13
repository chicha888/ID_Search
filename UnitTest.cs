using System;
using Xunit;
using Telegram_BOT;

namespace TestProject
{
    public class UnitTest
    {
        [Fact]
        public void TestFindPassport()
        {
            Passport expectedPassport = new Passport(77284497, "��������� ²�Ĳ����� ��˲ֲ� �������������� ²�Ĳ�� ���� � ���Ͳ����ʲ� �����Ҳ", "KP",
                "206691", "������� ����������� �������", "��������",
                "2012-01-16 00:00:00", "2012-01-17 00:00:00");
            Passport actualPassport = Passport.FindPassport("206691", @"..\..\..\..\Telegram_BOT\PassportDB");
            Assert.Equal(expectedPassport, actualPassport);
        }
        [Fact]
        public void TestValidationDirectory()
        {
            bool expectedResult = true;
            bool actualResult = Passport.DirectoryValidation(@"..\..\..\..\Telegram_BOT\PassportDB");
            Assert.Equal(expectedResult, actualResult);
        }
        [Fact]
        public void TestValidationFile()
        {
            bool expectedResult = true;
            bool actualResult = Passport.FileValidation(@"..\..\..\..\Telegram_BOT\SearchHistory.txt");
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
