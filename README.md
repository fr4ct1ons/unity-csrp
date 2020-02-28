# Cel-Shaded Render Pipeline  
A custom Scriptable Render Pipeline for Unity that focuses on cel-shading instead of PBR.  

![Sample image of the pipeline in action](https://github.com/lucena-fr4ct1ons/unity-csrp/blob/master/sample.png)  

## About  
With Unity abandoning the built-in render pipeline in favor of the new Universal Render Pipeline, it is becoming increasingly difficult to manipulate light information with custom shaders, something that already wasn't so easy to do before. The Cel-Shaded Render Pipeline is built with Unity's Scriptable Render Pipeline package in order to provide an easy way to render games with a toon-like lighting set. The CSRP aims to support multiple lights (not just a single directional light) normal mapping, specular mapping and post-processing.

## License
ASAP I will write a proper license for this package, but for now, you are free to use this package with any **non-profit product**. This means that if you are to profit from any project/product then you are not eligible to use the CSRP.  If you are not profitting in any way from this then you can use it for whatever you want as long as you modify it. Also, you don't have to credit me if you use the CSRP but please consider doing so, it helps a lot!  

## Roadmap (not finished)  
0.1.x - The origins: Created the project with cel-shaded support for a single direcitonal light ✔️✔️  
0.2.x - The batching update: Basic GPU batching ✔️  
0.3.x - The point light update: Support for multiple lights ✔️  
0.4.x - The shadow update: Lit materials can receive shadows ❌  
0.5.x - The normal map update: Lit materials can support normal maps ❌  
*✔️✔️ Completely ready  
✔️ Only minor adjustments required, but already available  
🛠️ Currently work in progress  
❌ Development has not begun*
**More versions to come!**
