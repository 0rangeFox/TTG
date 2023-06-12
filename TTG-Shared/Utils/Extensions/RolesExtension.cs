using System.Drawing;
using TTG_Shared.Models;

namespace TTG_Shared.Utils.Extensions; 

public static class RolesExtension {

    public static Color GetColor(this Roles role) => role switch {
        Roles.Traitor => Color.Red,
        _ => Color.White
    };

    public static string GetString(this Roles role) => role switch {
        Roles.Traitor => "Traitor",
        _ => "Citizen"
    };

}