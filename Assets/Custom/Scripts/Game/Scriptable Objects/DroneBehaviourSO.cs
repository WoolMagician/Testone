using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "DroneBehaviour", menuName = "Scriptable Objects/Drone Behaviour")]
public class DroneBehaviourSO : ScriptableObject
{
    [SerializeField]
    public string behaviourTypeName;

    public DroneBehaviour GetBehaviourScript()
    {
        if (behaviourTypeName == string.Empty) return null;        
        return (DroneBehaviour)Activator.CreateInstance(GetBehaviourTypeFromName(behaviourTypeName));
    }

    private Type GetBehaviourTypeFromName(string name)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where(type => type.IsSubclassOf(typeof(DroneBehaviour)) && type.FullName == name)
                        .Select(type => type as Type).FirstOrDefault();
    }
}

[CustomEditor(typeof(DroneBehaviourSO))]
public class DroneBehaviourSOEditor : Editor
{
    public int selectedIndex = 0;

    public override void OnInspectorGUI()
    {
        List<Type> bTypes = GetAvailableBehaviourScriptTypes();
        List<string> bTypeNames = bTypes.Select(x => x.FullName).ToList();

        DroneBehaviourSO droneBehaviourSO = (DroneBehaviourSO)target;
        serializedObject.Update();

        selectedIndex = Mathf.Clamp(EditorGUILayout.Popup("Behaviour Script", bTypeNames.IndexOf(droneBehaviourSO.behaviourTypeName), bTypeNames.ToArray()), 0, bTypes.Count);

        if(selectedIndex < bTypes.Count)
        {
            droneBehaviourSO.behaviourTypeName = bTypeNames[selectedIndex];
        }
        else
        {
            droneBehaviourSO.behaviourTypeName = string.Empty;
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    private List<Type> GetAvailableBehaviourScriptTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where(type => type.IsSubclassOf(typeof(DroneBehaviour)))
                        .Select(type => type as Type).ToList();
    }
}