using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.DomainDTOs;
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
            throw new NotImplementedException();
        }

        public override ContentMetadataDto GetMetadata()
        {
            throw new NotImplementedException();
        }

        public override int GetNumSections()
        {
            throw new NotImplementedException();
        }

        public override ContentSection GetSection(int index)
        {
            throw new NotImplementedException();
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
            Console.WriteLine($"Video ID is: {videoId}");


            var searchRequest = ytService.Videos.List("snippet");
            searchRequest.Id = videoId;
            var searchResponse = await searchRequest.ExecuteAsync();
            var videoSnippet = searchResponse.Items.FirstOrDefault();
            var ytClient = new YoutubeClient();
            var video = await ytClient.Videos.GetAsync(Url);
            
            Console.WriteLine($"Video has title {video.Title}, duration {video.Duration}, and uploadDate {video.UploadDate}");
            var tracks = await ytClient.Videos.ClosedCaptions.GetManifestAsync(videoId);
            storage.Metadata = new ContentMetadataDto
            {
                ContentUrl = Url,
                VideoUrl = Url,
                AudioUrl = Url,
                ContentType = "Youtube",
                ContentName = video.Title,
                Language = videoSnippet.Snippet.DefaultLanguage
            };
            Console.WriteLine(storage.Metadata.ContentName);

            storage.CaptionElements = new List<CaptionElement>();

            var trackInfo = tracks.TryGetByLanguage(storage.Metadata.Language);
            var track = await ytClient.Videos.ClosedCaptions.GetAsync(trackInfo);
            
            int idx = 0;
            foreach(var caption in track.Captions)
            {
                Console.WriteLine($"Caption is {caption.Text} at {caption.Duration}");
                var element = new CaptionElement
                {
                    Tag = "caption",
                    Value = caption.Text,
                    Index = idx,
                    TimeSpan = caption.Duration,
                };
                storage.CaptionElements.Add(element);
                if (idx > captionsPerSection)
                    idx = 0;
            }
        }
    }
}