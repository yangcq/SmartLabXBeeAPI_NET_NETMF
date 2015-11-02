namespace SmartLab.XBee.Helper
{
    /// <summary>
    /// Scans the neighborhood for beacon
    /// responses. The ATAS command is only valid as a local
    /// command. Response frames are structured as:
    /// </summary>
    public class ActiveScanResult
    {
        /// <summary>
        /// unsigned byte = 2 - ZB firmware uses a different format than Wi-Fi XBee, which is type 1
        /// </summary>
        public int AS_Type;

        /// <summary>
        /// unsigned byte
        /// </summary>
        public int Channel;

        /// <summary>
        /// unsigned word in big endian format
        /// </summary>
        public byte[] PanID;

        /// <summary>
        /// eight unsigned bytes in bit endian format
        /// Allow Join ¨C unsigned byte ¨C 1 indicates join is enabled, 0
        /// that it is disabled
        /// </summary>
        public byte[] ExtendedPanID;

        /// <summary>
        /// unsigned byte
        /// </summary>
        public bool AllowJoin;

        /// <summary>
        /// unsigned byte
        /// </summary>
        public int StackProfile;

        /// <summary>
        /// unsigned byte, higher values are better
        /// </summary>
        public int LQI;

        /// <summary>
        /// signed byte, lower values are better
        /// </summary>
        public sbyte RSSI;
    }
}