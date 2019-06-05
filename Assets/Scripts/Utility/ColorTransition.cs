using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorTransition : MonoBehaviour {

    public Material material; 

    public ColorOptions[] colorchoice;

    public int CurrentColor;

    public float transitionTime;

    private int lastColor;


    [Serializable]
    public class ColorOptions
    {
        [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
        public Color[] colors;

        public string[] VariableNames;
    }

    // Use this for initialization
    void Start () {
		
	}

    public void changeColor(int index)
    {
        lastColor = CurrentColor;
        CurrentColor = index;

        if(CurrentColor > colorchoice.Length)
        {
            CurrentColor = 0;
        }

        StartCoroutine(SwitchColor(CurrentColor));

    }


    public IEnumerator SwitchColor(int index)
    {

        float t = 0;
      

        while(t<transitionTime)
        {
            // t += Time.fixedUnscaledDeltaTime;
            t += Time.deltaTime;
            for (int i = 0; i < colorchoice[index].colors.Length; i++)
            {
                material.SetColor(colorchoice[CurrentColor].VariableNames[i], Color.Lerp(colorchoice[lastColor].colors[i] , colorchoice[CurrentColor].colors[i], t / transitionTime));

                yield return null;
            }

        }



    }

    private void OnValidate()
    {

        for (int i = 0; i < colorchoice[CurrentColor].colors.Length; i++)
        {
            material.SetColor(colorchoice[CurrentColor].VariableNames[i], colorchoice[CurrentColor].colors[i]);

        }

    }


    // Update is called once per frame
    void Update () {
		
	}
}
