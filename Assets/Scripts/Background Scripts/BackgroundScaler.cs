using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Screen.width / Screen.height;

        if(gameObject.name == "Background") {
            transform.localScale = new Vector3(width, height, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(width + 5f, 3f, transform.localScale.z);
        }
    }
}
