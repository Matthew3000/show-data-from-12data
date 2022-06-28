using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsvComposer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string filename = Application.dataPath + "/test.csv";
        WriteToCSV(filename);
    }

    void WriteToCSV(string filename)
    {
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Код, Название, Значение, Изменение в процентах, Время обновления");
        tw.Close();
    }

}
