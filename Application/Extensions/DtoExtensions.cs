using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs;
using Application.Utilities;
using Domain.DataObjects;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class DtoExtensions
    {
        public static UserTermDto AsUserTerm(this AbstractTermDto dto)
        {
            var uTerm = new UserTermDto
            {
                Value = dto.TermValue,
                Language = dto.Language,
                EaseFactor = dto.EaseFactor,
                SrsIntervalDays = dto.SrsIntervalDays,
                Rating = dto.Rating,
                Translations = dto.Translations,
                TimesSeen = dto.TimesSeen
            };
            return uTerm;
        }

        public static async Task<Result<Transcript>> CreateTranscriptFrom(this CreateTranscriptDto dto, DataContext context,int wordsPerChunk = 80)
        {
            var output = new Transcript
            {
                Language = dto.Language
            };
            var rawStrings = dto.FullText.Split(' ');
            var chunks = new List<TranscriptChunk>();
            string currentChunkString = "";
            int currentChunkSize = 0;
            for (int i = 0; i < rawStrings.Count(); ++i)
            {
                // 1. add the unaltered string to the current chunk
                currentChunkString += rawStrings[i] + ' ';
                currentChunkSize += 1;
                // 2. get the normalized, word-only version of the raw string
                var normalizedTerm = StringUtilityMethods.AsTermValue(rawStrings[i]);
                // 3. check if a term with this value/language exists and create one if it doesn't
                var matchingTerm = await context.Terms.FirstOrDefaultAsync
                (u => u.NormalizedValue == normalizedTerm
                 && u.Language == dto.Language);
                if (matchingTerm == null)
                {
                    var result = await context.CreateTerm(dto.Language, normalizedTerm);
                    if (!result.IsSuccess)
                        return Result<Transcript>.Failure("Term could not be found or created");
                }
                //Create and add the new chunk once it reaches its max size
                if (currentChunkSize > wordsPerChunk)
                {
                    var chunk = new TranscriptChunk
                    {
                        Language = dto.Language,
                        ChunkText = currentChunkString,
                        Transcript = output,
                        TranscriptChunkIndex = chunks.Count
                    };
                    chunks.Add(chunk);
                    currentChunkSize = 0;
                    currentChunkString = "";
                } 
            }
            //add the last partial chunk if one exists
            if (currentChunkSize > 0)
            {
                var chunk = new TranscriptChunk
                    {
                        Language = dto.Language,
                        ChunkText = currentChunkString,
                        Transcript = output,
                        TranscriptChunkIndex = chunks.Count
                    };
                    chunks.Add(chunk);
            }
            output.TranscriptChunks = chunks;
            return Result<Transcript>.Success(output);
        }

    }
}