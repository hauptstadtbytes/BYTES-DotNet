//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

//import namespace(s) requried from 'BYTES.NET' framework
using BYTES.NET.Persistance.API;

namespace BYTES.NET.Automation.Scripting
{
    /// <summary>
    /// the (automation) script (base) class
    /// </summary>
    [Serializable]
    public class Script : IXmlPersistable
    {

        #region private variable(s)

        private Guid _id = Guid.NewGuid();
        private ScriptingEntityMetadata _metadata = new ScriptingEntityMetadata();
        private ScriptingEntityArguments _arguments = new ScriptingEntityArguments() { { "RootSequence",Guid.Empty.ToString()} };

        private List<Sequence> _sequences = new List<Sequence>();

        #endregion

        #region public properties

        public Guid ID { get => _id; set => _id = value; }

        public ScriptingEntityMetadata Metadata { get => _metadata; set => _metadata = value; }
        public ScriptingEntityArguments Arguments { get => _arguments; set => _arguments = value; }

        public List<Sequence> Sequences { get => _sequences; set => _sequences = value; }

        #endregion

        #region public new instance method

        /// <summary>
        /// default new instance method
        /// </summary>
        public Script()
        {
            //create a new 'root' sequence
            //this.Sequences.Add(new Sequence());
            //this.UpdateRootSequence(this.Sequences.First().ID);
        }

        #endregion

        #region public method(s) implementing 'IXmlPersistable'

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void UpdateFromIPersistable(IPersistable data)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            //create an empty namespace for serialization
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            //write the script ID
            writer.WriteAttributeString("ID", this.ID.ToString());

            //serialize the metadata
            XmlSerializer metadataSerializer = new XmlSerializer(typeof(ScriptingEntityMetadata));
            metadataSerializer.Serialize(writer, this.Metadata,ns);

            //serialize the arguments -> To do
            XmlSerializer argumentsSerializer = new XmlSerializer(typeof(ScriptingEntityArguments), new XmlRootAttribute("Arguments")); //rename the root node
            argumentsSerializer.Serialize(writer, this.Arguments, ns);

            //serialize the sequences
            XmlSerializer sequencesSerializer = new XmlSerializer(typeof(List<Sequence>), new XmlRootAttribute("Sequences")); //rename the root node
            sequencesSerializer.Serialize(writer, this.Sequences,ns);

        }

        #endregion

        #region public method(s)

        /// <summary>
        /// sets the root sequence by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SetRootSequence(Guid id)
        {
            foreach (Sequence sequence in this.Sequences)
            {
                if (sequence.ID == id)
                {
                    _arguments["RootSequence"] = sequence.ID.ToString();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// executes the script
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ScriptExecutionResult Execute(ScriptExecutionContext context)
        {
            //execute the root sequence (prevalidation is done automatically)
            try
            {
                return context.Execute(this);
            }
            catch (Exception ex)
            {
                return new ScriptExecutionResult() { Successful = false , Message = "Script execuion failed: " + ex.Message };
            }

        }

        #endregion
    }
}
