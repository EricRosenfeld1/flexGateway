using DotNetSiemensPLCToolBoxLibrary.Communication;
using flexGateway.Interface;
using Newtonsoft.Json;
using System;

namespace Sinumerik840d
{
    public class Sinumerik840dNodeConfiguration : INodeConfiguration
    {
        public int SyntaxId { get; set; }
        public int BereichEinheit { get; set; }
        public int Spalte { get; set; }
        public int Zeile { get; set; }
        public int Bausteintyp { get; set; }
        public int ZeilenZahl { get; set; }
        public int Typ { get; set; }
        public int Länge { get; set; }

        public Sinumerik840dNodeConfiguration() {}
    }
}
