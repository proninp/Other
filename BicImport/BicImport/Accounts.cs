using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BicImport
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Pronin P.S.")]
    [Serializable()]
    [XmlRoot]
    public class Accounts
    {
        [XmlAttribute]
        public string Account { get; set; }
        [XmlAttribute]
        public string RegulationAccountType { get; set; }
        [XmlAttribute]
        public string CK { get; set; }
        [XmlAttribute]
        public string AccountCBRBIC { get; set; }
        [XmlAttribute]
        public string DateIn { get; set; }
        [XmlAttribute]
        public string AccountStatus { get; set; }
        [XmlElement(ElementName = Params.ACCRSTRLIST, IsNullable = true)]
        public List<AccRstrList> AccRstrLists { get; set; }
        public int GetAccRstrListsCount() => AccRstrLists.Count;
        public AccRstrList GetAccRstrListsElement(int i) => AccRstrLists[i];
    }
}
