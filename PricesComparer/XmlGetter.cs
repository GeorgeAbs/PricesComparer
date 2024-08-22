using AngleSharp;
using AngleSharp.Xml.Parser;
using System.Xml;
using System.Xml.Linq;
namespace PricesComparer
{
    internal static class XmlGetter
    {
        public static Dictionary<string, ItemFromSite> GetInfo(string url)
        {
            List<ItemFromSite> infos = [];

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings()
            {
                DtdProcessing = DtdProcessing.Parse
            };

            XmlReader reader = XmlReader.Create(url, xmlReaderSettings);

            var xml = XElement.Load(reader);

            Console.WriteLine(xml.Element("shop").Elements("offers").Elements("offer").Count());
              
            var items = xml.Element("shop").Elements("offers").Elements("offer")
                .Select(item => new ItemFromSite { 
                    Url =  item.Element("url").Value,
                    Name = item.Element("name").Value ,
                    Id = item.Element("vendorCode").Value,
                    Price = item.Element("price").Value.Replace(",",".").Replace(" ", "").Trim()
                } );

            var res = new Dictionary<string, ItemFromSite>();

            foreach ( var item in items)
            {
                res.Add(item.Id, item);
            }
            return res;
        }
    }
}
