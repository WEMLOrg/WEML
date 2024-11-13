using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WEML.Models
{
    public class Article
    {
        public string Uri { get; set; }
        public string Lang { get; set; }
        public bool IsDuplicate { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public double Sentiment { get; set; }

        public string? Description { get; set; }
        public string? Symptom { get; set; }
    }

    public class ArticleResponse
    {
        [JsonProperty("articles")]
        public ArticlesData ArticlesData { get; set; }
    }

    public class ArticlesData
    {
        [JsonProperty("results")]
        public List<Article> Results { get; set; }
    }
}

