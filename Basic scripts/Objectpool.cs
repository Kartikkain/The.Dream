using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectpool : MonoBehaviour
{
    [SerializeField] GameObject enemypefab;
    [SerializeField] int poolsize=3;
    [SerializeField] float SpawnTime = 1.0f;
    float time;
    int number_of_child = 0;
    GameObject[] pool;
    // Start is called before the first frame update
    void Start()
    {
        time = SpawnTime;
        InstantiateObject();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(spawnenemy());   
    }

    // INSTANTIATE AN OBJECT WHEN THE GAME STARTS
   void InstantiateObject()
    {
        pool = new GameObject[poolsize]; 
        for(int i=0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(enemypefab, transform);
            pool[i].SetActive(false);
        }
    }

    // ENABLE THE NEW ENEMY WHEN THE PREVIOUS ENEMY IS DEAD
    void EnableEnemy()
    {
         
        // CHECKING IF THE CHILD COUNT IS GREATER THAN 1
        if (transform.childCount  >= 1)
       {
            //CHECKING IF THE FIRST CHILD IS ACTIVE OR NOT.
            if (!transform.GetChild(number_of_child).gameObject.activeInHierarchy)
            {
                //SETTING THE CHILD ACTIVE IF IT WAS NOT PREVIOUSLY
                transform.GetChild(number_of_child).gameObject.SetActive(true);

            }
            // INCREASE THE CHILD INDEX
            if(transform.GetChild(number_of_child).gameObject.GetComponent<Enemy>().enabled == false)
            {
                
                if(number_of_child <= (transform.childCount-2))
                {
                    time -= Time.deltaTime;
                    if(time <=0 )
                    {
                        Debug.Log("Child index is " + number_of_child++);
                        time = SpawnTime;
                    }
                }
            }
       }
        else
        {
            Destroy(gameObject);
        }
        
    }

    // SPAWNING THE ENEMY IN THE GAME
    IEnumerator spawnenemy()
    {
        EnableEnemy();
        yield return new WaitForSeconds(5); 
    }
}
