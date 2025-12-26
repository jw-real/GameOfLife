using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButtonController : MonoBehaviour
{
    public void GoToCellSelect()
    {
        SceneManager.LoadScene("CellSelectMenu");
    }

    public void GoToRule1()
    {
        SceneManager.LoadScene("Rule1");
    }

    public void GoToRule2()
    {
        SceneManager.LoadScene("Rule2");
    }

    public void GoToRule3()
    {
        SceneManager.LoadScene("Rule3");
    }

    public void GoToRule4()
    {
        SceneManager.LoadScene("Rule4");
    }
}
