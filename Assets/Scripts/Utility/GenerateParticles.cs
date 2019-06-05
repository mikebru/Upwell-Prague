using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class GenerateParticles : MonoBehaviour
{
    public GameObject ParticlePrefab;

    [Range(0,200)]
    public int SpawnCount;

    public float Radius;

    public List<GameObject> instances;

    [Range(0,255)]
    public int RandomSeed;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Spawn()
    {

        Random.InitState(RandomSeed);

        for(int i=0; i< SpawnCount; i++)
        {

            Vector3 randomPosition = new Vector3(Random.Range(-Radius, Radius), Random.Range(0, Radius), Random.Range(-Radius, Radius));

            GameObject newObject = Instantiate(ParticlePrefab, transform);

            newObject.transform.localPosition = randomPosition;

            instances.Add(newObject);
        }


    }


#if UNITY_EDITOR
    private void OnValidate()
    {

        // Trick for making Destroy work

        EditorApplication.delayCall += () =>
        {foreach (GameObject g in instances) { DestroyImmediate(g); }
            instances = new List<GameObject>();

            Spawn();
        };


    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);


    }
#endif




}
