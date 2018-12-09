using Hotel.DataContracts;
using System.Net.Security;
using System.ServiceModel;

namespace Hotel
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(ProtectionLevel = ProtectionLevel.None)]
    public interface IHotelService
    {
        [OperationContract(ProtectionLevel = ProtectionLevel.None)]
        Response ValidationXML(string value);
    }

}
