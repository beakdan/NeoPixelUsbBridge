using System;
using System.IO.Ports;
using System.Threading;

namespace NeoPixelUsbBridge
{
	class Program
	{
		const int BYTESPERPIXEL = 8;
		static string commPort = "COM11";
		static int baudRate = 3000000;
		static int pixelCount = 32;

		static readonly byte[] bitTriplets = new byte[]
		{
			0x5b, 0x1b, 0x53, 0x13,
			0x5a, 0x1a, 0x52, 0x12
		};

		static void Main(string[] args)
		{
			var colorBuffer = new int[pixelCount];
			var uartBuffer = new byte[pixelCount * BYTESPERPIXEL];

			Console.WriteLine("Bytes to send: {0}", uartBuffer.Length);

			using (var serialPort = new SerialPort(commPort, baudRate, Parity.None, 7, StopBits.One))
			{
				if (!serialPort.IsOpen)
					serialPort.Open();

				var chase = new LedEfects.ColorChase(pixelCount, 3, 6);

				for (;;)
				{
					TranslateColors(chase.MoveNext(), uartBuffer);
					serialPort.BaseStream.Write(uartBuffer, 0, uartBuffer.Length);
					serialPort.BaseStream.Flush();
					Thread.Sleep(25);
					if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X)
						break;
				}
			}
		}

		static void TranslateColors(int[] colors, byte[] UartData)
		{
			for (int i = 0; i < colors.Length; i++)
			{
				var color = colors[i];
				var pixOffset = i * BYTESPERPIXEL;
				
				//only 8 permutations so no need to use a for loop
				UartData[pixOffset] = bitTriplets[(color >> 21) & 0x07];
				UartData[pixOffset + 1] = bitTriplets[(color >> 18) & 0x07];
				UartData[pixOffset + 2] = bitTriplets[(color >> 15) & 0x07];
				UartData[pixOffset + 3] = bitTriplets[(color >> 12) & 0x07];
				UartData[pixOffset + 4] = bitTriplets[(color >> 9) & 0x07];
				UartData[pixOffset + 5] = bitTriplets[(color >> 6) & 0x07];
				UartData[pixOffset + 6] = bitTriplets[(color >> 3) & 0x07];
				UartData[pixOffset + 7] = bitTriplets[color & 0x07];
			}
		}
	}
}
