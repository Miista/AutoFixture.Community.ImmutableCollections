using System;
using System.Collections.Immutable;

namespace AutoFixture.Community.Immutable.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new ImmutableCollectionRelay());

            var immutableList = fixture.Create<ImmutableDictionary<string, string>>();
            Console.WriteLine("Hello World!");
        }
    }
}