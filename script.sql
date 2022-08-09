CREATE DATABASE  IF NOT EXISTS `kesifdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `kesifdb`;
-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
-- Host: localhost    Database: kesifdb
-- ------------------------------------------------------
-- Server version	8.0.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `alanholder`
--

DROP TABLE IF EXISTS `alanholder`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `alanholder` (
  `id` int NOT NULL AUTO_INCREMENT,
  `KesifId` int DEFAULT NULL,
  `MekanId` int DEFAULT NULL,
  `AlanAdi` varchar(80) DEFAULT NULL,
  `Not` varchar(200) DEFAULT NULL,
  `Konum` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `alanholder`
--

LOCK TABLES `alanholder` WRITE;
/*!40000 ALTER TABLE `alanholder` DISABLE KEYS */;
INSERT INTO `alanholder` VALUES (1,10,1,'DENEME',NULL,NULL),(11,12,2,'123ASSFA',NULL,NULL),(12,10,1,'A KABİNİ',NULL,NULL),(13,14,1,'ANA KAPI KABİNET','Teknik servis montajda eşlik edecek','Ana kapı pano odası'),(14,10,2,'DENEME',NULL,NULL),(15,10,3,'DIŞ',NULL,NULL);
/*!40000 ALTER TABLE `alanholder` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kategoriler`
--

DROP TABLE IF EXISTS `kategoriler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kategoriler` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UstId` int DEFAULT NULL,
  `KategoriAdi` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kategoriler`
--

