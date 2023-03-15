using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelizePass : ScriptableRenderPass
{
    [HideInInspector]
    public PixelizeFeature.CustomPassSettings settings;

    private RenderTargetIdentifier colorBuffer, pixelBuffer;
    private int pixelBufferID = Shader.PropertyToID("_PixelBuffer");

    private Material material;
    private int pixelScreenHeight, pixelScreenWidth;

    public PixelizePass(PixelizeFeature.CustomPassSettings settings)
    {
        this.settings = settings;
        this.renderPassEvent = settings.renderPassEvent;
        if (material == null) material = CoreUtils.CreateEngineMaterial("Hidden/Pixelize");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        pixelScreenHeight = settings.screenHeight;
        pixelScreenWidth = (int)(pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);

        colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        material.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
        Vector2 pixelSize = new Vector2(1.0f / (float)pixelScreenWidth, 1.0f / (float)pixelScreenHeight);
        material.SetVector("_BlockSize", pixelSize);
        material.SetVector("_HalfBlockSize", new Vector2(0.5f / (float)pixelScreenWidth, 0.5f / (float)pixelScreenHeight));

        Vector2 pixelOffset = new Vector2(
            (settings.pixelOffset.x % 1),
            (settings.pixelOffset.y % 1));
        pixelOffset = new Vector2(
            pixelOffset.x > .99 && pixelOffset.x < 1 ? 1 : pixelOffset.x,
            pixelOffset.y > .99 && pixelOffset.y < 1 ? 1 : pixelOffset.y
            );

        material.SetVector("_PixelOffset", pixelOffset);

        cmd.GetTemporaryRT(pixelBufferID, descriptor, FilterMode.Point);
        pixelBuffer = new RenderTargetIdentifier(pixelBufferID);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Pixelize Pass")))
        {
            Blit(cmd, colorBuffer, pixelBuffer, material);
            Blit(cmd, pixelBuffer, colorBuffer);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd == null) throw new System.ArgumentNullException("cmd");
        cmd.ReleaseTemporaryRT(pixelBufferID);
    }
}
