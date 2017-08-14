﻿using System.IO;
using Core;
using Frontend;
using HoloToolkit.Unity;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

public class ApplicationManager : Singleton<ApplicationManager>
{
    public UiNode Root;
    public AppState AppState;
    
    private readonly AppState InitialAppState = new AppState
    {
        FloorInteractionMode = new ReactiveProperty<FloorInteractionMode>(FloorInteractionMode.TapToMenu),
        AppData = null
    };
    
    private const string TreeDataFile = "TreeStructureTypes.json";
    private readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto
    };
    private string TreeDataPath;

    protected override void Awake()
    {
        AppState = InitialAppState;

        TreeDataPath = Path.Combine(Application.streamingAssetsPath, TreeDataFile);
        Root = DesirializeData();
        
        base.Awake();

//        SerializeData(Root);
    }

    private void Start()
    {
        TreeBuilder.Instance.GenerateTreeStructure((UiInnerNode) Root);
    }
    
    private void SerializeData(object obj)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented, JsonSerializerSettings);
        File.WriteAllText(TreeDataPath, json);
    }

    private UiInnerNode DesirializeData()
    {
        if (!File.Exists(TreeDataPath))
        {
            Debug.LogError("Connot load tree data, for there is no such file.");
            return null;
        }
        var json = File.ReadAllText(TreeDataPath);
        return JsonConvert.DeserializeObject<UiInnerNode>(json, JsonSerializerSettings);
    }
}