using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public int baseHearts = 5;
    private float heartValue;
    private List<float> heartStates = new List<float>();

    [SerializeField] public Sprite heartFullSprite;
    [SerializeField] public Sprite heartHalfFullSprite;
    
    [SerializeField] public Sprite heartFullToHalfFullSprite;
    [SerializeField] public Sprite heartFullToEmptySprite;
    [SerializeField] public Sprite heartHalfFullToEmptySprite;

    [SerializeField] public Sprite heartHalfFullToEmptyContinuousSprite;
    [SerializeField] public Sprite empty;
    [SerializeField] public int spriteSeparation = 113;

    [SerializeField] public float hitStopDuration;

    private List<GameObject> healthBalls = new List<GameObject>();
    // Start is called before the first frame update

    bool hitStopWaiting;
    private IEnumerator depleteCoroutine;
    void Start()
    {
        heartValue = (float) baseHearts;
        
        healthBalls.Add(gameObject);
        heartStates.Add(1.0f);

        RectTransform heartRectTransform;
        for (int i=1; i<baseHearts; i++) {
            GameObject heart = new GameObject();
            healthBalls.Add(heart);
            heartStates.Add(1.0f);

            healthBalls[i].transform.SetParent(healthBalls[i-1].transform);
            healthBalls[i].AddComponent(typeof(RectTransform));
            healthBalls[i].AddComponent(typeof(Image));

            heartRectTransform = healthBalls[i].GetComponent<RectTransform>();
            heartRectTransform.transform.localPosition = new Vector3(spriteSeparation, 0, 0);
            heartRectTransform.localScale = new Vector3(1,1,1);
            healthBalls[i].GetComponent<Image>().sprite = heartFullSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void depleteHealth(float damage) {
        // Debug.Log(damage);
        float healthToDeplete = damage;
        heartValue = Mathf.Max(0, heartValue-healthToDeplete);
        for (int i=baseHearts-1; i>-1; i--) {
            
            if (healthToDeplete <= 0.0f) {
                return;
            }

            if (heartStates[i] == 0.0f) {
                continue;
            }
            
            bool halfDelete = healthToDeplete == 0.5f ? true : false;
            
            healthToDeplete -= heartStates[i];
            
                Image image = healthBalls[i].GetComponent<Image>();

            if (halfDelete) {
                if (heartStates[i] == 1) {
                    image.sprite = heartFullToHalfFullSprite;
                    hitStop(hitStopDuration);
                    depleteCoroutine = waitForDepleteEffect(image, heartHalfFullSprite);
                    StartCoroutine(depleteCoroutine);
                    heartStates[i] = 0.5f;
                } else {
                    image.sprite = heartHalfFullToEmptySprite;
                    hitStop(hitStopDuration);
                    depleteCoroutine = waitForDepleteEffect(image,empty);
                    StartCoroutine(depleteCoroutine);
                    heartStates[i] = 0.0f;
                }
            }
            else {
                if (heartStates[i] == 1) {
                    healthBalls[i].GetComponent<Image>().sprite = heartFullToEmptySprite;
                    hitStop(hitStopDuration);
                    depleteCoroutine = waitForDepleteEffect(image,empty);
                    StartCoroutine(depleteCoroutine);
                    heartStates[i] = 0.0f;
                } else if (heartStates[i] == 0.5) {
                    healthBalls[i].GetComponent<Image>().sprite = heartHalfFullToEmptySprite;
                    hitStop(hitStopDuration);
                    depleteCoroutine = waitForDepleteEffect(image,empty);
                    StartCoroutine(depleteCoroutine);
                    heartStates[i] = 0.0f;
                }
            }
        }
        
    }

    public void resetHealth() {
        for (int i=0; i<baseHearts; i++) {
            healthBalls[i].GetComponent<Image>().sprite = heartFullSprite;
        }
    }

    public void hitStop(float duration) {
        if (hitStopWaiting) {
            return;
        }
        
        Time.timeScale = 0.0f;
        StartCoroutine(wait(duration));
    }

    IEnumerator wait(float duration) {
        hitStopWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        hitStopWaiting = false;
    }

    IEnumerator waitForDepleteEffect(Image image, Sprite endSprite) {
        while (Time.timeScale != 1.0f)
            yield return null;
        // image.sprite = depleteSprite;
        image.sprite = endSprite;
    }
}
