﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DS2S_META.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DS2S_META.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 0:  48 83 ec 28             sub    rsp,0x28 
        ///4:  48 b9 00 00 00 00 ff    movabs rcx,0xffffffff00000000 ;PlayerParam Pointer
        ///b:  ff ff ff
        ///e:  48 c7 c2 f4 01 00 00    mov    rdx,0x1f4 ;number of souls
        ///15: 49 be 00 00 00 00 ff    movabs r14,0xffffffff00000000 ;Give Souls func
        ///1c: ff ff ff
        ///1f: 41 ff d6                call   r14
        ///22: 48 83 c4 28             add    rsp,0x28
        ///26: c3                      ret .
        /// </summary>
        internal static string AddSouls {
            get {
                return ResourceManager.GetString("AddSouls", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to //Format: ID; SL VRG END VIT ATN STR DEX ADP INT FTH; Name [arbitrary]
        ///01 12 07 06 06 05 15 11 05 05 05 Warrior
        ///02 13 12 06 07 04 11 08 09 03 06 Knight
        ///04 11 09 07 11 02 09 14 03 01 08 Bandit
        ///06 14 10 03 08 10 11 05 04 04 12 Cleric
        ///07 11 05 06 05 12 03 07 08 14 04 Sorcerer
        ///08 10 07 06 09 07 06 06 12 05 05 Explorer
        ///09 12 04 08 04 06 09 16 06 07 05 Swordsman
        ///10 01 06 06 06 06 06 06 06 06 06 Deprived.
        /// </summary>
        internal static string Classes {
            get {
                return ResourceManager.GetString("Classes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 0:  48 81 ec e8 01 00 00    sub    rsp,0x1e8
        ///7:  41 b8 08 00 00 00       mov    r8d,0x8 ;item amount
        ///d:  49 bf 00 00 00 00 ff    movabs r15,0xffffffff00000000 ;Item Struct Address
        ///14: ff ff ff
        ///17: 49 8d 17                lea    rdx,[r15]
        ///1a: 48 b9 00 00 00 00 ff    movabs rcx,0xffffffff00000000 ;Item bag?
        ///21: ff ff ff
        ///24: 45 31 c9                xor    r9d,r9d
        ///27: 49 be 00 00 00 00 ff    movabs r14,0xffffffff00000000 ;Call add item function DarkSoulsII.exe+1A8C67
        ///2e: ff ff ff
        ///31: 41 ff d6          [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GiveItemWithMenu {
            get {
                return ResourceManager.GetString("GiveItemWithMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 0:  48 83 ec 28             sub    rsp,0x28
        ///4:  41 b8 08 00 00 00       mov    r8d,0x8
        ///a:  49 bf 00 00 00 00 ff    movabs r15,0xffffffff00000000 ;Item Struct Address
        ///11: ff ff ff
        ///14: 49 8d 17                lea    rdx,[r15]
        ///17: 48 b9 00 00 00 00 ff    movabs rcx,0xffffffff00000000 ;Item bag?
        ///1e: ff ff ff
        ///21: 45 31 c9                xor    r9d,r9d
        ///24: 49 be 00 00 00 00 ff    movabs r14,0xffffffff00000000 ;Call add item function DarkSoulsII.exe+1A8C67
        ///2b: ff ff ff
        ///2e: 41 ff d6                call    [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GiveItemWithoutMenu {
            get {
                return ResourceManager.GetString("GiveItemWithoutMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 0:  e9 00 00 00 00          jmp    ;Jump to new instructions
        ///5:  90                      nop
        ///6:  90                      nop
        ///7:  90                      nop
        ///8:  f3 0f 59 1d ff ff 00    mulss  xmm3,DWORD PTR [rip+0xffff] ;Move float into register
        ///f:  00
        ///10: e9 ff ff 00 00          jmp    ;Jump back to AoB Pointer + 0x8.
        /// </summary>
        internal static string SpeedFactor {
            get {
                return ResourceManager.GetString("SpeedFactor", resourceCulture);
            }
        }
    }
}