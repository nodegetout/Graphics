using UnityEngine;
using UnityEngine.Rendering;

namespace ScorpioRP.Runtime
{
    public partial class CameraRenderer
    {
        partial void DrawUnsupportedShaders();
#if UNITY_EDITOR
        static ShaderTagId[] legacyShaderTagIds =
        {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };

        static Material m_ErrorMaterial;

        partial void DrawUnsupportedShaders()
        {
            // If without error material override, standard material display fine.Why?
            if (m_ErrorMaterial == null)
            {
                m_ErrorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
            }

            var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(m_Camera))
            {
                overrideMaterial = m_ErrorMaterial
            };

            for (int i = 0; i < legacyShaderTagIds.Length; i++)
            {
                drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
            }

            var filteringSettings = FilteringSettings.defaultValue;
            m_Context.DrawRenderers(m_CullingResults, ref drawingSettings, ref filteringSettings);
        }
#endif
    }
}