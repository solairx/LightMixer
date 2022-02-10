using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace LightMixer.Model.Fixture
{
    public class WledServer
    {
        private HttpClient _httpClient = new HttpClient();
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
                        bri= 100,
                        sx = 128,
                        ix = 128,
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
                        bri= 100,
                        sx = 128,
                        ix = 128,
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
                        bri= 100,
                        sx = 128,
                        ix = 128,
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
        }

        public void Render()
        {
            var jsonstr = JsonConvert.SerializeObject(State, Formatting.Indented);
            if (jsonstr != lastQuery)
            {
                lastQuery = jsonstr;
                Task.Run(()=> _httpClient.PostAsync("http://" + ip + "/json/state", new StringContent(jsonstr, System.Text.Encoding.UTF8, "application/json")).Result);
            }

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

        public WledFixture(WledServer wledServer, WledServer.Seg seg)
        {
            client = new HttpClient();
            this.wledServer = wledServer;
            this.seg = seg;
        }

        public override WledServer HttpMulticastRenderer => wledServer;

        public override int DmxLenght => 1;

        public override bool IsRenderOnDmx => false;

        public override byte?[] Render()
        {
            this.SetOn();
            return null;
        }

        public void SetOn()
        {
            isDitry = false;
            try
            {
                var colorNegative = Color.FromArgb(Color.FromArgb(RedValue,GreenValue,BlueValue).ToArgb() ^ 0xffffff);
                var colorTert = Color.FromArgb(Color.FromArgb(RedValue, GreenValue, BlueValue).ToArgb()   ^ 0x777777);
                seg.on = this.RedValue + this.GreenValue + this.BlueValue > 0;
                //seg.fx = (int)WledEffect.FX_MODE_STATIC;
                seg.fx = (int)WledEffect.FX_MODE_CHASE_COLOR;
                seg.col[0][0] = this.RedValue;
                seg.col[0][1] = this.GreenValue;
                seg.col[0][2] = this.BlueValue;
                seg.col[0][3] = this.WhiteValue;

                seg.col[1][0] = colorNegative.R;
                seg.col[1][1] = colorNegative.G;
                seg.col[1][2] = colorNegative.B;

                seg.col[2][0] = colorTert.R;
                seg.col[2][1] = colorTert.G;
                seg.col[2][2] = colorTert.B;

            }
            catch (Exception v)
            {
            }


        }

    }
}
