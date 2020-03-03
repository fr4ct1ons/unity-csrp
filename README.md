# Cel-Shaded Render Pipeline  
A custom Scriptable Render Pipeline for Unity that focuses on cel-shading instead of PBR.  

![Sample image of the pipeline in action](https://github.com/lucena-fr4ct1ons/unity-csrp/blob/master/sample.png)  

## About  
With Unity abandoning the built-in render pipeline in favor of the new Universal Render Pipeline, it is becoming increasingly difficult to manipulate light information with custom shaders, something that already wasn't so easy to do before. The Cel-Shaded Render Pipeline is built with Unity's Scriptable Render Pipeline package in order to provide an easy way to render games with a toon-like lighting set. The CSRP aims to support multiple lights (not just a single directional light) normal mapping, specular mapping and post-processing.  
Special thanks to [Jasper Flick](https://catlikecoding.com/) for his amazing tutorials on rendering and SRP. Without his work, none of this would be possible!

## License
ASAP I will write a proper license for this package, but for now, you are free to use this package with any **non-profit product**. This means that if you are to profit from any project/product then you are not eligible to use the CSRP.  If you are not profitting in any way from this then you can use it for whatever you want as long as you modify it. Also, you don't have to credit me if you use the CSRP but please consider doing so, it helps a lot!  

## Roadmap (possible)  
0.1.x - The origins: Created the project with cel-shaded support for a single direcitonal light ✔️✔️  
0.2.x - The batching update: Basic GPU batching ✔️✔️  
0.3.x - The point light update: Support for multiple point lights ✔️  
0.4.x - The shadow update: Lit materials can receive shadows 🛠️  
0.5.x - The normal map update: Lit materials can support normal maps ❌  
0.6.x - The specular update: Lit materials can support specular maps & highlights ❌  
0.7.x - The post-processed update: Post-process now available with the pipeline ❌  
0.8.x - The baked update: Adding baked shadows and other light types ❌  
0.9.x - The polishing update: General polishing (multiple light bands, other details, testing everything out etc) ❌  
1.x.y - The final update: Documentation, tutorials and release! ❌  
  
Subtitles:  
_✔️✔️ Completely ready | ✔️ Only minor adjustments required | 🛠️ Work in progress | ❌ Development has not begun_  
 
## Contact
For now, you can contact me at https://twitter.com/fr4ct1ons or at my GitHub page.
