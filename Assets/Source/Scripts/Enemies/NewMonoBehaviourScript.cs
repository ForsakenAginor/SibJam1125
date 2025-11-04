using TMPro;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private TMP_Text _Text;
    [SerializeField] private Transform _target;
    
    // Update is called once per frame
    void Update()
    {
        _Text.text = _target.position.ToString();
    }
}


