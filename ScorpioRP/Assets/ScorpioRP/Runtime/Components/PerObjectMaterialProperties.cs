using UnityEngine;

namespace ScorpioRP.Runtime.Components
{
    [DisallowMultipleComponent]
    public class PerObjectMaterialProperties : MonoBehaviour
    {
        private static int s_BaseColorID = Shader.PropertyToID("_BaseColor");
        private static MaterialPropertyBlock s_MaterialPropertiesBlock;
        
        [SerializeField]
        Color baseColor = Color.white;

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
            
            s_MaterialPropertiesBlock.SetColor(s_BaseColorID, baseColor);
            GetComponent<Renderer>().SetPropertyBlock(s_MaterialPropertiesBlock);
        }
    }
}