using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sqlite.VFS.DotNet.SQLiteInterop;

public partial class Registration: IDisposable
{
    public HashSet<GCHandle> OpenHandles = new HashSet<GCHandle>();
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

    public void RegisterVFSStruct(object vfs, int makeDflt, IEnumerable<GCHandle> dependencyHandles)
    {
        GCHandle vfsHandle = GCHandle.Alloc(vfs, GCHandleType.Pinned);
        
        OpenHandles.Add(vfsHandle);
        foreach (GCHandle handle in dependencyHandles)
        {
            OpenHandles.Add(handle);
        }

        IntPtr vfsPtr = GCHandle.ToIntPtr(vfsHandle);
        sqlite3_vfs_register(vfsPtr, makeDflt);
    }

    public void Dispose()
    {
        foreach (GCHandle handle in OpenHandles)
        {
            handle.Free();
        }
    }
}