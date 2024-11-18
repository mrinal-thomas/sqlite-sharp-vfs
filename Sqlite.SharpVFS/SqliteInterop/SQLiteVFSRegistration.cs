using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sqlite.VFS.DotNet.SQLiteInterop;

public partial class Registration : IDisposable
{
    public HashSet<SQLiteVFS> RegisteredVFS = new HashSet<SQLiteVFS>();
    public HashSet<SQLiteIOMethods> RegisteredIOMethods = new HashSet<SQLiteIOMethods>();
    public HashSet<IntPtr> AllocatedMemory = new HashSet<IntPtr>();

    const string SQLiteDll = "e_sqlite3";

    //[LibraryImport(SQLiteDll)]
    //[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    //public static partial int sqlite3_vfs_register(IntPtr vfs, int makeDflt);

    //[LibraryImport(SQLiteDll)]
    //[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    //private static partial int sqlite3_vfs_unregister(IntPtr vfs);

    //[LibraryImport(SQLiteDll, StringMarshalling = StringMarshalling.Utf8)]
    //[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    //public static partial IntPtr sqlite3_vfs_find(string zVfsName);

    [DllImport(SQLiteDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int sqlite3_vfs_register(IntPtr vfs, int makeDflt);

    [DllImport(SQLiteDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr sqlite3_vfs_find(string zVfsName);

    public void RegisterVFSStruct(ISQLiteVFS vfs, int makeDflt)
    {
        SQLiteVFS vfsStructInManagedMem = new SQLiteVFS()
        {
            // Basic properties
            iVersion = vfs.iVersion,
            szOsFile = vfs.szOsFile,
            mxPathname = vfs.mxPathname,
            zName = vfs.zName,
            pAppData = vfs.pAppData,

            // Function pointers 
            xOpen = vfs.xOpen,
            xDelete = vfs.xDelete,
            xAccess = vfs.xAccess,
            xFullPathname = vfs.xFullPathname,
            xDlOpen = vfs.xDlOpen,
            xDlError = vfs.xDlError,
            xDlSym = vfs.xDlSym,
            xDlClose = vfs.xDlClose,
            xRandomness = vfs.xRandomness,
            xSleep = vfs.xSleep,
            xCurrentTime = vfs.xCurrentTime,
            xGetLastError = vfs.xGetLastError,

            // Version-2-and-later methods
            xCurrentTimeInt64 = vfs.xCurrentTimeInt64,

            // Version-3-and-later methods
            xSetSystemCall = vfs.xSetSystemCall,
            xGetSystemCall = vfs.xGetSystemCall,
            xNextSystemCall = vfs.xNextSystemCall
        };
        RegisteredVFS.Add(vfsStructInManagedMem);

        IntPtr vfsStructUnmanagedMem = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(SQLiteVFS)));
        Marshal.StructureToPtr(vfsStructInManagedMem, vfsStructUnmanagedMem, true);
        sqlite3_vfs_register(vfsStructUnmanagedMem, makeDflt);

        AllocatedMemory.Add(vfsStructUnmanagedMem);
    }

    public IntPtr RegisterIOMethods(ISQLiteIOMethods ioMethods)
    {
        SQLiteIOMethods ioMethodsStructInManagedMem = new SQLiteIOMethods()
        {
            iVersion = ioMethods.iVersion,

            // Function pointers for the I/O methods
            xClose = ioMethods.xClose,
            xRead = ioMethods.xRead,
            xWrite = ioMethods.xWrite,
            xTruncate = ioMethods.xTruncate,
            xSync = ioMethods.xSync,
            xFileSize = ioMethods.xFileSize,
            xLock = ioMethods.xLock,
            xUnlock = ioMethods.xUnlock,
            xCheckReservedLock = ioMethods.xCheckReservedLock,
            xFileControl = ioMethods.xFileControl,
            xSectorSize = ioMethods.xSectorSize,
            xDeviceCharacteristics = ioMethods.xDeviceCharacteristics,

            // Version-2 methods
            xShmMap = ioMethods.xShmMap,
            xShmLock = ioMethods.xShmLock,
            xShmBarrier = ioMethods.xShmBarrier,
            xShmUnmap = ioMethods.xShmUnmap,

            // Version-3 methods
            xFetch = ioMethods.xFetch,
            xUnfetch = ioMethods.xUnfetch
        };
        RegisteredIOMethods.Add(ioMethodsStructInManagedMem);

        IntPtr ioMethodsStructUnmanagedMem = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(SQLiteIOMethods)));
        Marshal.StructureToPtr(ioMethodsStructInManagedMem, ioMethodsStructUnmanagedMem, true);
        AllocatedMemory.Add(ioMethodsStructUnmanagedMem);
        return ioMethodsStructUnmanagedMem;
    }

    public void Dispose()
    {
        foreach (IntPtr memPtr in AllocatedMemory)
        {
            Marshal.FreeCoTaskMem(memPtr);
        }
    }
}