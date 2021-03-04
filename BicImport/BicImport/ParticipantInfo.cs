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
    public class ParticipantInfo
    {
        [XmlAttribute]
        public string NameP { get; set; }
        [XmlAttribute]
        public string Rgn { get; set; }
        [XmlAttribute]
        public string Ind { get; set; }
        [XmlAttribute]
        public string Tnp { get; set; }
        [XmlAttribute]
        public string Nnp { get; set; }
        [XmlAttribute]
        public string Adr { get; set; }
        [XmlAttribute]
        public string PrntBIC { get; set; }
        [XmlAttribute]
        public string DateIn { get; set; }
        [XmlAttribute]
        public string PtType { get; set; }
        [XmlAttribute]
        public string XchType { get; set; }
        [XmlAttribute]
        public string UID { get; set; }
        [XmlAttribute]
        public string NPSParticipant { get; set; }
        [XmlAttribute]
        public string ParticipantStatus { get; set; }
        [XmlElement(ElementName = Params.RSTRLIST, IsNullable = true)]
        public List<RstrList> RstrList { get; set; }
        public int GetRstrListCount() => RstrList.Count;
        public RstrList GetRstrListElement(int i) => RstrList[i];
    }
}
