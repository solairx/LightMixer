﻿namespace LightMixer
{
    public enum DMXProMsgLabel
    {
        REPROGRAM_FIRMWARE_REQUEST = 1,
        PROGRAM_FLASH_PAGE_REQUEST = 2,
        PROGRAM_FLASH_PAGE_REPLY = 2,
        GET_WIDGET_PARAMETERS_REQUEST = 3,
        GET_WIDGET_PARAMETERS_REPLY = 3,
        SET_WIDGET_PARAMETERS_REQUEST = 4,
        SET_WIDGET_PARAMETERS_REPLY = 4,
        RECEIVED_DMX_PACKET = 5,
        OUTPUT_ONLY_SEND_DMX_PACKET_REQUEST = 6,
        SEND_RDM_PACKET_REQUEST = 7,
        RECEIVE_DMX_ON_CHANGE = 8,
        RECEIVED_DMX_CHANGE_OF_STATE_PACKET = 9,
        GET_WIDGET_SERIAL_NUMBER_REQUEST = 10,
        GET_WIDGET_SERIAL_NUMBER_REPLY = 10,
        SEND_RDM_DISCOVERY_REQUEST = 11
    }
}
