using SimpleJSON;
using System;
using System.IO;
using TMPro;
using UnityEngine;

public class CsvComposer : MonoBehaviour
{
    private string filename;
    [SerializeField] private GameObject textWindow;
    void Start()
    {
        CreateCSV();
    }

    void CreateCSV()
    {
        //Creating new file with headers
        filename = Application.dataPath + "/Stonks_" + DateTime.Now.ToString("dd.MM.yyyy") + "_" + DateTime.Now.ToString("HH-mm-ss") + ".csv";
        using (TextWriter tw = new StreamWriter(filename, false))
        {
            //No "Name" of the stock, because API doesn't return it
            tw.WriteLine("Timestamp, Ticker, Value, Change");
        }
    }

    public void WriteToCSV(JSONNode stockData, string[] tickers)
    {
        //if no error code for the request — proceeding with parsing
        if (!stockData["code"])
        {
            using (TextWriter tw = new StreamWriter(filename, true))
            {
                //parse every ticker
                for (int i = 0; i < tickers.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(tickers[i]))
                    {
                        //catching error codes for a ticker
                        if (!stockData[tickers[i]]["code"])
                        {
                            string dateTime = stockData[tickers[i]]["values"][0]["datetime"];
                            float value = stockData[tickers[i]]["values"][0]["close"];
                            float prevValue = stockData[tickers[i]]["values"][1]["close"];
                            float change = value - prevValue;
                            textWindow.GetComponent<TMP_Text>().text += "succesfuly written data for " + tickers[i] + " at " + dateTime + "\n";
                            tw.WriteLine(dateTime + "," + tickers[i] + ",\"" + value + "\",\"" + change + "\"");
                        }
                        //showing error code
                        else
                        {
                            string code = stockData[tickers[i]]["code"];
                            textWindow.GetComponent<TMP_Text>().text += "no data for " + tickers[i] + " with error code: " + code + "\n";
                        }
                    }
                }
            }
        }
        
        else
        {
            //showing error code
            string code = stockData["code"];
            textWindow.GetComponent<TMP_Text>().text += "failed with error code: " + code + "\n";
        }
    }
}
