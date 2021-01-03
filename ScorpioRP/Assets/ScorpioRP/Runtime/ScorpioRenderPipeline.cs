using UnityEngine;
using UnityEngine.Rendering;

namespace ScorpioRP.Runtime
{
    public class ScorpioRenderPipeline : RenderPipeline
    {
        private bool useDynamicBatching, useGPUInstancing;
        CameraRenderer m_Renderer = new CameraRenderer();

        public ScorpioRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
        {
            GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
            this.useDynamicBatching = useDynamicBatching;
            this.useGPUInstancing = useGPUInstancing;
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                m_Renderer.Render(context, camera, useDynamicBatching, useGPUInstancing);
            }
        }
    }
}