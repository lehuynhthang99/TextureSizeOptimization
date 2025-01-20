using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CreateOptimizePNG : MonoBehaviour
{

    [SerializeField] Button btnExport;
    [SerializeField] InputField inputFolder;
    [SerializeField] GameObject objSuccess;




    Color[] _fillPixels = new Color[3000 * 3000];

    Texture2D oldTextureInput;
    Texture2D newTextureOutput;

    

    private void Start()
    {
        oldTextureInput = new Texture2D(1, 1);
        newTextureOutput = new Texture2D(1, 1);
        Color fillColor = Color.clear;
        btnExport.onClick.AddListener(CreatePNG);
        inputFolder.onValueChanged.AddListener(OnInputChange);
    }

    private void OnInputChange(string input)
    {
        if (objSuccess.activeSelf)
        {
            objSuccess.SetActive(false);
        }
    }

    [ContextMenu("TestCreatePNG")]
    void CreatePNG()
    {
        objSuccess.SetActive(false);

        string inputPath = inputFolder.text.Replace('\\', '/');
        if (inputPath[inputPath.Length - 1] != '/')
        {
            inputPath += '/';
        }

        string outputPath = inputPath + "Optimize PNG/";

        if (!Directory.Exists(inputPath))
        {
            return;
        }
        string[] dirs = Directory.GetFiles(inputPath, "*.png");

        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        for (int i = 0; i < dirs.Length; i++)
        {
            string inputFile = dirs[i];
            //Debug.Log(inputFile);

            byte[] data = System.IO.File.ReadAllBytes(inputFile);
            oldTextureInput.LoadImage(data);

            int oldWidth = oldTextureInput.width;
            int oldHeight = oldTextureInput.height;

            int newWidth = oldWidth % 4 == 0 ? oldWidth : (oldWidth / 4 + 1) * 4;
            int newHeight = oldHeight % 4 == 0 ? oldHeight : (oldHeight / 4 + 1) * 4;

            int deltaX = (newWidth - oldWidth) / 2;
            int deltaY = (newHeight - oldHeight) / 2;

            newTextureOutput.Reinitialize(newWidth, newHeight);



            newTextureOutput.SetPixels(_fillPixels);
            newTextureOutput.SetPixels(deltaX, deltaY, oldWidth, oldHeight, oldTextureInput.GetPixels());

            newTextureOutput.Apply();

            byte[] bytes = newTextureOutput.EncodeToPNG();


            File.WriteAllBytes(outputPath + Path.GetFileName(inputFile), bytes);

            objSuccess.SetActive(true);

        }


    }

    private void OnDestroy()
    {
        Object.Destroy(oldTextureInput);
        Object.Destroy(newTextureOutput);
    }
}
