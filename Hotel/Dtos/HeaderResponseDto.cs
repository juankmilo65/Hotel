using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel.Dtos
{
    public class HeaderResponseDto
    {
        public string EchoToken { get; set; }
        public string TimeStamp { get; set; }
        public string Version { get; set; }
        public string ResResponseType { get; set; }
    }
}