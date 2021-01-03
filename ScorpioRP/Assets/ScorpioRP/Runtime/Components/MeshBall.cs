using UnityEngine;
using Random = UnityEngine.Random;

public class MeshBall : MonoBehaviour
{
    private static int s_BaseColorPropertyID = Shader.PropertyToID("_BaseColor");
    
    private const int MESH_BALL_COUNT = 1023;

    [SerializeField] private Mesh mesh = default;
    [SerializeField] private Material material = default;

    private Matrix4x4[] m_Matrices = new Matrix4x4[MESH_BALL_COUNT];
    private Vector4[] m_BaseColors = new Vector4[MESH_BALL_COUNT];
    private MaterialPropertyBlock m_MaterialPropertyBlock;

    private void Awake()
    {
        for (int i = 0; i < MESH_BALL_COUNT; i++)
        {
            m_Matrices[i] = Matrix4x4.TRS(
                Random.insideUnitSphere * 10f, Quaternion.identity, Vector3.one
            );
            m_BaseColors[i] = new Vector4(Random.value, Random.value, Random.value, 1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (material.enableInstancing == false)
        {
            Debug.LogError("Please enable material gpu instancing!!");
            return;
        }
        
        if (m_MaterialPropertyBlock == null)
        {
            m_MaterialPropertyBlock = new MaterialPropertyBlock();
            m_MaterialPropertyBlock.SetVectorArray(s_BaseColorPropertyID, m_BaseColors);
        }
        
        Graphics.DrawMeshInstanced(mesh, 0, material, m_Matrices, MESH_BALL_COUNT, m_MaterialPropertyBlock);
    }
}