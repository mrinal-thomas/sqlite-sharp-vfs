using System;
using System.Runtime.InteropServices;

namespace Sqlite.VFS.DotNet.SQLiteInterop;

public class SQLiteVFSDelegates
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xOpenDelegate(IntPtr vfs, IntPtr zName, IntPtr file, int flags, IntPtr pOutFlags);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xDeleteDelegate(IntPtr vfs, IntPtr zName, int syncDir);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xAccessDelegate(IntPtr vfs, IntPtr zName, int flags, IntPtr pResOut);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xFullPathnameDelegate(IntPtr vfs, IntPtr zName, int nOut, IntPtr zOut);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr xDlOpenDelegate(IntPtr vfs, IntPtr zFilename);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void xDlErrorDelegate(IntPtr vfs, int nByte, IntPtr zErrMsg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr xDlSymDelegate(IntPtr vfs, IntPtr p, IntPtr zSymbol);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void xDlCloseDelegate(IntPtr vfs, IntPtr p);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xRandomnessDelegate(IntPtr vfs, int nByte, IntPtr zOut);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xSleepDelegate(IntPtr vfs, int microseconds);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xCurrentTimeDelegate(IntPtr vfs, IntPtr pOut);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xGetLastErrorDelegate(IntPtr vfs, int n, IntPtr zErrMsg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xCurrentTimeInt64Delegate(IntPtr vfs, IntPtr pOut);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xSetSystemCallDelegate(IntPtr vfs, IntPtr zName, IntPtr syscallPtr);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr xGetSystemCallDelegate(IntPtr vfs, IntPtr zName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr xNextSystemCallDelegate(IntPtr vfs, IntPtr zName);
}

// Define the VFS structure and methods you need here.
[StructLayout(LayoutKind.Sequential)]
public struct SQLiteVFS
{
    /// <summary>
    /// Structure version number (currently 3).
    /// </summary>
    public int iVersion;

    /// <summary>
    /// Size of subclassed sqlite3_file.
    /// </summary>
    public int szOsFile;

    /// <summary>
    /// Maximum file pathname length.
    /// </summary>
    public int mxPathname;

    /// <summary>
    /// Pointer to the next registered VFS.
    /// </summary>
    public IntPtr pNext;

    /// <summary>
    /// Name of this virtual file system.
    /// </summary>
    [MarshalAs(UnmanagedType.LPUTF8Str)]
    public string? zName;

    /// <summary>
    /// Pointer to application-specific data.
    /// </summary>
    public IntPtr pAppData;

    // Function pointers

    /// <summary>
    /// Function pointer for xOpen.
    /// </summary>
    public SQLiteVFSDelegates.xOpenDelegate xOpen;

    /// <summary>
    /// Function pointer for xDelete.
    /// </summary>
    public SQLiteVFSDelegates.xDeleteDelegate xDelete;

    /// <summary>
    /// Function pointer for xAccess.
    /// </summary>
    public SQLiteVFSDelegates.xAccessDelegate xAccess;

    /// <summary>
    /// Function pointer for xFullPathname.
    /// </summary>
    public SQLiteVFSDelegates.xFullPathnameDelegate xFullPathname;

    /// <summary>
    /// Function pointer for xDlOpen.
    /// </summary>
    public SQLiteVFSDelegates.xDlOpenDelegate xDlOpen;

    /// <summary>
    /// Function pointer for xDlError.
    /// </summary>
    public SQLiteVFSDelegates.xDlErrorDelegate xDlError;

    /// <summary>
    /// Function pointer for xDlSym.
    /// </summary>
    public SQLiteVFSDelegates.xDlSymDelegate xDlSym;

    /// <summary>
    /// Function pointer for xDlClose.
    /// </summary>
    public SQLiteVFSDelegates.xDlCloseDelegate xDlClose;

    /// <summary>
    /// Function pointer for xRandomness.
    /// </summary>
    public SQLiteVFSDelegates.xRandomnessDelegate xRandomness;

    /// <summary>
    /// Function pointer for xSleep.
    /// </summary>
    public SQLiteVFSDelegates.xSleepDelegate xSleep;

    /// <summary>
    /// Function pointer for xCurrentTime.
    /// </summary>
    public SQLiteVFSDelegates.xCurrentTimeDelegate xCurrentTime;

    /// <summary>
    /// Function pointer for xGetLastError.
    /// </summary>
    public SQLiteVFSDelegates.xGetLastErrorDelegate xGetLastError;

#region version-2-and-later-methods

    /// <summary>
    /// Function pointer for xCurrentTimeInt64.
    /// </summary>
    public IntPtr xCurrentTimeInt64;
#endregion 
#region version-3-and-later-methods

    /// <summary>
    /// Function pointer for xSetSystemCall.
    /// </summary>
    public IntPtr xSetSystemCall;

    /// <summary>
    /// Function pointer for xGetSystemCall.
    /// </summary>
    public IntPtr xGetSystemCall;

    /// <summary>
    /// Function pointer for xNextSystemCall.
    /// </summary>
    public IntPtr xNextSystemCall;
#endregion
}
