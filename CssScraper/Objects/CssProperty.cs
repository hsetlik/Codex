using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CssScraper.Objects
{
    public class CssProperty<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
        public CssProperty()
        {
        }
        public CssProperty(string name, T value)
        {
            Value = value;
            Name = name;
        }
    }
}