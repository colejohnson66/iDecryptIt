/* =============================================================================
 * File:   Common.cs
 * Author: Cole Johnson
 * =============================================================================
 * Copyright (c) 2014, Cole Johnson
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 *   along with this program. If not, see <http://www.gnu.org/licenses/>.
 * =============================================================================
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xpwn.Dmg
{
    public class DmgCommon
    {
        public const uint ChecksumCrc32 = 0x00000002;
        public const ushort ChecksumMKBlock = 0x0002;
        public const ushort ChecksumNone = 0x0000;

        public const uint BlockZlib = 0x80000005;
        public const uint BlockRaw = 0x00000001;
        public const uint BlockIgnore = 0x00000002;
        public const uint BlockComment = 0x7FFFFFFE;
        public const uint BlockTerminator = 0xFFFFFFFF;

        public const int SectorSize = 512;

        public const ushort DriverDescriptorSignature = 0x4552;
        public const ushort ApplePartitionMapSignature = 0x504D;
        public const uint UdifBlockSignature = 0x6D697368;
        public const uint KolySignature = 0x6B6F6C79;
        public const ushort HfsxSignature = 0x4858;

        public const ushort AttributeHdiUtil = 0x0050;

        public const string HfsxVolumeType = "Apple_HFSX";

        public const int DDMSize = 1;
        public const int PartitionSize = 0x3f;
        public const int AtapiSize = 8;
        public const int FreeSize = 10;
        public const int ExtraSize = AtapiOffset + AtapiSize + FreeSize;

        public const int DDMOffset = 0;
        public const int PartitionOffset = DDMSize;
        public const int AtapiOffset = 64;
        public const int UserOffset = AtapiOffset + AtapiSize;

        public const uint BootCodeDmmy = 0x444D4D59;
        public const uint BootCodeGoon = 0x676F6F6E;

        public const int kUdifFlagsFlattened = 1;
        public const int kUdifDeviceImageType = 1;
        public const int kUdifPartitionImageType = 2;
    }

    public struct UdifChecksum
    {
        public uint Type;
        public uint Size;
        public uint[] Data; // size = 0x20 (32)
    }

    public struct UdifID
    {
        public uint Data1; // smallest
        public uint Data2;
        public uint Data3;
        public uint Data4; // largest
    }

    public class UDIFResourceFile
    {
        public uint Signature;
        public uint Version;
        public uint HeaderSize;
        public uint Flags;

        public ulong RunningDataForkOffset;
        public ulong DataForkOffset;
        public ulong DataForkLength;
        public ulong RsrcForkOffset;
        public ulong RsrcForkLength;

        public uint SegmentNumber;
        public uint SegmentCount;
        // a 128-bit number like a GUID, but does not seem to be a OSF GUID,
        //   since it doesn't have the proper versioning byte
        public UdifID SegmentID;

        public UdifChecksum DataForkChecksum;

        public ulong XmlOffset;
        public ulong XmlLength;

        // this is actually the perfect amount of space to store every thing in this struct until the checksum
        private byte[] _reserved1; // size = 0x78

        public UdifChecksum MasterChecksum;

        public uint ImageVariant;
        public ulong SectorCount;

        private uint _reserved2;
        private uint _reserved3;
        private uint _reserved4;
    }

    public struct BlkxRun
    {
        public uint Type;
        private uint _reserved;
        public ulong SectorStart;
        public ulong SectorCount;
        public ulong CompOffset;
        public ulong CompLength;
    }

    public struct SizeResource
    {
        public ushort Version; // set to 5
        // first dword of v53 (ImageInfoRec)
        public uint IsHfs;
        // second dword of v53: garbage if HFS+, stuff relating to HFS embedded if it's that
        public uint unknown1;
        // length of data that proceeds, comes right before the data in ImageInfoRec; set to 0 for HFS, HFS+
        public byte DataLength;
        // other data from v53, dataLen + 1 bytes, the rest NULL filled; maybe a string? Not set for HFS, HFS+
        public byte[] Data; // size = 255
        // 8 bytes before volumeModified in v53; seems to be always 0 for HFS, HFS+
        public uint unknown2;
        // 4 bytes before volumeModified in v53; seems to be always 0 for HFS, HFS+
        public uint unknown3;
        // offset 272 in v53
        public uint VolumeModified;
        // seems to always be 0 for UDIF
        public uint unknown4;
        // HX in our case
        public ushort VolumeSignature;
        // always set to 1
        public ushort SizePresent;
    }

    public struct CSumResource
    {
        public ushort Version; // always set to 1
        public uint Type; // set to 0x2 for MKBlockChecksum
        public uint Checksum;
    }
}
/*
#include <hfs/hfsplus.h>
#include "abstractfile.h"

typedef struct NSizResource
{
    char isVolume;
    unsigned char* sha1Digest;
    uint32_t blockChecksum2;
    uint32_t bytes;
    uint32_t modifyDate;
    uint32_t partitionNumber;
    uint32_t version;
    uint32_t volumeSignature;
    struct NSizResource* next;
}
NSizResource;

#define DDM_DESCRIPTOR 0xFFFFFFFF
#define ENTIRE_DEVICE_DESCRIPTOR 0xFFFFFFFE

typedef struct {
    uint32_t fUDIFBlocksSignature;
uint32_t infoVersion;
uint64_t firstSectorNumber;
uint64_t sectorCount;

uint64_t dataStart;
uint32_t decompressBufferRequested;
uint32_t blocksDescriptor;

uint32_t reserved1;
uint32_t reserved2;
uint32_t reserved3;
uint32_t reserved4;
uint32_t reserved5;
uint32_t reserved6;

UDIFChecksum checksum;

uint32_t blocksRunCount;
BLKXRun runs[0];
} __attribute__((__packed__)) BLKXTable;

typedef struct {
    uint32_t ddBlock;
uint16_t ddSize;
uint16_t ddType;
} __attribute__((__packed__)) DriverDescriptor;

typedef struct {
    uint16_t pmSig;
uint16_t pmSigPad;
uint32_t pmMapBlkCnt;
uint32_t pmPyPartStart;
uint32_t pmPartBlkCnt;
unsigned char pmPartName[32];
unsigned char pmParType[32];
uint32_t pmLgDataStart;
uint32_t pmDataCnt;
uint32_t pmPartStatus;
uint32_t pmLgBootStart;
uint32_t pmBootSize;
uint32_t pmBootAddr;
uint32_t pmBootAddr2;
uint32_t pmBootEntry;
uint32_t pmBootEntry2;
uint32_t pmBootCksum;
unsigned char pmProcessor[16];
uint32_t bootCode;
uint16_t pmPad[186];
} __attribute__((__packed__)) Partition;

typedef struct {
    uint16_t sbSig;
uint16_t sbBlkSize;
uint32_t sbBlkCount;
uint16_t sbDevType;
uint16_t sbDevId;
uint32_t sbData;
uint16_t sbDrvrCount;
uint32_t ddBlock;
uint16_t ddSize;
uint16_t ddType;
DriverDescriptor ddPad[0];
} __attribute__((__packed__)) DriverDescriptorRecord;

typedef struct ResourceData
{
    uint32_t attributes;
    unsigned char* data;
    size_t dataLength;
    int id;
    char* name;
    struct ResourceData* next;
}
ResourceData;

typedef void (* FlipDataFunc)(unsigned char* data, char out);
typedef void (* ChecksumFunc)(void* ckSum, const unsigned char* data, size_t len);

typedef struct ResourceKey
{
    unsigned char* key;
    ResourceData* data;
    struct ResourceKey* next;
	FlipDataFunc flipData;
}
ResourceKey;

#define SHA1_DIGEST_SIZE 20

typedef struct {
    uint32_t state[5];
uint32_t count[2];
uint8_t buffer[64];
} SHA1_CTX;

typedef struct {
    uint32_t block;
uint32_t crc;
SHA1_CTX sha1;
} ChecksumToken;

static inline uint32_t readUInt32(AbstractFile* file)
{
    uint32_t data;

    ASSERT(file->read(file, &data, sizeof(data)) == sizeof(data), "fread");
    FLIPENDIAN(data);

    return data;
}

static inline void writeUInt32(AbstractFile* file, uint32_t data)
{
    FLIPENDIAN(data);
    ASSERT(file->write(file, &data, sizeof(data)) == sizeof(data), "fwrite");
}

static inline uint32_t readUInt64(AbstractFile* file)
{
    uint64_t data;

    ASSERT(file->read(file, &data, sizeof(data)) == sizeof(data), "fread");
    FLIPENDIAN(data);

    return data;
}

static inline void writeUInt64(AbstractFile* file, uint64_t data)
{
    FLIPENDIAN(data);
    ASSERT(file->write(file, &data, sizeof(data)) == sizeof(data), "fwrite");
}

#ifdef __cplusplus
extern "C" {
#endif
	void outResources(AbstractFile* file, AbstractFile* out);

uint32_t CRC32Checksum(uint32_t* crc, const unsigned char* buf, size_t len);
uint32_t MKBlockChecksum(uint32_t* ckSum, const unsigned char* data, size_t len);

void BlockSHA1CRC(void* token, const unsigned char* data, size_t len);
void BlockCRC(void* token, const unsigned char* data, size_t len);
void CRCProxy(void* token, const unsigned char* data, size_t len);

void SHA1Init(SHA1_CTX* context);
void SHA1Update(SHA1_CTX* context, const uint8_t* data, const size_t len);
void SHA1Final(uint8_t digest[SHA1_DIGEST_SIZE], SHA1_CTX* context);

void flipUDIFChecksum(UDIFChecksum* o, char out);
void readUDIFChecksum(AbstractFile* file, UDIFChecksum* o);
void writeUDIFChecksum(AbstractFile* file, UDIFChecksum* o);
void readUDIFID(AbstractFile* file, UDIFID* o);
void writeUDIFID(AbstractFile* file, UDIFID* o);
void readUDIFResourceFile(AbstractFile* file, UDIFResourceFile* o);
void writeUDIFResourceFile(AbstractFile* file, UDIFResourceFile* o);

ResourceKey* readResources(AbstractFile* file, UDIFResourceFile* resourceFile);
void writeResources(AbstractFile* file, ResourceKey* resources);
void releaseResources(ResourceKey* resources);

NSizResource* readNSiz(ResourceKey* resources);
ResourceKey* writeNSiz(NSizResource* nSiz);
void releaseNSiz(NSizResource* nSiz);

	extern const char* plistHeader;
	extern const char* plistFooter;

ResourceKey* getResourceByKey(ResourceKey* resources, const char* key);
ResourceData* getDataByID(ResourceKey* resource, int id);
ResourceKey* insertData(ResourceKey* resources, const char* key, int id, const char* name, const char* data, size_t dataLength, uint32_t attributes);
ResourceKey* makePlst();
ResourceKey* makeSize(HFSPlusVolumeHeader* volumeHeader);

void flipDriverDescriptorRecord(DriverDescriptorRecord* record, char out);
void flipPartition(Partition* partition, char out, unsigned int BlockSize);
void flipPartitionMultiple(Partition* partition, char multiple, char out, unsigned int BlockSize);

void readDriverDescriptorMap(AbstractFile* file, ResourceKey* resources);
DriverDescriptorRecord* createDriverDescriptorMap(uint32_t numSectors, unsigned int BlockSize);
int writeDriverDescriptorMap(int pNum, AbstractFile* file, DriverDescriptorRecord* DDM, unsigned int BlockSize, ChecksumFunc dataForkChecksum, void* dataForkToken, ResourceKey** resources);
void readApplePartitionMap(AbstractFile* file, ResourceKey* resources, unsigned int BlockSize);
Partition* createApplePartitionMap(uint32_t numSectors, const char* volumeType, unsigned int BlockSize);
int writeApplePartitionMap(int pNum, AbstractFile* file, Partition* partitions, unsigned int BlockSize, ChecksumFunc dataForkChecksum, void* dataForkToken, ResourceKey** resources, NSizResource** nsizIn);
int writeATAPI(int pNum, AbstractFile* file, unsigned int BlockSize, ChecksumFunc dataForkChecksum, void* dataForkToken, ResourceKey** resources, NSizResource** nsizIn);
int writeFreePartition(int pNum, AbstractFile* outFile, uint32_t offset, uint32_t numSectors, ResourceKey** resources);

void extractBLKX(AbstractFile* in, AbstractFile* out, BLKXTable* blkx);
BLKXTable* insertBLKX(AbstractFile* out, AbstractFile* in, uint32_t firstSectorNumber, uint32_t numSectors, uint32_t blocksDescriptor,
            uint32_t checksumType, ChecksumFunc uncompressedChk, void* uncompressedChkToken, ChecksumFunc compressedChk,
            void* compressedChkToken, Volume* volume, int addComment);


int extractDmg(AbstractFile* abstractIn, AbstractFile* abstractOut, int partNum);
int buildDmg(AbstractFile* abstractIn, AbstractFile* abstractOut, unsigned int BlockSize);
int convertToISO(AbstractFile* abstractIn, AbstractFile* abstractOut);
int convertToDMG(AbstractFile* abstractIn, AbstractFile* abstractOut);
#ifdef __cplusplus
}
#endif

#endif
*/