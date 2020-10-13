using System.Text;
using System.Text.Json.Serialization;
namespace Task_6.Testing.Models
{
    public class Company
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("catchPhrase")]
        public string CatchPhrase { get; set; }
        [JsonPropertyName("bs")]
        public string Bs { get; set; }
        public override bool Equals(object obj)
        {
            Company company = obj as Company;
            if (company.Name.Equals(this.Name)
                && company.CatchPhrase.Equals(this.CatchPhrase)
                && company.Bs.Equals(this.Bs))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            StringBuilder companyStringBuilder = new StringBuilder();
            companyStringBuilder.Append("Company{");
            companyStringBuilder.Append($"Name=\"{this.Name}\", ");
            companyStringBuilder.Append($"CatchPhrase=\"{this.CatchPhrase}\", ");
            companyStringBuilder.Append($"Bs=\"{this.Bs}\"");
            companyStringBuilder.Append("}");
            return companyStringBuilder.ToString();
        }
    }
}
