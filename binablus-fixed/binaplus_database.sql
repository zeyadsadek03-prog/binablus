-- ============================================================
-- BinaPlus - Apartman Yonetim Sistemi
-- Tam Veritabani Kurulum Scripti
-- ============================================================

DROP DATABASE IF EXISTS binaplus;
CREATE DATABASE binaplus CHARACTER SET utf8mb4 COLLATE utf8mb4_turkish_ci;
USE binaplus;

-- ============================================================
-- TABLOLAR
-- ============================================================

CREATE TABLE binaplus_daireler (
  daire_id    VARCHAR(64) PRIMARY KEY,
  blok        VARCHAR(16) NOT NULL,
  daire_no    VARCHAR(16) NOT NULL,
  tip         VARCHAR(16) NOT NULL,
  m2          FLOAT,
  durum       VARCHAR(16) DEFAULT 'Bos',
  CHECK (tip IN ('1+0','1+1','2+1','3+1','4+1','Dubleks'))
);

CREATE TABLE binaplus_kisiler (
  kisi_id     VARCHAR(64) PRIMARY KEY,
  tc_no       VARCHAR(11) NOT NULL UNIQUE,
  ad          VARCHAR(64) NOT NULL,
  soyad       VARCHAR(64) NOT NULL,
  telefon     VARCHAR(25),
  mail        VARCHAR(250),
  rol         VARCHAR(16) NOT NULL,
  CHECK (rol IN ('Sahip','Kiraci','Yonetici'))
);

CREATE TABLE binaplus_aidatlar (
  aidat_id            VARCHAR(64) PRIMARY KEY,
  daire_id            VARCHAR(64) NOT NULL,
  ay                  INT NOT NULL,
  yil                 INT NOT NULL,
  tutar               FLOAT NOT NULL,
  son_odeme_tarihi    DATE,
  durum               VARCHAR(16) DEFAULT 'Odenmedi',
  UNIQUE (daire_id, ay, yil),
  FOREIGN KEY (daire_id) REFERENCES binaplus_daireler(daire_id)
    ON DELETE CASCADE ON UPDATE CASCADE,
  CHECK (ay BETWEEN 1 AND 12),
  CHECK (durum IN ('Odenmedi','Kismi','Tamamlandi'))
);

CREATE TABLE binaplus_odemeler (
  odeme_id        VARCHAR(64) PRIMARY KEY,
  kisi_id         VARCHAR(64) NOT NULL,
  aidat_id        VARCHAR(64) NOT NULL,
  odeme_tarihi    DATETIME NOT NULL,
  tutar           FLOAT NOT NULL,
  tur             VARCHAR(25) NOT NULL,
  aciklama        VARCHAR(250),
  FOREIGN KEY (kisi_id) REFERENCES binaplus_kisiler(kisi_id)
    ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (aidat_id) REFERENCES binaplus_aidatlar(aidat_id)
    ON DELETE CASCADE ON UPDATE CASCADE,
  CHECK (tur IN ('Nakit','Kredi Karti','Banka Havalesi'))
);

CREATE TABLE binaplus_bakim_talepleri (
  talep_id        VARCHAR(64) PRIMARY KEY,
  daire_id        VARCHAR(64) NOT NULL,
  kategori        VARCHAR(64) NOT NULL,
  aciklama        VARCHAR(250),
  durum           VARCHAR(16) DEFAULT 'Acik',
  talep_tarihi    DATETIME NOT NULL,
  cozum_tarihi    DATETIME,
  FOREIGN KEY (daire_id) REFERENCES binaplus_daireler(daire_id)
    ON DELETE CASCADE ON UPDATE CASCADE,
  CHECK (durum IN ('Acik','Islemde','Tamamlandi'))
);

CREATE TABLE binaplus_daire_sakinleri (
  kisi_id             VARCHAR(64) NOT NULL,
  daire_id            VARCHAR(64) NOT NULL,
  rol                 VARCHAR(16) NOT NULL,
  baslangic_tarihi    DATE NOT NULL,
  PRIMARY KEY (kisi_id, daire_id),
  FOREIGN KEY (kisi_id) REFERENCES binaplus_kisiler(kisi_id)
    ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (daire_id) REFERENCES binaplus_daireler(daire_id)
    ON DELETE CASCADE ON UPDATE CASCADE,
  CHECK (rol IN ('Sahip','Kiraci'))
);

-- ============================================================
-- STORED PROCEDURES - DAİRELER
-- ============================================================
DELIMITER $$

CREATE PROCEDURE bp_DairelerHepsi()
BEGIN
  SELECT daire_id AS ID, blok AS Blok, daire_no AS No,
         tip AS Tip, m2, durum AS Durum
  FROM binaplus_daireler;
