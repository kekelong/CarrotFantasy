using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Carrot;

[CustomEditor(typeof(MapMakerTool))]
public class MapTool : Editor {

    private MapMakerTool mapMakerTool;
    //关卡文件列表
    private List<FileInfo> fileList = new List<FileInfo>();
    private string[] fileNameList;


    //当前编辑的关卡索引
    private int selectIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            mapMakerTool = MapMakerTool.Instance;

            EditorGUILayout.BeginVertical();
            //获取操作的文件名
            fileNameList = GetNames(fileList);
            int currentIndex = EditorGUILayout.Popup(selectIndex, fileNameList);
            if (currentIndex != selectIndex)//当前选择对象是否改变
            {
                selectIndex = currentIndex;

                //实例化地图的方法
                mapMakerTool.InitMap();
                //加载当前选择的level文件
                mapMakerTool.LoadLevelFile(mapMakerTool.LoadLevelInfoFile(fileNameList[selectIndex]));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("读取关卡列表"))
            {
                LoadLevelFiles();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("恢复地图编辑器默认状态"))
            {
                mapMakerTool.RecoverTowerPoint();
            }

            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("清除怪物路点"))
            {
                mapMakerTool.ClearMonsterPath();
            }
            EditorGUILayout.EndVertical();



            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("保存当前关卡数据文件"))
            {
                mapMakerTool.SaveLevelFileByJson();
            }
            EditorGUILayout.EndVertical();
        }
    }

    //加载关卡数据文件
    private void LoadLevelFiles()
    {
        ClearList();
        fileList = GetLevelFiles();
    }

    //清除文件列表
    private void ClearList()
    {
        fileList.Clear();
        selectIndex = -1;
    }

    //具体读取关卡列表
    private List<FileInfo> GetLevelFiles()
    {
        string[] files = Directory.GetFiles(Application.dataPath+"/Resources/Json/Level/","*.json");
        
        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            list.Add(file);
        }
        return list;
    }

    //获取关卡文件的文字
    private string[] GetNames(List<FileInfo> files)
    {
        List<string> names = new List<string>();
        foreach (FileInfo file in files)
        {
            names.Add(file.Name);
        }
        return names.ToArray();
    }
}


