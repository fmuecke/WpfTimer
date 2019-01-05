using System;
using System.IO;

namespace WpfTimer
{
    // https://stackoverflow.com/questions/400140/how-do-i-automatically-delete-tempfiles-in-c
    internal sealed class TempFile : IDisposable
    {
        private string path;

        public TempFile() : this(System.IO.Path.GetTempFileName())
        {
        }

        public TempFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            this.path = path;
        }

        ~TempFile()
        {
            Dispose(false);
        }

        public string Path
        {
            get
            {
                if (path == null)
                {
                    throw new ObjectDisposedException(GetType().Name);
                }

                return path;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
            if (path != null)
            {
                try { File.Delete(path); }
                catch { } // best effort
                path = null;
            }
        }
    }
}