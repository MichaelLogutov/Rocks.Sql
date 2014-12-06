using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Rocks.Sql
{
	/// <summary>
	///     A collection that supports only adding items either by name (overwriting existing with the same name)
	///     or sequenced (without name, just like simple list).
	/// </summary>
	/// <typeparam name="TKey">Item key type.</typeparam>
	/// <typeparam name="TValue">Item value type.</typeparam>
	public class OrderedHybridCollection<TKey, TValue> : IEnumerable<TValue>
	{
		#region Private fields

		private readonly IList<TValue> values;
		private readonly IDictionary<TKey, int> indexes;

		#endregion

		#region Construct

		/// <summary>
		///     Initializes a new instance of the <see cref="OrderedHybridCollection&lt;TKey, TValue&gt;" /> class.
		/// </summary>
		public OrderedHybridCollection (IEqualityComparer<TKey> comparer = null)
		{
			this.values = new List<TValue> ();
			this.indexes = new Dictionary<TKey, int> (comparer);
		}

		#endregion

		#region Public properties

		/// <summary>
		///     (GET) Number of items in the collection.
		/// </summary>
		public int Count { get { return this.values.Count; } }


		/// <summary>
		///     (GET) Item by <paramref name="key" />.
		///     Returns default (<typeparamref name="TValue" />) if item with specified
		///     <paramref name="key" /> was not found.
		/// </summary>
		/// <param name="key">Item key.</param>
		public virtual TValue this [[NotNull] TKey key]
		{
			get
			{
				var index = this.GetIndex (key);
				if (index == null)
					return default (TValue);

				return this.values[index.Value];
			}
		}


		/// <summary>
		///     (GET) Item by <paramref name="index" />.
		/// </summary>
		/// <param name="index">Item index.</param>
		public virtual TValue this [int index] { get { return this.values[index]; } }

		#endregion

		#region Public methods

		/// <summary>
		///     Adds new keyed item to the collection.
		///		If there is an item with the same <paramref name="key" /> present,
		///     it will be overwritten (if <paramref name="overwrite" /> = true),
		///     or nothing will be added (if <paramref name="overwrite" /> = false).
		/// </summary>
		/// <param name="key">Item key.</param>
		/// <param name="value">Item value.</param>
		/// <param name="overwrite">If true then will overwrite existed item with the same <paramref name="key" />.</param>
		public void AddKeyed ([NotNull] TKey key, TValue value, bool overwrite = false)
		{
			int index;

			if (this.indexes.TryGetValue (key, out index))
			{
				if (overwrite)
					this.values[index] = value;
			}
			else
			{
				this.values.Add (value);
				this.indexes[key] = this.values.Count - 1;
			}
		}


		/// <summary>
		///     Adds new not keyed item to the collection.
		/// </summary>
		/// <param name="value">Item value.</param>
		public void AddSequenced (TValue value)
		{
			this.values.Add (value);
		}


		/// <summary>
		///     Returns true if there is an item with specified <paramref name="key" />.
		/// </summary>
		/// <param name="key">Item key.</param>
		public bool Contains ([NotNull] TKey key)
		{
			return this.indexes.ContainsKey (key);
		}


		/// <summary>
		///     Gets index of the item by it's key.
		///     Returns null if no item with specified key exist.
		/// </summary>
		/// <param name="key">Item key.</param>
		public int? GetIndex ([NotNull] TKey key)
		{
			int index;
			if (!this.indexes.TryGetValue (key, out index))
				return null;

			return index;
		}

		#endregion

		#region IEnumerable<TValue> Members

		/// <summary>
		///     Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<TValue> GetEnumerator ()
		{
			return this.values.GetEnumerator ();
		}


		/// <summary>
		///     Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return this.GetEnumerator ();
		}

		#endregion
	}
}