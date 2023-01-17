using UnityEngine;
using UnityEditor;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

public class AssemblyDefinitionData
{
    public string name = string.Empty;
#if UNITY_2020_2_OR_NEWER
    public string rootNamespace = string.Empty;
#endif
    public string[] references = new string[0];
    public string[] includePlatforms = new string[0];
    public string[] excludePlatforms = new string[0];
    public bool allowUnsafeCode = false;
    public bool overrideReferences = false;
    public string[] precompiledReferences = new string[0];
    public bool autoReferenced = false;
    public string[] defineConstraints = new string[0];
    public string[] versionDefines = new string[0];
    public bool noEngineReferences = false;
}

public class AsmdefGenerator : EditorWindow
{
    bool scanning = false;
    List<string> allFolders = new List<string>();
    List<string> scriptFolders = new List<string>();
    List<bool> toggleList = new List<bool>();
    List<string> destinationPaths = new List<string>();
    List<string> asmdefName = new List<string>();
    List<string> scriptFolderNames = new List<string>();
    Vector2 scrollPos = Vector2.zero;
    float progress = 0;

    [MenuItem("Tools/ASMDEF Generator")]
    public static void ShowWindow()
    {
        AsmdefGenerator window = GetWindow<AsmdefGenerator>("ASMDEF Generator");
        window.ScanFolders();
    }

    void OnGUI()
    {
        // GUI elements here
        if (scanning)
        {
            EditorGUILayout.LabelField("Scanning folders...");
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Box("", GUILayout.Width(position.width - 10), GUILayout.Height(20));
            GUILayout.EndHorizontal();
            Rect progressRect = GUILayoutUtility.GetLastRect();
            EditorGUI.ProgressBar(progressRect, progress, "");
        }
        else
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Scan Folders", GUILayout.Width(96)))
            {
                ScanFolders();
            }
            GUILayout.EndHorizontal();

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            if (destinationPaths != null)
            {
                for (int i = 0; i < destinationPaths.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    toggleList[i] = EditorGUILayout.Toggle(toggleList[i],GUILayout.Width(14));
                    destinationPaths[i] = EditorGUILayout.TextField(destinationPaths[i]);
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();

            EditorGUI.BeginDisabledGroup(true);
            if(destinationPaths != null)
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Select All"))
            {
                CreateDefinitions();
            }
            if (GUILayout.Button("Deselect All"))
            {
                CreateDefinitions();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create Editor Definitions"))
            {
                CreateDefinitions();
            }
            if (GUILayout.Button("Create Demo Definitions"))
            {
                CreateDefinitions();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Create All Definitions"))
            {
                CreateDefinitions();
            }

            EditorGUI.BeginDisabledGroup(true);
            if (GUILayout.Button("Create Selected Definitions"))
            {
                CreateDefinitions();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }
    }


    async void ScanFolders()
    {
        allFolders.Clear();
        scriptFolders.Clear();
        toggleList.Clear();
        destinationPaths.Clear();
        asmdefName.Clear();
        scriptFolderNames.Clear();
        scanning = true;
        await Task.Run(() =>
        {
            allFolders.AddRange(Directory.GetDirectories("Assets", "*", SearchOption.AllDirectories));
            
            for (int i = 0; i < allFolders.Count; i++)
            {
                string folder = allFolders[i];
                var parentName = new DirectoryInfo(folder).Name;
                if (Directory.GetFiles(folder, "*.cs", SearchOption.AllDirectories).Length > 0 && Directory.GetFiles(folder, "*.asmdef").Length == 0)
                {
                    scriptFolders.Add(folder);
                }

            }

            //scriptFolderNames = new string[scriptFolders.Count];
            //asmdefName = new string[scriptFolders.Count];


            for (int i = 0; i < scriptFolders.Count; i++)
            {
                string scriptFolder = scriptFolders[i];
                if (!scriptFolder.EndsWith("Editor") && !scriptFolder.EndsWith("Demo"))
                {
                    asmdefName.Add(new DirectoryInfo(scriptFolder).Name);
                }
                else
                {
                    asmdefName.Add(new DirectoryInfo(scriptFolder).Name);
                    if (Directory.GetDirectories(scriptFolder, "Editor").Length > 0)
                    {
                        asmdefName[i] += ".Editor";
                    }
                    else if (Directory.GetDirectories(scriptFolder, "Demo").Length > 0)
                    {
                        asmdefName[i] += ".Demo";
                    }
                }
                toggleList.Add(true);
                string currentDestinationPath = scriptFolder + "\\" + asmdefName[i] + ".asmdef";
                destinationPaths.Add(currentDestinationPath);
                // scriptFolderNames.Add(new DirectoryInfo(scriptFolders[i]).Name;
                progress = (float)i / 100;
            }
        });
        scanning = false;
    }

    void CreateDefinitions()
    {
        for (int i = 0; i < destinationPaths.Count; i++)
        {
            if (toggleList[i])
            {
                // why was this added? string directory = Path.GetDirectoryName(destinationPaths[i]);
                var asmdef = new AssemblyDefinitionData
                {
                    name = asmdefName[i],
                    allowUnsafeCode = false,
                    autoReferenced = true,
                    overrideReferences = false,
                    noEngineReferences = false,
                    includePlatforms = destinationPaths[i].EndsWith(".Editor") ? new[] { "Editor" } : new string[0]
                };

                var asmdefJson = JsonUtility.ToJson(asmdef, true);
                File.WriteAllText(destinationPaths[i], asmdefJson, Encoding.UTF8);
                AssetDatabase.Refresh();
                //destinationPaths.Remove(destinationPaths[i]); // Remove the processed item
            }
        }
        ScanFolders();
    }
}
