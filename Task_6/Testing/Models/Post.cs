using System;
using System.Text;
using System.Text.Json.Serialization;
namespace Task_6.Testing.Models
{
    public class Post : IComparable
    {
        [JsonPropertyName("userId")]
        public int? UserId { get; set; }
        [JsonPropertyName("id")]
        public int? PostId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }

        public int CompareTo(object obj)
        {
            Post post = obj as Post;
            if (this.PostId < post.PostId)
                return -1;
            else if (this.PostId == post.PostId)
                return 0;
            else
                return 1;            
        }

        public override string ToString()
        {
            StringBuilder postStringBuilder = new StringBuilder();
            postStringBuilder.Append("Post{");
            postStringBuilder.Append($"UserId=\"{this.UserId}\", ");
            postStringBuilder.Append($"PostId=\"{this.PostId}\", ");
            postStringBuilder.Append($"Title=\"{this.Title}\", ");
            postStringBuilder.Append($"Body=\"{this.Body}\"");
            postStringBuilder.Append("}");
            return postStringBuilder.ToString();
        }
    }
}
