using UnityEngine;
using UnityEngine.Rendering;

namespace ScorpioRP.Runtime
{
    public class CameraRenderer
    {
        private const string bufferName = "Render Camera";

        // Why just SRPDefaultUnlit take effect;
        private static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

        static ShaderTagId[] legacyShaderTagIds =
        {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };

        private CommandBuffer m_Buffer = new CommandBuffer()
        {
            name = bufferName
        };

        private ScriptableRenderContext m_Context;
        private Camera m_Camera;
        private CullingResults m_CullingResults;

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            this.m_Context = context;
            this.m_Camera = camera;

            if (!Cull())
            {
                return;
            }

            Setup();
            DrawVisibleGeometry();
            DrawUnsupportedShaders();
            Submit();
        }

        bool Cull()
        {
            if (m_Camera.TryGetCullingParameters(out ScriptableCullingParameters p))
            {
                m_CullingResults = m_Context.Cull(ref p);
                return true;
            }

            return false;
        }

        void Setup()
        {
            m_Context.SetupCameraProperties(m_Camera);
            m_Buffer.ClearRenderTarget(true, true, Color.clear);
            m_Buffer.BeginSample(bufferName);
            ExecuteBuffer();
        }

        void ExecuteBuffer()
        {
            m_Context.ExecuteCommandBuffer(m_Buffer);
            m_Buffer.Clear();
        }

        void DrawVisibleGeometry()
        {
            var sortingSettings = new SortingSettings(m_Camera)
            {
                criteria = SortingCriteria.CommonOpaque
            };
            var drawingSetting = new DrawingSettings(
                unlitShaderTagId, sortingSettings
            );
            var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            m_Context.DrawRenderers(m_CullingResults, ref drawingSetting, ref filteringSettings);

            m_Context.DrawSkybox(m_Camera);

            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSetting.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;
            m_Context.DrawRenderers(m_CullingResults, ref drawingSetting, ref filteringSettings);
        }

        void DrawUnsupportedShaders()
        {
            var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(m_Camera));
            // var filteringSettings = new FilteringSettings();
            for (int i = 0; i < legacyShaderTagIds.Length; i++)
            {
                drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
            }
        }

        void Submit()
        {
            m_Buffer.EndSample(bufferName);
            ExecuteBuffer();
            m_Context.Submit();
        }
    }
}