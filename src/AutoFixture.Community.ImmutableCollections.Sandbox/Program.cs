using System;
using System.Collections.Immutable;

namespace AutoFixture.Community.ImmutableCollections.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var fixture = new Fixture().Customize(new ImmutableCollectionCustomization());

            var immutableList = fixture.Create<ImmutableDictionary<string, string>>();
            Console.WriteLine("Hello World!");
        }
    }
}