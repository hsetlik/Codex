using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Core;
using Application.Extensions;
using Application.Interfaces;
using Application.Parsing;
using Application.Utilities;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.EntityFrameworkCore;
using Persistence;
using YoutubeExplode;
using YoutubeExplode.Videos.ClosedCaptions;

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

        public async Task<Result<List<VideoCaptionElement>>> GetCaptions(string videoId, int fromMs, int numCaptions, string language)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            /*
            var ytService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyD0oGjw3IM-1f8Sf9N5OO8aLiChFAbL-Y4",
            });
            Console.WriteLine($"Video has ID: {videoId}");
            var searchRequest = ytService.Videos.List("snippet");
            var captionsTask = ytService.Captions.List("snippet", videoId);
            var gCaptions = await captionsTask.ExecuteAsync();
          
            searchRequest.Id = videoId;
            var searchResponse = await searchRequest.ExecuteAsync();
            var videoSnippet = searchResponse.Items.FirstOrDefault();
            watch.Stop();
            Console.WriteLine($"Getting video snippet took {watch.ElapsedMilliseconds} ms");

            */
            var output = new List<VideoCaptionElement>();

            var youtube = new YoutubeClient();
            var manifest = await youtube.Videos.ClosedCaptions.GetManifestAsync(videoId);
            watch.Stop();
            // Console.WriteLine($"Getting track manifest took {watch.ElapsedMilliseconds} ms");
            var trackInfo = manifest.GetByLanguage(language);
            watch.Restart();
            var track = await youtube.Videos.ClosedCaptions.GetAsync(trackInfo);
            watch.Stop();
            Console.WriteLine($"Getting track took {watch.ElapsedMilliseconds} ms");
            watch.Restart();
            var firstCaption = track.CaptionOnOrAfter((double)fromMs);
            // var shortestCaption = track.Captions.WithMin(cap => cap.Duration);
            // Console.WriteLine($"Shortest caption has length {shortestCaption.Duration} and text {shortestCaption.Text}");
            var startIndex = track.Captions.ToList().IndexOf(firstCaption);
            var count = (startIndex + numCaptions < track.Captions.Count) ? numCaptions : track.Captions.Count - startIndex;
            var captions = track.Captions.ToList().GetRange(startIndex, count).ToList();
            foreach(var cap in captions)
            {
                Console.WriteLine($"Caption text: {cap.Text}");
                var element = new VideoCaptionElement
                {
                    CaptionText = cap.Text.WithoutNewlines(),
                    StartMs = (int)cap.Offset.TotalMilliseconds,
                    EndMs = (int)cap.Offset.TotalMilliseconds + (int)cap.Duration.TotalMilliseconds
                };
                if (output.Count > 0)
                {
                     if (output[output.Count - 1].EndMs > element.StartMs)
                     {
                         output[output.Count - 1].EndMs = element.StartMs;
                     }
                }
                output.Add(element);
            }
            watch.Stop();
            Console.WriteLine($"Parsing into {output.Count} captions took {watch.ElapsedMilliseconds} ms");
            return Result<List<VideoCaptionElement>>.Success(output);
        }

    }
    public static class CaptionTrackExtensions
    {
        public static ClosedCaption CaptionOnOrAfter(this ClosedCaptionTrack track, double ms)
        {
            var current = track.TryGetByTime(TimeSpan.FromMilliseconds(ms));
            if (current != null)
                return current;
            while (current == null)
            {
               current = track.TryGetByTime(TimeSpan.FromMilliseconds(ms));
               ms += 2.0f; 
            }
            return current;
        }
        
    }
}