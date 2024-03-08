using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] GameObject specialButton;
    private int specialBtnClickCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void disableSpecial()
    {
        specialBtnClickCount++;
        if(specialBtnClickCount == 3) specialButton.SetActive(false);
        
    }

    public void enableSpecial()
    {
        specialBtnClickCount = 0;
        specialButton.SetActive(true);
    }
}
