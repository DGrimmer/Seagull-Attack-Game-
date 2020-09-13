using UnityEngine;

public class BuildFunctionality : MonoBehaviour
{
    private void Start() {
        //Set Cursor to not be visible
        Cursor.visible = false;
    }
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
            Application.Quit();
    }
}
