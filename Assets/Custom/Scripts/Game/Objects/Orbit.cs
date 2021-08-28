using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

public class Orbit : MonoBehaviour
{
    public OrbitData orbitData = new OrbitData();
    public BezierSpline OrbitSpline;

    // Start is called before the first frame update
    void Start()
    {
        this.OrbitSpline = this.GetComponent<BezierSpline>();       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}