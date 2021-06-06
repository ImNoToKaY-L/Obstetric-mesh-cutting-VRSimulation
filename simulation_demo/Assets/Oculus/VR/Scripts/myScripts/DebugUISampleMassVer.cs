using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
// Show off all the Debug UI components.
public class DebugUISampleMassVer : MonoBehaviour
{
    private bool inMenu;
    private Text khValue;
    private Text khLabel;
    private Text knValue;
    private Text knLabel;
    private Text distanceValue;
    private Text distanceLabel;
    private int curFetusPos;
    private bool showHintText;
    private bool transparentMaterial;
    private Material m_origin_fetus;
    public ControllerScript_massVer script;
    public GameObject fetus;
    public GameObject controllerModel;
    public LineRenderer lr;
    public LaserPointer lp;
    public GameObject belly;
    public Canvas hintText;
    public Material ladySkin;
    public Material transparentSkin;
    public bool buttonReceived;


    void Start ()
    {
        // store origin material for ray reminder
        m_origin_fetus = script.m_fetus_area;

        curFetusPos = 0;
        SetFetusPos(curFetusPos);
        DebugUIBuilder.instance.AddLabel("Mass-spring parameters setting");

        var khSlider = DebugUIBuilder.instance.AddSlider("Slider", 0.001f, 0.01f, KhSliderPressed, false);
        var khTextElementsInSlider = khSlider.GetComponentsInChildren<Text>();
        Assert.AreEqual(khTextElementsInSlider.Length, 2, "Slider prefab format requires 2 text components (label + value)");
        khLabel = khTextElementsInSlider[0];
        khLabel.text = "Kh";
        khValue = khTextElementsInSlider[1];
        Assert.IsNotNull(khValue, "No text component on slider prefab");
        khValue.text = khSlider.GetComponentInChildren<Slider>().value.ToString();

        var knSlider = DebugUIBuilder.instance.AddSlider("Slider", 0.001f, 0.01f, KnSliderPressed, false);
        var knTextElementsInSlider = knSlider.GetComponentsInChildren<Text>();
        Assert.AreEqual(knTextElementsInSlider.Length, 2, "Slider prefab format requires 2 text components (label + value)");
        knLabel = knTextElementsInSlider[0];
        knLabel.text = "Kn";
        knValue = knTextElementsInSlider[1];
        Assert.IsNotNull(knValue, "No text component on slider prefab");
        knValue.text = knSlider.GetComponentInChildren<Slider>().value.ToString();

        var distanceSlider = DebugUIBuilder.instance.AddSlider("Slider", 0.01f, 0.03f, DistanceSliderPressed, false);
        var distanceTextElementsInSlider = distanceSlider.GetComponentsInChildren<Text>();
        Assert.AreEqual(distanceTextElementsInSlider.Length, 2, "Slider prefab format requires 2 text components (label + value)");
        distanceLabel = distanceTextElementsInSlider[0];
        distanceLabel.text = "Push distance";
        distanceValue = distanceTextElementsInSlider[1];
        Assert.IsNotNull(distanceValue, "No text component on slider prefab");
        distanceValue.text = distanceSlider.GetComponentInChildren<Slider>().value.ToString();

        DebugUIBuilder.instance.AddDivider();
        DebugUIBuilder.instance.AddLabel("Fetus setting");
        DebugUIBuilder.instance.AddButton("Change fetus position", ChangeFetusPos);

        DebugUIBuilder.instance.AddDivider();
        DebugUIBuilder.instance.AddLabel("Helpers");
        DebugUIBuilder.instance.AddToggle("Transparent", SetTransparent, false);
        DebugUIBuilder.instance.AddToggle("Show hint text", ShowHintText, true);
        DebugUIBuilder.instance.AddToggle("Ray colour reminder", RayColour, true);

        DebugUIBuilder.instance.AddDivider();
        DebugUIBuilder.instance.AddButton("Close this window", CloseMenu);

        //DebugUIBuilder.instance.Show();
        inMenu = false;
        buttonReceived = false;

    }

    void Update()
    {
        if (buttonReceived)
        {
            Debug.Log("Button received, inMenu = " + inMenu);
            if (inMenu) // close the menu
            {
                CloseMenu();
            }
            else // show the menu
            {
                ShowMenu();
            }
            
            buttonReceived = false;
            Debug.Log("After pressed button, inMenu = " + inMenu);
        }
    }

    void CloseMenu()
    {
        controllerModel.GetComponent<MeshRenderer>().enabled = true;
        lr.enabled = true;
        lp.GetComponent<LineRenderer>().enabled = false;
        script.uiStop = false;
        DebugUIBuilder.instance.Hide();
        inMenu = false;
    }

    void ShowMenu()
    {
        controllerModel.GetComponent<MeshRenderer>().enabled = false;
        lr.enabled = false;
        lp.GetComponent<LineRenderer>().enabled = true;
        script.uiStop = true;
        DebugUIBuilder.instance.Show();
        inMenu = true;
    }

    void ChangeFetusPos()
    {
        int randn = curFetusPos;
        while (randn == curFetusPos)
        {
            randn = Random.Range(0, 4);
        }

        SetFetusPos(randn);
        curFetusPos = randn;
        Debug.Log("Change fetus pos to pos " + randn);
    }

    void SetFetusPos(int randn)
    {
        if (randn == 0)
        {
            fetus.transform.position = new Vector3(180f, -59.4f, 66f);
            fetus.transform.rotation = Quaternion.Euler(-90f, 0f, 90f);
            fetus.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        }
        else if (randn == 1)
        {
            fetus.transform.position = new Vector3(-171f, 2f, 66f);
            fetus.transform.rotation = Quaternion.Euler(-262.883f, -270f, 0f);
            fetus.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        }
        else if (randn == 2)
        {
            fetus.transform.position = new Vector3(171f, -4.2f, 37.8f);
            fetus.transform.rotation = Quaternion.Euler(-3.018f, 1.26f, 99.092f);
            fetus.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        }
        else if (randn == 3)
        {
            fetus.transform.position = new Vector3(-174.7f, -0.3f, 83.3f);
            fetus.transform.rotation = Quaternion.Euler(-166.386f, -365.279f, -76.25699f);
            fetus.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        }

    }

    public void KhSliderPressed(float f)
    {
        Debug.Log("kh slider: " + f);
        khValue.text = f.ToString();
        script.kh = f;
    }

    public void KnSliderPressed(float f)
    {
        Debug.Log("kn slider: " + f);
        knValue.text = f.ToString();
        script.kn = f;
    }

    public void DistanceSliderPressed(float f)
    {
        Debug.Log("distance slider: " + f);
        distanceValue.text = f.ToString();
        script.press_distance = f;
    }

    public void SetTransparent(Toggle t)
    {
        if(t.isOn)
        {
            belly.GetComponent<MeshRenderer>().material = transparentSkin;
            Debug.Log("Skin set transparent");
        }
        else
        {
            belly.GetComponent<MeshRenderer>().material = ladySkin;
            Debug.Log("Skin set normal");
        }
    }

    public void ShowHintText(Toggle t)
    {
        if (t.isOn)
        {
            hintText.GetComponent<Canvas>().enabled = true;
            Debug.Log("Hint text shown");
        }
        else
        {
             hintText.GetComponent<Canvas>().enabled = false;
             Debug.Log("Hint text hided");
        }
    }

    public void RayColour(Toggle t)
    {
        if (t.isOn)
        {
            script.m_fetus_area = m_origin_fetus;
        }
        else
        {
            // cancel the reminder
            script.m_fetus_area = script.m_surgeon_area;
        }
    }
}
