#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using Microting.KanbanBase.Infrastructure.Enums;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

/// <summary>
/// Covers the Userback *sync* schema change (20260901000000_UserbackSyncSchema), the same way
/// <see cref="UserbackMigrationTests"/> covers the schema change before it.
/// </summary>
[TestFixture]
public class UserbackSyncSchemaTests
{
    // ---------------------------------------------------------------------------------------
    // Persisted enum values. These are stored as ints in the database, so renumbering a member
    // silently reinterprets every existing row. Nothing else in the codebase pins them.
    // ---------------------------------------------------------------------------------------

    [Test]
    public void UserbackImportLogEntryStatus_ValuesArePinned()
    {
        Assert.Multiple(() =>
        {
            Assert.That((int)UserbackImportLogEntryStatus.Imported, Is.EqualTo(0));
            Assert.That((int)UserbackImportLogEntryStatus.Skipped, Is.EqualTo(1));
            Assert.That((int)UserbackImportLogEntryStatus.Failed, Is.EqualTo(2));
            Assert.That((int)UserbackImportLogEntryStatus.Updated, Is.EqualTo(3));
            Assert.That((int)UserbackImportLogEntryStatus.Deleted, Is.EqualTo(4));
        });
    }

    [Test]
    public void UserbackImportMode_ValuesArePinned()
    {
        Assert.Multiple(() =>
        {
            Assert.That((int)UserbackImportMode.Incremental, Is.EqualTo(0));
            Assert.That((int)UserbackImportMode.Full, Is.EqualTo(1));
        });
    }

    [Test]
    public void CardSource_ValuesArePinned()
    {
        Assert.Multiple(() =>
        {
            Assert.That((int)CardSource.Manual, Is.EqualTo(0));
            Assert.That((int)CardSource.Userback, Is.EqualTo(1));
            Assert.That((int)CardSource.Api, Is.EqualTo(2));
            Assert.That((int)CardSource.Email, Is.EqualTo(3));
        });
    }

    // ---------------------------------------------------------------------------------------
    // CLR defaults must line up with the column defaults the migration writes.
    // ---------------------------------------------------------------------------------------

    [Test]
    public void CardTag_SourceDefaultsToManual()
    {
        Assert.That(new CardTag().Source, Is.EqualTo(CardSource.Manual));
        Assert.That(new CardTagVersion().Source, Is.EqualTo(CardSource.Manual));
    }

    [Test]
    public void UserbackImportRun_NewFields_HaveCorrectDefaults()
    {
        var run = new UserbackImportRun();

        Assert.Multiple(() =>
        {
            Assert.That(run.Mode, Is.EqualTo(UserbackImportMode.Incremental));
            Assert.That(run.CardsCreated, Is.EqualTo(0));
            Assert.That(run.CardsUpdated, Is.EqualTo(0));
            Assert.That(run.CardsSkipped, Is.EqualTo(0));
            Assert.That(run.CardsSoftDeleted, Is.EqualTo(0));
            Assert.That(run.MediaFailed, Is.EqualTo(0));
            Assert.That(run.TotalToProcess, Is.EqualTo(0));
            Assert.That(run.CardsImported, Is.EqualTo(0));
            Assert.That(run.LastHeartbeatAt, Is.Null);
        });
    }

    [Test]
    public void UserbackProjectSyncState_WatermarkStartsNull()
    {
        // NULL means "never synced, do a full pull". A non-nullable DateTime would default to
        // 0001-01-01, which is outside MySQL/MariaDB's DATETIME range and throws under
        // STRICT_TRANS_TABLES rather than coercing.
        Assert.That(new UserbackProjectSyncState().LastSyncedModifiedAt, Is.Null);
        Assert.That(new UserbackProjectSyncStateVersion().LastSyncedModifiedAt, Is.Null);

        Assert.That(
            typeof(UserbackProjectSyncState).GetProperty(nameof(UserbackProjectSyncState.LastSyncedModifiedAt))!.PropertyType,
            Is.EqualTo(typeof(DateTime?)));
        Assert.That(
            typeof(UserbackProjectSyncStateVersion).GetProperty(nameof(UserbackProjectSyncStateVersion.LastSyncedModifiedAt))!.PropertyType,
            Is.EqualTo(typeof(DateTime?)));
    }

    [Test]
    public void Comment_UserbackCommentIdIsNullableLong()
    {
        PropertyInfo? prop = typeof(Comment).GetProperty(nameof(Comment.UserbackCommentId));

        Assert.That(prop, Is.Not.Null);
        Assert.That(prop!.PropertyType, Is.EqualTo(typeof(long?)));
        Assert.That(new Comment().UserbackCommentId, Is.Null);
    }

    [Test]
    public void Attachment_SourceUrlAndHashAreNullableStrings()
    {
        var attachment = new Attachment();

        Assert.Multiple(() =>
        {
            Assert.That(attachment.SourceUrl, Is.Null);
            Assert.That(attachment.SourceUrlHash, Is.Null);
            Assert.That(typeof(AttachmentVersion).GetProperty(nameof(AttachmentVersion.SourceUrl)), Is.Not.Null);
            Assert.That(typeof(AttachmentVersion).GetProperty(nameof(AttachmentVersion.SourceUrlHash)), Is.Not.Null);
        });
    }

    // ---------------------------------------------------------------------------------------
    // KanbanPnBase.MapVersion copies by exact property NAME and swallows every mismatch into a
    // Console.WriteLine. A rename or a field missed on the version entity therefore drops
    // silently out of the audit row with no error anywhere. This is the only thing that catches
    // it.
    // ---------------------------------------------------------------------------------------

    [Test]
    public void UserbackImportRun_MapVersion_CopiesEveryScalarField()
    {
        var run = new UserbackImportRun();
        var expected = new Dictionary<string, object?>();

        int seed = 1;
        foreach (PropertyInfo prop in typeof(UserbackImportRun).GetProperties())
        {
            if (!prop.CanWrite || IsEntityProperty(prop))
            {
                continue;
            }

            object? value = SyntheticValue(prop.PropertyType, seed++);
            prop.SetValue(run, value);
            expected[prop.Name] = value;
        }

        // Sanity: the loop must actually have populated the new sync fields.
        Assert.That(expected.Keys, Does.Contain(nameof(UserbackImportRun.Mode)));
        Assert.That(expected.Keys, Does.Contain(nameof(UserbackImportRun.CardsCreated)));
        Assert.That(expected.Keys, Does.Contain(nameof(UserbackImportRun.LastHeartbeatAt)));

        object version = InvokeMapVersion(run);

        Assert.That(version, Is.InstanceOf<UserbackImportRunVersion>());
        Type versionType = version.GetType();

        Assert.Multiple(() =>
        {
            foreach (KeyValuePair<string, object?> entry in expected)
            {
                // Id is remapped onto <ClassName>Id rather than copied straight across.
                string targetName = entry.Key == "Id"
                    ? $"{nameof(UserbackImportRun)}Id"
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
            return new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc).AddMinutes(seed);
        }

        if (target == typeof(string))
        {
            return $"value-{seed}";
        }

        throw new NotSupportedException(
            $"UserbackSyncSchemaTests.SyntheticValue has no case for {target.FullName}. " +
            "Add one so the MapVersion round-trip keeps covering every field.");
    }
}
