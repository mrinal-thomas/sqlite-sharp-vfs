using System.Runtime.InteropServices;
using Prometheus;
using Sqlite.VFS.DotNet.SQLiteInterop;

namespace Sqlite.SharpVFS.Metrics;

public interface ICounterFactory
{
    public ICounter CreateCounter<TLabels>(TLabels labels);
}

public interface ICounter
{

}

public class PrometheusShimVFS : ISQLiteVFS
{
    private IntPtr _underlyingVFSPtr;
    private SQLiteVFS _underlyingVFS;
    private IntPtr _prometheusShimIOMethods;
    private Counter fnCallCounter = Prometheus.Metrics.CreateCounter("sharp_vfs_calls", "",
            new CounterConfiguration() { LabelNames = ["function", "filename"] });

    public PrometheusShimVFS(IntPtr underlyingVFSPtr, IntPtr prometheusShimIOMethods)
    {
        _underlyingVFSPtr = underlyingVFSPtr;
        _underlyingVFS = Marshal.PtrToStructure<SQLiteVFS>(underlyingVFSPtr);
        _prometheusShimIOMethods = prometheusShimIOMethods;
    }

    public int iVersion => _underlyingVFS.iVersion;

    public int szOsFile => Marshal.SizeOf(typeof(PrometheusShimFile)) + _underlyingVFS.szOsFile;

    public int mxPathname => _underlyingVFS.mxPathname;

    public string? zName => "prometheus-shim-vfs";

    public nint pAppData => _underlyingVFS.pAppData;

    public int xOpen(IntPtr vfs, IntPtr zName, IntPtr file, int flags, IntPtr pOutFlags)
    {
        string nameStr = Marshal.PtrToStringAnsi(zName)!;
        fnCallCounter.WithLabels(nameof(xOpen), nameStr).Inc(1);
        
        PrometheusShimFile ioFile = new PrometheusShimFile()
        {
            ShimIOMethods = _prometheusShimIOMethods
        };
        Marshal.StructureToPtr(ioFile, file, false);

        IntPtr underlyingFile = file + Marshal.SizeOf(typeof(PrometheusShimFile));
        return _underlyingVFS.xOpen(_underlyingVFSPtr, zName, underlyingFile, flags, pOutFlags);
    }

    public int xDelete(IntPtr vfs, IntPtr zName, int syncDir)
    {
        string? nameStr = Marshal.PtrToStringAnsi(zName);
        fnCallCounter.WithLabels(nameof(xDelete), nameStr ?? "null").Inc(1);
        
        return _underlyingVFS.xDelete(_underlyingVFSPtr, zName, syncDir);;
    }

    public int xAccess(IntPtr vfs, IntPtr zName, int flags, IntPtr pResOut)
    {
        string? nameStr = Marshal.PtrToStringAnsi(zName);
        fnCallCounter.WithLabels(nameof(xAccess), nameStr ?? "null").Inc(1);

        return _underlyingVFS.xAccess(_underlyingVFSPtr, zName, flags, pResOut);
    }

    public int xFullPathname(IntPtr vfs, IntPtr zName, int nOut, IntPtr zOut)
    {
        string? nameStr = Marshal.PtrToStringAnsi(zName);
        fnCallCounter.WithLabels(nameof(xFullPathname), nameStr ?? "null").Inc(1);

        return _underlyingVFS.xFullPathname(_underlyingVFSPtr, zName, nOut, zOut);
    }

    public IntPtr xDlOpen(IntPtr vfs, IntPtr zFilename)
    {
        string? filenameStr = Marshal.PtrToStringAnsi(zFilename);
        fnCallCounter.WithLabels(nameof(xDlOpen), filenameStr ?? "null").Inc(1);

        return _underlyingVFS.xDlOpen(_underlyingVFSPtr, zFilename);
    }

    public void xDlError(IntPtr vfs, int nByte, IntPtr zErrMsg)
    {
        fnCallCounter.WithLabels(nameof(xOpen), "null").Inc(1);
        _underlyingVFS.xDlError(_underlyingVFSPtr, nByte, zErrMsg);
    }

    public IntPtr xDlSym(IntPtr vfs, IntPtr pDlopen, IntPtr zSymbol)
    {
        fnCallCounter.WithLabels(nameof(xDlSym), "null").Inc(1);
        return _underlyingVFS.xDlSym(_underlyingVFSPtr, pDlopen, zSymbol);
    }

    public void xDlClose(IntPtr vfs, IntPtr pDlopen)
    {
        fnCallCounter.WithLabels(nameof(xDlClose), "null").Inc(1);
        _underlyingVFS.xDlClose(_underlyingVFSPtr, pDlopen);
    }

    public int xRandomness(IntPtr vfs, int nByte, IntPtr zOut)
    {
        fnCallCounter.WithLabels(nameof(xRandomness), "null").Inc(1);
        return _underlyingVFS.xRandomness(_underlyingVFSPtr, nByte, zOut);
    }

    public int xSleep(IntPtr vfs, int microseconds)
    {
        fnCallCounter.WithLabels(nameof(xSleep), "null").Inc(1);
        return _underlyingVFS.xSleep(_underlyingVFSPtr, microseconds);
    }

    public int xCurrentTime(IntPtr vfs, IntPtr pNum)
    {
        fnCallCounter.WithLabels(nameof(xCurrentTime), "null").Inc(1);
        return _underlyingVFS.xCurrentTime(_underlyingVFSPtr, pNum);
    }

    public int xGetLastError(IntPtr vfs, int nBuf, IntPtr zBuf)
    {
        fnCallCounter.WithLabels(nameof(xGetLastError), "null").Inc(1);
        return _underlyingVFS.xGetLastError(_underlyingVFSPtr, nBuf, zBuf);
    }

    public int xCurrentTimeInt64(IntPtr vfs, IntPtr pOut)
    {
        fnCallCounter.WithLabels(nameof(xCurrentTimeInt64), "null").Inc(1);
        return _underlyingVFS.xCurrentTimeInt64(_underlyingVFSPtr, pOut);
    }

    public int xSetSystemCall(IntPtr vfs, IntPtr zName, IntPtr syscallPtr)
    {
        fnCallCounter.WithLabels(nameof(xSetSystemCall), "null").Inc(1);
        return _underlyingVFS.xSetSystemCall(_underlyingVFSPtr, zName, syscallPtr);
    }

    public IntPtr xGetSystemCall(IntPtr vfs, IntPtr zName)
    {
        fnCallCounter.WithLabels(nameof(xGetSystemCall), "null").Inc(1);
        return _underlyingVFS.xGetSystemCall(_underlyingVFSPtr, zName);
    }

    public IntPtr xNextSystemCall(IntPtr vfs, IntPtr zName)
    {
        fnCallCounter.WithLabels(nameof(xNextSystemCall), "null").Inc(1);
        return _underlyingVFS.xNextSystemCall(_underlyingVFSPtr, zName);
    }
}

