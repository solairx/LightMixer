using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace MidiController
{
    class Program
    {

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        static void Main(string[] args)
        {
            MidiOutController controller = null;
            _handler += new EventHandler((a) =>
            {
                controller.Dispose();
                return true;
            });
            SetConsoleCtrlHandler(_handler, true);


            byte command = 0x90;
            byte note = 0x48;
            byte velocity = 0x7F;
            int message = (velocity << 16) + (note << 8) + command;
            while (true)
            {
                try

                {
                    var x = Console.ReadLine();
                    if (x == null)
                    {
                        Thread.Sleep(30);
                    }
                    if (x == "x")
                    {
                        return;
                    }

                    if (controller == null)
                    {
                        controller = new MidiOutController(int.Parse(x));
                    }
                    var res = controller.Send(int.Parse(x));
                    Console.WriteLine(res);
                    Thread.Sleep(1);

                }
                catch (Exception vexp)
                {
                    Console.WriteLine(vexp.ToString());
                }

            }
        }

        private static bool KillHandler(CtrlType sig)
        {
            return true;
        }
    }

    public class MidiOutController : IDisposable
    {
        [DllImport("winmm.dll")]
        internal static extern int midiOutClose(
        int hMidiIn);

        private static int handle = 0;
        protected delegate void MidiCallback(int handle, int msg, int instance, int param1, int param2);
        [DllImport("winmm.dll")]
        private static extern int midiOutOpen(ref int handle, int deviceID, MidiCallback proc, int instance, int flags);
        [DllImport("winmm.dll")]

        private static extern int midiOutMessage(ref int handle, int msg, int dw1, int dw2);

        [DllImport("winmm.dll")]
        private static extern int midiOutLongMsg(int hmo, IntPtr pmh, uint cbmh);

        [DllImport("winmm.dll")]

        protected static extern int midiOutShortMsg(int handle, int message);

        [DllImport("winmm.dll")]
        public static extern uint midiOutPrepareHeader(
        int hMidiOut,
        IntPtr lpMidiOutHdr,
        uint uSize);
        [DllImport("winmm.dll")]
        public static extern uint midiOutUnprepareHeader(
            IntPtr hMidiOut,
            IntPtr lpMidiOutHdr,
            uint uSize);

        public MidiOutController()
        {

        }
        public MidiOutController(int device)
        {
            int result = midiOutOpen(ref handle, device, null, 0, 0);
            if (result != 0)
                throw new Exception("Cannot open midi device");
            //    var resetStatus = SendLongMessage(new byte[] { 0xF0, 0x41, 0x10, 0x42, 0x12, 0x40, 0x00, 0x7F, 0x00, 0x41, 0xF7 });
            var resetStatus = SendLongMessage(new byte[] { 0xF0, 0x7f, 0x7f, 0x06, 0x01, 0xf7 });
            var resetStatus1 = SendLongMessage(new byte[] { 0xF0, 0x7f, 0x00, 0x06, 0x01, 0xf7 });
            var resetStatus2 = SendLongMessage(new byte[] { 0xF0, 0x00, 0x20, 0x7f, 0x03, 0x01, 0xf7 });
        }

        public int Send(int message)
        {
            var res = midiOutShortMsg(handle, message);
            return res;
        }

        public void Dispose()
        {
            var closeRes = midiOutClose(handle);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MIDIHDR
        {
            public IntPtr lpData;
            public uint dwBufferLength;
            public uint dwBytesRecorded;
            public int dwUser;
            public uint dwFlags;
            public IntPtr lpNext;
            public int Reserved;
            public uint dwOffset;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] reservedArray;
        }

        public bool SendLongMessage(byte[] messageBuffer)
        {
            IntPtr DataBufferPointer = IntPtr.Zero;
            uint lngReturn;
            MIDIHDR typMsgHeader = new MIDIHDR();
            bool blnResult;

            blnResult = false;

            typMsgHeader.dwBufferLength = (uint)messageBuffer.Count();
            typMsgHeader.dwFlags = 0;

            try
            {
                typMsgHeader.lpData = Marshal.AllocHGlobal(messageBuffer.Count());
                Marshal.Copy(messageBuffer, 0, typMsgHeader.lpData, messageBuffer.Count());

                DataBufferPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typMsgHeader));
                Marshal.StructureToPtr(typMsgHeader, DataBufferPointer, true);
            }
            catch (OutOfMemoryException ex)
            {
                throw (ex);
            }

            if (DataBufferPointer != IntPtr.Zero)
            {
                //Header must be prepared before use.
                lngReturn = midiOutPrepareHeader(handle, DataBufferPointer,
                    (uint)Marshal.SizeOf(typMsgHeader));
                lngReturn = (uint)midiOutLongMsg(handle, DataBufferPointer, (uint)Marshal.SizeOf(typMsgHeader));

                Marshal.Release(typMsgHeader.lpData);
                Marshal.Release(DataBufferPointer);

            }

            return blnResult;
        }
    }

}
