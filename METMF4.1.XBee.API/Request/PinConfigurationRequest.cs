﻿using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Request
{
    public class PinConfigurationRequest : ATCommandRequest
    {
        public PinConfigurationRequest(byte frameID, Pin pin, Pin.Functions function)
            : base(frameID, new ATCommand(pin.COMMAND), new byte[] { (byte)function })
        { }

        public void SetPinFunction(Pin.Functions functions) { this.SetContent(4, (byte)functions); }
    }
}