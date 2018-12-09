using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel.Dtos
{
    public class WarningsDto
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string RecordID { get; set; }
        public string Message { get; set; }
    }
}