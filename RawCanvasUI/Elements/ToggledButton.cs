using RawCanvasUI.Mouse;

namespace RawCanvasUI.Elements
{
    /// <summary>
    /// Represents a labeled texture button that can be toggled on and off.
    /// </summary>
    public class ToggledButton : TextureButton
{
    private readonly string activeTextureName;
    private readonly string inactiveTextureName;
    private readonly bool isActivateOnly;
    private bool isActive = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggledButton"/> class.
    /// </summary>
    /// <param name="id">The unique id of the button.</param>
    /// <param name="width">The width of the button.</param>
    /// <param name="height">The height of the button.</param>
    /// <param name="activeTextureName">The texture name for the button in an active state.</param>
    /// <param name="inactiveTextureName">The texture name for the button in an inactive state.</param>
    /// <param name="text">The text to display on the button.</param>
    /// <param name="isActivateOnly">When set to true, the button can only be pressed into the on state, it cannot be toggled off by clicking.</param>
    public ToggledButton(string id, int width, int height, string activeTextureName, string inactiveTextureName, string text, bool isActivateOnly = false)
        : base(id, inactiveTextureName, width, height, text)
    {
        this.activeTextureName = activeTextureName;
        this.inactiveTextureName = inactiveTextureName;
        this.isActivateOnly = isActivateOnly;
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not the button is active (i.e. toggled on).
    /// </summary>
    public bool IsActive
    {
        get
        {
            return this.isActive;
        }

        set
        {
            this.isActive = value;
            this.SetTextureName(this.isActive ? this.activeTextureName : this.inactiveTextureName);
        }
    }

    /// <inheritdoc/>
    public override void Click(Cursor cursor)
    {
        if (!this.isActivateOnly || !this.isActive)
        {
            this.Toggle();
            base.Click(cursor);
        }
    }

    /// <summary>
    /// Toggles the state of the button.
    /// </summary>
    public void Toggle()
    {
        this.IsActive = !this.IsActive;
    }
}
}
