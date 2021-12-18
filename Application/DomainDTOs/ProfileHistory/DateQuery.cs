using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.ProfileHistory
{
    public class DateQuery
    {
        DateQuery(string input)
        {
            var components = (input.Contains('-')) ? input.Split('-') : input.Split('/');
            if (components.Length != 3)
                Console.WriteLine($"{components.Length} date string components found");
            this.Day = int.Parse(components[0]);
            this.Month = int.Parse(components[1]);
            this.Year = int.Parse(components[2]);
        }
        DateQuery(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool Matches(DateTime dateTime)
        {
            if (dateTime.Date.Day == Day && dateTime.Date.Month == Month && dateTime.Date.Year == Year)
                return true;
            return false;
        }
    }
}