using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Transitioner : MonoBehaviour {

    public bool isStarted {get; set;}
    public GameObject[] particles { get; set; }
    public bool ParticlesCreated {get; set;}

    private SpawnShape[] ShapeSpawners;
    private bool StopRain = false;

    [Header("Audio Source")]
    [SerializeField] private AudioSource A_Source;
    [Header("Timer Event Settings")]
    [SerializeField] private float startSpawningTimer = 67.0f;
    [SerializeField] private float staySpawningTimer = 56.0f;
    [SerializeField] private float stopSpawningTimer = 10.0f;
    [SerializeField] private float particleFreezeTimer = 85.0f;
    [SerializeField] private float particleFlyTimer = 20.0f;
    [Header("UI Start Menu")]
    [SerializeField] private UIController startMenu;

    private ViewShift cameraShifter;

    public UnityEvent[] transitionEvents;

    private MovementTimeControl EffectControl;

    /*
    public float FadeTime; //57
    public float FastStayTime; //10
    public float RainTime; //90
    public float EndPauseTime; //15
    */

    void Start () {
        EffectControl = FindObjectOfType<MovementTimeControl>();
        cameraShifter = GetComponent<ViewShift>();
        StopRain = false;

		isStarted = false;

		StartCoroutine (DelayActivate ());
	}

	void Update()
	{

		if (Input.GetKeyDown (KeyCode.Space) && isStarted == false) {

			StartGame ();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene (0);
		}


	}


    IEnumerator CameraShift()
    {
        int randNum = Random.Range(0, 2);

        if (randNum == 1)
        {
            StartCoroutine(cameraShifter.GoToHeadPosition());

            yield return new WaitForSecondsRealtime(Random.Range(25, 35));
        }
        else
        {
            StartCoroutine(cameraShifter.ReturnToAudience());

            yield return new WaitForSecondsRealtime(Random.Range(35, 45));
        }

        yield return null;

        if(isStarted == true)
        {
            StartCoroutine(CameraShift());
        }

    }

	public void StartGame()
	{
        startMenu.CallAlphaAnimation(false);

		ShapeSpawners = FindObjectsOfType<SpawnShape> ();

		A_Source.Play ();		

		StartCoroutine (RevampedMainTimer ());

        //StartCoroutine(CameraShift());

        isStarted = true;
	}

    public void resetGame()
    {
        startMenu.CallAlphaAnimation(true);

        isStarted = false;

    }

    public void StartSpawning()
	{
		for (int i = 0; i < ShapeSpawners.Length; i++) {
			ShapeSpawners [i].enabled = true;
			ShapeSpawners [i].StartSpawning ();
			ShapeSpawners [i].FadeOut = true;
		}

		//StartCoroutine (DelaySwitch ());
	}

	public void StaySpawning()
	{
		for (int i = 0; i < ShapeSpawners.Length; i++) {
			ShapeSpawners [i].FadeOut = false;
		}

		//StartCoroutine (DelaySwitch ());
	}

	public void StopSpawning()
	{
		for (int i = 0; i < ShapeSpawners.Length; i++) {
			ShapeSpawners [i].stopSpawning = true;
			ShapeSpawners [i].enabled = false;
		}

		//StartCoroutine (DelaySwitch ());
	}


	public void FreezeAll()
	{
		//for (int i = 0; i < particles.Length; i++) {
		//	particles [i].GetComponent<ParticleMovement> ().enabled = false;
		//	particles [i].GetComponent<Rigidbody> ().velocity = Vector3.zero;

		//}

		Time.timeScale = .1f;
	}

	IEnumerator ParticlesFlyAway()
	{
		for (int i = 0; i < particles.Length; i++) {
			particles [i].GetComponent<SphereCollider> ().enabled = false;
			StartCoroutine (particles [i].GetComponent<ParticleMovement> ().FlyAway ());

			yield return new WaitForFixedUpdate ();
		}

		//wait 5 seconds and then restart the scene 
        //SceneManager.LoadScene (0);

        
	}

    /*
	IEnumerator MainTimer()
	{
        StartSpawning();

        yield return new WaitForSecondsRealtime (FadeTime);

		StaySpawning ();

		//spawning fast and staying 
		yield return new WaitForSecondsRealtime (FastStayTime);
		StopSpawning ();
		ActivateMovingParticle ();
        StartCoroutine(DelayDownActivate(false));
        ParticlesCreated = true;
		//rain effect
		yield return new WaitForSecondsRealtime (RainTime);

        FreezeAll();

		//end pause, divided because time scale is changed
		yield return new WaitForSecondsRealtime (EndPauseTime);

        StopRain = true;
		Time.timeScale = 1;
		StartCoroutine(ParticlesFlyAway ());
		//particles fly up
	}
    */

    IEnumerator RevampedMainTimer() {
        yield return new WaitForSecondsRealtime(startSpawningTimer);

        Debug.Log("StartSpawningCalled");
        StartSpawning();
        transitionEvents[0].Invoke();

        //8.0f
        yield return new WaitForSecondsRealtime(staySpawningTimer);

        Debug.Log("StaySpawningCalled");
        StaySpawning();
        transitionEvents[1].Invoke();

        //58.0f
        yield return new WaitForSecondsRealtime(stopSpawningTimer);


        Debug.Log("StopSpawningCalled");
        StopSpawning();
        ActivateMovingParticle();
        StartCoroutine(DelayDownActivate(false));
        transitionEvents[2].Invoke();

        EffectControl.ToggleTimeStop(true);
        //100.0f
        yield return new WaitForSecondsRealtime(particleFreezeTimer);

        Debug.Log("FreezeCalled");
        //FreezeAll();
        EffectControl.ToggleTimeStop(false);
        StartCoroutine(ParticlesMove(true));

        transitionEvents[3].Invoke();

        //10.0f
        yield return new WaitForSecondsRealtime(particleFlyTimer);

        Debug.Log("FlyAwayCalled");
        StopRain = true;
        
        //Time.timeScale = 1;
        transitionEvents[4].Invoke();

        //StartCoroutine(ParticlesMove(true));
        StartCoroutine(ParticlesFlyAway());

        yield return new WaitForSecondsRealtime(5f);

        //bring the menu back and reset state
        resetGame();



        //To do: Figure time to fade out
        //12??

    }
    

    IEnumerator DelayActivate()
	{

		yield return new WaitForSeconds (1);

		ShapeSpawners = FindObjectsOfType<SpawnShape> ();

	}

	public void ActivateMovingParticle()
	{
		particles = GameObject.FindGameObjectsWithTag ("projectile");

		for (int i = 0; i < particles.Length; i++) {
			particles [i].GetComponent<ParticleMovement> ().enabled = true;
			//particles [i].GetComponent<TrailRenderer> ().enabled = true;

		}
			
		//StartCoroutine (DelayDownActivate (false));
	}

	IEnumerator DelayDownActivate(bool isUp)
	{
		yield return new WaitForSeconds (20);

		StartCoroutine (ParticlesMove (isUp));
	}

	IEnumerator ParticlesMove(bool isUp)
	{
		for (int i = 0; i < particles.Length; i++) {
			particles [i].GetComponent<ParticleMovement> ().isMoveUp = isUp;

			if (isUp == true) {
				StartCoroutine (particles [i].GetComponent<ParticleMovement> ().MoveUp ());
			} else {
				StartCoroutine (particles [i].GetComponent<ParticleMovement> ().MoveDown ());
			}

			if (StopRain == true) {
				break;
			}

			yield return new WaitForSeconds (.01f);
		}

		isUp = !isUp;

		//if (StopRain == false) {
			//StartCoroutine (DelayDownActivate (isUp));
		//}
	}
}