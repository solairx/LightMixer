using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LightMixer.Model.Fixture
{
    public class WledServer
    {
        private HttpClient _httpClient = HttpClientFactory.Create();
        private readonly string ip;
        private string lastQuery = "";
        public WledJson State { get; }

        public class WledJson
        {
            public bool on { get; set; }
            public int bri { get; set; }
            public int transition { get; set; }
            public int ps { get; set; }
            public int pl { get; set; }
            public int lor { get; set; }
            public int mainseg { get; set; }
            public Seg[] seg { get; set; }
        }

        public class Seg
        {
            public int id { get; set; }
            public bool on { get; set; }
            public int bri { get; set; }
            public int[][] col { get; set; }
            public int fx { get; set; }
            public int sx { get; set; }
            public int ix { get; set; }
            public int pal { get; set; }
            public bool sel { get; set; }
        }

        public WledServer(string ip)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            State = new WledJson
            {
                on = true,
                bri = 255,
                pl = -1,
                ps = -1,
                lor = 0,
                seg = new Seg[]
                {
                    new Seg{
                        id = 0,
                        fx = 1,
                        on = true,
                        sx = 128,
                        ix = 128,
                        bri= 255,
                        col = new int[][]
                        {
                            new int[] { 100, 100, 100, 0},
                            new int[] { 100, 100, 100, 0},
                            new int[] { 100, 100, 100, 0},
                        }
                    },
                    new Seg{
                        id = 1,
                        fx = 1,
                        on = true,
                        sx = 128,
                        ix = 128,
                        bri= 255,
                        col = new int[][]
                        {
                            new int[] { 100, 100, 100, 0},
                            new int[] { 100, 100, 100, 0},
                            new int[] { 100, 100, 100, 0},
                        }
                    },
                    new Seg{
                        id = 2,
                        fx = 1,
                        on = true,
                        sx = 128,
                        ix = 128,
                        bri= 255,
                        col = new int[][]
                        {
                            new int[] { 100, 100, 100, 0},
                            new int[] { 100, 100, 100, 0},
                            new int[] { 100, 100, 100, 0},
                        }
                    },
                    new Seg{
                        id = 3,
                        on = true,
                        fx = 1,
                        sx = 128,
                        ix = 128,
                        bri= 255,
                        col = new int[][]
                        {
                            new int[] { 100, 100, 100, 0},
                            new int[] { 100, 100, 100, 0},
                            new int[] { 100, 100, 100, 0},
                        }
                    }
                }
            };
            this.ip = ip;
            PreparatedQuery = JsonConvert.SerializeObject(State, Formatting.Indented);

            Task.Run(() => {
                while (true)
                {
                    try
                    {
                        if (PreparatedQuery != lastQuery)
                        {
                            lastQuery = PreparatedQuery;
                            GetResult(PreparatedQuery);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Thread.Sleep(5);
                }
            });
        }

        public CancellationTokenSource cts = new CancellationTokenSource();

        public string PreparatedQuery = string.Empty;

        public void Render()
        {
            PreparatedQuery = JsonConvert.SerializeObject(State, Formatting.Indented);
        }

        private HttpResponseMessage GetResult(string jsonstr)
        {
            var res = _httpClient.PostAsync("http://" + ip + "/json/state", new StringContent(jsonstr, System.Text.Encoding.UTF8, "application/json"), cts.Token).Result;
            return res;
        }
    }

    public class WledFixture : RgbFixture
    {
        private readonly string ip = "";
        private readonly int segment;
        private readonly WledServer wledServer;
        private readonly WledServer.Seg seg;
        private HttpClient client;
        private string lastQuery = "";

        public event CurrentEffectChangedEventHandler CurrentEffectChanged;

        public delegate void CurrentEffectChangedEventHandler();

        public WledFixture(string serverIp)
        {
            IsStaticOnly = true;
            client = HttpClientFactory.Create();
            client.Timeout = TimeSpan.FromSeconds(10);
            this.wledServer = new WledServer(serverIp);
            this.seg = this.wledServer.State.seg[0];
        }

        public WledFixture(WledServer wledServer, WledServer.Seg seg)
        {
            client = HttpClientFactory.Create();
            client.Timeout = TimeSpan.FromSeconds(10);
            this.wledServer = wledServer;
            this.seg = seg;
        }

        public override WledServer HttpMulticastRenderer => wledServer;

        public override int DmxLenght => 1;

        public override bool IsRenderOnDmx => false;

        private static WLedEffectI CurrentWledEffect;
        private static DateTime lastWledEffectChanged = DateTime.Now;
        private WledEffectCategory wledEffectCategory = WledEffectCategory.High;

        public WledEffectCategory WledEffectCategory
        {
            get => wledEffectCategory;
            set 
            {
                if (wledEffectCategory != value)
                {
                    wledEffectCategory = value;
                    CurrentEffectChanged?.Invoke();
                }
            }
        }

        public static IEnumerable<WledEffectCategory> WledEffectCategoryList => Enum.GetValues(typeof(WledEffectCategory))
            .Cast<WledEffectCategory>();
        public override byte?[] Render()
        {
            this.SetOn();
            return null;
        }

        public bool IsStaticOnly { get; set; }

        public void SetOn()
        {
            isDitry = false;
            try
            {
                seg.on = this.RedValue + this.GreenValue + this.BlueValue > 0;
                if (WledEffectCategory == WledEffectCategory.off)
                {
                    seg.on = false;
                }

                if (CurrentWledEffect == null || DateTime.Now.Subtract(lastWledEffectChanged).TotalSeconds > 5 || CurrentWledEffect.Category != WledEffectCategory)
                {
                    var allAvailableEffect = WledEffect2.EffectList
                        .Where(o => o.Category == WledEffectCategory);

                    if (!seg.on || CurrentWledEffect?.Category != WledEffectCategory)
                    {
                        CurrentWledEffect = allAvailableEffect
                            .SkipWhile(o => o != CurrentWledEffect)
                            .Skip(1)
                            .FirstOrDefault();
                    }

                    if (CurrentWledEffect == null)
                    {
                        CurrentWledEffect = allAvailableEffect.First();
                    }

                    lastWledEffectChanged = DateTime.Now;
                }

                seg.col[0][0] = this.RedValue;
                seg.col[0][1] = this.GreenValue;
                seg.col[0][2] = this.BlueValue;
                seg.col[0][3] = this.WhiteValue;

                seg.col[1][0] = this.Red2Value;
                seg.col[1][1] = this.Green2Value;
                seg.col[1][2] = this.Blue2Value;

                seg.col[2][0] = this.Red3Value;
                seg.col[2][1] = this.Green3Value;
                seg.col[2][2] = this.Blue3Value;
                if (this.IsStaticOnly)
                {
                    CurrentWledEffect = WledEffect2.EffectList.Single(e => e.ID == 0);
                }
                CurrentWledEffect?.Apply(seg);
            }
            catch (Exception)
            {
            }
        }
    }
}