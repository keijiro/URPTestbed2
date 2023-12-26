using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

sealed class OverlayRendererPass : ScriptableRenderPass
{
    public override void Execute
      (ScriptableRenderContext context, ref RenderingData data)
    {
        var control = data.cameraData.camera.GetComponent<OverlayController>();
        if (control == null || !control.IsReady) return;

        var cmd = CommandBufferPool.Get("Overlay");
        CoreUtils.DrawFullScreen(cmd, control.Material, control.Properties);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}

public sealed class OverlayRendererFeature : ScriptableRendererFeature
{
    OverlayRendererPass _pass;

    public override void Create()
      => _pass = new OverlayRendererPass
           { renderPassEvent = RenderPassEvent.AfterRenderingOpaques };

    public override void AddRenderPasses
      (ScriptableRenderer renderer, ref RenderingData data)
      => renderer.EnqueuePass(_pass);
}
