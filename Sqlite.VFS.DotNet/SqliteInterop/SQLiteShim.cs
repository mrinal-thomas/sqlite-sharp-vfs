
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Sqlite.VFS.DotNet.SQLiteInterop;

public static class SQLiteVFSShim
{
    public static SQLiteVFSDelegates.xOpenDelegate? xOpenShim = null;
    public static SQLiteVFSDelegates.xDeleteDelegate? xDeleteShim = null;
    public static SQLiteVFSDelegates.xAccessDelegate? xAccessShim = null;
    public static SQLiteVFSDelegates.xFullPathnameDelegate? xFullPathnameShim = null;
    public static SQLiteVFSDelegates.xDlOpenDelegate? xDlOpenShim = null;
    public static SQLiteVFSDelegates.xDlErrorDelegate? xDlErrorShim = null;
    public static SQLiteVFSDelegates.xDlSymDelegate? xDlSymShim = null;
    public static SQLiteVFSDelegates.xDlCloseDelegate? xDlCloseShim = null;
    public static SQLiteVFSDelegates.xRandomnessDelegate? xRandomnessShim = null;
    public static SQLiteVFSDelegates.xSleepDelegate? xSleepShim = null;
    public static SQLiteVFSDelegates.xCurrentTimeDelegate? xCurrentTimeShim = null;
    public static SQLiteVFSDelegates.xGetLastErrorDelegate? xGetLastErrorShim = null;
    public static SQLiteVFSDelegates.xCurrentTimeInt64Delegate? xCurrentTimeInt64Shim = null;
    public static SQLiteVFSDelegates.xSetSystemCallDelegate? xSetSystemCallShim = null;
    public static SQLiteVFSDelegates.xGetSystemCallDelegate? xGetSystemCallShim = null;
    public static SQLiteVFSDelegates.xNextSystemCallDelegate? xNextSystemCallShim = null;


