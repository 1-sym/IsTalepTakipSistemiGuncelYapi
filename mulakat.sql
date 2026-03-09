create database IsTakip;
go

Create Table KullaniciTuru(
kullaniciTuruID int Not Null identity primary key,
kullaniciTuru nvarchar(50) Not Null,
varmi tinyint default(1) Not Null
)
go

create view KullaniciTuruAYRINTI as
SELECT        kullaniciTuruID, kullaniciTuru, varmi
FROM            dbo.KullaniciTuru
where varmi=1

go
Create Table Kullanici(
kullaniciID int Not Null  identity primary key,
 adi nvarchar(50) Not Null,
 soyadi nvarchar(50) Not Null,
 TcNo nvarchar(11) Not Null Unique,
 telefon nvarchar(11) Not Null Unique,
 sifre nvarchar(8) Not Null,
 i_kullaniciTuruID int Not Null,
varmi tinyint default(1) Not Null

 Foreign key (i_kullaniciTuruID) references KullaniciTuru(kullaniciTuruID)
)

go

Create View KullaniciAYRINTI as
SELECT        dbo.Kullanici.kullaniciID, dbo.Kullanici.adi, dbo.Kullanici.soyadi, dbo.Kullanici.TcNo, dbo.Kullanici.telefon, dbo.Kullanici.sifre,
dbo.Kullanici.i_kullaniciTuruID, dbo.KullaniciTuru.kullaniciTuru, dbo.Kullanici.varmi as kullaniciVarmi,dbo.KullaniciTuru.varmi as turvarmi 


FROM            dbo.Kullanici INNER JOIN
                         dbo.KullaniciTuru ON dbo.Kullanici.i_kullaniciTuruID = dbo.KullaniciTuru.kullaniciTuruID


 where dbo.Kullanici.varmi=1 and dbo.KullaniciTuru.varmi=1
go

Create table Birim (
birimID int Not Null  identity primary key,
birimAdi nvarchar(50) Not Null,
varmi tinyint default(1) Not Null
)
go

create view BrimAYRINTI as
SELECT        dbo.Birim.birimID,  dbo.Birim.birimAdi,  dbo.Birim.varmi
FROM            dbo.Birim
where  dbo.Birim.varmi=1

go

Create table Personel (
    personelID int Not Null  identity primary key,
    adi nvarchar(50) Not Null,
    soyadi nvarchar(50) Not Null,
    TcNo nvarchar(11) Not Null Unique,
    telefon nvarchar(11) Not Null Unique,
    i_birimID int Not Null,
    izinlimi tinyint default(0) Not Null,
     girisTarihi datetime Not Null,
     ayrilisTarihi datetime,
     i_kullaniciID int Not Null,
     varmi tinyint default(1) Not Null
    Foreign key (i_birimID) references Birim(birimID),
    Foreign key (i_kullaniciID) references Kullanici(kullaniciID)
)
go

Create View PersonelAYRINTI as

SELECT        dbo.Personel.personelID, dbo.Personel.adi, dbo.Personel.soyadi, dbo.Personel.TcNo, dbo.Personel.telefon, dbo.Personel.i_birimID, dbo.Birim.birimAdi, dbo.Personel.izinlimi, dbo.Personel.girisTarihi, 
                         dbo.Personel.ayrilisTarihi, dbo.Personel.varmi as personelVarmi, dbo.Birim.varmi as birimVarmi, dbo.Personel.i_kullaniciID,dbo.KullaniciAYRINTI.kullaniciTuru
FROM            dbo.Birim INNER JOIN
                         dbo.Personel ON dbo.Birim.birimID = dbo.Personel.i_birimID INNER JOIN
                         dbo.KullaniciAYRINTI On dbo.Personel.i_kullaniciID = dbo.KullaniciAYRINTI.kullaniciID

where dbo.Personel.varmi=1 and dbo.Birim.varmi=1
go

create table IsOncelikBilgisi(
oncelikID tinyint primary key identity,
detay nvarchar(20)

)

go

Create table IsTalebi (
    talepID int Not Null identity primary key,
    talepTarihi datetime Not Null,
    guncellemeTarihi datetime,
    i_personelID int Not Null,
    talepAyrinti nvarchar(250) Not Null,
    talepTamamlandimi tinyint default(0) Not Null,
    i_talepEdenKullaniciID int Not Null,
    i_oncelikID tinyint Not Null,
    varmi tinyint default(1) Not Null,
    Foreign key (i_personelID) references Personel(personelID),
    Foreign key (i_talepEdenKullaniciID) references Kullanici(kullaniciID),
    Foreign key (i_oncelikID) references IsOncelikBilgisi(oncelikID)
)

go

Create View IsTalebiAYRINTI as

