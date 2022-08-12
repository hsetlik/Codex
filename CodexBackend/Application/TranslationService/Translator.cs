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
            string keysPath = "../ApiKeys/api-keys.json";
            //===========================================
            var reader = File.OpenText(keysPath);
            string fileString = reader.ReadToEnd();
            Console.WriteLine($"Credentials file: {fileString}");
            //===========================================
            var credential = GoogleCredential.FromFile(keysPath);
            client = TranslationClient.Create(credential);
        }

        public async Task<Result<TranslatorResponse>> GetTranslation(TranslatorQuery query)
        {
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