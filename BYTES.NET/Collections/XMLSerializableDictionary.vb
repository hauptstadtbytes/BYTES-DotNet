'import .net namespace(s) required
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization

Imports System.IO

Namespace Collections

    ''' <summary>
    ''' a XML serializable dictionary
    ''' </summary>
    ''' <typeparam name="TKey"></typeparam>
    ''' <typeparam name="TValue"></typeparam>
    ''' <remarks>based on the article found at 'http://www.playswithcomputers.com/SGDCollection.aspx'</remarks>
    <Serializable>
    <XmlRoot("Dictionary")>
    Public Class XMLSerializableDictionary(Of TKey, TValue)

        Inherits Dictionary(Of TKey, TValue)

        Implements System.Xml.Serialization.IXmlSerializable

#Region "private variable(s)"

        Private _typesNotToBeSerialized As List(Of Type) = New List(Of Type) From {GetType(String)}

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()

            MyBase.New

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="comparer"></param>
        Public Sub New(Optional ByVal comparer As IEqualityComparer(Of TKey) = Nothing)

            'create a new base-class instance
            MyBase.New(comparer)

        End Sub

#End Region

#Region "public method(s), implementing the 'IXMLSerializable' interface"

        ''' <summary>
        ''' method for reading the data from XML
        ''' </summary>
        ''' <param name="reader"></param>
        Public Sub ReadXml(reader As XmlReader) Implements IXmlSerializable.ReadXml

            If (IsTypeNotToBeSerialized(GetType(TKey)) And IsTypeNotToBeSerialized(GetType(TValue))) Then 'there is not need do serialize the data types

                While reader.ReadToFollowing("Item")

                    Dim key As TKey = DirectCast(Convert.ChangeType(reader.GetAttribute("Key"), GetType(TKey)), TKey)
                    Dim value As TValue = DirectCast(Convert.ChangeType(reader.ReadString, GetType(TValue)), TValue)

                    Me.Add(key, value)

                End While

            Else 'the data has to be serialized

                'create new serializers
                Dim keySerializer As New XmlSerializer(GetType(TKey))
                Dim valueSerializer As New XmlSerializer(GetType(TValue))

                'read the data
                Dim wasEmpty As Boolean = reader.IsEmptyElement
                reader.Read()

                If (wasEmpty) Then Return

                While (reader.NodeType <> System.Xml.XmlNodeType.EndElement)

                    reader.ReadStartElement("Item")

                    reader.ReadStartElement("Key")

                    Dim key As TKey = CType(keySerializer.Deserialize(reader), TKey)

                    reader.ReadEndElement()

                    reader.ReadStartElement("Value")

                    Dim value As TValue = CType(valueSerializer.Deserialize(reader), TValue)

                    reader.ReadEndElement()

                    Me.Add(key, value)

                    reader.ReadEndElement()

                    reader.MoveToContent()

                End While

                reader.ReadEndElement()

            End If

        End Sub

        ''' <summary>
        ''' method writing the data to XML
        ''' </summary>
        ''' <param name="writer"></param>
        Public Sub WriteXml(writer As XmlWriter) Implements IXmlSerializable.WriteXml

            'validate key and value for being serializable (if required)

            If Not IsTypeNotToBeSerialized(GetType(TKey)) OrElse Not IsTypeNotToBeSerialized(GetType(TValue)) Then

                If Not GetType(TKey).IsSerializable Then

                    Throw New Exception("Unable to serialize object of type '" & GetType(TKey).ToString & "' (Key): Object is not marked as serializable")

                End If

                If Not GetType(TValue).IsSerializable Then

                    Throw New Exception("Unable to serialize object of type '" & GetType(TValue).ToString & "' (Value): Object is not marked as serializable")

                End If

            End If

            'create new serializers
            Dim keySerializer As New XmlSerializer(GetType(TKey))
            Dim valueSerializer As New XmlSerializer(GetType(TValue))

            'write the output
            For Each key As TKey In Me.Keys

                writer.WriteStartElement("Item") 'start writing the 'item'

                If (IsTypeNotToBeSerialized(GetType(TKey)) And IsTypeNotToBeSerialized(GetType(TValue))) Then 'there is not need do serialize the data types

                    'write the key
                    writer.WriteAttributeString("Key", key.ToString())

                    'write the value
                    writer.WriteString(Me(key).ToString())

                Else 'the data has to be serialized

                    'write the key
                    writer.WriteStartElement("Key")

                    keySerializer.Serialize(writer, key)

                    writer.WriteEndElement()

                    'write the value
                    writer.WriteStartElement("Value")

                    Dim value As TValue = Me(key)
                    valueSerializer.Serialize(writer, value)

                    writer.WriteEndElement()

                End If

                writer.WriteEndElement() 'finish writing the 'item'

            Next

        End Sub

        ''' <summary>
        ''' method returning the 
        ''' </summary>
        ''' <returns></returns>
        Public Function GetSchema() As XmlSchema Implements IXmlSerializable.GetSchema

            Return Nothing

        End Function

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method writing the data to memory stream
        ''' </summary>
        ''' <param name="stream"></param>
        Public Sub Write(ByRef stream As MemoryStream)

            Dim serializer As XmlSerializer = New XmlSerializer(GetType(XMLSerializableDictionary(Of TKey, TValue)))
            Dim writer As StreamWriter = New StreamWriter(stream)

            serializer.Serialize(writer, Me)

            writer.Close()

        End Sub

        ''' <summary>
        ''' method writing the data to file stream
        ''' </summary>
        ''' <param name="stream"></param>
        Public Sub Write(ByRef stream As FileStream)

            Dim serializer As XmlSerializer = New XmlSerializer(GetType(XMLSerializableDictionary(Of TKey, TValue)))
            Dim writer As StreamWriter = New StreamWriter(stream)

            serializer.Serialize(writer, Me)

            writer.Close()

        End Sub

        ''' <summary>
        ''' method reading the data from memory stream
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <returns></returns>
        Public Function Read(ByRef stream As MemoryStream) As XMLSerializableDictionary(Of TKey, TValue)

            Dim serializer As XmlSerializer = New XmlSerializer(GetType(XMLSerializableDictionary(Of TKey, TValue)))
            Dim reader As StreamReader = New StreamReader(stream)

            Dim tmp As XMLSerializableDictionary(Of TKey, TValue) = serializer.Deserialize(reader)

            reader.Close()

            Return tmp

        End Function

        ''' <summary>
        ''' method reading the data from file stream
        ''' </summary>
        ''' <param name="stream"></param>
        ''' <returns></returns>
        Public Function Read(ByRef stream As FileStream) As XMLSerializableDictionary(Of TKey, TValue)

            Dim serializer As XmlSerializer = New XmlSerializer(GetType(XMLSerializableDictionary(Of TKey, TValue)))
            Dim reader As StreamReader = New StreamReader(stream)

            Dim tmp As XMLSerializableDictionary(Of TKey, TValue) = serializer.Deserialize(reader)

            reader.Close()

            Return tmp

        End Function

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method validating a type for not to be serialized
        ''' </summary>
        ''' <param name="type"></param>
        ''' <returns></returns>
        Private Function IsTypeNotToBeSerialized(type As Type) As Boolean


            'check for the type(s) listed
            For Each listedType As Type In _typesNotToBeSerialized

                If type.Equals(listedType) Then

                    Return True
                End If

            Next

            'return the default output value
            Return False

        End Function

#End Region

    End Class

End Namespace