using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Goal : MonoBehaviour
{
    Material material;
    Transform localTransform;

    Vector4 rotationMatrix;
    Vector4 rotationMatrix2;
    Vector4 rotationMatrix3;
    Vector4 Pos1;
    Vector4 Pos2;
    Vector4 Cutoffs;

    float diffuseWrappedTime;
    float alphaWrappedTime;
    float alpha2WrappedTime;
    float alpha3WrappedTime;
    float aCutoff1WrappedTime;
    float aCutoff2WrappedTime;
    float aCutoff3WrappedTime;
    
    public float diffuseRotationSpeed = 0f;
    public float alphaRotationSpeed = 0f;
    public float alpha2RotationSpeed = 0f;
    public float alpha3RotationSpeed = 0f;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        localTransform = GetComponent<Transform>();
    }

    void Update()
    {
        diffuseWrappedTime = Mathf.Repeat(diffuseWrappedTime + diffuseRotationSpeed * Time.deltaTime, 360);
        alphaWrappedTime = Mathf.Repeat(alphaWrappedTime + alphaRotationSpeed * Time.deltaTime, 360);
        alpha2WrappedTime = Mathf.Repeat(alpha2WrappedTime + alpha2RotationSpeed * Time.deltaTime, 360);
        alpha3WrappedTime = Mathf.Repeat(alpha3WrappedTime + alpha3RotationSpeed * Time.deltaTime, 360);

        Matrix4x4 rMat1 = Matrix4x4.Rotate(Quaternion.Euler(0, 0, diffuseWrappedTime));
        Matrix4x4 rMat2 = Matrix4x4.Rotate(Quaternion.Euler(0, 0, alphaWrappedTime));
        Matrix4x4 rMat3 = Matrix4x4.Rotate(Quaternion.Euler(0, 0, alpha2WrappedTime));
        Matrix4x4 rMat4 = Matrix4x4.Rotate(Quaternion.Euler(0, 0, alpha3WrappedTime));

        material.SetVector("_Offset", new Vector4(localTransform.position.x - 0.5f, localTransform.position.y - 0.5f, 0f, 0f));
        material.SetMatrix("_RotationChR", rMat1);
        material.SetMatrix("_RotationChG", rMat2);
        material.SetMatrix("_RotationChB", rMat3);

        aCutoff1WrappedTime = Mathf.Repeat(aCutoff1WrappedTime + 0.7f * Time.deltaTime, Mathf.PI * 2);
        aCutoff2WrappedTime = Mathf.Repeat(aCutoff2WrappedTime + 1.4f * Time.deltaTime, Mathf.PI * 2);
        aCutoff3WrappedTime = Mathf.Repeat(aCutoff3WrappedTime + 2.9f * Time.deltaTime, Mathf.PI * 2);
        
        Pos1 = new Vector4(Mathf.Sin(aCutoff1WrappedTime) * 0.05f, Mathf.Cos(aCutoff1WrappedTime) * 0.05f, 0f, 0f);
        Pos2 = new Vector4(Mathf.Cos(aCutoff1WrappedTime) * 0.05f, Mathf.Sin(aCutoff1WrappedTime) * 0.05f, 0f, 0f);
        
        material.SetColor("_Pos1", Pos1);
        material.SetColor("_Pos2", Pos2);
    }
}
