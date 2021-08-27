using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

[ExecuteInEditMode]
public class Orbit : BezierSpline
{
    public OrbitSO OrbitData;
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
