using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fadeintext : MonoBehaviour
{
    
    
        private TextMeshProUGUI i;
        private void Start()
        {
            i = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            Color c = i.color;
            c += new Color(0, 0, 0, .1f) * Time.deltaTime;
            i.color = c;

            if (c.a >= 1)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    

