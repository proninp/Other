using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdlParse
{
    public class Dataset
    {
        public string DataSorceRef { get; set; } = "";
        public string DatasetName { get; set; } = "";
        public string DatasetContent { get; set; } = "";
        public string DataSourceName { get; set; }
        public bool IsStoredProcedure { get; set; } = false;
        public Dataset()
        {

        }
        public Dataset(string dataSourceRef,string datasetContent,bool isStoredProcedure)
        {
            DataSorceRef = dataSourceRef;
            DatasetContent = datasetContent;
            IsStoredProcedure = isStoredProcedure;
        }
        public Dataset(string datasetName, string datasetContent): this(datasetName,datasetContent,false) { }
    }
}
