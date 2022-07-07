using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenDmx
{
    /// <summary>
    /// http://www.erwinrol.com/index.php?stagecraft/dmx.php
    /// </summary>
    public class OpenDMX : IDisposable
    {
        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_Open(UInt32 uiPort, ref uint ftHandle);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_Close(uint ftHandle);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_Read(uint ftHandle, IntPtr lpBuffer, UInt32 dwBytesToRead, ref UInt32 lpdwBytesReturned);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_Write(uint ftHandle, IntPtr lpBuffer, UInt32 dwBytesToRead, ref UInt32 lpdwBytesWritten);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetDataCharacteristics(uint ftHandle, byte uWordLength, byte uStopBits, byte uParity);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetFlowControl(uint ftHandle, char usFlowControl, byte uXon, byte uXoff);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_GetModemStatus(uint ftHandle, ref UInt32 lpdwModemStatus);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_Purge(uint ftHandle, UInt32 dwMask);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetBreakOn(uint ftHandle);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetBreakOff(uint ftHandle);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_GetStatus(uint ftHandle, ref UInt32 lpdwAmountInRxQueue, ref UInt32 lpdwAmountInTxQueue, ref UInt32 lpdwEventStatus);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_ResetDevice(uint ftHandle);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetDivisor(uint ftHandle, ushort usDivisor);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_SetBaudRate(uint ftHandle, uint usBaud);

        [DllImport("FTD2XX.dll")]
        public static extern FT_STATUS FT_ClrRts(uint ftHandle);

        private byte[] buffer;
        private uint handle;
        private bool done = false;
        private FT_STATUS status;

        private const byte BITS_8 = 8;
        private const byte STOP_BITS_2 = 2;
        private const byte PARITY_NONE = 0;
        private const UInt16 FLOW_NONE = 0;
        private const byte PURGE_RX = 1;
        private const byte PURGE_TX = 2;
        public const int CHANNEL_COUNT = 513;  // can be any length up to 512. The shorter the faster.
        private bool mswitchOn = false;
        private DateTime mLastUpdated = DateTime.MinValue;
        private TimeSpan mInactivityUpdateInterval = new TimeSpan(0, 0, 5);
        private byte mMaxValue = 32;
        //private TimeSpan mBreakLenght = new TimeSpan(22728 );
        // a 50000 on a des pause qui sont trop longue
        // a 22728 on a des frame corrompu, trop vite

        private DmxLib.Dmx mDMXSetting;

        public DmxLib.Dmx DMXSetting
        {
            get
            {
                return mDMXSetting;
            }
        }

        public bool SwitchOn
        {
            get
            {
                return mswitchOn;
            }
            set
            {
                mswitchOn = value;
                if (!mswitchOn)
                    SwitchAllOff();
            }
        }

        private void SwitchAllOn()
        {
            int x;
            for (x = 1; x < CHANNEL_COUNT; x++)
            {
                SetDmxValue(x, mMaxValue); // set DMX channel 1 to maximum value
            }
        }

        private void SwitchAllOff()
        {
            int x;
            for (x = 1; x < CHANNEL_COUNT; x++)
            {
                SetDmxValue(x, 0); // set DMX channel 1 to maximum value
            }
        }

        public OpenDMX()
        {
            buffer = new byte[CHANNEL_COUNT];
            SwitchOn = true;
        }

        public void Start()
        {
            mDMXSetting = new DmxLib.Dmx();
            handle = 0;
            status = FT_Open(0, ref handle);
            Thread thread = new Thread(new ThreadStart(writeData));
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        public void Stop()
        {
            this.done = true;
        }

        public void SetDmxValue(int channel, byte value)
        {
            if ((channel >= CHANNEL_COUNT)
                ||
                (channel < 0))
                throw new InvalidOperationException("Channel number must be between 0 and " + CHANNEL_COUNT);

            buffer[channel] = value;
        }

        private void writeData()
        {
            initOpenDMX();
            FT_ResetDevice(handle);
            System.Threading.Thread.Sleep(new TimeSpan(mDMXSetting.MBB));
            FT_SetBreakOn(handle);
            while (!done)
            {
                if (SwitchOn)
                {
                    SwitchAllOn();
                }

                if (!AllOff || DateTime.UtcNow > mLastUpdated + mInactivityUpdateInterval)
                {
                    FT_SetBreakOff(handle);

                    System.Threading.Thread.Sleep(new TimeSpan(mDMXSetting.MAB));

                    writeUnsafe(handle, buffer, buffer.Length);
                    System.Threading.Thread.Sleep(new TimeSpan(mDMXSetting.MBB));
                    FT_SetBreakOn(handle);

                    mLastUpdated = DateTime.UtcNow;
                }
                System.Threading.Thread.Sleep(new TimeSpan(mDMXSetting.BreakLenght));
            }
            FT_Close(handle);
        }

        private bool AllOff
        {
            get
            {
                int x;
                for (x = 1; x < CHANNEL_COUNT; x++)
                {
                    if (buffer[x] != 0)
                        return false;
                }
                return true;
            }
        }

        private uint writeUnsafe(uint handle, byte[] data, int length)
        {
            IntPtr ptr = IntPtr.Zero;
            uint bytesWritten = 0;
            try
            {
                ptr = Marshal.AllocHGlobal((int)length);
                Marshal.Copy(data, 0, ptr, (int)length);
                FT_Write(handle, ptr, (uint)length, ref bytesWritten);
            }
            catch (Exception)
            {
                // error writting
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
            return bytesWritten;
        }

        private void initOpenDMX()
        {
            status = FT_ResetDevice(handle);
            status = FT_SetDivisor(handle, 12); // set baud rate
                                                //   status = FT_SetBaudRate(handle, 250000); // set baud rate

            status = FT_SetDataCharacteristics(handle, BITS_8, STOP_BITS_2, PARITY_NONE);
            status = FT_SetFlowControl(handle, (char)FLOW_NONE, 0, 0);
            status = FT_ClrRts(handle);
            status = FT_Purge(handle, PURGE_TX);
            status = FT_Purge(handle, PURGE_RX);
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Stop();
        }

        #endregion IDisposable Members
    }

    public enum FT_STATUS
    {
        FT_OK = 0,
        FT_INVALID_HANDLE,
        FT_DEVICE_NOT_FOUND,
        FT_DEVICE_NOT_OPENED,
        FT_IO_ERROR,
        FT_INSUFFICIENT_RESOURCES,
        FT_INVALID_PARAMETER,
        FT_INVALID_BAUD_RATE,
        FT_DEVICE_NOT_OPENED_FOR_ERASE,
        FT_DEVICE_NOT_OPENED_FOR_WRITE,
        FT_FAILED_TO_WRITE_DEVICE,
        FT_EEPROM_READ_FAILED,
        FT_EEPROM_WRITE_FAILED,
        FT_EEPROM_ERASE_FAILED,
        FT_EEPROM_NOT_PRESENT,

        FT_EEPROM_NOT_PROGRAMMED,
        FT_INVALID_ARGS,
        FT_OTHER_ERROR
    };
}