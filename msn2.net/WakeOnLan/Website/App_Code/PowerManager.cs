// --------------------------------------------------------------------------
// Copyright (c) MEMOS Software s.r.o. All rights reserved.
//
// Wake on LAN
// 
// File     : PowerManager.cs
// Author   : Lukas Neumann <lukas.neumann@memos.cz>
// Created  : 080614
//
// -------------------------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

/// <summary>
/// Class for remote power management
/// </summary>
public static class PowerManager
{

    /// <summary>
    /// UDP port to be used for the broadcast 
    /// (basically any valid 2 byte number should work, but 7 is my lucky number...)
    /// </summary>
    private const int UDP_PORT = 7;


    /// <summary>
    /// Wake up the device by sending a 'magic' packet
    /// </summary>
    /// <param name="macAddress"></param>
    public static void WakeUp(string macAddress)
    {
        if (String.IsNullOrEmpty(macAddress))
            throw new ArgumentNullException("macAddress", "MAC Address must be specified");

        Regex macCheck = new Regex(@"([\dA-Fa-f]{2})(:[A-Fa-f\d]{2}){5}");
        if (!macCheck.IsMatch(macAddress))
            throw new ArgumentException("macAddress", "MAC Address must contain 6 bytes separated by colon.");

        string[] byteStrings = macAddress.Split(':');
        Debug.Assert(byteStrings.Length == 6);

        byte[] bytes = new byte[6];
        for (int i = 0; i < 6; i++)
            bytes[i] = (byte)Int32.Parse(byteStrings[i], System.Globalization.NumberStyles.AllowHexSpecifier);

        WakeUp(bytes);
    }

    /// <summary>
    /// Wake up the device by sending a 'magic' packet
    /// </summary>
    /// <param name="macAddress"></param>
    public static void WakeUp(byte[] macAddress)
    {
        if (macAddress == null)
            throw new ArgumentNullException("macAddress", "MAC Address must be specified");

        if (macAddress.Length != 6)
            throw new ArgumentOutOfRangeException("MAC Address is always 6 bytes");


        //Construct the packet
        List<byte> packet = new List<byte>();

        //Trailer of 6 FF packets
        for (int i = 0; i < 6; i++)
            packet.Add(0xFF);

        //Repeat 16 time the MAC address (which is 6 bytes)        
        for (int i = 0; i < 16; i++)
            packet.AddRange(macAddress);

        //We should have packet of 16*6+6 = 102 bytes
        Debug.Assert(packet.Count == 102);

        //Send the packet to broadcast address
        UdpClient client = new UdpClient();
        client.Connect(IPAddress.Broadcast, UDP_PORT);
        client.Send(packet.ToArray(), packet.Count);

    }
	
}
