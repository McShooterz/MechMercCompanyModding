using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(PostProcessPixelateRenderer), PostProcessEvent.BeforeStack, "Custom/Pixelate Post Process")]
public class PostProcessingPixelate : PostProcessEffectSettings
{
    public IntParameter pixelation = new IntParameter { value = 1 };
}

public sealed class PostProcessPixelateRenderer : PostProcessEffectRenderer<PostProcessingPixelate>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Custom/Pixelate"));
        sheet.properties.SetFloat("_PixelationPower", settings.pixelation);

        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, true).inverse;
        sheet.properties.SetMatrix("_ClipToView", clipToView);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}