    public static (SQLiteVFSShimInner, object[]) New(string underlyingVFSName, string newVFSName)
    {
  
        IntPtr vfsPtr = Registration.sqlite3_vfs_find(underlyingVFSName);
        SQLiteVFSShimInner vfsShimWrapper = new SQLiteVFSShimInner();

        vfsShimWrapper._underlyingVFSPtr = vfsPtr;
        SQLiteVFS underlyingVFS = Marshal.PtrToStructure<SQLiteVFS>(vfsShimWrapper._underlyingVFSPtr );

        vfsShimWrapper._shimVFSImpl.szOsFile = underlyingVFS.szOsFile;
        vfsShimWrapper._shimVFSImpl.mxPathname = underlyingVFS.mxPathname;
        vfsShimWrapper._shimVFSImpl.pNext = underlyingVFS.pNext;
        vfsShimWrapper._shimVFSImpl.zName = newVFSName;
        vfsShimWrapper._shimVFSImpl.pAppData = underlyingVFS.pAppData;

        //xOpenWrapper
        xOpenShim = (IntPtr vfs, string zName, IntPtr file, int flags, IntPtr pOutFlags) => 
        {
            int rc = underlyingVFS.xOpen(vfsShimWrapper._underlyingVFSPtr, zName, file, flags, pOutFlags);
            Console.WriteLine($"Call to xOpen file {zName} returned {rc}");
            return rc;
        };
        vfsShimWrapper._shimVFSImpl.xOpen = xOpenShim!;
        // vfsShimWrapper._shimVFSImpl.xOpen = underlyingVFS.xOpen;

        // xDelete Wrapper
        xDeleteShim = (IntPtr vfs, string zName, int syncDir) =>
        {
            int rc = underlyingVFS.xDelete(vfsShimWrapper._underlyingVFSPtr, zName, syncDir);
            Console.WriteLine($"Call to xDelete file {zName} returned {rc}");
            return rc;
        };
        vfsShimWrapper._shimVFSImpl.xDelete = xDeleteShim!;

        // xAccess Wrapper
        xAccessShim = (IntPtr vfs, string zName, int flags, IntPtr pOut) =>
        {
            int rc = underlyingVFS.xAccess(vfsShimWrapper._underlyingVFSPtr, zName, flags, pOut);
            Console.WriteLine($"Call to xAccess file {zName} with flags {flags} returned {rc}");
            return rc ;
        };
        vfsShimWrapper._shimVFSImpl.xAccess = xAccessShim!;

        // xFullPathname Wrapper
        xFullPathnameShim = (IntPtr vfs, string zName, int nOut, IntPtr zOut) =>
        {
            int rc = underlyingVFS.xFullPathname(vfsShimWrapper._underlyingVFSPtr, zName, nOut, zOut);
            Console.WriteLine($"Call to xFullPathname file {zName} returned {rc}");
            return rc;
        };
        vfsShimWrapper._shimVFSImpl.xFullPathname = xFullPathnameShim!;

        // xDlOpen Wrapper
        xDlOpenShim = (IntPtr vfs, string zFilename) =>
        {
            IntPtr dlPtr = underlyingVFS.xDlOpen(vfsShimWrapper._underlyingVFSPtr, zFilename);
            Console.WriteLine($"Call to xDlOpen file {zFilename}");
            return dlPtr;
        };
        vfsShimWrapper._shimVFSImpl.xDlOpen = xDlOpenShim!;

        // xDlError Wrapper
        xDlErrorShim = (IntPtr vfs, int nByte, IntPtr zErrMsg) =>
        {
            underlyingVFS.xDlError(vfsShimWrapper._underlyingVFSPtr, nByte, zErrMsg);
            Console.WriteLine("Call to xDlError");
        };
        vfsShimWrapper._shimVFSImpl.xDlError = xDlErrorShim!;

        // xDlSym Wrapper
        xDlSymShim = (IntPtr vfs, IntPtr pDlopen, string zSymbol) =>
        {
            IntPtr dlPtr = underlyingVFS.xDlSym(vfsShimWrapper._underlyingVFSPtr, pDlopen, zSymbol);
            Console.WriteLine($"Call to xDlSym for symbol {zSymbol}");
            return dlPtr;
        };
        vfsShimWrapper._shimVFSImpl.xDlSym = xDlSymShim!;

        // xDlClose Wrapper
        xDlCloseShim = (IntPtr vfs, IntPtr pDlopen) =>
        {
            underlyingVFS.xDlClose(vfsShimWrapper._underlyingVFSPtr, pDlopen);
            Console.WriteLine($"Call to xDlClose");
        };
        vfsShimWrapper._shimVFSImpl.xDlClose = xDlCloseShim!;

        // xRandomness Wrapper
        xRandomnessShim = (IntPtr vfs, int nByte, IntPtr zOut) =>
        {
            int rc = underlyingVFS.xRandomness(vfsShimWrapper._underlyingVFSPtr, nByte, zOut);
            Console.WriteLine($"Call to xRandomness for {nByte} bytes returned {rc}");
            return rc;
        };
        vfsShimWrapper._shimVFSImpl.xRandomness = xRandomnessShim!;

        // xSleep Wrapper
        xSleepShim = (IntPtr vfs, int microseconds) =>
        {
            int rc = underlyingVFS.xSleep(vfsShimWrapper._underlyingVFSPtr, microseconds);
            Console.WriteLine($"Call to xSleep for {microseconds} microseconds returned {rc}");
            return rc;
        };
        vfsShimWrapper._shimVFSImpl.xSleep = xSleepShim!;

        // xCurrentTime Wrapper
        xCurrentTimeShim = (IntPtr vfs, IntPtr pNum) =>
        {
            int rc = underlyingVFS.xCurrentTime(vfsShimWrapper._underlyingVFSPtr, pNum);
            Console.WriteLine($"Call to xCurrentTime returned {rc}");
            return rc;
        };
        vfsShimWrapper._shimVFSImpl.xCurrentTime = xCurrentTimeShim!;

        // xGetLastError Wrapper
        xGetLastErrorShim = (IntPtr vfs, int nBuf, IntPtr zBuf) =>
        {
            int rc = underlyingVFS.xGetLastError(vfsShimWrapper._underlyingVFSPtr, nBuf, zBuf);
            Console.WriteLine($"Call to xGetLastError returned {rc}");
            return rc;
        };
        vfsShimWrapper._shimVFSImpl.xGetLastError = xGetLastErrorShim!;

        // Version 2 and 3 methods
        vfsShimWrapper._shimVFSImpl.xCurrentTimeInt64 = underlyingVFS.xCurrentTimeInt64;
        vfsShimWrapper._shimVFSImpl.xSetSystemCall = underlyingVFS.xSetSystemCall;
        vfsShimWrapper._shimVFSImpl.xGetSystemCall = underlyingVFS.xGetSystemCall;
        vfsShimWrapper._shimVFSImpl.xNextSystemCall = underlyingVFS.xNextSystemCall;

        return (vfsShimWrapper, new object[] { });
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct SQLiteVFSShimInner
{
    internal SQLiteVFS _shimVFSImpl;
    internal IntPtr _underlyingVFSPtr;
}