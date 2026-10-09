#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using Microting.KanbanBase.Infrastructure.Enums;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

/// <summary>
/// Covers the Chrome-extension capture schema change (20261009000000_ChromeExtensionCaptureSchema),
/// the same way <see cref="UserbackSyncSchemaTests"/> covers the schema change before it.
/// </summary>
[TestFixture]
public class ChromeExtensionCaptureSchemaTests
{
    // ---------------------------------------------------------------------------------------
    // Persisted enum values. CardConsoleLogs.Level is an int column, so renumbering a member
    // silently reinterprets every existing row. New members must be APPENDED.
    // ---------------------------------------------------------------------------------------

    [Test]
    public void ConsoleLogLevel_ExistingValuesAreNotRenumbered()
    {
        Assert.Multiple(() =>
        {
            Assert.That((int)ConsoleLogLevel.Log, Is.EqualTo(0));
            Assert.That((int)ConsoleLogLevel.Info, Is.EqualTo(1));
            Assert.That((int)ConsoleLogLevel.Warn, Is.EqualTo(2));
            Assert.That((int)ConsoleLogLevel.Error, Is.EqualTo(3));
            Assert.That((int)ConsoleLogLevel.Debug, Is.EqualTo(4));
        });
    }

    [Test]
    public void ConsoleLogLevel_NewCdpValuesAreAppended()
    {
        Assert.Multiple(() =>
        {
            // CDP Log.entryAdded level "verbose".
            Assert.That((int)ConsoleLogLevel.Verbose, Is.EqualTo(5));
            // CDP Runtime.exceptionThrown, which carries no console level of its own.
            Assert.That((int)ConsoleLogLevel.Exception, Is.EqualTo(6));
        });
    }

    [Test]
    public void ConsoleLogLevel_IsContiguousFromZero()
    {
        // A gap or a duplicate here means somebody assigned a value by hand rather than
        // appending, which is the shape renumbering takes in practice.
        int[] values = Enum.GetValues<ConsoleLogLevel>().Cast<int>().OrderBy(v => v).ToArray();

        Assert.That(values, Is.EqualTo(Enumerable.Range(0, values.Length).ToArray()));
    }

    // ---------------------------------------------------------------------------------------
    // The pre-existing writer (UserbackImportService) sets exactly CardId, Level, Message,
    // Source and Timestamp. Those five must keep their names and CLR types or it stops
    // compiling against the published package.
    // ---------------------------------------------------------------------------------------

    [Test]
    public void CardConsoleLog_KeepsTheFiveFieldsTheUserbackImporterAssigns()
    {
        Assert.Multiple(() =>
        {
            AssertPropertyType<CardConsoleLog>(nameof(CardConsoleLog.CardId), typeof(int));
            AssertPropertyType<CardConsoleLog>(nameof(CardConsoleLog.Level), typeof(ConsoleLogLevel));
            AssertPropertyType<CardConsoleLog>(nameof(CardConsoleLog.Message), typeof(string));
            AssertPropertyType<CardConsoleLog>(nameof(CardConsoleLog.Source), typeof(string));
            AssertPropertyType<CardConsoleLog>(nameof(CardConsoleLog.Timestamp), typeof(DateTime?));
        });
    }

    [Test]
    public void CardConsoleLog_NewCdpFieldsAreNullableAndDefaultToNull()
    {
        // Nullable so every row the Userback importer already writes stays valid, and so a CDP
        // event that carries only some of these does not need filler values.
        var log = new CardConsoleLog();

        Assert.Multiple(() =>
        {
            Assert.That(log.StackTrace, Is.Null);
            Assert.That(log.LineNumber, Is.Null);
            Assert.That(log.ColumnNumber, Is.Null);
            Assert.That(log.ArgsJson, Is.Null);
            Assert.That(log.RequestId, Is.Null);

            AssertPropertyType<CardConsoleLog>(nameof(CardConsoleLog.StackTrace), typeof(string));
            AssertPropertyType<CardConsoleLog>(nameof(CardConsoleLog.LineNumber), typeof(int?));
            AssertPropertyType<CardConsoleLog>(nameof(CardConsoleLog.ColumnNumber), typeof(int?));
            AssertPropertyType<CardConsoleLog>(nameof(CardConsoleLog.ArgsJson), typeof(string));
            AssertPropertyType<CardConsoleLog>(nameof(CardConsoleLog.RequestId), typeof(string));
        });
    }

