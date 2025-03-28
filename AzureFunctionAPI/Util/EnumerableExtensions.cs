using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionAPI.Util {
	public static class EnumerableExtensions {
		public static T Find<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) {
			foreach (var current in enumerable)
				if (predicate(current))
					return current;
			return enumerable.First();
		}
	}
}
