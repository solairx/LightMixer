using LightMixer;
using LightMixer.Model;
using LightMixer.Model.Fixture;
using Unity;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

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
        RgbFixture fixtureLed8 = new RgbFixture(0);
        RgbFixture fixtureLed1 = new RgbFixture(3);
        RgbFixture fixtureLed2 = new RgbFixture(6);
        RgbFixture fixtureLed3 = new RgbFixture(9);
        RgbFixture fixtureLed4 = new RgbFixture(12);
        RgbFixture fixtureLed5 = new RgbFixture(15);
        RgbFixture fixtureLed6 = new RgbFixture(18);
        RgbFixture fixtureLed7 = new RgbFixture(21);



        RgbFixture bootDjLed1 = new RgbFixture(24);
        RgbFixture bootDjLed2 = new RgbFixture(27);
        RgbFixture bootDjLed3 = new RgbFixture(30);
        RgbFixture bootDjLed4 = new RgbFixture(33);
        RgbFixture bootDjLed5 = new RgbFixture(36);
        RgbFixture bootDjLed6 = new RgbFixture(39);

        var haRgb1 = new WledFixture("192.168.1.46");
        var haRgb2 = new WledFixture("192.168.1.31");
        var haRgb3 = new WledFixture("192.168.1.6");
        var haRgb4 = new WledFixture("192.168.1.3");
        var haRgb5 = new WledFixture("192.168.1.37");
        var haRgb6 = new WledFixture("192.168.1.11");
        var haRgb7 = new WledFixture("192.168.1.51");
        var haRgbDM = new ShellyDimmerFixture("light.dimmer_plafond_ss");

        WledServer djBoothWled = new WledServer("192.168.1.9");
        var wledBooth1 = new WledFixture(djBoothWled, djBoothWled.State.seg[0]);
        var wledBooth2 = new WledFixture(djBoothWled, djBoothWled.State.seg[1]);
        var wledBooth3 = new WledFixture(djBoothWled, djBoothWled.State.seg[2]);
        var wledBooth4 = new WledFixture(djBoothWled, djBoothWled.State.seg[3]);

        // Four head schematic :
        // 2 1
        // 4 3
        FixtureGroup movingHeadGroupFloor = new FixtureGroup();
        var djBoothfourPov1 = new List<PointOfInterest>
        {
            new PointOfInterest{  Location = PointOfInterestLocation.DJ, Tilt = 10000, Pan = 4000},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBall, Pan = 0, Pan2=0, Tilt = 4000, Tilt2 = 7250},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBallStatic, Pan = 0, Pan2=0, Tilt = 6250, Tilt2 = 6250},
            new PointOfInterest{  Location = PointOfInterestLocation.Circle, Tilt = 0, Tilt2=65535, Pan = 65535, Pan2 = 1}
        };

        var djBoothfourPov2 = new List<PointOfInterest>
        {
            new PointOfInterest{  Location = PointOfInterestLocation.DJ, Tilt = 10000, Pan = 38000},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBall, Pan = 0, Pan2=0, Tilt = 3000, Tilt2 = 6250},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBallStatic,Pan = 0, Pan2=0, Tilt = 4000, Tilt2 = 4000},
            new PointOfInterest{  Location = PointOfInterestLocation.Circle, Tilt = 0, Tilt2=65535, Pan = 65535, Pan2 = 1}
        };

        var djBoothfourPov3 = new List<PointOfInterest>
        {
         new PointOfInterest{  Location = PointOfInterestLocation.DJ, Tilt = 3000, Pan = 0},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBall, Pan = 0, Pan2=0, Tilt = 3000, Tilt2 = 6250},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBallStatic,Pan = 0, Pan2=0, Tilt = 4000, Tilt2 = 4000},
            new PointOfInterest{  Location = PointOfInterestLocation.Circle, Tilt = 0, Tilt2=65535, Pan = 65535, Pan2 = 1}
        };
        var djBoothfourPov4 = new List<PointOfInterest>
        {
            new PointOfInterest{  Location = PointOfInterestLocation.DJ, Tilt = 3000, Pan = 0},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBall, Pan = 0, Pan2=0, Tilt = 3000, Tilt2 = 6250},
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBallStatic,Pan = 0, Pan2=0, Tilt = 4000, Tilt2 = 4000},
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
            new PointOfInterest{  Location = PointOfInterestLocation.DiscoBallStatic, Tilt = 3000, Tilt2=3000, Pan = 0, Pan2 = 0},
            new PointOfInterest{  Location = PointOfInterestLocation.Circle, Tilt = 0, Tilt2=65535, Pan = 65535, Pan2 = 1}
        };
        var movingHeadMasterDJ = new MovingHeadFixture(399, djBoothPov);
        movingHeadGroupBooth.FixtureInGroup.Add(movingHeadMasterDJ);
        movingHeadGroupBooth.FixtureInGroup.Add(new MovingHeadFixture(299, djBoothPov)); /// remember that we have a 0 here , it start at 1 on the ctrl

        FixtureGroup groupBooth1 = new FixtureGroup();
        groupBooth1.FixtureInGroup.Add(wledBooth1);
        FixtureGroup groupBooth2 = new FixtureGroup();
        groupBooth2.FixtureInGroup.Add(wledBooth2);
        FixtureGroup groupBooth3 = new FixtureGroup();
        groupBooth3.FixtureInGroup.Add(wledBooth3);
        FixtureGroup groupBooth4 = new FixtureGroup();
        groupBooth4.FixtureInGroup.Add(wledBooth4);

        FixtureGroup group1 = new FixtureGroup();
        group1.FixtureInGroup.Add(fixtureLed1);
        group1.FixtureInGroup.Add(fixtureLed2);
        group1.FixtureInGroup.Add(haRgb1);
        group1.FixtureInGroup.Add(haRgb2);

        FixtureGroup group2 = new FixtureGroup();
        group2.FixtureInGroup.Add(fixtureLed3);
        group2.FixtureInGroup.Add(haRgb3);
        group2.FixtureInGroup.Add(haRgb6);
        group2.FixtureInGroup.Add(fixtureLed4);

        FixtureGroup group3 = new FixtureGroup();
        group3.FixtureInGroup.Add(fixtureLed5);
        group3.FixtureInGroup.Add(haRgb4);
        group3.FixtureInGroup.Add(haRgb7);
        group3.FixtureInGroup.Add(fixtureLed6);

        FixtureGroup group4 = new FixtureGroup();
        group4.FixtureInGroup.Add(fixtureLed7);
        group4.FixtureInGroup.Add(haRgb5);
        group4.FixtureInGroup.Add(fixtureLed8);
        group4.FixtureInGroup.Add(haRgbDM);

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
        FixtureCollection boothDownLight = new RGBLedFixtureCollection();
        FixtureCollection movingHeadDanceFloor = new MovingHeadFixtureCollection();
        FixtureCollection movingHeadDJ = new MovingHeadFixtureCollection();
        djBoothZone.FixtureTypes.Add(boothDownLight);
        danceFloorZone.FixtureTypes.Add(rgbLedDownLight);
        danceFloorZone.FixtureTypes.Add(movingHeadDanceFloor);
        djBoothZone.FixtureTypes.Add(movingHeadDJ);
        rgbLedDownLight.FixtureGroups.AddRange(new[] { group1, group2, group3, group4 });

        boothDownLight.FixtureGroups.AddRange(new[] { groupBooth1, groupBooth2, groupBooth3, groupBooth4 });

        movingHeadDanceFloor.FixtureGroups.Add(movingHeadGroupFloor);
        movingHeadDJ.FixtureGroups.Add(movingHeadGroupBooth);

        var sharedEffect = LightMixerBootStrap.UnityContainer.Resolve<SharedEffectModel>();
        FixtureCollection poolRgbLed = new RGBLedFixtureCollection();
        poolZone.FixtureTypes.Add(poolRgbLed);
        poolRgbLed.FixtureGroups.AddRange(new[] { boothGroup1, boothGroup2, boothGroup3, boothGroup4, boothGroup5, boothGroup6 });
        boothDownLight.BindCollectionToIntensityGetter(() => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity);
        boothDownLight.EffectList.AddRange(new EffectBase[] { new AllOffEffect(), new AllOnEffect(), new FlashAllEffect(), new RandomEffect(), new BreathingEffect(), new ZoneFlashEffect(), new ZoneRotateEffect(), new StaticColorFlashEffect() });

        rgbLedDownLight.BindCollectionToIntensityGetter(() => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity);
        rgbLedDownLight.EffectList.AddRange(new EffectBase[] { new AllOffEffect(), new AllOnEffect(), new FlashAllEffect(), new RandomEffect(), new BreathingEffect(), new ZoneFlashEffect(), new ZoneRotateEffect(), new StaticColorFlashEffect() });

        poolRgbLed.BindCollectionToIntensityGetter(() => sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity);
        poolRgbLed.EffectList.AddRange(new EffectBase[] { new AllOffEffect(), new AllOnEffect(), new FlashAllEffect(), new RandomEffect(), new BreathingEffect(), new ZoneFlashEffect(), new ZoneRotateEffect(), new StaticColorFlashEffect() });

        movingHeadDanceFloor.BindCollectionToIntensityGetter(() => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead);
        movingHeadDanceFloor.EffectList.AddRange(new EffectBase[] { new MovingHeadOffEffect(), new MovingHeadFlashAll(), new MovingHeadAllOn() });

        movingHeadDJ.BindCollectionToIntensityGetter(() => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead);
        movingHeadDJ.EffectList.AddRange(new EffectBase[] { new MovingHeadOffEffect(), new MovingHeadFlashAll(), new MovingHeadAllOn() });
    }

    
}
