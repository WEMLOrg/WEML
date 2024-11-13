using System.ComponentModel.DataAnnotations;

namespace WEML.Models
{
    public class Challange
    {
        [Key]public int cId {  get; set; }
        public int maxScore { get; set; }
        public int minScoreToPass { get; set; }
        public string objective { get; set; }
        public string name { get; set; }

        public ICollection<ChallangeQuestions> ChallangeQuestions { get; set; }

        public Challange() { }

        public Challange(int cId, int maxScore, int minScoreToPass, string objective, string name)
        {
            this.cId = cId;
            this.maxScore = maxScore;
            this.minScoreToPass = minScoreToPass;
            this.objective = objective;
            this.name = name;
        }
    }
}
