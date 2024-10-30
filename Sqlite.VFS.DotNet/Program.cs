using System.Runtime.InteropServices;
using Microsoft.Data.Sqlite;
using Sqlite.VFS.DotNet.SQLiteInterop;
using SQLitePCL;

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

            public sqlitepcl_vfs(){}
		}

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            // See https://aka.ms/new-console-template for more information
            IntPtr vfsPtr = Registration.sqlite3_vfs_find("unix");
            SQLiteVFS vfs;
            if (vfsPtr != IntPtr.Zero)
            {
                // Convert the pointer to the SQLiteVFS struct
                vfs = Marshal.PtrToStructure<SQLiteVFS>(vfsPtr);
                
                // TODO: compare this with the find vfs from SQLitePCL!!!
                
                // Use the fields of the vfs struct as needed
                Console.WriteLine("VFS Name: " + vfs.zName); // Assuming zName is a char* in C
            }

            SQLitePCL.Batteries.Init();
            (SQLiteVFSShimInner shimVFS, object[] refs) = SQLiteVFSShim.New("unix", "my-custom-vfs");

            int len = Marshal.SizeOf(typeof(SQLiteVFS));
            int expectedMem = Marshal.SizeOf(typeof(sqlitepcl_vfs));
            IntPtr vfs_mem = Marshal.AllocCoTaskMem(len);
            try
            {
                Marshal.StructureToPtr(shimVFS._shimVFSImpl, vfs_mem, true);
                Registration.sqlite3_vfs_register(vfs_mem, 1);

                RunDbTest();
            }
            finally
            {
                Marshal.FreeCoTaskMem(vfs_mem);
            }
            // using Registration vfsRegistration = new Registration();
            // vfsRegistration.RegisterVFSStruct(shimVFS, 0, []);
        }

        private static void RunDbTest()
        {
            Console.WriteLine($"Creating file at {Directory.GetCurrentDirectory()}");
            using SqliteConnection connection = new SqliteConnection("Data Source=file:test.sqlite?vfs=my-custom-vfs");
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
