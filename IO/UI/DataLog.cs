using System.Collections;

namespace IO.UI
{
	class DataLog : IEnumerable<string>
	{
		public readonly int MAX_SIZE;
		private Queue<string> _queue;

		public DataLog(int maxSize)
		{
			MAX_SIZE = maxSize;
			_queue = new Queue<string>(MAX_SIZE);
		}

		public void WriteLine(string item)
		{
			if (_queue.Count == MAX_SIZE)
				PopLine();

			_queue.Enqueue(item);
		}

		public string PopLine()
		{
			return _queue.Dequeue();
		}

		public void Peek()
		{
			_queue.Peek();
		}

		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			return _queue.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _queue.GetEnumerator();
		}
	}
}
