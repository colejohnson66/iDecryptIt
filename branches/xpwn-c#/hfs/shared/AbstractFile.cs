/* =============================================================================
 * File:   AbstractFile.cs
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

namespace Xpwn
{
    class AbstractFile
    {

    }
}
/*
#ifndef ABSTRACTFILE_H
#define ABSTRACTFILE_H

#include "common.h"
#include <stdint.h>

typedef struct AbstractFile AbstractFile;
typedef struct AbstractFile2 AbstractFile2;

typedef size_t (*WriteFunc)(AbstractFile* file, const void* data, size_t len);
typedef size_t (*ReadFunc)(AbstractFile* file, void* data, size_t len);
typedef int (*SeekFunc)(AbstractFile* file, off_t offset);
typedef off_t (*TellFunc)(AbstractFile* file);
typedef void (*CloseFunc)(AbstractFile* file);
typedef off_t (*GetLengthFunc)(AbstractFile* file);
typedef void (*SetKeyFunc)(AbstractFile2* file, const unsigned int* key, const unsigned int* iv);

typedef enum AbstractFileType {
	AbstractFileTypeFile,
	AbstractFileType8900,
	AbstractFileTypeImg2,
	AbstractFileTypeImg3,
	AbstractFileTypeLZSS,
	AbstractFileTypeIBootIM,
	AbstractFileTypeMem,
	AbstractFileTypeMemFile,
	AbstractFileTypeDummy
} AbstractFileType;

struct AbstractFile {
	void* data;
	WriteFunc write;
	ReadFunc read;
	SeekFunc seek;
	TellFunc tell;
	GetLengthFunc getLength;
	CloseFunc close;
	AbstractFileType type;
};

struct AbstractFile2 {
	AbstractFile super;
	SetKeyFunc setKey;
};


typedef struct {
	size_t offset;
	void** buffer;
	size_t bufferSize;
} MemWrapperInfo;

typedef struct {
	size_t offset;
	void** buffer;
	size_t* bufferSize;
	size_t actualBufferSize;
} MemFileWrapperInfo;

#ifdef __cplusplus
extern "C" {
#endif
	AbstractFile* createAbstractFileFromFile(FILE* file);
	AbstractFile* createAbstractFileFromDummy();
	AbstractFile* createAbstractFileFromMemory(void** buffer, size_t size);
	AbstractFile* createAbstractFileFromMemoryFile(void** buffer, size_t* size);
	AbstractFile* createAbstractFileFromMemoryFileBuffer(void** buffer, size_t* size, size_t actualBufferSize);
	void abstractFilePrint(AbstractFile* file, const char* format, ...);
	io_func* IOFuncFromAbstractFile(AbstractFile* file);
#ifdef __cplusplus
}
#endif

#endif

*/