using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour
{
    public GameObject grassTile;
    public GameObject waterTile;

    public int width = 50;
    public int height = 50;

    public float scale = 10.0f;
    public float amplitude = 1.0f;

    void Awake()
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                float sampleX = x / scale;
                float sampleZ = z / scale;
                float pNoiseVal = Mathf.PerlinNoise(sampleX, sampleZ) * amplitude;

               // float dist = Vector3.Distance(new Vector3(x, 0, z), new Vector3(width/2, 0, height/2));

                float xDistance = Vector3.Distance(new Vector3(x, 0, height/2), new Vector3(width/2, 0, z));
                float zDistance = Vector3.Distance(new Vector3(width/2, 0, z), new Vector3(x, 0, height/2));

                if ((xDistance>(height/3))||(zDistance>width/3))
                {
                    
                    float lerpedPNoiseVal = (getLerpXDistance(x, z) + getLerpZDistance(x, z));
                    pNoiseVal = (pNoiseVal + lerpedPNoiseVal) / 2;
                   pNoiseVal = Mathf.Clamp(pNoiseVal, 0.0f, 0.7f*amplitude);
                   
                    if (pNoiseVal < (0.8f*amplitude ))
                    {
                        pNoiseVal = pNoiseVal - 0.1f;
                    }

                   /* 
                    if(x > xDistance - 8)
                    {
                        pNoiseVal = Mathf.Clamp(pNoiseVal, 0.0f, 0.8f);
                    }
                    if (z > zDistance - 8)
                    {
                        pNoiseVal = Mathf.Clamp(pNoiseVal, 0.0f, 0.8f);
                    }    
                    */
                
                }

                if (pNoiseVal > 0.8f) {
                    Instantiate(grassTile, new Vector3(x, pNoiseVal, z), grassTile.transform.rotation);
                } else {
                    Instantiate(waterTile, new Vector3(x, pNoiseVal, z), waterTile.transform.rotation);
                }
            }
        }
    }

    float getLerpXDistance(int x, int z)
    {
        float xDist = Vector3.Distance(new Vector3(x, 0, height/2), new Vector3(width/2, 0, z));
        // Work out the distance from x to z max
        float scaledXDist = (xDist / (width / 2));
        // scale down into 0->1 range.
        float LerpXDistance = Mathf.Lerp(0.6f, 0, scaledXDist);
        //invert the result from the scale using lerp
        return (LerpXDistance*amplitude);
    }

    float getLerpZDistance(int x, int z)
    {
        float zDist = Vector3.Distance(new Vector3(width/2, 0, z), new Vector3(x, 0, height/2));
        // Work out the distance from x to z max
        float scaledZDist = (zDist / (height / 2));
        // scale down into 0->1 range.
        float LerpZDistance = Mathf.Lerp(0.6f, 0, scaledZDist);
        //invert the result from the scale using lerp
        return (LerpZDistance*amplitude);
    }
}
