﻿using BeatDetector;
using LightMixer.Model.Fixture;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using UIFrameWork;

namespace LightMixer.Model
{
    public class SharedEffectModel : BaseViewModel
    {
        private double _maxLightIntesityMovingHead = 100;
        private double _maxLightIntesity = 75;
        private double _maxLightFlashIntesity = 75;
        private double _maxBoothIntesity = 100;
        private double _maxBoothFlashIntesity = 100;
        private double _maxSpeed = 1;
        private bool _autoChangeGobo = false;
        private bool _autoChangeProgram = false;
        private bool _autoChangeColorOnBeat = true;
        private int _secondBetweenGoboChange = 10;
        private int _secondBetweenProgramChange = 10;
        private DateTime _lastGoboChange = DateTime.Now;
        private DateTime _lastProgramChange = DateTime.Now;
        private byte _red = 255;
        private byte _green = 255;
        private byte _blue = 255;
        private MovingHeadFixture.Gobo _currentMovingHeadGobo = MovingHeadFixture.Gobo.Open;
        private MovingHeadFixture.Program _currentMovingHeadProgram = MovingHeadFixture.Program.Circle;

        public SharedEffectModel(VirtualDjServer virtualDjServer)
        {
            VirtualDjServer = virtualDjServer;
            if (virtualDjServer != null)
            {
                virtualDjServer.OS2lServerEvent += VirtualDjServer_OS2lServerEvent;
            }
        }

        private void VirtualDjServer_OS2lServerEvent(OS2lEvent os2lEvent)
        {
            IsBeat = true;
        }

        public double MaxBoothFlashIntesity
        {
            get
            {
                return _maxBoothFlashIntesity;
            }
            set
            {
                _maxBoothFlashIntesity = value;
                AsyncOnPropertyChange(o => MaxBoothFlashIntesity);
            }
        }

        public static BeatDetector.BeatDetector BeatDetector { get; set; }

        public double MaxBoothIntesity
        {
            get
            {
                return _maxBoothIntesity;
            }
            set
            {
                _maxBoothIntesity = value;
                AsyncOnPropertyChange(o => this.MaxBoothIntesity);
            }
        }

        public double MaxLightFlashIntesity
        {
            get
            {
                return _maxLightFlashIntesity;
            }
            set
            {
                _maxLightFlashIntesity = value;
                AsyncOnPropertyChange(o => this.MaxLightFlashIntesity);
            }
        }

        public double MaxLightIntesity
        {
            get
            {
                return _maxLightIntesity;
            }
            set
            {
                _maxLightIntesity = value;
                AsyncOnPropertyChange(o => this.MaxLightIntesity);
            }
        }

        public bool AutoChangeColorOnBeat
        {
            get
            {
                return _autoChangeColorOnBeat;
            }
            set
            {
                _autoChangeColorOnBeat = value;
                AsyncOnPropertyChange(o => this.AutoChangeColorOnBeat);
            }
        }

        public double MaxSpeed
        {
            get
            {
                return _maxSpeed;
            }
            set
            {
                _maxSpeed = value;
                AsyncOnPropertyChange(o => this.MaxSpeed);
            }
        }

        public bool AutoChangeGobo
        {
            get
            {
                return _autoChangeGobo;
            }
            set
            {
                _autoChangeGobo = value;
                AsyncOnPropertyChange(o => this.AutoChangeGobo);
            }
        }

        public bool AutoChangeProgram
        {
            get
            {
                return _autoChangeProgram;
            }
            set
            {
                _autoChangeProgram = value;
                AsyncOnPropertyChange(o => this.AutoChangeProgram);
            }
        }

        public int SecondBetweenGoboChange
        {
            get
            {
                return this._secondBetweenGoboChange;
            }
            set
            {
                _secondBetweenGoboChange = value;
                AsyncOnPropertyChange(o => this.SecondBetweenGoboChange);
            }
        }

