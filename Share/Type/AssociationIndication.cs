namespace SmartLab.XBee.Type
{
    /// <summary>
    /// New non-zero AI values may be added in later
    /// firmware versions. Applications should read AI until it
    /// returns 0x00, indicating a successful startup
    /// (coordinator) or join (routers and end devices)
    /// </summary>
    public enum AssociationIndication
    {
        /// <summary>
        /// Formed joined a network.
        /// Coordinators form a network, routers and end devices join a network.
        /// </summary>
        Successfully = 0x00,

        Scan_found_no_PANs = 0x21,

        /// <summary>
        /// Based on current SC and ID settings.
        /// </summary>
        Scan_found_no_valid_PANs = 0x22,


        /// <summary>
        /// Valid_Coordinator or Routers found, but they are not allowing joining.
        /// </summary>
        Not_allowing_joining_NJ_expired = 0x23,

        No_joinable_beacons_were_found = 0x24,

        /// <summary>
        /// Node should not be attempting to join at this time.
        /// </summary>
        Unexpected_state = 0x25,

        /// <summary>
        /// Typically due to incompatible security settings.
        /// </summary>
        Node_Joining_attempt_failed = 0x27,

        Coordinator_Start_attempt_failed = 0x2A,

        Checking_for_an_existing_coordinator = 0x2B,

        Attempt_to_leave_the_network_failed = 0x2C,

        Attempted_to_join_a_device_that_did_not_respond = 0xAB,

        /// <summary>
        /// Secure join error.
        /// </summary>
        Network_security_key_received_unsecured = 0xAC,

        /// <summary>
        /// Secure join error.
        /// </summary>
        Network_security_key_not_received = 0xAD,

        /// <summary>
        /// Secure join error.
        /// </summary>
        Joining_device_does_not_have_the_right_preconfigured_link_key = 0xAF,

        /// <summary>
        /// routers and end devices.
        /// </summary>
        Scanning_for_a_ZigBee_network = 0xFF,
    }
}
