using LightMixer.Model.Fixture;
using System.Collections.Generic;
using LightMixer.Model;
using Microsoft.Practices.Unity;
using LightMixer;

public class SceneService
{
    public const string indoorSceneName = "indoor";
    public const string outDoorSceneName = "outdoor";
    public const string basementZoneName = "basement";
    public const string djboothZoneName = "djbooth";
    public const string poolZoneName = "pool";
    private readonly BeatDetector.BeatDetector beatDetector;
    public List<Scene> Scenes { get; set; } = new List<Scene>();

    public SceneService(BeatDetector.BeatDetector beatDetector)
    {
        this.beatDetector = beatDetector;
        Build();
    }

    private void Build()
    {
        RgbFixture fixtureLed3 = new RgbFixture(0);
        RgbFixture fixtureLed4 = new RgbFixture(3);
        RgbFixture fixtureLed5 = new RgbFixture(6);
        RgbFixture fixtureLed6 = new RgbFixture(9);
        RgbFixture fixtureLed1 = new RgbFixture(12);
        RgbFixture fixtureLed2 = new RgbFixture(15);
        RgbFixture fixtureLed7 = new RgbFixture(18);
        RgbFixture fixtureLed8 = new RgbFixture(21);

        RgbFixture bootDjLed1 = new RgbFixture(24);
        RgbFixture bootDjLed2 = new RgbFixture(27);
        RgbFixture bootDjLed3 = new RgbFixture(30);
        RgbFixture bootDjLed4 = new RgbFixture(33);
        RgbFixture bootDjLed5 = new RgbFixture(36);
        RgbFixture bootDjLed6 = new RgbFixture(39);

        var haRgb1 = new TasmotaRGWFixture("192.168.1.46", false);
        var haRgb2 = new TasmotaRGWFixture("192.168.1.31", false);
        var haRgb3 = new TasmotaRGWFixture("192.168.1.6", false);
        var haRgb4 = new TasmotaRGWFixture("192.168.1.3", false);
        var haRgb5 = new TasmotaRGWFixture("192.168.1.37", false);
        var haRgb6 = new TasmotaRGWFixture("192.168.1.11", false);
        var haRgb7 = new TasmotaRGWFixture("192.168.1.51", false);
        var haRgbDM = new ShellyDimmerFixture("light.dimmer_plafond_ss", true);

        WledServer djBoothWled = new WledServer("192.168.1.9");
        var wledBooth1 = new WledFixture(djBoothWled, djBoothWled.State.seg[0]);
        var wledBooth2 = new WledFixture(djBoothWled, djBoothWled.State.seg[1]);
        var wledBooth3 = new WledFixture(djBoothWled, djBoothWled.State.seg[2]);
        var wledBooth4 = new WledFixture(djBoothWled, djBoothWled.State.seg[3]);

        FixtureGroup movingHeadGroupFloor = new FixtureGroup();
        var djBoothfourPov1 = new List<PointOfInterest>
        {
            new PointOfInterest{  Location = PointOfInterestLocation.DJ, Tilt = 7500, Pan = 35000},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBall, Tilt = 3000, Tilt2=2000, Pan = 5750, Pan2 = 4000},
            new PointOfInterest{  Location = PointOfInterestLocation.Circle, Tilt = 0, Tilt2=65535, Pan = 65535, Pan2 = 1}
        };

