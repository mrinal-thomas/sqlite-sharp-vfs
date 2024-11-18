using System;

namespace Sqlite.VFS.DotNet.SQLiteInterop
{
    /// <summary>
    /// Interface defining the method signatures for the SQLiteIO functions.
    /// </summary>
    public interface ISQLiteIOMethods
    {
        /// <summary>
        /// Structure version number.
        /// </summary>
        public int iVersion { get; }

        // Function signatures for IO operations

        /// <summary>
        /// Function signature for xClose.
        /// </summary>
        public int xClose(IntPtr file);

        /// <summary>
        /// Function signature for xRead.
        /// </summary>
        public int xRead(IntPtr file, IntPtr buffer, int amount, long offset);

        /// <summary>
        /// Function signature for xWrite.
        /// </summary>
        public int xWrite(IntPtr file, IntPtr buffer, int amount, long offset);

        /// <summary>
        /// Function signature for xTruncate.
        /// </summary>
        public int xTruncate(IntPtr file, long size);

        /// <summary>
        /// Function signature for xSync.
        /// </summary>
        public int xSync(IntPtr file, int flags);

        /// <summary>
        /// Function signature for xFileSize.
        /// </summary>
        public int xFileSize(IntPtr file, out long size);

        /// <summary>
        /// Function signature for xLock.
        /// </summary>
        public int xLock(IntPtr file, int lockType);

        /// <summary>
        /// Function signature for xUnlock.
        /// </summary>
        public int xUnlock(IntPtr file, int lockType);

        /// <summary>
        /// Function signature for xCheckReservedLock.
        /// </summary>
        public int xCheckReservedLock(IntPtr file, out int result);

        /// <summary>
        /// Function signature for xFileControl.
        /// </summary>
        public int xFileControl(IntPtr file, int operation, IntPtr argument);

        /// <summary>
        /// Function signature for xSectorSize.
        /// </summary>
        public int xSectorSize(IntPtr file);

        /// <summary>
        /// Function signature for xDeviceCharacteristics.
        /// </summary>
        public int xDeviceCharacteristics(IntPtr file);

        // Version 2 methods

        /// <summary>
        /// Function signature for xShmMap.
        /// </summary>
        public int xShmMap(IntPtr file, int pageIndex, int pageSize, int flags, out IntPtr pData);

        /// <summary>
        /// Function signature for xShmLock.
        /// </summary>
        public int xShmLock(IntPtr file, int offset, int count, int flags);

        /// <summary>
        /// Function signature for xShmBarrier.
        /// </summary>
        public void xShmBarrier(IntPtr file);

        /// <summary>
        /// Function signature for xShmUnmap.
        /// </summary>
        public int xShmUnmap(IntPtr file, int deleteFlag);

        // Version 3 methods

        /// <summary>
        /// Function signature for xFetch.
        /// </summary>
        public int xFetch(IntPtr file, long offset, int amount, out IntPtr pp);

        /// <summary>
        /// Function signature for xUnfetch.
        /// </summary>
        public int xUnfetch(IntPtr file, long offset, IntPtr p);
    }
}