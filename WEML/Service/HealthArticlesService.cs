using WEML.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WEML.Service
{
    public class HealthArticlesService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly string apiKey = "e30a621d-1839-4d61-8a83-903ee5739788";

        //public async Task<List<Article>> GetArticlesAsync(string symptom)
        //{
        //    string apiUrl = GetArticlesUrl(symptom);
        //    Console.WriteLine("Generated API URL: " + apiUrl);

        //    try
        //    {
        //        var response = await httpClient.GetAsync(apiUrl);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            throw new Exception($"API request failed with status code: {response.StatusCode}");
        //        }
        //        string responseContent = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine("API Response: " + responseContent);


        //        string jsonContent = responseContent.Replace("JSON_CALLBACK(", "").TrimEnd(')');
        //        var articleResponse = JsonConvert.DeserializeObject<ArticleResponse>(jsonContent);

        //        if (articleResponse?.Results == null || !articleResponse.Results.Any())
        //        {
        //            throw new Exception("No articles found in the response.");
        //        }

        //        List<Article> articles = articleResponse?.Results ?? new List<Article>();
        //        foreach (var article in articles)
        //        {
        //            article.Description = GetShortDescription(article.Body);  
        //            article.Symptom = symptom;
        //        }

        //        return articles.Take(3).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return new List<Article>();
        //    }
        //}

        public async Task<List<Article>> GetArticlesAsync(string symptom)
        {
            string apiUrl = GetArticlesUrl(symptom);
            //Console.WriteLine("Generated API URL: " + apiUrl);

            try
            {
                var response = await httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API request failed with status code: {response.StatusCode}");
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                //Console.WriteLine("API Response: " + responseContent);

                string jsonContent = responseContent.Replace("JSON_CALLBACK(", "").TrimEnd(')');

                var articleResponse = JsonConvert.DeserializeObject<ArticleResponse>(jsonContent);

                if (articleResponse?.ArticlesData?.Results == null || !articleResponse.ArticlesData.Results.Any())
                {
                    throw new Exception("No articles found in the response.");
                }

                List<Article> articles = articleResponse.ArticlesData.Results.Select(article =>
                {
                    article.Description = GetShortDescription(article.Body); 
                    article.Symptom = symptom; 
                    return article;
                }).Take(3).ToList();

                return articles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Article>();
            }
        }


        public string GetArticlesUrl(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException("Keyword (symptom) cannot be null or empty.", nameof(keyword));
            }
            string baseUrl = "https://newsapi.ai/api/v1/article/getArticles?query=%7B%22%24query%22%3A%7B%22%24and%22%3A%5B%7B%22%24or%22%3A%5B%7B%22sourceUri%22%3A%22webmd.com%22%7D%2C%7B%22sourceUri%22%3A%22health.harvard.edu%22%7D%2C%7B%22sourceUri%22%3A%22consumer.fda.gov.tw%22%7D%2C%7B%22sourceUri%22%3A%22medlineplus.gov%22%7D%2C%7B%22sourceUri%22%3A%22euro.who.int%22%7D%5D%7D%2C%7B%22lang%22%3A%22eng%22%7D%5D%7D%2C%22%24filter%22%3A%7B%22forceMaxDataTimeWindow%22%3A%2231%22%2C%22dataType%22%3A%5B%22news%22%2C%22blog%22%5D%2C%22startSourceRankPercentile%22%3A0%2C%22endSourceRankPercentile%22%3A50%2C%22isDuplicate%22%3A%22skipDuplicates%22%7D%7D&resultType=articles&articlesSortBy=rel&includeArticleEventUri=false&includeArticleOriginalArticle=true&apiKey=e30a621d-1839-4d61-8a83-903ee5739788&callback=JSON_CALLBACK";
            return baseUrl;
            //return string.Format(baseUrl, Uri.EscapeDataString(keyword));
        }


        private string GetShortDescription(string fullText)
        {
            if (string.IsNullOrEmpty(fullText))
            {
                return string.Empty; 
            }

            var sentences = fullText.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var shortDescription = string.Join(".", sentences.Take(2));

            return shortDescription.Trim() + (shortDescription.Length > 0 ? "." : "");
        }

    }
}
