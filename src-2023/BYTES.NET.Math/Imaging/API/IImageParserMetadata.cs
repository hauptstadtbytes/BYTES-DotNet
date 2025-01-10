using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math.Imaging.API
{
    public interface IImageParserMetadata
    {
        string Name { get; }

        string FileExtension { get; }
    }
}
