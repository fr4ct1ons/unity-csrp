# Cel-Shaded Render Pipeline  
A custom Scriptable Render Pipeline for Unity that focuses on cel-shading instead of PBR.  

![Sample image of the pipeline in action](https://github.com/lucena-fr4ct1ons/unity-csrp/blob/master/sample.png)  

## About  
With Unity abandoning the built-in render pipeline in favor of the new Universal Render Pipeline, it is becoming increasingly difficult to manipulate light information with custom shaders, something that already wasn't so easy to do before. The Cel-Shaded Render Pipeline is built with Unity's Scriptable Render Pipeline package in order to provide an easy way to render games with a toon-like lighting set. The CSRP aims to support multiple lights (not just a single directional light) normal mapping, specular mapping and post-processing.  
Special thanks to [Jasper Flick](https://catlikecoding.com/) for his amazing tutorials on rendering and SRP. Without his work, none of this would be possible!

## License
Just don't modify the source code and publically provide its modifications and you can do whatever you want with it. This includes commercial use.

## Roadmap
Right now I want to add point light shadows as soon as possible. After that I will add BRDF to the lit shader.  
I guess that's all for now? Consider this the simplest roadmap out there.
 
## Contact
For now, you can contact me at https://twitter.com/fr4ct1ons or at my GitHub page.
