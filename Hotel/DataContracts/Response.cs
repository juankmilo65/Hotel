using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace Hotel.DataContracts
{
    [DataContract]
    public class Response
    {
        private int code;
        private string xmlResponse = string.Empty;


        [DataMember]
        public int Code
        {
            get { return code; }
            set { code = value; }
        }


        [DataMember]
        public string Xml
        {
            get { return xmlResponse; }
            set { xmlResponse = value; }
        }
    }
}