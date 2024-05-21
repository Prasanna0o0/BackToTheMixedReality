using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneModelClassificator : MonoBehaviour
{
    private OVRSemanticClassification _classification;

    public string semanticClass;
    // Start is called before the first frame update
    void Start()
    {
        _classification = GetComponent<OVRSemanticClassification>();
        TagSceneObjectsBasedOnClass();
    }


    void TagSceneObjectsBasedOnClass()
    {
        if (_classification != null)
        {
            semanticClass = _classification.ToString();
            if (!_classification.Contains(OVRSceneManager.Classification.Floor))
            {
                transform.tag = Tags.WallsAndVolumes;
                transform.gameObject.layer = LayerMask.NameToLayer("WallAndVolume");

                foreach (Transform t in transform)
                {
                    t.tag = Tags.WallsAndVolumes;
                    t.gameObject.layer = LayerMask.NameToLayer("WallAndVolume");

                }
            }

            if (_classification.Contains(OVRSceneManager.Classification.Floor))
            {
                transform.gameObject.layer = LayerMask.NameToLayer("Floor");
                foreach (Transform t in transform)
                {
                    t.gameObject.layer = LayerMask.NameToLayer("Floor");

                }
            }
        }
    }


}
