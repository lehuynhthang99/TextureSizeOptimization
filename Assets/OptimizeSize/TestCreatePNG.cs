using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestCreatePNG : MonoBehaviour
{
    [SerializeField] string userInputPath;
    [SerializeField] string userOutputPath;

    Color[] _fillPixels = new Color[3000 * 3000];

    Texture2D oldTextureInput = new Texture2D(1, 1);
    Texture2D newTextureOutput = new Texture2D(1, 1);

    private void Start()
    {
        Color fillColor = Color.clear;
    }

    [ContextMenu("TestCreatePNG")]
    void CreatePNG()
    {
        string inputPath = userInputPath.Replace('\\',  '/');
        string outputPath = userOutputPath.Replace('\\',  '/');

        string[] dirs = Directory.GetFiles(inputPath, "*.png");

        for (int i = 0; i < dirs.Length; i++)
        {
            string inputFile = dirs[i];
            Debug.Log(inputFile);

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
        }

        Object.DestroyImmediate(oldTextureInput);
        Object.DestroyImmediate(newTextureOutput);
    }
}
