using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
//using UnityEditor;
using Simple;
public class cmdLst : MonoBehaviour
{
    private AddAtoms other;
    public string[] tags = new string[8] { "ribbons", "bonds", "alpha", "beta", "hetatmbs", "hetatms", "water", "ion" };
    public string[] reps = new string[2] { "stick", "CPK" };
    //This contains a list of the audio files attached to corresponding points of interest (POI's)
    public List<AudioSource> audioList = new List<AudioSource>();
    //This carries all of the references to the POI's
    public List<GameObject> poiList = new List<GameObject>();
    //heres the full list of gameobjects (this could be written better but for now.....)
    public List<GameObject> atomsList = new List<GameObject>();
    //List of the Colliders 
    public List<SphereCollider> colList = new List<SphereCollider>();
    public int numAtoms = 0;
    // This is used to highlight certain objects we want to take a look at
    public Color HIGHLIGHT;
    void Start()
    {
        //SimpleConsole.AddCommand("cngrep", "Changes the representation of a structure", cngrep);
        SimpleConsole.AddCommand("readScript", "reads script for educator", read);
        SimpleConsole.AddCommand("hide", "hides a certain index of atoms and also will hide structure", hide);
        SimpleConsole.AddCommand("poi", "adds point of interest", poi);
        SimpleConsole.AddCommand("attchsnd", "adds sound to object", attachsnd);
        SimpleConsole.AddCommand("highlight", "highlights an object", highlight);
        foreach (string s in tags)
        {
            numAtoms += GameObject.FindGameObjectsWithTag(s).Length;
            atomsList.AddRange(GameObject.FindGameObjectsWithTag(s));
        }
        atomsList.OrderBy(x => int.Parse(x.name));
        PlayerPrefs.SetString("stuoredu", "True");

    }
    string highlight(SimpleContainer param)
    {
        if (param.getAtIndex(0).sData == "index")
        {
            if (!param.getAtIndex(1).isInt())
            {
                SimpleConsole.print("ERROR: PLEASE CHECK YOUR PARAMATERS FOR THIS FUNCTION (int to int)");
            }
            else if (param.getAtIndex(1).iData > numAtoms | param.getAtIndex(1).iData < 0)
            {
                SimpleConsole.print("ERROR: MAKE SURE YOUR INTEGERS ARE WITHIN RANGE ");
            }
            else if (param.getAtIndex(2).sData == "to")
            {

                int a = param.getAtIndex(1).iData;
                int c = param.getAtIndex(3).iData;
                //Time to mark these guys with an object first
                //Then what I need to do is turn off or make less visable the other atoms
                // SimpleConsole.print(atomsList.Count.ToString());
                for (int i = a; i <= c; i++)
                {
                    atomsList[i].GetComponentInChildren<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");

                    // SimpleConsole.print("End");

                }
            }
            else
            {
                GameObject go = GameObject.Find(param.getAtIndex(1).iData.ToString());
                go.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                Debug.Log("Success");
            }
        }


        return "OK";
    }
    /*string saveScene()
    {
        var newScene = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.DefaultGameObjects);
        UnityEditor.EditorApplication.SaveScene("Assets/RegionScene/" + "stuff" + ".unity"); //newScene


        return "SCENE SAVED";

    }*/
    string read(SimpleContainer param)
    {
        if (!param.getAtIndex(0).isString())
        {

            SimpleConsole.print("ERROR: PLEASE CHECK YOU PARAMATER (read 'filename for script'");
        }
        else
        {
            try
            {
                //THIS MUST BE CHANGED BUT FOR NOW JUST THE SKELETON
                //Load file 
                StreamReader sr = new StreamReader("Assets/Resources/" + param.getAtIndex(0).sData);
                while (!sr.EndOfStream)
                {
                    //Parse file
                    // Read the stream to a string, and write the string to the console.
                    string line = sr.ReadLine();
                    if (line.ToCharArray()[0] != '#' & line.ToCharArray()[0] != '-')
                    {
                        //Run command list
     
                        SimpleConsole.eval(line);
                        
                        

                    }
                    /*else if(line.ToCharArray()[0] == '-')
                    {
                        if (line == "-stutoedu")
                        {
                            PlayerPrefs.SetString("stutoedu", "True");
                        }
                        else if (line == "-edutostu")
                        {
                            PlayerPrefs.SetString("stutoedu", "False");
                        }

                    }*/
                }
                /*if(PlayerPrefs.GetString("stutoedu") == "False")
                {
                    SphereCollider[] sp = GameObject.FindObjectsOfType<SphereCollider>();
                    foreach(SphereCollider s in sp)
                    {
                        s.enabled = false;
                    }
                }
                else
                {
                    SphereCollider[] sp = GameObject.FindObjectsOfType<SphereCollider>();
                    foreach (SphereCollider s in sp)
                    {
                        s.enabled = true;
                    }
                }*/


            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }
        return "OK";
    }
    string attachsnd(SimpleContainer param)
    {
        /*if (!param.getAtIndex(0).isString() | !param.getAtIndex(1).isInt() && !param.getAtIndex(1).isFloat())
        {
            SimpleConsole.print("What the flying fuck are you doing?");
        }
        //Heres if we would like to attach the sound to a specific atom
        else*/
        if (param.getAtIndex(1).sData == "index")
        {
            GameObject go = GameObject.Find(param.getAtIndex(2).iData.ToString());
            if (!poiList.Contains(go) | poiList.Count() == 0)
            {
                SimpleConsole.print("ERROR CHECK YOUR POI'S WITH 'poi -list' command");
            }
            else
            {
                GameObject go1 = GameObject.Find(param.getAtIndex(2).iData.ToString());
                if (go1.GetComponent<AudioSource>() == null)
                {
                    go1.AddComponent<AudioSource>();
                }
                else
                {
                    SimpleConsole.print("ERROR: THIS POI ALREADY HAS AUDIO ATTACHED TO IT. PLEASE CHECK YOUR SCRIPT");

                }
                GameObject.Find(param.getAtIndex(2).iData.ToString()).GetComponent<AudioSource>().clip =
                Resources.Load(param.getAtIndex(0).sData) as AudioClip;
            }
        }
        //Heres if we would like to attach the sound to a specific vector transform
        else if (param.getAtIndex(1).sData == "vec")
        {
            GameObject go1 = GameObject.Find(param.getAtIndex(2).iData.ToString());
            if (go1.GetComponent<AudioSource>() == null)
            {
                go1.AddComponent<AudioSource>();
            }
            else
            {
                SimpleConsole.print("ERROR: THIS POI ALREADY HAS AUDIO ATTACHED TO IT. PLEASE CHECK YOUR SCRIPT");

            }
            Vector3 vec = new Vector3(param.getAtIndex(2).fData, param.getAtIndex(3).fData, param.getAtIndex(4).fData);
            var listItem = poiList.FirstOrDefault(x => x.transform.position == vec);
            if (listItem != null)
            {
                AudioSource ao = go1.GetComponent<AudioSource>();
                ao.clip = Resources.Load(param.getAtIndex(0).sData) as AudioClip;
                audioList.Add(ao);
            }
            else
            {
                SimpleConsole.print("ERROR: THAT POI DOESNT EXIST IN THE DATABASE ADD IT WITH 'poi vec Vector3 Vector3 Vector3'");
            }

        }
        return "OK";
    }
    string poi(SimpleContainer param)
    {
        /* For vectors what we'll will do is rather simple: calculate nearest atom to vector point, store said atom in variable, then calculate the difference.*/ 

        if (param.getAtIndex(0).sData == "index")
        {
            if (!param.getAtIndex(1).isInt())
            {
                SimpleConsole.print("ERROR: PLEASE CHECK YOUR PARAMATERS FOR THIS FUNCTION (int to int)");
            }
            else if (param.getAtIndex(1).iData > numAtoms | param.getAtIndex(1).iData < 0)
            {
                SimpleConsole.print("ERROR: MAKE SURE YOUR INTEGERS ARE WITHIN RANGE ");
            }
            else
            {
                other = GameObject.Find("UserObject").GetComponent<AddAtoms>();
                //Attach gameobject for camera to move around
                //Specifically, we will attach a collider to the atom and once the player is within that range then the sound will play
                GameObject go = GameObject.Find(param.getAtIndex(1).iData.ToString());
                if (!poiList.Contains(go))
                {
                    if (go.GetComponent<attchas>() == null)
                    {

                        go.AddComponent<attchas>();

                    }
                    poiList.Add(go);
                    //SimpleConsole.print("ADDED POI TO LIST");
                    SphereCollider test = GameObject.Find(param.getAtIndex(1).iData.ToString()).AddComponent<SphereCollider>();
                    //SimpleConsole.print("SET TEST");
                    GameObject.Find(param.getAtIndex(1).iData.ToString()).GetComponent<SphereCollider>().contactOffset = param.getAtIndex(3).fData;

                }
                else
                {
                    SimpleConsole.print("ERROR: POI ALREADY IN LIST");
                }




            }



        }
        else if (param.getAtIndex(0).sData == "-list")
        {
            if (poiList.Count == 0)
            {
                SimpleConsole.print("EMPTY LIST");

            }
            else
            {
                foreach (GameObject go in poiList)
                {
                    SimpleConsole.print(go.name);
                }
            }
        }
        else if (param.getAtIndex(0).sData == "vec")
        {
            AddAtoms other = GameObject.Find("UserObject").GetComponent<AddAtoms>();
            Vector3 pos = new Vector3(param.getAtIndex(1).fData, param.getAtIndex(2).fData, param.getAtIndex(3).fData);
            GameObject go = new GameObject();
       
            go.transform.position = pos;
            if (go.GetComponent<attchas>() == null)
            {
                go.AddComponent<attchas>();


            }

                SphereCollider test = go.AddComponent<SphereCollider>();

                test.contactOffset = param.getAtIndex(3).fData;

            poiList.Add(go);
        }

        return "finished";
    }
    string hide(SimpleContainer param)
    {

        if (param.getAtIndex(0).sData == "index")
        {
            if (!param.getAtIndex(1).isInt() | param.getAtIndex(2).sData != "to" | !param.getAtIndex(3).isInt())
            {
                SimpleConsole.print("ERROR: PLEASE CHECK YOUR PARAMATERS FOR THIS FUNCTION (int to int)");
            }
            else if (param.getAtIndex(1).iData > numAtoms | param.getAtIndex(3).iData > numAtoms | param.getAtIndex(1).iData < 0 | param.getAtIndex(3).iData < 0)
            {
                SimpleConsole.print("ERROR: MAKE SURE YOUR INTEGERS ARE WITHIN RANGE ");


            }
            else
            {
                //This has to be done manually considering that its for an index range.
                int a = param.getAtIndex(1).iData;
                int c = param.getAtIndex(3).iData;
                //Time to mark these guys with an object first
                //Then what I need to do is turn off or make less visable the other atoms
                // SimpleConsole.print(atomsList.Count.ToString());
                for (int i = a; i <= c; i++)
                {
                    Renderer[] renderers = atomsList[i].GetComponentsInChildren<Renderer>();
                    foreach (Renderer r in renderers)
                    {

                        if (r.enabled == false)
                        {
                            r.enabled = true;
                        }
                        else if (r.enabled == true)
                        {
                            r.enabled = false;
                        }
                    }
                    // SimpleConsole.print("End");


                }
            }

        }
        if (param.getAtIndex(0).sData == "ribbons")
        {
            SimpleConsole.print("ribbon work");
            other = GameObject.Find("UserObject").GetComponent<AddAtoms>();
            if (other.ribbonShowing)
            {
                other.ribbonShowing = other.resetProtein("ribbons");
            }
            else
            {
                other.ribbonShowing = other.showMode("ribbons");
            }



        }
        //Again, this has to be done manually considering that its for the specific atoms in residues.
        if (param.getAtIndex(0).sData == "res-id")
        {
            other = GameObject.Find("UserObject").GetComponent<AddAtoms>();
            if (other.proteinRes.Count != 0)
            {
                int a = param.getAtIndex(1).iData;
                int b = other.resIDS.FindIndex(x => x == a);
                int[] res = other.proteinRes[b];
                for (int i = 0; i <= res.Length; i++)
                {
                    Renderer[] renderers = atomsList[res[i]].GetComponentsInChildren<Renderer>();
                    foreach (Renderer r in renderers)
                    {

                        if (r.enabled == false)
                        {
                            r.enabled = true;
                        }
                        else if (r.enabled == true)
                        {
                            r.enabled = false;
                        }
                    }
                    // SimpleConsole.print("End");


                }
            }
        }
        return "Ok";
    }

}
