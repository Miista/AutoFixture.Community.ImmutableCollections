# AutoFixture.Community.ImmutableCollections

Creating specimens for System.Collections.Immutable.

Supports **.NET Core** (.NET Standard 2+)

## Supports
The following types from the System.Collection.Immutable namespace are supported.

### Structs
* `ImmutableArray`

### Classes
* `ImmutableDictionary`
* `ImmutableHashSet`
* `ImmutableList`
* `ImmutableSortedDictionary`
* `ImmutableStack`
* `ImmutableQueue`

### Interfaces
* `IImmutableDictionary`
* `IImmutableList`
* `IImmutableSet`
* `IImmutableSortedSet`
* `IImmutableStack`
* `IImmutableQueue`

## Installation

```
PM> Install-Package AutoFixture.Community.ImmutableCollections
```

## Usage
```csharp
var fixture = new Fixture().Customize(new ImmutableCollectionsCustomization());
var immutableList = fixture.Create<ImmutableList<string>>();
```
