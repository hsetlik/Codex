using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;

namespace Application.DomainDTOs.ProfileHistory.DailyData
{
    public class DataPointQuery
    {
        public string MetricName { get; set; }
        public DateTime DateTime { get; set; }
        public Guid LanguageProfileId { get; set; }
        
    }
    public class DailyDataPoint
    {
        public string MetricName { get; set; }
        public string ValueTypeName { get; protected set; }
        public string ValueString { get; protected set; }
        public Guid LanguageProfileId { get; protected set; }
        public DateTime DateTime { get; protected set; }
        public T GetValue<T>() where T: struct
        {
            return (T)Convert.ChangeType(ValueString, typeof(T));
        }
        public void SetValue<T>(T newValue) where T: struct
        {
            string tString = typeof(T).ToString();
            if (tString != ValueTypeName) // if we've gotten a different type we need to change this
            {
                ValueTypeName = tString;
            }
            ValueString = Convert.ToString(newValue);
        }

        public DailyDataPoint(Guid profileId, DateTime time)
        {
            LanguageProfileId = profileId;
            DateTime = time; 
        }
    }

    public class KnownWordsDataPoint : DailyDataPoint
    {
        public KnownWordsDataPoint(Guid profileId, DateTime time, int value=0) : base(profileId, time)
        {
            MetricName = "KnownWords";
            ValueTypeName = "int";
            ValueString = Convert.ToString(value);
        }
    }

    public class NumUserTermsDataPoint : DailyDataPoint
    {
        public NumUserTermsDataPoint(Guid profileId, DateTime time, int value=0) : base(profileId, time)
        {
            MetricName = "NumUserTerms";
            ValueTypeName = "int";
            ValueString = Convert.ToString(value);
        }
    }
}