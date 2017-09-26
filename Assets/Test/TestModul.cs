using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModul : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Do();
    }

    public SpriteRenderer sr;
    protected void Do()
    {
        sr = this.GetComponent<SpriteRenderer>();
        Debug.Log("x : " + sr.sprite.texture.width+ " y : " + sr.sprite.texture.height);
    }

}
