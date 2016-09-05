using UnityEngine;

public class ComputeShaderScript : MonoBehaviour {
    public ComputeShader computeShader;
    public const int vertexCount = 10*10*10*10*10*10;
    public ComputeBuffer outBuffer;

    public Shader pointShader;
    Material pointMat;

    public bool DebugRender = false;
    int csKernel;
    void InitBuffers()
    {
        outBuffer = new ComputeBuffer(vertexCount, (sizeof(float) * 3) + (sizeof(int) * 6));

        computeShader.SetBuffer(csKernel, "outputBuffer", outBuffer);
        if (DebugRender)
            pointMat.SetBuffer("buf_Points", outBuffer);
    }
    void Dispatch()
    {
        computeShader.Dispatch(csKernel, 10, 10, 10);
    }
    void Release()
    {
        outBuffer.Release();
    }

    void Start()
    {
        csKernel = computeShader.FindKernel("CSMain");
        if (DebugRender)
        {
            pointMat = new Material(pointShader);
            pointMat.SetVector("_worldPos", transform.position);
        }
        InitBuffers();
    }
    void OnRenderObject()
    {
        if (DebugRender)
        {
            Dispatch();
            pointMat.SetPass(0);
            pointMat.SetVector("_worldPos", transform.position);
            Graphics.DrawProcedural(MeshTopology.Points, vertexCount);
        }
    }
    void ReleaseBuffers()
    {
        outBuffer.Release();
    }
    void OnDisable()
    {
        ReleaseBuffers();
    }

}
