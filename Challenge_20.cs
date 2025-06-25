using static System.Console;

namespace LINQ_Challeges
{
    public class Challenge_20
    {
        List<(int AgentId, string Name)> agents = new()
        {
            (1, "Alice"), (2, "Bob"), (3, "Charlie"), (4, "Diana"), (5, "Ethan"),
            (6, "Fiona"), (7, "George"), (8, "Holly"), (9, "Ian"), (10, "Julia")
        };

        List<(int CategoryId, string Title)> categories = new()
        {
            (1, "Networking"), (2, "Software"), (3, "Hardware"), (4, "Security"), (5, "Other")
        };

        List<(int TicketId, int AgentId, int CategoryId, DateTime OpenedOn)> tickets = new()
        {
            (1001, 1, 1, DateTime.Parse("2024-01-10")),
            (1002, 2, 2, DateTime.Parse("2024-01-11")),
            (1003, 1, 2, DateTime.Parse("2024-01-13")),
            (1004, 3, 3, DateTime.Parse("2024-01-15")),
            (1005, 2, 4, DateTime.Parse("2024-01-18")),
            (1006, 4, 1, DateTime.Parse("2024-01-22")),
            (1007, 5, 2, DateTime.Parse("2024-01-25")),
            (1008, 3, 5, DateTime.Parse("2024-01-29")),
            (1009, 6, 3, DateTime.Parse("2024-02-01")),
            (1010, 1, 1, DateTime.Parse("2024-02-05")),
            (1011, 7, 2, DateTime.Parse("2024-02-08")),
            (1012, 8, 3, DateTime.Parse("2024-02-10")),
            (1013, 9, 4, DateTime.Parse("2024-02-12")),
            (1014, 10, 2, DateTime.Parse("2024-02-14")),
            (1015, 1, 5, DateTime.Parse("2024-02-16")),
        };

        List<(int TicketId, int HoursSpent, int Score)> resolutions = new()
        {
            (1001, 4, 5), (1002, 2, 4), (1003, 5, 5),
            (1004, 3, 3), (1005, 7, 4), (1006, 6, 5),
            (1007, 4, 2), (1008, 5, 3), (1009, 6, 4),
            (1010, 5, 5), (1011, 3, 4), (1012, 2, 3),
            (1013, 6, 5), (1014, 4, 3), (1015, 3, 4)
        };

        public Challenge_20()
        {
            //TopPerformingAgents_t1();
            //MostActiveTicketCategories_t2();
            EfficiencyByCategory_t3();
        }

        // 🔹 Task 1: Top-Performing Support Agent
        // Your goal is to assess which support agents consistently deliver high-quality resolutions.Using the ticket and feedback data:
        //- Identify agents who have handled at least three support tickets.
        //- For each of those agents, calculate their average customer feedback score.
        //- Rank the agents based on their average scores in descending order.
        //- Present the results in pages with two agents per page.
        private void TopPerformingAgents_t1()
        {
            var topAgents = from agent in agents
                            join ticket in tickets on agent.AgentId equals ticket.AgentId
                            join resolv in resolutions on ticket.TicketId equals resolv.TicketId
                            group new { ticket, resolv } by agent into agentGroup

                            let ticketCount = agentGroup.Count()

                            where ticketCount >= 3

                            let avgScore = agentGroup.Average(x => x.resolv.Score)

                            orderby avgScore descending

                            select new
                            {
                                agentGroup.Key.Name,
                                TicketCount = ticketCount,
                                AvgScore = Math.Round(avgScore, 2)
                            };

            foreach (var agent in topAgents)
            {
                WriteLine($"{agent.Name}\t{agent.TicketCount} tickets\t\t{agent.AvgScore}");
            }

            WriteLine();

            var recordsPerPage = 2;
            for (var pageNum = 0; ;)
            {
                var results = topAgents.Skip(pageNum * recordsPerPage).Take(recordsPerPage);
                if (!results.Any()) return;

                WriteLine($"\nPage[ {++pageNum} ]");
                foreach (var agent in results)
                {
                    WriteLine($"{agent.Name}\t{agent.TicketCount} tickets\t\t{agent.AvgScore}");
                }
            }
        }

        // Task 2: Most Active Ticket Categories
        //You want to understand which types of issues are most frequently reported.Using the ticket history:
        //- Determine how many tickets fall under each category.
        //- Rank the categories from most to least active.
        //🔍 Expected Insight: A list showing which issue categories (e.g., Networking, Software, Hardware) generate the most support activity.
        private void MostActiveTicketCategories_t2()
        {
            var topCategories = from categ in categories
                                join ticket in tickets on categ.CategoryId equals ticket.CategoryId
                                group ticket by categ into categGroup
                                let ticketCount = categGroup.Count()
                                orderby ticketCount descending
                                select new
                                {
                                    Category = categGroup.Key,
                                    TicketCount = ticketCount
                                };

            foreach (var categ in topCategories)
            {
                WriteLine($"{categ.Category.Title}\t\t{categ.TicketCount}");
            }
        }

        // 🔹 Task 3: Efficiency of Ticket Resolutions by Category
        //You’d like to assess how efficiently issues are being resolved, depending on their category:
        //- For each category, calculate the average time spent resolving tickets and the average feedback score.
        //- Only include categories that have at least three resolved tickets.
        //🔍 Expected Insight: A summary comparing resolution speed and quality across categories, helping pinpoint which areas are performing well or may need improvement.
        private void EfficiencyByCategory_t3()
        {
            var effiByCategory = (from categ in categories
                                  join ticket in tickets on categ.CategoryId equals ticket.CategoryId
                                  join resolv in resolutions on ticket.TicketId equals resolv.TicketId
                                  group new { ticket, resolv } by categ into cateGroup

                                  let ticketCount = cateGroup.Count()

                                  where ticketCount >= 3

                                  let avgTime = cateGroup.Average(x => x.resolv.HoursSpent)
                                  let avgScore = cateGroup.Average(x => x.resolv.Score)

                                  orderby avgScore descending

                                  select new
                                  {
                                      Category = cateGroup.Key.Title,
                                      TicketCount = ticketCount,
                                      AvgTime = Math.Round(avgTime, 2),
                                      AvgScore = Math.Round(avgScore, 2)
                                  }).ToList();

            var timeAvg = Math.Round(effiByCategory.Average(x => x.AvgTime), 2);
            var scoreAvg = Math.Round(effiByCategory.Average(x => x.AvgScore), 2);

            WriteLine($"Global Avg Time: {timeAvg}");
            WriteLine($"Global Avg Score: {scoreAvg}\n");

            foreach (var item in effiByCategory)
            {
                WriteLine($"{item.Category}\t\t{item.TicketCount} tickets \t\t{item.AvgTime} AvgTime\t\t{item.AvgScore} AvgScore\t\t" +
                    $"{(item.AvgScore >= scoreAvg ? "Above Score" : "Below Score")}\t\t" +
                    $"{(item.AvgTime > timeAvg ? "More than Avg Time" : "Less than Avg time")}");
            }
        }
    }
}
