using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  //
  // Summary:
  //     Modes for formats the report to be exported to.
  public enum StimulExportFormat
  {
    //     Export will not be done.
    // None = 0,
    //     Adobe PDF format for export.
    Pdf = 1,
    //     Microsoft Xps format for export.
    Xps = 2,
    //     RTF format for export.
    Rtf = 6,
    //     Text format for export.
    Text = 11,
    //     Excel BIFF (Binary Interchange File Format) format for export.
    Excel = 12,
    //     Excel 2007 format for export.
    Excel2007 = 14,
    //     Word 2007 format for export.
    Word2007 = 15,
    //     Xml format for export.
    Xml = 16,
    //     CSV (Comma Separated Value) file format for export.
    Csv = 17,
    //     Image in GIF format for export.
    Gif = 21,
    //     Image in BMP format for export.
    Bmp = 22,
    //     Image in PNG format for export.
    Png = 23,
    //     Image in TIFF format for export.
    Tiff = 24,
    //     Image in JPEG format for export.
    Jpeg = 25,
    //     Image in PCX format for export.
    Pcx = 26,
    //     Image in EMF format for export.
    Emf = 27,
    //     Image in SVG format for export.
    Svg = 28,
    //     Image in SVGZ format for export.
    Svgz = 29,
    //     PowerPoint 2007 format for export
    Ppt2007 = 35,
    //     HTML5 format for export.
    Html = 36
  }
}