LOCK TABLES `kategoriler` WRITE;
/*!40000 ALTER TABLE `kategoriler` DISABLE KEYS */;
INSERT INTO `kategoriler` VALUES (1,NULL,'DENEME'),(9,NULL,'DENEMEEEE');
/*!40000 ALTER TABLE `kategoriler` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kesifler`
--

DROP TABLE IF EXISTS `kesifler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kesifler` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ad` varchar(150) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kesifler`
--

LOCK TABLES `kesifler` WRITE;
/*!40000 ALTER TABLE `kesifler` DISABLE KEYS */;
INSERT INTO `kesifler` VALUES (10,'A'),(12,'HHHH'),(14,'ISMAIL KEŞİF');
/*!40000 ALTER TABLE `kesifler` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kesifmekanholder`
--

DROP TABLE IF EXISTS `kesifmekanholder`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kesifmekanholder` (
  `id` int NOT NULL AUTO_INCREMENT,
  `KesifId` int DEFAULT NULL,
  `MekanId` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kesifmekanholder`
--

LOCK TABLES `kesifmekanholder` WRITE;
/*!40000 ALTER TABLE `kesifmekanholder` DISABLE KEYS */;
INSERT INTO `kesifmekanholder` VALUES (1,10,1),(2,12,1),(6,12,2),(7,14,1),(8,10,2),(9,10,3);
/*!40000 ALTER TABLE `kesifmekanholder` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kullanıcı`
--

DROP TABLE IF EXISTS `kullanıcı`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kullanıcı` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Ad` varchar(40) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `psswrd` varchar(60) DEFAULT NULL,
  `RolId` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_roller` (`RolId`),
  CONSTRAINT `fk_roller` FOREIGN KEY (`RolId`) REFERENCES `roller` (`RolId`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kullanıcı`
--

LOCK TABLES `kullanıcı` WRITE;
/*!40000 ALTER TABLE `kullanıcı` DISABLE KEYS */;
INSERT INTO `kullanıcı` VALUES (1,'Mert','mert@gmail.com','123',1),(2,'Mw','mert2@gmail.com','123',2),(3,'Mert3','mert3@gmail.com','123',4),(8,'Mert3','mertsahin011@gmail.com','123',1),(21,'İSMAİL YILDIRIM','ismail.yildirim@protalya.com','28121978',1);
/*!40000 ALTER TABLE `kullanıcı` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mekantürleri`
--

DROP TABLE IF EXISTS `mekantürleri`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mekantürleri` (
  `id` int NOT NULL AUTO_INCREMENT,
  `MekanAdi` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mekantürleri`
--

LOCK TABLES `mekantürleri` WRITE;
/*!40000 ALTER TABLE `mekantürleri` DISABLE KEYS */;
INSERT INTO `mekantürleri` VALUES (1,'GENEL MEKAN'),(2,'İÇ MEKAN'),(3,'DIŞ MEKAN');
/*!40000 ALTER TABLE `mekantürleri` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paket`
--

DROP TABLE IF EXISTS `paket`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paket` (
  `id` int NOT NULL AUTO_INCREMENT,
  `PaketAdi` varchar(60) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paket`
--

LOCK TABLES `paket` WRITE;
/*!40000 ALTER TABLE `paket` DISABLE KEYS */;
INSERT INTO `paket` VALUES (2,'DENEMEPAKET2'),(3,'DENEME PAKET 3'),(4,'DENEME PAKET 4'),(5,'DENEME PAKET 5'),(6,'DENEME PAKET 6'),(7,'DENEME PAKET 7'),(39,'AS');
/*!40000 ALTER TABLE `paket` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paketholder`
--

DROP TABLE IF EXISTS `paketholder`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paketholder` (
  `id` int NOT NULL AUTO_INCREMENT,
  `urunId` int DEFAULT NULL,
  `paketId` int DEFAULT NULL,
  `urunAdeti` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paketholder`
--

LOCK TABLES `paketholder` WRITE;
/*!40000 ALTER TABLE `paketholder` DISABLE KEYS */;
INSERT INTO `paketholder` VALUES (1,7,2,1),(2,21,2,12),(3,28,2,15),(4,28,2,13),(5,21,2,13),(6,33,3,99),(7,28,3,99),(8,28,3,8),(9,21,39,3),(10,29,39,3),(11,33,39,35);
/*!40000 ALTER TABLE `paketholder` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roller`
--

DROP TABLE IF EXISTS `roller`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roller` (
  `RolId` int NOT NULL AUTO_INCREMENT,
  `RolAdi` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`RolId`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roller`
--

LOCK TABLES `roller` WRITE;
/*!40000 ALTER TABLE `roller` DISABLE KEYS */;
INSERT INTO `roller` VALUES (1,'ADMIN'),(2,'DİREKTÖR'),(3,'SAHA EKİBİ'),(4,'MUHASEBE');
/*!40000 ALTER TABLE `roller` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stokısenabled`
--

DROP TABLE IF EXISTS `stokısenabled`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stokısenabled` (
  `id` int NOT NULL,
  `isEnabled` tinyint DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stokısenabled`
--

LOCK TABLES `stokısenabled` WRITE;
/*!40000 ALTER TABLE `stokısenabled` DISABLE KEYS */;
INSERT INTO `stokısenabled` VALUES (1,0);
/*!40000 ALTER TABLE `stokısenabled` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `urunholder`
--

DROP TABLE IF EXISTS `urunholder`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `urunholder` (
  `id` int NOT NULL AUTO_INCREMENT,
  `AlanId` int DEFAULT NULL,
  `UrunId` int DEFAULT NULL,
  `UrunAdet` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `urunholder`
--

LOCK TABLES `urunholder` WRITE;
/*!40000 ALTER TABLE `urunholder` DISABLE KEYS */;
INSERT INTO `urunholder` VALUES (14,1,7,200),(17,11,21,4),(18,4,21,50),(19,13,7,5),(20,14,8,299),(24,15,28,200),(25,1,33,280),(32,1,29,70),(36,1,21,300),(37,12,33,10),(39,12,29,90);
/*!40000 ALTER TABLE `urunholder` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `urunler`
--

DROP TABLE IF EXISTS `urunler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `urunler` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UrunKodu` varchar(10) DEFAULT NULL,
  `UrunAdi` varchar(100) DEFAULT NULL,
  `Marka` varchar(50) DEFAULT NULL,
  `UrunFiyati` float DEFAULT NULL,
  `SatisFiyati` float DEFAULT NULL,
  `UrunAdet` int DEFAULT NULL,
  `UrunKategorisi` varchar(50) DEFAULT NULL,
  `KullanilanUrunAdet` int DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `urunler`
--

LOCK TABLES `urunler` WRITE;
/*!40000 ALTER TABLE `urunler` DISABLE KEYS */;
INSERT INTO `urunler` VALUES (7,'KX11H93XVX','DEN','DEN1',10.9,19.6,2000,'DENEME',205),(8,'O2DAR227LW','DEN','DEN',99,99,13,'DENEME',266),(21,'EFYOQ5CRQS','DENEMESSS','DENEMESS',18,36,300,'DENEME',300),(28,'H1DGHP0CY4','DENEME TAHTASI','TAHTA',4.5,10.9,200,'DENEME',200),(29,'7PITTCHV42','FLOAT DENEME','TAHTA',7.9,16.6,999,'DENEMEEEE',160),(33,'GLKPGZWEV3','MERHABA','Mer',12.3,15.3,310,'DENEME',290);
/*!40000 ALTER TABLE `urunler` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'kesifdb'
--

--
-- Dumping routines for database 'kesifdb'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-08-09 11:24:11
