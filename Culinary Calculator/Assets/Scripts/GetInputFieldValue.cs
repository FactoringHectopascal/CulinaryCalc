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
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
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
    string dbPath = "Assets/Text/Database.txt";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    PriceDB myPrices = new();
    
    float resultsNumber;

    [Serializable]
    class PriceDB
    {
        public List<Ingredient> ings = new();

        public void Add(Ingredient ingredient)
        {
            ings.Add(ingredient);
        }
    }

    [Serializable]
    public class Ingredient
    {
        public string name;
        public float price;
        public Ingredient (string name, float price)
        {
            this.name = name;
            this.price = price;
        }

        public string getLine()
        {
            return name + ", $" + price.ToString();
        }
    }

    void Start()
    {
        // load text file data
        StreamReader reader = new StreamReader(dbPath);
        string inJson = reader.ReadToEnd();
        myPrices = JsonUtility.FromJson<PriceDB>(inJson);

        // if the database object failed to load, create an empty one
        if (myPrices == null)
        {
            myPrices = new();
        }
        reader.Close();

        // update the list UI
        UpdateDropdown();

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
        myPrices.Add(new Ingredient(saveTextField.text, resultsNumber));
        UpdateDropdown();

        //List<string> newOption = new() {saveTextField.text};
        //dropDown.AddOptions(newOption);


        // write to file
        string outJson = JsonUtility.ToJson(myPrices);
        StreamWriter writer = new(dbPath, false);
        writer.WriteLine(outJson);
        writer.Close();

    }

    public void UpdateDropdown()
    {
        // populate list
        dropDown.ClearOptions();
        List<string> newOptions = new();
        for (int ii = 0; ii < myPrices.ings.Count; ii++)
        {
            newOptions.Add(myPrices.ings[ii].getLine());
        }
        dropDown.AddOptions(newOptions);
    }
}
