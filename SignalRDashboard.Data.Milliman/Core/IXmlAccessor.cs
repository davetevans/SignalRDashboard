using System.Xml.Linq;

namespace SignalRDashboard.Data.Milliman.Core
{
    public interface IXmlAccessor
    {
        string BaseUrl { get; }
        XElement GetXml(string path);
        XElement GetXml();
    }
}