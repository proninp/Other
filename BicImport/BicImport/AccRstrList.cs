using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace BicImport
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Pronin P.S.")]
    [Serializable()]
    [XmlRoot]
    public class AccRstrList
    {
        [XmlAttribute]
        public string AccRstr { get; set; }
        [XmlAttribute]
        public string AccRstrDate { get; set; }
    }
}
