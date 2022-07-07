using System;
using Un4seen.Bass;
using VisualControler;

namespace LaserDisplay
{
    public class LaserDisplay
    {
        private WaveLib.WaveFormatExtensible waveFormat;
        private ResamplingService Resampler;

        private WaveLib.WaveOutPlayer m_Player;
        private WaveLib.FifoStream m_Fifo = new WaveLib.FifoStream();

        public short w = 1;
        public short fact = -1;

        public bool MixColor { get; set; }

        private dj djSetting = new dj();
        private WaveLib.SpeakerPosition _RighChannel = 0;
        private WaveLib.SpeakerPosition _LeftChannel = 0;
        private WaveLib.SpeakerPosition _GreenChannel = 0;
        private WaveLib.SpeakerPosition _RedChannel = 0;
        private WaveLib.SpeakerPosition _BlueChannel = 0;
        private LaserDisplay pSlave;

        private int _RightFilerSpeaker = 0;
        private int _LeftFilerSpeaker = 0;
        private int _GreenChannelSpeaker = 0;
        private int _RedChannelSpeaker = 0;
        private int _BlueChannelSpeaker = 0;

        public AudienceScanning AudienceScanningEffect = new AudienceScanning();
        public AudienceScanningR AudienceScanningREffect = new AudienceScanningR();
        public BeatSky BeatSkySanning = new BeatSky();
        public StaticSky StaticSkyEffect = new StaticSky();
        public TunnelEffect TunnelEffectScanning = new TunnelEffect();
        public SpinningEffect SpinningEffectScanning = new SpinningEffect();
        public SpinningEffectR SpinningEffectRScanning = new SpinningEffectR();
        public SpinningEffectFull SpinningEffectFullScanning = new SpinningEffectFull();
        private ILaserEffet mCurrentEffect = null;

