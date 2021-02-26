using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace BicImport
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("Pronin P.S.")]
    [Serializable()]
    [XmlRoot]
    public class SWBICS
    {
        [XmlAttribute]
        public string SWBIC { get; set; }
        [XmlAttribute]
        public string DefaultSWBIC { get; set; }
    }
}
