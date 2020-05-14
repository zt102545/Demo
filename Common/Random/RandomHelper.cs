using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
	/// <summary>
	/// 多线程环境下严格随机的Random实现
	/// </summary>
	public class RandomHelper
    {
		#region RandomGen内部类

		/// <summary>
		/// 多线程环境下严格随机的Random实现
		/// </summary>
		private static class RandomGen
		{
			private static RNGCryptoServiceProvider _global = new RNGCryptoServiceProvider();

			[ThreadStatic]
			private static Random _local;

			public static int Next()
			{
				Random inst = _local;
				if (inst == null)
				{
					byte[] buffer = new byte[4];
					_global.GetBytes(buffer);
					_local = inst = new Random(BitConverter.ToInt32(buffer, 0));
				}
				return inst.Next();
			}

			public static int Next(int min, int max)
			{
				Random inst = _local;
				if (inst == null)
				{
					byte[] buffer = new byte[4];
					_global.GetBytes(buffer);
					_local = inst = new Random(BitConverter.ToInt32(buffer, 0));
				}
				return inst.Next(min, max);
			}
		}

		#endregion

		/// <summary>
		/// 返回介于min和max之间的一个随机数。包括min，不包括max
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int Random(int min, int max)
		{
			return RandomGen.Next(min, max);
		}

		/// <summary>
		/// 返回非负随机数（多线程下也严格随机）
		/// </summary>
		/// <returns></returns>
		public static int Random()
		{
			return RandomGen.Next();
		}

		/// <summary>
		/// 随机以rate分之一的概率返回true，其他时候返回false
		/// </summary>
		/// <param name="rate"></param>
		/// <returns></returns>
		public static bool RandomRate(int rate)
		{
			int rest = Random() % rate;

			bool ret = (rest == 0);
			return ret;
		}

		/// <summary>
		/// 生成指定长度的随机字符串（大写字母和数字）
		/// </summary>
		/// <param name="len"></param>
		/// <returns></returns>
		public static string RandomString(int len)
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var stringChars = new char[len];

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[Random() % chars.Length];
			}

			var finalString = new String(stringChars);
			return finalString;
		}

		/// <summary>
		/// 生成指定长度的随机字符串（小写字母、大写字母和数字）
		/// </summary>
		/// <param name="len"></param>
		/// <returns></returns>
		public static string RandomFullString(int len)
		{
			var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var stringChars = new char[len];

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[Random() % chars.Length];
			}

			var finalString = new String(stringChars);
			return finalString;
		}

		public static string Random(string chars, int len)
		{
			var stringChars = new char[len];
			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[Random() % chars.Length];
			}
			var finalString = new String(stringChars);
			return finalString;
		}

	}
}
