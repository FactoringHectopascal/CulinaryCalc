using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Rendering;
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
    [SerializeField]
    string dbPath = "Assets/Text/Bruh.txt";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PriceDB myPrices = new();
    float resultsNumber;

    class PriceDB
    {
        public List<string> ingNames = new();
        public List<float> ingPrices = new();
    }


    void Start()
    {
        StreamReader reader = new StreamReader(dbPath);
        PriceDB testPrices = JsonUtility.FromJson<PriceDB>(reader.ReadToEnd());
        reader.Close();
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
        resultsNumber = MathF.Round((costFloat / unitFloat) * 100.0f) * 0.01f;
        result.text = "$" + resultsNumber.ToString();
    }
    public void Save()
    {
        //List<string> newOption = new() {saveTextField.text};
        myPrices.ingNames.Add(saveTextField.text);
        myPrices.ingPrices.Add(resultsNumber);
        List<string> newOption = new() {saveTextField.text};
        dropDown.AddOptions(newOption);

        string testJason = JsonUtility.ToJson(myPrices);

        StreamWriter writer = new StreamWriter(dbPath, false);
        writer.WriteLine(testJason);
        writer.Close();

    }

}
