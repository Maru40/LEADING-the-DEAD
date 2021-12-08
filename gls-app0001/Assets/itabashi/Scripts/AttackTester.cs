using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Test
{
    void A();
}

public class AttackTester : MonoBehaviour, Test
{
    [SerializeField]
    private AudioOptioner audioOptioner;

    [SerializeField]
    private GameStateManager stateManager;

    private GameControls m_gamecontrols;

    [SerializeField]
    private UnityEngine.Events.UnityEvent testEvent;

    private void Awake()
    {
        m_gamecontrols = new GameControls();
        this.RegisterController(m_gamecontrols);

        m_gamecontrols.Player.UseMeat.performed += _ => testEvent?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void A()
    {
        Debug.Log("成功");
    }

    private void OnCollisionEnter(Collision collision)
    {
        var takeDamager = collision.gameObject.GetComponent<AttributeObject.TakeDamageObject>();

        takeDamager?.TakeDamage(new AttributeObject.DamageData(0,true));
    }
}
