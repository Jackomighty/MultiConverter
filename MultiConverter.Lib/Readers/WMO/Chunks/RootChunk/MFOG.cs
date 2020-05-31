﻿using MultiConverter.Lib.Readers.Base;
using MultiConverter.Lib.Readers.WMO.Entries;
using MultiConverter.Lib.Util;
using System.Collections.Generic;
using System.IO;

namespace MultiConverter.Lib.Readers.WMO.Chunks
{
    public class MFOG : ChunkBase
    {
        public override string Signature => "MFOG";
        public override uint Length => (uint)Write().Length;
        public override uint Order => 16;

        /// <summary>
        /// List of <see cref="MFOGEntry"/>
        /// </summary>
        public List<MFOGEntry> MFOGs = new List<MFOGEntry>();

        public override void Read(byte[] inData)
        {
            using (var stream = new MemoryStream(inData))
            using (var reader = new BinaryReader(stream))
            {
                var mfogSize = inData.Length / 48;
                for (var i = 0; i < mfogSize; ++i)
                {
                    MFOGs.Add(new MFOGEntry
                    {
                        Flags = reader.ReadUInt32(),
                        Position = reader.ReadC3Vector(),
                        SmallRadius = reader.ReadSingle(),
                        LargeRadius = reader.ReadSingle(),
                        FogEnd = reader.ReadSingle(),
                        FogStartMultiplier = reader.ReadSingle(),
                        FogColor = reader.ReadCArgb(),
                        Unk1 = reader.ReadSingle(),
                        Unk2 = reader.ReadSingle(),
                        FogColor2 = reader.ReadCArgb()
                    });
                }
            }
        }

        public override byte[] Write()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                foreach (var mfog in MFOGs)
                {
                    writer.Write(mfog.Flags);
                    writer.WriteC3Vector(mfog.Position);
                    writer.Write(mfog.SmallRadius);
                    writer.Write(mfog.LargeRadius);
                    writer.Write(mfog.FogEnd);
                    writer.Write(mfog.FogStartMultiplier);
                    writer.WriteCArgb(mfog.FogColor);
                    writer.Write(mfog.Unk1);
                    writer.Write(mfog.Unk2);
                    writer.WriteCArgb(mfog.FogColor2);
                }

                return stream.ToArray();
            }
        }
    }
}
