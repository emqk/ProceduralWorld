using System.IO;
using UnityEngine;
//using UnityEditor.Formats.Fbx.Exporter;

public static class MeshExporter
{
    public static void ExportGameObjects(Object[] objects, string path)
    {
        //string path = "Exported/" + "Trees/" + "MyObject.fbx";


        //Its working. Disable for build
        //path = "Exported/" + path + ".fbx";
        //string filePath = Path.Combine(Application.dataPath, path);
        //ModelExporter.ExportObjects(filePath, objects);
        //Debug.Log("Exported to: " + Application.dataPath + path);
        


        // ModelExporter.ExportObject can be used instead of 
        // ModelExporter.ExportObjects to export a single game object
    }


    //It works, but it saves whole scene in format that need to be imported with special function(Which is available in docs). I need to export to raw .fbx
    /*public static void ExportScene(string fileName)
    {
        using (FbxManager fbxManager = FbxManager.Create())
        {
            // configure IO settings.
            fbxManager.SetIOSettings(FbxIOSettings.Create(fbxManager, Globals.IOSROOT));

            // Export the scene
            using (FbxExporter exporter = FbxExporter.Create(fbxManager, "myExporter"))
            {

                // Initialize the exporter.
                bool status = exporter.Initialize(fileName, -1, fbxManager.GetIOSettings());

                // Create a new scene to export
                FbxScene scene = FbxScene.Create(fbxManager, "myScene");

                // Export the scene to the file.
                exporter.Export(scene);
            }
        }
    }*/
}