using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivateGameObjectOnEnable : MonoBehaviour {

    [SerializeField]
    List<GameObject> m_gameObjects = new List<GameObject>();

    [SerializeField]
    List<GameObject> m_gameObjectsToDeactivate = new List<GameObject>();

    void OnEnable()
    {
        Apply(true);
        _forceActivate = true;
    }

    void OnDisable()
    {
        Apply(false);
    }

    bool _forceActivate = true;
    void Update()
    {
        if (_forceActivate)
        {
            _forceActivate = false;
            Apply(true);
        }
    }

    public void Apply(bool p_value)
    {
        foreach (var v_object in m_gameObjects)
        {
            if (v_object != null)
                v_object.SetActive(p_value);
        }

        foreach (var v_object in m_gameObjectsToDeactivate)
        {
            if (v_object != null)
                v_object.SetActive(!p_value);
        }
    }
}
