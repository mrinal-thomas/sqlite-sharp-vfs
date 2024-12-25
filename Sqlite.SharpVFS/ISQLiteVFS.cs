namespace Sqlite.VFS.DotNet.SQLiteInterop;

public interface ISQLiteVFS
{
    /// <summary>
    /// Structure version number (currently 3).
    /// </summary>
    public int iVersion { get; }

    /// <summary>
    /// Size of subclassed sqlite3_file.
    /// </summary>
    public int szOsFile { get; }

    /// <summary>
    /// Maximum file pathname length.
    /// </summary>
    public int mxPathname { get; }

    /// <summary>
    /// Name of this virtual file system.
    /// </summary>
    public string? zName { get; }

    /// <summary>
    /// Pointer to application-specific data.
    /// </summary>
    public IntPtr pAppData { get; }

    // Function signatures for methods in the VFS interface
    #region version-1-methods

    /// <summary>
    /// Function signature for xOpen.
    /// </summary>
    public int xOpen(IntPtr vfs, IntPtr zName, IntPtr file, int flags, IntPtr pOutFlags);

    /// <summary>
    /// Function signature for xDelete.
    /// </summary>
    public int xDelete(IntPtr vfs, IntPtr zName, int syncDir);

    /// <summary>
    /// Function signature for xAccess.
    /// </summary>
    public int xAccess(IntPtr vfs, IntPtr zName, int flags, IntPtr pResOut);

    /// <summary>
    /// Function signature for xFullPathname.
    /// </summary>
    public int xFullPathname(IntPtr vfs, IntPtr zName, int nOut, IntPtr zOut);

    /// <summary>
    /// Function signature for xDlOpen.
    /// </summary>
    public IntPtr xDlOpen(IntPtr vfs, IntPtr zFilename);

    /// <summary>
    /// Function signature for xDlError.
    /// </summary>
    public void xDlError(IntPtr vfs, int nByte, IntPtr zErrMsg);

    /// <summary>
    /// Function signature for xDlSym.
    /// </summary>
    public IntPtr xDlSym(IntPtr vfs, IntPtr p, IntPtr zSymbol);

    /// <summary>
    /// Function signature for xDlClose.
    /// </summary>
    public void xDlClose(IntPtr vfs, IntPtr p);

    /// <summary>
    /// Function signature for xRandomness.
    /// </summary>
    public int xRandomness(IntPtr vfs, int nByte, IntPtr zOut);

    /// <summary>
    /// Function signature for xSleep.
    /// </summary>
    public int xSleep(IntPtr vfs, int microseconds);

    /// <summary>
    /// Function signature for xCurrentTime.
    /// </summary>
    public int xCurrentTime(IntPtr vfs, IntPtr pOut);

    /// <summary>
    /// Function signature for xGetLastError.
    /// </summary>
    public int xGetLastError(IntPtr vfs, int n, IntPtr zErrMsg);

    #endregion
    #region version-2-and-later-methods

    /// <summary>
    /// Function signature for xCurrentTimeInt64.
    /// </summary>
    public int xCurrentTimeInt64(IntPtr vfs, IntPtr pOut);

    #endregion
    #region version-3-and-later-methods

    /// <summary>
    /// Function signature for xSetSystemCall.
    /// </summary>
    public int xSetSystemCall(IntPtr vfs, IntPtr zName, IntPtr syscallPtr);

    /// <summary>
    /// Function signature for xGetSystemCall.
    /// </summary>
    public IntPtr xGetSystemCall(IntPtr vfs, IntPtr zName);

    /// <summary>
    /// Function signature for xNextSystemCall.
    /// </summary>
    public IntPtr xNextSystemCall(IntPtr vfs, IntPtr zName);

    #endregion
}