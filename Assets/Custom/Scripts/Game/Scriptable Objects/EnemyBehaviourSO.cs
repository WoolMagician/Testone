using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "Scriptable Objects/Enemy Behaviour")]
public class EnemyBehaviourSO : ScriptableObject
{
    [SerializeField]
    public string behaviourTypeName;

    public EnemyBehaviour GetBehaviourScript()
    {
        if (behaviourTypeName == string.Empty) return null;
        return (EnemyBehaviour)Activator.CreateInstance(GetBehaviourTypeFromName(behaviourTypeName));
    }

    private Type GetBehaviourTypeFromName(string name)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where(type => type.IsSubclassOf(typeof(EnemyBehaviour)) && type.FullName == name)
                        .Select(type => type as Type).FirstOrDefault();
    }
}

[CustomEditor(typeof(EnemyBehaviourSO))]
public class EnemyBehaviourSOEditor : Editor
{
    public int selectedIndex = 0;

    public override void OnInspectorGUI()
    {
        List<Type> bTypes = GetAvailableBehaviourScriptTypes();
        List<string> bTypeNames = bTypes.Select(x => x.FullName).ToList();

        EnemyBehaviourSO enemyBehaviourSO = (EnemyBehaviourSO)target;
        serializedObject.Update();

        selectedIndex = Mathf.Clamp(EditorGUILayout.Popup("Behaviour Script", bTypeNames.IndexOf(enemyBehaviourSO.behaviourTypeName), bTypeNames.ToArray()), 0, bTypes.Count);

        if (selectedIndex < bTypes.Count)
        {
            enemyBehaviourSO.behaviourTypeName = bTypeNames[selectedIndex];
        }
        else
        {
            enemyBehaviourSO.behaviourTypeName = string.Empty;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private List<Type> GetAvailableBehaviourScriptTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where(type => type.IsSubclassOf(typeof(EnemyBehaviour)))
                        .Select(type => type as Type).ToList();
    }
}