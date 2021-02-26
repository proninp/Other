using System;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.IO;
using System.IO.Compression;

namespace BicImport
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Pronin P.S.")]
    public class WorkShell
    {
        /// <summary>
        /// Download zip archive from cbr.ru by link
        /// </summary>
        /// <param name="connectionLink">web link from cbr.ru</param>
        /// <param name="fullZipFileName">Directory plus name of new .zip file</param>
        /// <param name="errorText">Exception text, if error appears</param>
        /// <returns></returns>
        public bool DownloadZipFile(string connectionLink, string fullZipFileName, ref string errorText)
        {
            errorText = "";
            if (connectionLink == string.Empty) connectionLink = Params.CBR_DOWNLOAD_LINK;
            if (fullZipFileName == string.Empty)
            {
                string destinationDirectory = Params.ZIP_DOWNLOAD_DIR;
                if (destinationDirectory.Substring(destinationDirectory.Length - 1, 1) != "\\") destinationDirectory += "\\";
                fullZipFileName = string.Concat(destinationDirectory, Params.ZIP_FILE_NAME);
            }
            if (File.Exists(fullZipFileName)) File.Delete(fullZipFileName);
            WebClient webClient = new WebClient();
            try { webClient.DownloadFile(connectionLink, fullZipFileName); }
            catch (Exception ex)
            {
                errorText = ex.Message;
                return false;
            }
            webClient.Dispose();
            return true;
        }
        /// <summary>
        /// Gets .xml file name from archive
        /// </summary>
        /// <param name="fullZipFileName">Full path to .zip file</param>
        /// <param name="errorText">Exception text, if error appears</param>
        /// <returns></returns>
        public string GetXmlFileName(string fullZipFileName, ref string errorText)
        {
            string xmlFileName = "";
            if (fullZipFileName == string.Empty)
            {
                string destinationDirectory = Params.ZIP_DOWNLOAD_DIR;
                if (destinationDirectory.Substring(destinationDirectory.Length - 1, 1) != "\\") destinationDirectory += "\\";
                fullZipFileName = string.Concat(destinationDirectory, Params.ZIP_FILE_NAME);
            }
            try
            {
                ZipArchive zipArchive = ZipFile.OpenRead(fullZipFileName);
                if (zipArchive.Entries.Count == 0)
                {
                    errorText = string.Format("Архив {0} пуст.", fullZipFileName);
                    return "";
                }
                foreach (var entry in zipArchive.Entries) xmlFileName = entry.FullName;
                if (xmlFileName == string.Empty)
                {
                    errorText = string.Format("Не удалось получить файл из архива {0}.", fullZipFileName);
                    return "";
                }
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
                return "";
            }
            return xmlFileName;
        }

        /// <summary>
        /// Unzip archive file
        /// </summary>
        /// <param name="fullZipFileName">>Directory plus name of new .zip file</param>
        /// <param name="unzipDir">Destination directory</param>
        /// <param name="errorText">Exception text, if error appears</param>
        /// <returns></returns>
        public bool UnzipFile(string fullZipFileName, string unzipDir, string xmlFileName, ref string errorText)
        {
            errorText = "";
            string fullXmlFileName = "";
            if (xmlFileName == string.Empty)
            {
                errorText = string.Format("Не удалось получить файл из архива {0}.", fullZipFileName);
                return false;
            }
            if (unzipDir == string.Empty) unzipDir = Params.XML_DOWNLOAD_DIR;
            if (unzipDir.Substring(unzipDir.Length - 1, 1) != "\\") unzipDir += "\\";
            fullXmlFileName = string.Concat(unzipDir, xmlFileName);
            try
            {
                if (File.Exists(fullXmlFileName)) File.Delete(fullXmlFileName);
                ZipFile.ExtractToDirectory(fullZipFileName, unzipDir);
            }
            catch (Exception ex)
            {
                errorText = ex.Message;
                return false;
            }
            string content = File.ReadAllText(fullXmlFileName, Encoding.GetEncoding(Params.WINDOWS_1251));
            File.WriteAllText(fullXmlFileName, content, Encoding.UTF8);
            return true;
        }
        /// <summary>
        /// Parse .xml file to ED807 object
        /// </summary>
        /// <param name="fullXmlFileName">Full .xml path</param>
        /// <param name="errorText">Exception text, if error appears</param>
        /// <returns></returns>
        public ED807 ParseBicList(string fullXmlFileName, ref string errorText)
        {
            errorText = "";
            ED807 eD = new ED807();
            try
            {
                TextReader textReader = new StreamReader(fullXmlFileName);
                eD = (ED807)new XmlSerializer(typeof(ED807)).Deserialize(textReader);
            }
            catch (Exception ex) { errorText = ex.Message; }
            return eD;
        }
        public string GetStandartZipArchiveFullName()
        {
            string destinationDirectory = Params.ZIP_DOWNLOAD_DIR;
            if (destinationDirectory.Substring(destinationDirectory.Length - 1, 1) != "\\") destinationDirectory += "\\";
            return(string.Concat(destinationDirectory, Params.ZIP_FILE_NAME));
        }
        public string GetStandartXmlFileDirectory() => Params.XML_DOWNLOAD_DIR;

    }
}
