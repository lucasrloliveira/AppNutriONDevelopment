using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(RefeicaoManager), true)]
public class RefeicaoManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Ordena Alphabeticamente"))
        {
            Order();
        }
        base.OnInspectorGUI();
    }

    public void Order()
    {
        serializedObject.Update();
        RefeicaoManager v_manager = target as RefeicaoManager;
        var v_arrayDefinicao = v_manager.DefinicaoDeAlimentos.ToArray();
        System.Array.Sort(v_arrayDefinicao, 
            delegate (Aula.Alimento x, Aula.Alimento y) 
            {
                return System.String.Compare(x.Key, y.Key);
            });
        v_manager.DefinicaoDeAlimentos = new System.Collections.Generic.List<Aula.Alimento>(v_arrayDefinicao);

        var v_arrayRefeicao = v_manager.RefeicoesDefinitions.ToArray();
        System.Array.Sort(v_arrayRefeicao, 
            delegate (Aula.RefeicaoPredefinida x, Aula.RefeicaoPredefinida y) 
            {
                return System.String.Compare(x.Key, y.Key);
            });
        v_manager.RefeicoesDefinitions = new System.Collections.Generic.List<Aula.RefeicaoPredefinida>(v_arrayRefeicao);

        EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
