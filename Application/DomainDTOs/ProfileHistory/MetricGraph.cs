using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs.ProfileHistory.DailyData;

namespace Application.DomainDTOs.ProfileHistory
{
    public class MetricGraphQuery
    {
        public string MetricName { get; set; }
        public Guid LanguageProfileId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public List<DataPointQuery> GetQueries(double intervalDays=1.0f)
        {
            var output = new List<DataPointQuery>();
            DateTime current = Start;
            int idx = 0;
            while (current < End)
            {
                var query = new DataPointQuery
                {
                    MetricName = this.MetricName,
                    LanguageProfileId = this.LanguageProfileId,
                    DateTime = current
                };
                Console.WriteLine($"{idx} is at {current}");
                output.Add(query);
                current = current.AddDays(intervalDays);
                ++idx;
            }
            return output;
        }
    }

    public class MetricGraph
    {
        public string MetricName { get; set; }
        public Guid LanguageProfileId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<DailyDataPoint> DataPoints { get; set; } = new List<DailyDataPoint>();

    }
}