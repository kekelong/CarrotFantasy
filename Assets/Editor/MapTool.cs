using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Carrot;

[CustomEditor(typeof(MapMakerTool))]
public class MapTool : Editor {

    private MapMakerTool mapMakerTool;
    //�ؿ��ļ��б�
    private List<FileInfo> fileList = new List<FileInfo>();
    private string[] fileNameList;


    //��ǰ�༭�Ĺؿ�����
    private int selectIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            mapMakerTool = MapMakerTool.Instance;

            EditorGUILayout.BeginVertical();
            //��ȡ�������ļ���
            fileNameList = GetNames(fileList);
            int currentIndex = EditorGUILayout.Popup(selectIndex, fileNameList);
            if (currentIndex != selectIndex)//��ǰѡ������Ƿ�ı�
            {
                selectIndex = currentIndex;

                //ʵ������ͼ�ķ���
                mapMakerTool.InitMap();
                //���ص�ǰѡ���level�ļ�
                mapMakerTool.LoadLevelFile(mapMakerTool.LoadLevelInfoFile(fileNameList[selectIndex]));
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("��ȡ�ؿ��б�"))
            {
                LoadLevelFiles();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("�ָ���ͼ�༭��Ĭ��״̬"))
            {
                mapMakerTool.RecoverTowerPoint();
            }

            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("�������·��"))
            {
                mapMakerTool.ClearMonsterPath();
            }
            EditorGUILayout.EndVertical();



            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("���浱ǰ�ؿ������ļ�"))
            {
                mapMakerTool.SaveLevelFileByJson();
            }
            EditorGUILayout.EndVertical();
        }
    }

    //���عؿ������ļ�
    private void LoadLevelFiles()
    {
        ClearList();
        fileList = GetLevelFiles();
    }

    //����ļ��б�
    private void ClearList()
    {
        fileList.Clear();
        selectIndex = -1;
    }

    //�����ȡ�ؿ��б�
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

    //��ȡ�ؿ��ļ�������
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