    // ---------------------------------------------------------------------------------------
    // CardNetworkLog shape invariants.
    // ---------------------------------------------------------------------------------------

    [Test]
    public void CardNetworkLog_EverythingButCardIdIsOptional()
    {
        // Which CDP events a request produced depends on how it ended: a failed request has no
        // StatusCode, a cache hit has no wire timings, a request still in flight at Stop has only
        // the requestWillBeSent half. A non-nullable column anywhere here would force the client
        // to invent a value.
        var required = new[] { nameof(CardNetworkLog.CardId) };

        foreach (PropertyInfo prop in DeclaredScalarProperties(typeof(CardNetworkLog)))
        {
            if (required.Contains(prop.Name))
            {
                continue;
            }

            bool optional = Nullable.GetUnderlyingType(prop.PropertyType) != null
                            || prop.PropertyType == typeof(string);

            Assert.That(optional, Is.True,
                $"CardNetworkLog.{prop.Name} is a non-nullable {prop.PropertyType.Name}; every " +
                "captured field must be optional.");
        }
    }

    [Test]
    public void CardNetworkLog_BodyFlagsExistSoEmptyIsDistinguishableFromDropped()
    {
        var log = new CardNetworkLog();

        Assert.Multiple(() =>
        {
            Assert.That(log.RequestBodyTruncated, Is.Null);
            Assert.That(log.ResponseBodyTruncated, Is.Null);
            Assert.That(log.ResponseBodyBase64, Is.Null);

            AssertPropertyType<CardNetworkLog>(nameof(CardNetworkLog.RequestBodyTruncated), typeof(bool?));
            AssertPropertyType<CardNetworkLog>(nameof(CardNetworkLog.ResponseBodyTruncated), typeof(bool?));
            AssertPropertyType<CardNetworkLog>(nameof(CardNetworkLog.ResponseBodyBase64), typeof(bool?));
        });
    }

    [Test]
    public void CardNetworkLog_EncodedDataLengthIsLongNotInt()
    {
        // A response body can exceed int.MaxValue bytes; CDP reports it as a double-typed byte
        // count. int would silently overflow on a large download.
        AssertPropertyType<CardNetworkLog>(nameof(CardNetworkLog.EncodedDataLength), typeof(long?));
    }

    [Test]
    public void Card_ExposesNetworkLogsAsAnEmptyCollection()
    {
        var card = new Card();

        Assert.That(card.NetworkLogs, Is.Not.Null);
        Assert.That(card.NetworkLogs, Is.Empty);
    }

    // ---------------------------------------------------------------------------------------
    // KanbanPnBase.MapVersion copies by exact property NAME and swallows every mismatch into a
    // Console.WriteLine. A rename or a field missed on the version entity therefore drops
    // silently out of the audit row with no error anywhere. These two tests are the only thing
    // that catches it.
    // ---------------------------------------------------------------------------------------

    [Test]
    public void CardNetworkLog_MapVersion_CopiesEveryScalarField()
    {
        var log = new CardNetworkLog();
        var expected = PopulateEveryScalar(log);

        // Sanity: the loop must actually have reached the fields this migration adds.
        Assert.That(expected.Keys, Does.Contain(nameof(CardNetworkLog.Url)));
        Assert.That(expected.Keys, Does.Contain(nameof(CardNetworkLog.ResponseBody)));
        Assert.That(expected.Keys, Does.Contain(nameof(CardNetworkLog.FailureText)));
        Assert.That(expected.Keys, Does.Contain(nameof(CardNetworkLog.TimingJson)));

        AssertVersionReceivedEverything<CardNetworkLog, CardNetworkLogVersion>(log, expected);
    }

    [Test]
    public void CardConsoleLog_MapVersion_CopiesEveryScalarField()
    {
        var log = new CardConsoleLog();
        var expected = PopulateEveryScalar(log);

        Assert.That(expected.Keys, Does.Contain(nameof(CardConsoleLog.StackTrace)));
        Assert.That(expected.Keys, Does.Contain(nameof(CardConsoleLog.LineNumber)));
        Assert.That(expected.Keys, Does.Contain(nameof(CardConsoleLog.ColumnNumber)));
        Assert.That(expected.Keys, Does.Contain(nameof(CardConsoleLog.ArgsJson)));
        Assert.That(expected.Keys, Does.Contain(nameof(CardConsoleLog.RequestId)));

        AssertVersionReceivedEverything<CardConsoleLog, CardConsoleLogVersion>(log, expected);
    }

