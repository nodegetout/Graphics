using UnityEngine;
using UnityEngine.Rendering;

namespace ScorpioRP.Runtime
{
    public class ScorpioRenderPipeline : RenderPipeline
    {
        CameraRenderer m_Renderer = new CameraRenderer();

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                m_Renderer.Render(context, camera);
            }
        }
    }
}