namespace Argos.Framework
{
    /// Author: (Twitter) @matheuslrod
    /// Source: https://gist.github.com/matheuslessarodrigues/13d08f49977a828b6565a76a2e8967e5
    /// 
    /// Modification:
    /// Author: José Miguel Sánchez Fernandez (Twitter) @ex3_tlsa
    /// 
    /// Added field for set a custom label and optional tooltip message.
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class ButtonAttribute : System.Attribute
    {
        public string Label;
        public string TooltipMessage;

        /// <summary>
        /// Quick button inspector.
        /// </summary>
        /// <param name="label">Button label. If leave empty, uses the method name.</param>
        /// <param name="tooltip">Optional button tooltip.</param>
        public ButtonAttribute(string label = "", string tooltip = "")
        {
            this.Label = label;
            this.TooltipMessage = tooltip;
        }
    }
}