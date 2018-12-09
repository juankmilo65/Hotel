using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Hotel.Utilities
{
    public class XmlToDynamic
    {
        public static void Parse(dynamic parent, XElement node)
        {
            if (node.HasElements)
            {
                if (node.Elements().Count() > 1)
                {
                    bool isForList = false;
                    string name = string.Empty;

                    foreach (var item in node.Elements())
                    {
                        if (string.IsNullOrEmpty(name))
                        {
                            name = item.Name.LocalName;
                        }
                        else
                        {
                            if (name == item.Name.LocalName)
                            {
                                isForList = true;
                            }
                        }
                    }


                    if (isForList)
                    {
                        //list
                        var item = new ExpandoObject();
                        var list = new List<dynamic>();
                        foreach (var element in node.Elements())
                        {
                            Parse(list, element);
                        }

                        AddProperty(item, node.Elements().First().Name.LocalName, list);
                        AddProperty(parent, node.Name.LocalName.ToString(), item);
                    }
                    else
                    {
                        CreateChild(parent, node);
                    }
                }
                else
                {
                    CreateChild(parent, node);
                }
            }
            else
            {

                AddProperty(parent, node.Name.LocalName.ToString(), node.Value.Trim());

                foreach (var attribute in node.Attributes())
                {
                    AddProperty(parent, attribute.Name.ToString(), attribute.Value.Trim());
                }
            }
        }

        private static void CreateChild(dynamic parent, XElement node)
        {
            var item = new ExpandoObject();

            foreach (var attribute in node.Attributes())
            {
                AddProperty(item, attribute.Name.ToString(), attribute.Value.Trim());
            }

            //element
            foreach (var element in node.Elements())
            {
                Parse(item, element);
            }


            AddProperty(parent, node.Name.LocalName.ToString(), item);
        }

        private static void AddProperty(dynamic parent, string name, object value)
        {
            if (parent is List<dynamic>)
            {
                (parent as List<dynamic>).Add(value);
            }
            else
            {
                (parent as IDictionary<String, object>)[name] = value;
            }
        }
    }
}