    // ---------------------------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------------------------

    private static void AssertPropertyType<T>(string name, Type expected)
    {
        PropertyInfo? prop = typeof(T).GetProperty(name);

        Assert.That(prop, Is.Not.Null, $"{typeof(T).Name} has no property '{name}'.");
        Assert.That(prop!.PropertyType, Is.EqualTo(expected),
            $"{typeof(T).Name}.{name} is {prop.PropertyType.Name}, expected {expected.Name}.");
    }

    private static IEnumerable<PropertyInfo> DeclaredScalarProperties(Type type)
        => type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(p => p.CanWrite && !IsEntityProperty(p));

    private static Dictionary<string, object?> PopulateEveryScalar(object entity)
    {
        var expected = new Dictionary<string, object?>();

        int seed = 1;
        foreach (PropertyInfo prop in entity.GetType().GetProperties())
        {
            if (!prop.CanWrite || IsEntityProperty(prop))
            {
                continue;
            }

            object? value = SyntheticValue(prop.PropertyType, seed++);
            prop.SetValue(entity, value);
            expected[prop.Name] = value;
        }

        return expected;
    }

    private static void AssertVersionReceivedEverything<TEntity, TVersion>(
        TEntity entity,
        Dictionary<string, object?> expected)
        where TEntity : KanbanPnBase
    {
        object version = InvokeMapVersion(entity);

        Assert.That(version, Is.InstanceOf<TVersion>());
        Type versionType = version.GetType();

        Assert.Multiple(() =>
        {
            foreach (KeyValuePair<string, object?> entry in expected)
            {
                // Id is remapped onto <ClassName>Id rather than copied straight across.
                string targetName = entry.Key == "Id"
                    ? $"{typeof(TEntity).Name}Id"
                    : entry.Key;

                PropertyInfo? target = versionType.GetProperty(targetName);
                Assert.That(target, Is.Not.Null,
                    $"{versionType.Name} has no property '{targetName}' — MapVersion would have " +
                    "silently dropped it into a Console.WriteLine.");
                Assert.That(target!.GetValue(version), Is.EqualTo(entry.Value),
                    $"{versionType.Name}.{targetName} did not receive the source value.");
            }
        });
    }

    private static bool IsEntityProperty(PropertyInfo prop)
        => prop.PropertyType.FullName?.Contains("Microting.KanbanBase.Infrastructure.Data.Entities") == true;

    private static object InvokeMapVersion(object entity)
    {
        MethodInfo? mapVersion = typeof(KanbanPnBase)
            .GetMethod("MapVersion", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.That(mapVersion, Is.Not.Null, "KanbanPnBase.MapVersion was renamed or removed.");

        object? result = mapVersion!.Invoke(entity, new object?[] { entity });

        Assert.That(result, Is.Not.Null, "MapVersion returned null — the ...Version type was not found.");
        return result!;
    }

    private static object? SyntheticValue(Type type, int seed)
    {
        Type target = Nullable.GetUnderlyingType(type) ?? type;

        if (target.IsEnum)
        {
            // Pick a member other than the default so an uncopied field cannot pass by accident.
            Array values = Enum.GetValues(target);
            return values.GetValue(values.Length - 1);
        }

        if (target == typeof(int))
        {
            return 1000 + seed;
        }

        if (target == typeof(long))
        {
            return 100000L + seed;
        }

        if (target == typeof(bool))
        {
            return true;
        }

        if (target == typeof(DateTime))
        {
            return new DateTime(2026, 10, 9, 0, 0, 0, DateTimeKind.Utc).AddMinutes(seed);
        }

        if (target == typeof(string))
        {
            return $"value-{seed}";
        }

        throw new NotSupportedException(
            $"ChromeExtensionCaptureSchemaTests.SyntheticValue has no case for {target.FullName}. " +
            "Add one so the MapVersion round-trip keeps covering every field.");
    }
}
