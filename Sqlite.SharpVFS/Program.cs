using System.Runtime.InteropServices;
using Microsoft.Data.Sqlite;
using Sqlite.VFS.DotNet.SQLiteInterop;

namespace Sqlite.VFS.DotNet
{
    public class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct sqlitepcl_vfs
        {
            public int iVersion = 3;
            public int szOsFile;
            public int mxPathname;
            public IntPtr pNext;

            [MarshalAs(UnmanagedType.LPUTF8Str)]
            public string zName;
            public IntPtr pAppData;
            public IntPtr xOpen;
            public SQLiteDeleteDelegate xDelete;
            public IntPtr xAccess;
            public IntPtr xFullPathname;
            public IntPtr xDlOpen;
            public IntPtr xDlError;
            public IntPtr xDlSym;
            public IntPtr xDlClose;
            public IntPtr xRandomness;
            public IntPtr xSleep;
            public IntPtr xCurrentTime;
            public IntPtr xGetLastError;

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public unsafe delegate int SQLiteDeleteDelegate(IntPtr pVfs, byte* zName, int syncDir);

            public sqlitepcl_vfs() { }
        }

        public static void Main(string[] args)
        {
            // SQLitePCL.Batteries.Init();

            using Registration vfsRegistration = new Registration();

            IntPtr unixVfsPtr = Registration.sqlite3_vfs_find("unix");
            LoggerShimVFS loggerShimVFS = new LoggerShimVFS(unixVfsPtr);
            vfsRegistration.RegisterVFSStruct(loggerShimVFS, 1);
            GC.Collect();
            RunDbTest();
        }

        private static void RunDbTest()
        {
            Console.WriteLine($"Creating file at {Directory.GetCurrentDirectory()}");
            using SqliteConnection connection = new SqliteConnection("Data Source=file:test.sqlite?vfs=my-custom-vfs&mode=rwc");
            connection.Open();

            int result;
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS People (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Address TEXT NOT NULL,
                    Job TEXT NOT NULL
                )";
            using (var command = new SqliteCommand(createTableQuery, connection))
            {
                result = command.ExecuteNonQuery();
                Console.WriteLine("Create table result = " + result);
            }

            string insertQuery = @"
                INSERT INTO People (Name, Address, Job) VALUES
                ('Alice Johnson', '123 Maple St, Springfield', 'Engineer'),
                ('Bob Smith', '456 Oak St, Metropolis', 'Designer'),
                ('Charlie Brown', '789 Pine St, Gotham', 'Writer')";
            using (var command = new SqliteCommand(insertQuery, connection))
            {
                result = command.ExecuteNonQuery();
                Console.WriteLine("Insert result = " + result);
            }

            connection.Close();
        }
    }
}
