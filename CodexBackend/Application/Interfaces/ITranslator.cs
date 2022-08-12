using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Translator;

namespace Application.Interfaces
{
    public interface ITranslator
    {
        Task<Result<TranslatorResponse>> GetTranslation(TranslatorQuery query, string key);
    }
}