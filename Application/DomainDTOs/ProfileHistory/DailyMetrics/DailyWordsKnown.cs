using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Extensions;
using Persistence;

namespace Application.DomainDTOs.ProfileHistory.DailyMetrics
{
    public class DailyWordsKnown : DailyMetricBase<int>
    {
        private bool _prepared;
        private int _value;
        public DailyWordsKnown()
        {
            _value = 0;
            _prepared = false;
        }

        public override bool IsPrepared => _prepared;

        public override DateTime DateTime { get ; set ; }

        public override int Value => _value;

        public override async Task PrepareAsync(DataContext context, Guid languageProfileId)
        {
            var recordResult = await context.GetClosestRecord(languageProfileId, DateTime);
            if (recordResult.IsSuccess)
            {
                this._value = recordResult.Value.KnownWords;
            }
            this._prepared = true;
        }
    }
}