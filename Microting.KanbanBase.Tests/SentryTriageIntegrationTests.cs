#nullable enable
using System;
using System.Linq;
using System.Reflection;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

[TestFixture]
public class SentryTriageIntegrationTests
{
    private const string EntityNamespace = "Microting.KanbanBase.Infrastructure.Data.Entities";

    // Every scalar property of the entity (except Id) must exist on the version twin with the same
    // type, because KanbanPnBase.MapVersion copies by name and only logs a miss to the console.
    private static void AssertVersionParity(Type entity, Type version)
    {
        var scalars = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != "Id"
                        && p.PropertyType.Namespace != EntityNamespace
                        && !(p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments()
                            .Any(a => a.Namespace == EntityNamespace)));
        Assert.Multiple(() =>
        {
            foreach (var p in scalars)
            {
                var v = version.GetProperty(p.Name);
                Assert.That(v, Is.Not.Null, $"{version.Name} is missing {p.Name}");
                Assert.That(v?.PropertyType, Is.EqualTo(p.PropertyType), $"{version.Name}.{p.Name} type");
            }
            Assert.That(version.GetProperty(entity.Name + "Id")?.PropertyType, Is.EqualTo(typeof(int)));
        });
    }

    [Test]
    public void Card_SentryFields_DefaultToNull()
    {
        var card = new Card();
        Assert.That(card.SentryIssueId, Is.Null);
        Assert.That(card.SentryShortId, Is.Null);
        Assert.That(card.SentryPermalink, Is.Null);
        Assert.That(card.Tenants, Is.Not.Null.And.Empty);
    }

    [Test]
    public void CardVersion_HasSentryFields()
    {
        Assert.That(typeof(CardVersion).GetProperty(nameof(Card.SentryIssueId))?.PropertyType, Is.EqualTo(typeof(string)));
        Assert.That(typeof(CardVersion).GetProperty(nameof(Card.SentryShortId))?.PropertyType, Is.EqualTo(typeof(string)));
        Assert.That(typeof(CardVersion).GetProperty(nameof(Card.SentryPermalink))?.PropertyType, Is.EqualTo(typeof(string)));
    }

    [Test]
    public void CardTenant_VersionParity() => AssertVersionParity(typeof(CardTenant), typeof(CardTenantVersion));

    [Test]
    public void IntegrationApiKey_VersionParity() => AssertVersionParity(typeof(IntegrationApiKey), typeof(IntegrationApiKeyVersion));

    [Test]
    public void IntegrationApiKey_Defaults()
    {
        var key = new IntegrationApiKey();
        Assert.That(key.Scope, Is.EqualTo("sentry-triage"));
        Assert.That(key.RevokedAt, Is.Null);
        Assert.That(key.LastUsedAt, Is.Null);
        Assert.That(key.KeyHash, Is.EqualTo(string.Empty));
    }
}