        public ILaserEffet CurrentEffect
        {
            get
            {
                if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 0)
                {
                    this.mCurrentEffect = null;
                }
                else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 1)
                {
                    this.mCurrentEffect = this.StaticSkyEffect;
                }
                else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 2)
                {
                    this.mCurrentEffect = this.BeatSkySanning;
                }
                else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 3)
                {
                    this.mCurrentEffect = this.AudienceScanningEffect;
                }
                else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 4)
                {
                    this.mCurrentEffect = this.AudienceScanningREffect;
                }
                else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 5)
                {
                    this.mCurrentEffect = this.TunnelEffectScanning;
                }
                else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 6)
                {
                    this.mCurrentEffect = this.SpinningEffectScanning;
                }
                else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 7)
                {
                    this.mCurrentEffect = this.SpinningEffectRScanning;
                }
                else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 8)
                {
                    this.mCurrentEffect = this.SpinningEffectFullScanning;
                }
                return this.mCurrentEffect;
            }
            set
            {
                this.mCurrentEffect = value;
            }
        }

        public double BPM = 80;
        public bool _OnBeat = false;

        public int _DeviceID = -1;

        public WaveLib.SpeakerPosition RighChannel
        {
            get
            {
                return _RighChannel;
            }
        }

        public WaveLib.SpeakerPosition LeftChannel
        {
            get
            {
                return _LeftChannel;
            }
        }

        public LaserDisplay(WaveLib.SpeakerPosition rightChannel, WaveLib.SpeakerPosition leftChannel, WaveLib.SpeakerPosition pRedChannel, WaveLib.SpeakerPosition pBlueChannel, WaveLib.SpeakerPosition pGreenChannel, int device, bool NotRealTimeMotor)
        {
            VisualControler.ServiceExchangeSingleton.Instance.Red = true;
            VisualControler.ServiceExchangeSingleton.Instance.Blue = true;
            VisualControler.ServiceExchangeSingleton.Instance.Green = true;
            MixColor = true;
            VisualControler.ServiceExchangeSingleton.Instance.LaserColorMode = ColorMode.Manual;
            this.CurrentEffect = BeatSkySanning;

            _DeviceID = device;
            _LeftChannel = leftChannel;
            _RighChannel = rightChannel;
            _RedChannel = pRedChannel;
            _BlueChannel = pBlueChannel;
            _GreenChannel = pGreenChannel;

            _GreenChannelSpeaker = SpeakerFillerValue(_GreenChannel);
            _RedChannelSpeaker = SpeakerFillerValue(_RedChannel);
            _BlueChannelSpeaker = SpeakerFillerValue(_BlueChannel);
            _RightFilerSpeaker = SpeakerFillerValue(rightChannel);
            _LeftFilerSpeaker = SpeakerFillerValue(leftChannel);
            Resampler = new ResamplingService();
            Resampler.Filter = ResamplingFilters.Bell;
        }

        private int SpeakerFillerValue(WaveLib.SpeakerPosition pSpeaker)
        {
            if (pSpeaker == WaveLib.SpeakerPosition.SPEAKER_FRONT_LEFT) return 0;
            if (pSpeaker == WaveLib.SpeakerPosition.SPEAKER_FRONT_RIGHT) return 1;
            if (pSpeaker == WaveLib.SpeakerPosition.SPEAKER_LOW_FREQUENCY) return 2;
            if (pSpeaker == WaveLib.SpeakerPosition.SPEAKER_FRONT_CENTER) return 3;
            if (pSpeaker == WaveLib.SpeakerPosition.SPEAKER_BACK_LEFT) return 4;
            if (pSpeaker == WaveLib.SpeakerPosition.SPEAKER_BACK_RIGHT) return 5;
            else return -1;
        }

        private int timebetween = 0;

        private int vBlueSample = 1;
        private int vRedSample = 1;
        private int vGreenSample = 1;

        private void CalcColor(int BeatIteration, bool vOnBeat)
        {
            if (VisualControler.ServiceExchangeSingleton.Instance.LaserColorMode == ColorMode.Hard)
            {
                if (vOnBeat)
                {
                    if (vBlueSample == 1)
                    {
                        if (VisualControler.ServiceExchangeSingleton.Instance.Green)
                        {
                            vBlueSample = 0;
                            vRedSample = 0;
                            vGreenSample = 1;
                        }
                        else if (VisualControler.ServiceExchangeSingleton.Instance.Red)
                        {
                            vBlueSample = 0;
                            vRedSample = 1;
                            vGreenSample = 0;
                        }
                    }
                    else if (vGreenSample == 1)
                    {
                        if (VisualControler.ServiceExchangeSingleton.Instance.Red)
                        {
                            vBlueSample = 0;
                            vRedSample = 1;
                            vGreenSample = 0;
                        }
                        else if (VisualControler.ServiceExchangeSingleton.Instance.Blue)
                        {
                            vBlueSample = 1;
                            vRedSample = 0;
                            vGreenSample = 0;
                        }
                    }
                    else if (vRedSample == 1)
                    {
                        if (VisualControler.ServiceExchangeSingleton.Instance.Blue)
                        {
                            vBlueSample = 1;
                            vRedSample = 0;
                            vGreenSample = 0;
                        }
                        else if (VisualControler.ServiceExchangeSingleton.Instance.Green)
                        {
                            vBlueSample = 0;
                            vRedSample = 0;
                            vGreenSample = 1;
                        }
                    }
                }
            }
            if (VisualControler.ServiceExchangeSingleton.Instance.LaserColorMode == ColorMode.Smooth)
            {
            }
            if (VisualControler.ServiceExchangeSingleton.Instance.LaserColorMode == ColorMode.Manual)
            {
                vBlueSample = VisualControler.ServiceExchangeSingleton.Instance.Blue ? 1 : 0;
                vRedSample = VisualControler.ServiceExchangeSingleton.Instance.Red ? 1 : 0;
                vGreenSample = VisualControler.ServiceExchangeSingleton.Instance.Green ? 1 : 0;
            }
        }

        private void Filler(IntPtr data, int size)
        {
            try
            {
                bool vCurrentOnBeat = _OnBeat;
                if (ServiceExchangeSingleton.Instance.UseBeatTurnOff && _OnBeat) BeatIteration = 5;
                if (ServiceExchangeSingleton.Instance.OnBeatReverse && _OnBeat) BeatIteration = 5;

                short[] Buffer = this.ManualFiller(size);
                byte[] BufferFinal = new byte[size];

                int x = 0;

                CalcColor(BeatIteration, vCurrentOnBeat);

                short[] vTurnOff = new short[BufferFinal.Length / 6];

                if (BeatIteration > 0 && ServiceExchangeSingleton.Instance.UseBeatTurnOff)
                {
                    for (int i = 0; i < vTurnOff.Length; i++)
                    {
                        vTurnOff[i] = short.MinValue;
                    }
                    BeatIteration--;
                }
                else if (ServiceExchangeSingleton.Instance.UseBeatTurnOff)
                {
                    for (int i = 0; i < vTurnOff.Length; i++)
                    {
                        vTurnOff[i] = short.MaxValue;
                    }
                }
                else if (BeatIteration > 0)
                {
                    for (int i = 0; i < vTurnOff.Length; i++)
                    {
                        vTurnOff[i] = short.MaxValue;
                    }
                    BeatIteration--;
                }

                int y = 0;
                {
                    for (int i = 0; i < BufferFinal.Length; i += 12)
                    {
                        var ScannerX = BitConverter.GetBytes(Buffer[x]);
                        x++;
                        var ScannerY = BitConverter.GetBytes(Buffer[x]);
                        x++;

                        var Green = BitConverter.GetBytes(Resample(vGreenSample, y / 6, vTurnOff[y] == short.MaxValue));
                        var Blue = BitConverter.GetBytes(Resample(vBlueSample, y / 6 + 3, vTurnOff[y] == short.MaxValue));

                        BufferFinal[i] = ScannerX[0];
                        BufferFinal[i + 1] = ScannerX[1];

                        BufferFinal[i + 2] = ScannerY[0];
                        BufferFinal[i + 3] = ScannerY[1];

                        BufferFinal[i + 4] = 0;
                        BufferFinal[i + 5] = 0;

                        BufferFinal[i + 6] = 0;
                        BufferFinal[i + 7] = 0;

                        BufferFinal[i + 8] = Green[0]; //green
                        BufferFinal[i + 9] = Green[1]; //green

                        BufferFinal[i + 10] = Blue[0]; //Blue
                        BufferFinal[i + 11] = Blue[1]; //Blue

                        y++;
                    }
                }

                System.Runtime.InteropServices.Marshal.Copy(BufferFinal, 0, data, BufferFinal.Length);
            }
            catch (NotImplementedException d)
            {
                throw d;
            }
            catch (Exception)
            {
            }
        }

        public short Resample(int pFrequance, int Iterator, bool onBeat)
        {
            if (VisualControler.ServiceExchangeSingleton.Instance.LaserPause) return short.MaxValue;
            if (pFrequance == 0) return short.MaxValue;
            if (pFrequance == 0 && onBeat) return short.MaxValue;
            if ((Iterator % pFrequance) == 0 && onBeat) return short.MaxValue;
            return short.MinValue;
        }

        private static int BeatIteration = 0;

        public short[] ManualFiller(int size)
        {
            try
            {
                if (VisualControler.ServiceExchangeSingleton.Instance.AutoChangeEventLaser)
                {
                    if (timebetween <= 0)
                    {
                        timebetween = 25 * VisualControler.ServiceExchangeSingleton.Instance.AutoMixDelay;
                        if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 8)
                        {
                            VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID = 3;
                        }
                        else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID >= 3)
                        {
                            VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID++;
                        }
                    }
                    timebetween--;
                }
                ILaserEffet ef = this.CurrentEffect;
                short[] Buffer = null;

                if (ef != null)
                {
                    Resampler.Size = size;
                    Resampler.Filter = ResamplingFilters.Box;
                    Buffer = this.CurrentEffect.DrawOnLaser(0, 1, 2, size / 3, Resampler, VisualControler.ServiceExchangeSingleton.Instance.LaserPause, BPM, _OnBeat);
                    _OnBeat = false;
                    ef.Transform();
                }
                else
                {
                    Buffer = new short[size / 2];

                    for (int i = 0; i < Buffer.Length; i += 2)
                    {
                        Buffer[i + 0] = 0;
                        Buffer[i + 1] = 0;
                    }
                }
                return Buffer;
            }
            catch (Exception d)
            {
                throw d;
                //System.Windows.Forms.MessageBox.Show(d.ToString());
            }
            return null;
        }

        private static STREAMPROC _myStreamCreate;
        private static byte[] _data = null; // our local buffer

        private int MyFileProc(int handle, IntPtr buffer, int length, IntPtr user)
        {
            Filler(buffer, length);
            return length;
            // implementing the callback for BASS_StreamCreate...
            // here we need to deliver PCM sample data

            // increase the data buffer as needed
            /*    if (_data == null || _data.Length < length)
                    _data = new byte[length];

                int x = 0;
                for (x = 0; x < length; x = x + 12)
                {
                    var ScannerX = BitConverter.GetBytes(0);
                    var ScannerY = BitConverter.GetBytes(short.MaxValue);
                    var Green = BitConverter.GetBytes(short.MaxValue);
                    var Blue = BitConverter.GetBytes(short.MinValue);
                    _data[x] = ScannerX[0];
                    _data[x + 1] = ScannerX[1];

                    _data[x + 2] = ScannerY[0];
                    _data[x + 3] = ScannerY[1];

                    _data[x + 4] = 0;
                    _data[x + 5] = 0;

                    _data[x + 6] = 0;
                    _data[x + 7] = 0;

                    _data[x + 8] = Green[0]; //green
                    _data[x + 9] = Green[1]; //green

                    _data[x + 10] = Blue[0]; //Blue
                    _data[x + 11] = Blue[1]; //Blue
                }*/

            //  Marshal.Copy(_data, 0, buffer, length);
            return length;
        }

        private static int? GetLaserUsbDevice()
        {
            var usbDevice = Bass.BASS_GetDeviceInfos();
            for (int x = 0; x < Bass.BASS_GetDeviceCount(); x++)
            {
                if (Bass.BASS_GetDeviceInfo(x).name.Contains("USB"))
                {
                    return x;
                }
            }

            return null;
        }

        public void Start()
        {
            Stop();
            try
            {
                var usbDevice = GetLaserUsbDevice();
                if (usbDevice != null)
                {
                    Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 250);
                    Bass.BASS_Init(usbDevice.Value, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
                    _myStreamCreate = new STREAMPROC(MyFileProc);

                    int stream = Bass.BASS_StreamCreate(44100, 6, BASSFlag.BASS_DEFAULT, _myStreamCreate, IntPtr.Zero);
                    Bass.BASS_ChannelPlay(stream, false);
                }
            }
            catch (Exception)
            {
                Stop();
            }
        }

        public void Stop()
        {
            /*  if (m_Player != null)
                  try
                  {
                      m_Player.Dispose();
                  }
                  finally
                  {
                      m_Player = null;
                  //}*/
        }
    }

    public enum ColorMode
    {
        Manual,
        Hard,
        Smooth
    }
}