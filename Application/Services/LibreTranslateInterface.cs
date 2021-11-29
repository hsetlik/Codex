using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs.Term;

namespace Application.Services
{
    public class LibreTranslateInterface
    {
        public static async Task<DictionaryEntryDto> GetTranslation(TermDto term)
        {
            var requestDto = new LibreRequestDto
            {

            };
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:4000");
            var response = await client.PostAsync("/translate", JsonContent.Create<LibreRequestDto>(requestDto));


            //var object = ReadFromJsonAsync(response.Content, typeof())

            return new DictionaryEntryDto
            {
                TermValue = term.Value,
                TermLanguage = term.Language,
                TranslationValue = ""
            };
        }
    }
}