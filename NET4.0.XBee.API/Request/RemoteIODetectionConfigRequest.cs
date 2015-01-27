﻿using SmartLab.XBee.Options;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Request
{
    public class RemoteIODetectionConfigRequest : RemoteATCommandRequest
    {
        public RemoteIODetectionConfigRequest(byte frameID, Address remoteAddress, Device.Pin[] pins)
            : base(frameID, remoteAddress, ATCommand.Digital_IO_Change_Detection, RemoteCommandOptions.ApplyChanges, Device.Pin.IOChangeDetectionConfiguration(pins))
        { }
    }
}