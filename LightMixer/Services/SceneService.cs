﻿using LightMixer.Model.Fixture;
using System.Collections.Generic;
using LightMixer.Model;
using Microsoft.Practices.Unity;
using LightMixer;

public class SceneService
{
    public const string indoorSceneName = "indoor";
    public const string outDoorSceneName = "outdoor";
    public const string basementZoneName = "basement";
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

        FixtureGroup movingHeadGroup = new FixtureGroup();
        movingHeadGroup.FixtureInGroup.Add(new MovingHeadFixture(399));
        movingHeadGroup.FixtureInGroup.Add(new MovingHeadFixture(299)); /// remmeber that we have a 0 here , it start at 1 on the ctrl

        FixtureGroup group1 = new FixtureGroup();
        group1.FixtureInGroup.Add(fixtureLed1);
        group1.FixtureInGroup.Add(fixtureLed2);

        FixtureGroup group2 = new FixtureGroup();
        group2.FixtureInGroup.Add(fixtureLed3);
        group2.FixtureInGroup.Add(fixtureLed4);

        FixtureGroup group3 = new FixtureGroup();
        group3.FixtureInGroup.Add(fixtureLed5);
        group3.FixtureInGroup.Add(fixtureLed6);

        FixtureGroup group4 = new FixtureGroup();
        group4.FixtureInGroup.Add(fixtureLed7);
        group4.FixtureInGroup.Add(fixtureLed8);

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
        var basementZone = new Zone { Name = basementZoneName };
        var poolZone = new Zone { Name = poolZoneName };
        this.Scenes.Add(indoorScene);
        this.Scenes.Add(outdoorScene);
        indoorScene.Zones.Add(basementZone);
        outdoorScene.Zones.Add(poolZone);
        FixtureCollection rgbLedDownLight = new RGBLedFixtureCollection();
        FixtureCollection movingHead = new MovingHeadFixtureCollection();
        basementZone.FixtureTypes.Add(rgbLedDownLight);
        basementZone.FixtureTypes.Add(movingHead);
        rgbLedDownLight.FixtureGroups.AddRange(new[] { group1, group2, group3, group4 });

        movingHead.FixtureGroups.Add(movingHeadGroup);
        var sharedEffect = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
        FixtureCollection poolRgbLed = new RGBLedFixtureCollection();
        poolZone.FixtureTypes.Add(poolRgbLed);
        poolRgbLed.FixtureGroups.AddRange(new[] { boothGroup1, boothGroup2, boothGroup3, boothGroup4, boothGroup5, boothGroup6 });
        rgbLedDownLight.EffectList.Add(new AllOffEffect(beatDetector, rgbLedDownLight,()=> sharedEffect.MaxLightIntesity, () => sharedEffect.MaxLightFlashIntesity));
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

        movingHead.EffectList.Add(new MovingHeadOffEffect(beatDetector, movingHead, () => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead));
        movingHead.EffectList.Add(new MovingHeadFlashAll(beatDetector, movingHead, () => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead));
        movingHead.EffectList.Add(new MovingHeadAllOn(beatDetector, movingHead, () => sharedEffect.MaxLightIntesityMovingHead, () => sharedEffect.MaxLightIntesityMovingHead));
    }
}


