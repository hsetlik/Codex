using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataObjects;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Terms
{
    public class AbstractTermDto
    {
        public string TermValue { get; set; }
        public string TrailingCharacters { get; set; } 
        public string Language { get; set; } 
        public int IndexInChunk { get; set; }
        public bool HasUserTerm { get; set; } // Whether a matching userTerm exists
        public float EaseFactor { get; set; }
        public float SrsIntervalDays { get; set; }
        public int Rating  { get; set; }
        public List<string> Translations { get; set; }
        public Guid UserTermId { get; set; }
        public int TimesSeen { get; set; }
    }

    public static class AbstractTermFactory
    {
        public static AbstractTermDto Generate(TermDto source) 
        {
            var output = new AbstractTermDto
            {
                TermValue = source.Value,
                Language = source.Language,
                HasUserTerm = false,
                SrsIntervalDays = 0,
                Rating = 0,
                Translations = new List<string>()
            };
            var type = source.GetType();
            if (type == typeof(RawTermDto))
            {
                output.HasUserTerm = false;
            }
            else
            {
                var userTerm = (UserTermDto)source;
                output.HasUserTerm = true;
                output.SrsIntervalDays = userTerm.SrsIntervalDays;
                output.Rating = userTerm.Rating;
                output.Translations = userTerm.Translations;
                output.EaseFactor = userTerm.EaseFactor;
                output.UserTermId = userTerm.UserTermId;
                output.TimesSeen = userTerm.TimesSeen;
            }
            return output;
        }
    }
}