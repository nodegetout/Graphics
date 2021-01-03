using UnityEngine;
using UnityEngine.Rendering;

namespace ScorpioRP.Runtime
{
    [CreateAssetMenu(menuName = "Rendering/Scorpio Render Pipeline")]
    public class ScorpioRenderPipelineAsset : RenderPipelineAsset
    {
        [SerializeField]
        private bool useDynamicBatching = true, useGPUInstancing = true, useSRPBatcher = true;
        protected override RenderPipeline CreatePipeline()
        {
            return new ScorpioRenderPipeline(useDynamicBatching, useGPUInstancing, useSRPBatcher);
        }
    }
}