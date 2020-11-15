using System;
using AutoFixture.Community.ImmutableCollections;

// ReSharper disable once CheckNamespace
namespace AutoFixture
{
  public class ImmutableCollectionCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      if (fixture == null) throw new ArgumentNullException(nameof(fixture));
      
      fixture.Customizations.Add(new ImmutableCollectionRelay());
    }
  }
}