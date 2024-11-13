using System.ComponentModel.DataAnnotations;

namespace WEML.Models
{
    public class Question
    {
        [Key]public int qId {  get; set; }
        public string question { get; set; }
        public string correctAnswer {  get; set; }
        public string incorrectAnswer1 {  get; set; }
        public string? incorrectAnswer2 {  get; set; }
        public string? incorrectAnswer3 {  get; set; }
        public int points { get; set; }

        public ICollection<ChallangeQuestions> ChallangeQuestions { get; set; }

        public Question() { }

        public Question(int qId, string question, string correctAnswer, string incorrectAnswer1, string? incorrectAnswer2, string? incorrectAnswer3, int points)
        {
            this.qId = qId;
            this.question = question;
            this.correctAnswer = correctAnswer;
            this.incorrectAnswer1 = incorrectAnswer1;
            this.incorrectAnswer2 = incorrectAnswer2;
            this.incorrectAnswer3 = incorrectAnswer3;
            this.points = points;
        }


    }
}
