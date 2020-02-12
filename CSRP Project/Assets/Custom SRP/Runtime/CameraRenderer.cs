using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer {

    ScriptableRenderContext context;

    Camera camera;

    public void Render (ScriptableRenderContext context, Camera camera) {
        this.context = context;
        this.camera = camera;

        Setup();
        DrawVisibleGeometry();
        Submit();
    }

    private void Setup()
    {
        context.SetupCameraProperties(camera);
    }

    private void Submit()
    {
        context.Submit();
    }

    private void DrawVisibleGeometry()
    {
        context.DrawSkybox(camera);
    }
}