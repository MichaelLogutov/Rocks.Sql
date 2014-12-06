using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Sql.Tests.CommonTest
{
	[TestClass]
	public class OrderedHybridCollectionTests
	{
		[TestMethod]
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


		[TestMethod]
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


		[TestMethod]
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


		[TestMethod]
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


		[TestMethod]
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


		[TestMethod]
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


		[TestMethod]
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


		[TestMethod]
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


		[TestMethod]
		public void GetCount_OneKeyedItem_ReturnsOne ()
		{
			// arrange
			var sut = new OrderedHybridCollection<string, string> ();
			sut.AddKeyed ("a", "aaa");


			// act


			// assert
			sut.Count.Should ().Be (1);
		}


		[TestMethod]
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