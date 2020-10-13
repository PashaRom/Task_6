using System.Text;
using System.Text.Json.Serialization;
namespace Task_6.Testing.Models
{
    public class User
    {       
        [JsonPropertyName("id")]
        public int UserId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("website")]
        public string Website { get; set; }
        [JsonPropertyName("address")]
        public Address Address { get; set; } = new Address();
        [JsonPropertyName("company")]
        public Company Company { get; set; } = new Company();

        public override bool Equals(object obj)
        {
            User user = obj as User;
            if (user.UserId == this.UserId
                && user.Name.Equals(this.Name)
                && user.UserName.Equals(this.UserName)
                && user.Email.Equals(this.Email)
                && user.Phone.Equals(this.Phone)
                && user.Website.Equals(this.Website)
                && user.Address.Equals(this.Address)
                && user.Company.Equals(this.Company))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            StringBuilder userStringBuilder = new StringBuilder();
            userStringBuilder.Append("User{");
            userStringBuilder.Append($"UserId=\"{this.UserId}\", ");
            userStringBuilder.Append($"Name=\"{this.Name}\", ");
            userStringBuilder.Append($"UserName=\"{this.UserName}\", ");
            userStringBuilder.Append($"Email=\"{this.Email}\", ");
            userStringBuilder.Append($"Phone=\"{this.Phone}\", ");
            userStringBuilder.Append($"Website=\"{this.Website}\", ");
            userStringBuilder.Append($"{this.Address}, ");
            userStringBuilder.Append(this.Company.ToString());
            userStringBuilder.Append("}");
            return userStringBuilder.ToString();
        }
    }
}
