using System.Reflection;
using System.Runtime.InteropServices;

namespace Sqlite.VFS.DotNet.SQLiteInterop.Test;

public class FileState
{
    public delegate FileState GetFileState();

    public LoggerShimIOFile IOFile;
    public Dictionary<string, int> MethodCallCounts { get; set; } = new Dictionary<string, int>();

    public FileState(IntPtr ioMethodsPtr)
    {
        IOFile = new LoggerShimIOFile
        {
            ShimIOMethods = ioMethodsPtr,
            GetManagedCodeState = () => this
        };
    }
}

public struct LoggerShimIOFile
{
    public IntPtr ShimIOMethods;

    public FileState.GetFileState GetManagedCodeState;
}

public class LoggerShimIOMethods : ISQLiteIOMethods
{
    public int iVersion => 3;

    public int xCheckReservedLock(IntPtr filePtr, out int result)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(filePtr);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xCheckReservedLock), 0) + 1;
        fileState.MethodCallCounts[nameof(xCheckReservedLock)] = callCount;

        IntPtr underlyingFilePtr = filePtr + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xCheckReservedLock(underlyingFilePtr, out result);
        Console.WriteLine($"Call to {nameof(xCheckReservedLock)} (n={callCount}), rc={rc}, result={result}");
        return rc;
    }

    public int xClose(IntPtr file)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xClose), 0) + 1;
        fileState.MethodCallCounts[nameof(xClose)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xClose(underlyingFilePtr);
        Console.WriteLine($"Call to {nameof(xClose)} (n={callCount}), rc={rc}");
        return rc;
    }

    public int xDeviceCharacteristics(IntPtr file)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xDeviceCharacteristics), 0) + 1;
        fileState.MethodCallCounts[nameof(xDeviceCharacteristics)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xDeviceCharacteristics(underlyingFilePtr);
        Console.WriteLine($"Call to {nameof(xDeviceCharacteristics)} (n={callCount}), rc={rc}");
        return rc;
    }

    public int xFileControl(IntPtr file, int operation, IntPtr argument)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xFileControl), 0) + 1;
        fileState.MethodCallCounts[nameof(xFileControl)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xFileControl(underlyingFilePtr, operation, argument);
        Console.WriteLine($"Call to {nameof(xFileControl)} (n={callCount}), rc={rc}");
        return rc;
    }

    public int xFileSize(IntPtr file, out long size)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xFileSize), 0) + 1;
        fileState.MethodCallCounts[nameof(xFileSize)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xFileSize(underlyingFilePtr, out size);
        Console.WriteLine($"Call to {nameof(xFileSize)} (n={callCount}), rc={rc}, size={size}");
        return rc;
    }

    public int xLock(IntPtr file, int lockType)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xLock), 0) + 1;
        fileState.MethodCallCounts[nameof(xLock)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xLock(underlyingFilePtr, lockType);
        Console.WriteLine($"Call to {nameof(xLock)} (n={callCount}), rc={rc}");
        return rc;
    }

    public int xRead(IntPtr file, IntPtr buffer, int amount, long offset)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xRead), 0) + 1;
        fileState.MethodCallCounts[nameof(xRead)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xRead(underlyingFilePtr, buffer, amount, offset);
        Console.WriteLine($"Call to {nameof(xRead)} (n={callCount}), rc={rc}");
        return rc;
    }

    public int xSectorSize(IntPtr file)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xSectorSize), 0) + 1;
        fileState.MethodCallCounts[nameof(xSectorSize)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xSectorSize(underlyingFilePtr);
        Console.WriteLine($"Call to {nameof(xSectorSize)} (n={callCount}), rc={rc}");
        return rc;
    }

    public int xSync(IntPtr file, int flags)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xSync), 0) + 1;
        fileState.MethodCallCounts[nameof(xSync)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xSync(underlyingFilePtr, flags);
        Console.WriteLine($"Call to {nameof(xSync)} (n={callCount}), rc={rc}");
        return rc;
    }

    public int xTruncate(IntPtr file, long size)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xTruncate), 0) + 1;
        fileState.MethodCallCounts[nameof(xTruncate)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xTruncate(underlyingFilePtr, size);
        Console.WriteLine($"Call to {nameof(xTruncate)} (n={callCount}), rc={rc}");
        return rc;
    }

    public int xUnlock(IntPtr file, int lockType)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xUnlock), 0) + 1;
        fileState.MethodCallCounts[nameof(xUnlock)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xUnlock(underlyingFilePtr, lockType);
        Console.WriteLine($"Call to {nameof(xUnlock)} (n={callCount}), rc={rc}");
        return rc;
    }

    public int xWrite(IntPtr file, IntPtr buffer, int amount, long offset)
    {
        LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(file);
        FileState fileState = shimIOFile.GetManagedCodeState();
        int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xWrite), 0) + 1;
        fileState.MethodCallCounts[nameof(xWrite)] = callCount;

        IntPtr underlyingFilePtr = file + Marshal.SizeOf<LoggerShimIOFile>();
        IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
        SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

        int rc = underlyingFileIOMethods.xWrite(underlyingFilePtr, buffer, amount, offset);
        Console.WriteLine($"Call to {nameof(xWrite)} (n={callCount}), rc={rc}");
        return rc;
    }

    // Version 2

    public void xShmBarrier(IntPtr file)
    {
        throw new NotImplementedException();
    }

    public int xShmLock(IntPtr file, int offset, int count, int flags)
    {
        throw new NotImplementedException();
    }

    public int xShmMap(IntPtr file, int pageIndex, int pageSize, int flags, out IntPtr pData)
    {
        throw new NotImplementedException();
    }

    public int xShmUnmap(IntPtr file, int deleteFlag)
    {
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

// public class LoggerShimIOMethods : ISQLiteIOMethods
// {
//     public int iVersion => 3;

//     public int xCheckReservedLock(IntPtr filePtr, out int result)
//     {
//         LoggerShimIOFile shimIOFile = Marshal.PtrToStructure<LoggerShimIOFile>(filePtr);
//         FileState fileState = shimIOFile.GetManagedCodeState();
//         int callCount = fileState.MethodCallCounts.GetValueOrDefault(nameof(xCheckReservedLock), 0) + 1;
//         fileState.MethodCallCounts[nameof(xCheckReservedLock)] = callCount;

//         IntPtr underlyingFilePtr = filePtr + Marshal.SizeOf<LoggerShimIOFile>();
//         IntPtr underlyingFileIOMethodsPtr = Marshal.PtrToStructure<IntPtr>(underlyingFilePtr);
//         SQLiteIOMethods underlyingFileIOMethods = Marshal.PtrToStructure<SQLiteIOMethods>(underlyingFileIOMethodsPtr);

//         int rc = underlyingFileIOMethods.xCheckReservedLock(underlyingFilePtr, out result);
//         Console.WriteLine($"Call to {nameof(xCheckReservedLock)} (n={callCount}), rc={rc}, result={result}");
//         return rc;
//     }

//     public int xClose(IntPtr file)
//     {
//         throw new NotImplementedException();
//     }

//     public int xDeviceCharacteristics(IntPtr file)
//     {
//         throw new NotImplementedException();
//     }

//     public int xFileControl(IntPtr file, int operation, IntPtr argument)
//     {
//         throw new NotImplementedException();
//     }

//     public int xFileSize(IntPtr file, out long size)
//     {
//         throw new NotImplementedException();
//     }

//     public int xLock(IntPtr file, int lockType)
//     {
//         throw new NotImplementedException();
//     }

//     public int xRead(IntPtr file, IntPtr buffer, int amount, long offset)
//     {
//         throw new NotImplementedException();
//     }

//     public int xSectorSize(IntPtr file)
//     {
//         throw new NotImplementedException();
//     }

//     public int xSync(IntPtr file, int flags)
//     {
//         throw new NotImplementedException();
//     }

//     public int xTruncate(IntPtr file, long size)
//     {
//         throw new NotImplementedException();
//     }

//     public int xUnlock(IntPtr file, int lockType)
//     {
//         throw new NotImplementedException();
//     }

//     public int xWrite(IntPtr file, IntPtr buffer, int amount, long offset)
//     {
//         throw new NotImplementedException();
//     }

//     // Version 2

//     public void xShmBarrier(IntPtr file)
//     {
//         throw new NotImplementedException();
//     }

//     public int xShmLock(IntPtr file, int offset, int count, int flags)
//     {
//         throw new NotImplementedException();
//     }

//     public int xShmMap(IntPtr file, int pageIndex, int pageSize, int flags, out IntPtr pData)
//     {
//         throw new NotImplementedException();
//     }

//     public int xShmUnmap(IntPtr file, int deleteFlag)
//     {
//         throw new NotImplementedException();
//     }

//     // Version 3 methods

//     public int xFetch(IntPtr file, long offset, int amount, out IntPtr pp)
//     {
//         throw new NotImplementedException();
//     }

//     public int xUnfetch(IntPtr file, long offset, IntPtr p)
//     {
//         throw new NotImplementedException();
//     }
// }