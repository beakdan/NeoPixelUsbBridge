using System;

namespace NeoPixelUsbBridge
{
	class LedEfects
	{
		public static int[] sinColorTable = new int[128];

		static LedEfects()
		{
			FillColorTable();
		}

		public class ColorChase
		{
			private int _blockCount;
			private int _blockLenght;
			private int _position;
			private int _color;
			private int[] pixels; 

			public ColorChase(int LedCount, int Blocks, int BlockLength)
			{
				pixels = new int[LedCount];
				_blockCount = Blocks;
				_blockLenght = BlockLength;
			}

			public int[] MoveNext()
			{
				//Clear all the pixels
				for (int i = 0; i < pixels.Length; i++)
					pixels[i] = 0;

				for (int b = 0; b < _blockCount; b++)
				{
					var blockIndex = ((pixels.Length / _blockCount) * b) + _position;
					for (int i = 0; i < _blockLenght; i++)
					{
						var pixIndex = (blockIndex + i) % pixels.Length;
						var colorIndex = (_color + i) % LedEfects.sinColorTable.Length;
						pixels[pixIndex] = LedEfects.sinColorTable[colorIndex];
					}
				}

				_position = (_position + 1) % pixels.Length;
				_color = (_color + 1) % LedEfects.sinColorTable.Length;
				return pixels;
			}
		}

		private static void FillColorTable()
		{
			var refValues = new byte[sinColorTable.Length];
			var phase = 120d;
			//The on time for a color is 180°, the full cycle is phase * 3
			var onTimeRatio = refValues.Length * 180d / (phase * 3);
			var radian = Math.PI / onTimeRatio;

			//compute the values to do a simple lookup
			for (int i = 0; i < refValues.Length; i++)
			{
				//Console.WriteLine(Math.Sin(radian * i) * 255);
				refValues[i] = (byte)(i < (int)onTimeRatio ? Math.Sin(radian * i) * 255 : 0);
			}
			//now asign each color with an offset of 120°
			for (int i = 0; i < sinColorTable.Length; i++)
			{
				var greenOffset = (int)(i + (onTimeRatio * phase * 2 / 180d)) % sinColorTable.Length;
				var blueOffset = (int)(i + (onTimeRatio * phase / 180d)) % sinColorTable.Length;
				sinColorTable[i] =
					(refValues[i] << 8) |				//red 
					(refValues[greenOffset] << 16) |	//green
					refValues[blueOffset];				//blue
			}
		}
	}
}
