using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static BeatDetector.VDJSong;

namespace BeatDetector
{
    public class MLSongModel
    {
        public MLSongModel(VDJSong vDJSong)
        {
            VDJSong = vDJSong;
        }

        public bool MusicMLLoad { get; set; } = false;
        public bool MusicMLLoading { get; set; } = false;
        public bool MusicMLFailed { get; set; } = false;
        public VDJSong VDJSong { get; }

        internal void LoadMusicML()
        {
            string mlFile = VDJSong.FilePath + ".ml";
            /*if (File.Exists(mlFile))
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if (MusicMLLoad)
                            return;
                        string analysisJson = null;
                        using (var reader = new StreamReader(mlFile))
                        {
                            analysisJson = reader.ReadToEnd();
                        }

                        ComputeAnalysisAndBuildPOI(analysisJson);
                    }
                    catch (Exception)
                    {
                        
                    }
                    finally
                    {
                        MusicMLLoading = false;
                    }
                });
            }
            else*/ if (!MusicMLFailed && !MusicMLLoading)
            {
                MusicMLLoading = true;
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo("env1\\Scripts\\python.exe", "MLAnalyser.py \"" + VDJSong.FilePath + "\"");
                        startInfo.RedirectStandardOutput = true;
                        startInfo.UseShellExecute = false;
                        var mlAnalyzer = Process.Start(startInfo);
                        var analysisResult = mlAnalyzer.StandardOutput.ReadToEnd();
                        var analysisJson = analysisResult.Substring(analysisResult.IndexOf("Result =") + 9);
                        ComputeAnalysisAndBuildPOI(analysisJson);

                        using (var streamWriter = new StreamWriter(mlFile))
                        {
                            streamWriter.Write(analysisJson);
                        }
                    }
                    catch (Exception)
                    {
                        MusicMLFailed = true;
                    }
                    finally
                    {
                        MusicMLLoading = false;
                    }
                });
            }

        }

        private void ComputeAnalysisAndBuildPOI(string analysisJson)
        {
            var json = JsonConvert.DeserializeObject<MusicMLModel>(analysisJson);
            var unsortedTimeLine = new List<MusicMLTimeLineItem>();

            for (int pos = 0; pos < json.SmallSampleRate.Length; pos++)
            {
                var line = new MusicMLTimeLineItem { Position = new TimeSpan(0, 0, 0, 0, pos *480), TagType = MusicMLTagType.Small, };
                for (int tagPos = 0; tagPos < json.SmallSampleTags.Length; tagPos++)
                {
                    line.Tag.Add(new MusicMLTag { Value = json.SmallSampleRate[pos][tagPos], Tag = json.GetOrCreateMLTag(json.SmallSampleTags[tagPos]) });
                }
                unsortedTimeLine.Add(line);
            }

            for (int pos = 0; pos < json.BigSampleRate.Length; pos++)
            {
                var line = new MusicMLTimeLineItem { Position = new TimeSpan(0, 0, 0, 0, 5000 * pos), TagType = MusicMLTagType.Big, };
                for (int tagPos = 0; tagPos < json.BigSampleTags.Length; tagPos++)
                {
                    line.Tag.Add(new MusicMLTag { Value = json.BigSampleRate[pos][tagPos], Tag = json.GetOrCreateMLTag(json.BigSampleTags[tagPos]) });
                }
                unsortedTimeLine.Add(line);
            }

            foreach (var tag in json.MLTags)
            {
                var allValueForGivenTag = unsortedTimeLine.Where(o => o.TagType == MusicMLTagType.Big)
                .SelectMany(o => o.Tag)
                .Where(o => o.Tag.Name == tag.Name);
                tag.AverageLow25 = allValueForGivenTag
                .OrderBy(o => o.Value)
                .Take(allValueForGivenTag.Count() / 4)
                .Select(o => o.Value)
                .Average();

                tag.AverageHigh25 = allValueForGivenTag
                .OrderByDescending(o => o.Value)
                .Take(allValueForGivenTag.Count() / 4)
                .Select(o => o.Value)
                .Average();

                tag.Average = allValueForGivenTag
                .OrderBy(o => o.Value)
                .Select(o => o.Value)
                .Average();
            }
            json.TimeLines.AddRange(unsortedTimeLine.OrderBy(ml => ml.Position.Ticks));
            bool isBreakBig = false;
            bool isBreakSmall = false;
            bool recordedEndBrakePoi = false;
            bool recordedBrakePoi = false;
            var pois = new List<VDJPoi>();
            TimeSpan accuratePos = new TimeSpan(0);
            TimeSpan grossPos = new TimeSpan(0);
            int iteration = 0;
            foreach (var timeLine in json.TimeLines)
            {
                var breakTag = "dance";
                if (json.MLTags.FirstOrDefault(o=> o.Name == breakTag)?.AverageHigh25 > 0.4 )
                {
                    var currentTag = timeLine.Tag.FirstOrDefault(o => o.Tag.Name == breakTag);
                    if (filter(json, breakTag, currentTag))
                    {
                        if (timeLine.TagType == MusicMLTagType.Big && isBreakBig)
                        {
                            isBreakBig = false;
                            grossPos = timeLine.Position;
                        }
                        if (!isBreakBig && !recordedEndBrakePoi && timeLine.TagType == MusicMLTagType.Big)
                        {
                            //record endbreak poi
                            recordedEndBrakePoi = true;
                            recordedBrakePoi = false;
                            var relativePos = json.TimeLines.IndexOf(timeLine) - 10;
                            if (relativePos < 0)
                            {
                                relativePos = 0;
                            }

                            var testSmallList = json.TimeLines.Skip(relativePos)
                                .Take(21)
                                .Where( o=>o.TagType == MusicMLTagType.Small);
                            var accurate = testSmallList.FirstOrDefault(o=> filterSmall(json,breakTag,o.Tag.FirstOrDefault(w => w.Tag.Name == breakTag)));
                            if (accurate !=null)
                            {
                                pois.Add(new VDJPoi("End Break", accurate.Position, null));
                            }
                            else
                            {
                                pois.Add(new VDJPoi("End Break", grossPos, null));
                            }
                            
                        }
                    }
                    else
                    {
                        // break 

                        if (timeLine.TagType == MusicMLTagType.Big && !isBreakBig)
                        {
                            isBreakBig = true;
                            grossPos = timeLine.Position;
                        }
                        if (isBreakBig && !recordedBrakePoi && timeLine.TagType == MusicMLTagType.Big)
                        {
                            //record break poi
                            recordedBrakePoi = true;
                            recordedEndBrakePoi = false;
                            pois.Add(new VDJPoi("Break", grossPos, null));
                            var relativePos = json.TimeLines.IndexOf(timeLine) - 10;
                            if (relativePos < 0)
                            {
                                relativePos = 0;
                            }
                            var accurate = json.TimeLines.Skip(relativePos)
                                .Take(20)
                                .FirstOrDefault(o => !filterSmall(json, breakTag, o.Tag.FirstOrDefault(w => w.Tag.Name == breakTag)));
                            if (accurate != null)
                            {
                                pois.Add(new VDJPoi("End Break", accurate.Position, null));
                            }
                            else
                            {
                                pois.Add(new VDJPoi("End Break", grossPos, null));
                            }
                        }
                    }
                }
                iteration++;
            }

            MusicMLLoad = true;
        }

        private static bool filter(MusicMLModel json, string breakTag, MusicMLTag currentTag)
        {
            return currentTag.Value > (json.MLTags.FirstOrDefault(o => o.Name == breakTag).Median);
        }

        private static bool filterSmall(MusicMLModel json, string breakTag, MusicMLTag currentTag)
        {
            return currentTag.Value > .35;
        }
    }
}