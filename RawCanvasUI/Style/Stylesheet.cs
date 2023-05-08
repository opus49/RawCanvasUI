using System.Collections.Generic;
using System.Xml;

namespace RawCanvasUI.Style
{
    public class Stylesheet
    {
        private readonly Dictionary<string, Dictionary<string, string>> styles = new Dictionary<string, Dictionary<string, string>>();

        public Stylesheet(string path)
        {
            Logging.Debug($"loading stylesheet from {path}");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNodeList styleNodes = doc.SelectNodes("/Stylesheet/Style");
            Logging.Debug($"loaded {styleNodes.Count} styles");

            foreach (XmlNode styleNode in styleNodes)
            {
                string styleName = styleNode.Attributes["name"].Value;
                Logging.Debug($"style name: {styleName}");

                XmlNodeList propertyNodes = styleNode.ChildNodes;
                Logging.Debug($"contains {propertyNodes.Count} property nodes");

                Dictionary<string, string> styleProperties = new Dictionary<string, string>();
                foreach (XmlNode propertyNode in propertyNodes)
                {
                    string propertyName = propertyNode.Attributes["name"].Value;
                    string propertyValue = propertyNode.Attributes["value"].Value;
                    Logging.Debug($"name: {propertyName} value: {propertyValue}");
                    styleProperties[propertyName] = propertyValue;
                }

                styles[styleName] = styleProperties;
            }
        }

        public Dictionary<string, string> GetStyle(string styleName)
        {
            if (styles.ContainsKey(styleName))
            {
                return styles[styleName];
            }
            else
            {
                return null;
            }
        }
    }
}

