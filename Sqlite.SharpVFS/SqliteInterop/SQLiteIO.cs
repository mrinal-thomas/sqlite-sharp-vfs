using System;
using System.Runtime.InteropServices;

namespace Sqlite.VFS.DotNet.SQLiteInterop;

// Delegate definitions for the method pointers
public class SQLiteIODelegates
{
    public delegate int xCloseDelegate(IntPtr file);
    public delegate int xReadDelegate(IntPtr file, IntPtr buffer, int amount, long offset);
    public delegate int xWriteDelegate(IntPtr file, IntPtr buffer, int amount, long offset);
    public delegate int xTruncateDelegate(IntPtr file, long size);
    public delegate int xSyncDelegate(IntPtr file, int flags);
    public delegate int xFileSizeDelegate(IntPtr file, out long size);
    public delegate int xLockDelegate(IntPtr file, int lockType);
    public delegate int xUnlockDelegate(IntPtr file, int lockType);
    public delegate int xCheckReservedLockDelegate(IntPtr file, out int result);
    public delegate int xFileControlDelegate(IntPtr file, int operation, IntPtr argument);
    public delegate int xSectorSizeDelegate(IntPtr file);
    public delegate int xDeviceCharacteristicsDelegate(IntPtr file);

    // Version 2 methods
    public delegate int xShmMapDelegate(IntPtr file, int pageIndex, int pageSize, int flags, out IntPtr pData);
    public delegate int xShmLockDelegate(IntPtr file, int offset, int count, int flags);
    public delegate void xShmBarrierDelegate(IntPtr file);
    public delegate int xShmUnmapDelegate(IntPtr file, int deleteFlag);

    // Version 3 methods
    public delegate int xFetchDelegate(IntPtr file, long offset, int amount, out IntPtr pp);
    public delegate int xUnfetchDelegate(IntPtr file, long offset, IntPtr p);
}

// Define the IO methods structure
[StructLayout(LayoutKind.Sequential)]
public struct SQLiteIOMethods
{
    /// <summary>
    /// Structure version number.
    /// </summary>
    public int iVersion;

    /// <summary>
    /// Function pointer for xClose.
    /// </summary>
    public IntPtr xClose;

    /// <summary>
    /// Function pointer for xRead.
    /// </summary>
    public IntPtr xRead;

    /// <summary>
    /// Function pointer for xWrite.
    /// </summary>
    public IntPtr xWrite;

    /// <summary>
    /// Function pointer for xTruncate.
    /// </summary>
    public IntPtr xTruncate;

    /// <summary>
    /// Function pointer for xSync.
    /// </summary>
    public IntPtr xSync;

    /// <summary>
    /// Function pointer for xFileSize.
    /// </summary>
    public IntPtr xFileSize;

    /// <summary>
    /// Function pointer for xLock.
    /// </summary>
    public IntPtr xLock;

    /// <summary>
    /// Function pointer for xUnlock.
    /// </summary>
    public IntPtr xUnlock;

    /// <summary>
    /// Function pointer for xCheckReservedLock.
    /// </summary>
    public IntPtr xCheckReservedLock;

    /// <summary>
    /// Function pointer for xFileControl.
    /// </summary>
    public IntPtr xFileControl;

    /// <summary>
    /// Function pointer for xSectorSize.
    /// </summary>
    public IntPtr xSectorSize;

    /// <summary>
    /// Function pointer for xDeviceCharacteristics.
    /// </summary>
    public IntPtr xDeviceCharacteristics;

    // Version 2 methods
    public IntPtr xShmMap;
    public IntPtr xShmLock;
    public IntPtr xShmBarrier;
    public IntPtr xShmUnmap;

    // Version 3 methods
    public IntPtr xFetch;
    public IntPtr xUnfetch;
}
