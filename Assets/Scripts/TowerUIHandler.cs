using UnityEngine;

/// <summary>
/// Endless játékmódban a tornyok fölött megjelenő UIt irányítja oly módon, hogy egyszerre csak egy legyen megjelenítve.
/// </summary>
public class TowerUIHandler : MonoBehaviour
{
    private Tower target;

    public void SetTarget(Tower _target)
    {

        if (target == null)
        {
            //ha még nemvolt előző target, akkor ennek felkapcs
            target = _target;
            target.canvas.gameObject.SetActive(true);
            target.SetIsLevelShowing(true);
            
            return;

        }

        if (target == _target && target.GetIsLevelShowing() == true)
        {
            //ha ugyanarra kattint és felvan kapcsolva, akkor lekapcs
            target.canvas.gameObject.SetActive(false);
            target.SetIsLevelShowing(false);

        }
        else
        {
            //elozo lekapcs
            target.canvas.gameObject.SetActive(false);
            target.SetIsLevelShowing(false);

            //uj felkapcs
            _target.SetLevelText();
            _target.canvas.SetActive(true);
            _target.SetIsLevelShowing(true);
        }

        
        target = _target;


    }




}
