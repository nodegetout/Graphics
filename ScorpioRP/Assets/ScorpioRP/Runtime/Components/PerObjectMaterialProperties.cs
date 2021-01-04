using UnityEngine;

namespace ScorpioRP.Runtime.Components
{
    [DisallowMultipleComponent]
    public class PerObjectMaterialProperties : MonoBehaviour
    {
        private static int s_BaseColorPropertyID = Shader.PropertyToID("_BaseColor");
        private static int s_CutoffPropertyID    = Shader.PropertyToID("_Cutoff");
        private static MaterialPropertyBlock s_MaterialPropertiesBlock;
        
        // [SerializeField]
        public Color baseColor = Color.white;
        [Range(0.0f, 1.0f)]
        public float cutoff = 0.5f;

        void Awake()
        {
            UpdateMaterialPropertiesBlock();
        }

        void OnValidate()
        {
            UpdateMaterialPropertiesBlock();
        }

        private void UpdateMaterialPropertiesBlock()
        {
            if (s_MaterialPropertiesBlock == null)
            {
                s_MaterialPropertiesBlock = new MaterialPropertyBlock();
            }
            
            s_MaterialPropertiesBlock.SetColor(s_BaseColorPropertyID, baseColor);
            s_MaterialPropertiesBlock.SetFloat(s_CutoffPropertyID,    cutoff);
            GetComponent<Renderer>().SetPropertyBlock(s_MaterialPropertiesBlock);
        }
    }
}