using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBase : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawntemplate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public Vector3 GetARandomTreePos()
    {

        Mesh planeMesh = gameObject.GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;

        float minX = gameObject.transform.position.x - gameObject.transform.localScale.x * bounds.size.x * 0.5f;
        float minZ = gameObject.transform.position.z - gameObject.transform.localScale.z * bounds.size.z * 0.5f;
        Debug.Log(minX);
        Debug.Log("ssusy");
        Debug.Log(minZ);
        Vector3 newVec = new Vector3(UnityEngine.Random.Range(minX, minX+gameObject.transform.localScale.x*bounds.size.x),
                                     gameObject.transform.position.y,
                                     UnityEngine.Random.Range(minZ, minZ+gameObject.transform.localScale.z*bounds.size.z));
        return newVec;
    }

    public void SpawnX(int amount, int interval)
    {
        for (int x = 0; x < amount; x++)
        {
            Vector3 spawnpos = GetARandomTreePos();
            GameObject NewProjectile = Instantiate(spawntemplate, spawnpos + new Vector3 (0f,1f,0f), gameObject.transform.rotation);
        }
    }
}
