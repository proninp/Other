using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdlParse
{
    public class RDLReport
    {
        public string FileName { get; set; }
        public string FileNameWithoutExtension { get; }
        public string FullPath { get; set; }
        public string NewFullPath { get; set; }
        public string NewPathWithoutFileName { get; set; }
        public string Content { get; set; }
        public List<Dataset> DataSetList { get; set; }
        public RDLReport(string fullPath, string oldRdlDirectory, string newRdlDirectory, string content)
        {
            FullPath = fullPath;
            FileName = Path.GetFileName(fullPath);
            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);
            NewFullPath = Path.GetDirectoryName(fullPath.Replace(oldRdlDirectory, newRdlDirectory)) + @"\" + FileNameWithoutExtension + @"\" + FileName;
            NewPathWithoutFileName = NewFullPath.Replace(FileName, "");
            Content = content;
            DataSetList = new List<Dataset>();
        }

    }
}
