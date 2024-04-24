using LightMixer.Model.Fixture;
using System.Collections.ObjectModel;

namespace LightMixerStandard.Model.Fixture.Laser
{

    public class Laser : FixtureBase
    {
        private HeliosController heliosController;
        private LaserEffect selectedEffect;
        private CancellationTokenSource currentEffectTokenSource;

        public ObservableCollection<LaserEffect> Effects { get; private set; } = new ObservableCollection<LaserEffect>();

        public LaserEffect SelectedEffect
        {
            get => selectedEffect;
            set
            {
                if (selectedEffect != value)
                {
                    selectedEffect = value;
                    RunSelectedEffect();
                }
            }
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
            Task.Run(() => RenderIlda(SelectedEffect.Points, currentEffectTokenSource.Token), currentEffectTokenSource.Token);
        }

        private void RenderIlda(HeliosPoint[][] frames, CancellationToken token)
        {
            int numberOfDevices = 1;
            int deviceId = 0;
            DateTime now = DateTime.Now;
            var elapsed = DateTime.Now.Subtract(now);
            int framepersecond = 60;

            Console.WriteLine("\nSending a test animation to each DAC...");
            for (int j = 0; j < frames.Count(); j++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                try
                {
                    // Wait for ready status
                    bool isReady = false;
                    for (int k = 0; k < 50; k++)
                    {
                        if (heliosController.GetStatus(deviceId))
                        {
                            isReady = true;
                            break;
                        }
                    }
                    // Send the next frame if received a ready signal
                    if (isReady)
                    {
                        elapsed = DateTime.Now.Subtract(now);
                        var currentFrame = elapsed.TotalMilliseconds / (1000 / framepersecond);
                        if (j % 60 == 0)
                        {
                            Console.WriteLine(j + " " + DateTime.Now.Second + " " + currentFrame);
                        }
                        if (j > currentFrame)
                        {
                            j = Convert.ToInt32(currentFrame);
                        }
                        if (j < currentFrame)
                        {
                            j = Convert.ToInt32(currentFrame);
                            if (j >= frames.Count())
                            {
                                j = frames.Count() - 1;
                            }
                        }
                        heliosController.WriteFrame(deviceId, 25000, frames[j++]);
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failure during writing of laser frame to Helios DAC: " + ex.Message);
                }
            }

            this.SelectedEffect = Effects.First();
        }

        private void LoadDefault()
        {
            Effects.Add(LaserEffect.LoadFrom("empty.ild", "Empty"));
            Effects.Add(LaserEffect.LoadFrom("example1114.ild", "Test"));
            Effects.Add(LaserEffect.LoadFrom("SpinningHexa.ild", "SpinningHexa"));
            Effects.Add(LaserEffect.LoadFrom("Sky.ild", "Sky"));
            Effects.Add(LaserEffect.LoadFrom("ShrinkingSky.ild", "Shrinking Sky"));
            Effects.Add(LaserEffect.LoadFrom("SkiDot.ild", "Ski Dot"));
            Effects.Add(LaserEffect.LoadFrom("3flashingDot.ild", "3 Flashing Dot"));
            Effects.Add(LaserEffect.LoadFrom("SpiningSmallLines.ild", "Spinning SmallLines"));
            Effects.Add(LaserEffect.LoadFrom("5DotMoving.ild", "5 Dot Moving"));
            Effects.Add(LaserEffect.LoadFrom("ScalingCircle.ild", "Scaling Circle"));
            Effects.Add(LaserEffect.LoadFrom("DancingDot.ild", "Dancing Dot"));
            Effects.Add(LaserEffect.LoadFrom("HexaGone.ild", "Hexa Gone"));
            Effects.Add(LaserEffect.LoadFrom("4MulticolorCircle.ild", "4 MultiColor Circle"));
            Effects.Add(LaserEffect.LoadFrom("ScanningParallel.ild", "Scanning Parallel"));
            Effects.Add(LaserEffect.LoadFrom("4points.ild", "4 Points"));
            Effects.Add(LaserEffect.LoadFrom("4Circle.ild", "4 Circle"));
            Effects.Add(LaserEffect.LoadFrom("zigzagReverse.ild", "ZigZag Reverse"));
            Effects.Add(LaserEffect.LoadFrom("zigzag.ild", "Zig Zag"));
            

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

        public override byte?[] Render()
        {

            return new byte?[0];
        }
    }
}