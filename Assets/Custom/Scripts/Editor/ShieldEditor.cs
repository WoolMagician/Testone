using UnityEngine;
using UnityEditor;
using FXV;

//THE EDITOR SCRIPT FOR YOUR MONO CLASS
[CustomEditor(typeof(Shield)), CanEditMultipleObjects]
public class ShieldEditor : Editor
{
    //GameObject thisObjRef;

    //private void OnEnable()
    //{
    //    thisObjRef = ((Shield)target).gameObject;
    //}

    //private void OnDestroy()
    //{
    //    //The component was removed but not the gameobject
    //    if (target == null && thisObjRef != null)
    //    {
    //        //Remove Dependents
    //        DestroyImmediate(thisObjRef.GetComponent<MeshFilter>());
    //        DestroyImmediate(thisObjRef.GetComponent<MeshRenderer>());
    //        DestroyImmediate(thisObjRef.GetComponent<SphereCollider>());
    //        DestroyImmediate(thisObjRef.GetComponent<Rigidbody>());
    //        DestroyImmediate(thisObjRef.GetComponent<FXVShield>());
    //    }
    //}

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //}
}