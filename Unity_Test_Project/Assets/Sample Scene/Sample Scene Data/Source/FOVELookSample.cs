using UnityEngine;
using System.Collections;

public class FOVELookSample : MonoBehaviour {
    public Light l;
	public OSC osc;
	Collider my_collider;
    private Material m;
    private bool light_enabled = false;


    // Use this for initialization
    void Start() {
        my_collider = GetComponent<Collider>();

        l = this.transform.GetComponentInChildren<Light>();
        if (l)
        {
            light_enabled = true;
            l.enabled = false;
        }
        m = gameObject.GetComponent<Renderer>().material;
    }

	// Update is called once per frame
	void Update () {
		//sendMsg (my_collider);
        if (FoveInterface.IsLookingAtCollider(my_collider))
//send osc message mycollider.position
		{	
			//Debug.Log ("HIER"+my_collider.transform.position.x+" "+my_collider.transform.position.y+" "+my_collider.transform.position.z);
			sendMsg (my_collider);
			//OscSender.sendMsg (my_collider);
            if (light_enabled)
            {
                l.enabled = true;
                m.SetColor("_EmissionColor", l.color);
                DynamicGI.SetEmissive(GetComponent<Renderer>(), l.color);
                m.EnableKeyword("_EMISSION");
            }
            //bool check = FoveInterface.IsLookingAtCollider(my_collider);
        } else
		{
			gameObject.GetComponent<Renderer> ().material.color = Color.white;
            //GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            m.DisableKeyword("_EMISSION");
            if (light_enabled)
            {
                l.enabled = false;
                DynamicGI.SetEmissive(GetComponent<Renderer>(), Color.black);
            }
        }
	}

	public void sendMsg(Collider my_collider)
	{
		OscMessage message = new OscMessage();

		message.address = "/sound/" + my_collider.gameObject.name.Substring(0,4);//"/UpdateXYZ";
		//Debug.Log ("COLLIDER:" +my_collider.transform.position.x+" "+my_collider.transform.position.y+" "+my_collider.transform.position.z);
		//Decide which object is my_collider and thus give it an integer to choose correct soundfile
		//int i=0;
		//Debug.Log (my_collider.gameObject.name);
		/*switch (my_collider.gameObject.name) {
		case "head":
			Debug.Log ("HOOFD");
			i = 0;
			break;
		case "ball":
			i = 1;
			break;
		case "rocket":
			i = 2;
			break;
		
		default:
			i = 3;
			Debug.Log ("een gewone doos");
			break;
		}*/
		//Debug.Log("")
		//message.values.Add (my_collider.gameObject.name);
		//Add XYZ values of my_collider
		message.values.Add(my_collider.transform.position.x);
		message.values.Add(my_collider.transform.position.y);
		message.values.Add(my_collider.transform.position.z);
		Debug.Log ("OSC Message: " + message);
		osc.Send(message);
	}
}
