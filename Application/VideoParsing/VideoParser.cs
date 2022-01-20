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
            var output = new List<VideoCaptionElement>();
            var youtube = new YoutubeClient();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var manifest = await youtube.Videos.ClosedCaptions.GetManifestAsync(videoId);
            watch.Stop();
            Console.WriteLine($"Getting track manifest took {watch.ElapsedMilliseconds} ms");
            var trackInfo = manifest.GetByLanguage(language);
            watch.Restart();
            var track = await youtube.Videos.ClosedCaptions.GetAsync(trackInfo);
            watch.Stop();
            Console.WriteLine($"Getting track manifest took {watch.ElapsedMilliseconds} ms");
            var firstCaption = track.CaptionAfter((double)fromMs);
            var startIndex = track.Captions.ToList().IndexOf(firstCaption);
            var count = (startIndex + numCaptions < track.Captions.Count) ? numCaptions : track.Captions.Count - startIndex;
            var captions = track.Captions.ToList().GetRange(startIndex, count).ToList();
            watch.Stop();

            foreach(var cap in captions)
            {
                output.Add(new VideoCaptionElement
                {
                    CaptionText = cap.Text,
                    StartMs = (int)cap.Offset.TotalMilliseconds,
                    EndMs = (int)cap.Offset.TotalMilliseconds + (int)cap.Duration.TotalMilliseconds
                });
            }
            return Result<List<VideoCaptionElement>>.Success(output);
        }

    }
    public static class CaptionTrackExtensions
    {
        public static ClosedCaption CaptionAfter(this ClosedCaptionTrack track, double ms)
        {
            return track.Captions.FirstOrDefault(c => c.Offset.TotalMilliseconds >  ms);
        }
        public static ClosedCaption CaptionBefore(this ClosedCaptionTrack track, double ms)
        {
            var after = track.CaptionAfter(ms);
            var aIndex = track.Captions.ToList().IndexOf(after);
            return (aIndex < 1) ? track.Captions[0] : track.Captions[aIndex + 1];
        }
    }
}