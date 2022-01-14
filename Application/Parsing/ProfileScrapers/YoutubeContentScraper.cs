using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Application.DomainDTOs.Content.Responses;
using Application.Parsing.ContentStorage;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YoutubeExplode;

namespace Application.Parsing.ProfileScrapers
{
    public class YoutubeContentScraper : AbstractScraper
    {
        private const int captionsPerSection = 15;
        private YoutubeContentStorage storage = new YoutubeContentStorage();
        
        public YoutubeContentScraper(string url) : base(url)
        {
            
            
        }

        public override List<ContentSection> GetAllSections()
        {
            return storage.Sections.Select(s => (ContentSection)s).ToList();
        }

        public override ContentMetadataDto GetMetadata()
        {
            return storage.Metadata;
        }

        public override int GetNumSections()
        {
            return storage.Sections.Count;
        }

        public override ContentSection GetSection(int index)
        {
            return storage.Sections[index];
        }

        private static string YoutubeVideoId(string url)
        {
            const string exp = @"(?<=v=)[\w\W]+";
            var match = Regex.Match(url, exp);
            return match.Value;
        }

        public override async Task PrepareAsync()
        {
            var ytService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyD0oGjw3IM-1f8Sf9N5OO8aLiChFAbL-Y4",
            });
            var videoId = YoutubeVideoId(Url);
            var searchRequest = ytService.Videos.List("snippet");
            searchRequest.Id = videoId;
            var searchResponse = await searchRequest.ExecuteAsync();
            var videoSnippet = searchResponse.Items.FirstOrDefault();
            
            var ytClient = new YoutubeClient();
            var video = await ytClient.Videos.GetAsync(Url);

            var language = videoSnippet.Snippet.DefaultAudioLanguage.Substring(0, 2);
            Console.WriteLine($"Video has title {video.Title}, duration {video.Duration}, uploadDate {video.UploadDate}, and language {language}");
            var tracks = await ytClient.Videos.ClosedCaptions.GetManifestAsync(videoId);
            
            storage.Metadata = new ContentMetadataDto
            {
                ContentUrl = Url,
                VideoUrl = Url,
                AudioUrl = Url,
                ContentType = "Youtube",
                ContentName = video.Title,
                Language = language
            };
            Console.WriteLine(storage.Metadata.ContentName);

            storage.Sections = new List<YoutubeSection>();

            var trackInfo = tracks.TryGetByLanguage(storage.Metadata.Language);
            
            var track = await ytClient.Videos.ClosedCaptions.GetAsync(trackInfo);
            int idx = 0;
            Console.WriteLine($"Url is: {Url}");
            var currentSection = new YoutubeSection
            {
                ContentUrl = Url,
                Index = 0,
                SectionHeader = "0",
                TextElements = new List<VideoCaptionElement>()
            };
            foreach(var caption in track.Captions)
            {
                Console.WriteLine($"Caption is {caption.Text} at {caption.Offset.TotalMilliseconds} with duration {(float)(caption.Duration.Milliseconds)} ms");
                var element = new VideoCaptionElement
                {
                    Tag = "caption",
                    ElementText = caption.Text,
                    Index = idx,
                    StartMs = (int)caption.Offset.TotalMilliseconds,
                    EndMs = (int)(caption.Offset.TotalMilliseconds + caption.Duration.TotalMilliseconds),
                    ContentUrl = Url
                };
                //handle overlapping captions
                if (!(storage.Sections.Count == 0 && currentSection.TextElements.Count == 0))
                {
                    if (currentSection.TextElements.Count < 1)
                    {   // in this case we need to get the last section from storage
                        var lastElement = storage.Sections.Last().TextElements.Last();
                        if (lastElement.EndMs > element.StartMs)
                        {
                            storage.Sections[storage.Sections.Count - 1].TextElements[storage.Sections.Last().TextElements.Count - 1].EndMs = element.StartMs;
                        }
                    }
                    else if (currentSection.TextElements.Last().EndMs > element.StartMs)
                    {
                        currentSection.TextElements[currentSection.TextElements.Count - 1].EndMs = element.StartMs;
                    }
                }
                currentSection.TextElements.Add(element);
                if (idx > captionsPerSection)
                {
                    idx = 0;
                    storage.Sections.Add(currentSection);
                    currentSection = new YoutubeSection
                    {
                        ContentUrl = Url,
                        Index = storage.Sections.Count,
                        SectionHeader = storage.Sections.Count.ToString(),
                        TextElements = new List<VideoCaptionElement>()
                    };
                }
                ++idx;
            }
            if (currentSection.TextElements.Count > 0 && storage.Sections.Last() != currentSection)
            {
                storage.Sections.Add(currentSection);

            }
            storage.Metadata.NumSections = storage.Sections.Count;
        }

        public override ContentPageHtml GetPageHtml()
        {
            throw new NotImplementedException();
        }
    }
}