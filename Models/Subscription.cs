using System.ComponentModel.DataAnnotations;

namespace Lab5NET.Models
{
    public class Subscription
    {

        // Composite key
        public int FanId { get; set; }
        public string SportClubId { get; set; }

        // Navigation properties
        public Fan Fan { get; set; }
        public SportClub SportClub { get; set; }

    }
}
