using Oculus.Avatar;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OculusAvatarView : MonoBehaviourPun, IPunObservable
{
    public bool IsLocalAvatar
    {
        get => photonView.IsMine;
    }

    public OvrAvatar Avatar;
    private int PacketSequence = 0;
    LinkedList<byte[]> packetQueue = new LinkedList<byte[]>();
    private void Start()
    {
        if (Avatar == null) Avatar = GetComponent<OvrAvatar>();
        if(!IsLocalAvatar)
        {
            var ovrRemoteDriver = gameObject.AddComponent<OvrAvatarRemoteDriver>();
            ovrRemoteDriver.Mode = OvrAvatarDriver.PacketMode.SDK;
            Avatar.Driver = ovrRemoteDriver;
            Destroy(gameObject.GetComponent<OvrAvatarLocalDriver>());
        }
        if (IsLocalAvatar)
        {
            Avatar.RecordPackets = true;
            Avatar.PacketRecorded += OnLocalAvatarPacketRecorded;
        }
        else
        {
            Avatar.RecordPackets = false;
        }
    }

    private void OnLocalAvatarPacketRecorded(object sender, OvrAvatar.PacketEventArgs args)
    {
        using (MemoryStream outputStream = new MemoryStream())
        {
            BinaryWriter writer = new BinaryWriter(outputStream);

            if (Avatar.UseSDKPackets)
            {

                var size = CAPI.ovrAvatarPacket_GetSize(args.Packet.ovrNativePacket);
                byte[] data = new byte[size];
                CAPI.ovrAvatarPacket_Write(args.Packet.ovrNativePacket, size, data);

                print($"PacketSequence = {PacketSequence}");
                print($"Local avatar size = {size}");

                writer.Write(PacketSequence++);
                writer.Write(size);
                writer.Write(data);
            }
            else
            {
                writer.Write(PacketSequence++);
                args.Packet.Write(outputStream);
            }

            SendPacketData(outputStream.ToArray());
        }
        print("local avatar packed recorded");
    }

    void SendPacketData(byte[] data)
    {
        byte[] newData = new byte[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            newData[i] = data[i];
        }

        packetQueue.AddLast(newData);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (packetQueue.Count > 0)
            {
                print("im write packet now");
                stream.SendNext(packetQueue.Last.Value);
            }
        }
        else 
        if (stream.IsReading)
        {
            if (!IsLocalAvatar)
            {
                ReceivePacketData((byte[])stream.ReceiveNext());
            }
        }
    }

    void ReceivePacketData(byte[] data)
    {
        using (MemoryStream inputStream = new MemoryStream(data))
        {
            BinaryReader reader = new BinaryReader(inputStream);
            int sequence = reader.ReadInt32();
            print($"Readed sequence {sequence}");
            OvrAvatarPacket avatarPacket;
            if (Avatar.UseSDKPackets)
            {
                int size = reader.ReadInt32();
                byte[] sdkData = reader.ReadBytes(size);

                print($"Size of packet = {size}");
                print($"Bytes = {sdkData}");

                IntPtr packet = CAPI.ovrAvatarPacket_Read((UInt32)data.Length, sdkData);
                avatarPacket = new OvrAvatarPacket { ovrNativePacket = packet };
            }
            else
            {
                avatarPacket = OvrAvatarPacket.Read(inputStream);
            }

            Avatar.GetComponent<OvrAvatarRemoteDriver>().QueuePacket(sequence, avatarPacket);
        }
    }
}
