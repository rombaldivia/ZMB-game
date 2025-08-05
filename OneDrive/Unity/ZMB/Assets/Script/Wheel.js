class Weapon
{
    var Name : String = "";
    var icon : Texture2D;
    var object : GameObject;
    //var drawWeaponAnim : Animation;
}

public var weapons : Weapon[];
public var buttonSize : Vector2 = Vector2(50, 50);
public var spread : float = 100;
public var iconStyle : GUIStyle;

private var selectedWeapon : int;
private var guiSettingsApplied : boolean;
private var fxClamp : float;
private var curFXFloat : float;
private var buttonRect : Rect;

function Start()
{
    for(var i = 0; i < weapons.Length; i++)
    {
        if(i != selectedWeapon) weapons[i].object.SetActive(false);
        else weapons[i].object.SetActive(true);
    }
}

function Update()
{
    if(Input.GetKeyDown("z"))
    {
        if(fxClamp == 0) fxClamp = 1;
        else fxClamp = 0;
    }
    if(curFXFloat != fxClamp) curFXFloat = Mathf.MoveTowards(curFXFloat, fxClamp, Time.deltaTime * 5);
}

function OnGUI()
{
    if(curFXFloat != 0.0)
    {
        GUI.color.a = curFXFloat;
        var fxOffset = spread * (1.0 + (1.0 - curFXFloat));
        for(var i : float = 0; i < weapons.Length; i++)
        {
            var progression = i / weapons.Length;
            var angle = progression * Mathf.PI * 2 + curFXFloat;
            buttonRect = Rect(Screen.width/2.0 + (Mathf.Sin(angle) * fxOffset) - (buttonSize.x/2.0), Screen.height/2 + (Mathf.Cos(angle) * fxOffset) - (buttonSize.y/2.0), buttonSize.x, buttonSize.y);
            if(buttonRect.Contains(Event.current.mousePosition))
            {
                buttonRect.width *= 1.2;
                buttonRect.height *= 1.2;
                buttonRect.x -= (buttonRect.width - buttonSize.x) / 2.0;
                buttonRect.y -= (buttonRect.height - buttonSize.y) / 2.0;
            }
            if(weapons[i].icon)
            {
                iconStyle.normal.background = weapons[i].icon;
                if(GUI.Button(buttonRect, "", iconStyle))
                {
                    SwapWeapons(i);
                }
            }
            else
            {
                iconStyle.normal.background = null;
                if(GUI.Button(buttonRect, weapons[i].Name, iconStyle))
                {
                    SwapWeapons(i);
                }
            }
        }
        if(weapons[selectedWeapon].icon) GUI.DrawTexture(Rect(Screen.width/2 - (buttonSize.x/2.0), Screen.height/2 - (buttonSize.y/2.0), buttonSize.x, buttonSize.y), weapons[selectedWeapon].icon);
        else GUI.Label(Rect(Screen.width/2 - (buttonSize.x/2.0), Screen.height/2 - (buttonSize.y/2.0), buttonSize.x, buttonSize.y), weapons[selectedWeapon].Name);
    }
}

function SwapWeapons(to : int)
{
    weapons[selectedWeapon].object.SetActive(false);
    weapons[to].object.SetActive(true);
    selectedWeapon = to;
    //Play weapons[to].animation on your animation component. Make sure you uncomment it in the class above
}