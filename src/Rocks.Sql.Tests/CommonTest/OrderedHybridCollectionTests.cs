using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Rocks.Sql.Tests.CommonTest
{
	public class OrderedHybridCollectionTests
	{
		[Fact]
		public void AddKeyed_TwoItemsWithDifferentKeys_AddsBothItems ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();


			// act
			sut.AddKeyed ("a", "aaa");
			sut.AddKeyed ("b", "bbb");


			// assert
			sut.Should ().Equal ("aaa", "bbb");
			sut["a"].Should ().Be ("aaa");
			sut["b"].Should ().Be ("bbb");
		}


		[Fact]
		public void AddKeyed_TwoItemsWithTheSameKeys_ByDefault_DoesNotAddsNewItem ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();


			// act
			sut.AddKeyed ("a", "aaa");
			sut.AddKeyed ("a", "bbb");


			// assert
			sut.Should ().Equal ("aaa");
		}


		[Fact]
		public void AddKeyed_TwoItemsWithTheSameKeys_WithOverwrite_OverwritesOldItem ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();


			// act
			sut.AddKeyed ("a", "aaa");
			sut.AddKeyed ("a", "bbb", overwrite: true);


			// assert
			sut.Should ().Equal ("bbb");
		}


		[Fact]
		public void AddSequenced_TwoItems_AddsBothItems ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();


			// act
			sut.AddSequenced ("aaa");
			sut.AddSequenced ("bbb");


			// assert
			sut.Should ().Equal ("aaa", "bbb");
		}


		[Fact]
		public void AddKeyed_And_AddSequenced_AddsBothItems ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();


			// act
			sut.AddKeyed ("a", "aaa");
			sut.AddSequenced ("bbb");


			// assert
			sut.Should ().Equal ("aaa", "bbb");
			sut["a"].Should ().Be ("aaa");
		}


		[Fact]
		public void GetIndex_HasCorrespondingKeyedItem_ReturnsItsIndex ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();
			sut.AddKeyed ("a", "aaa");

			// act
			var result = sut.GetIndex ("a");


			// assert
			result.Should ().Be (0);
		}


		[Fact]
		public void GetIndex_NoCorrespondingKeyedItem_ReturnsNull ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();
			sut.AddKeyed ("a", "aaa");

			// act
			var result = sut.GetIndex ("b");


			// assert
			result.Should ().NotHaveValue ();
		}


		[Fact]
		public void GetIndex_NoCorrespondingKeyedItem_HasSequencedItem_ReturnsNull ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();
			sut.AddSequenced ("aaa");

			// act
			var result = sut.GetIndex ("a");


			// assert
			result.Should ().NotHaveValue ();
		}


		[Fact]
		public void GetCount_OneKeyedItem_ReturnsOne ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();
			sut.AddKeyed ("a", "aaa");


			// act


			// assert
			sut.Count.Should ().Be (1);
		}


		[Fact]
		public void GetCount_OnSequencedItem_ReturnsOne ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();
			sut.AddSequenced ("aaa");


			// act


			// assert
			sut.Count.Should ().Be (1);
		}
	}
}


