using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace BicImport
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Pronin P.S.")]
    [Serializable()]
    [XmlRoot]
    public class RstrList
    {
        [XmlAttribute]
        public string Rstr { get; set; }
        [XmlAttribute]
        public string RstrDate { get; set; }
    }
}
