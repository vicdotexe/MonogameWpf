using Vez.Maths;

namespace Vez.Utils.Extensions
{
    public static class FloatExt
	{
		public static bool Approximately(this float self, float other)
		{
			return Mathf.Approximately(self, other);
		}
	}
}