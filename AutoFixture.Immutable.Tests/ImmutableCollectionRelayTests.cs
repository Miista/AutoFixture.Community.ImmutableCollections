using System.Collections.Immutable;
using FluentAssertions;
using Xunit;

namespace AutoFixture.Immutable.Tests
{
    public class ImmutableCollectionRelayTests
    {
        [Fact]
        public void Can_create_immutable_queue()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new ImmutableCollectionRelay());
            
            // Act
            var result = fixture.Create<ImmutableQueue<string>>();
            
            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Can_create_immutable_dictionary()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new ImmutableCollectionRelay());
            
            // Act
            var result = fixture.Create<ImmutableDictionary<string, string>>();
            
            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Can_create_immutable_queue_interface()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new ImmutableCollectionRelay());
            
            // Act
            var result = fixture.Create<IImmutableQueue<string>>();
            
            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Can_create_immutable_list()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new ImmutableCollectionRelay());
            
            // Act
            var result = fixture.Create<ImmutableList<string>>();
            
            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Can_create_immutable_stack()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new ImmutableCollectionRelay());
            
            // Act
            var result = fixture.Create<ImmutableStack<string>>();
            
            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Can_create_immutable_hash_set()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new ImmutableCollectionRelay());
            
            // Act
            var result = fixture.Create<ImmutableHashSet<string>>();
            
            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Can_create_immutable_array()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new ImmutableCollectionRelay());
            
            // Act
            var result = fixture.Create<ImmutableArray<string>>();
            
            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
        
        [Fact]
        public void Can_create_immutable_sorted_set()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new ImmutableCollectionRelay());
            
            // Act
            var result = fixture.Create<ImmutableSortedSet<string>>();
            
            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
    }
}