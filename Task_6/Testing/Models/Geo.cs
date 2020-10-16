using System.Text;
using System.Text.Json.Serialization;
namespace Task_6.Testing.Models
{
    public class Geo
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; }
        [JsonPropertyName("lng")]
        public string Lng { get; set; }

        public override bool Equals(object obj)
        {
            Geo geo = obj as Geo;
            if (geo.Lat.Equals(this.Lat) && geo.Lng.Equals(this.Lng))
                return true;
            else
                return false;            
        }

        public override string ToString()
        {
            StringBuilder geoStringBuilder = new StringBuilder();
            geoStringBuilder.Append("Geo{");
            geoStringBuilder.Append($"Lat=\"{this.Lat}\", ");
            geoStringBuilder.Append($"Lng=\"{this.Lng}\"");
            geoStringBuilder.Append("}");
            return geoStringBuilder.ToString();
        }
    }     
}
