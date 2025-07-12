using System.Text.Json;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;

args = args ?? Array.Empty<string>();

string CursorPath = SelectRandomCursor();

if (args.Contains("AlwaysSetCustom"))
{
    CreateCustomCheck();
    var parts = ReadCursorPartsFromJson();
    SetCustomCursor(parts);
    return;
}

if (CheckForCustom())
{
    SetDefaultCursor();
}
else
{
    var parts = ReadCursorPartsFromJson();
    SetCustomCursor(parts);
}

void SetCustomCursor(CursorParts parts)
{
    // NormalSelect
    var c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.NormalSelect), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_NORMAL);
    // TextSelect
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.TextSelect), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_IBEAM);
    // BusySelect
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.BusySelect), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_WAIT);
    // PrecisionSelect
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.PrecisionSelect), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_CROSS);
    // AlternateSelect
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.AlternateSelect), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_UP);
    // DiagonalResize1
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.DiagonalResize1), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_SIZENWSE);
    // DiagonalResize2
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.DiagonalResize2), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_SIZENESW);
    // HorizontalResize
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.HorizontalResize), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_SIZEWE);
    // VerticalResize
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.VerticalResize), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_SIZENS);
    // Move
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.Move), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_SIZEALL);
    // Unavailable
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.Unavailable), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_NO);
    // LinkSelect
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.LinkSelect), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_HAND);
    // WorkingInBackground
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.WorkingInBackground), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_APPSTARTING);
    // AppStarting
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.AppStarting), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_APPSTARTING);
    // HelpSelect
    c = PInvoke.LoadImage(null, Path.Combine(CursorPath, parts.HelpSelect), GDI_IMAGE_TYPE.IMAGE_CURSOR, 0, 0, IMAGE_FLAGS.LR_LOADFROMFILE);
    PInvoke.SetSystemCursor(c, SYSTEM_CURSOR_ID.OCR_HELP);
}

void SetDefaultCursor()
{
    unsafe
    {
        PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_SETCURSORS, 0, null, SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS.SPIF_SENDCHANGE);
    }
}

static bool CheckForCustom()
{
    const string fileName = "IsCustom";
    try
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
            return true;
        }

        using FileStream fs = File.Create(fileName);
        return false;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
    return false;
}

static void CreateCustomCheck()
{
    const string fileName = "IsCustom";
    if (File.Exists(fileName))
    {
        return;
    }
    try
    {
        using FileStream fs = File.Create(fileName);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

CursorParts ReadCursorPartsFromJson()
{
    var filename = Path.Combine(CursorPath, "SetCursorParts.json");
    try
    {
        // Read the JSON string from the file
        string jsonString = File.ReadAllText(filename);

        // Deserialize the JSON string to the specified type
        CursorParts data = JsonSerializer.Deserialize<CursorParts>(jsonString) ?? new CursorParts();

        return data;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while reading JSON: {ex.Message}");
        throw;
    }
}

string SelectRandomCursor()
{
    string _return = string.Empty;
    if (!Directory.Exists("Cursors"))
    {
        return string.Empty;
    }

    var dirs = Directory.GetDirectories("Cursors");
    if (dirs.Length == 0)
    {
        return string.Empty;
    }

    Random random = new Random();
    while (string.IsNullOrEmpty(_return) || !Directory.Exists(_return))
    {
        int i = random.Next(0, dirs.Length);
        _return = dirs[i];
    }
    return _return;
}

public class CursorParts
{
    public string NormalSelect { get; set; } = string.Empty;
    public string TextSelect { get; set; } = string.Empty;
    public string BusySelect { get; set; } = string.Empty;
    public string PrecisionSelect { get; set; } = string.Empty;
    public string AlternateSelect { get; set; } = string.Empty;
    public string DiagonalResize1 { get; set; } = string.Empty;
    public string DiagonalResize2 { get; set; } = string.Empty;
    public string HorizontalResize { get; set; } = string.Empty;
    public string VerticalResize { get; set; } = string.Empty;
    public string Move { get; set; } = string.Empty;
    public string Unavailable { get; set; } = string.Empty;
    public string LinkSelect { get; set; } = string.Empty;
    public string WorkingInBackground { get; set; } = string.Empty;
    public string AppStarting { get; set; } = string.Empty;
    public string HelpSelect { get; set; } = string.Empty;
}