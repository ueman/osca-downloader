using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Osca.Services.Osca.Magic
{
	public static class StringUtil
	{
		public static string IntArrayToString(int[] intArr)
			=> new string(intArr.Select(i => (char)i).ToArray());

		public static int[] StringToIntArray(string text)
			=> text.ToCharArray().Select(c => (int)c).ToArray();

		public static string StringToMd5(string text)
		{
			MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
			var bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(text));

			StringBuilder hash = new StringBuilder();
			foreach (var b in bytes)
			{
				hash.Append(b.ToString("x2"));

			}
			return hash.ToString().ToUpper();
		}
	}
}
