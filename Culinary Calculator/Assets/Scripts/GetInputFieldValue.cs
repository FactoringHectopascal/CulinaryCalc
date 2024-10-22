using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
public class GetInputFieldValue : MonoBehaviour
{
    [SerializeField]
    TMP_InputField cost;
    [SerializeField]
    TMP_InputField unit;
    [SerializeField]
    TMP_Text result;
    [SerializeField]
    Canvas canvas1;
    [SerializeField]
    Canvas canvas2;
    [SerializeField]
    TMPro.TMP_Dropdown dropDown;
    [SerializeField]
    TMP_InputField saveThing;
    [SerializeField]
    TMP_InputField saveTextField;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SwitchPanel()
    {
        canvas1.GetComponent<Canvas>().enabled = false;
        canvas2.GetComponent<Canvas>().enabled = true;
    }

    public void SwitchPanelOther()
    {
        canvas1.GetComponent<Canvas>().enabled = true;
        canvas2.GetComponent<Canvas>().enabled = false;
    }

    public void Calculate()
    {
        float.TryParse(unit.text, out float unitFloat);
        float.TryParse(cost.text, out float costFloat);
        result.text = "$" + (costFloat / unitFloat).ToString();
    }
    public void Save()
    {
        List<string> newOption = new() {"" + saveTextField.text};
        dropDown.AddOptions(newOption);
    }
}
