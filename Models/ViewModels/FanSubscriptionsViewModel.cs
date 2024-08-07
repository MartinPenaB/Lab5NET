namespace Lab5NET.Models.ViewModels
{
    public class FanSubscriptionsViewModel
    {
        public Fan Fan { get; set; }
        public List<SportClub> SportClubs { get; set; }
        public List<string> SelectedSportClubIds { get; set; } = new List<string>();
    }


}
