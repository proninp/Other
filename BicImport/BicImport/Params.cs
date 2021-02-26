using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BicImport
{
    public class Params
    {
        public const string XMLNS = "urn:cbr-ru:ed:v2.0";
        public const string BICDIRECTORYENTRY = "BICDirectoryEntry";
        public const string ACCRSTRLIST = "AccRstrList";
        public const string ACCOUNTS = "Accounts";
        public const string CBR_DOWNLOAD_LINK = @"http://www.cbr.ru/s/newbik";
        public const string ZIP_FILE_NAME = @"BicZip.zip";
        public const string ZIP_DOWNLOAD_DIR = @"\\SampleDir\BicImport\";
        public const string XML_DOWNLOAD_DIR = @"\\SampleDir\BicImport\Xml\";
        public const string WINDOWS_1251 = "Windows-1251";
    }
}
