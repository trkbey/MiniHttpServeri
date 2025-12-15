# Mini HTTP Server (TCP Tabanlı REST API)

Bu proje, **Bilgisayar Ağları dersi** kapsamında HTTP protokolünün TCP soketleri üzerinden nasıl çalıştığını göstermek amacıyla geliştirilmiş, framework kullanılmayan basit bir **REST API sunucusudur**.

---

##  Projenin Amacı

* HTTP protokolünün çalışma mantığını uygulamalı olarak göstermek
* TCP tabanlı istemci–sunucu haberleşmesini anlamak
* GET, POST, PUT ve DELETE HTTP metodlarının nasıl çalıştığını göstermek
* URL path parametrelerinin manuel olarak nasıl ayrıştırıldığını incelemek

---

##  Kullanılan Teknolojiler

* **Programlama Dili:** C#
* **Ağ Yapısı:** TCP Sockets
* **Sınıflar:** TcpListener, TcpClient, NetworkStream
* **Veri Formatı:** JSON
* **Test Aracı:** Postman

>  Projede ASP.NET, Web API veya harici framework kullanılmamıştır.

---

##  Proje Mimarisi

* Sunucu 8080 portu üzerinden TCP bağlantılarını dinler
* Gelen HTTP istekleri ham metin (raw request) olarak alınır
* HTTP metod ve path bilgileri manuel olarak ayrıştırılır
* Veriler sunucu belleğinde (RAM) tutulur
* Yanıtlar HTTP standardına uygun şekilde oluşturulur

---

##  Desteklenen HTTP Endpoint’leri

| Method | Endpoint    | Açıklama                                   |
| ------ | ----------- | ------------------------------------------ |
| GET    | /users      | Tüm kullanıcıları listeler                 |
| GET    | /users/{id} | Belirtilen ID’ye sahip kullanıcıyı getirir |
| POST   | /users      | Yeni kullanıcı ekler                       |
| PUT    | /users/{id} | Var olan kullanıcıyı günceller             |
| DELETE | /users/{id} | Kullanıcıyı siler                          |

---

##  Postman Test Örnekleri

###  GET – Tüm Kullanıcılar

```
GET http://localhost:8080/users
```

###  POST – Yeni Kullanıcı

```
POST http://localhost:8080/users
Content-Type: application/json

{
  "id": 3,
  "name": "Ahmet"
}
```

###  PUT – Kullanıcı Güncelleme

```
PUT http://localhost:8080/users/3
Content-Type: application/json

{
  "id": 3,
  "name": "Mehmet"
}
```

###  DELETE – Kullanıcı Silme

```
DELETE http://localhost:8080/users/3
```

---


##  Sonuç

Bu proje, Bilgisayar Ağları dersi kapsamında HTTP protokolünün TCP seviyesinde nasıl çalıştığını göstermek amacıyla geliştirilmiş eğitim odaklı bir uygulamadır.

---

**Hazırlayan:**
Bilgisayar Mühendisliği Öğrencisi
