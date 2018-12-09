using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel.Dtos
{
    public class HotelReservationDto
    {
        public string UniqueID_Type { get; set; }
        public string UniqueID_Id { get; set; }
        public List<HotelReservationIDDto> HotelReservationIDs { get; set; }
    }
}