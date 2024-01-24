using UnityEngine;

[DisallowMultipleComponent]
public class GUI_Cursor : MonoBehaviour
{
    float goRate = 0.2f;
    int goCursorState = 0;
    int goCursorNum;

    float deployCursorSize = 40.0f;
    float deployRate = 0.4f;
    int deployCursorState = 0;
    int deployCursorNum;

    float InvalidSize = 30.0f;
    float FixSize = 30.0f;
    float SellSize = 30.0f;

    float bIntRate = 0.4f;
    int bIntCursorState = 0;
    int bIntCursorNum;
    float bIntSize = 40.0f;

    float mouseX, mouseY;

    float CursorTimer = 0;

    float hoverSize = 0.8f;
    float currentHoverSize;
    float maxHoverSize = 1.5f;
    float hoverRate = 1.2f;

    float attackRate = 0.2f;
    int attackCursorState = 0;
    int attackCursorNum;

    public enum CursorState
    {
        normal,
        hover,
        go,
        attack,
        deploy,
        invalid,
        invalidDeploy,
        fix,
        fixInvalid,
        sell,
        sellInvalid,
        bInt,
    };

    public CursorState cursorState = CursorState.normal;

    GUI_CursorSprites GUISprites;

    void Start()
    {
        Cursor.visible = false;
        GUISprites = GetComponent<GUI_CursorSprites>();        

        currentHoverSize = maxHoverSize;

        attackCursorNum = GUISprites.AttackCursor.Length;
        goCursorNum = GUISprites.GoCursor.Length;
        deployCursorNum = GUISprites.DeployCursor.Length;
        bIntCursorNum = GUISprites.InteractCursor.Length;
    }

