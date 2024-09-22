﻿using BeatDetector;
using LightMixer;
using LightMixer.Model.Fixture;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LightMixerStandard.Model.Fixture.Laser
{

    public class Laser : FixtureBase
    {
        private HeliosController heliosController;
        private LaserEffect selectedEffect;
        private CancellationTokenSource currentEffectTokenSource;
        private bool loop;
        DateTime startedLoop = DateTime.Now;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<LaserEffect> Effects { get; private set; } = new ObservableCollection<LaserEffect>();


        public override void ProcessNotification()
        {
            var ev = CurrentVDJEvent;

            var filename = ev.VDJSong.FilePath + ".ild";
            var name = ev.FileName;
            //here we process vdjupdate event
            LightMixerBootStrap.Dispatcher.Invoke(() =>
            {
                var existingEffect = Effects.FirstOrDefault(o => o.FileName == filename);
                if (existingEffect == null)
                {
                    existingEffect = LaserEffect.LoadFromAsync(LaserEffectMood.None, filename, name);
                    Effects.Add(existingEffect);
                }
                if (selectedEffect != existingEffect && existingEffect.Points?.Any() == true)
                {
                    SelectedEffect = existingEffect;
                }
                else if (selectedEffect != existingEffect && selectedEffect.Stretch)
                {
                    SelectedEffect = Effects.Single(o => o.Name == "Done");
                }
            });

        }


        public bool Loop
        {
            get => loop;
            set
            {
                if (value != loop)
                {
                    startedLoop = DateTime.Now;
                    loop = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Loop)));
                }
            }
        }

        public LaserEffect SelectedEffect
        {
            get => selectedEffect;
            set
            {

                if (!LaserOn)
                {
                    selectedEffect = Effects.Skip(1).First();
                }
                else if (selectedEffect != value)
                {
                    selectedEffect = value;
                    RunSelectedEffect();
                    if (Effects.Skip(1).First() == value || Effects.First() == value)
                    {
                        Loop = false;
                    }
                }
                LightMixerBootStrap.Dispatcher.Invoke(() =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedEffect)));
                });
            }
        }

        public void SetEffectExternal(LaserEffect effect)
        {
            if (Effects.Skip(1).First() != SelectedEffect || effect == Effects.First())
            {
                this.SelectedEffect = effect;
            }
        }

        public void SetEffectExternalRandomMood(LaserEffectMood mood, bool loop)
        {
            this.Loop = loop;
            if (mood == LaserEffectMood.None)
            {
                startedLoop = DateTime.Now;
                this.SelectedEffect = Effects.First();
            }
            else if (SelectedEffect.Mood != mood && SelectedEffect != Effects.Skip(1).First())
            {
                startedLoop = DateTime.Now;
                Shuffle(mood);
            }
        }

        private void Shuffle(LaserEffectMood mood)
        {
            var moodEffect = Effects.Where(o => o.Mood == mood).ToArray();
            Random random = new Random();
            this.SelectedEffect = moodEffect[random.Next(moodEffect.Count())];
        }

        public Laser()
        {
            LoadDefault();
            Task.Run(() => Start());
        }

        private void Start()
        {

            Connect();
            RunSelectedEffect();
        }

        private void RunSelectedEffect()
        {
            var tokensource = currentEffectTokenSource;
            if (tokensource != null && !tokensource.IsCancellationRequested)
            {
                tokensource.Cancel();
                currentEffectTokenSource = null;
            }
            currentEffectTokenSource = new CancellationTokenSource();
            Task.Run(() => RenderIlda(SelectedEffect, currentEffectTokenSource.Token), currentEffectTokenSource.Token);
        }

        private void RenderIlda(LaserEffect effect, CancellationToken token)
        {
            HeliosPoint[][] frames = effect.Points;
            if (frames?.Any() == false)
                return;
            int numberOfDevices = 1;
            int deviceId = 0;
            var now = DateTime.Now;

            int framepersecond = 60;

            Console.WriteLine("\nSending a test animation to each DAC...");
            int frameNumber = 0;
            while (frameNumber <= frames.Count()-1 || effect.Stretch)
            //for (frameNumber = 0; frameNumber < frames.Count(); frameNumber++)
            {
                var currentEffect = CurrentVDJEvent;
                if (token.IsCancellationRequested)
                {
                    return;
                }
                try
                {
                    // Wait for ready status
                    bool isReady = false;
                    int k;
                    for (k = 0; k < 50; k++)
                    {
                        if (heliosController.GetStatus(deviceId))
                        {
                            isReady = true;
                            break;
                        }
                    }
                    if (isReady)
                    {
                        var elapsed = DateTime.Now.Subtract(now);
                        if (effect.Stretch)
                        {
                            elapsed = currentEffect.ExtrapoledElapsedBpmAdjusted;
                        }
                                                
                        var currentFrame = elapsed.TotalMilliseconds / (1000 / framepersecond);
                        if (frameNumber > currentFrame)
                        {
                            frameNumber = Convert.ToInt32(currentFrame);
                        }
                        if (frameNumber < currentFrame)
                        {
                            frameNumber = Convert.ToInt32(currentFrame);
                            if (frameNumber >= frames.Count() -1 )
                            {
                                frameNumber = frames.Count() - 1;
                            }
                        }
                        if (heliosController.GetStatus(deviceId))
                        {
                            if (frameNumber >= frames.Count() - 1 || frameNumber < 0 )
                            {
                                heliosController.WriteFrame(deviceId, 25000, new HeliosPoint[1]);
                            }
                            else 
                            {
                                heliosController.WriteFrame(deviceId, 25000, frames[frameNumber]);
                            }
                        }
                        frameNumber++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failure during writing of laser frame to Helios DAC: " + ex.Message);
                }
            }

            var elapsedForcurrentLoop = DateTime.Now.Subtract(startedLoop).TotalSeconds;
            if (selectedEffect != Effects.First() && selectedEffect != Effects.Skip(1).First())
            {
                if (elapsedForcurrentLoop < 10 || (Loop && SelectedEffect.Mood != LaserEffectMood.None))
                //if ((Loop && SelectedEffect.Mood != LaserEffectMood.None))
                {
                    Shuffle(SelectedEffect.Mood);
                    RunSelectedEffect();
                }
                else
                {
                    this.SelectedEffect = Effects.Skip(1).First();
                }
            }
        }

        

        private void LoadDefault()
        {
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.None, "empty.ild", "Empty"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.None, "empty.ild", "Done"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.None, "example1114.ild", "Test"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Hight, "SpinningHexa.ild", "SpinningHexa"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Mid, "Sky.ild", "Sky"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Hight, "ShrinkingSky.ild", "Shrinking Sky"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Low, "SkiDot.ild", "Ski Dot"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Mid, "SpiningSmallLines.ild", "Spinning SmallLines"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Low, "5DotMoving.ild", "5 Dot Moving"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Hight, "ScalingCircle.ild", "Scaling Circle"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Low, "DancingDot.ild", "Dancing Dot"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Hight, "HexaGone.ild", "Hexa Gone"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Hight, "4MulticolorCircle.ild", "4 MultiColor Circle"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Mid, "ScanningParallel.ild", "Scanning Parallel"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Low, "4points.ild", "4 Points"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Mid, "4Circle.ild", "4 Circle"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Mid, "zigzagReverse.ild", "ZigZag Reverse"));
            Effects.Add(LaserEffect.LoadFrom(LaserEffectMood.Mid, "zigzag.ild", "Zig Zag"));

            SelectedEffect = Effects.FirstOrDefault();
        }

        private void Connect()
        {
            heliosController = new HeliosController();
            int numberOfDevices = 0;
            while (numberOfDevices == 0)
            {
                try
                {
                    numberOfDevices = heliosController.OpenDevices();

                    Console.WriteLine($"Found {numberOfDevices} Helios DACs:");
                    for (int deviceId = 0; deviceId < numberOfDevices; deviceId++)
                    {
                        Console.WriteLine(heliosController.GetName(deviceId));
                    }
                }
                catch (Exception ex)
                {
                    numberOfDevices = 0;
                    Console.WriteLine("Failure during detecting and opening of Helios DACs: " + ex.Message);
                    Thread.Sleep(1000);
                }
            }

        }
        public override int DmxLenght => 0;

        public bool LaserOn { get; internal set; }

        public override byte?[] Render()
        {

            return new byte?[0];
        }
    }
}