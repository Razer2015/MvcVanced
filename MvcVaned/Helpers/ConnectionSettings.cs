using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Xml;

namespace MvcVanced.Helpers
{
    public class ConnectionSettings
    {
        private static NameValueCollection _appSettings;

        public static NameValueCollection AppSettings {
            get {
                if (_appSettings == null) {
                    var collection = new NameValueCollection();

                    try {
                        XmlTextReader reader = new XmlTextReader(HttpContext.Current.Server.MapPath("~/RDSCon.config"));
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(reader);

                        XmlNodeList nodeList = xmlDoc.SelectNodes("configuration");

                        foreach (XmlNode node in nodeList) {
                            foreach (XmlNode item in node.ChildNodes) {
                                collection.Add(item.Name, item.InnerText);
                            }
                        }
                    }
                    catch { }

                    _appSettings = collection;
                }

                return _appSettings;
            }
        }
    }
}