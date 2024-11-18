using System.Runtime.InteropServices;

namespace Sqlite.VFS.DotNet.SQLiteInterop.Test;

public class LoggerShimVFS : ISQLiteVFS
{
    private struct IOMethodsInternal
    {
        public IntPtr IOMethodsPtr;
    }

    IntPtr _underlyingVFSPtr;
    SQLiteVFS _underlyingVFS;
    IntPtr _loggerShimIOMethods;
    Dictionary<string, FileState> _files = new Dictionary<string, FileState>();

    public int iVersion => _underlyingVFS.iVersion;

    public int szOsFile => Marshal.SizeOf(typeof(LoggerShimIOFile)) + _underlyingVFS.szOsFile;

    public int mxPathname => _underlyingVFS.szOsFile;

    public string? zName => "my-custom-vfs";

    public IntPtr pAppData => _underlyingVFS.pAppData;

    // xOpen Wrapper
    public int xOpen(IntPtr vfs, IntPtr zName, IntPtr file, int flags, IntPtr pOutFlags)
    {
        string nameStr = Marshal.PtrToStringAnsi(zName)!;
        
        FileState fileState = new FileState(_loggerShimIOMethods);
        Marshal.StructureToPtr(fileState.IOFile, file, false);
        _files[nameStr] = fileState;

        IntPtr underlyingFile = file + Marshal.SizeOf(typeof(LoggerShimIOFile));
        int rc = _underlyingVFS.xOpen(_underlyingVFSPtr, zName, underlyingFile, flags, pOutFlags);

        Console.WriteLine($"Call to xOpen file {nameStr} with flags 0x{flags:X8} returned {rc}");

        return rc;
    }

    // xDelete Wrapper
    public int xDelete(IntPtr vfs, IntPtr zName, int syncDir)
    {
        int rc = _underlyingVFS.xDelete(_underlyingVFSPtr, zName, syncDir);

        string? nameStr = Marshal.PtrToStringAnsi(zName);
        Console.WriteLine($"Call to xDelete file {nameStr} returned {rc}");

        return rc;
    }

    // xAccess Wrapper
    public int xAccess(IntPtr vfs, IntPtr zName, int flags, IntPtr pResOut)
    {
        int rc = _underlyingVFS.xAccess(_underlyingVFSPtr, zName, flags, pResOut);

        string? nameStr = Marshal.PtrToStringAnsi(zName);
        Console.WriteLine($"Call to xAccess file {nameStr} with flags 0x{flags:X8} returned {rc}");

        return rc;
    }

    // xFullPathname Wrapper
    public int xFullPathname(IntPtr vfs, IntPtr zName, int nOut, IntPtr zOut)
    {
        int rc = _underlyingVFS.xFullPathname(_underlyingVFSPtr, zName, nOut, zOut);

        string? nameStr = Marshal.PtrToStringAnsi(zName);
        Console.WriteLine($"Call to xFullPathname for file {nameStr} returned {rc}");

        return rc;
    }

    // xDlOpen Wrapper
    public IntPtr xDlOpen(IntPtr vfs, IntPtr zFilename)
    {
        IntPtr dlPtr = _underlyingVFS.xDlOpen(_underlyingVFSPtr, zFilename);

        string? filenameStr = Marshal.PtrToStringAnsi(zFilename);
        Console.WriteLine($"Call to xDlOpen for file {filenameStr}");

        return dlPtr;
    }

    // xDlError Wrapper
    public void xDlError(IntPtr vfs, int nByte, IntPtr zErrMsg)
    {
        _underlyingVFS.xDlError(_underlyingVFSPtr, nByte, zErrMsg);
        Console.WriteLine("Call to xDlError");
    }

    // xDlSym Wrapper
    public IntPtr xDlSym(IntPtr vfs, IntPtr pDlopen, IntPtr zSymbol)
    {
        IntPtr dlPtr = _underlyingVFS.xDlSym(_underlyingVFSPtr, pDlopen, zSymbol);

        string? symbolStr = Marshal.PtrToStringAnsi(zSymbol);
        Console.WriteLine($"Call to xDlSym for symbol {symbolStr}");

        return dlPtr;
    }

    // xDlClose Wrapper
    public void xDlClose(IntPtr vfs, IntPtr pDlopen)
    {
        _underlyingVFS.xDlClose(_underlyingVFSPtr, pDlopen);
        Console.WriteLine("Call to xDlClose");
    }

    // xRandomness Wrapper
    public int xRandomness(IntPtr vfs, int nByte, IntPtr zOut)
    {
        int rc = _underlyingVFS.xRandomness(_underlyingVFSPtr, nByte, zOut);
        Console.WriteLine($"Call to xRandomness for {nByte} bytes returned {rc}");

        return rc;
    }

    // xSleep Wrapper
    public int xSleep(IntPtr vfs, int microseconds)
    {
        int rc = _underlyingVFS.xSleep(_underlyingVFSPtr, microseconds);
        Console.WriteLine($"Call to xSleep for {microseconds} microseconds returned {rc}");

        return rc;
    }

    // xCurrentTime Wrapper
    public int xCurrentTime(IntPtr vfs, IntPtr pNum)
    {
        int rc = _underlyingVFS.xCurrentTime(_underlyingVFSPtr, pNum);
        Console.WriteLine("Call to xCurrentTime returned " + rc);

        return rc;
    }

    // xGetLastError Wrapper
    public int xGetLastError(IntPtr vfs, int nBuf, IntPtr zBuf)
    {
        int rc = _underlyingVFS.xGetLastError(_underlyingVFSPtr, nBuf, zBuf);
        Console.WriteLine($"Call to xGetLastError returned {rc}");

        return rc;
    }

    // Version 2 and 3 methods (direct forwarding)
    public int xCurrentTimeInt64(IntPtr vfs, IntPtr pOut)
    {
        return _underlyingVFS.xCurrentTimeInt64(_underlyingVFSPtr, pOut);
    }

    public int xSetSystemCall(IntPtr vfs, IntPtr zName, IntPtr syscallPtr)
    {
        return _underlyingVFS.xSetSystemCall(_underlyingVFSPtr, zName, syscallPtr);
    }

    public IntPtr xGetSystemCall(IntPtr vfs, IntPtr zName)
    {
        return _underlyingVFS.xGetSystemCall(_underlyingVFSPtr, zName);
    }

    public IntPtr xNextSystemCall(IntPtr vfs, IntPtr zName)
    {
        return _underlyingVFS.xNextSystemCall(_underlyingVFSPtr, zName);
    }

    public LoggerShimVFS(IntPtr underlyingVFSPtr, IntPtr loggerShimIOMethods)
    {
        _underlyingVFSPtr = underlyingVFSPtr;
        _underlyingVFS = Marshal.PtrToStructure<SQLiteVFS>(underlyingVFSPtr);
        _loggerShimIOMethods = loggerShimIOMethods;
    }
}