SELECT        dbo.IsTalebi.talepID, dbo.IsTalebi.talepTarihi, dbo.IsTalebi.guncellemeTarihi, dbo.IsTalebi.i_personelID, dbo.Personel.adi, dbo.Personel.soyadi, dbo.Personel.i_birimID, dbo.IsTalebi.talepAyrinti, 
                         dbo.IsTalebi.talepTamamlandimi, dbo.IsTalebi.i_talepEdenKullaniciID, dbo.Kullanici.adi AS kullaniciAdi, dbo.Kullanici.soyadi AS kullaniciSoyadi, dbo.IsTalebi.i_oncelikID, dbo.IsOncelikBilgisi.detay,dbo.IsTalebi.varmi
FROM            dbo.IsTalebi INNER JOIN
                         dbo.IsOncelikBilgisi ON dbo.IsTalebi.i_oncelikID = dbo.IsOncelikBilgisi.oncelikID INNER JOIN
                         dbo.Personel ON dbo.IsTalebi.i_personelID = dbo.Personel.personelID INNER JOIN
                         dbo.Kullanici ON dbo.IsTalebi.i_talepEdenKullaniciID = dbo.Kullanici.kullaniciID

where  dbo.IsTalebi.varmi=1
go

Create table TalepTakip(
talepTakipID int Not Null primary key identity,
i_talepID int Not Null,
tarih datetime Not Null,
talepDurumBilgi nvarchar(250) Not Null,
varmi tinyint default(1) Not Null,
tamamlanmaOrani tinyint,
Foreign key (i_talepID) references IsTalebi(talepID)
)


go

create view TalepTakipAYRINTI as

SELECT        dbo.TalepTakip.talepTakipID,dbo.TalepTakip.i_talepID, dbo.IsTalebi.talepTarihi, dbo.IsTalebi.guncellemeTarihi, dbo.IsTalebi.i_personelID, dbo.Personel.adi, dbo.Personel.soyadi, dbo.IsTalebi.talepAyrinti, dbo.IsTalebi.talepTamamlandimi, dbo.TalepTakip.tarih, 
                         dbo.TalepTakip.talepDurumBilgi,  dbo.IsTalebi.varmi as istalebiVarmi, dbo.TalepTakip.varmi as talepTakipVarmi,  dbo.TalepTakip.tamamlanmaOrani
FROM            dbo.IsTalebi INNER JOIN
                         dbo.TalepTakip ON dbo.IsTalebi.talepID = dbo.TalepTakip.i_talepID INNER JOIN
                         dbo.Personel ON dbo.IsTalebi.i_personelID = dbo.Personel.personelID


where  dbo.IsTalebi.varmi=1 and dbo.TalepTakip.varmi=1


go

-- Kullanıcı Türleri
INSERT INTO KullaniciTuru (kullaniciTuru,varmi) VALUES ('Yönetici',1), ('Operatör',1), ('Müşteri Temsilcisi',1);

-- Birimler
INSERT INTO Birim (birimAdi,varmi) VALUES ('Yazılım',1), ('Donanım',1), ('İnsan Kaynakları',1), ('Pazarlama',1);

-- İş Öncelik Bilgileri
INSERT INTO IsOncelikBilgisi (detay) VALUES ('Düşük'), ('Normal'), ('Yüksek'), ('Acil');

-- Kullanıcılar (Sistemi kullananlar)
INSERT INTO Kullanici (adi, soyadi, TcNo, telefon, sifre, i_kullaniciTuruID, varmi) 
VALUES 
('Ahmet', 'Yılmaz', '12345678901', '05321112233', '1234', 1, 1),
('Ayşe', 'Demir', '98765432109', '05442223344', '4321', 2, 1),
('Demir', 'Yılmaz', '51445678901', '05351112233', '12314', 1, 1),
('Suat', 'Akkaya', '98765432109', '05442223344', '44321', 2, 1);

-- Personeller (İşi yapacak olanlar)
INSERT INTO Personel (adi, soyadi, TcNo, telefon, i_birimID, calisiyormu, girisTarihi,varmi)
VALUES 
('Mehmet', 'Kaya', '11122233344', '05559998877', 1, 1, GETDATE(),1),
('Fatma', 'Çelik', '55566677788', '05054445566', 2, 1, GETDATE(),1);

-- İş Talepleri
INSERT INTO IsTalebi (talepTarihi, i_personelID, talepAyrinti, talepTamamlandimi, i_talepEdenKullaniciID, i_oncelikID,varmi)
VALUES 
(GETDATE(), 1, 'Sunucu bağlantı hatası giderilecek.', 0, 1, 4,1),
(GETDATE(), 2, 'Yeni laptop kurulumu yapılacak.', 0, 2, 2,1);

-- Talep Takip (Yapılan işlemlerin geçmişi)
INSERT INTO TalepTakip (i_talepID, tarih, talepDurumBilgi,varmi)
VALUES 
(1, GETDATE(), 'Hata inceleniyor, log dosyaları kontrol ediliyor.',1),
(1, DATEADD(HOUR, 2, GETDATE()), 'Veritabanı bağlantısı optimize edildi.',1);