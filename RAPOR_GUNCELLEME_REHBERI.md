# AutoService ERP – Rapor Modülü Güncellemesi

## Eklenen özellikler
- Profesyonel, responsive rapor tasarımı
- Tarih aralığı filtresi
- PDF dışa aktarma
- Excel dışa aktarma (5 çalışma sayfası)
- Yazdırma görünümü
- Aylık gelir, servis durumu ve teknisyen grafikleri
- KPI kartları ve yönetici özeti
- Boş veri ekranları
- Kritik stok ve parça kullanım tabloları

## Değiştirilen dosyalar
- `AutoService.Web/AutoService.Web.csproj`
- `AutoService.Web/Program.cs`
- `AutoService.Web/Controllers/ReportController.cs`
- `AutoService.Web/Views/Report/Index.cshtml`
- `AutoService.Web/wwwroot/css/site.css`

## Yeni dosyalar
- `AutoService.Web/Services/IReportExportService.cs`
- `AutoService.Web/Services/ReportExportService.cs`

## Visual Studio'da çalıştırma
1. `AutoService.Web.sln` dosyasını açın.
2. Solution'a sağ tıklayıp **Restore NuGet Packages** seçin.
3. `AutoService.Web` projesini başlangıç projesi yapın.
4. **Build > Rebuild Solution** çalıştırın.
5. Uygulamayı başlatıp `/Report` sayfasına gidin.

## Eklenen NuGet paketleri
- ClosedXML `0.105.0`
- QuestPDF `2026.7.1`

## Not
Bu ortamda .NET SDK bulunmadığı için solution burada derlenemedi. Visual Studio ilk açılışta NuGet paketlerini indirip projeyi derleyecektir. Herhangi bir derleme mesajı çıkarsa ekran görüntüsünü paylaşın.
