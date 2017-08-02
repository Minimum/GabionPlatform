using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabionCache.Storage
{
    public class MemoryStorage
    {
        public long MaxSize
        {
            get
            {
                return SizeLimit;
            }

            set
            {
                SizeLimit = value;

                GarbageCollect();
            }
        }

        protected long SizeLimit;
        protected long Size;

        protected StorageFile FirstFile;
        protected StorageFile LastFile;

        protected VirtualDirectory Cache;

        public void GarbageCollect()
        {
            GarbageCollect(Size);

            return;
        }

        public bool GarbageCollect(long size)
        {
            long sizeDelta = SizeLimit - size;

            while (sizeDelta < 0 && FirstFile != null)
            {
                StorageFile file = FirstFile;

                sizeDelta += file.Size;
                Size -= file.Size;

                FirstFile = file.Next;

                file.Remove();
            }

            return sizeDelta >= 0;
        }

        public bool FileExists(String path)
        {
            bool found = false;
            String[] pathing = path.Split('/', '\\');
            VirtualDirectory currentDir = Cache;

            for (int x = 0; x < pathing.Length-1; x++)
            {
                currentDir = currentDir.GetDirectory(pathing[x]);

                if (currentDir == null)
                {
                    break;
                }
            }

            if (currentDir != null)
            {
                found = currentDir.GetFile(pathing[pathing.Length - 1]) != null;
            }

            return found;
        }

        public StorageFile GetFile(String path)
        {
            StorageFile file = null;
            String[] pathing = path.Split('/', '\\');
            VirtualDirectory currentDir = Cache;

            for (int x = 0; x < pathing.Length - 1; x++)
            {
                currentDir = currentDir.GetDirectory(pathing[x]);

                if (currentDir == null)
                {
                    break;
                }
            }

            if (currentDir != null)
            {
                file = currentDir.GetFile(pathing[pathing.Length - 1]);
            }

            return file;
        }

        public bool AddFile(String path, StorageFile file)
        {
            String[] pathing = path.Split('/', '\\');
            VirtualDirectory currentDir = Cache;
            bool success = false;

            // Move through virtual pathing
            for (int x = 0; x < pathing.Length - 1; x++)
            {
                VirtualDirectory nextDir = currentDir.GetDirectory(pathing[x]);

                // Check if next dir exists
                if (nextDir == null)
                {
                    VirtualDirectory newDir = new VirtualDirectory();
                    newDir.Name = pathing[x];

                    // Create next dir
                    if (currentDir.AddDirectory(newDir))
                    {
                        nextDir = newDir;
                    }
                    else if ((nextDir = currentDir.GetDirectory(pathing[x])) == null)
                    {
                        currentDir = null;

                        break;
                    }
                }

                currentDir = nextDir;
            }

            if (currentDir != null)
            {
                // Set file name
                file.Name = pathing[pathing.Length - 1];

                // Add file
                success = currentDir.AddFile(file);
            }

            return success;
        }

        public void RefreshFile(StorageFile file)
        {
            lock (FirstFile)
            {
                // Bring file out of GC queue
                if (file.Previous != null)
                    file.Previous.Next = file.Next;

                if (file.Next != null)
                    file.Next.Previous = file.Previous;

                // Insert file into end of GC queue
                file.Previous = LastFile;
                LastFile.Next = file;

                LastFile = file;
            }

            return;
        }

        public void AddFileGarbage(StorageFile file)
        {
            lock (FirstFile)
            {
                if (GarbageCollect(Size + file.Size))
                {
                    Size += file.Size;

                    file.Previous = LastFile;
                    LastFile.Next = file;

                    LastFile = file;
                }
            }

            return;
        }
    }
}
