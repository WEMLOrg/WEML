using System.ComponentModel.DataAnnotations;

namespace WEML.Models
{
    public class ChallangeQuestions
    {
        [Key]public int cId { get; set; }
        [Key]public int questionId { get; set; }

        public Challange Challange { get; set; }
        public Question Question { get; set; }

        public ChallangeQuestions() { }

        public ChallangeQuestions(int cId, int questionId)
        {
            this.cId = cId;
            this.questionId = questionId;
        }
    }
}
