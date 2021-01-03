using UnityEngine;
using UnityEngine.Rendering;

namespace ScorpioRP.Runtime
{
    public partial class CameraRenderer
    {
        partial void DrawGizmos();
        partial void DrawUnsupportedShaders();
        partial void PrepareForSceneWindow();
        partial void PrepareBuffer();

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

        static Material s_ErrorMaterial;
        private string SampleName { get; set; }

        partial void DrawUnsupportedShaders()
        {
            // If without error material override, standard material display fine.Why?
            if (s_ErrorMaterial == null)
            {
                s_ErrorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
            }

            var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(m_Camera))
            {
                overrideMaterial = s_ErrorMaterial
            };

            for (int i = 0; i < legacyShaderTagIds.Length; i++)
            {
                drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
            }

            var filteringSettings = FilteringSettings.defaultValue;
            m_Context.DrawRenderers(m_CullingResults, ref drawingSettings, ref filteringSettings);
        }

        partial void DrawGizmos()
        {
        }

        partial void PrepareForSceneWindow()
        {
            if (m_Camera.cameraType is CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(m_Camera);
            }
        }

        partial void PrepareBuffer()
        {
            m_Buffer.name = SampleName = m_Camera.name;
        }
#else
    const string SampleName = buffer.name;
#endif
    }
}