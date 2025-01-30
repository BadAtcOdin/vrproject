using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    public int Channel { get; set; }
    public int Step { get; set; }
    public GameObject Source { get; set; }
}

public class AudioManager : MonoBehaviour
{
    int bpm = 120;
    int grid = 16;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
