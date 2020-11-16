using System;

namespace AutoFixture.Community.ImmutableCollections.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var fixture = new Fixture().Customize(new ImmutableCollectionsCustomization());

            var immutableDictionary = fixture.Create<System.Collections.Immutable.ImmutableDictionary<string, string>>();
            var immutableIDictionary = fixture.Create<System.Collections.Immutable.IImmutableDictionary<string, string>>();
            var immutableStack = fixture.Create<System.Collections.Immutable.ImmutableStack<string>>();
            Console.WriteLine("Hello World!");
        }
    }
}