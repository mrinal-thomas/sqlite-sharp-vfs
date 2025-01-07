using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Prometheus;
using Sqlite.VFS.DotNet.SQLiteInterop;

namespace Sqlite.SharpVFS.Metrics.Test;

[TestClass]
public class PrometheusShimVFSTests
{
    private const string DB_FILENAME = "test.sqlite";
    private const string JOURNAL_NAME = DB_FILENAME + "-journal";
    private const string WAL_NAME = DB_FILENAME + "-wal";

    Dictionary<string, string[]> expectedVFSOperations = new Dictionary<string, string[]>()
    {
        {"xOpen", [DB_FILENAME, JOURNAL_NAME]},
        {"xAccess", [JOURNAL_NAME, WAL_NAME]},
        {"xDelete", [JOURNAL_NAME]},
        {"xFullPathname", [DB_FILENAME]}
    };
    
    string[] expectedFileOperations = 
    [
        "xDeviceCharacteristics",
        "xRead",
        "xFileControl",
        "xLock",
        "xFileSize",
        "xUnlock",
        "xWrite",
        "xSync",
        "xClose"
    ];

    Registration vfsRegistration;
    Counter vfsFnCallsCounter = Prometheus.Metrics.CreateCounter("sharp_vfs_calls", "",
            new CounterConfiguration() { LabelNames = ["function", "filename"] });
    Counter fileFnCallsCounter =  Prometheus.Metrics.CreateCounter("sharp_vfs_file_calls", "",
            new CounterConfiguration() { LabelNames = ["function"] });
    Counter fileIOCounter = Prometheus.Metrics.CreateCounter("sharp_vfs_file_io", "",
            new CounterConfiguration() { LabelNames = ["function"] });
    
    [TestMethod]
    public async Task TestMetricsAreLogged()
    {
        await PerformDBOperations();
        GC.Collect();

        foreach ((string vfsOperation, string[] filenames) in expectedVFSOperations)
        {
            foreach (string file in filenames)
            {
                AssertLabelSetExists(vfsFnCallsCounter, LabelEndsWithFn(vfsOperation, file), GreaterThanFn(0),
                    $"Expected at least 1 call to {vfsOperation} on {file}");
            }
        }

        foreach (string fileOperation in expectedFileOperations)
        {
            AssertLabelSetExists(fileFnCallsCounter, LabelEndsWithFn(fileOperation), null,
                $"Expected at least 1 call to {fileFnCallsCounter}");
        }

        long fileSize = new System.IO.FileInfo(DB_FILENAME).Length;
        double writeAmount = fileIOCounter.WithLabels("xWrite").Value;
        Assert.IsTrue(writeAmount >= fileSize,
            $"{DB_FILENAME} is {fileSize} bytes large, but metric only measured {writeAmount} written");
    }

    private async Task PerformDBOperations()
    {
        Console.WriteLine($"Creating file at {Directory.GetCurrentDirectory()}");
        using SqliteConnection connection = new SqliteConnection($"Data Source=file:{DB_FILENAME}?vfs=prometheus-shim-vfs&mode=rwc");
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
        }
        Console.WriteLine("Insert result = " + result);

        connection.Close();
        connection.Dispose();

        Console.WriteLine("Done");
        await Task.Delay(5);
    }

    private Func<string[], bool> LabelEndsWithFn(params string?[] suffixes)
    {
        return (string[] labels) => labels.Zip(suffixes)
            .All(pair => pair.Second == null ? true : pair.First.EndsWith(pair.Second));
    }

    private Func<double, bool> GreaterThanFn(int threshhold)
    {
        return (double val) => val > threshhold;
    }

    private void AssertLabelSetExists(Counter counter, Func<string[], bool>? labelsCriteria,
        Func<double, bool>? valueCriteria, string message)
    {
        Assert.IsTrue(counter.GetAllLabelValues()
            .Where(labels => labelsCriteria == null ? true : labelsCriteria(labels))
            .Where(labels => valueCriteria == null ? true : valueCriteria(counter.WithLabels(labels).Value))
            .Any(),
        message);
    }

    public PrometheusShimVFSTests()
    {
        vfsRegistration = new Registration();
        PrometheusShimIOMethods ioMethods = new PrometheusShimIOMethods();
        IntPtr ioMethodsPtr = vfsRegistration.RegisterIOMethods(ioMethods);

        IntPtr unixVfsPtr = Registration.sqlite3_vfs_find("unix");
        PrometheusShimVFS loggerShimVFS = new PrometheusShimVFS(unixVfsPtr, ioMethodsPtr);
        vfsRegistration.RegisterVFSStruct(loggerShimVFS, 1);
    }
}