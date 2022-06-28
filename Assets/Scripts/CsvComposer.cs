using SimpleJSON;
using System;
using System.IO;
using UnityEngine;

public class CsvComposer : MonoBehaviour
{
    private string filename;
    void Start()
    {
        CreateCSV();
    }

    void CreateCSV()
    {
        filename = Application.dataPath + "/Stonks_" + DateTime.Now.ToString("dd.MM.yyyy") + "_" + DateTime.Now.ToString("HH-mm-ss") + ".csv";
        using (TextWriter tw = new StreamWriter(filename, false))
        {
            //No "Name" of the stock, because API doesn't return it
            tw.WriteLine("Timestamp, Ticker, Value, Change");
        }
    }

    public void WriteToCSV(JSONNode stockData, string[] tickers)
    {
        using (TextWriter tw = new StreamWriter(filename, true))
        {
            for (int i = 0; i < tickers.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(tickers[i]))
                {
                    if (!stockData[tickers[i]]["code"])
                    {
                        string dateTime = stockData[tickers[i]]["values"][0]["datetime"];
                        float value = stockData[tickers[i]]["values"][0]["close"];
                        float prevValue = stockData[tickers[i]]["values"][1]["close"];
                        float change = value - prevValue;
                        Debug.Log(change + " = " + value + " - " + prevValue);
                        tw.WriteLine(dateTime + "," + tickers[i] + ",\"" + value + "\",\"" + change + "\"");
                    }
                }
            }
        }
    }
}
