﻿using System.Runtime.CompilerServices;

namespace Vez.Utils.Extensions
{
	public static class ArrayExt
	{
		/// <summary>
		/// checks to see if value exists in source
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Contains<T>(this T[] source, T value)
		{
			return Array.IndexOf(source, value) >= 0;
		}
	}
}