END $$

CREATE PROCEDURE bp_DaireEkle(
  id VARCHAR(64), bl VARCHAR(16), no VARCHAR(16),
  tp VARCHAR(16), metre FLOAT, drm VARCHAR(16))
BEGIN
  INSERT INTO binaplus_daireler VALUES (id, bl, no, tp, metre, drm);
END $$

CREATE PROCEDURE bp_DaireGuncelle(
  id VARCHAR(64), bl VARCHAR(16), no VARCHAR(16),
  tp VARCHAR(16), metre FLOAT, drm VARCHAR(16))
BEGIN
  UPDATE binaplus_daireler
  SET blok=bl, daire_no=no, tip=tp, m2=metre, durum=drm
  WHERE daire_id=id;
END $$

CREATE PROCEDURE bp_DaireSil(id VARCHAR(64))
BEGIN
  DELETE FROM binaplus_daireler WHERE daire_id=id;
END $$

CREATE PROCEDURE bp_DaireBul(filtre VARCHAR(64))
BEGIN
  SELECT daire_id AS ID, blok AS Blok, daire_no AS No,
         tip AS Tip, m2, durum AS Durum
  FROM binaplus_daireler
  WHERE blok LIKE CONCAT('%',filtre,'%')
     OR daire_no LIKE CONCAT('%',filtre,'%')
     OR tip LIKE CONCAT('%',filtre,'%')
     OR durum LIKE CONCAT('%',filtre,'%');
END $$

-- ============================================================
-- STORED PROCEDURES - KİŞİLER
-- ============================================================

CREATE PROCEDURE bp_KisilerHepsi()
BEGIN
  SELECT kisi_id AS ID, tc_no AS TCNo, ad AS Ad, soyad AS Soyad,
         telefon AS Telefon, mail AS Mail, rol AS Rol
  FROM binaplus_kisiler;
END $$

CREATE PROCEDURE bp_KisiEkle(
  id VARCHAR(64), tc VARCHAR(11), adi VARCHAR(64), soy VARCHAR(64),
  tel VARCHAR(25), ml VARCHAR(250), rl VARCHAR(16))
BEGIN
  INSERT INTO binaplus_kisiler VALUES (id, tc, adi, soy, tel, ml, rl);
END $$

CREATE PROCEDURE bp_KisiGuncelle(
  id VARCHAR(64), tc VARCHAR(11), adi VARCHAR(64), soy VARCHAR(64),
  tel VARCHAR(25), ml VARCHAR(250), rl VARCHAR(16))
BEGIN
  UPDATE binaplus_kisiler
  SET tc_no=tc, ad=adi, soyad=soy, telefon=tel, mail=ml, rol=rl
  WHERE kisi_id=id;
END $$

CREATE PROCEDURE bp_KisiSil(id VARCHAR(64))
BEGIN
  DELETE FROM binaplus_kisiler WHERE kisi_id=id;
END $$

CREATE PROCEDURE bp_KisiBul(filtre VARCHAR(64))
BEGIN
  SELECT kisi_id AS ID, tc_no AS TCNo, ad AS Ad, soyad AS Soyad,
         telefon AS Telefon, mail AS Mail, rol AS Rol
  FROM binaplus_kisiler
  WHERE tc_no LIKE CONCAT('%',filtre,'%')
     OR ad LIKE CONCAT('%',filtre,'%')
     OR soyad LIKE CONCAT('%',filtre,'%')
     OR telefon LIKE CONCAT('%',filtre,'%')
     OR mail LIKE CONCAT('%',filtre,'%')
     OR rol LIKE CONCAT('%',filtre,'%');
END $$

-- BUG FIX: bp_KisiBakiye - bu procedure eksikti, uygulama bunu kullanıyor
CREATE PROCEDURE bp_KisiBakiye(id VARCHAR(64))
BEGIN
  DECLARE toplam_aidat FLOAT DEFAULT 0;
  DECLARE toplam_odeme FLOAT DEFAULT 0;
  
  SELECT COALESCE(SUM(a.tutar), 0) INTO toplam_aidat
  FROM binaplus_aidatlar a
  INNER JOIN binaplus_daire_sakinleri ds ON a.daire_id = ds.daire_id
  WHERE ds.kisi_id = id;
  
  SELECT COALESCE(SUM(tutar), 0) INTO toplam_odeme
  FROM binaplus_odemeler
  WHERE kisi_id = id;
  
  SELECT toplam_odeme - toplam_aidat AS Bakiye;
END $$

