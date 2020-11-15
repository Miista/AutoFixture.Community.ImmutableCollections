using System;
using AutoFixture.Community.ImmutableCollections;

// ReSharper disable once CheckNamespace
namespace AutoFixture
{
  public class ImmutableCollectionsCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      if (fixture == null) throw new ArgumentNullException(nameof(fixture));
      
      fixture.Customizations.Add(new ImmutableCollectionRelay());
    }
  }
}