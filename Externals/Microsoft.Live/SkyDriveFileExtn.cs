using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

internal static class SkyDriveFileExtn
{
    private static Dictionary<string, string> excelMapping = new Dictionary<string, string>();
    private static char[] invalidFileCharacters = new char[] { '\\', '/', ':', ';', '*', '“', '<', '>', '|', '?' };
    public static int maxSkyDriveFileSize = 0x3200000;
    private static Dictionary<string, string> oneNoteMapping = new Dictionary<string, string>();
    private static Dictionary<string, string> otherDocMapping = new Dictionary<string, string>();
    private static Dictionary<string, string> photoMapping = new Dictionary<string, string>();
    private static Dictionary<string, string> powerPointMapping = new Dictionary<string, string>();
    private static Dictionary<string, string> publisherMapping = new Dictionary<string, string>();
    private static Dictionary<string, string> videoMapping = new Dictionary<string, string>();
    private static Dictionary<string, string> wordMapping = new Dictionary<string, string>();

    static SkyDriveFileExtn()
    {
        photoMapping.Add(".bmp", "image/bmp");
        photoMapping.Add(".dib", "image/bmp");
        photoMapping.Add(".gif", "image/gif");
        photoMapping.Add(".ico", "image/x-icon");
        photoMapping.Add(".jfif", "image/jpeg");
        photoMapping.Add(".jpeg", "image/jpeg");
        photoMapping.Add(".jpg", "image/jpeg");
        photoMapping.Add(".png", "image/png");
        photoMapping.Add(".wdp", "image/vnd.ms-photo");
        videoMapping.Add(".wmv", "video/x-ms-wmv");
        wordMapping.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        wordMapping.Add(".docm", "application/vnd.ms-word.document.macroEnabled.12");
        wordMapping.Add(".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
        wordMapping.Add(".dotm", "application/vnd.ms-word.template.macroEnabled.12");
        wordMapping.Add(".doc", "application/msword");
        wordMapping.Add(".dot", "application/msword");
        wordMapping.Add(".htm", "text/html");
        wordMapping.Add(".html", "text/html");
        wordMapping.Add(".rtf", "application/msword");
        wordMapping.Add(".mht", "message/rfc822");
        wordMapping.Add(".mhtml", "message/rfc822");
        wordMapping.Add(".xml", "text/xml");
        wordMapping.Add(".odt", "application/vnd.oasis.opendocument.text");
        wordMapping.Add(".txt", "text/plain");
        wordMapping.Add(".wpd", "application/wordperfect");
        wordMapping.Add(".wps", "application/msworks");
        excelMapping.Add(".xl", "application/x-msexcel");
        excelMapping.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        excelMapping.Add(".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12");
        excelMapping.Add(".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12");
        excelMapping.Add(".xlam", "application/vnd.ms-excel.addin.macroEnabled.12");
        excelMapping.Add(".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
        excelMapping.Add(".xltm", "application/vnd.ms-excel.template.macroEnabled.12");
        excelMapping.Add(".xls", "application/vnd.ms-excel");
        excelMapping.Add(".xlt", "application/vnd.ms-excel");
        excelMapping.Add(".htm", "text/html");
        excelMapping.Add(".html", "text/html");
        excelMapping.Add(".mht", "message/rfc822");
        excelMapping.Add(".mhtml", "message/rfc822");
        excelMapping.Add(".xml", "text/xml");
        excelMapping.Add(".xla", "application/vnd.ms-excel");
        excelMapping.Add(".xlm", "application/vnd.ms-excel");
        excelMapping.Add(".xlw", "application/vnd.ms-excel");
        excelMapping.Add(".odc", "text/x-ms-odc");
        excelMapping.Add(".uxdc", "application/vnd.ms-excel");
        excelMapping.Add(".ods", "application/vnd.oasis.opendocument.spreadsheet");
        excelMapping.Add(".prm", "application/vnd.ms-excel");
        excelMapping.Add(".txt", "text/plain");
        excelMapping.Add(".csv", "application/vnd.ms-excel");
        excelMapping.Add(".udl", "application/vnd.ms-excel");
        excelMapping.Add(".dsn", "application/vnd.ms-excel");
        excelMapping.Add(".mdb", "application/msaccess");
        excelMapping.Add(".mde", "application/msaccess");
        excelMapping.Add(".accdb", "application/msaccess");
        excelMapping.Add(".accde", "application/msaccess");
        excelMapping.Add(".dbc", "application/dbc");
        excelMapping.Add(".iqy", "text/x-ms-iqy");
        excelMapping.Add(".dqy", "application/vnd.ms-excel");
        excelMapping.Add(".rqy", "text/x-ms-rqy");
        excelMapping.Add(".oqy", "application/vnd.ms-excel");
        excelMapping.Add(".cub", "application/vnd.ms-excel");
        excelMapping.Add(".dbf", "application/vnd.ms-excel");
        excelMapping.Add(".xll", "application/vnd.ms-excel");
        excelMapping.Add(".xlb", "application/vnd.ms-excel");
        excelMapping.Add(".slk", "application/vnd.ms-excel");
        excelMapping.Add(".dif", "video/x-dv");
        excelMapping.Add(".xlk", "application/vnd.ms-excel");
        excelMapping.Add(".bak", "application/vnd.ms-excel");
        excelMapping.Add(".prn", "application/vnd.ms-excel");
        excelMapping.Add(".xps", "application/vnd.ms-xpsdocument ");
        powerPointMapping.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
        powerPointMapping.Add(".ppt", "application/vnd.ms-powerpoint");
        powerPointMapping.Add(".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12");
        powerPointMapping.Add(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
        powerPointMapping.Add(".pps", "application/vnd.ms-powerpoint");
        powerPointMapping.Add(".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12");
        powerPointMapping.Add(".potx", "application/vnd.openxmlformats-officedocument.presentationml.template");
        powerPointMapping.Add(".pot", "application/vnd.ms-powerpoint");
        powerPointMapping.Add(".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12");
        powerPointMapping.Add(".odp", "application/vnd.oasis.opendocument.presentation");
        powerPointMapping.Add(".xml", "text/xml");
        powerPointMapping.Add(".htm", "text/html");
        powerPointMapping.Add(".html", "text/html");
        powerPointMapping.Add(".mht", "message/rfc822");
        powerPointMapping.Add(".mhtml", "message/rfc822");
        powerPointMapping.Add(".thmx", "application/vnd.ms-officetheme");
        powerPointMapping.Add(".txt", "text/plain");
        powerPointMapping.Add(".rtf", "application/msword");
        powerPointMapping.Add(".doc", "application/msword");
        powerPointMapping.Add(".wpd", "application/wordperfect");
        powerPointMapping.Add(".wps", "application/msworks");
        powerPointMapping.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        powerPointMapping.Add(".docm", "application/vnd.ms-word.document.macroEnabled.12");
        powerPointMapping.Add(".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12");
        powerPointMapping.Add(".ppa", "application/vnd.ms-powerpoint");
        powerPointMapping.Add(".xps", "application/vnd.ms-xpsdocument ");
        oneNoteMapping.Add(".one", "application/msonenote");
        oneNoteMapping.Add(".onepkg", "application/msonenote");
        oneNoteMapping.Add(".onetmp", "application/msonenote");
        oneNoteMapping.Add(".onetoc2", "application/msonenote");
        oneNoteMapping.Add(".onebin", "application/msonenote");
        oneNoteMapping.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        oneNoteMapping.Add(".doc", "application/msword");
        oneNoteMapping.Add(".pdf", "application/pdf");
        oneNoteMapping.Add(".xps", "application/vnd.ms-xpsdocument ");
        oneNoteMapping.Add(".mht", "message/rfc822");
        publisherMapping.Add(".pub", "application/pub");
        otherDocMapping.Add(".pdf", "application/vnd.ms-publisher");
    }

    public static string GetDocumentContentType(string extension)
    {
        string str;
        if (wordMapping.TryGetValue(extension.ToLower(CultureInfo.InvariantCulture), out str))
        {
            return str;
        }
        if (excelMapping.TryGetValue(extension.ToLower(CultureInfo.InvariantCulture), out str))
        {
            return str;
        }
        if (powerPointMapping.TryGetValue(extension.ToLower(CultureInfo.InvariantCulture), out str))
        {
            return str;
        }
        if (oneNoteMapping.TryGetValue(extension.ToLower(CultureInfo.InvariantCulture), out str))
        {
            return str;
        }
        if (publisherMapping.TryGetValue(extension.ToLower(CultureInfo.InvariantCulture), out str))
        {
            return str;
        }
        if (otherDocMapping.TryGetValue(extension.ToLower(CultureInfo.InvariantCulture), out str))
        {
            return str;
        }
        return string.Empty;
    }

    public static string GetPhotoContentType(string extension)
    {
        string str;
        if (photoMapping.TryGetValue(extension.ToLower(CultureInfo.InvariantCulture), out str))
        {
            return str;
        }
        if (videoMapping.TryGetValue(extension.ToLower(CultureInfo.InvariantCulture), out str))
        {
            return str;
        }
        return string.Empty;
    }

    public static bool IsFileNameValid(string fileName)
    {
        string str = ".";
        if (string.IsNullOrEmpty(fileName))
        {
            return false;
        }
        if (fileName.StartsWith(str) || fileName.EndsWith(str))
        {
            return false;
        }
        foreach (char ch in invalidFileCharacters)
        {
            if (fileName.ToCharArray().Contains<char>(ch))
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsValidExcelFile(string extension)
    {
        return excelMapping.ContainsKey(extension);
    }

    public static bool IsValidOneNoteFile(string extension)
    {
        return oneNoteMapping.ContainsKey(extension);
    }

    public static bool IsValidOtherDocumentFile(string extension)
    {
        return otherDocMapping.ContainsKey(extension);
    }

    public static bool IsValidPhotoFile(string extension)
    {
        return photoMapping.ContainsKey(extension);
    }

    public static bool IsValidPowerPointFile(string extension)
    {
        return powerPointMapping.ContainsKey(extension);
    }

    public static bool IsValidPublisherFile(string extension)
    {
        return publisherMapping.ContainsKey(extension);
    }

    public static bool IsValidVideoFile(string extension)
    {
        return videoMapping.ContainsKey(extension);
    }

    public static bool IsValidWordFile(string extension)
    {
        return wordMapping.ContainsKey(extension);
    }
}

