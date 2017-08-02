using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionCache.Storage
{
    public class StorageFile
    {
        public bool Initialized { get; set; }

        // File Info
        public String Name { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }

        // Memory GC
        public StorageFile Previous { get; set; }
        public StorageFile Next { get; set; }

        public VirtualDirectory MemoryDirectory { get; set; }

        private long AppendPosition;

        public StorageFile(long size)
        {
            Name = "";
            Size = size;
            Data = new byte[size];

            Previous = null;
            Next = null;

            MemoryDirectory = null;

            AppendPosition = 0;
        }

        public StorageFile(byte[] data)
        {
            Name = "";
            Size = data.LongLength;
            Data = data;

            Previous = null;
            Next = null;

            MemoryDirectory = null;
        }

        public void Remove()
        {
            MemoryDirectory.RemoveFile(this);

            return;
        }

        public void Append(byte[] data, bool finished)
        {
            lock (this)
            {
                long appendLength = data.LongLength;
                long newAppendPosition = appendLength + AppendPosition;
                long readPosition = 0;

                // Check if data array can support new data
                if (appendLength + AppendPosition > Data.LongLength)
                {
                    // Restructure data array
                    byte[] newArray = new byte[newAppendPosition];

                    for (long x = 0; x < Size; x++)
                    {
                        newArray[x] = Data[x];
                    }

                    Data = newArray;
                }

                // Copy data over to object
                for (long x = AppendPosition; x < newAppendPosition; x++)
                {
                    Data[x] = data[readPosition];
                }

                // Set new append position and size
                AppendPosition = newAppendPosition;
                Size += appendLength;

                // Set initialized state
                Initialized = finished;
            }

            return;
        }

        public void Load(Stream stream, int bufferSize)
        {
            lock (this)
            {
                Queue<byte[]> byteQueue = new Queue<byte[]>();
                byte[] buffer = new byte[bufferSize];
                byte[] lastBuffer = new byte[bufferSize];
                int read;
                int lastBufferSize = 0;
                long bufferCount = 0;
                long writePosition = 0;

                // Read stream into queue
                while ((read = stream.Read(buffer, 0, bufferSize)) > 0)
                {
                    if (read == bufferSize)
                    {
                        byteQueue.Enqueue(buffer);
                        bufferCount++;
                    }
                    else
                    {
                        lastBuffer = buffer;
                        lastBufferSize = read;
                    }
                }

                // Set new size
                Size = bufferCount * bufferSize + lastBufferSize;
                AppendPosition = Size;

                // Create new byte array
                Data = new byte[Size];

                // Copy buffers into byte array
                for (int x = 0; x < bufferCount; x++)
                {
                    buffer = byteQueue.Dequeue();

                    for (int y = 0; y < bufferSize; y++)
                    {
                        Data[writePosition] = buffer[y];
                    }
                }

                // Copy last buffer into byte array
                for (int x = 0; x < lastBufferSize; x++)
                {
                    Data[writePosition] = lastBuffer[x];
                }
            }

            return;
        }

        public StorageReadState Read(out byte[] buffer, long bufferSize, long offset, out long newOffset)
        {
            StorageReadState result = StorageReadState.Continue;
            buffer = new byte[bufferSize];
            long bufferPosition = 0;

            lock (this)
            {
                newOffset = Size;

                if (offset + bufferSize >= Size)
                {
                    if (Initialized)
                    {
                        result = StorageReadState.Finished;
                    }
                    else
                    {
                        result = StorageReadState.Initializing;
                    }
                }
                else
                {
                    if (!Initialized)
                    {
                        result = StorageReadState.Initializing;
                    }

                    newOffset = offset + bufferSize;
                }

                for (long x = offset; x < newOffset; x++)
                {
                    buffer[bufferPosition] = Data[x];

                    bufferPosition++;
                }
            }

            return result;
        }
    }

    public enum StorageReadState
    {
        Initializing = 0,
        Continue,
        Finished
    }
}
