using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace BicImport
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Pronin P.S.")]
    [Serializable()]
    [XmlRoot(Namespace = Params.XMLNS)]
    public class ED807
    {
        [XmlAttribute]
        public string EDNo { get; set; }
        [XmlAttribute]
        public string EDDate { get; set; }
        [XmlAttribute]
        public string EDAuthor { get; set; }
        [XmlAttribute]
        public string CreationReason { get; set; }
        [XmlAttribute]
        public string CreationDateTime { get; set; }
        [XmlAttribute]
        public string InfoTypeCode { get; set; }
        [XmlAttribute]
        public string BusinessDay { get; set; }
        [XmlElement(ElementName = Params.BICDIRECTORYENTRY)]
        public List<BICDirectoryEntry> BICDirectoryEntries { get; set; }
        public int GetBICDirectoryEntryCount() => BICDirectoryEntries.Count;
        public BICDirectoryEntry GetBICDirectoryEntryElement(int i) => BICDirectoryEntries[i];
    }
}
