using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EHeardSoundCategory
{
    EFootstep,
    EJump
}

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public playerController playerScript;

    [Header("----- UI -----")]
    public GameObject pauseMenu;
    public GameObject playerDamageScreen;
    public GameObject playerDeadMenu;
    public GameObject winMenu;
    public TextMeshProUGUI enemiesLeft;

    public bool isPaused;
    public int enemiesToKill;


    // Vision
    public List<DetectableTarget> allTargets { get; private set; } = new List<DetectableTarget>();
    // Hearing
    public List<HearingSensor> allSensors { get; private set; } = new List<HearingSensor>();

    // Start is called before the first frame update
    void Awake()
    { 
        if (instance != null)
        {
            Debug.LogError("Multiple DetectableTargetManager found: " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);

            if (isPaused)
                Pause();
            else
                unPause();
        }
    }
    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void unPause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public IEnumerator playerDamageFlash()
    {
        playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageScreen.SetActive(false);
    }
    public void youWin()
    {
        winMenu.SetActive(true);
        Pause();
    }
    public void updateEnemyNumbers()
    {
        enemiesToKill--;
        updateUI();

        if (enemiesToKill <= 0)
            youWin();
    }
    public void updateUI()
    {
        enemiesLeft.text = enemiesToKill.ToString("F0");
    }


    // Vision Registers
    public void VisionRegister(DetectableTarget target)
    {
        allTargets.Add(target);
    }

    public void VisionDeregister(DetectableTarget target)
    {
        allTargets.Remove(target);
    }

    // Hearing Registers
    public void HearingRegister(HearingSensor sensor)
    {
        allSensors.Add(sensor);
    }

    public void HearingDeregister(HearingSensor sensor)
    {
        allSensors.Remove(sensor);
    }

    public void OnSoundEmitted(GameObject source, Vector3 location, EHeardSoundCategory category, float intensity)
    {
        // Notify all of the sensores
        foreach (var sensor in allSensors)
        {
            sensor.OnHeardSound(source, location, category, intensity);
        }
    }
}
