# NeoPixelUsbBridge
Simple program written in C# to control Neopixel (WS2812B & SK6812) leds directly from a PC without a microcontroller. Instead the data stream is formed in the program memory and send to a common USB to Serial bridge.
## Suported usb to serial bridges
Any usb to serial converter should work as long as it can send data at 2mbps minimum but the following had been tested and worked OK
  * FT232R
  * CH340
  * PL2303HX
  * CP2102N
## Limitations
Code tested only in Windows. You can drive up to 512 leds with the Virtual Com Port drivers.
This limit are related to the driver which sends bursts of 4096 bytes. Because a delay in the bit stream sent to the NeoPixels could be interpreted as a reset signal, any delay inserted between this bursts could introduce glitches in the intended sequence.
We use 8 bytes to encode the color data for a single Neopixel led thus we can only control 512 leds glitch free.
The CP2102N can drive many more leds and has been tested with up to 2048 leds without any specific change. The FT232R can also go higher than 512 leds but only when the direct API is used.
