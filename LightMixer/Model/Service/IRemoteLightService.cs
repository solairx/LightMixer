using LaserDisplay;
using LightMixer.Model.Fixture;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace LightMixer.Model.Service
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IRemoteLightService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IRemoteLightService
    {
        [OperationContract]
        ObservableCollection<MovingHeadFixture.Program> MovingHeadProgram();

        [OperationContract]
        ObservableCollection<MovingHeadFixture.Gobo> MovingHeadGobo();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<string> MovingHeadProgramString();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<string> MovingHeadGoboString();

        [OperationContract]
        ObservableCollection<ColorMode> LaserColorModeList();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<string> LaserColorModeListString();

        [OperationContract()]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        ObservableCollection<string> LedEffectCollection();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        ObservableCollection<string> MovingHeadEffectCollection();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        ObservableCollection<string> BoothEffectCollection();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<string> LaserEffectList();

        [OperationContract]
        void UpdateLaser(LaserDataContract contract);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "UpdateMovingHead?effect={effect}&program={program}&gobo={gobo}&secondBetweenGoboChange={secondBetweenGoboChange}&secondBetweenProgramChange={secondBetweenProgramChange}&movingHeadMaxSpeed={movingHeadMaxSpeed}&autoChangeGobo={autoChangeGobo}&autoChangeProgram={autoChangeProgram}")]
        void UpdateMovingHead(string effect, string program, string gobo, int secondBetweenGoboChange, int secondBetweenProgramChange, double movingHeadMaxSpeed, bool autoChangeGobo, bool autoChangeProgram);


        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "UpdateLaserRest?AutoChangeEvent={AutoChangeEvent}&AutoChangeEventLaser={AutoChangeEventLaser}&AutoMixDelay={AutoMixDelay}&Blue={Blue}&Green={Green}&LaserPause={LaserPause}&LaserSpeedAdj={LaserSpeedAdj}&LaserSpeedRatio={LaserSpeedRatio}&ManualBeat={ManualBeat}&ManualBeatOnly={ManualBeatOnly}&OnBeat={OnBeat}&OnBeatReverse={OnBeatReverse}&UseBeatTurnOff={UseBeatTurnOff}&LaserCurrentEventID={LaserCurrentEventID}&LaserColorMode={LaserColorMode}")]
        void UpdateLaserRest(bool AutoChangeEvent, bool AutoChangeEventLaser, int AutoMixDelay, bool Blue, bool Green, bool LaserPause, int LaserSpeedAdj, int LaserSpeedRatio, bool ManualBeat, bool ManualBeatOnly, bool OnBeat, bool OnBeatReverse, bool UseBeatTurnOff, string LaserCurrentEventID, string LaserColorMode);

        [OperationContract]
        LaserDataContract GetLaserStatus();

        [OperationContract]
        void UpdateDmx(DmxDataContract contract);


        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "CurrentLedEffect?effect={currentEffect}")]
        void CurrentLedEffect(string currentEffect);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "Intensity?led={led}&flash={flash}&head={head}&beatRepeat={beatRepeat}")]
        void Intensity(double led, double flash, double head, double beatRepeat);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Json, Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "Color?red={red}&green={green}&blue={blue}&autoChangeColor={autoChangeColor}")]
        void Color(int red, int green, int blue, bool autoChangeColor);

        [OperationContract]
        DmxDataContract GetDmxStatus();
    }
}
