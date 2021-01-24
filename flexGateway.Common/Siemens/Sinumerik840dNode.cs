using flexGateway.Common.MachineNode;
using System;

namespace flexGateway.Common.Siemens
{
    public class Sinumerik840dNode : INode
    {
        public Sinumerik840dNode(Guid guid, string name) { }

        #region nc var definition
        public int Syntax { get; internal set; }
        public int BereichEinheit { get; internal set; }
        public int Spalte { get; internal set; }
        public int Zeile { get; internal set; }
        public int BausteinTyp { get; internal set; }
        public int ZeilenAnzahl { get; internal set; }
        public int Typ { get; internal set; }
        public int Laenge { get; internal set; }
        #endregion

        #region interface properties
        public Guid Guid => throw new NotImplementedException();
        public string NodeName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public INode ParentNode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public NodeDataType NodeType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion
    }

}
