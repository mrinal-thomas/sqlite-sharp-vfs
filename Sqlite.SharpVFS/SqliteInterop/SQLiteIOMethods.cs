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
    public SQLiteIODelegates.xCloseDelegate xClose;

    /// <summary>
    /// Function pointer for xRead.
    /// </summary>
    public SQLiteIODelegates.xReadDelegate xRead;

    /// <summary>
    /// Function pointer for xWrite.
    /// </summary>
    public SQLiteIODelegates.xWriteDelegate xWrite;

    /// <summary>
    /// Function pointer for xTruncate.
    /// </summary>
    public SQLiteIODelegates.xTruncateDelegate xTruncate;

    /// <summary>
    /// Function pointer for xSync.
    /// </summary>
    public SQLiteIODelegates.xSyncDelegate xSync;

    /// <summary>
    /// Function pointer for xFileSize.
    /// </summary>
    public SQLiteIODelegates.xFileSizeDelegate xFileSize;

    /// <summary>
    /// Function pointer for xLock.
    /// </summary>
    public SQLiteIODelegates.xLockDelegate xLock;

    /// <summary>
    /// Function pointer for xUnlock.
    /// </summary>
    public SQLiteIODelegates.xUnlockDelegate xUnlock;

    /// <summary>
    /// Function pointer for xCheckReservedLock.
    /// </summary>
    public SQLiteIODelegates.xCheckReservedLockDelegate xCheckReservedLock;

    /// <summary>
    /// Function pointer for xFileControl.
    /// </summary>
    public SQLiteIODelegates.xFileControlDelegate xFileControl;

    /// <summary>
    /// Function pointer for xSectorSize.
    /// </summary>
    public SQLiteIODelegates.xSectorSizeDelegate xSectorSize;

    /// <summary>
    /// Function pointer for xDeviceCharacteristics.
    /// </summary>
    public SQLiteIODelegates.xDeviceCharacteristicsDelegate xDeviceCharacteristics;

    // Version 2 methods
    public SQLiteIODelegates.xShmMapDelegate xShmMap;
    public SQLiteIODelegates.xShmLockDelegate xShmLock;
    public SQLiteIODelegates.xShmBarrierDelegate xShmBarrier;
    public SQLiteIODelegates.xShmUnmapDelegate xShmUnmap;

    // Version 3 methods
    public SQLiteIODelegates.xFetchDelegate xFetch;
    public SQLiteIODelegates.xUnfetchDelegate xUnfetch;
}
