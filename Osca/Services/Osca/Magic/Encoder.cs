namespace Osca.Services.Osca.Magic
{
	public class Encoder
	{
		public static byte[] ByteKey = {
			52, 48, 54, 97, 54, 50, 50, 50, 51, 99,
			55, 54, 53, 52, 55, 101, 51, 98, 53, 101,
			51, 49, 53, 53, 52, 48, 54, 52, 53, 48,
			55, 101, 53, 99, 50, 56, 52, 101, 54, 49,
			54, 54, 53, 48, 55, 100, 50, 56, 51, 57,
			53, 49, 50, 56, 50, 52, 52, 51
		};

		public static string DoMagic(string s)
		{
			int[] arr = StringUtil.StringToIntArray(s);

			int[] ai = Encode(arr);

			string str = StringUtil.IntArrayToString(ai);

			return Base64.Encode(str);
		}

		public static int[] Encode(int[] stringAsIntArray)
		{
			Encoder encoder = new Encoder();

			EncoderKeys encoderKeys = encoder.SecondCalledMethod(ByteKey);

			return encoder.ThirdCalledMethod(stringAsIntArray, encoderKeys);
		}

		private long DoSomething(long longOne, long longTwo, long[] arrayOne, long longThree)
		{
			return longOne & -1L ^ longThree & -1L ^ ((arrayOne[(int)(longTwo >> 24) & 0xff]
					+ arrayOne[((int)(longTwo >> 16) & 0xff) + 256] & -1L ^ arrayOne[((int)(longTwo >> 8) & 0xff) + 512]) & -1L)
					+ arrayOne[(int)(longTwo & 255L) + 768] & -1L;
		}

		private EncoderKeys SecondCalledMethod(byte[] key)
		{

			int keyLength = key.Length > 72 ? 72 : key.Length;

			EncoderKeys encoderKeys = new EncoderKeys();
			int something = 0;

			for (int k = 0; k < encoderKeys.keyOne.Length; k++)
			{
				int l = something + 1;

				long l1 = key[something];

				something = l;
				if (l >= keyLength)
				{
					something = 0;
				}

				l = something + 1;
				long l2 = key[something];
				something = l;
				if (l >= keyLength)
				{
					something = 0;
				}
				l = something + 1;

				long l3 = key[something];
				something = l;
				if (l >= keyLength)
				{
					something = 0;
				}
				l = something + 1;

				long l4 = key[something];
				if (l >= keyLength)
				{
					something = 0;
				}
				else
				{
					something = l;
				}

				encoderKeys.keyOne[k] = encoderKeys.keyOne[k] ^ (((l1 << 8 | l2) << 8 | l3) << 8 | l4);
			}

			long[] longArray = { 0L, 0L };

			for (int index = 0; index < encoderKeys.keyOne.Length; index += 2)
			{
				DoMuchWithEncoderKeys(longArray, encoderKeys);
				encoderKeys.keyOne[index] = longArray[0];
				encoderKeys.keyOne[index + 1] = longArray[1];
			}

			for (int index = 0; index < encoderKeys.keyTwo.Length; index += 2)
			{
				DoMuchWithEncoderKeys(longArray, encoderKeys);
				encoderKeys.keyTwo[index] = longArray[0];
				encoderKeys.keyTwo[index + 1] = longArray[1];
			}
			return encoderKeys;
		}

		private int[] ThirdCalledMethod(int[] stringAsIntArray, EncoderKeys encoderKeys1)
		{

			int[] secret = { 30, 214, 186, 72, 38, 84, 2, 16 };
			int secretIndexCounter = 0;
			long[] longArray = new long[2];

			int length = stringAsIntArray.Length;
			int[] intArray = new int[length];
			int counter = length;
			length = 0;

			while (counter > 0)
			{

				DoMagic(encoderKeys1, secret, longArray, secretIndexCounter);

				int newValue = stringAsIntArray[length] ^ secret[secretIndexCounter];

				intArray[length] = newValue;
				secret[secretIndexCounter] = newValue;

				if (secretIndexCounter >= secret.Length - 1)
				{
					secretIndexCounter = 0;
				}
				else
				{
					secretIndexCounter = secretIndexCounter + 1;
				}

				length++;
				counter--;
			}

			return intArray;
		}

		private void DoMagic(EncoderKeys encoderKeys, int[] secret, long[] longArray, int secretIndex)
		{
			if (secretIndex != 0)
			{
				return;
			}

			longArray[0] = (long)(secret[0] << 24) | (long)(secret[1] << 16) | (long)(secret[2] << 8) | (long)secret[3];
			longArray[1] = (long)(secret[4] << 24) | (long)(secret[5] << 16) | (long)(secret[6] << 8) | (long)secret[7];

			DoMuchWithEncoderKeys(longArray, encoderKeys);

			long l3 = longArray[0];

			secret[0] = (int)(l3 >> 24 & 255L);
			secret[1] = (int)(l3 >> 16 & 255L);
			secret[2] = (int)(l3 >> 8 & 255L);
			secret[3] = (int)(l3 & 255L);

			l3 = longArray[1];

			secret[4] = (int)(l3 >> 24 & 255L);
			secret[5] = (int)(l3 >> 16 & 255L);
			secret[6] = (int)(l3 >> 8 & 255L);
			secret[7] = (int)(l3 & 255L);
		}

		private void DoMuchWithEncoderKeys(long[] longArray, EncoderKeys encoderKeys)
		{
			long first = longArray[0];
			long second = longArray[1];
			first = first & -1L ^ encoderKeys.keyOne[0] & -1L;
			second = DoSomething(second & -1L, first, encoderKeys.keyTwo, encoderKeys.keyOne[1]) & -1L;
			first = DoSomething(first, second, encoderKeys.keyTwo, encoderKeys.keyOne[2]) & -1L;
			second = DoSomething(second, first, encoderKeys.keyTwo, encoderKeys.keyOne[3]) & -1L;
			first = DoSomething(first, second, encoderKeys.keyTwo, encoderKeys.keyOne[4]) & -1L;
			second = DoSomething(second, first, encoderKeys.keyTwo, encoderKeys.keyOne[5]) & -1L;
			first = DoSomething(first, second, encoderKeys.keyTwo, encoderKeys.keyOne[6]) & -1L;
			second = DoSomething(second, first, encoderKeys.keyTwo, encoderKeys.keyOne[7]) & -1L;
			first = DoSomething(first, second, encoderKeys.keyTwo, encoderKeys.keyOne[8]) & -1L;
			second = DoSomething(second, first, encoderKeys.keyTwo, encoderKeys.keyOne[9]) & -1L;
			first = DoSomething(first, second, encoderKeys.keyTwo, encoderKeys.keyOne[10]) & -1L;
			second = DoSomething(second, first, encoderKeys.keyTwo, encoderKeys.keyOne[11]) & -1L;
			first = DoSomething(first, second, encoderKeys.keyTwo, encoderKeys.keyOne[12]) & -1L;
			second = DoSomething(second, first, encoderKeys.keyTwo, encoderKeys.keyOne[13]) & -1L;
			first = DoSomething(first, second, encoderKeys.keyTwo, encoderKeys.keyOne[14]) & -1L;
			second = DoSomething(second, first, encoderKeys.keyTwo, encoderKeys.keyOne[15]) & -1L;
			first = DoSomething(first, second, encoderKeys.keyTwo, encoderKeys.keyOne[16]);

			long lastElement = encoderKeys.keyOne[encoderKeys.keyOne.Length - 1];
			longArray[1] = first & -1L;
			longArray[0] = (second ^ lastElement & -1L) & -1L;
		}
	}
}