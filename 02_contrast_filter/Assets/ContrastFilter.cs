using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ContrastFilterRenderer), PostProcessEvent.AfterStack, "Custom/Contrast")]
public sealed class ContrastFilter : PostProcessEffectSettings
{
    [Range(-1.0f, 1.0f), Tooltip("contrastFilter effect intensity.")]
    public FloatParameter contrast = new FloatParameter { value = 0.0f };
}

public sealed class ContrastFilterRenderer : PostProcessEffectRenderer<ContrastFilter>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Contrast"));
        sheet.properties.SetFloat("_Contrast", settings.contrast);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}