using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ModelType
{
    Null,
    Player,
    Monster,
    Collect
}

public class SceneModelData
{
    public string prefabName;
    public ModelType type;

    public float posX;
    public float posY;
    public float posZ;

    public float scaleX;
    public float scaleY;
    public float scaleZ;

    public float rotX;
    public float rotY;
    public float rotZ;
}


public class SceneEditor : EditorWindow
{
    static Transform parentTransform;

    static GameObject[] selectObjects;
    static int[] typeArr;

    string[] typeName = { "Null", "Player", "Monster", "Collect" };

    [MenuItem("Editor/场景编辑器")]
    private static void SceneEditorMethod()
    {
        parentTransform = GameObject.Find("MapRoot").transform;
        SceneEditor sceneEditor = EditorWindow.GetWindow<SceneEditor>();
        sceneEditor.Show();
        selectObjects = Selection.gameObjects;
        typeArr = new int[selectObjects.Length];
    }



    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("读取本地配置"))
        {
            Debug.Log("读取本地配置");

            Scene scene = SceneManager.GetActiveScene();
            string path = Application.dataPath + "/Resources/" + scene.name + ".json";
            Debug.Log(path);
            if (File.Exists(path))
            {
                string s = File.ReadAllText(path);
                List<SceneModelData> data = JsonConvert.DeserializeObject<List<SceneModelData>>(s);
                for (int i = 0; i < data.Count; i++)
                {
                    CreateModel(data[i]);
                }
            }
            else
            {
                Debug.LogWarning("这个场景没有本地配置！");
            }
            //File.WriteAllText(Application.dataPath + "/Resources/" + scene.name + ".json", str, Encoding.UTF8);
        }
        if (GUILayout.Button("保存地图数据"))
        {
            Debug.Log("保存地图数据");
            List<SceneModelData> list = new List<SceneModelData>();

            for (int i = 0; i < selectObjects.Length; i++)
            {
                SceneModelData modelData = new SceneModelData();
                GameObject obj = selectObjects[i];
                //取模型的名字
                string name = obj.name;
                //去掉空字符串
                name = name.Replace(" ", "");
                name = name.Split('(')[0];

                modelData.prefabName = name;
                modelData.posX = obj.transform.position.x;
                modelData.posY = obj.transform.position.y;
                modelData.posZ = obj.transform.position.z;

                modelData.rotX = obj.transform.eulerAngles.x;
                modelData.rotY = obj.transform.eulerAngles.y;
                modelData.rotZ = obj.transform.eulerAngles.z;

                modelData.scaleX = obj.transform.localScale.x;
                modelData.scaleY = obj.transform.localScale.y;
                modelData.scaleZ = obj.transform.localScale.z;

                modelData.type = (ModelType)typeArr[i];

                Debug.Log(name);
                list.Add(modelData);
            }
            //JsonUtility.ToJson(list, true);
            string str = JsonConvert.SerializeObject(list);
            Scene scene = SceneManager.GetActiveScene();
            string path = Application.dataPath + "/Resources/" + scene.name + ".json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            Debug.LogWarning(str + list.Count);
            File.WriteAllText(Application.dataPath + "/Resources/" + scene.name + ".json", str, Encoding.UTF8);
            AssetDatabase.Refresh();
        }
        GUILayout.EndHorizontal();

        for (int i = 0; i < selectObjects.Length; i++)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(selectObjects[i].name))
            {

            }
            typeArr[i] = EditorGUILayout.Popup(typeArr[i], typeName);
            GUILayout.EndHorizontal();
        }

    }
    void CreateModel(SceneModelData data)
    {
        GameObject clone = GameObject.Instantiate(Resources.Load<GameObject>("Role/" + data.prefabName), parentTransform, false);
        clone.transform.position = new Vector3(data.posX, data.posY, data.posZ);
        clone.transform.eulerAngles = new Vector3(data.rotX, data.rotY, data.rotZ);
        clone.transform.localScale = new Vector3(data.scaleX, data.scaleY, data.scaleZ);


        switch (data.type)
        {
            case ModelType.Null:
                break;
            case ModelType.Player:
                break;
            case ModelType.Monster:
                break;
            case ModelType.Collect:
                break;
            default:
                break;
        }
    }
}
