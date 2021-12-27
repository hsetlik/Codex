using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Application.Parsing.ContentStorage;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace Application.Parsing.ProfileScrapers
{
    public class YoutubeContentScraper : AbstractScraper
    {
        private YouTubeService ytService = new YouTubeService();
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

        public static string YoutubeVideoId(string url)
        {
            const string exp = @"(?<=v=)[\w\W]+";
            var match = Regex.Match(url, exp);
            return match.Value;
        }

        public override async Task PrepareAsync()
        {
            Console.WriteLine($"Preparing for youtube video at: {Url}");
            ytService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyD0oGjw3IM-1f8Sf9N5OO8aLiChFAbL-Y4",
            });
            var videoId = YoutubeVideoId(Url);
            Console.WriteLine($"Video ID is: {videoId}");


            var searchRequest = ytService.Videos.List("snippet");
            searchRequest.Id = videoId;
            var searchResponse = await searchRequest.ExecuteAsync();
            var videoSnippet = searchResponse.Items.FirstOrDefault();
            Console.WriteLine($"Found video with name: {videoSnippet.Snippet.Title} and language: {videoSnippet.Snippet.DefaultLanguage}");
            


            throw new NotImplementedException();
        }
    }
}