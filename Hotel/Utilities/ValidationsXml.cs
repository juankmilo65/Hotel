using Hotel.DataContracts;
using Hotel.Dtos;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Hotel.Utilities
{
    public class ValidationsXml
    {
        List<ErrorDto> errorList = new List<ErrorDto>();
        private string TimeStamp
        {
            get { return DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"); }
        }

        /// <summary>
        /// Methos to validate if xml is valid.
        /// </summary>
        /// <param name="value">String value base 64.</param>
        /// <param name="response">Response Object.</param>
        /// <returns>Boolean validator.</returns>
        public Response XmlProcessor(string value)
        {
            Response response = new Response();

            try
            {
                if (XmlIsNull(value, response))
                    return response;

                response = XmlHaveCorruptContent(value, response);
                return response;
            }
            catch (Exception ex)
            {
                HeaderResponseDto headerResponse = new HeaderResponseDto
                {
                    EchoToken = "abc-123",
                    TimeStamp = TimeStamp,
                    Version = "1.0"
                };

                ErrorDto error = new ErrorDto
                {
                    Code = "392",
                    Message = ex.Message,
                    Type = "6"

                };

                errorList.Add(error);

                return CreateErrorMessages(headerResponse, errorList, (int)HttpStatusCode.BadRequest);
            }
        }

        private bool XmlIsNull(string value, Response response)
        {
            if (string.IsNullOrEmpty(value))
            {
                response.Xml = Convert.ToBase64String(Encoding.UTF8.GetBytes(Resources.XmlError));
                response.Code = (int)HttpStatusCode.BadRequest;
                return true;
            }

            response.Code = (int)HttpStatusCode.OK;
            return false;
        }

        private Response XmlHaveCorruptContent(string value, Response response)
        {
            string xmlString = Encoding.UTF8.GetString(Convert.FromBase64String(value));
            dynamic OTA_HotelResNotifRQ = DynamicXml.Parse(xmlString);
            HeaderResponseDto headerResponse = new HeaderResponseDto();
            List<WarningsDto> warningsList = new List<WarningsDto>();
            List<HotelReservationDto> reservationsList = new List<HotelReservationDto>();
            List<HotelReservationIDDto> HotelReservationIDs = new List<HotelReservationIDDto>();
            bool haveErrorsOrWarnings = false;

            //Ejemplo llamado warning y success
            if (OTA_HotelResNotifRQ.HotelReservations != null)
            {
                headerResponse = new HeaderResponseDto
                {
                    EchoToken = "abc-123",
                    TimeStamp = TimeStamp,
                    Version = "1.0"
                };

                HotelReservationIDs.Add(new HotelReservationIDDto
                {
                    ResID_Type = "18",
                    ResID_Value = "82329987"
                });

                reservationsList.Add(new HotelReservationDto
                {
                    UniqueID_Id = "3741",
                    UniqueID_Type = "14",
                    HotelReservationIDs = HotelReservationIDs
                });


                if (OTA_HotelResNotifRQ.HotelReservations.HotelReservation.Count == null)
                {
                    haveErrorsOrWarnings = ReservationValidation(warningsList, haveErrorsOrWarnings, OTA_HotelResNotifRQ.HotelReservations.HotelReservation);
                }
                else
                {
                    foreach (var hotelReservation in OTA_HotelResNotifRQ.HotelReservations.HotelReservation)
                    {
                        haveErrorsOrWarnings = ReservationValidation(warningsList, haveErrorsOrWarnings, hotelReservation);
                    }
                }

                if (haveErrorsOrWarnings)
                {
                    response = CreateWarningMessages(headerResponse, warningsList, reservationsList, (int)HttpStatusCode.PartialContent);
                }
                else
                {
                    //ejemplo success
                    response = CreateSuccessMessages(headerResponse, reservationsList, (int)HttpStatusCode.OK);
                }

                return response;
            }
            else
            {
                //ejemplo llamar a error
            }


            return new Response();
        }

        private Response CreateSuccessMessages(HeaderResponseDto headerResponse, List<HotelReservationDto> reservationsList, int statusCode)
        {
            Response response = new Response();
            string reservations = string.Empty;
            string hotelReservationIds = string.Empty;
            string hotelReservations = string.Empty;


            foreach (var reservation in reservationsList)
            {
                foreach (var hotelReservationId in reservation.HotelReservationIDs)
                {
                    hotelReservationIds = hotelReservationIds + string.Format(Resources.XmlSuccessHotelReservation, hotelReservationId.ResID_Type, hotelReservationId.ResID_Value);
                }

                hotelReservations = hotelReservations + string.Format(Resources.XmlSuccessBody, reservation.UniqueID_Type, reservation.UniqueID_Id, hotelReservationIds);
                hotelReservationIds = string.Empty;
            }

            response.Xml = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format(Resources.XmlSuccess, headerResponse.EchoToken, headerResponse.TimeStamp, headerResponse.Version, headerResponse.ResResponseType, hotelReservations)));
            response.Code = statusCode;

            return response;
        }

        private static bool ReservationValidation(List<WarningsDto> warningsList, bool haveErrorsOrWarnings, dynamic hotelReservation)
        {
            int count = 0;
            if (string.IsNullOrEmpty(hotelReservation.ResGlobalInfo.Total.AmountAfterTax))
            {
                haveErrorsOrWarnings = true;


                warningsList.Add(new WarningsDto
                {
                    Code = "666",
                    Message = "AmountAfterTax node " + count + " is mandatory.",
                    RecordID = "BDC-1234567890",
                    Type = "3"
                });

                count++;
            }

            return haveErrorsOrWarnings;
        }

        private Response CreateErrorMessages(HeaderResponseDto headerResponse, List<ErrorDto> errorsList, int statusCode)
        {
            Response response = new Response();
            string errors = string.Empty;

            foreach (var error in errorsList)
            {
                errors = errors + string.Format(Resources.XmlErrorContent, error.Type, error.Code, error.Message);
            }

            response.Xml = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format(Resources.XmlError, headerResponse.Version, headerResponse.TimeStamp, headerResponse.EchoToken, errors)));
            response.Code = statusCode;

            return response;
        }

        private Response CreateWarningMessages(HeaderResponseDto headerResponse, List<WarningsDto> warningsList, List<HotelReservationDto> reservationsList, int statusCode)
        {
            Response response = new Response();
            string warnings = string.Empty;
            string reservations = string.Empty;
            string hotelReservationIds = string.Empty;
            string hotelReservations = string.Empty;

            foreach (var warning in warningsList)
            {
                warnings = warnings + string.Format(Resources.XmlWarningContenWarning, warning.Type, warning.Code, warning.RecordID, warning.Message);
            }

            foreach (var reservation in reservationsList)
            {
                foreach (var hotelReservationId in reservation.HotelReservationIDs)
                {
                    hotelReservationIds = hotelReservationIds + string.Format(Resources.XmlWarningHotelReservationId, hotelReservationId.ResID_Type, hotelReservationId.ResID_Value);
                }

                hotelReservations = hotelReservations + string.Format(Resources.XmlWarningHotelReservation, reservation.UniqueID_Type, reservation.UniqueID_Id, hotelReservationIds);
                hotelReservationIds = string.Empty;
            }

            response.Xml = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format(Resources.XmlWarning, headerResponse.EchoToken, headerResponse.TimeStamp, headerResponse.Version, headerResponse.ResResponseType, warnings, hotelReservations)));
            response.Code = statusCode;

            return response;
        }

        //private Response CreateSuccessMessage(HeaderResponseDto headerResponse)
        //{ }
    }
}