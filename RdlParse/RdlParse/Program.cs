using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace RdlParse
{
    /// <summary>
    /// This is a fun project for .rdl-files parsing, getting datasets code and finding, wich of existing datasets has a stored procedures
    /// rdlDir - .rdl files directory. All .rdl files should located in that directory
    /// newRdlDir - directory, where processed files will be stored
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            userName = userName.Substring(userName.IndexOf("\\")+1);
            string dwnldDir = string.Format("{0}{1}{2}", Constants.USERDIR, userName, @"\Downloads\");
            string storedProceduresFile = dwnldDir + "storedprocedurs.txt";
            string rdlDir = dwnldDir + @"rdl\";
            string newRdlDir = dwnldDir +  @"new-rdl\";
            string requairedReportsListFullFilename = dwnldDir + "needed_reports.txt";
            if (!Directory.Exists(rdlDir))
                ShowConsoleMessage("There is no rdl directory.", ConsoleColor.Red, true, true);
                
            string[] files = Directory.GetFiles(rdlDir, "*.rdl", SearchOption.AllDirectories);
            if (files.Length == 0)
                ShowConsoleMessage("There is no .rdl files in directory:\n" + rdlDir, ConsoleColor.Red, true, true);
                
            if (!Directory.Exists(newRdlDir))
                Directory.CreateDirectory(newRdlDir);
            List<RDLReport> allRdlReportsList = new List<RDLReport>();
            List<RDLReport> requiredRdlReportsList = new List<RDLReport>();
            List<string> requiredReportsList = ReadDataFromFileToList(requairedReportsListFullFilename);
            List<Tuple<string, string, string>> storedProceduresList = new List<Tuple<string, string, string>>();
            foreach (string filePath in files)
            {
                RDLReport rDLReport = new RDLReport(filePath, rdlDir, newRdlDir, File.ReadAllText(filePath, Encoding.UTF8));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rDLReport.Content);
                XmlNodeList xmlDataSetList = xmlDoc.GetElementsByTagName(Constants.DATASETS_TAG);
                foreach (XmlNode dataSet in xmlDataSetList)
                {
                    string dataSetName = dataSet.Attributes["Name"].Value;
                    foreach (XmlNode querry in dataSet)
                        if (querry.Name.Equals(Constants.QUERY_NODE_TAG))
                        {
                            Dataset dataset = new Dataset();
                            dataset.DatasetName = dataSetName;
                            foreach (XmlNode item in querry.ChildNodes)
                                switch (item.Name)
                                {
                                    case Constants.COMMAND_TYPE_TAG:
                                        if (item.InnerText == Constants.STORED_PROC)
                                            dataset.IsStoredProcedure = true;
                                        break;
                                    case Constants.COMMAND_TEXT_TAG:
                                        dataset.DatasetContent = FormatTSQLScript(item.InnerText);
                                        break;
                                    case Constants.DATA_SOURCE_TAG:
                                        dataset.DataSourceName = item.InnerText;
                                        break;
                                }
                            XmlNodeList xmlDataSources = xmlDoc.GetElementsByTagName(Constants.DATASOURCE_TAG);
                            for (int j = 0; j < xmlDataSources.Count; j++)
                                if (xmlDataSources[j].Attributes["Name"].Value == dataset.DataSourceName)
                                    foreach (XmlNode dataSourceChilds in xmlDataSources[j].ChildNodes)
                                        if (dataSourceChilds.Name == Constants.DATASOURCEREF_TAG)
                                        {
                                            dataset.DataSorceRef = dataSourceChilds.InnerText;
                                            while (dataset.DataSorceRef.Contains("/"))
                                                dataset.DataSorceRef = (dataset.DataSorceRef.IndexOf("/") == 0) ? dataset.DataSorceRef.Replace("/", "") : dataset.DataSorceRef.Replace("/", "_");
                                        }
                            if (dataset.IsStoredProcedure)
                                storedProceduresList.Add(new Tuple<string, string, string>(rDLReport.FileName, dataset.DataSorceRef, dataset.DatasetContent));
                            rDLReport.DataSetList.Add(dataset);
                        }
                }
                allRdlReportsList.Add(rDLReport);
            }
            allRdlReportsList = allRdlReportsList.Distinct().ToList();
            string reqStoredProc = "";
            foreach (var item in allRdlReportsList)
                if (requiredReportsList.Contains(item.FileNameWithoutExtension) || requiredReportsList.Count == 0)
                {
                    requiredRdlReportsList.Add(item);
                    if (!File.Exists(item.NewFullPath))
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(item.NewFullPath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(item.NewFullPath));
                        File.Copy(item.FullPath, item.NewFullPath, true);
                    }

                    for (int i = 0; i < item.DataSetList.Count; i++)
                    {
                        CreateFile(string.Format("{0}{1}{2}{3}{4}",
                            item.NewPathWithoutFileName, (item.DataSetList[i].IsStoredProcedure ? "sp_" : "query_"),
                            (!item.DataSetList[i].DataSorceRef.Equals("") ? item.DataSetList[i].DataSorceRef : "Dataset_" + (i + 1)),
                            "_" + item.DataSetList[i].DatasetName,".txt"), item.DataSetList[i].DatasetContent);
                        if (item.DataSetList[i].IsStoredProcedure)
                            reqStoredProc += item.NewFullPath + ";" + item.FileNameWithoutExtension + ";" + item.DataSetList[i].DataSorceRef + 
                                ";" + item.DataSetList[i].DatasetContent + "\n";
                    }
                }
            if (!reqStoredProc.Equals(""))
                CreateFile(storedProceduresFile, reqStoredProc);
            ShowConsoleMessage("All Done. Requared reports: " + requiredRdlReportsList.Count + "\nTotal reports count: " + allRdlReportsList.Count,
                ConsoleColor.Green, true);
        }
        /// <summary>
        /// Creates text file with reports full names within rdl directory
        /// </summary>
        /// <param name="newFilePath"></param>
        /// <param name="content"></param>
        public static void CreateFile(string newFilePath, string content)
        {
            if (!Directory.Exists(Path.GetDirectoryName(newFilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(newFilePath));
            if (!File.Exists(newFilePath))
            {
                File.Create(newFilePath).Dispose();
                using (TextWriter tw = new StreamWriter(newFilePath))
                    tw.Write(content);
            }
            else if (File.Exists(newFilePath))
                using (TextWriter tw = new StreamWriter(newFilePath))
                    tw.Write(content);
        }
        /// <summary>
        /// Read text file line by line
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<string> ReadDataFromFileToList(string filename)
        {
            List<string> list = new List<string>();
            if (!File.Exists(filename))
                return list;
            int bufferSize = 128;
            using (var fileStream = File.OpenRead(filename))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                        if (line.Length != 0)
                            list.Add(line);
                }
            return list;
        }
        /// <summary>
        /// https://docs.microsoft.com/ru-ru/dotnet/api/microsoft.sqlserver.transactsql.scriptdom?view=sql-dacfx-140.3881.1
        /// SQL Scripts formatter
        /// </summary>
        /// <param name="script">query string</param>
        /// <returns></returns>
        public static string FormatTSQLScript(string script)
        {
            var query = script;
            var parser = new TSql120Parser(false);
            IList<ParseError> errors;
            var parsedQuery = parser.Parse(new StringReader(query), out errors);
            if (errors.Count > 0)
                foreach (var err in errors)
                    ShowConsoleMessage(err.Message, ConsoleColor.Red, false);
            var generator = new Sql120ScriptGenerator(new SqlScriptGeneratorOptions()
            {
                KeywordCasing = KeywordCasing.Uppercase,
                IncludeSemicolons = true,
                NewLineBeforeFromClause = true,
                NewLineBeforeOrderByClause = true,
                NewLineBeforeWhereClause = true,
                AlignClauseBodies = false
            });
            string formattedQuery;
            generator.GenerateScript(parsedQuery, out formattedQuery);
            return formattedQuery;
        }
        /// <summary>
        /// Method shows information console message
        /// </summary>
        /// <param name="message">Message string</param>
        /// <param name="consoleColor">Foreground console color</param>
        /// <param name="isStopProcessing">Console.ReadLine executes befor exit if this param set to true</param>
        public static void ShowConsoleMessage(string message, ConsoleColor consoleColor, bool isStopProcessing, bool isExitEnviron = false)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(message);
            if (isStopProcessing)
                Console.ReadLine();
            Console.ResetColor();
            if (isExitEnviron)
                Environment.Exit(0);
        }
    }
}
