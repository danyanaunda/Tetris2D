using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    public List<int> randomize;
    
    private int GetRandom() 
    {
        int n = Random.Range(0,100);
        int total = 0;
        for (int i = 0; i < randomize.Count; i++)
        {
            total += randomize[i];
            
            if (total >= n) return i;
        }
        return 0;

    }


    public GameObject NewTetromino()
    {
        return Instantiate(Tetrominoes[GetRandom()], transform.position, Quaternion.identity);
    }
}
