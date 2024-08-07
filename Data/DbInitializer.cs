using Lab5NET.Models;

namespace Lab5NET.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SportsDbContext context)
        {
            context.Database.EnsureCreated();

            // If data already exists, return
            if (context.Fans.Any() && context.SportClubs.Any() && context.Subscriptions.Any() && context.Predictions.Any())
            {
                return;   // DB has been seeded
            }

            // Seed Fans
            var fans = new Fan[]
            {
                new Fan { FirstName = "Carson", LastName = "Alexander", BirthDate = DateTime.Parse("1995-01-09") },
                new Fan { FirstName = "Meredith", LastName = "Alonso", BirthDate = DateTime.Parse("1992-09-05") },
                new Fan { FirstName = "Arturo", LastName = "Anand", BirthDate = DateTime.Parse("1993-10-09") }
            };

            foreach (var fan in fans)
            {
                context.Fans.Add(fan);
            }
            context.SaveChanges();

            // Seed SportClubs
            var sportClubs = new SportClub[]
            {
                new SportClub { Id = "A1", Title = "Alpha", Fee = 300 },
                new SportClub { Id = "B1", Title = "Beta", Fee = 130 },
                new SportClub { Id = "O1", Title = "Omega", Fee = 390 }
            };

            foreach (var sportClub in sportClubs)
            {
                context.SportClubs.Add(sportClub);
            }
            context.SaveChanges();

            // Seed Subscriptions
            var subscriptions = new Subscription[]
            {
                new Subscription { FanId = 1, SportClubId = "A1" },
                new Subscription { FanId = 1, SportClubId = "B1" },
                new Subscription { FanId = 1, SportClubId = "O1" },
                new Subscription { FanId = 2, SportClubId = "A1" },
                new Subscription { FanId = 2, SportClubId = "B1" },
                new Subscription { FanId = 3, SportClubId = "A1" }
            };

            foreach (var subscription in subscriptions)
            {
                context.Subscriptions.Add(subscription);
            }
            context.SaveChanges();



            var predictions = new Prediction[]
            {
                new Prediction { FileName = "Earth", Url = "https://pena0035lab6.blob.core.windows.net/earthimages/nasa-map.jpg", SportClubId = "A1" },
                new Prediction { FileName = "Computer", Url = "https://pena0035lab6.blob.core.windows.net/computerimages/960x0.jpg", SportClubId = "B1" }
            };

            foreach (var prediction in predictions)
            {
                context.Predictions.Add(prediction);
            }
            context.SaveChanges();
        }
    }

}
