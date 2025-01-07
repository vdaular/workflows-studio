using Elsa.Studio.Contracts;
using MudBlazor;
using MudBlazor.Utilities;

namespace Elsa.Studio.Services;

/// <inheritdoc />
public class DefaultThemeService : IThemeService
{
    private MudTheme _currentTheme = CreateDefaultTheme();
    private bool _isDarkMode = false;

    /// <inheritdoc />
    public event Action? CurrentThemeChanged;

    /// <inheritdoc />
    public event Action? IsDarkModeChanged;

    /// <inheritdoc />
    public MudTheme CurrentTheme
    {
        get => _currentTheme;
        set
        {
            _currentTheme = value;
            CurrentThemeChanged?.Invoke();
        }
    }

    /// <inheritdoc />
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            _isDarkMode = value;
            IsDarkModeChanged?.Invoke();
        }
    }

    private static MudTheme CreateDefaultTheme()
    {
        var theme = new MudTheme
        {
            LayoutProperties =
            {
                DefaultBorderRadius = "4px",
            },
            PaletteLight =
            {
                Primary = new MudColor("#2563EB"), // Azul profesional, equilibrado y confiable
                Secondary = new MudColor("#10B981"), // Verde suave para elementos secundarios (éxito, validación)
                AppbarBackground = new MudColor("#F3F4F6"), // Barra de navegación con gris claro
                AppbarText = new MudColor("#1F2937"), // Texto oscuro para legibilidad
                DrawerBackground = new MudColor("#F9FAFB"), // Fondo del menú lateral, gris claro
                Background = new MudColor("#FFFFFF"), // Fondo principal, blanco limpio
                Surface = new MudColor("#F3F4F6"), // Fondo de tarjetas o áreas destacadas
                TextPrimary = new MudColor("#1F2937"), // Texto principal, gris oscuro
                TextSecondary = new MudColor("#6B7280"), // Texto secundario, gris medio
                ActionDefault = new MudColor("#2563EB"), // Color para botones y acciones principales
                ActionDisabled = new MudColor("#D1D5DB"), // Botones deshabilitados, gris claro
                Divider = new MudColor("#E5E7EB"), // Color de separadores entre secciones
                Success = new MudColor("#10B981"), // Verde para mensajes de éxito
                Warning = new MudColor("#F59E0B"), // Amarillo para advertencias
                Error = new MudColor("#EF4444"), // Rojo para errores
                Info = new MudColor("#3B82F6") // Azul más claro para información
            },
            PaletteDark = 
            {
                Primary = new MudColor("#2563EB"), // Azul profesional y menos saturado
                AppbarBackground = new MudColor("#0F172A"), // Fondo principal, tono oscuro
                DrawerBackground = new MudColor("#0F172A"),
                Background = new MudColor("#0B1120"), // Fondo base más oscuro
                Surface = new MudColor("#1E293B"), // Fondo secundario para separar tarjetas o secciones
                TextPrimary = new MudColor("#E5E7EB"), // Texto principal, gris claro
                TextSecondary = new MudColor("#9CA3AF"), // Texto secundario, gris medio
                ActionDefault = new MudColor("#2563EB"), // Azul primario para acciones principales
                ActionDisabled = new MudColor("#4B5563"), // Acciones deshabilitadas, gris tenue
                Success = new MudColor("#10B981"), // Verde para mensajes de éxito
                Warning = new MudColor("#F59E0B"), // Amarillo para advertencias
                Error = new MudColor("#EF4444")   // Rojo para errores
            }
        };

        return theme;
    }
}