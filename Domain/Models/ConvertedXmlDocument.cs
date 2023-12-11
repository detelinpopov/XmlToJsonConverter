using System.Xml;

namespace Domain.Models;

public class ConvertedXmlDocument : XmlDocument
{
    public bool TryParseXml(Stream fileStream)
    {
        try
        {
            Load(fileStream);
            return true;
        }
        catch (XmlException)
        {
            return false;
        }
    }
}