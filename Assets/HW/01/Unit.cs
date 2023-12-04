using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    int health = 50;
    bool isHeallingCoroutineStart = false;
    bool stopCorotunes = false;
    private float time = 3;

    private float timeLeft = 0f;

    void Start()
    {
        ReaceiveHealling();
    }

    public void ReaceiveHealling()
    {
        if (stopCorotunes)
        {
            Debug.Log("stopCorotunes - "+ stopCorotunes);
            StopAllCoroutines();
            stopCorotunes = false;
            isHeallingCoroutineStart = false;
            return;
        }
        if (isHeallingCoroutineStart == false)
        {
            isHeallingCoroutineStart = true;
            timeLeft = time;
            StartCoroutine(HeallingCoroutine());
            StartCoroutine(StartTimer());
        }
    }

    IEnumerator HeallingCoroutine()
    {
        while (isHeallingCoroutineStart)
        {
            if (timeLeft <= 0)
            {
                stopCorotunes = true;
                ReaceiveHealling();
                yield break;
            }
            if (health < 100)
            {
                health += 5;
                Debug.Log("health + 5  - " + health);
            }
            else
            {
                health = 100;
                Debug.Log("health > 100  - " + health);
                stopCorotunes = true;
                ReaceiveHealling();
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator StartTimer()
    {
        while (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
            //Debug.Log("timeLeft - " + timeLeft);
            yield return null;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            Debug.Log("Input.GetKeyDown");
            if(timeLeft<=0) health = Random.Range(20,100);
            ReaceiveHealling();
        }
    }
}
