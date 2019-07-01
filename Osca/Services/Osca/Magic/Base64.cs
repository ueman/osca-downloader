using System;
using System.Linq;

namespace Osca.Services.Osca.Magic
{
	public static class Base64
	{
		/// <summary>
		/// Das Custom Charset von Osca für Base64
		/// </summary>
		private const string customAlphabet = "ipkIBozSlL8CVH3J7PvQfWYuemxOcR4rn5bgDUqMaKXTy60h9wt-NZdjFAE12Gs.";

		/// <summary>
		/// Das Base64-Charset siehe: https://en.wikipedia.org/wiki/Base64#Base64_table
		/// </summary>
		private static readonly string defaultBase64Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		/// <summary>
		/// Diese Methode funktioniert auf jeden Fall
		/// </summary>
		/// <returns>The encode.</returns>
		/// <param name="textToEncode">Text to encode.</param>
		/// <param name="alphabet">Alphabet.</param>
		public static string Encode(string textToEncode, string alphabet = customAlphabet)
		{
			int inputLength = textToEncode.Length;
			char[] charArrayOne = new char[3];
			char[] charArrayTwo = new char[4];
			string stringOne = "";
			int i = 0;
			int j = 0;
			do
			{
				int inputLengthMinusOne = inputLength - 1;
				if (inputLength <= 0)
				{
					break;
				}
				int l1 = i + 1;
				inputLength = j + 1;
				charArrayOne[i] = textToEncode[j];
				if (l1 == charArrayOne.Length)
				{
					charArrayTwo[0] = (char)((charArrayOne[0] & 0xfc) >> 2);
					charArrayTwo[1] = (char)(((charArrayOne[0] & 3) << 4) + ((charArrayOne[1] & 0xf0) >> 4));
					charArrayTwo[2] = (char)(((charArrayOne[1] & 0xf) << 2) + ((charArrayOne[2] & 0xc0) >> 6));
					charArrayTwo[3] = (char)(charArrayOne[2] & 0x3f);
					for (i = 0; i < charArrayTwo.Length; i++)
					{
						stringOne = stringOne + alphabet[charArrayTwo[i]];
					}

					j = inputLength;
					i = 0;
				}
				else
				{
					i = l1;
					j = inputLength;
				}

				inputLength = inputLengthMinusOne;
			} while (true);


			if (i <= 0)
			{
				return stringOne;
			}

			for (int k = i; k < charArrayOne.Length; k++)
			{
				charArrayOne[k] = '\0';
			}

			charArrayTwo[0] = (char)((charArrayOne[0] & 0xfc) >> 2);
			charArrayTwo[1] = (char)(((charArrayOne[0] & 3) << 4) + ((charArrayOne[1] & 0xf0) >> 4));
			charArrayTwo[2] = (char)(((charArrayOne[1] & 0xf) << 2) + ((charArrayOne[2] & 0xc0) >> 6));
			charArrayTwo[3] = (char)(charArrayOne[2] & 0x3f);
			int counter = 0;
			int integer = 0;

			while (counter <= i + 1)
			{
				integer = i;
				textToEncode = stringOne;
				stringOne = stringOne + alphabet[charArrayTwo[counter]];
				counter++;
			}

			string stringTwo = "";

			while (integer <= 3)
			{
				stringTwo = textToEncode;
				textToEncode = textToEncode + "=";
				integer++;
			}

			return stringTwo;
		}

		/// <summary>
		/// Die hier ist schöner, sollte aber noch mehr getestet werden
		/// </summary>
		/// <returns>The encode.</returns>
		/// <param name="s">S.</param>
		/// <param name="alphabet">Alphabet.</param>
		public static string AlternativeEncode(string s, string alphabet = customAlphabet)
		{
			var bits = string.Empty;
			foreach (var character in s)
			{
				bits += Convert.ToString(character, 2).PadLeft(8, '0');
			}

			string base64 = string.Empty;

			const byte threeOctets = 24;
			var octetsTaken = 0;
			while (octetsTaken < bits.Length)
			{
				var currentOctects = bits.Skip(octetsTaken).Take(threeOctets).ToList();

				const byte sixBits = 6;
				int hextetsTaken = 0;
				while (hextetsTaken < currentOctects.Count())
				{
					var chunk = currentOctects.Skip(hextetsTaken).Take(sixBits);
					hextetsTaken += sixBits;

					var bitString = chunk.Aggregate(string.Empty, (current, currentBit) => current + currentBit);

					if (bitString.Length < 6)
					{
						bitString = bitString.PadRight(6, '0');
					}
					var singleInt = Convert.ToInt32(bitString, 2);

					base64 += alphabet[singleInt];
				}

				octetsTaken += threeOctets;
			}

			// Pad with = for however many octects we have left
			for (var i = 0; i < (bits.Length % 3); i++)
			{
				base64 += "=";
			}

			return base64;
		}
	}
}
