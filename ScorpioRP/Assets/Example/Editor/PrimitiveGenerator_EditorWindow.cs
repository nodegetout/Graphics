using ScorpioRP.Runtime.Components;
using UnityEditor;
using UnityEngine;

namespace Example.Editor
{
    public class PrimitiveGenerator : EditorWindow
    {
        private static readonly string k_PrimitiveTypeLable = "Primitive Type";
        private static readonly string k_DrawModeLable = "Draw Mode";
        private static readonly string k_PrimitiveCountLable = "Primitive Count";
        private static readonly string k_RandomRadiusVectorLable = "Random Radius Vector";
        private static readonly string k_GenerateButtonText = "Generate";

        private static readonly Rect k_PrimitiveTypeEnumPopPosition = new Rect(0, 5, 300, 45);
        private static readonly Rect k_DrawModeEnumPopPosition = new Rect(0, 25, 300, 45);
        private static readonly Rect k_GenerateButtonPosition = new Rect(0, 25, 300, 45);

        private PrimitiveType m_PrimitiveType = PrimitiveType.Cube;
        private DrawMode m_DrawMode = DrawMode.Random;
        private int m_PrimitiveCount;
        private Vector3 m_RandomRadiusVector;

        [MenuItem("ScorpioTools/Generator/Primitives")]
        private static void ShowWindow()
        {
            var window = GetWindow<PrimitiveGenerator>();
            window.titleContent = new GUIContent("Primitive Generator");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            m_PrimitiveType = (PrimitiveType) EditorGUI.EnumPopup(k_PrimitiveTypeEnumPopPosition, k_PrimitiveTypeLable, m_PrimitiveType);
            // EditorGUILayout.Space(5);
            m_DrawMode = (DrawMode) EditorGUI.EnumPopup(k_DrawModeEnumPopPosition, k_DrawModeLable, m_DrawMode);
            EditorGUILayout.Space(50);
            m_PrimitiveCount = EditorGUILayout.IntField(k_PrimitiveCountLable, m_PrimitiveCount);
            EditorGUILayout.Space(5);
            m_RandomRadiusVector = EditorGUILayout.Vector3Field(k_RandomRadiusVectorLable, m_RandomRadiusVector);
            EditorGUILayout.Space(5);
            if (GUILayout.Button(k_GenerateButtonText))
            {
                ExecuteGeneration();
            }

            EditorGUILayout.EndVertical();
        }

        private void ExecuteGeneration()
        {
            switch (m_DrawMode)
            {
                case DrawMode.Matrix:
                    break;
                default:
                    DrawPrimitiveRandomly();
                    break;
            }
        }

        private void DrawPrimitiveRandomly()
        {
            var parent = new GameObject();
            parent.name = "PrimitiveObjects";
            parent.transform.position = Vector3.zero;

            m_RandomRadiusVector.x = Mathf.Abs(m_RandomRadiusVector.x);
            m_RandomRadiusVector.y = Mathf.Abs(m_RandomRadiusVector.y);
            m_RandomRadiusVector.z = Mathf.Abs(m_RandomRadiusVector.z);

            for (int i = 0; i < m_PrimitiveCount; i++)
            {
                var go = GameObject.CreatePrimitive(m_PrimitiveType);
                go.transform.parent = parent.transform;
                var randomSphereVector = UnityEngine.Random.insideUnitSphere;
                randomSphereVector.x *= m_RandomRadiusVector.x;
                randomSphereVector.y *= m_RandomRadiusVector.y;
                randomSphereVector.z *= m_RandomRadiusVector.z;
                Debug.Log(randomSphereVector);
                go.transform.localPosition = randomSphereVector;
                var perObjectMaterialPropComponent = go.AddComponent<PerObjectMaterialProperties>();
                perObjectMaterialPropComponent.baseColor = new Color(randomSphereVector.x, randomSphereVector.y, randomSphereVector.z, Random.value);
            }
        }
    }

    enum DrawMode
    {
        Random,
        Matrix,
    }
}