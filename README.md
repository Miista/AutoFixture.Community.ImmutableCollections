# AutoFixture.Community.ImmutableCollections

Creating specimens for System.Collections.Immutable.

Supports **.NET Core** (.NET Standard 2+)

## Installation

```
PM> Install-Package AutoFixture.Community.ImmutableCollections
```

## Usage
```csharp
var fixture = new Fixture().Customize(new ImmutableCollectionsCustomization());
var immutableList = fixture.Create<ImmutableList<string>>();
```
