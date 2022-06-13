using System.ServiceModel;

namespace DmxLib
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDmxService" in both code and config file together.
    [ServiceContract]
    public interface IDmxService
    {
        [OperationContract]
        void SetDmxChannel(int channel, byte value);

        [OperationContract]
        void UpdateAllDmxValue(byte[] value);

        [OperationContract]
        void SetBreak(long value);
        [OperationContract]
        void SetMBB(long value);
        [OperationContract]
        void SetMab(long value);

    }
}
