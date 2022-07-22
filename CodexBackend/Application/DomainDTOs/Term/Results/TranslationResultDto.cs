using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs
{
    public class TranslationResultDto
{
    public string Value { get; set; }
    public string Annotation { get; set; }

    private static string CountAsAnnotation(int number)
    {
        return $"Used {number} times";
    }

    public TranslationResultDto()
    {

    }
    public TranslationResultDto(string value, int numOccurences)
    {
        this.Value = value;
        this.Annotation = CountAsAnnotation(numOccurences);
    }

    public TranslationResultDto(string value)
    {
        this.Value = value;
        this.Annotation = "from Google";
    }
}
}