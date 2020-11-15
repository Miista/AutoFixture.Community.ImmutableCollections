# AutoFixture.Community.Immutable

Creating specimens for System.Collections.Immutable.

Supports **.NET Core** (.NET Standard 1.5+)

## Installation

```
PM> Install-Package AutoFixture.Community.Immutable
```

## Usage
```csharp
var fixture = new Fixture().Customize(new ImmutableCollectionCustomization());
var optionalString = fixture.Create<Option<string>>();
```
