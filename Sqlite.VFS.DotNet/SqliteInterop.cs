using System;
using System.Runtime.InteropServices;

namespace Sqlite.VFS.DotNet;

public static class SQLiteInterop
{
    const string SQLiteDll = "sqlite3";

    [DllImport(SQLiteDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern int sqlite3_vfs_register(IntPtr vfs, int makeDflt);

    public class Delegates
    {
        public delegate int xOpenDelegate(IntPtr vfs, string zName, IntPtr file, int flags, IntPtr pOutFlags);
        public delegate int xDeleteDelegate(IntPtr vfs, string zName, int syncDir);
        public delegate int xAccessDelegate(IntPtr vfs, string zName, int flags, IntPtr pResOut);
        public delegate int xFullPathnameDelegate(IntPtr vfs, string zName, int nOut, IntPtr zOut);
        public delegate IntPtr xDlOpenDelegate(IntPtr vfs, string zFilename);
        public delegate void xDlErrorDelegate(IntPtr vfs, int nByte, IntPtr zErrMsg);
        public delegate IntPtr xDlSymDelegate(IntPtr vfs, IntPtr p, string zSymbol);
        public delegate void xDlCloseDelegate(IntPtr vfs, IntPtr p);
        public delegate int xRandomnessDelegate(IntPtr vfs, int nByte, IntPtr zOut);
        public delegate int xSleepDelegate(IntPtr vfs, int microseconds);
        public delegate int xCurrentTimeDelegate(IntPtr vfs, IntPtr pOut);
        public delegate int xGetLastErrorDelegate(IntPtr vfs, int n, IntPtr zErrMsg);
        public delegate int xCurrentTimeInt64Delegate(IntPtr vfs, IntPtr pOut);
        public delegate int xSetSystemCallDelegate(IntPtr vfs, string zName, IntPtr syscallPtr);
        public delegate IntPtr xGetSystemCallDelegate(IntPtr vfs, string zName);
        public delegate IntPtr xNextSystemCallDelegate(IntPtr vfs, string zName);
    }

    // Define the VFS structure and methods you need here.
    [StructLayout(LayoutKind.Sequential)]
    public struct SQLiteVFS
    {
        /// <summary>
        /// Structure version number (currently 3)
        /// </summary>
        public int iVersion = 3;            /* Structure version number (currently 3) */
        public int szOsFile;            /* Size of subclassed sqlite3_file */
        public int mxPathname;          /* Maximum file pathname length */
        public IntPtr pNext;      /* Next registered VFS */
        public IntPtr zName;       /* Name of this virtual file system */
        
        public IntPtr pAppData;        /* Pointer to application-specific data */

        // Function pointers
        public IntPtr xOpen;           /* Function pointer for xOpen */
        public IntPtr xDelete;         /* Function pointer for xDelete */
        public IntPtr xAccess;         /* Function pointer for xAccess */
        public IntPtr xFullPathname;   /* Function pointer for xFullPathname */
        public IntPtr xDlOpen;         /* Function pointer for xDlOpen */
        public IntPtr xDlError;        /* Function pointer for xDlError */
        public IntPtr xDlSym;          /* Function pointer for xDlSym */
        public IntPtr xDlClose;        /* Function pointer for xDlClose */
        public IntPtr xRandomness;     /* Function pointer for xRandomness */
        public IntPtr xSleep;          /* Function pointer for xSleep */
        public IntPtr xCurrentTime;    /* Function pointer for xCurrentTime */
        public IntPtr xGetLastError;   /* Function pointer for xGetLastError */
        
        // Version 2 and later methods
        public IntPtr xCurrentTimeInt64; /* Function pointer for xCurrentTimeInt64 */

        // Version 3 and greater methods
        public IntPtr xSetSystemCall;  /* Function pointer for xSetSystemCall */
        public IntPtr xGetSystemCall;  /* Function pointer for xGetSystemCall */
        public IntPtr xNextSystemCall;  /* Function pointer for xNextSystemCall */
    }
}

typedef struct sqlite3_vfs sqlite3_vfs;
typedef void (*sqlite3_syscall_ptr)(void);
struct sqlite3_vfs {
    /// <summary>
    /// Structure version number.
    /// </summary>
    int iVersion = 3;

    /// <summary>
    /// Size of subclassed sqlite3_file
    /// </summary>
    int szOsFile = 0;

    /// <summary>
    /// Maximum file pathname length
    /// </summary>
    int mxPathname = 1024;

    /// <summary>
    /// Next registered VFS. Managed by SQLite.
    /// </summary>
    sqlite3_vfs *pNext;
    
    /// <summary>
    /// Name of this virtual file system.
    /// </summary>
    const char *zName;

    /// <summary>
    /// Pointer to application-specific data
    /// </summary>
    void *pAppData;
    
    int (*xOpen)(sqlite3_vfs*, sqlite3_filename zName, sqlite3_file*,
                int flags, int *pOutFlags);
    int (*xDelete)(sqlite3_vfs*, const char *zName, int syncDir);
    int (*xAccess)(sqlite3_vfs*, const char *zName, int flags, int *pResOut);
    int (*xFullPathname)(sqlite3_vfs*, const char *zName, int nOut, char *zOut);
    void *(*xDlOpen)(sqlite3_vfs*, const char *zFilename);
    void (*xDlError)(sqlite3_vfs*, int nByte, char *zErrMsg);
    void (*(*xDlSym)(sqlite3_vfs*,void*, const char *zSymbol))(void);
    void (*xDlClose)(sqlite3_vfs*, void*);
    int (*xRandomness)(sqlite3_vfs*, int nByte, char *zOut);
    int (*xSleep)(sqlite3_vfs*, int microseconds);
    int (*xCurrentTime)(sqlite3_vfs*, double*);
    int (*xGetLastError)(sqlite3_vfs*, int, char *);
    /*
        The methods above are in version 1 of the sqlite_vfs object
        definition.  Those that follow are added in version 2 or later
    */
    int (*xCurrentTimeInt64)(sqlite3_vfs*, sqlite3_int64*);
    
    /*
        The methods above are in versions 1 and 2 of the sqlite_vfs object.
        Those below are for version 3 and greater. These aren't used by SQLite core,
        but rather are intended for testing purposes
    */
    int (*xSetSystemCall)(sqlite3_vfs*, const char *zName, sqlite3_syscall_ptr);
    sqlite3_syscall_ptr (*xGetSystemCall)(sqlite3_vfs*, const char *zName);
    const char *(*xNextSystemCall)(sqlite3_vfs*, const char *zName);
    /*
        The methods above are in versions 1 through 3 of the sqlite_vfs object.
        New fields may be appended in future versions.  The iVersion
        value will increment whenever this happens.
    */
};