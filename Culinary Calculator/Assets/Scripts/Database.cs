using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
public class Database : MonoBehaviour
{


    [SerializeField]
    TMPro.TMP_Dropdown dropDown;
    [SerializeField]
    TMPro.TMP_Dropdown unitType;
    [SerializeField]
    TMP_InputField nameTextField;
    [SerializeField]
    string dbPath = "Assets/Text/Database.txt";
    Ingredient currIngredient;
    [SerializeField]
    public TMP_InputField cost;
    [SerializeField]
    TMP_InputField units;
    float totalPrice = 0f;
    [SerializeField]
    TMP_Text qtyResult;
    [SerializeField]
    TMP_Text totalPriceText;
    [SerializeField]
    TMP_InputField markupField;
    [SerializeField]
    TMP_InputField servingsField;
    [SerializeField]
    TMP_Text servingFieldNumber;
    [SerializeField]
    TMP_Text markupPriceNumber;
    [SerializeField]
    public TMP_InputField qty;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    PriceDB myPrices = new();



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
        public float units;
        public string unitType;
        public Ingredient(string name, float price, float units, string unitType)
        {
            this.name = name;
            this.price = price;
            this.units = units;
            this.unitType = unitType;
        }

        public string getLine()
        {
            //return name + ", $" + price.ToString();
            return name + ", $" + MathF.Round(price / units, 2).ToString() + " per " + unitType;
        }
    }

    void Start()
    {   // if the database.json doesn't exist, create a new one
        if (!File.Exists(dbPath))
        {
            File.Create(dbPath).Close();
        }
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

        if (myPrices.ings.Count != 0)
            LoadCurrIngredient(dropDown.value);

        dropDown.onValueChanged.AddListener(delegate
        {
            LoadCurrIngredient(dropDown.value);
        });

        markupField.onValueChanged.AddListener(delegate
        {
            UpdateMarkupServings();
        });

        servingsField.onValueChanged.AddListener(delegate
        {
            UpdateMarkupServings();
        });
        qty.onValueChanged.AddListener(delegate
        {
            UpdateMarkupServings();
        });
    }

    // Update is called once per frame
    void Update()
    {
    }

    // parse the text inside of the input fields as floats so you can divide them and round them, then output it to the result text

    // add a new object of type "Ingredient" with it's name and price data
    // then update the dropdown
    public void Save()
    {
        myPrices.Add(new Ingredient(nameTextField.text, float.Parse(cost.text), float.Parse(units.text), unitType.captionText.text));
        UpdateDropdown();

        //List<string> newOption = new() {saveTextField.text};
        //dropDown.AddOptions(newOption);


        // write to file

        SaveJSON();
    }

    public void SaveJSON()
    {
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

    public void DeleteCurrIngredient()
    {
        myPrices.ings.Remove(currIngredient);
        SaveJSON();
        UpdateDropdown();

        if (myPrices.ings.Count != 0)
            LoadCurrIngredient(dropDown.value);
    }

    public void LoadCurrIngredient(int index)
    {
        currIngredient = myPrices.ings[index];
        nameTextField.text = currIngredient.name;
        cost.text = currIngredient.price.ToString();
        units.text = currIngredient.units.ToString();
        unitType.captionText.text = currIngredient.unitType;
    }

    public void AddQuantityToTotal()
    {
        if (qtyResult.text == "-.--")
            return;
        totalPrice += float.Parse(qtyResult.text);
        //Debug.Log(float.Parse(qtyResult.text));
        totalPriceText.text = MathF.Round(totalPrice, 2).ToString();
        UpdateMarkupServings();

    }

    public void ClearTotal()
    {
        totalPrice = 0f;
        totalPriceText.text = "-.--";
        markupPriceNumber.text = "-.--";
        servingFieldNumber.text = "-.--";
        servingsField.text = "";
        markupField.text = "";
    }

    public void UpdateMarkupServings()
    {
        if (totalPrice == 0) return;

        if (float.TryParse(markupField.text, out float markupFloat) == false)
        {
            markupPriceNumber.text = "-.--";
            servingFieldNumber.text = "-.--";
            return;
        }
        float markupTotal = MathF.Round(totalPrice * (1 + markupFloat), 2);
        markupPriceNumber.text = markupTotal.ToString();
        if (float.TryParse(servingsField.text, out float servingFloat) == false)
        {
            servingFieldNumber.text = "-.--";
            return;
        }
        servingFieldNumber.text = (MathF.Round(markupTotal / float.Parse(servingsField.text), 2)).ToString();
    }
}
