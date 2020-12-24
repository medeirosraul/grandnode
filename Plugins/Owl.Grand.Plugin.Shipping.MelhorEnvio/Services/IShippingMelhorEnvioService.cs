using Grand.Domain;
using Owl.Grand.Plugin.Shipping.MelhorEnvio.Domain;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio.Services
{
    public partial interface IShippingMelhorEnvioService
    {
        Task DeleteShippingMelhorEnvioRecord(ShippingMelhorEnvioRecord shippingMelhorEnvioRecord);

        Task<IPagedList<ShippingMelhorEnvioRecord>> GetAll(int pageIndex = 0, int pageSize = int.MaxValue);

        Task<ShippingMelhorEnvioRecord> FindRecord(string shippingMethodId,
            string storeId, string warehouseId,
            string countryId, string stateProvinceId, string zip, decimal weight);

        Task<ShippingMelhorEnvioRecord> GetById(string shippingMelhorEnvioRecordId);

        Task InsertShippingMelhorEnvioRecord(ShippingMelhorEnvioRecord shippingMelhorEnvioRecord);

        Task UpdateShippingMelhorEnvioRecord(ShippingMelhorEnvioRecord shippingMelhorEnvioRecord);
    }

}
