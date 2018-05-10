using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Argos.Framework.Input
{
    /// <summary>
    /// Valid keyboard and mouse button codes.
    /// </summary>
    public enum KeyboardMouseCodes
    {
        /// <summary>
        /// Not assigned (never returned as the result of a keystroke).
        /// </summary>
        None = KeyCode.None,
        /// <summary>
        /// The backspace key.
        /// </summary>
        Backspace = KeyCode.Backspace,
        /// <summary>
        /// The tab key.
        /// </summary>
        Tab = KeyCode.Tab,
        /// <summary>
        /// The Clear key.
        /// </summary>
        Clear = KeyCode.Clear,
        /// <summary>
        /// Return key.
        /// </summary>
        Return = KeyCode.Return,
        /// <summary>
        /// Escape key.
        /// </summary>
        Escape = KeyCode.Escape,
        /// <summary>
        /// Space key.
        /// </summary>
        Space = KeyCode.Space,
        /// <summary>
        /// Exclamation mark key '!'.
        /// </summary>
        Exclaim = KeyCode.Exclaim,
        /// <summary>
        /// Double quote key '"'.
        /// </summary>
        DoubleQuote = KeyCode.DoubleQuote,
        /// <summary>
        /// Hash key '#'.
        /// </summary>
        Hash = KeyCode.Hash,
        /// <summary>
        /// Dollar sign key '$'.
        /// </summary>
        Dollar = KeyCode.Dollar,
        /// <summary>
        /// Ampersand key '&'.
        /// </summary>
        Ampersand = KeyCode.Ampersand,
        /// <summary>
        /// Quote key '.
        /// </summary>
        Quote = KeyCode.Quote,
        /// <summary>
        /// Left Parenthesis key '('.
        /// </summary>
        LeftParen = KeyCode.LeftParen,
        /// <summary>
        /// Right Parenthesis key ')'.
        /// </summary>
        RightParen = KeyCode.RightParen,
        /// <summary>
        /// Asterisk key '*'.
        /// </summary>
        Asterisk = KeyCode.Asterisk,
        /// <summary>
        /// Plus key '+'.
        /// </summary>
        Plus = KeyCode.Plus,
        /// <summary>
        /// Comma ',' key.
        /// </summary>
        Comma = KeyCode.Comma,
        /// <summary>
        /// Minus '-' key.
        /// </summary>
        Minus = KeyCode.Minus,
        /// <summary>
        /// Period '.' key.
        /// </summary>
        Period = KeyCode.Period,
        /// <summary>
        /// Slash '/' key.
        /// </summary>
        Slash = KeyCode.Slash,
        /// <summary>
        /// The '0' key on the top of the alphanumeric keyboard.
        /// </summary>
        Alpha0 = KeyCode.Alpha0,
        /// <summary>
        /// The '1' key on the top of the alphanumeric keyboard.
        /// </summary>
        Alpha1 = KeyCode.Alpha1,
        /// <summary>
        /// The '2' key on the top of the alphanumeric keyboard.
        /// </summary>
        Alpha2 = KeyCode.Alpha2,
        /// <summary>
        /// The '3' key on the top of the alphanumeric keyboard.
        /// </summary>
        Alpha3 = KeyCode.Alpha3,
        /// <summary>
        /// The '4' key on the top of the alphanumeric keyboard.
        /// </summary>
        Alpha4 = KeyCode.Alpha4,
        /// <summary>
        /// The '5' key on the top of the alphanumeric keyboard.
        /// </summary>
        Alpha5 = KeyCode.Alpha5,
        /// <summary>
        /// The '6' key on the top of the alphanumeric keyboard.
        /// </summary>
        Alpha6 = KeyCode.Alpha6,
        /// <summary>
        /// The '7' key on the top of the alphanumeric keyboard.
        /// </summary>
        Alpha7 = KeyCode.Alpha7,
        /// <summary>
        /// The '8' key on the top of the alphanumeric keyboard.
        /// </summary>
        Alpha8 = KeyCode.Alpha8,
        /// <summary>
        /// The '9' key on the top of the alphanumeric keyboard.
        /// </summary>
        Alpha9 = KeyCode.Alpha9,
        /// <summary>
        /// Colon ':' key.
        /// </summary>
        Colon = KeyCode.Colon,
        /// <summary>
        /// Semicolon ';' key.
        /// </summary>
        Semicolon = KeyCode.Semicolon,
        /// <summary>
        /// Less than '<' key.
        /// </summary>
        Less = KeyCode.Less,
        /// <summary>
        /// Equals '=' key.
        /// </summary>
        Equals = KeyCode.Equals,
        /// <summary>
        /// Greater than '>' key.
        /// </summary>
        Greater = KeyCode.Greater,
        /// <summary>
        /// Question mark '?' key.
        /// </summary>
        Question = KeyCode.Question,
        /// <summary>
        /// At key '@'.
        /// </summary>
        At = KeyCode.At,
        /// <summary>
        /// Left square bracket key '['.
        /// </summary>
        LeftBracket = KeyCode.LeftBracket,
        /// <summary>
        /// Backslash key '\'.
        /// </summary>
        Backslash = KeyCode.Backslash,
        /// <summary>
        /// Right square bracket key ']'.
        /// </summary>
        RightBracket = KeyCode.RightBracket,
        /// <summary>
        /// Caret key '^'.
        /// </summary>
        Caret = KeyCode.Caret,
        /// <summary>
        /// Underscore '_' key.
        /// </summary>
        Underscore = KeyCode.Underscore,
        /// <summary>
        /// Back quote key '`'.
        /// </summary>
        BackQuote = KeyCode.BackQuote,
        /// <summary>
        /// 'a' key.
        /// </summary>
        A = KeyCode.A,
        /// <summary>
        /// 'b' key.
        /// </summary>
        B = KeyCode.B,
        /// <summary>
        /// 'c' key.
        /// </summary>
        C = KeyCode.C,
        /// <summary>
        /// 'd' key.
        /// </summary>
        D = KeyCode.D,
        /// <summary>
        /// 'e' key.
        /// </summary>
        E = KeyCode.E,
        /// <summary>
        /// 'f' key.
        /// </summary>
        F = KeyCode.F,
        /// <summary>
        /// 'g' key.
        /// </summary>
        G = KeyCode.G,
        /// <summary>
        /// 'h' key.
        /// </summary>
        H = KeyCode.H,
        /// <summary>
        /// 'i' key.
        /// </summary>
        I = KeyCode.I,
        /// <summary>
        /// 'j' key.
        /// </summary>
        J = KeyCode.J,
        /// <summary>
        /// 'k' key.
        /// </summary>
        K = KeyCode.K,
        /// <summary>
        /// 'l' key.
        /// </summary>
        L = KeyCode.L,
        /// <summary>
        /// 'm' key.
        /// </summary>
        M = KeyCode.M,
        /// <summary>
        /// 'n' key.
        /// </summary>
        N = KeyCode.N,
        /// <summary>
        /// 'o' key.
        /// </summary>
        O = KeyCode.O,
        /// <summary>
        /// 'p' key.
        /// </summary>
        P = KeyCode.P,
        /// <summary>
        /// 'q' key.
        /// </summary>
        Q = KeyCode.Q,
        /// <summary>
        /// 'r' key.
        /// </summary>
        R = KeyCode.R,
        /// <summary>
        /// 's' key.
        /// </summary>
        S = KeyCode.S,
        /// <summary>
        /// 't' key.
        /// </summary>
        T = KeyCode.T,
        /// <summary>
        /// 'u' key.
        /// </summary>
        U = KeyCode.U,
        /// <summary>
        /// 'v' key.
        /// </summary>
        V = KeyCode.V,
        /// <summary>
        /// 'w' key.
        /// </summary>
        W = KeyCode.W,
        /// <summary>
        /// 'x' key.
        /// </summary>
        X = KeyCode.X,
        /// <summary>
        /// 'y' key.
        /// </summary>
        Y = KeyCode.Y,
        /// <summary>
        /// 'z' key.
        /// </summary>
        Z = KeyCode.Z,
        /// <summary>
        /// The forward delete key.
        /// </summary>
        Delete = KeyCode.Delete,
        /// <summary>
        /// Numeric keypad 0.
        /// </summary>
        Keypad0 = KeyCode.Keypad0,
        /// <summary>
        /// Numeric keypad 1.
        /// </summary>
        Keypad1 = KeyCode.Keypad1,
        /// <summary>
        /// Numeric keypad 2.
        /// </summary>
        Keypad2 = KeyCode.Keypad2,
        /// <summary>
        /// Numeric keypad 3.
        /// </summary>
        Keypad3 = KeyCode.Keypad3,
        /// <summary>
        /// Numeric keypad 4.
        /// </summary>
        Keypad4 = KeyCode.Keypad4,
        /// <summary>
        /// Numeric keypad 5.
        /// </summary>
        Keypad5 = KeyCode.Keypad5,
        /// <summary>
        /// Numeric keypad 6.
        /// </summary>
        Keypad6 = KeyCode.Keypad6,
        /// <summary>
        /// Numeric keypad 7.
        /// </summary>
        Keypad7 = KeyCode.Keypad7,
        /// <summary>
        /// Numeric keypad 8.
        /// </summary>
        Keypad8 = KeyCode.Keypad8,
        /// <summary>
        /// Numeric keypad 9.
        /// </summary>
        Keypad9 = KeyCode.Keypad9,
        /// <summary>
        /// Numeric keypad '.'.
        /// </summary>
        KeypadPeriod = KeyCode.KeypadPeriod,
        /// <summary>
        /// Numeric keypad '/'.
        /// </summary>
        KeypadDivide = KeyCode.KeypadDivide,
        /// <summary>
        /// Numeric keypad '*'.
        /// </summary>
        KeypadMultiply = KeyCode.KeypadMultiply,
        /// <summary>
        /// Numeric keypad '-'.
        /// </summary>
        KeypadMinus = KeyCode.KeypadMinus,
        /// <summary>
        /// Numeric keypad '+'.
        /// </summary>
        KeypadPlus = KeyCode.KeypadPlus,
        /// <summary>
        /// Numeric keypad enter.
        /// </summary>
        KeypadEnter = KeyCode.KeypadEnter,
        /// <summary>
        /// Numeric keypad '='.
        /// </summary>
        KeypadEquals = KeyCode.KeypadEquals,
        /// <summary>
        /// Up arrow key.
        /// </summary>
        UpArrow = KeyCode.UpArrow,
        /// <summary>
        /// Down arrow key.
        /// </summary>
        DownArrow = KeyCode.DownArrow,
        /// <summary>
        /// Right arrow key.
        /// </summary>
        RightArrow = KeyCode.RightArrow,
        /// <summary>
        /// Left arrow key.
        /// </summary>
        LeftArrow = KeyCode.LeftArrow,
        /// <summary>
        /// Insert key key.
        /// </summary>
        Insert = KeyCode.Insert,
        /// <summary>
        /// Home key.
        /// </summary>
        Home = KeyCode.Home,
        /// <summary>
        /// End key.
        /// </summary>
        End = KeyCode.End,
        /// <summary>
        /// Page up.
        /// </summary>
        PageUp = KeyCode.PageUp,
        /// <summary>
        /// Page down.
        /// </summary>
        PageDown = KeyCode.PageDown,
        /// <summary>
        /// F1 function key.
        /// </summary>
        F1 = KeyCode.F1,
        /// <summary>
        /// F2 function key.
        /// </summary>
        F2 = KeyCode.F2,
        /// <summary>
        /// F3 function key.
        /// </summary>
        F3 = KeyCode.F3,
        /// <summary>
        /// F4 function key.
        /// </summary>
        F4 = KeyCode.F4,
        /// <summary>
        /// F5 function key.
        /// </summary>
        F5 = KeyCode.F5,
        /// <summary>
        /// F6 function key.
        /// </summary>
        F6 = KeyCode.F6,
        /// <summary>
        /// F7 function key.
        /// </summary>
        F7 = KeyCode.F7,
        /// <summary>
        /// F8 function key.
        /// </summary>
        F8 = KeyCode.F8,
        /// <summary>
        /// F9 function key.
        /// </summary>
        F9 = KeyCode.F9,
        /// <summary>
        /// F10 function key.
        /// </summary>
        F10 = KeyCode.F10,
        /// <summary>
        /// F11 function key.
        /// </summary>
        F11 = KeyCode.F11,
        /// <summary>
        /// F12 function key.
        /// </summary>
        F12 = KeyCode.F12,
        /// <summary>
        /// F13 function key.
        /// </summary>
        F13 = KeyCode.F13,
        /// <summary>
        /// F14 function key.
        /// </summary>
        F14 = KeyCode.F14,
        /// <summary>
        /// F15 function key.
        /// </summary>
        F15 = KeyCode.F15,
        /// <summary>
        /// Right shift key.
        /// </summary>
        RightShift = KeyCode.RightShift,
        /// <summary>
        /// Left shift key.
        /// </summary>
        LeftShift = KeyCode.LeftShift,
        /// <summary>
        /// Right Control key.
        /// </summary>
        RightControl = KeyCode.RightControl,
        /// <summary>
        /// Left Control key.
        /// </summary>
        LeftControl = KeyCode.LeftControl,
        /// <summary>
        /// Right Alt key.
        /// </summary>
        RightAlt = KeyCode.RightAlt,
        /// <summary>
        /// Left Alt key.
        /// </summary>
        LeftAlt = KeyCode.LeftAlt,
        /// <summary>
        /// Alt Gr key.
        /// </summary>
        AltGr = KeyCode.AltGr,
        /// <summary>
        /// The Left (or primary) mouse button.
        /// </summary>
        LeftMouseButton = KeyCode.Mouse0,
        /// <summary>
        /// Right mouse button (or secondary mouse button).
        /// </summary>
        RightMouseButton = KeyCode.Mouse1,
        /// <summary>
        /// Middle mouse button (or third button).
        /// </summary>
        MiddleMouseButton = KeyCode.Mouse2,
        /// <summary>
        /// Additional (fourth) mouse button.
        /// </summary>
        AdditionalMouseButton4 = KeyCode.Mouse3,
        /// <summary>
        /// Additional (fifth) mouse button.
        /// </summary>
        AdditionalMouseButton5 = KeyCode.Mouse4,
        /// <summary>
        /// Additional (or sixth) mouse button.
        /// </summary>
        AdditionalMouseButton6 = KeyCode.Mouse5,
        /// <summary>
        /// Additional (or seventh) mouse button.
        /// </summary>
        AdditionalMouseButton7 = KeyCode.Mouse6
    }
}