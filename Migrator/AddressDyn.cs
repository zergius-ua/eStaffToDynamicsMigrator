using Microsoft.Xrm.Sdk;

namespace Migrator
{
    public class AddressDyn
    {
        public string AddressId { get; set; }
        public OptionSetValue AddressTypeCode { get; set; }
        public string City { get; set; }
        public string Composite { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string Fax { get; set; }
        public OptionSetValue FreightTermsCode { get; set; }
        public decimal Latitude { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public decimal Longitude { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public string PostOfficeBox { get; set; }
        public string PrimaryContactName { get; set; }
        public OptionSetValue ShippingMethodCode { get; set; }
        public string StateOrProvince { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Telephone3 { get; set; }
        public string UPSZone { get; set; }
        public short UTCOffset { get; set; }
    }
}