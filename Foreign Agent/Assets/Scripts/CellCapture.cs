using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CellCapture : MonoBehaviour
{
    public GameObject player;
    [HideInInspector]
    public bool startCap = false;
    public float capTime = 5f;
	public Slider slider;
    [HideInInspector]
    public float currTime;
    [HideInInspector]
    public bool capped = false;
    public GameObject disruptionAnim;
    private GameObject captureAnim;
    private Material mat;
    private AudioSource capturingSound;
    private AudioSource cellPop;
    private GameObject parentCell;
    private GameObject particleSystem;
    private Color origColour;
    private Color emisColour;
    private Renderer cellRenderer;
    private float colourTimer = 0f;
   
    void Start()
    {
        colourTimer = 0f;
        parentCell = transform.parent.gameObject;
        cellRenderer = parentCell.GetComponent<Renderer>();
        origColour = cellRenderer.material.color;
        emisColour = cellRenderer.material.GetColor("_EmissionColor");

        particleSystem = parentCell.transform.Find("ReplicationEffect").gameObject;     
        mat = parentCell.transform.GetComponent<Renderer>().material;
        capturingSound = transform.Find("capturingSound").gameObject.GetComponent<AudioSource>();
        cellPop = transform.Find("cellPop").gameObject.GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (!capped && other.gameObject == player)
        {
            captureAnim = Instantiate(disruptionAnim, gameObject.GetComponent<Collider>().bounds.center, Quaternion.identity);
            startCap = true;
            capturingSound.Play();
            currTime = capTime;
			Debug.Log("starting cap");
        }
        else if (startCap && other.gameObject.CompareTag("Tcell"))
        {
            Debug.Log("death");
            if (capturingSound.isPlaying)
                capturingSound.Stop();
            startCap = false;
            GameController.Instance.death = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (startCap && !capped && !GameController.Instance.death)
        {
            colourTimer += Time.deltaTime / capTime;
            currTime -= Time.deltaTime;
			float complete = (capTime - currTime) / capTime;
			slider.value = complete;

            cellRenderer.material.color = Color.Lerp(origColour, Color.black, colourTimer);
            cellRenderer.material.SetColor("_EmissionColor", Color.Lerp(emisColour, Color.red, colourTimer));

            if (currTime <= 0)
            {
                Debug.Log("Capped");
                if (capturingSound.isPlaying)
                    capturingSound.Stop();
                colourTimer = 0f;
                cellPop.Play();
                companionSpawn.Instance.numCompanions += 1;
                GameController.Instance.numCaptures += 1; //temporary
                capped = true;
                startCap = false;
                ParticleSystem parts = particleSystem.GetComponent<ParticleSystem>();
                parts.Play();
                StartCoroutine(DissolveOverTime(2.0f));
                float totalDuration = parts.duration + parts.startLifetime;
                StartCoroutine(DestroyReplicationEffect(totalDuration));
                Destroy(captureAnim);
                
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == player)
        {
          

            Destroy(captureAnim);
            if(capturingSound.isPlaying)
                capturingSound.Stop();
            if (currTime > 0 && !capped)
            {
                slider.value = 0;
                startCap = false;
                StartCoroutine(ColourReset(1f));
            }
            colourTimer = 0f;
            currTime = capTime;
            startCap = false;

        }
    }
    IEnumerator DissolveOverTime(float duration)
    {
        slider.gameObject.SetActive(false);
        for (float t = 0; t <= duration - 0.2f; t += Time.deltaTime)
        {

            mat.SetFloat("_Cutoff", t / duration);

            yield return null;
        }
       
        mat.SetInt("_Cutoff", 1);
    }
    IEnumerator DestroyReplicationEffect(float duration)
    {
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            yield return null;
        }
        Destroy(particleSystem);
        Destroy(parentCell);
    }
    IEnumerator ColourReset(float duration)
    {
       
        Color currColour = cellRenderer.material.color;
        Color currEmisColour = cellRenderer.material.GetColor("_EmissionColor");

        float t = 0;
        while (t < duration && !startCap)
        {
            cellRenderer.material.color = Color.Lerp(currColour, origColour, t / duration);
            cellRenderer.material.SetColor("_EmissionColor", Color.Lerp(currEmisColour, emisColour, t / duration));
            t += Time.deltaTime;
            yield return null;
        }
        
    }
}
