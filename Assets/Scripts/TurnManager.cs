using UnityEngine;

public class TurnManager {

    public event System.Action OnTick;
    public event System.Action OnTickEnded;
    
    private int m_TurnCount;

    public TurnManager() {

        m_TurnCount = 1;

    }

    public void Tick() {

        m_TurnCount += 1;
        
        //Debug.Log("Current Turn: " + m_TurnCount);
        
        OnTick?.Invoke();
        OnTickEnded?.Invoke();
    }
}