        var djBoothfourPov2 = new List<PointOfInterest>
        {
            new PointOfInterest{  Location = PointOfInterestLocation.DJ, Tilt = 7500, Pan = 35000},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBall, Tilt = 3000, Tilt2=2000, Pan = 6250, Pan2 = 4500},
            new PointOfInterest{  Location = PointOfInterestLocation.Circle, Tilt = 0, Tilt2=65535, Pan = 65535, Pan2 = 1}
        };

        var djBoothfourPov3 = new List<PointOfInterest>
        {
         new PointOfInterest{  Location = PointOfInterestLocation.DJ, Tilt = 7500, Pan = 35000},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBall, Tilt = 3000, Tilt2=2000, Pan = 6250, Pan2 = 4500},
            new PointOfInterest{  Location = PointOfInterestLocation.Circle, Tilt = 0, Tilt2=65535, Pan = 65535, Pan2 = 1}
        };
        var djBoothfourPov4 = new List<PointOfInterest>
        {
            new PointOfInterest{  Location = PointOfInterestLocation.DJ, Tilt = 7500, Pan = 35000},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBall, Tilt = 2250, Tilt2=500, Pan = 3000, Pan2 = 500},
            new PointOfInterest{  Location = PointOfInterestLocation.Circle, Tilt = 0, Tilt2=65535, Pan = 65535, Pan2 = 1}
        };
        var fourHead1 = new RgbwMovingHeadMasterFixture(349, djBoothfourPov1, 0);
        var fourHead2 = new RgbwMovingHeadSlaveFixture(360, djBoothfourPov2, 0.25, fourHead1);
        var fourHead3 = new RgbwMovingHeadSlaveFixture(369, djBoothfourPov3, .5, fourHead1);
        var fourHead4 = new RgbwMovingHeadSlaveFixture(378, djBoothfourPov4, .75, fourHead1);
        movingHeadGroupFloor.FixtureInGroup.Add(fourHead1);
        movingHeadGroupFloor.FixtureInGroup.Add(fourHead2);
        movingHeadGroupFloor.FixtureInGroup.Add(fourHead3);
        movingHeadGroupFloor.FixtureInGroup.Add(fourHead4);

        FixtureGroup movingHeadGroupBooth = new FixtureGroup();
        var djBoothPov = new List<PointOfInterest>
        {
            new PointOfInterest{  Location = PointOfInterestLocation.DJ, Tilt = 25000, Pan = 2000},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBall, Tilt = 3000, Tilt2=2000, Pan = 500, Pan2 = 6000},
            new PointOfInterest{  Location = PointOfInterestLocation.Circle, Tilt = 0, Tilt2=65535, Pan = 65535, Pan2 = 1}
        };
        var movingHeadMasterDJ = new MovingHeadFixture(399, djBoothPov);
        movingHeadGroupBooth.FixtureInGroup.Add(movingHeadMasterDJ);
        movingHeadGroupBooth.FixtureInGroup.Add(new MovingHeadFixture(299, djBoothPov)); /// remember that we have a 0 here , it start at 1 on the ctrl


        FixtureGroup group1 = new FixtureGroup();
        group1.FixtureInGroup.Add(fixtureLed1);
        group1.FixtureInGroup.Add(fixtureLed2);
        group1.FixtureInGroup.Add(haRgb1);
        group1.FixtureInGroup.Add(haRgb2);
        group1.FixtureInGroup.Add(wledBooth1);

        FixtureGroup group2 = new FixtureGroup();
        group2.FixtureInGroup.Add(fixtureLed3);
        group2.FixtureInGroup.Add(haRgb3);
        group2.FixtureInGroup.Add(haRgb6);
        group2.FixtureInGroup.Add(fixtureLed4);
        group2.FixtureInGroup.Add(wledBooth2);

        FixtureGroup group3 = new FixtureGroup();
        group3.FixtureInGroup.Add(fixtureLed5);
        group3.FixtureInGroup.Add(haRgb4);
        group3.FixtureInGroup.Add(haRgb7);
        group3.FixtureInGroup.Add(fixtureLed6);
        group3.FixtureInGroup.Add(wledBooth3);

        FixtureGroup group4 = new FixtureGroup();
        group4.FixtureInGroup.Add(fixtureLed7);
        group4.FixtureInGroup.Add(haRgb5);
        group4.FixtureInGroup.Add(fixtureLed8);
        group4.FixtureInGroup.Add(haRgbDM);
        group4.FixtureInGroup.Add(wledBooth4);


        FixtureGroup boothGroup1 = new FixtureGroup();
        boothGroup1.FixtureInGroup.Add(bootDjLed1);

        FixtureGroup boothGroup2 = new FixtureGroup();
        boothGroup2.FixtureInGroup.Add(bootDjLed2);

        FixtureGroup boothGroup3 = new FixtureGroup();
        boothGroup3.FixtureInGroup.Add(bootDjLed3);

        FixtureGroup boothGroup4 = new FixtureGroup();
        boothGroup4.FixtureInGroup.Add(bootDjLed4);

        FixtureGroup boothGroup5 = new FixtureGroup();
        boothGroup5.FixtureInGroup.Add(bootDjLed5);

        FixtureGroup boothGroup6 = new FixtureGroup();
        boothGroup6.FixtureInGroup.Add(bootDjLed6);

        Scene indoorScene = new Scene { Name = indoorSceneName };
        Scene outdoorScene = new Scene { Name = outDoorSceneName };
        var danceFloorZone = new Zone { Name = basementZoneName };
        var djBoothZone = new Zone { Name = djboothZoneName };
        var poolZone = new Zone { Name = poolZoneName };
        this.Scenes.Add(indoorScene);
        this.Scenes.Add(outdoorScene);
        indoorScene.Zones.Add(danceFloorZone);
        indoorScene.Zones.Add(djBoothZone);
        outdoorScene.Zones.Add(poolZone);
        FixtureCollection rgbLedDownLight = new RGBLedFixtureCollection();
        FixtureCollection movingHeadDanceFloor = new MovingHeadFixtureCollection();
        FixtureCollection movingHeadDJ = new MovingHeadFixtureCollection();
        danceFloorZone.FixtureTypes.Add(rgbLedDownLight);
        danceFloorZone.FixtureTypes.Add(movingHeadDanceFloor);
        djBoothZone.FixtureTypes.Add(movingHeadDJ);
        rgbLedDownLight.FixtureGroups.AddRange(new[] { group1, group2, group3, group4 });

        movingHeadDanceFloor.FixtureGroups.Add(movingHeadGroupFloor);
        movingHeadDJ.FixtureGroups.Add(movingHeadGroupBooth);

        var sharedEffect = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
        FixtureCollection poolRgbLed = new RGBLedFixtureCollection();
        poolZone.FixtureTypes.Add(poolRgbLed);
        poolRgbLed.FixtureGroups.AddRange(new[] { boothGroup1, boothGroup2, boothGroup3, boothGroup4, boothGroup5, boothGroup6 });
        rgbLedDownLight.EffectList.Add(new AllOffEffect(beatDetector, rgbLedDownLight, () => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity));
        rgbLedDownLight.EffectList.Add(new AllOnEffect(beatDetector, rgbLedDownLight, () => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity));
        rgbLedDownLight.EffectList.Add(new RandomEffect(beatDetector, rgbLedDownLight, () => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity));
        rgbLedDownLight.EffectList.Add(new BreathingEffect(beatDetector, rgbLedDownLight, () => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity));
        rgbLedDownLight.EffectList.Add(new FlashAllEffect(beatDetector, rgbLedDownLight, () => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity));
        rgbLedDownLight.EffectList.Add(new ZoneFlashEffect(beatDetector, rgbLedDownLight, () => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity));
        rgbLedDownLight.EffectList.Add(new ZoneRotateEffect(beatDetector, rgbLedDownLight, () => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity));
        rgbLedDownLight.EffectList.Add(new StaticColorFlashEffect(beatDetector, rgbLedDownLight, () => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity));

        poolRgbLed.EffectList.Add(new AllOffEffect(beatDetector, poolRgbLed, () => sharedEffect.MaxBoothIntesity, () => sharedEffect.MaxBoothFlashIntesity));
        poolRgbLed.EffectList.Add(new AllOnEffect(beatDetector, poolRgbLed, () => sharedEffect.MaxBoothIntesity, () => sharedEffect.MaxBoothFlashIntesity));
        poolRgbLed.EffectList.Add(new FlashAllEffect(beatDetector, poolRgbLed, () => sharedEffect.MaxBoothIntesity, () => sharedEffect.MaxBoothFlashIntesity));
        poolRgbLed.EffectList.Add(new RandomEffect(beatDetector, poolRgbLed, () => sharedEffect.MaxBoothIntesity, () => sharedEffect.MaxBoothFlashIntesity));
        poolRgbLed.EffectList.Add(new BreathingEffect(beatDetector, poolRgbLed, () => sharedEffect.MaxBoothIntesity, () => sharedEffect.MaxBoothFlashIntesity));
        poolRgbLed.EffectList.Add(new ZoneFlashEffect(beatDetector, poolRgbLed, () => sharedEffect.MaxBoothIntesity, () => sharedEffect.MaxBoothFlashIntesity));
        poolRgbLed.EffectList.Add(new ZoneRotateEffect(beatDetector, poolRgbLed, () => sharedEffect.MaxBoothIntesity, () => sharedEffect.MaxBoothFlashIntesity));
        poolRgbLed.EffectList.Add(new StaticColorFlashEffect(beatDetector, poolRgbLed, () => sharedEffect.MaxBoothIntesity, () => sharedEffect.MaxBoothFlashIntesity));

        movingHeadDanceFloor.EffectList.Add(new MovingHeadOffEffect(beatDetector, movingHeadDanceFloor, () => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead));
        movingHeadDanceFloor.EffectList.Add(new MovingHeadFlashAll(beatDetector, movingHeadDanceFloor, () => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead));
        movingHeadDanceFloor.EffectList.Add(new MovingHeadAllOn(beatDetector, movingHeadDanceFloor, () => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead));

        movingHeadDJ.EffectList.Add(new MovingHeadOffEffect(beatDetector, movingHeadDJ, () => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead));
        movingHeadDJ.EffectList.Add(new MovingHeadFlashAll(beatDetector, movingHeadDJ, () => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead));
        movingHeadDJ.EffectList.Add(new MovingHeadAllOn(beatDetector, movingHeadDJ, () => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead));


    }
}