        public int SecondBetweenProgramChange
        {
            get
            {
                return this._secondBetweenProgramChange;
            }
            set
            {
                _secondBetweenProgramChange = value;
                AsyncOnPropertyChange(o => this.SecondBetweenProgramChange);
            }
        }

        public bool IsBeat
        {
            get
            {
                return this._isBeat;
            }

            set
            {
                _isBeat = value;
                AsyncOnPropertyChange(o => this.IsBeat);
                Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(100);
                    _isBeat = false;
                    AsyncOnPropertyChange(o => this.IsBeat);
                });
            }
        }

        public double MaxLightIntesityMovingHead
        {
            get
            {
                return _maxLightIntesityMovingHead;
            }
            set
            {
                _maxLightIntesityMovingHead = value;
                AsyncOnPropertyChange(o => this.MaxLightIntesityMovingHead);
            }
        }

        private Color TargetColor =  Color.Red;
        private SceneService sceneService;
        private bool _isBeat;

        public void RotateColor()
        {
            Color currentColor = Color.FromArgb(this.Red, this.Green, this.Blue);
            Color newColor = TransitionColorTo(currentColor, TargetColor);

            /* if (CompareColor(currentColor, Colors.AliceBlue))
                 TargetColor = Colors.Red;
             else if (CompareColor(currentColor, Colors.Red))
                 TargetColor = Colors.Yellow;
             else if (CompareColor(currentColor, Colors.Yellow))
                 TargetColor = Colors.Blue;
             else if (CompareColor(currentColor, Colors.Blue))
                 TargetColor = Colors.Beige;
             else if (CompareColor(currentColor, Colors.Beige))
                 TargetColor = Colors.MistyRose;
             else if (CompareColor(currentColor, Colors.MistyRose))
                 TargetColor = Colors.Purple;
             else if (CompareColor(currentColor, Colors.Purple))
                 TargetColor = Colors.AliceBlue;*/

            if (CompareColor(currentColor, Color.Blue))
                TargetColor = Color.Red;
            else if (CompareColor(currentColor, Color.Red))
                TargetColor = Color.Green;
            else if (CompareColor(currentColor, Color.Green))
                TargetColor = Color.Blue;

            this.Red = newColor.R;
            this.Green = newColor.G;
            this.Blue = newColor.B;
        }

        public Color TransitionColorTo(Color current, Color target)
        {
            var beat = Convert.ToByte(BeatDetector?.BeatRepeat ?? 3 * 10);
            byte modifer = beat;
            var newColor = Color.FromArgb(GetSpecterColorByte(current.R, target.R, modifer), GetSpecterColorByte(current.G, target.G, modifer), GetSpecterColorByte(current.B, target.B, modifer));
            
            return newColor;
        }

        public byte GetSpecterColorByte(int current, int target, int modifier)
        {
            if (current == target)
                return (byte)target;

            if (target < current)
            {
                if (current - modifier >= target)
                {
                    return (byte)(current - modifier);
                }
                else
                {
                    return (byte)target;
                }
            }
            else
            {
                if (current + modifier <= target)
                {
                    return (byte)(current + modifier);
                }
                else
                {
                    return (byte)target;
                }
            }
        }

        public bool CompareColor(Color a, Color b)
        {
            return (a.R == b.R &&
                a.G == b.G &&
                a.B == b.B);
        }

        public byte Red
        {
            get
            {
                return _red;
            }
            set
            {
                _red = value;
                this.OnPropertyChanged(() => this.Red);
                this.OnPropertyChanged(() => this.Color);
            }
        }

        public byte Green
        {
            get
            {
                return _green;
            }
            set
            {
                _green = value;
                this.OnPropertyChanged(() => this.Green);
                this.OnPropertyChanged(() => this.Color);
            }
        }

        public byte Blue
        {
            get
            {
                return _blue;
            }
            set
            {
                _blue = value;
                this.OnPropertyChanged(() => this.Blue);
                this.OnPropertyChanged(() => this.Color);
            }
        }

        public Color Color
        {
            get
            {
                return Color.FromArgb(Red, Green, Blue);
            }
        }

        public MovingHeadFixture.Gobo CurrentMovingHeadGobo
        {
            get
            {
                if (this.AutoChangeGobo &&
                    DateTime.Now.Subtract(this._lastGoboChange).Seconds > this.SecondBetweenGoboChange)
                {
                    this._lastGoboChange = DateTime.Now;
                    this.ChangeGobo();
                    this.OnPropertyChanged(() => this.CurrentMovingHeadGobo);
                }

                return this._currentMovingHeadGobo;
            }
            set
            {
                _currentMovingHeadGobo = value;
                this.OnPropertyChanged(() => this.CurrentMovingHeadGobo);
            }
        }

        private void ChangeGobo()
        {
            if (this._currentMovingHeadGobo == MovingHeadFixture.Gobo.Floyer)
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.FloyerSpiral;
            if (this._currentMovingHeadGobo == MovingHeadFixture.Gobo.FloyerSpiral)
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.HearExplosion;
            if (this._currentMovingHeadGobo == MovingHeadFixture.Gobo.HearExplosion)
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.Open;
            if (this._currentMovingHeadGobo == MovingHeadFixture.Gobo.Open)
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.SpinningArrow;
            if (this._currentMovingHeadGobo == MovingHeadFixture.Gobo.SpinningArrow)
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.Spiral;
            if (this._currentMovingHeadGobo == MovingHeadFixture.Gobo.Spiral)
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.SquareSpiral;
            if (this._currentMovingHeadGobo == MovingHeadFixture.Gobo.SquareSpiral)
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.Star;
            if (this._currentMovingHeadGobo == MovingHeadFixture.Gobo.Star)
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.TriangleSpiral;
            if (this._currentMovingHeadGobo == MovingHeadFixture.Gobo.TriangleSpiral)
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.TriangleSpiral2;
            if (this._currentMovingHeadGobo == MovingHeadFixture.Gobo.TriangleSpiral2)
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.Floyer;
            else
                this._currentMovingHeadGobo = MovingHeadFixture.Gobo.Floyer;
        }

        public DateTime IsCurrentMovingHeadProgramDirty = DateTime.Now;

        public MovingHeadFixture.Program CurrentMovingHeadProgram
        {
            get
            {
                if (this.AutoChangeProgram &&
                    DateTime.Now.Subtract(this._lastProgramChange).Seconds > this.SecondBetweenProgramChange)
                {
                    this.ChangeProgram();
                    this._lastProgramChange = DateTime.Now;
                    this.OnPropertyChanged(() => this.CurrentMovingHeadProgram);
                }

                return this._currentMovingHeadProgram;
            }
            set
            {
                if (_currentMovingHeadProgram != value)
                {
                    IsCurrentMovingHeadProgramDirty = DateTime.Now;
                    _currentMovingHeadProgram = value;
                    this.OnPropertyChanged(() => this.CurrentMovingHeadProgram);
                }
            }
        }

        private void ChangeProgram()
        {
            if (this._currentMovingHeadProgram == MovingHeadFixture.Program.Circle)
                this._currentMovingHeadProgram = MovingHeadFixture.Program.Auto1;
            else if (this._currentMovingHeadProgram == MovingHeadFixture.Program.Auto1)
                this._currentMovingHeadProgram = MovingHeadFixture.Program.Auto2;
            else if (this._currentMovingHeadProgram == MovingHeadFixture.Program.Auto2)
                this._currentMovingHeadProgram = MovingHeadFixture.Program.Auto3;
            else if (this._currentMovingHeadProgram == MovingHeadFixture.Program.Auto3)
                this._currentMovingHeadProgram = MovingHeadFixture.Program.Auto4;
            else if (this._currentMovingHeadProgram == MovingHeadFixture.Program.Auto4)
                this._currentMovingHeadProgram = MovingHeadFixture.Program.Auto5;
            else if (this._currentMovingHeadProgram == MovingHeadFixture.Program.Auto5)
                this._currentMovingHeadProgram = MovingHeadFixture.Program.Auto6;
            else if (this._currentMovingHeadProgram == MovingHeadFixture.Program.Auto6)
                this._currentMovingHeadProgram = MovingHeadFixture.Program.Auto7;
            else if (this._currentMovingHeadProgram == MovingHeadFixture.Program.Auto7)
                this._currentMovingHeadProgram = MovingHeadFixture.Program.Auto8;
            else if (this._currentMovingHeadProgram == MovingHeadFixture.Program.Auto8)
                this._currentMovingHeadProgram = MovingHeadFixture.Program.Auto1;
            else
                this._currentMovingHeadProgram = MovingHeadFixture.Program.Circle;
        }

        public static ObservableCollection<MovingHeadFixture.Program> MovingHeadProgram
        {
            get
            {
                ObservableCollection<MovingHeadFixture.Program> list = new ObservableCollection<MovingHeadFixture.Program>();
                list.Add(MovingHeadFixture.Program.CodeDisable);
                list.Add(MovingHeadFixture.Program.Circle);
                list.Add(MovingHeadFixture.Program.Balancing1);
                list.Add(MovingHeadFixture.Program.DiscoBall);
                list.Add(MovingHeadFixture.Program.DiscoBallStatic);
                list.Add(MovingHeadFixture.Program.DJ);
                list.Add(MovingHeadFixture.Program.Auto1);
                list.Add(MovingHeadFixture.Program.Auto2);
                list.Add(MovingHeadFixture.Program.Auto3);
                list.Add(MovingHeadFixture.Program.Auto4);
                list.Add(MovingHeadFixture.Program.Auto5);
                list.Add(MovingHeadFixture.Program.Auto6);
                list.Add(MovingHeadFixture.Program.Auto7);
                list.Add(MovingHeadFixture.Program.Auto8);
                list.Add(MovingHeadFixture.Program.Disable);
                list.Add(MovingHeadFixture.Program.SoundAuto1);
                list.Add(MovingHeadFixture.Program.SoundAuto2);
                list.Add(MovingHeadFixture.Program.SoundAuto3);
                list.Add(MovingHeadFixture.Program.SoundAuto4);
                list.Add(MovingHeadFixture.Program.SoundAuto5);
                list.Add(MovingHeadFixture.Program.SoundAuto6);
                list.Add(MovingHeadFixture.Program.SoundAuto7);
                list.Add(MovingHeadFixture.Program.SoundAuto8);
                list.Add(MovingHeadFixture.Program.Test);
                return list;
            }
        }

        public static ObservableCollection<MovingHeadFixture.Gobo> MovingHeadGobo
        {
            get
            {
                ObservableCollection<MovingHeadFixture.Gobo> list = new ObservableCollection<MovingHeadFixture.Gobo>();
                list.Add(MovingHeadFixture.Gobo.Floyer);
                list.Add(MovingHeadFixture.Gobo.FloyerSpiral);
                list.Add(MovingHeadFixture.Gobo.HearExplosion);
                list.Add(MovingHeadFixture.Gobo.Open);
                list.Add(MovingHeadFixture.Gobo.SpinningArrow);
                list.Add(MovingHeadFixture.Gobo.Spiral);
                list.Add(MovingHeadFixture.Gobo.SquareSpiral);
                list.Add(MovingHeadFixture.Gobo.Star);
                list.Add(MovingHeadFixture.Gobo.TriangleSpiral);
                list.Add(MovingHeadFixture.Gobo.TriangleSpiral2);

                return list;
            }
        }

        public VirtualDjServer VirtualDjServer { get; }
    }
}