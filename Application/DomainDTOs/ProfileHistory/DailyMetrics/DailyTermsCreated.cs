using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DomainDTOs.ProfileHistory.DailyMetrics
{

    public class DailyTermsCreated : DailyMetricBase<int>
    {
        private int _value;
        private bool _prepared;

        public DailyTermsCreated()
        {
            _value = 0;
            _prepared = false;
        }
        public override int Value { get {return _value; } }
        public override DateTime DateTime { get; set; }

        public override bool IsPrepared { get {return _prepared; }}

        public override async Task PrepareAsync(DataContext context, Guid languageProfileId)
        {
            var terms = await context.UserTerms.Where(u => u.LanguageProfileId == languageProfileId && u.CreatedAt.Date == DateTime.Date).ToListAsync();
            _value = terms.Count;
            _prepared = true;
        }
    }
}