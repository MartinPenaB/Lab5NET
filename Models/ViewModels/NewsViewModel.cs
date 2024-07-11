namespace Lab5NET.Models.ViewModels
{
    public class NewsViewModel
    {
        public IEnumerable<Fan> Fans { get; set; }
        public IEnumerable<SportClub> SportClubs { get; set; }
        public IEnumerable<Subscription> Subscriptions { get; set; }

    }
}
