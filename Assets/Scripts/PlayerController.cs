using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rect moveArea;

    private Rigidbody2D _body;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float posX = mousePos.x;
        float posY = mousePos.y;

        // Keep paddle in authorized area
        posX = Mathf.Clamp(posX, moveArea.x + transform.localScale.x / 2, moveArea.x + moveArea.width - transform.localScale.x / 2);
        posY = Mathf.Clamp(posY, moveArea.y + transform.localScale.y / 2, moveArea.y + moveArea.height - transform.localScale.y / 2);

        _body.MovePosition(new Vector2(posX, posY));
    }

}
