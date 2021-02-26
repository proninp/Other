using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace BicImport
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Pronin P.S.")]
    [Serializable()]
    [XmlRoot]
    public class BICDirectoryEntry
    {
        [XmlAttribute("BIC")]
        public string Bic { get; set; }
        [XmlElement]
        public ParticipantInfo ParticipantInfo { get; set; }
        [XmlElement(IsNullable = true)]
        public SWBICS SWBICS { get; set; }
        [XmlElement(ElementName = Params.ACCOUNTS, IsNullable = true)]
        public List<Accounts> AccountsList { get; set; }
        public int GetAccountsListCount() => AccountsList.Count;
        public Accounts GetAccountsListElement(int i) => AccountsList[i];
    }
}
