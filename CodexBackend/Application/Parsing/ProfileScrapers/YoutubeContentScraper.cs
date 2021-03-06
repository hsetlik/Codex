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
using Google.Apis.YouTube.v3.Data;
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
            Console.WriteLine($"Video has ID: {videoId}");
            var searchRequest = ytService.Videos.List("snippet");
            searchRequest.Id = videoId;
            var searchResponse = await searchRequest.ExecuteAsync();
            var videoSnippet = searchResponse.Items.FirstOrDefault();
            
            var ytClient = new YoutubeClient();
            var video = await ytClient.Videos.GetAsync(Url);
            var language = videoSnippet.Snippet.DefaultAudioLanguage.Substring(0, 2);
            
            storage.Metadata = new ContentMetadataDto
            {
                ContentUrl = Url,
                VideoId = videoId,
                AudioUrl = Url,
                ContentType = "Youtube",
                ContentName = video.Title,
                Language = language
            };

        }

        public override ContentPageHtml GetPageHtml()
        {
            throw new NotImplementedException();
        }

        public override string GetPageText()
        {
            string output = "";
            var ytService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyD0oGjw3IM-1f8Sf9N5OO8aLiChFAbL-Y4",
            });
            var videoId = YoutubeVideoId(Url);
            var searchRequest = ytService.Videos.List("snippet");
            searchRequest.Id = videoId;
            var searchResponse = Task.Run<VideoListResponse>(async () => await searchRequest.ExecuteAsync());
            var videoSnippet = searchResponse.Result.Items.FirstOrDefault();
            var ytClient = new YoutubeClient();
            var video = Task.Run(async () => await ytClient.Videos.GetAsync(Url));
            var language = videoSnippet.Snippet.DefaultAudioLanguage.Substring(0, 2);
            var tracks = Task.Run(async () => await ytClient.Videos.ClosedCaptions.GetManifestAsync(videoId));
            var trackInfo = tracks.Result.GetByLanguage(videoSnippet.Snippet.DefaultAudioLanguage);
            var track = Task.Run(async () => await ytClient.Videos.ClosedCaptions.GetAsync(trackInfo));
            foreach(var caption in track.Result.Captions)
            {
                output += caption.Text;
            } 
            return output;
        }
    }
}