using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LighSaber : MonoBehaviour
{
    // max angle for the saber
    [SerializeField] private float maxAngle;
    [SerializeField] private float minAngle;
    // ray length for the saber
    [SerializeField] private float rayLength;
    [SerializeField] private TMP_Text sweepAngleTmp;
    [SerializeField] private TMP_Text scoreTmp;
    [SerializeField] private TMP_Text onScreenTmp;
    [SerializeField] private ParticleSystem spark;
    // material of the light saber tube
    private Material lightMaterial ;
    //total score obtained from game
    private int score;
    // current score obtained from each try
    private int cScore;
    //range of the angles
    private float range;
    // flag for the cylinder click
    private bool isClicked;
    // flag for the mouse is down or not. only used for the particle system
    private bool isMouseDown;
    private ParticleSystem.MainModule particleMain ;
    // start angle of the light saber for checking when the sweep is completed
    private float startAngle;
   

    // Start is called before the first frame update
    void Awake()
    {
        GameManger.startGame += restart;
        range = maxAngle - minAngle;
        isClicked = false;
        lightMaterial = transform.Find("lightTube").GetComponent<Renderer>().material;
        particleMain = spark.main;
    }

    private void restart()
    {
        onScreenTmp.text = "Move light saber from left end to right end";
        scoreTmp.text ="Score :"+ "0";
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManger.isgameRunning)
        {
            if(Input.GetMouseButtonDown(0))
            {
                lightMaterial.color = Color.yellow;
                isMouseDown = true;
            }
            float angle = (((Input.mousePosition.x/Screen.width))*range/1)+minAngle;
            angle = Mathf.Clamp(angle,minAngle,maxAngle);
            
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x,angle,transform.eulerAngles.z);
            RaycastHit hit;
            if(Physics.Raycast(transform.position,transform.up,out hit,rayLength))
            {
                float width = hit.transform.localScale.x;
                float length = hit.transform.localScale.y;
                float hitDistance = Vector3.Distance(hit.point,hit.transform.position-new Vector3(0,0,length));
                float ratio = (width*0.5f)/3f;
                Vector3 direction = transform.position-new Vector3(0,0,length) -hit.point ;
                float sign = Mathf.Sign(direction.x);
                spark.transform.position = hit.point;
                if(isMouseDown)
                {
                    if(!spark.isPlaying)
                    {
                        spark.Play();
                    }
                }
                
                if(hitDistance>=ratio*2)
                {
                    if(Input.GetMouseButtonDown(0)&& sign>0)
                    {
                        isClicked = true;
                        startAngle = angle;
                    }
                    cScore = 100;
                    
                    particleMain.startColor = Color.red;
                }
                else if( hitDistance<ratio*2 && hitDistance >= ratio)
                {
                    cScore = 50;
                    particleMain.startColor = Color.yellow;
                }
                else if(hitDistance<ratio)
                {
                    cScore = 25;
                    particleMain.startColor = Color.gray;
                }
                else
                {
                    print("Out" +"Hit distance :"+ hitDistance + "ratio " + ratio);
                }
                
                if(Input.GetMouseButtonUp(0))
                {
                    if(isClicked && sign<0)
                    {
                        score += cScore;
                        scoreTmp.text ="Score :"+ score.ToString();
                        onScreenTmp.text = "You Scored "+cScore.ToString();
                        sweepAngleTmp.text = "Sweep Angle : "+Mathf.Abs(Mathf.Round(startAngle-angle)).ToString();
                    }
                    else
                    {
                        onScreenTmp.text = "Try Again ";
                    }
                }
            }
            else
            {
                spark.Stop();
            }
            if(Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
                isClicked = false;
                lightMaterial.color = Color.gray;
                spark.Stop();
            }
            

            
        }
        

        
        // Debug.DrawRay(transform.position,transform.transform.up,Color.black,1);
    }
    void OnDestroy()
    {
        GameManger.startGame -= restart;
    }
    

    
}
