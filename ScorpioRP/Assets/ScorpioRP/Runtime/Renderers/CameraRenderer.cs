using UnityEngine;
using UnityEngine.Rendering;

namespace ScorpioRP.Runtime
{
    public partial class CameraRenderer
    {
        private const string bufferName = "Render Camera";

        // Why just SRPDefaultUnlit take effect;
        private static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

        private CommandBuffer m_Buffer = new CommandBuffer()
        {
            name = bufferName
        };

        private ScriptableRenderContext m_Context;
        private Camera m_Camera;
        private CullingResults m_CullingResults;

        public void Render(ScriptableRenderContext context, Camera camera, bool useDynamicBatching, bool useGPUInstancing)
        {
            this.m_Context = context;
            this.m_Camera = camera;

            PrepareBuffer();
            PrepareForSceneWindow();
            if (!Cull())
            {
                return;
            }

            Setup();
            DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
            DrawUnsupportedShaders();
            DrawGizmos();
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
            CameraClearFlags clearFlags = m_Camera.clearFlags;
            m_Buffer.ClearRenderTarget(
                clearFlags <= CameraClearFlags.Depth,
                clearFlags == CameraClearFlags.Color,
                clearFlags == CameraClearFlags.Color ? m_Camera.backgroundColor.linear : Color.clear);
            m_Buffer.BeginSample(bufferName);
            ExecuteBuffer();
        }

        void ExecuteBuffer()
        {
            m_Context.ExecuteCommandBuffer(m_Buffer);
            m_Buffer.Clear();
        }

        void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
        {
            var sortingSettings = new SortingSettings(m_Camera)
            {
                criteria = SortingCriteria.CommonOpaque
            };
            var drawingSetting = new DrawingSettings(
                unlitShaderTagId, sortingSettings
            )
            {
                enableDynamicBatching = useDynamicBatching,
                enableInstancing = useGPUInstancing
            };

            var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            m_Context.DrawRenderers(m_CullingResults, ref drawingSetting, ref filteringSettings);

            m_Context.DrawSkybox(m_Camera);

            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSetting.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;
            m_Context.DrawRenderers(m_CullingResults, ref drawingSetting, ref filteringSettings);
        }

        void Submit()
        {
            m_Buffer.EndSample(bufferName);
            ExecuteBuffer();
            m_Context.Submit();
        }
    }
}