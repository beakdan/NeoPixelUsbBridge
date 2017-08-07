# NeoPixelUsbBridge
Simple program written in C# to control Neopixel (WS2812B & SK6812) leds directly from a PC without a microcontroller. Instead the data stream is formed in the program memory and send to a common USB to Serial bridge.
## Suported usb to serial bridges
Any usb to serial converter should work as long as it can send data at 2mbps minimum but the following had been tested and worked OK
..*CHG340
..*FT232R
..*PL2303HX
..*CP2102N
## Limitations
You can drive up to 512 leds. The CP2102N can go higher and has been tested with up to 2048 leds. The FT232R can also go higher than 512 but only if you use the direct API. When this limit is exceeded, the intended sequence sometimes get broken.
