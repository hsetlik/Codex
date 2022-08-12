using System;
using System.Collections.Generic;
using System.IO;
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
            client = null;
        }

        public async Task<Result<TranslatorResponse>> GetTranslation(TranslatorQuery query, string key)
        {
            if (client == null)
            {
                var cred = GoogleCredential.FromJson(key);
                client = TranslationClient.Create(cred);
            }
            TranslationResult response = null;
            try
            {
                response = await client.TranslateTextAsync(query.QueryValue, query.ResponseLanguage, query.QueryLanguage);
            }
            catch (Exception ex)
            {
                return Result<TranslatorResponse>.Failure($"client.TranslateTextAsync() threw exception: {ex.Message}");
                throw;
            }
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