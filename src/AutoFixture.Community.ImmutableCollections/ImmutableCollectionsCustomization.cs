using System;
using AutoFixture.Community.ImmutableCollections;

// ReSharper disable once CheckNamespace
namespace AutoFixture
{
  /// <summary>
  /// A customization that enables creating specimens of the collections found in the System.Collections.Immutable namespace.
  /// </summary>
  public class ImmutableCollectionsCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      if (fixture == null) throw new ArgumentNullException(nameof(fixture));
      
      fixture.Customizations.Add(new ImmutableCollectionRelay());
    }
  }
}