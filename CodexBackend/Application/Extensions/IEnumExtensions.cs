using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class IEnumExtensions
    {
        public static T WithMax<T, P>(this IEnumerable<T> list, Func<T, P> func) 
        where P : 
        struct, IComparable<P>
        {
            var output = list.First();
            foreach (var item in list)
            {
                var check = func(item);
                if (check.CompareTo(func(output)) > 0)
                {
                    output = item;
                }
            }
            return output;
        }

        public static T WithMin<T, P>(this IEnumerable<T> list, Func<T, P> func) 
        where P : 
        struct, IComparable<P>
        {
            var output = list.First();
            foreach (var item in list)
            {
                var check = func(item);
                if (check.CompareTo(func(output)) < 0)
                {
                    output = item;
                }
            }
            return output;
        }
        
    }
}