using System.IO;
using UnityEngine;
using UnityEditor.Formats.Fbx.Exporter; // Disable for build

public static class MeshExporter
{
    public static void ExportGameObjects(Object[] objects, string path)
    {
        //Its working. Disable for build
        path = "Exported/" + path + ".fbx";
        string filePath = Path.Combine(Application.dataPath, path);
        ModelExporter.ExportObjects(filePath, objects);
        Debug.Log("Exported to: " + Application.dataPath + path);
    }
}