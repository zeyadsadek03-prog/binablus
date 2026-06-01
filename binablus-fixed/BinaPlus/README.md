# BinaPlus - Apartman Yönetim Sistemi
## Kurulum Adımları

### 1. Gereksinimler
- Visual Studio 2022 (Community veya üstü)
- MySQL Server (localhost)
- .NET 8 SDK

### 2. MySQL Bağlantısı
`DAL/VeritabaniYardimcisi.cs` dosyasında bağlantı bilgilerinizi güncelleyin:
```
Server=localhost;Database=binaplus;Uid=root;Pwd=SIFRENIZ;CharSet=utf8;
```

### 3. NuGet Paketi
Visual Studio'da projeyi açtıktan sonra:
- Tools → NuGet Package Manager → Package Manager Console
- `Install-Package MySql.Data`

### 4. Veritabanı
Önce SQL dosyanızdaki tüm CREATE TABLE, PROCEDURE, FUNCTION ve TRIGGER kodlarını
MySQL'de çalıştırın.

### 5. Çalıştırma
F5 ile başlatın.

## Proje Yapısı (N-Katmanlı Mimari)

```
BinaPlus/
├── DAL/                    ← Data Access Layer
│   ├── VeritabaniYardimcisi.cs   (MySQL bağlantı yardımcısı)
│   ├── DaireDAL.cs
│   ├── KisiDAL.cs
│   ├── AidatDAL.cs
│   ├── OdemeDAL.cs
│   ├── BakimTalepDAL.cs
│   └── DaireSakinDAL.cs
├── BL/                     ← Business Layer
│   ├── DaireBL.cs
│   ├── KisiBL.cs
│   ├── AidatBL.cs
│   ├── OdemeBL.cs
│   ├── BakimTalepBL.cs
│   └── DaireSakinBL.cs
├── UI/                     ← Presentation Layer
│   └── AnaForm.cs          (Ana WinForms penceresi)
├── Program.cs
└── BinaPlus.csproj
```

## Özellikler
- Tüm CRUD işlemleri Stored Procedure üzerinden
- SELECT/INSERT/UPDATE/DELETE SQL komutları uygulama kodunda YOK
- 6 sekme: Daireler, Kişiler, Aidatlar, Ödemeler, Bakım Talepleri, Daire Sakinleri
- Her sekmede: Ekle, Güncelle, Sil, Ara
- Trigger demo: Ödeme eklenince aidat durumu otomatik güncellenir
