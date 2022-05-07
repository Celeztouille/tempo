using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathTagHandler : MonoBehaviour
{
    // Prefab to show
    [SerializeField] private GameObject tagPrefab;

    private Transform avatarTr;     // Transform of the avatar

    // Data is stored in this format x1;y1;z1|x2;y2;z2|x3;y3;z3
    private string data;

    // Bind avatar transform and load tags data
    void Start()
    {
        avatarTr = GameObject.Find("Avatar").transform;
        LoadData();
    }

    // Add a tag on screen and save the list of tags
    public void AddTag()
    {
        Ray ray = new Ray(avatarTr.position, Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Buildings")))
        {
            GameObject tagInstance = Instantiate(tagPrefab, hit.point - 0.05f * Vector3.forward, Quaternion.identity, transform);
            tagInstance.transform.localScale *= 10f;
            SaveData(tagInstance.transform);
        }
        else
        {
            ray = new Ray(avatarTr.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                GameObject tagInstance = Instantiate(tagPrefab, hit.point - 0.05f * Vector3.down, Quaternion.Euler(90f, 0f, 0f), transform);
                tagInstance.transform.localScale = 10f * Vector3.one;
                SaveData(tagInstance.transform);
            }

        }

        

        
    }

    // Update and save tag date when instantiating a new tag
    private void SaveData(Transform newTag)
    {
        if (newTag != null)
        {
            if (data.Length > 1)
            {
                data += "|";
            }

            data += newTag.localPosition.x.ToString() + ";" + newTag.localPosition.y.ToString() + ";" + newTag.localPosition.z.ToString();
            PlayerPrefs.SetString("TagsData", data);
        }
        else
        {
            Debug.LogError("DeathTagHandler.SaveData() : cannot find new tag transform to save");
        }
    }

    // Load data of all saved tag and place them on the level
    private void LoadData()
    {
        data = PlayerPrefs.GetString("TagsData");

        if (data != "")
        {
            string[] splittedData = data.Split('|');

            foreach (string s in splittedData)
            {
                string[] spos = s.Split(';');
                Vector3 pos = new Vector3(float.Parse(spos[0]), float.Parse(spos[1]), float.Parse(spos[2]));

                GameObject tagInstance = Instantiate(tagPrefab, transform, false);
                tagInstance.transform.localPosition = pos;
                tagInstance.transform.localScale = 10f * Vector3.one;
                if (pos.z < 5f)
                {
                    tagInstance.transform.Rotate(90f, 0f, 0f);
                }

            }
        }
    }

    // Erase all tag data !!!
    private void ResetData()
    {
        PlayerPrefs.DeleteAll();
        data = "";
    }


    // DEBUG FUNCTION
    public void InputPressed(InputAction.CallbackContext context)
    {
        if (context.performed)  // Equivalent to Input.GetKeyDown()
        {
            if (context.action.name == "Debug4")
            {
                AddTag();
            }
            if (context.action.name == "Debug5")
            {
                ResetData();
            }
        }
    }
}
