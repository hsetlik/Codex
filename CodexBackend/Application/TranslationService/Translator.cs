using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Translator;
using Application.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;

namespace Application.TranslationService
{
    public class Translator : ITranslator
    {
        private TranslationClient client;
        public Translator()
        {
            var credential = GoogleCredential.FromFile("../ApiKeys/api-keys.json");
            client = TranslationClient.Create(credential);
        }

        public async Task<Result<TranslatorResponse>> GetTranslation(TranslatorQuery query)
        {
            var response = await client.TranslateTextAsync(query.QueryValue, query.ResponseLanguage, query.QueryLanguage);
            if (response == null)
                return Result<TranslatorResponse>.Failure("Could not get translator response");
            var translation = new TranslatorResponse
            {
                Query = query,
                ResponseValue = response.TranslatedText
            };
            return Result<TranslatorResponse>.Success(translation);
        }
    }
}