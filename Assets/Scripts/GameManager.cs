using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tenpin;
    [SerializeField]
    private GameObject[] balls;
    [SerializeField]
    Text lts, TxScore;

    private Vector3[] ballPositions;
    Vector3 targetScale;

    private string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public char[] random3 = new char[3];
    public int throwIndex = 0;
    public bool touchable = true;
    public int score = 0;
    public AudioSource audio,audioArka;
    public AudioClip dogru, yanlis, arka;

    void Start()
    {
        audioArka = transform.GetChild(0).GetComponent<AudioSource>();
        audio = GetComponent<AudioSource>();
        lts = GameObject.Find("LtsBoard").GetComponent<Text>();
        TxScore = GameObject.Find("Score").GetComponent<Text>();
        targetScale = balls[0].transform.localScale;
        ballPositions = new Vector3[]{new Vector3(-4,-8.5f,-1), new Vector3(0,-8.5f,-1), new Vector3(4,-8.5f,-1)};

        foreach(GameObject pin in tenpin){
            pin.SetActive(false);
        }

        

        audioArka.clip = arka;
        audioArka.Play();

        StartCoroutine("newTurn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void makeRandomLetters(){

        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                if(random3[i] == random3[j])
                    random3[i] = letters[Random.Range(0,25)];
            }
            
            balls[i].GetComponent<ThrowBall>().letter[0] = random3[i];
            balls[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = random3[i].ToString() ;
        }
        lts.text = random3[0] + " " + random3[1] + " " + random3[2];

    }

    public void setUp(){

        TxScore.text = score.ToString();
        if(throwIndex > 2)
            {
                StartCoroutine("newTurn");
            }else{
                nextThrow();
            }

    }

    public void nextThrow(){

        StartCoroutine("spawnTenpin");
        
    }

    void reshuffle(GameObject[] ballsArray)
    {
      
        for (int i = 0; i < ballsArray.Length; i++ )
        {
            GameObject tmp = ballsArray[i];
            int r = Random.Range(i, ballsArray.Length);
            ballsArray[i] = ballsArray[r];
            ballsArray[r] = tmp;
        }
    }

    IEnumerator spawnTenpin(){

        
        float waitTime = 0.1f;

        foreach(GameObject pin in tenpin){
            pin.SetActive(false);
        }

        foreach(GameObject pin in tenpin){

                Vector3 targetScale = pin.transform.localScale;
                pin.transform.localScale = Vector3.zero;
                pin.SetActive(true);

                
                float elapsedTime = 0;

                while (elapsedTime < waitTime)
                {
                    pin.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, (elapsedTime / waitTime));
                    elapsedTime += Time.deltaTime;
                
                yield return null;
                }
                pin.transform.localScale = targetScale;
                yield return null;
        }

        yield return null;
        touchable = true;

        if(throwIndex == 0)
        lts.text = "";

        StartCoroutine("spawnBalls");
        
    }

    IEnumerator spawnBalls(){

        float waitTime = 0.2f;
      
        
        for(int i=0; i<balls.Length; i++){
            balls[i].transform.localScale = Vector3.zero;
            balls[i].transform.position = ballPositions[i];
        }


            float elapsedTime = 0;

            while (elapsedTime < waitTime)
            {
                foreach(GameObject ball in balls){
                    ball.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, (elapsedTime / waitTime));
                    yield return null;
                }

                elapsedTime += Time.deltaTime;
            
            yield return null;
            }

            foreach(GameObject ball in balls)
                ball.transform.localScale = targetScale;
           
           

    }

IEnumerator newTurn(){

    foreach(GameObject pin in tenpin){
            pin.SetActive(false);
        }

    yield return new WaitForSeconds(1);

    for(int i=0; i<3; i++){
            balls[i].SetActive(true);
            balls[i].transform.GetChild(0).gameObject.SetActive(true);
    }
        
        throwIndex = 0;
        makeRandomLetters();
        reshuffle(balls);
        StartCoroutine("spawnTenpin");


}
}
