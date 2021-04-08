using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowBall : MonoBehaviour
{
    GameManager gm;
    public char[] letter = new char[1];
    private GameObject[] balls;
    public GameObject confetti1, confetti2;
    ParticleSystem system1, system2;
    Vector3 currentScale;
    Text lts;
    

    void Start()
    {
        system1 = confetti1.GetComponent<ParticleSystem>();
        system2 = confetti2.GetComponent<ParticleSystem>();
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
        balls = GameObject.FindGameObjectsWithTag("ball");
        currentScale = transform.localScale;
        lts = GameObject.Find("LtsBoard").GetComponent<Text>();
    }

    void Update()
    {
        

    }

    void OnMouseDown(){

        if(gm.touchable){

            transform.GetChild(0).gameObject.SetActive(false);
            gm.touchable = false;

            StartCoroutine("clearOthers");
            
            if(isTrueChar())
                StartCoroutine("moveStraight");
            else
                StartCoroutine("moveSide");
            
        }
    }

    void OnTouchDown(){

        

        if(isTrueChar() && gm.touchable){
            gm.touchable = false;
            StartCoroutine("moveStraight");
        }
    }

    public bool isTrueChar(){

        if(letter[0]==gm.random3[gm.throwIndex])
            return true;
        else
            return false;

    }

    IEnumerator moveStraight(){

        

        gm.audio.clip = gm.dogru;
        gm.audio.Play();

        Vector3 target = new Vector3(0, 3.5f, -10);
        Vector3 current = transform.position;
        Vector3 targetScale = new Vector3(0.55f, 0.6f, 0.55f);
        Vector3 currentScale = transform.localScale;

        float waitTime = 1.5f;
        float elapsedTime = 0;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(current, target, (elapsedTime / waitTime));
            transform.localScale = Vector3.Lerp(currentScale, targetScale, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
        
            yield return null;
        }  
        
        transform.localScale = targetScale;
        transform.position = target;
        
        system1.Play();
        system2.Play();

        lts.text += letter[0];
        gm.score += 10;
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
        gm.throwIndex++;
        gm.setUp();

        yield return null;
        

    }


     IEnumerator moveSide(){

        gm.audio.clip = gm.yanlis;
        gm.audio.Play();

        Vector3 target = new Vector3(-4.4f, 2.2f, -10);
        Vector3 current = transform.position;
        Vector3 targetScale = new Vector3(0.55f, 0.6f, 0.55f);
        Vector3 currentScale = transform.localScale;

        float waitTime = 1f;
        float elapsedTime = 0;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(current, target, (elapsedTime / waitTime));
            transform.localScale = Vector3.Lerp(currentScale, targetScale, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
        
            yield return null;
        }  
        
        transform.localScale = targetScale;
        transform.position = target;

        

        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
        gm.throwIndex = 3;

        gm.setUp();
        yield return null;
        
    }

    IEnumerator clearOthers() {

        float waitTime = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < waitTime)
        {
                foreach(GameObject ball in balls){
                    
                    if(!GameObject.ReferenceEquals(ball, gameObject))
                        ball.transform.localScale = Vector3.Lerp(currentScale, Vector3.zero, (elapsedTime / waitTime));
            
                }

            elapsedTime += Time.deltaTime;
            yield return null;
        }  

        yield return null;
    }

}
