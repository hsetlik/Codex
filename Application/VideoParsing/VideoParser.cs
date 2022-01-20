using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Parsing;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YoutubeExplode;

namespace Application.VideoParsing
{
    public class VideoParser : IVideoParser
    {
        public VideoParser()
        {

        }
        private static string YoutubeVideoId(string url)
        {
            const string exp = @"(?<=v=)[\w\W]+";
            var match = Regex.Match(url, exp);
            return match.Value;
        }

        public async Task<Result<List<VideoCaptionElement>>> GetNextCaptions(string contentUrl, int fromMs, int numCaptions)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var ytService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyD0oGjw3IM-1f8Sf9N5OO8aLiChFAbL-Y4",
            });
            var videoId = YoutubeVideoId(contentUrl);
            var searchRequest = ytService.Videos.List("snippet");
            searchRequest.Id = videoId;
            var searchResponse = await searchRequest.ExecuteAsync();
            var videoSnippet = searchResponse.Items.FirstOrDefault();
            var language = videoSnippet.Snippet.DefaultAudioLanguage.Substring(0, 2);
            watch.Stop();
            Console.WriteLine($"Getting language {language} took {watch.ElapsedMilliseconds} ms");

            watch.Restart();
            var output = new List<VideoCaptionElement>();
            var youtube = new YoutubeClient();
            var manifest = await youtube.Videos.ClosedCaptions.GetManifestAsync(videoId);
            var trackInfo = manifest.GetByLanguage(language);
            var track = await youtube.Videos.ClosedCaptions.GetAsync(trackInfo);
            var firstCaption = track.GetByTime(TimeSpan.FromMilliseconds((double)fromMs));
            var startIndex = track.Captions.ToList().IndexOf(firstCaption);
            var count = (startIndex + numCaptions < track.Captions.Count) ? numCaptions : track.Captions.Count - startIndex;
            var captions = track.Captions.ToList().GetRange(startIndex, count).ToList();
            watch.Stop();
            Console.WriteLine($"Got {captions.Count} captions in {watch.ElapsedMilliseconds} ms");

            foreach(var cap in captions)
            {
                output.Add(new VideoCaptionElement
                {
                    Tag = "caption",
                    ElementText = cap.Text,
                    ContentUrl = contentUrl,
                    StartMs = (int)cap.Offset.TotalMilliseconds,
                    EndMs = (int)cap.Offset.TotalMilliseconds + (int)cap.Duration.TotalMilliseconds
                });
            }
            return Result<List<VideoCaptionElement>>.Success(output);
            
        }
    }
}