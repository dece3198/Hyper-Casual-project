using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isFizzCupMachine = false;
    private int level = 1;
    public int Level
    {
        get { return level; }
        set 
        { 
            level = value;
            switch (level)
            { 
                case 3: LevelUp(upObj[0], virtualCamera.gameObject, 3); break;
                case 5: LevelUp(upObj[1], virtualCamera.gameObject, 3); break;
                case 10: LevelUp(upObj[2], virtualCamera.gameObject, 3); break;
            }
        }
    }
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider slider;
    [SerializeField] private float experience;
    [SerializeField] private GameObject[] upObj;
    [SerializeField] private Transform[] camPos;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private int posCount = 0;

    public float Experience
    {
        get { return experience; }
        set 
        { 
            experience = value; 
            if(experience >= maxExperience)
            {
                float remain = experience -= maxExperience;
                Level++;
                experience = remain;
                maxExperience += 10 * Level;
            }
        }
    }
    private float maxExperience;

    private void Awake()
    {
        instance = this;
        maxExperience = 100;
    }

    private void Update()
    {
        levelText.text = level.ToString();
        slider.value = Experience / maxExperience;

        if(Input.GetKeyDown(KeyCode.K))
        {
            Experience += 20;
        }
    }

    private void LevelUp(GameObject objA,GameObject objB, float count)
    {
        objA.SetActive(true);
        objB.transform.position = camPos[posCount].position;
        objB.SetActive(true);
        StartCoroutine(ObjCo(objB, count));
        posCount++;
    }

    private IEnumerator ObjCo(GameObject gameObject, float count)
    {
        yield return new WaitForSeconds(count);
        gameObject.SetActive(false);
    }
}