    void OnGUI()
    {
        GetInputs();

        CursorTimer += Time.deltaTime;

        if (Event.current.type.Equals(EventType.Repaint))
        {
            switch (cursorState)
            {
                case CursorState.normal:
                    Graphics.DrawTexture(new Rect(mouseX, Screen.height - mouseY, 20, 20), GUISprites.NormalCursor, GUISprites.CursorMaterial);
                    break;
                case CursorState.hover:
                    currentHoverSize -= hoverRate * Time.deltaTime;
                    if (currentHoverSize <= hoverSize) currentHoverSize = maxHoverSize;
                    Graphics.DrawTexture(new Rect(mouseX - (10 * currentHoverSize), Screen.height - mouseY - (10 * currentHoverSize), (20 * currentHoverSize), (20 * currentHoverSize)), GUISprites.HoverCursor, GUISprites.CursorMaterial);
                    break;
                case CursorState.go:
                    if (CursorTimer > goRate)
                    {
                        CursorTimer = 0;
                        goCursorState++;
                        if (goCursorState >= goCursorNum) goCursorState = 0;
                    }
                    Graphics.DrawTexture(new Rect(mouseX - (20), Screen.height - mouseY - (20), (40), (40)), GUISprites.GoCursor[goCursorState], GUISprites.CursorMaterial);
                    break;
                case CursorState.attack:
                    if (CursorTimer > attackRate)
                    {
                        CursorTimer = 0;
                        attackCursorState++;
                        if (attackCursorState >= attackCursorNum) attackCursorState = 0;
                    }
                    Graphics.DrawTexture(new Rect(mouseX - (20), Screen.height - mouseY - (20), (40), (40)), GUISprites.AttackCursor[attackCursorState], GUISprites.CursorMaterial);
                    break;
                case CursorState.deploy:
                    if (CursorTimer > deployRate)
                    {
                        CursorTimer = 0;
                        deployCursorState++;
                        if (deployCursorState >= deployCursorNum) deployCursorState = 0;
                    }
                    Graphics.DrawTexture(new Rect(mouseX - (deployCursorSize / 2), Screen.height - mouseY - (deployCursorSize / 2), (deployCursorSize), (deployCursorSize)), GUISprites.DeployCursor[deployCursorState], GUISprites.CursorMaterial);
                    break;
                case CursorState.invalid:
                    Graphics.DrawTexture(new Rect(mouseX - (InvalidSize / 2), Screen.height - mouseY - (InvalidSize / 2), (InvalidSize), (InvalidSize)), GUISprites.InvalidCursor, GUISprites.CursorMaterial);
                    break;
                case CursorState.invalidDeploy:
                    if (CursorTimer > deployRate)
                    {
                        CursorTimer = 0;
                        deployCursorState++;
                        if (deployCursorState >= deployCursorNum) deployCursorState = 0;
                    }
                    Graphics.DrawTexture(new Rect(mouseX - (deployCursorSize / 2), Screen.height - mouseY - (deployCursorSize / 2), (deployCursorSize), (deployCursorSize)), GUISprites.DeployCursor[deployCursorState], GUISprites.CursorMaterial);
                    Graphics.DrawTexture(new Rect(mouseX - (InvalidSize / 2), Screen.height - mouseY - (InvalidSize / 2), (InvalidSize), (InvalidSize)), GUISprites.InvalidCursor, GUISprites.CursorMaterial);
                    break;
                case CursorState.fix:
                    Graphics.DrawTexture(new Rect(mouseX - (FixSize / 2), Screen.height - mouseY - (FixSize / 2), FixSize, FixSize), GUISprites.FixCursor, GUISprites.CursorMaterial);
                    break;
                case CursorState.fixInvalid:
                    Graphics.DrawTexture(new Rect(mouseX - (FixSize / 2), Screen.height - mouseY - (FixSize / 2), FixSize, FixSize), GUISprites.FixCursor, GUISprites.CursorMaterial);
                    Graphics.DrawTexture(new Rect(mouseX - (InvalidSize / 2), Screen.height - mouseY - (InvalidSize / 2), (InvalidSize), (InvalidSize)), GUISprites.InvalidCursor, GUISprites.CursorMaterial);
                    break;
                case CursorState.sell:
                    Graphics.DrawTexture(new Rect(mouseX - (SellSize / 2), Screen.height - mouseY - (SellSize / 2), SellSize, SellSize), GUISprites.SellCursor, GUISprites.CursorMaterial);
                    break;
                case CursorState.sellInvalid:
                    Graphics.DrawTexture(new Rect(mouseX - (SellSize / 2), Screen.height - mouseY - (SellSize / 2), SellSize, SellSize), GUISprites.SellCursor, GUISprites.CursorMaterial);
                    Graphics.DrawTexture(new Rect(mouseX - (InvalidSize / 2), Screen.height - mouseY - (InvalidSize / 2), (InvalidSize), (InvalidSize)), GUISprites.InvalidCursor, GUISprites.CursorMaterial);
                    break;
                case CursorState.bInt:
                    if (CursorTimer > bIntRate)
                    {
                        CursorTimer = 0;
                        bIntCursorState++;
                        if (bIntCursorState >= bIntCursorNum) bIntCursorState = 0;
                    }
                    Graphics.DrawTexture(new Rect(mouseX - (bIntSize / 2), Screen.height - mouseY - (bIntSize / 2), bIntSize, bIntSize), GUISprites.InteractCursor[bIntCursorState], GUISprites.CursorMaterial);
                    break;
            }
        }
    }

    void GetInputs()
    {
        Vector2 MousePosition = Input.mousePosition;
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;
    }

    public void SetCursorState(string state)
    {
        switch (state)
        {
            case "normal":
                cursorState = CursorState.normal;
                break;
            case "hover":
                cursorState = CursorState.hover;
                break;
            case "attack":
                cursorState = CursorState.attack;
                break;
            case "deploy":
                cursorState = CursorState.deploy;
                break;
            case "go":
                cursorState = CursorState.go;
                break;
            case "invalid":
                cursorState = CursorState.invalid;
                break;
            case "invalidDeploy":
                cursorState = CursorState.invalidDeploy;
                break;
            case "sell":
                cursorState = CursorState.sell;
                break;
            case "sellInvalid":
                cursorState = CursorState.sellInvalid;
                break;
            case "fix":
                cursorState = CursorState.fix;
                break;
            case "fixInvalid":
                cursorState = CursorState.fixInvalid;
                break;
            case "bInt":
                cursorState = CursorState.bInt;
                break;
            default:
                break;
        }
    }
}
