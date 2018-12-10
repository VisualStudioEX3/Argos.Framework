namespace Argos.Framework
{
    /// <summary>
    /// Attribute used to make a method (void) in a script be a button.
    /// </summary>
    /// <remarks>
    /// Source: https://gist.github.com/matheuslessarodrigues/13d08f49977a828b6565a76a2e8967e5
    /// 
    /// Modification and extras:
    /// Added field for set a custom label and optional tooltip message.</remarks>
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class ButtonAttribute : System.Attribute
    {
        #region Public vars
        public readonly string Label;
        public readonly string TooltipMessage;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Button label. If leave empty, uses the method name.</param>
        /// <param name="tooltip">Optional button tooltip.</param>
        public ButtonAttribute(string label = "", string tooltip = "")
        {
            this.Label = label;
            this.TooltipMessage = tooltip;
        }
        #endregion
    }
}