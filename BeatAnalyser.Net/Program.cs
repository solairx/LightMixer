using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib;
using NAudio.Wave;
using NAudio.Flac;
using File = System.IO.File;
using Newtonsoft.Json;

namespace BeatAnalyser.Net
{
    internal partial class Program
    {
        private class FileRetry
        {
            public string Name { get; set; }
            public int RetryCount { get; set; } 
            public bool Success { get; set; }
        }

        static void Main(string[] args)
        {
            string tempsFolder = @"C:\tmp\audio";
            Directory.CreateDirectory(tempsFolder);

            //   string filename = @"C:\tmp\1.aiff";
            string filename = @"C:\tmp\3.flac";
            //string filename = @"C:\tmp\2.mp3";

            List<FileRetry> processesFile = new List<FileRetry>();
            int loop = 0;

            while (true)
            {
                try
                {

                    var fs = File.Open(@"D:\VirtualDJ\lightmixer.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs);
                    var str = sr.ReadToEnd()
                        .Replace("\n", "")
                        .Split('\r')
                        .Reverse();
                    sr.Close();
                    fs.Close();

                    foreach (var newFile in str)
                    {
                        try
                        {
                            if (File.Exists(newFile) && !File.Exists(newFile + ".json"))
                            {
                                FileRetry currentFileRetryCount = processesFile.Where(o => o.Name == newFile).FirstOrDefault();

                                if (currentFileRetryCount == null)
                                {
                                    currentFileRetryCount = new FileRetry { Name = newFile };
                                    Console.WriteLine("Processing " + newFile);
                                    processesFile.Add(currentFileRetryCount);
                                }


                                if (currentFileRetryCount.RetryCount < 3 && !currentFileRetryCount.Success)
                                {
                                    currentFileRetryCount.Success = ProcessFile(tempsFolder, newFile);
                                    currentFileRetryCount.RetryCount++;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    if (loop > 100)
                    {
                        loop = 0;
                        Console.WriteLine("Keep Pc Awake");
                        SendKeys.SendWait(@"{ENTER}");
                    }

                    System.Threading.Thread.Sleep(500);
                    loop++;

                }
                catch (Exception v)
                {
                    Console.WriteLine(v.ToString());

                }
            }

        }

        private static bool ProcessFile(string tempsFolder, string filename)
        {
            IntPtr window;
            Process p =null;
            try
            {
                string zplaneFileName = filename;
                string tempFile = null;
                TimeSpan totalTime = new TimeSpan();

                var fileStream = System.IO.File.OpenRead(filename);
                FileInfo fi = new FileInfo(filename);

                try
                {
                    if (filename.ToLower().EndsWith(".avi")
                        || filename.ToLower().EndsWith(".mkg")
                        || new FileInfo(filename).Length > 100 * 1024 * 1024)
                    {
                        Console.WriteLine("File not compatible" );
                        return true;
                    }
                    else if (filename.ToLower().EndsWith(".aiff"))
                    {
                        var r = new NAudio.Wave.AiffFileReader(fileStream);
                        totalTime = r.TotalTime;
                        r.Dispose();
                    }

                    else if (filename.ToLower().EndsWith(".mp3"))
                    {
                        Console.WriteLine("Converting ... " + filename);
                        zplaneFileName = tempsFolder + "\\" + fi.Name + ".wav";
                        tempFile = zplaneFileName;
                        ConvertMp3ToWav(filename, zplaneFileName);
                        var r = new NAudio.Wave.Mp3FileReader(fileStream);
                        totalTime = r.TotalTime;
                        r.Dispose();
                    }

                    else if (filename.ToLower().EndsWith(".flac"))
                    {
                        Console.WriteLine("Converting ..." + filename);
                        zplaneFileName = tempsFolder + "\\" + fi.Name + ".wav";
                        tempFile = zplaneFileName;
                        ConvertFlacToWav(filename, zplaneFileName);
                        var r = new FlacReader(fileStream);
                        totalTime = r.TotalTime;
                        r.Dispose();
                    }
                    else
                    {
                        try
                        {
                            Console.WriteLine("Converting User Media Fundation ..." + filename);
                            zplaneFileName = tempsFolder + "\\" + fi.Name + ".wav";
                            tempFile = zplaneFileName;
                            ConvertOtherToWav(filename, zplaneFileName);
                            var r = new MediaFoundationReader(filename);
                            totalTime = r.TotalTime;
                            r.Dispose();
                        }
                        catch (Exception vexp)
                        {
                            Console.WriteLine("Media Fundation failed as well fuck it ");
                        }
                    }

                }
                catch (Exception exInConvert)
                {
                    try
                    {
                        Console.WriteLine("Error in normal convert trying media fondation");
                        Console.WriteLine("Converting User Media Fundation ..." + filename);
                        zplaneFileName = tempsFolder + "\\" + fi.Name + ".wav";
                        tempFile = zplaneFileName;
                        ConvertOtherToWav(filename, zplaneFileName);
                        var r = new MediaFoundationReader(filename);
                        totalTime = r.TotalTime;
                        r.Dispose();
                    }
                    catch (Exception vexp)
                    {
                        Console.WriteLine("Media Fundation failed as well fuck it ");
                    }
                    

                }

                var processStartInfo = new ProcessStartInfo { FileName = @"barbeatQDemo.exe" };
                p = Process.Start(processStartInfo);
                System.Threading.Thread.Sleep(2000);
                var cap = new BeatAnalyser.ScreenCapture();
                window = p.MainWindowHandle;
                cap.CaptureWindowToFile(p.MainWindowHandle, "test.bmp", ImageFormat.Bmp);
                ScreenCapture.Post(p.Handle, filename);
                RECT rct = new RECT();
                Helper.GetWindowRect(p.MainWindowHandle, out rct);

                var point = new System.Drawing.Point();
                point.X = rct.X + 35;
                point.Y = rct.Y + rct.Height - 35;


                ClickOnPointTool.ClickOnPoint(p.MainWindowHandle, point);
                System.Threading.Thread.Sleep(250);
                Helper.MoveWindow(p.MainWindowHandle, 0, 0, 1736, 503, true);
                Console.WriteLine("Sending file over :" + zplaneFileName);
                
                
                char[] specialChars = { '{', '}', '(', ')', '+','^' };
                foreach (char letter in zplaneFileName)
                {
                    bool _specialCharFound = false;

                    for (int i = 0; i < specialChars.Length; i++)
                    {
                        if (letter == specialChars[i])
                        {
                            _specialCharFound = true;
                            break;
                        }
                    }

                    if (_specialCharFound)
                        SendKeys.SendWait("{" + letter.ToString() + "}");
                    else
                        SendKeys.SendWait(letter.ToString());
                }

                SendKeys.SendWait(@"{ENTER}");

                Console.WriteLine("waiting...");
                System.Threading.Thread.Sleep(15000);




                var bmp = Helper.PrintWindow(p.MainWindowHandle);
                p.Kill();

                System.IO.DirectoryInfo di = new DirectoryInfo(tempsFolder);

                foreach (FileInfo file in di.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception v)
                    {

                    }
                }

                int column = 31;
                var colorList = new List<KeyValuePair<TimeSpan, Color>>();

                for (column = 31; column < 1657; column++) // 1704 (1673)
                {
                    var thic = totalTime.TotalMilliseconds / 1673;
                    var color = bmp.GetPixel(column, 132);
                    colorList.Add(new KeyValuePair<TimeSpan, Color>(new TimeSpan(0, 0, 0, 0, Convert.ToInt32((column - 31) * thic)), color));
                }

                var zplacePois = new List<ZPlanePOI>();
                var currentPois = new ZPlanePOI();
                currentPois.Start = new TimeSpan();
                currentPois.R = colorList[4].Value.R;
                currentPois.G = colorList[4].Value.G;
                currentPois.B = colorList[4].Value.B;
                zplacePois.Add(currentPois);
                foreach (var color in colorList.Skip(3))
                {

                    if ((currentPois.R != color.Value.R || currentPois.G != color.Value.G || currentPois.B != color.Value.B) && (color.Value.G > color.Value.R || color.Value.B > color.Value.R))
                    {
                        if (color.Key.Subtract(currentPois.Start).TotalMilliseconds < 3000)
                        {
                            currentPois.R = color.Value.R;
                            currentPois.G = color.Value.G;
                            currentPois.B = color.Value.B;
                        }
                        else
                        {
                            currentPois.Stop = color.Key;
                            currentPois = new ZPlanePOI();
                            currentPois.Start = color.Key;
                            currentPois.R = color.Value.R;
                            currentPois.G = color.Value.G;
                            currentPois.B = color.Value.B;
                            zplacePois.Add(currentPois);
                        }
                    }
                }
                currentPois.Stop = totalTime;




                if (zplacePois.Count > 2)
                {
                    Save(filename, zplacePois);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Process crashed killing it " + ex.ToString());
                try
                {
                    p.Kill();
                }
                catch (Exception vexp)
                {

                }

                return false;

            }

        }

        private static void Save(string filename, List<ZPlanePOI> zplacePois)
        {
            string json = JsonConvert.SerializeObject(zplacePois.ToArray());

            //write string to file
            System.IO.File.WriteAllText(filename + ".json", json);
        }

        private static void ConvertMp3ToWav(string _inPath_, string _outPath_)
        {
            
            using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(_outPath_, pcm);
                }
            }
        }

        private static void ConvertOtherToWav(string _inPath_, string _outPath_)
        {
            using (var mp3 = new MediaFoundationReader(_inPath_))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(_outPath_, pcm);
                }
            }
        }

        private static void ConvertFlacToWav(string _inPath_, string _outPath_)
        {
            using (var mp3 = new NAudio.Flac.FlacReader(_inPath_))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(_outPath_, pcm);
                }
            }
        }
    }

}
