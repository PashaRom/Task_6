using System.Text;
using System.Text.Json.Serialization;
namespace Task_6.Testing.Models
{
    public class Address
    {       
        [JsonPropertyName("street")]
        public string Street{ get; set; }
        [JsonPropertyName("suite")]
        public string Suite { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("zipcode")]
        public string Zipcode { get; set; }
        [JsonPropertyName("geo")]
        public Geo Geo { get; set; } = new Geo();

        public override bool Equals(object obj)
        {
            Address address = obj as Address;
            if (address.Street.Equals(this.Street)
                && address.Suite.Equals(this.Suite)
                && address.City.Equals(this.City)
                && address.Zipcode.Equals(this.Zipcode)
                && address.Geo.Equals(this.Geo))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            StringBuilder addressStringBuilder = new StringBuilder();
            addressStringBuilder.Append("Address{");
            addressStringBuilder.Append($"Street=\"{this.Street}\", ");
            addressStringBuilder.Append($"Suite=\"{this.Suite}\", ");
            addressStringBuilder.Append($"City=\"{this.City}\", ");
            addressStringBuilder.Append($"Zipcode=\"{this.Zipcode}\", ");
            addressStringBuilder.Append(this.Geo.ToString());
            addressStringBuilder.Append("}");
            return addressStringBuilder.ToString();
        }
    }
}
