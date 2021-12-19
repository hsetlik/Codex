using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.UserLanguageProfile;
using Application.Interfaces;

namespace Application.Extensions
{
    public static class FactoryExtensions
    {
       public static async Task<Result<KnownWordsDto>> KnownWordsForList(this IDataRepository factory, List<string> words, Guid languageProfileId)
       {
           int known = 0;
           await Task.Run(() => 
           {
               Parallel.ForEach(words, async word => 
               {
                var isKnown = await factory.WordIsKnown(word, languageProfileId);
                if (isKnown.IsSuccess && isKnown.Value)
                    known += 1;
               });
           });
           return Result<KnownWordsDto>.Success(new KnownWordsDto
           {
               KnownWords = known,
               TotalWords = words.Count
           });
       } 
        
    }
}