-- ============================================================
-- STORED PROCEDURES - AİDATLAR
-- (BUG FIX: bp_AidatlarHepsi artık DaireNo kolonunu da döndürüyor
--  çünkü UI'daki AidatComboDoldur bu kolonu kullanıyor)
-- ============================================================

CREATE PROCEDURE bp_AidatlarHepsi()
BEGIN
  SELECT a.aidat_id AS ID,
         a.daire_id AS DaireID,
         CONCAT(d.blok, '-', d.daire_no) AS DaireNo,
         a.ay AS Ay,
         a.yil AS Yil,
         a.tutar AS Tutar,
         a.son_odeme_tarihi AS SonOdeme,
         a.durum AS Durum
  FROM binaplus_aidatlar a
  INNER JOIN binaplus_daireler d ON a.daire_id = d.daire_id;
END $$

CREATE PROCEDURE bp_AidatEkle(
  id VARCHAR(64), did VARCHAR(64), ayy INT, yl INT,
  ttr FLOAT, son DATE, drm VARCHAR(16))
BEGIN
  INSERT INTO binaplus_aidatlar VALUES (id, did, ayy, yl, ttr, son, drm);
END $$

CREATE PROCEDURE bp_AidatGuncelle(
  id VARCHAR(64), did VARCHAR(64), ayy INT, yl INT,
  ttr FLOAT, son DATE, drm VARCHAR(16))
BEGIN
  UPDATE binaplus_aidatlar
  SET daire_id=did, ay=ayy, yil=yl, tutar=ttr, son_odeme_tarihi=son, durum=drm
  WHERE aidat_id=id;
END $$

CREATE PROCEDURE bp_AidatSil(id VARCHAR(64))
BEGIN
  DELETE FROM binaplus_aidatlar WHERE aidat_id=id;
END $$

CREATE PROCEDURE bp_AidatBul(filtre VARCHAR(64))
BEGIN
  SELECT a.aidat_id AS ID,
         a.daire_id AS DaireID,
         CONCAT(d.blok, '-', d.daire_no) AS DaireNo,
         a.ay AS Ay,
         a.yil AS Yil,
         a.tutar AS Tutar,
         a.son_odeme_tarihi AS SonOdeme,
         a.durum AS Durum
  FROM binaplus_aidatlar a
  INNER JOIN binaplus_daireler d ON a.daire_id = d.daire_id
  WHERE a.durum LIKE CONCAT('%',filtre,'%')
     OR d.daire_no LIKE CONCAT('%',filtre,'%')
     OR d.blok LIKE CONCAT('%',filtre,'%')
     OR CAST(a.ay AS CHAR) LIKE CONCAT('%',filtre,'%')
     OR CAST(a.yil AS CHAR) LIKE CONCAT('%',filtre,'%');
END $$

-- BUG FIX: bp_AidatToplam - eksikti
CREATE PROCEDURE bp_AidatToplam()
BEGIN
  SELECT COALESCE(SUM(tutar), 0) AS Toplam FROM binaplus_aidatlar;
END $$

-- ============================================================
-- STORED PROCEDURES - ÖDEMELER
-- ============================================================

CREATE PROCEDURE bp_OdemelerHepsi()
BEGIN
  SELECT o.odeme_id AS ID,
         o.kisi_id AS KisiID,
         CONCAT(k.ad, ' ', k.soyad) AS KisiAdSoyad,
         o.aidat_id AS AidatID,
         o.odeme_tarihi AS Tarih,
         o.tutar AS Tutar,
         o.tur AS Tur,
         o.aciklama AS Aciklama
  FROM binaplus_odemeler o
  INNER JOIN binaplus_kisiler k ON o.kisi_id = k.kisi_id;
END $$

CREATE PROCEDURE bp_OdemeEkle(
  id VARCHAR(64), kid VARCHAR(64), aid VARCHAR(64),
  trh DATETIME, ttr FLOAT, tr VARCHAR(25), ack VARCHAR(250))
BEGIN
  INSERT INTO binaplus_odemeler VALUES (id, kid, aid, trh, ttr, tr, ack);
END $$

CREATE PROCEDURE bp_OdemeGuncelle(
  id VARCHAR(64), kid VARCHAR(64), aid VARCHAR(64),
  trh DATETIME, ttr FLOAT, tr VARCHAR(25), ack VARCHAR(250))
BEGIN
  UPDATE binaplus_odemeler
  SET kisi_id=kid, aidat_id=aid, odeme_tarihi=trh, tutar=ttr, tur=tr, aciklama=ack
  WHERE odeme_id=id;
END $$

CREATE PROCEDURE bp_OdemeSil(id VARCHAR(64))
BEGIN
  DELETE FROM binaplus_odemeler WHERE odeme_id=id;
END $$

CREATE PROCEDURE bp_OdemeBul(filtre VARCHAR(64))
BEGIN
  SELECT o.odeme_id AS ID,
         o.kisi_id AS KisiID,
         CONCAT(k.ad, ' ', k.soyad) AS KisiAdSoyad,
         o.aidat_id AS AidatID,
         o.odeme_tarihi AS Tarih,
         o.tutar AS Tutar,
         o.tur AS Tur,
         o.aciklama AS Aciklama
  FROM binaplus_odemeler o
  INNER JOIN binaplus_kisiler k ON o.kisi_id = k.kisi_id
  WHERE k.ad LIKE CONCAT('%',filtre,'%')
     OR k.soyad LIKE CONCAT('%',filtre,'%')
     OR o.tur LIKE CONCAT('%',filtre,'%')
     OR o.aciklama LIKE CONCAT('%',filtre,'%');
END $$

-- BUG FIX: bp_OdemeToplam - eksikti
CREATE PROCEDURE bp_OdemeToplam()
BEGIN
  SELECT COALESCE(SUM(tutar), 0) AS Toplam FROM binaplus_odemeler;
END $$

-- ============================================================
-- STORED PROCEDURES - BAKIM TALEPLERİ
-- ============================================================

CREATE PROCEDURE bp_BakimHepsi()
BEGIN
  SELECT t.talep_id AS ID,
         t.daire_id AS DaireID,
         CONCAT(d.blok, '-', d.daire_no) AS DaireNo,
         t.kategori AS Kategori,
         t.aciklama AS Aciklama,
         t.durum AS Durum,
         t.talep_tarihi AS Tarih,
         t.cozum_tarihi AS CozumTarihi
  FROM binaplus_bakim_talepleri t
  INNER JOIN binaplus_daireler d ON t.daire_id = d.daire_id;
END $$

CREATE PROCEDURE bp_BakimEkle(
  id VARCHAR(64), did VARCHAR(64), kat VARCHAR(64),
  ack VARCHAR(250), drm VARCHAR(16), trh DATETIME, coz DATETIME)
BEGIN
  INSERT INTO binaplus_bakim_talepleri VALUES (id, did, kat, ack, drm, trh, coz);
END $$

CREATE PROCEDURE bp_BakimGuncelle(
  id VARCHAR(64), did VARCHAR(64), kat VARCHAR(64),
  ack VARCHAR(250), drm VARCHAR(16), trh DATETIME, coz DATETIME)
BEGIN
  UPDATE binaplus_bakim_talepleri
  SET daire_id=did, kategori=kat, aciklama=ack, durum=drm, talep_tarihi=trh, cozum_tarihi=coz
  WHERE talep_id=id;
END $$

CREATE PROCEDURE bp_BakimSil(id VARCHAR(64))
BEGIN
  DELETE FROM binaplus_bakim_talepleri WHERE talep_id=id;
END $$

CREATE PROCEDURE bp_BakimBul(filtre VARCHAR(64))
BEGIN
  SELECT t.talep_id AS ID,
         t.daire_id AS DaireID,
         CONCAT(d.blok, '-', d.daire_no) AS DaireNo,
         t.kategori AS Kategori,
         t.aciklama AS Aciklama,
         t.durum AS Durum,
         t.talep_tarihi AS Tarih,
         t.cozum_tarihi AS CozumTarihi
  FROM binaplus_bakim_talepleri t
  INNER JOIN binaplus_daireler d ON t.daire_id = d.daire_id
  WHERE t.kategori LIKE CONCAT('%',filtre,'%')
     OR t.aciklama LIKE CONCAT('%',filtre,'%')
     OR t.durum LIKE CONCAT('%',filtre,'%')
     OR d.daire_no LIKE CONCAT('%',filtre,'%');
END $$

-- ============================================================
-- STORED PROCEDURES - DAİRE SAKİNLERİ
-- ============================================================

CREATE PROCEDURE bp_SakinlerHepsi()
BEGIN
  SELECT ds.kisi_id AS KisiID,
         ds.daire_id AS DaireID,
         CONCAT(k.ad, ' ', k.soyad) AS KisiAdSoyad,
         k.tc_no AS TCNo,
         CONCAT(d.blok, '-', d.daire_no) AS DaireNo,
         ds.rol AS Rol,
         ds.baslangic_tarihi AS BaslangicTarihi
  FROM binaplus_daire_sakinleri ds
  INNER JOIN binaplus_kisiler k ON ds.kisi_id = k.kisi_id
  INNER JOIN binaplus_daireler d ON ds.daire_id = d.daire_id;
END $$

CREATE PROCEDURE bp_SakinEkle(
  kid VARCHAR(64), did VARCHAR(64), rl VARCHAR(16), bas DATE)
BEGIN
  INSERT INTO binaplus_daire_sakinleri VALUES (kid, did, rl, bas);
END $$

CREATE PROCEDURE bp_SakinGuncelle(
  kid VARCHAR(64), did VARCHAR(64), rl VARCHAR(16), bas DATE)
BEGIN
  UPDATE binaplus_daire_sakinleri
  SET rol=rl, baslangic_tarihi=bas
  WHERE kisi_id=kid AND daire_id=did;
END $$

CREATE PROCEDURE bp_SakinSil(kid VARCHAR(64), did VARCHAR(64))
BEGIN
  DELETE FROM binaplus_daire_sakinleri
  WHERE kisi_id=kid AND daire_id=did;
END $$

DELIMITER ;

-- ============================================================
-- FONKSİYONLAR
-- (BUG FIX: 'odened' yazım hatası 'odenen' olarak düzeltildi)
-- ============================================================
DELIMITER $$

-- Fonksiyon 1: Bir dairenin toplam kalan borcu
CREATE FUNCTION bp_DaireBorcu(d_id VARCHAR(64))
RETURNS FLOAT DETERMINISTIC READS SQL DATA
BEGIN
  DECLARE borc FLOAT;
  DECLARE odenen FLOAT;   -- BUG FIX: önceki kodda 'odened' yazılmıştı
  
  SELECT COALESCE(SUM(tutar), 0) INTO borc
    FROM binaplus_aidatlar WHERE daire_id = d_id;
    
  SELECT COALESCE(SUM(o.tutar), 0) INTO odenen   -- BUG FIX: önceki kodda 'odened' yazılmıştı
    FROM binaplus_odemeler o
    INNER JOIN binaplus_aidatlar a ON o.aidat_id = a.aidat_id
    WHERE a.daire_id = d_id;
    
  RETURN borc - odenen;
END $$

-- Fonksiyon 2: Bir aidatin kalan ödenmemiş tutarı
CREATE FUNCTION bp_AidatKalan(a_id VARCHAR(64))
RETURNS FLOAT DETERMINISTIC READS SQL DATA
BEGIN
  DECLARE toplam FLOAT;
  DECLARE odenen FLOAT;   -- BUG FIX: önceki kodda 'odened' yazılmıştı
  
  SELECT COALESCE(tutar, 0) INTO toplam
    FROM binaplus_aidatlar WHERE aidat_id = a_id;
    
  SELECT COALESCE(SUM(tutar), 0) INTO odenen   -- BUG FIX: önceki kodda 'odened' yazılmıştı
    FROM binaplus_odemeler WHERE aidat_id = a_id;
    
  RETURN toplam - odenen;
END $$

DELIMITER ;

-- ============================================================
-- TRİGGERLAR
-- ============================================================
DELIMITER //

-- Trigger 1: Ödeme tutarı kalan borçtan fazla olamaz
CREATE TRIGGER tg_odeme_kontrol
BEFORE INSERT ON binaplus_odemeler FOR EACH ROW
BEGIN
  DECLARE kalan FLOAT;
  DECLARE mesaj VARCHAR(250);
  SET kalan = bp_AidatKalan(NEW.aidat_id);
  IF NEW.tutar > kalan THEN
    SET mesaj = CONCAT('Hata! Odeme ', NEW.tutar, ' TL ama kalan ', kalan, ' TL.');
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = mesaj;
  END IF;
END;//

-- Trigger 2: Ödeme sonrası aidat durumunu otomatik güncelle
CREATE TRIGGER tg_aidat_durum_guncelle
AFTER INSERT ON binaplus_odemeler FOR EACH ROW
BEGIN
  DECLARE toplam FLOAT;
  DECLARE hedef FLOAT;
  SELECT COALESCE(tutar, 0) INTO hedef
    FROM binaplus_aidatlar WHERE aidat_id = NEW.aidat_id;
  SELECT COALESCE(SUM(tutar), 0) INTO toplam
    FROM binaplus_odemeler WHERE aidat_id = NEW.aidat_id;
  IF toplam >= hedef THEN
    UPDATE binaplus_aidatlar SET durum = 'Tamamlandi'
      WHERE aidat_id = NEW.aidat_id;
  ELSEIF toplam > 0 THEN
    UPDATE binaplus_aidatlar SET durum = 'Kismi'
      WHERE aidat_id = NEW.aidat_id;
  END IF;
END;//

DELIMITER ;
