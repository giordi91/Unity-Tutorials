using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class parallelTransport : MonoBehaviour
{

    public GameObject[] objects;
    public float twistAmount;
    public float3 initialVector;
    public float debugVectorScale;

    public static float4x4 computeFrame(float3 start, float3 end, float3 up, float3 pos)
    {
        //compute an orthonormal frame from two points and an up vector
        float3 aim = math.normalize(start - end);
        float3 cross = math.normalize(math.cross(aim, up));
        up = math.normalize(math.cross(cross, aim));

        //generating the matrix
        return new float4x4(
            new float4(aim, 0.0f),
            new float4(up, 0.0f),
            new float4(cross, 0.0f),
            new float4(pos, 1.0f));
    }

    //creating a rotation matrix from a given axis and angle
    //https://en.wikipedia.org/wiki/Rotation_matrix#Rotation_matrix_from_axis_and_angle
    float4x4 getRotationMatrix(float3 axis, float angle)
    {
        float rad = math.radians(angle);
        float cosA = math.cos(rad);
        float sinA = math.sin(rad);
        return math.transpose( new float4x4
        (
            cosA + axis.x * axis.x * (1.0f - cosA), axis.x * axis.y * (1.0f - cosA) - axis.z * sinA,
            axis.x * axis.z * (1.0f - cosA) + axis.y * sinA, 0,
            axis.y * axis.x * (1.0f - cosA) + axis.z * sinA, cosA + axis.y * axis.y * (1.0f - cosA),
            axis.y * axis.z * (1.0f - cosA) - axis.x * sinA, 0,
            axis.z * axis.x * (1.0f - cosA) - axis.y * sinA, axis.z * axis.y * (1.0f - cosA) + axis.z * sinA,
            cosA + axis.z * axis.z * (1.0f - cosA), 0,
            0, 0, 0, 1
        ));

    }

    // Update is called once per frame
    void Update ()
	{
	    int objLen = objects.Length;
        float3[] positions = new float3[objLen];
        float4x4[] frames = new float4x4[objLen];

        //extracting the data from game objects
	    for (int i = 0; i < objLen; ++i)
	    {
	        positions[i] = objects[i].transform.position;
	    }

	    float3 up = initialVector;
        //calculate the frames
	    float twistStep = twistAmount / (float)(objLen - 1);
	    for (int i = 0; i < objLen-1; ++i)
	    {
            //computing the orthonormal frame
	        frames[i] = computeFrame(positions[i], positions[i + 1], up, positions[i]);
            //applying twist
	        frames[i] = math.mul( getRotationMatrix(frames[i].c0.xyz, twistStep*(float)i) , frames[i]);
            //critical part of parallel transport, the up vector gets updated at every step
	        up = frames[i].c1.xyz;
	    }

        //draw the frames
	    var root = transform.localToWorldMatrix;
	    for (int i = 0; i < objLen-1; ++i)
	    {
	        float3 currP = positions[i ];
            Debug.DrawLine(currP, currP + (frames[i].c1.xyz)*debugVectorScale , Color.green);
            Debug.DrawLine(currP, currP + (frames[i].c2.xyz)*debugVectorScale , Color.blue);
            Debug.DrawLine(currP, positions[i + 1]  , Color.red);
	    }
    }
}
