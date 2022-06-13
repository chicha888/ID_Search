using System;
using System.Collections.Generic;
using System.IO;

namespace Telegram_BOT
{
    public class Passport
    {
        public ulong ID { get; set; }
        public string policeDepartment { get; set; }
        public string pSeries { get; set; }
        public string pNumber { get; set; }
        public string pType { get; set; }
        public string pStatus { get; set; }
        public string theftDate { get; set; }
        public string insertDate { get; set; }
        public Passport(ulong ID, string policeDepartment, string pSeries, string pNumber, string pType, string pStatus,
            string theftDate, string insertDate)
        {
            this.ID = ID;
            this.policeDepartment = policeDepartment;
            this.pSeries = pSeries;
            this.pNumber = pNumber;
            this.pType = pType;
            this.pStatus = pStatus;
            this.theftDate = theftDate;
            this.insertDate = insertDate;
            
        }
        public override bool Equals(object obj)
        {
            Passport pData = obj as Passport;
            if (pData == null) return false;
            else if ((this.ID == pData.ID) && (this.policeDepartment == pData.policeDepartment) && (this.pSeries == pData.pSeries) &&
                (this.pNumber == pData.pNumber) && (this.pType == pData.pType) &&
                (this.pStatus == pData.pStatus) && (this.theftDate == pData.theftDate) && (this.insertDate == insertDate)) return true;
            else return false;
        }
        public string ToStringUkr()
        {
            string passportInfo = "\r\nУнікальний номер: " + ID + "\r\nНазва відділу: " + policeDepartment + "\r\nСерія: " + pSeries +
                "\r\nНомер: " + pNumber + "\r\nТип: " + pType + "\r\nСтатус: " + pStatus +
                "\r\nДата втрати паспорту: " + theftDate + "\r\nДата подання заяви: " + insertDate;
            return passportInfo;
        }
        public string ToStringEng()
        {
            string passportInfoString = "\r\nID: " + ID + "\r\nDepartment name: " + policeDepartment + "\r\nSeries: " + pSeries +
                "\r\nNumber: " + pNumber + "\r\nType: " + pType + "\r\nStatus: " + pStatus +
                "\r\nPassport loss date: " + theftDate + "\r\nDate of application submission: " + insertDate;
            return passportInfoString;
        }
        public static Passport FindPassport(string passportNumber, string pathToFolder)
        {
            string[] fileNames = Directory.GetFiles(pathToFolder, "*.csv");
            if (fileNames.Length == 0)
            {
                return null;
            }
            foreach (string fileName in fileNames)
            {
                string line;
                string[] passport;
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        passport = line.Split(';');
                        if (passport[3] == passportNumber)
                        {
                            return new Passport( Convert.ToUInt64(passport[0]), passport[1], passport[2], passport[3], passport[4], passport[5], passport[6], passport[7]);
                        }
                    }
                }
            }
            return null;
        }
        public static void WriteResultToFile(string number, string pathToFile, bool flag)
        {
            string str;
            if (flag) str = "Successful search for a passport with a number : " + number;
            else str = "Unsuccessful search for a passport with a number : " + number;
            using (StreamWriter sw = new StreamWriter(pathToFile, true))
            {
                sw.WriteLine(str);
            }
        }
        public static bool DirectoryValidation(string pathToFolder)
        {
            if (!Directory.Exists(pathToFolder))
            {
                Console.WriteLine("Папки с базою даних не існує!");
                return false;
            }
            if (Directory.GetFiles(pathToFolder).Length == 0)
            {
                Console.WriteLine("Папка с базою даних порожня!");
                return false;
            }
            return true;
        }
        public static bool FileValidation(string pathToFile)
        {
            if (!File.Exists(pathToFile))
            {
                Console.WriteLine("Файл для веденя історії пошуку не уснує!");
                return false;
            }
            return true;
        }
        
    }
}
