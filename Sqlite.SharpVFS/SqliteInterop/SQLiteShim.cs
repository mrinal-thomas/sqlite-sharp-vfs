
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Sqlite.VFS.DotNet.SQLiteInterop;

public static class SQLiteVFSShim
{
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
        vfsShimWrapper._shimVFSImpl.xOpen = (IntPtr vfs, IntPtr zName, IntPtr file, int flags, IntPtr pOutFlags) => 
        {
            int rc = underlyingVFS.xOpen(vfsShimWrapper._underlyingVFSPtr, zName, file, flags, pOutFlags);

            string nameStr = Marshal.PtrToStringAnsi(zName);
            Console.WriteLine($"Call to xOpen file {nameStr} with flags 0x{flags:X8} returned {rc}");

            return rc;
        };

        // xDelete Wrapper
        vfsShimWrapper._shimVFSImpl.xDelete = (IntPtr vfs, IntPtr zName, int syncDir) =>
        {
            int rc = underlyingVFS.xDelete(vfsShimWrapper._underlyingVFSPtr, zName, syncDir);

            string nameStr = Marshal.PtrToStringAnsi(zName);
            Console.WriteLine($"Call to xDelete file {nameStr} returned {rc}");

            return rc;
        };

        // xAccess Wrapper
        vfsShimWrapper._shimVFSImpl.xAccess = (IntPtr vfs, IntPtr zName, int flags, IntPtr pOut) =>
        {
            int rc = underlyingVFS.xAccess(vfsShimWrapper._underlyingVFSPtr, zName, flags, pOut);

            string nameStr = Marshal.PtrToStringAnsi(zName);
            Console.WriteLine($"Call to xAccess file {nameStr} with flags 0x{flags:X8} returned {rc}");

            return rc ;
        };

        // xFullPathname Wrapper
        vfsShimWrapper._shimVFSImpl.xFullPathname = (IntPtr vfs, IntPtr zName, int nOut, IntPtr zOut) =>
        {
            int rc = underlyingVFS.xFullPathname(vfsShimWrapper._underlyingVFSPtr, zName, nOut, zOut);
            Console.WriteLine($"Call to xFullPathname file {zName} returned {rc}");
            return rc;
        };

        // xDlOpen Wrapper
        vfsShimWrapper._shimVFSImpl.xDlOpen = (IntPtr vfs, IntPtr zFilename) =>
        {
            IntPtr dlPtr = underlyingVFS.xDlOpen(vfsShimWrapper._underlyingVFSPtr, zFilename);
            Console.WriteLine($"Call to xDlOpen file {zFilename}");
            return dlPtr;
        };

        // xDlError Wrapper
        vfsShimWrapper._shimVFSImpl.xDlError = (IntPtr vfs, int nByte, IntPtr zErrMsg) =>
        {
            underlyingVFS.xDlError(vfsShimWrapper._underlyingVFSPtr, nByte, zErrMsg);
            Console.WriteLine("Call to xDlError");
        };

        // xDlSym Wrapper
        vfsShimWrapper._shimVFSImpl.xDlSym = (IntPtr vfs, IntPtr pDlopen, IntPtr zSymbol) =>
        {
            IntPtr dlPtr = underlyingVFS.xDlSym(vfsShimWrapper._underlyingVFSPtr, pDlopen, zSymbol);
            Console.WriteLine($"Call to xDlSym for symbol {zSymbol}");
            return dlPtr;
        };

        // xDlClose Wrapper
        vfsShimWrapper._shimVFSImpl.xDlClose = (IntPtr vfs, IntPtr pDlopen) =>
        {
            underlyingVFS.xDlClose(vfsShimWrapper._underlyingVFSPtr, pDlopen);
            Console.WriteLine($"Call to xDlClose");
        };

        // xRandomness Wrapper
        vfsShimWrapper._shimVFSImpl.xRandomness = (IntPtr vfs, int nByte, IntPtr zOut) =>
        {
            int rc = underlyingVFS.xRandomness(vfsShimWrapper._underlyingVFSPtr, nByte, zOut);
            Console.WriteLine($"Call to xRandomness for {nByte} bytes returned {rc}");
            return rc;
        };

        // xSleep Wrapper
        vfsShimWrapper._shimVFSImpl.xSleep = (IntPtr vfs, int microseconds) =>
        {
            int rc = underlyingVFS.xSleep(vfsShimWrapper._underlyingVFSPtr, microseconds);
            Console.WriteLine($"Call to xSleep for {microseconds} microseconds returned {rc}");
            return rc;
        };

        // xCurrentTime Wrapper
        vfsShimWrapper._shimVFSImpl.xCurrentTime = (IntPtr vfs, IntPtr pNum) =>
        {
            int rc = underlyingVFS.xCurrentTime(vfsShimWrapper._underlyingVFSPtr, pNum);
            Console.WriteLine($"Call to xCurrentTime returned {rc}");
            return rc;
        };

        // xGetLastError Wrapper
        vfsShimWrapper._shimVFSImpl.xGetLastError = (IntPtr vfs, int nBuf, IntPtr zBuf) =>
        {
            int rc = underlyingVFS.xGetLastError(vfsShimWrapper._underlyingVFSPtr, nBuf, zBuf);
            Console.WriteLine($"Call to xGetLastError returned {rc}");
            return rc;
        };

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