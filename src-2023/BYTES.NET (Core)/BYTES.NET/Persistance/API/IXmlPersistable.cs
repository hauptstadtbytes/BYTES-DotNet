//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BYTES.NET.Persistance.API
{
    public interface IXmlPersistable : IPersistable, IXmlSerializable
    {
    }
}
