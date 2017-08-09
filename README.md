# NeoPixelUsbBridge
Simple program written in C# to control Neopixel (WS2812B & SK6812) leds directly from a PC without a microcontroller. The data stream is formed in the program memory and send to a common USB to Serial bridge.
## Suported usb to serial bridges
Any usb to serial converter should work as long as it can send data at 2Mbps minimum. 3Mbps produces a signal within the limits specified in the datasheet and would allow higher refresh rates. The following modules had been tested and work as expected:
  * FT232R
  * CH340
  * PL2303HX
  * CP2102N
## Limitations
Code tested only in Windows, but it should work on Linux too, with Mono. You can drive up to 512 leds with the Virtual Com Port drivers.
This limit are related to the driver which sends bursts of 4096 bytes. Because a delay in the bit stream sent to the NeoPixels could be interpreted as a reset signal, any delay inserted between this bursts introduce glitches in the intended sequence.

We use 8 bytes to encode the color data for a single Neopixel led thus we can control at most 512 leds glitch free (4096 / 8).

The CP2102N can drive many more leds and has been tested with up to 2048 leds without any specific change. The FT232R can also go higher than 512 leds but only when the direct API is used. The code does not use any manufacturer features.

You'll need an inverter at the TX pin. I used a simple NPN inverter for this task. The FT232R does not need this inverter as it can be configured to invert the TX signal via firmware.

![Alt Text](https://adanbucio.files.wordpress.com/2017/07/simple-transistor-inverter1.png)

