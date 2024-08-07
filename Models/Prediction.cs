using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5NET.Models
{
    //public enum Question
    //{
    //    Earth,
    //    Computer
    //}

    public class Prediction
    {
        [Key]
        public int PredictionId { get; set; }

        //[Required]
        [StringLength(255)]
        public string FileName { get; set; }

        //[Required]
        [Url]
        public string Url { get; set; }

        //[Required]
        //public Question Question { get; set; }


        // Foreign key for SportClub
        public string SportClubId { get; set; }

        // Navigation property for SportClub
        public SportClub SportClub { get; set; }
    }
}
