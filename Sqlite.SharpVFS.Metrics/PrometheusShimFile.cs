using System.Runtime.InteropServices;
using Prometheus;
using Sqlite.VFS.DotNet.SQLiteInterop;

namespace Sqlite.SharpVFS.Metrics;

public struct PrometheusShimFile
{
    public IntPtr ShimIOMethods;
}

public class PrometheusShimIOMethods : ISQLiteIOMethods
{
    public int iVersion => 1;
    private Counter fnCallCounter = Prometheus.Metrics.CreateCounter("sharp_vfs_file_calls", "",
            new CounterConfiguration() { LabelNames = ["function"] });
    private Counter ioCounter = Prometheus.Metrics.CreateCounter("sharp_vfs_file_io", "",
            new CounterConfiguration() { LabelNames = ["function"] });

   public int xCheckReservedLock(IntPtr filePtr, out int result)
    {
        fnCallCounter.WithLabels(nameof(xCheckReservedLock)).Inc(1);

        IntPtr underlyingFilePtr = filePtr + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);
        
        return underlyingFileIOMethods.xCheckReservedLock(underlyingFilePtr, out result);
    }

    public int xClose(IntPtr file)
    {   
        fnCallCounter.WithLabels(nameof(xClose)).Inc(1);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);
        
        return underlyingFileIOMethods.xClose(underlyingFilePtr);
    }

    public int xDeviceCharacteristics(IntPtr file)
    {
        fnCallCounter.WithLabels(nameof(xDeviceCharacteristics)).Inc(1);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        return underlyingFileIOMethods.xDeviceCharacteristics(underlyingFilePtr);
    }

    public int xFileControl(IntPtr file, int operation, IntPtr argument)
    {
        fnCallCounter.WithLabels(nameof(xFileControl)).Inc(1);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        return underlyingFileIOMethods.xFileControl(underlyingFilePtr, operation, argument);
    }

    public int xFileSize(IntPtr file, out long size)
    {
        fnCallCounter.WithLabels(nameof(xFileSize)).Inc(1);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        return underlyingFileIOMethods.xFileSize(underlyingFilePtr, out size);
    }

    public int xLock(IntPtr file, int lockType)
    {
        fnCallCounter.WithLabels(nameof(xLock)).Inc(1);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        return underlyingFileIOMethods.xLock(underlyingFilePtr, lockType);
    }

    public int xRead(IntPtr file, IntPtr buffer, int amount, long offset)
    {
        fnCallCounter.WithLabels(nameof(xRead)).Inc(1);
        ioCounter.WithLabels(nameof(xRead)).Inc(amount);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        return underlyingFileIOMethods.xRead(underlyingFilePtr, buffer, amount, offset);
    }

    public int xSectorSize(IntPtr file)
    {
        fnCallCounter.WithLabels(nameof(xSectorSize)).Inc(1);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        return underlyingFileIOMethods.xSectorSize(underlyingFilePtr);
    }

    public int xSync(IntPtr file, int flags)
    {
        fnCallCounter.WithLabels(nameof(xSync)).Inc(1);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        return underlyingFileIOMethods.xSync(underlyingFilePtr, flags);
    }

    public int xTruncate(IntPtr file, long size)
    {
        fnCallCounter.WithLabels(nameof(xTruncate)).Inc(1);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        return underlyingFileIOMethods.xTruncate(underlyingFilePtr, size);
    }

    public int xUnlock(IntPtr file, int lockType)
    {
        fnCallCounter.WithLabels(nameof(xUnlock)).Inc(1);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        return underlyingFileIOMethods.xUnlock(underlyingFilePtr, lockType);
    }

    public int xWrite(IntPtr file, IntPtr buffer, int amount, long offset)
    {
        fnCallCounter.WithLabels(nameof(xWrite)).Inc(1);
        ioCounter.WithLabels(nameof(xWrite)).Inc(amount);

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<PrometheusShimFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);
 
        return underlyingFileIOMethods.xWrite(underlyingFilePtr, buffer, amount, offset);
    }

    // Version 2 methods

    public void xShmBarrier(IntPtr file)
    {
        // Increment the counter for this function
        fnCallCounter.WithLabels(nameof(xShmBarrier)).Inc(1);
        throw new NotImplementedException();
    }

    public int xShmLock(IntPtr file, int offset, int count, int flags)
    {
        // Increment the counter for this function
        fnCallCounter.WithLabels(nameof(xShmLock)).Inc(1);
        throw new NotImplementedException();
    }

    public int xShmMap(IntPtr file, int pageIndex, int pageSize, int flags, out IntPtr pData)
    {
        // Increment the counter for this function
        fnCallCounter.WithLabels(nameof(xShmMap)).Inc(1);
        throw new NotImplementedException();
    }

    public int xShmUnmap(IntPtr file, int deleteFlag)
    {
        // Increment the counter for this function
        fnCallCounter.WithLabels(nameof(xShmUnmap)).Inc(1);
        throw new NotImplementedException();
    }

    // Version 3 methods

    public int xFetch(IntPtr file, long offset, int amount, out IntPtr pp)
    {
        throw new NotImplementedException();
    }

    public int xUnfetch(IntPtr file, long offset, IntPtr p)
    {
        throw new NotImplementedException();
    }
}
